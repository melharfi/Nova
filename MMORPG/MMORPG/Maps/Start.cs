using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;
using MMORPG.Net.Messages.Request;

namespace MMORPG.GameStates
{
    public class Start : IGameState
    {
        delegate void DelHandel(string s);   // delegate pour faire du cross threading
        public delegate void DelAnimAction(Bmp bmp, IList<Point> wayPointList, int speed);
        Timer interactionNotProceeded = new Timer();
        Bmp __iruka;
        int indexInteraction = 1;

        public void Init()
        {
            //////// chaque map dois avoir ce boup de code si son map a un evenemeny
            Bmp map = new Bmp(@"gfx\map\Start\bg\map.dat", new Point(0, 0), "__map", Manager.TypeGfx.Bgr, true, 1);
            CommonCode.DelWayPointCallBack isFreeCell = Start.isFreeCellToWalk;
            map.tag = isFreeCell;
            map.MouseMove += CommonCode.map_MouseMove;
            map.MouseOut += DefaultCursor;
            map.MouseClic += CommonCode.map_MouseClic;
            Manager.manager.GfxBgrList.Add(map);
            CommonCode.CurMapFreeCellToSpell = Start.isFreeCellToSpell;                        // mémoriser la méthode qui check les obstacles du map + les joueurs en combats pour accée a la methode isFreeCellToSpell
            CommonCode.CurMapFreeCellToWalk = Start.isFreeCellToWalk;                          // // mémoriser la méthode qui check les obstacles du map + les joueurs en combats pour accée a la methode isFreeCellToWalk
            CommonCode.CurMap = "Start";                                                       // memorisation du nom de la classe pour l'accé du waypoint
            ///////////////////////////////////////////////////////////////////
            
            if (MainForm.grid)
            {
                Bmp grid = new Bmp(@"gfx\map\grille.dat", new Point(0, 0), "__grille", Manager.TypeGfx.Obj, true, 1)
                {
                    zindex = 1000
                };
                Manager.manager.GfxObjList.Add(grid);
            }

            // affichage du 1er obstacle arbre1.dat
            Bmp obstacle1 = new Bmp(@"gfx\general\obj\1\arbre1.dat", new Point(30, 30), "__obstacle1", 0, true, 1);
            obstacle1.point = new Point((6 * 30) - (obstacle1.rectangle.Width / 2) + 10, (12 * 30) - obstacle1.rectangle.Height - 5);
            obstacle1.zindex = ((obstacle1.point.Y + obstacle1.rectangle.Height) / 30) * 100;
            obstacle1.TypeGfx = Manager.TypeGfx.Obj;
            Manager.manager.GfxObjList.Add(obstacle1);

            Bmp obstacle2 = new Bmp(@"gfx\general\obj\1\arbre1.dat", new Point(30, 30), "__obstacle2", 0, true, 1);
            obstacle2.point = new Point((11 * 30) - (obstacle2.rectangle.Width / 2) + 10, (5 * 30) - obstacle2.rectangle.Height - 5);
            obstacle2.zindex = ((obstacle2.point.Y + obstacle2.rectangle.Height) / 30) * 100;
            obstacle2.TypeGfx = Manager.TypeGfx.Obj;
            Manager.manager.GfxObjList.Add(obstacle2);

            Bmp obstacle3 = new Bmp(@"gfx\map\Start\obj\arbres.dat", new Point(30, 30), "__obstacle3", 0, true, 1)
            {
                point = new Point(0, 0),
                zindex = 100,
                TypeGfx = Manager.TypeGfx.Obj
            };
            Manager.manager.GfxObjList.Add(obstacle3);

            // demande des informations des joueurs
            GrabingMapActorsInformationRequestMessage grabingMapInformationRequestMessage =
                new GrabingMapActorsInformationRequestMessage();
            grabingMapInformationRequestMessage.Serialize();
            grabingMapInformationRequestMessage.Send();

            // demande des obj sur le map
            GrabingMapObjectsInformationRequestMessage grabinMapObjectsInformationRequestMessage =
                new GrabingMapObjectsInformationRequestMessage();
            grabinMapObjectsInformationRequestMessage.Serialize();
            grabinMapObjectsInformationRequestMessage.Send();

            HudHandle.ChatTextBox.Focus();

            // reinitialisation des variable
            CommonCode.blockNetFlow = false;
            CommonCode.ChatMsgFormat("S", "null", "blockNetFlow9 = false");
            Manager.manager.mainForm.Cursor = Cursors.Default;
            
            // image de iruka
            __iruka = new Bmp(@"gfx\general\classes\iruka.dat", Point.Empty, "__iruka", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("iruka", 0));
            __iruka.point.X = (14 * 30) + 15 - (__iruka.rectangle.Width / 2);
            __iruka.point.Y = (5 * 30) + 15 - (__iruka.rectangle.Height / 2);
            __iruka.MouseMove += CommonCode.CursorHand_MouseMove;
            __iruka.MouseClic += __iruka_MouseClic;
            __iruka.tag = new Actor { pseudo = "iruka" };
            CommonCode.AllActorsInMap.Add(__iruka);
            CommonCode.VerticalSyncZindex(__iruka);
            Manager.manager.GfxObjList.Add(__iruka);

            bool quete_FirstFight_done = ((Actor)CommonCode.MyPlayerInfo.instance.ibPlayer.tag).Quests.Exists(f => f.nom_quete == "FirstFight" && f.submited);

            Anim __interaction = new Anim(50, 1);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\1.dat", 0, -5, -35, 5000);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\clockwize1.dat", 0, 15, -35, 20);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\clockwize2.dat", 0, 35, -35, 20);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\clockwize1.dat", 0, 15, -35, 20);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\1.dat", 0, -5, -35, 20);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\anticlockwize1.dat", 0, -15, -35, 20);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\anticlockwize2.dat", 0, -25, -35, 20);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\anticlockwize1.dat", 0, -15, -35, 20);
            __interaction.AddCell(@"gfx\general\obj\anim\interaction1\" + (quete_FirstFight_done ? "done" : "idle") + @"\1.dat", 0, -5, -35, 20);
            __interaction.AutoResetAnim = true;
            __interaction.Ini(Manager.TypeGfx.Obj, "__interaction", true);
            __interaction.img.zindex = __iruka.zindex;
            __iruka.Child.Add(__interaction);
            __interaction.Start();

            interactionNotProceeded.Interval = 15000;   // attente d'une minute si le joueur ne trouve pas
            interactionNotProceeded.Tick += interactionNotProceeded_Tick;
            interactionNotProceeded.Start();
            
            // check si le joueur été déjà en combat
            if (CommonCode.MyPlayerInfo.instance.Event == "inBattle")       // ce systeme de DecoReco en combat dois etre implementé da, sma class AutoSelectActorInBattleResponseMessage
            {
                // on recréer l'image du joueur
                CommonCode.RedrawPlayerAfterRespawnInBattle();
                Network.SendMessage("cmd•requireBattleData", true);
            }
        }
        void __iruka_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            DrawPlayerContextMenu(bmp, e);
        }
        public void DrawPlayerContextMenu(Bmp ibplayer, MouseEventArgs e)
        {
            #region affiche le menu contextuel du PNJ
            if (e.Button == MouseButtons.Right)
            {
                // affichage du menu contextuel du joueur
                Rec contextMenuRecParent = new Rec(Brushes.Black, new Point(e.X, e.Y), new Size(86, 20), "__contextMenuRec1", Manager.TypeGfx.Top, true);
                contextMenuRecParent.MouseMove += contextMenuRecParent_MouseMove;
                Manager.manager.GfxTopList.Add(contextMenuRecParent);
                CommonCode.RemoveGfxWhenClicked = contextMenuRecParent;

                Rec contextMenuRecMP2 = new Rec(Brushes.Beige, new Point(1, 1), new Size(84, 18), "__contextMenuRec2", Manager.TypeGfx.Top, true);
                contextMenuRecParent.Child.Add(contextMenuRecMP2);

                if ((ibplayer.tag as Actor).pseudo != CommonCode.MyPlayerInfo.instance.pseudo)
                {
                    ///////// menu message privé
                    Rec contextMenuRecMP3 = new Rec(Brushes.White, new Point(4, 4), new Size(78, 12), "contextMenuRecPM2_" + (ibplayer.tag as Actor).pseudo, Manager.TypeGfx.Top, true);
                    contextMenuRecMP3.tag = (ibplayer.tag as Actor).pseudo;
                    contextMenuRecMP3.EscapeGfxWhileMouseClic = true;
                    contextMenuRecMP3.MouseClic += contextMenuRecMP3_MouseClic;
                    contextMenuRecMP3.MouseMove += contextMenuRecMP3_MouseMove;
                    contextMenuRecMP3.MouseOver += contextMenuRecDefie2_MouseOver;
                    contextMenuRecMP3.MouseOut += contextMenuRecMP3_MouseOut;
                    contextMenuRecMP3.EscapeGfxWhileMouseMove = true;
                    contextMenuRecParent.Child.Add(contextMenuRecMP3);

                    Txt contextMenuTxtPM = new Txt(CommonCode.TranslateText(157), new Point(5, 4), "__contextMenuTxtPM", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
                    contextMenuRecParent.Child.Add(contextMenuTxtPM);
                }

                // recadrage du menu contexctuel s'il sort des bords
                if (contextMenuRecParent.point.X < 0)
                    contextMenuRecParent.point.X = 0;
                else if (contextMenuRecParent.point.X + contextMenuRecParent.size.Width > ScreenManager.TilesWidth * 30)
                    contextMenuRecParent.point.X = (ScreenManager.TilesWidth * 30) - contextMenuRecParent.size.Width;

                if (contextMenuRecParent.point.Y < 0)
                    contextMenuRecParent.point.Y = 0;
                else if (contextMenuRecParent.point.Y + contextMenuRecParent.size.Height > ScreenManager.TilesHeight * 30)
                    contextMenuRecParent.point.Y = (ScreenManager.TilesHeight * 30) - contextMenuRecParent.size.Height;
            }
            #endregion
        }
        void contextMenuRecParent_MouseMove(Rec rec, MouseEventArgs e)
        {
            #region
            // survole menu contextuel
            CommonCode.CursorDefault_MouseOut(null, null);
            #endregion
        }
        void contextMenuRecMP3_MouseClic(Rec rec, MouseEventArgs e)
        {
            bool quete_FirstFight_done = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).Quests.Exists(f => f.nom_quete == "FirstFight" && f.submited);

            #region click sur message Parler sur le menu contextuel d'un joueur
            // code lors du clic, afficher le message
            Manager.manager.GfxTopList.Find(f => f.Name() == "__contextMenuRec1").Visible(false);
            Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "__contextMenuRec1");

            //if (!quete_FirstFight_done)
            //{
                // arret du timer qui affiche le message d'aide
                interactionNotProceeded.Stop();

                // affichage du guide
                Rec __helpMenuParent = new Rec(Brushes.Black, Point.Empty, Size.Empty, "__helpMenuParent", Manager.TypeGfx.Obj, true);
                __helpMenuParent.point.X = __iruka.point.X + 15 - (__helpMenuParent.size.Width / 2);
                __helpMenuParent.point.Y = __iruka.point.Y - 100;
                __helpMenuParent.MouseMove += MainForm.DefaultCursorRec;
                Manager.manager.GfxTopList.Add(__helpMenuParent);

                Rec __helpMenuRec1 = new Rec(Brushes.Green, new Point(1, 1), new Size(__helpMenuParent.size.Width - 2, __helpMenuParent.size.Height - 2), "__helpMenuRec1", Manager.TypeGfx.Obj, true);
                __helpMenuParent.Child.Add(__helpMenuRec1);

                addContenteToInteractionMessage("next");
            /*}
            else
            {
                // arret du timer qui affiche le message d'aide
                interactionNotProceeded.Stop();
                addContenteToInteractionMessage("next");

            }*/
            #endregion
        }
        void addContenteToInteractionMessage(string direction)
        {
            Rec __helpMenuParent = Manager.manager.GfxTopList.Find(f => f.Name() == "__helpMenuParent") as Rec;
            __helpMenuParent.Child.Clear();

            bool quete_FirstFight_done = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).Quests.Exists(f => f.nom_quete == "FirstFight" && f.submited);

            if (!quete_FirstFight_done)
            {
                // supression de quelque images qui sont généré que sur certain niveau de discution comme la fleche qui montre les endroits
                if (Manager.manager.GfxTopList.Exists(f => f.Name() == "__spell_indicator_anim"))
                {
                    Anim __spell_indicator_anim = Manager.manager.GfxTopList.Find(f => f.Name() == "__spell_indicator_anim") as Anim;
                    __spell_indicator_anim.Close();
                    Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "__spell_indicator_anim");
                }

                if (direction == "next")
                    indexInteraction++;
                else
                    indexInteraction--;

                if (indexInteraction == 1)
                {
                    #region 1er menu
                    // cadre de couleur blanc
                    Rec __helpMenuRec1 = new Rec(Brushes.White, new Point(1, 1), Size.Empty, "__helpMenuRec1", Manager.TypeGfx.Top, true);
                    __helpMenuParent.Child.Add(__helpMenuRec1);

                    // salut
                    Txt __Salut = new Txt(CommonCode.TranslateText(158), new Point(5, 5), "__Salut", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__Salut);

                    // nom du joueur
                    Txt __nom_du_joueur = new Txt(" " + CommonCode.MyPlayerInfo.instance.pseudo, new Point(__Salut.point.Y + TextRenderer.MeasureText(__Salut.Text, __Salut.font).Width, 5), "__nom_du_joueur", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Green);
                    __helpMenuParent.Child.Add(__nom_du_joueur);

                    Txt __msg1 = new Txt(CommonCode.TranslateText(159), new Point(5, 20), "__msg1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg1);

                    // determination de la chaine la plus langue pour recadrer le message avec la taille aproprié
                    int a1 = TextRenderer.MeasureText(__Salut.Text + " ", __Salut.font).Width + TextRenderer.MeasureText(__nom_du_joueur.Text, __nom_du_joueur.font).Width;
                    int a2 = TextRenderer.MeasureText(__msg1.Text, __msg1.font).Width;

                    int max = Math.Max(a1, a2);

                    __helpMenuParent.size.Width = 10 + max;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Width = 8 + max;

                    // determination de la longeur du cadre
                    int leng = 20 + TextRenderer.MeasureText(__msg1.Text, __msg1.font).Height + 5;
                    __helpMenuParent.size.Height = leng;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Height = leng - 2;

                    __helpMenuParent.point.X = __iruka.point.X + (__iruka.rectangle.Width / 2) - (__helpMenuParent.size.Width / 2);

                    // bouton next
                    Bmp __next = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__next", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 70));
                    __next.point.X = __helpMenuParent.size.Width - 5 - __next.rectangle.Size.Width;
                    __next.point.Y = __helpMenuParent.size.Height - 5 - __next.rectangle.Size.Height;
                    __next.MouseClic += __next_MouseClic;
                    __next.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__next);

                    // bouton fermer
                    Bmp __closeHelpMenu = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__closeHelpMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 72));
                    __closeHelpMenu.point = new Point(__helpMenuParent.size.Width - 1 - __closeHelpMenu.rectangle.Width - 1, 3);
                    __closeHelpMenu.MouseOver += CommonCode.CursorHand_MouseMove;
                    __closeHelpMenu.MouseOut += CommonCode.CursorDefault_MouseOut;
                    __closeHelpMenu.MouseClic += __closeHelpMenu_MouseClic;
                    __helpMenuParent.Child.Add(__closeHelpMenu);
                    #endregion
                }
                else if (indexInteraction == 2)
                {
                    #region 2éme menu presentation
                    // cadre de couleur blanc
                    Rec __helpMenuRec1 = new Rec(Brushes.White, new Point(1, 1), Size.Empty, "__helpMenuRec1", Manager.TypeGfx.Top, true);
                    __helpMenuParent.Child.Add(__helpMenuRec1);

                    // msg1 161
                    Txt __msg1 = new Txt(CommonCode.TranslateText(160), new Point(5, 5), "__msg1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg1);

                    // msg1 162
                    Txt __msg2 = new Txt(CommonCode.TranslateText(161), new Point(5, 20), "__msg2", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg2);

                    // msg1 163
                    Txt __msg3 = new Txt(CommonCode.TranslateText(162), new Point(5, 35), "__msg3", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg3);

                    // determination de la chaine la plus langue pour recadrer le message avec la taille aproprié
                    int a1 = TextRenderer.MeasureText(__msg1.Text, __msg1.font).Width;
                    int a2 = TextRenderer.MeasureText(__msg2.Text, __msg2.font).Width;
                    int a3 = TextRenderer.MeasureText(__msg3.Text, __msg3.font).Width;

                    int max = Math.Max(a1, a2);
                    max = Math.Max(max, a3);

                    __helpMenuParent.size.Width = 10 + max;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Width = 8 + max;

                    // determination de la longeur du cadre, on prend la position vertical du dernier item + sa longeur vertical
                    int leng = 35 + TextRenderer.MeasureText(__msg3.Text, __msg3.font).Height + 5;
                    __helpMenuParent.size.Height = leng;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Height = leng - 2;

                    __helpMenuParent.point.X = __iruka.point.X + (__iruka.rectangle.Width / 2) - (__helpMenuParent.size.Width / 2);

                    // bouton next
                    Bmp __next = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__next", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 70));
                    __next.point.X = __helpMenuParent.size.Width - 5 - __next.rectangle.Size.Width;
                    __next.point.Y = __helpMenuParent.size.Height - 5 - __next.rectangle.Size.Height;
                    __next.MouseClic += __next_MouseClic;
                    __next.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__next);

                    // bouton preview
                    Bmp __preview = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__preview", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 69));
                    __preview.point.X = __helpMenuParent.size.Width - 5 - __preview.rectangle.Size.Width - 2 - __preview.rectangle.Width;
                    __preview.point.Y = __helpMenuParent.size.Height - 5 - __preview.rectangle.Size.Height;
                    __preview.MouseClic += __preview_MouseClic;
                    __preview.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__preview);

                    // bouton fermer
                    Bmp __closeHelpMenu = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__closeHelpMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 72));
                    __closeHelpMenu.point = new Point(__helpMenuParent.size.Width - 1 - __closeHelpMenu.rectangle.Width - 1, 3);
                    __closeHelpMenu.MouseOver += CommonCode.CursorHand_MouseMove;
                    __closeHelpMenu.MouseOut += CommonCode.CursorDefault_MouseOut;
                    __closeHelpMenu.MouseClic += __closeHelpMenu_MouseClic;
                    __helpMenuParent.Child.Add(__closeHelpMenu);
                    #endregion
                }
                else if (indexInteraction == 3)
                {
                    #region 3eme menu des sorts
                    Rec __helpMenuRec1 = new Rec(Brushes.White, new Point(1, 1), Size.Empty, "__helpMenuRec1", Manager.TypeGfx.Top, true);
                    __helpMenuParent.Child.Add(__helpMenuRec1);

                    // msg1 164
                    Txt __msg1 = new Txt(CommonCode.TranslateText(163), new Point(5, 5), "__msg1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg1);

                    // msg1 165
                    Txt __msg2 = new Txt(CommonCode.TranslateText(164), new Point(5, 20), "__msg2", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg2);

                    // determination de la chaine la plus langue pour recadrer le message avec la taille aproprié
                    int a1 = TextRenderer.MeasureText(__msg1.Text, __msg1.font).Width;
                    int a2 = TextRenderer.MeasureText(__msg2.Text, __msg2.font).Width;

                    int max = Math.Max(a1, a2);

                    __helpMenuParent.size.Width = 10 + max;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Width = 8 + max;

                    // determination de la longeur du cadre, on prend la position vertical du dernier item + sa longeur vertical
                    int leng = 20 + TextRenderer.MeasureText(__msg2.Text, __msg2.font).Height + 5;
                    __helpMenuParent.size.Height = leng;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Height = leng - 2;

                    __helpMenuParent.point.X = __iruka.point.X + (__iruka.rectangle.Width / 2) - (__helpMenuParent.size.Width / 2);

                    // bouton next
                    Bmp __next = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__next", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 70));
                    __next.point.X = __helpMenuParent.size.Width - 5 - __next.rectangle.Size.Width;
                    __next.point.Y = __helpMenuParent.size.Height - 5 - __next.rectangle.Size.Height;
                    __next.MouseClic += __next_MouseClic;
                    __next.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__next);

                    // bouton preview
                    Bmp __preview = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__preview", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 69));
                    __preview.point.X = __helpMenuParent.size.Width - 5 - __preview.rectangle.Size.Width - 2 - __preview.rectangle.Width;
                    __preview.point.Y = __helpMenuParent.size.Height - 5 - __preview.rectangle.Size.Height;
                    __preview.MouseClic += __preview_MouseClic;
                    __preview.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__preview);

                    int x = HudHandle.all_sorts.point.X + (HudHandle.all_sorts.rectangle.Width / 2) - 35;
                    int y = HudHandle.all_sorts.point.Y - 42;
                    Anim __spell_indicator_anim = new Anim(1, 1);

                    __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x, y, SpriteSheet.GetSpriteSheet("_Main_option", 71), 1000);
                    for (int cnt = 1; cnt < 10; cnt++)
                        __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - (cnt * 10), y - (cnt * 10), SpriteSheet.GetSpriteSheet("_Main_option", 71), 30);

                    __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - 90, y - 90, SpriteSheet.GetSpriteSheet("_Main_option", 71), 100);

                    for (int cnt = 9; cnt > 0; cnt--)
                        __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - (cnt * 10), y - (cnt * 10), SpriteSheet.GetSpriteSheet("_Main_option", 71), 10);

                    __spell_indicator_anim.AutoResetAnim = true;
                    __spell_indicator_anim.Ini(Manager.TypeGfx.Top, "__spell_indicator_anim", true);
                    __spell_indicator_anim.img.zindex = HudHandle.all_sorts.zindex + 1000;
                    __spell_indicator_anim.Start();
                    Manager.manager.GfxTopList.Add(__spell_indicator_anim);

                    // bouton fermer
                    Bmp __closeHelpMenu = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__closeHelpMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 72));
                    __closeHelpMenu.point = new Point(__helpMenuParent.size.Width - 1 - __closeHelpMenu.rectangle.Width - 1, 3);
                    __closeHelpMenu.MouseOver += CommonCode.CursorHand_MouseMove;
                    __closeHelpMenu.MouseOut += CommonCode.CursorDefault_MouseOut;
                    __closeHelpMenu.MouseClic += __closeHelpMenu_MouseClic;
                    __helpMenuParent.Child.Add(__closeHelpMenu);
                    #endregion
                }
                else if (indexInteraction == 4)
                {
                    #region point de vie
                    Rec __helpMenuRec1 = new Rec(Brushes.White, new Point(1, 1), Size.Empty, "__helpMenuRec1", Manager.TypeGfx.Top, true);
                    __helpMenuParent.Child.Add(__helpMenuRec1);

                    // msg1 166
                    Txt __msg1 = new Txt(CommonCode.TranslateText(165), new Point(5, 5), "__msg1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg1);

                    // msg1 167
                    Txt __msg2 = new Txt(CommonCode.TranslateText(166), new Point(5, 20), "__msg2", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg2);

                    // msg1 168
                    Txt __msg3 = new Txt(CommonCode.TranslateText(167), new Point(5, 35), "__msg3", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg3);

                    // determination de la chaine la plus langue pour recadrer le message avec la taille aproprié
                    int a1 = TextRenderer.MeasureText(__msg1.Text, __msg1.font).Width;
                    int a2 = TextRenderer.MeasureText(__msg2.Text, __msg2.font).Width;
                    int a3 = TextRenderer.MeasureText(__msg3.Text, __msg3.font).Width;

                    int max = Math.Max(a1, a2);
                    max = Math.Max(max, a3);

                    __helpMenuParent.size.Width = 10 + max;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Width = 8 + max;

                    // determination de la longeur du cadre, on prend la position vertical du dernier item + sa longeur vertical
                    int leng = 35 + TextRenderer.MeasureText(__msg3.Text, __msg3.font).Height + 5;
                    __helpMenuParent.size.Height = leng;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Height = leng - 2;

                    __helpMenuParent.point.X = __iruka.point.X + (__iruka.rectangle.Width / 2) - (__helpMenuParent.size.Width / 2);

                    // bouton next
                    Bmp __next = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__next", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 70));
                    __next.point.X = __helpMenuParent.size.Width - 5 - __next.rectangle.Size.Width;
                    __next.point.Y = __helpMenuParent.size.Height - 5 - __next.rectangle.Size.Height;
                    __next.MouseClic += __next_MouseClic;
                    __next.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__next);

                    // bouton preview
                    Bmp __preview = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__preview", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 69));
                    __preview.point.X = __helpMenuParent.size.Width - 5 - __preview.rectangle.Size.Width - 2 - __preview.rectangle.Width;
                    __preview.point.Y = __helpMenuParent.size.Height - 5 - __preview.rectangle.Size.Height;
                    __preview.MouseClic += __preview_MouseClic;
                    __preview.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__preview);

                    int x = HudHandle.Pc_Indicator.point.X + (HudHandle.Pc_Indicator.rectangle.Width / 2) - 80;
                    int y = HudHandle.Pc_Indicator.point.Y - 62;
                    Anim __spell_indicator_anim = new Anim(1, 1);

                    __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x, y, SpriteSheet.GetSpriteSheet("_Main_option", 71), 1000);
                    for (int cnt = 1; cnt < 10; cnt++)
                        __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - (cnt * 10), y - (cnt * 10), SpriteSheet.GetSpriteSheet("_Main_option", 71), 30);

                    __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - 90, y - 90, SpriteSheet.GetSpriteSheet("_Main_option", 71), 100);

                    for (int cnt = 9; cnt > 0; cnt--)
                        __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - (cnt * 10), y - (cnt * 10), SpriteSheet.GetSpriteSheet("_Main_option", 71), 10);

                    __spell_indicator_anim.AutoResetAnim = true;
                    __spell_indicator_anim.Ini(Manager.TypeGfx.Top, "__spell_indicator_anim", true);
                    __spell_indicator_anim.img.zindex = HudHandle.Pc_Indicator.zindex + 1000;
                    __spell_indicator_anim.Start();
                    Manager.manager.GfxTopList.Add(__spell_indicator_anim);

                    // bouton fermer
                    Bmp __closeHelpMenu = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__closeHelpMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 72));
                    __closeHelpMenu.point = new Point(__helpMenuParent.size.Width - 1 - __closeHelpMenu.rectangle.Width - 1, 3);
                    __closeHelpMenu.MouseOver += CommonCode.CursorHand_MouseMove;
                    __closeHelpMenu.MouseOut += CommonCode.CursorDefault_MouseOut;
                    __closeHelpMenu.MouseClic += __closeHelpMenu_MouseClic;
                    __helpMenuParent.Child.Add(__closeHelpMenu);
                    #endregion
                }
                else if (indexInteraction == 5)
                {
                    #region point de vie
                    Rec __helpMenuRec1 = new Rec(Brushes.White, new Point(1, 1), Size.Empty, "__helpMenuRec1", Manager.TypeGfx.Top, true);
                    __helpMenuParent.Child.Add(__helpMenuRec1);

                    // msg1 169
                    Txt __msg1 = new Txt(CommonCode.TranslateText(168), new Point(5, 5), "__msg1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg1);

                    // msg1 170
                    Txt __msg2 = new Txt(CommonCode.TranslateText(169), new Point(5, 20), "__msg2", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg2);

                    // msg1 171
                    Txt __msg3 = new Txt(CommonCode.TranslateText(170), new Point(5, 35), "__msg3", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg3);

                    // determination de la chaine la plus langue pour recadrer le message avec la taille aproprié
                    int a1 = TextRenderer.MeasureText(__msg1.Text, __msg1.font).Width;
                    int a2 = TextRenderer.MeasureText(__msg2.Text, __msg2.font).Width;
                    int a3 = TextRenderer.MeasureText(__msg3.Text, __msg3.font).Width;

                    int max = Math.Max(a1, a2);
                    max = Math.Max(max, a3);

                    __helpMenuParent.size.Width = 10 + max;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Width = 8 + max;

                    // determination de la longeur du cadre, on prend la position vertical du dernier item + sa longeur vertical
                    int leng = 35 + TextRenderer.MeasureText(__msg3.Text, __msg3.font).Height + 5;
                    __helpMenuParent.size.Height = leng;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Height = leng - 2;

                    __helpMenuParent.point.X = __iruka.point.X + (__iruka.rectangle.Width / 2) - (__helpMenuParent.size.Width / 2);

                    // bouton next
                    Bmp __next = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__next", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 70));
                    __next.point.X = __helpMenuParent.size.Width - 5 - __next.rectangle.Size.Width;
                    __next.point.Y = __helpMenuParent.size.Height - 5 - __next.rectangle.Size.Height;
                    __next.MouseClic += __next_MouseClic;
                    __next.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__next);

                    // bouton preview
                    Bmp __preview = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__preview", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 69));
                    __preview.point.X = __helpMenuParent.size.Width - 5 - __preview.rectangle.Size.Width - 2 - __preview.rectangle.Width;
                    __preview.point.Y = __helpMenuParent.size.Height - 5 - __preview.rectangle.Size.Height;
                    __preview.MouseClic += __preview_MouseClic;
                    __preview.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__preview);

                    int x = HudHandle.HealthBarRec1.point.X + (HudHandle.HealthBarRec1.size.Width / 2) - 50;
                    int y = HudHandle.HealthBarRec1.point.Y - 62;
                    Anim __spell_indicator_anim = new Anim(1, 1);

                    __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x, y, SpriteSheet.GetSpriteSheet("_Main_option", 71), 1000);
                    for (int cnt = 1; cnt < 10; cnt++)
                        __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - (cnt * 10), y - (cnt * 10), SpriteSheet.GetSpriteSheet("_Main_option", 71), 30);

                    __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - 90, y - 90, SpriteSheet.GetSpriteSheet("_Main_option", 71), 100);

                    for (int cnt = 9; cnt > 0; cnt--)
                        __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - (cnt * 10), y - (cnt * 10), SpriteSheet.GetSpriteSheet("_Main_option", 71), 10);

                    __spell_indicator_anim.AutoResetAnim = true;
                    __spell_indicator_anim.Ini(Manager.TypeGfx.Top, "__spell_indicator_anim", true);
                    __spell_indicator_anim.img.zindex = HudHandle.Pc_Indicator.zindex + 1000;
                    __spell_indicator_anim.Start();
                    Manager.manager.GfxTopList.Add(__spell_indicator_anim);

                    // bouton fermer
                    Bmp __closeHelpMenu = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__closeHelpMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 72));
                    __closeHelpMenu.point = new Point(__helpMenuParent.size.Width - 1 - __closeHelpMenu.rectangle.Width - 1, 3);
                    __closeHelpMenu.MouseOver += CommonCode.CursorHand_MouseMove;
                    __closeHelpMenu.MouseOut += CommonCode.CursorDefault_MouseOut;
                    __closeHelpMenu.MouseClic += __closeHelpMenu_MouseClic;
                    __helpMenuParent.Child.Add(__closeHelpMenu);
                    #endregion
                }
                else if (indexInteraction == 6)
                {
                    #region point de caractéristiques
                    Rec __helpMenuRec1 = new Rec(Brushes.White, new Point(1, 1), Size.Empty, "__helpMenuRec1", Manager.TypeGfx.Top, true);
                    __helpMenuParent.Child.Add(__helpMenuRec1);

                    // msg1 172
                    Txt __msg1 = new Txt(CommonCode.TranslateText(171), new Point(5, 5), "__msg1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg1);

                    // msg1 173
                    Txt __msg2 = new Txt(CommonCode.TranslateText(172), new Point(5, 20), "__msg2", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __helpMenuParent.Child.Add(__msg2);

                    // determination de la chaine la plus langue pour recadrer le message avec la taille aproprié
                    int a1 = TextRenderer.MeasureText(__msg1.Text, __msg1.font).Width;
                    int a2 = TextRenderer.MeasureText(__msg2.Text, __msg2.font).Width;

                    int max = Math.Max(a1, a2);

                    __helpMenuParent.size.Width = 10 + max;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Width = 8 + max;

                    // determination de la longeur du cadre, on prend la position vertical du dernier item + sa longeur vertical
                    int leng = 20 + TextRenderer.MeasureText(__msg2.Text, __msg2.font).Height + 5;
                    __helpMenuParent.size.Height = leng;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Height = leng - 2;

                    __helpMenuParent.point.X = __iruka.point.X + (__iruka.rectangle.Width / 2) - (__helpMenuParent.size.Width / 2);

                    // bouton next
                    Bmp __next = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__next", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 70));
                    __next.point.X = __helpMenuParent.size.Width - 5 - __next.rectangle.Size.Width;
                    __next.point.Y = __helpMenuParent.size.Height - 5 - __next.rectangle.Size.Height;
                    __next.MouseClic += __next_MouseClic;
                    __next.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__next);

                    // bouton preview
                    Bmp __preview = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__preview", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 69));
                    __preview.point.X = __helpMenuParent.size.Width - 5 - __preview.rectangle.Size.Width - 2 - __preview.rectangle.Width;
                    __preview.point.Y = __helpMenuParent.size.Height - 5 - __preview.rectangle.Size.Height;
                    __preview.MouseClic += __preview_MouseClic;
                    __preview.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__preview);

                    int x = HudHandle.StatsIcon.point.X + (HudHandle.StatsIcon.rectangle.Width / 2) - 50;
                    int y = HudHandle.StatsIcon.point.Y - 62;
                    Anim __spell_indicator_anim = new Anim(1, 1);

                    __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x, y, SpriteSheet.GetSpriteSheet("_Main_option", 71), 1000);
                    for (int cnt = 1; cnt < 10; cnt++)
                        __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - (cnt * 10), y - (cnt * 10), SpriteSheet.GetSpriteSheet("_Main_option", 71), 30);

                    __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - 90, y - 90, SpriteSheet.GetSpriteSheet("_Main_option", 71), 100);

                    for (int cnt = 9; cnt > 0; cnt--)
                        __spell_indicator_anim.AddCell(@"gfx\general\obj\1\all1.dat", 0, x - (cnt * 10), y - (cnt * 10), SpriteSheet.GetSpriteSheet("_Main_option", 71), 10);

                    __spell_indicator_anim.AutoResetAnim = true;
                    __spell_indicator_anim.Ini(Manager.TypeGfx.Top, "__spell_indicator_anim", true);
                    __spell_indicator_anim.img.zindex = HudHandle.Pc_Indicator.zindex + 1000;
                    __spell_indicator_anim.Start();
                    Manager.manager.GfxTopList.Add(__spell_indicator_anim);

                    // bouton fermer
                    Bmp __closeHelpMenu = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__closeHelpMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 72));
                    __closeHelpMenu.point = new Point(__helpMenuParent.size.Width - 1 - __closeHelpMenu.rectangle.Width - 1, 3);
                    __closeHelpMenu.MouseOver += CommonCode.CursorHand_MouseMove;
                    __closeHelpMenu.MouseOut += CommonCode.CursorDefault_MouseOut;
                    __closeHelpMenu.MouseClic += __closeHelpMenu_MouseClic;
                    __helpMenuParent.Child.Add(__closeHelpMenu);
                    #endregion
                }
                else if (indexInteraction == 7)
                {
                    #region defie
                    Rec __helpMenuRec1 = new Rec(Brushes.White, new Point(1, 1), Size.Empty, "__helpMenuRec1", Manager.TypeGfx.Top, true);
                    __helpMenuRec1.zindex = 1;
                    __helpMenuParent.Child.Add(__helpMenuRec1);

                    // msg1 174
                    Txt __msg1 = new Txt(CommonCode.TranslateText(173), new Point(5, 5), "__msg1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                    __msg1.zindex = 2;
                    __helpMenuParent.Child.Add(__msg1);

                    // msg1 175
                    Txt __msg2 = new Txt(CommonCode.TranslateText(174), new Point(5, 20), "__msg2", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Blue);
                    __msg2.zindex = 3;
                    __msg2.MouseClic += __msg2_MouseClic;
                    __msg2.MouseMove += MainForm.HandCursorTxt;
                    __msg2.MouseOut += MainForm.DefaultCursorRec;
                    __msg2.EscapeGfxWhileMouseClic = true;
                    __helpMenuParent.Child.Add(__msg2);

                    Rec __startBattleRec = new Rec(Brushes.White, Point.Empty, Size.Empty, "__startBattleRec", Manager.TypeGfx.Top, true);
                    __helpMenuParent.Child.Add(__startBattleRec);
                    __startBattleRec.zindex = __msg2.zindex - 1;
                    __startBattleRec.MouseClic += __startBattleRec_MouseClic;
                    __startBattleRec.MouseMove += MainForm.HandCursorRec;
                    __startBattleRec.point = new Point(__msg2.point.X, __msg2.point.Y);
                    __startBattleRec.size = new Size(TextRenderer.MeasureText(__msg2.Text, __msg2.font).Width, TextRenderer.MeasureText(__msg2.Text, __msg2.font).Height);

                    // determination de la chaine la plus langue pour recadrer le message avec la taille aproprié
                    int a1 = TextRenderer.MeasureText(__msg1.Text, __msg1.font).Width;
                    int a2 = TextRenderer.MeasureText(__msg2.Text, __msg2.font).Width;

                    int max = Math.Max(a1, a2);

                    __helpMenuParent.size.Width = 10 + max;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Width = 8 + max;

                    // determination de la longeur du cadre, on prend la position vertical du dernier item + sa longeur vertical
                    int leng = 20 + TextRenderer.MeasureText(__msg2.Text, __msg2.font).Height + 5;
                    __helpMenuParent.size.Height = leng;
                    (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Height = leng - 2;
                    __helpMenuParent.point.X = __iruka.point.X + (__iruka.rectangle.Width / 2) - (__helpMenuParent.size.Width / 2);

                    // bouton preview
                    Bmp __preview = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__preview", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 69));
                    __preview.point.X = __helpMenuParent.size.Width - 5 - __preview.rectangle.Size.Width - 2 - __preview.rectangle.Width;
                    __preview.point.Y = __helpMenuParent.size.Height - 5 - __preview.rectangle.Size.Height;
                    __preview.MouseClic += __preview_MouseClic;
                    __preview.MouseMove += CommonCode.CursorHand_MouseMove;
                    __helpMenuParent.Child.Add(__preview);

                    // bouton fermer
                    Bmp __closeHelpMenu = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__closeHelpMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 72));
                    __closeHelpMenu.point = new Point(__helpMenuParent.size.Width - 1 - __closeHelpMenu.rectangle.Width - 1, 3);
                    __closeHelpMenu.MouseOver += CommonCode.CursorHand_MouseMove;
                    __closeHelpMenu.MouseOut += CommonCode.CursorDefault_MouseOut;
                    __closeHelpMenu.MouseClic += __closeHelpMenu_MouseClic;
                    __helpMenuParent.Child.Add(__closeHelpMenu);
                    #endregion
                }
            }
            else if(quete_FirstFight_done)
            {
                // quete deja faite, on affiche le message pour tp map village
                // supression de quelque images qui sont généré que sur certain niveau de discution comme la fleche qui montre les endroits
                if (Manager.manager.GfxTopList.Exists(f => f.Name() == "__spell_indicator_anim"))
                {
                    Anim __spell_indicator_anim = Manager.manager.GfxTopList.Find(f => f.Name() == "__spell_indicator_anim") as Anim;
                    __spell_indicator_anim.Close();
                    Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "__spell_indicator_anim");
                }

                /*if (direction == "next")
                    indexInteraction++;
                else
                    indexInteraction--;*/

                //if (indexInteraction == 2)
                //{
                #region tp
                Rec __helpMenuRec1 = new Rec(Brushes.White, new Point(1, 1), Size.Empty, "__helpMenuRec1", Manager.TypeGfx.Top, true);
                __helpMenuRec1.zindex = 1;
                __helpMenuParent.Child.Add(__helpMenuRec1);

                // msg1 177
                Txt __msg1 = new Txt(CommonCode.TranslateText(176), new Point(5, 5), "__msg1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                __msg1.zindex = 2;
                __helpMenuParent.Child.Add(__msg1);

                // msg1 178
                Txt __msg2 = new Txt(CommonCode.TranslateText(177), new Point(5, 20), "__msg2", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                __msg2.zindex = 2;
                __helpMenuParent.Child.Add(__msg2);

                // msg1 179
                Txt __msg3 = new Txt(CommonCode.TranslateText(178), new Point(5, 35), "__msg3", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Blue);
                __msg3.zindex = 3;
                __msg3.MouseClic += __msg3_MouseClic;
                __msg3.MouseMove += MainForm.HandCursorTxt;
                __msg3.MouseOut += MainForm.DefaultCursorRec;
                __msg3.EscapeGfxWhileMouseClic = true;
                __helpMenuParent.Child.Add(__msg3);

                Rec __TpRec = new Rec(Brushes.White, Point.Empty, Size.Empty, "__TpRec", Manager.TypeGfx.Top, true);
                __helpMenuParent.Child.Add(__TpRec);
                __TpRec.zindex = __msg3.zindex - 1;
                __TpRec.MouseClic += __TpRec_MouseClic;
                __TpRec.MouseMove += MainForm.HandCursorRec;
                __TpRec.point = new Point(__msg3.point.X, __msg3.point.Y);
                __TpRec.size = new Size(TextRenderer.MeasureText(__msg3.Text, __msg3.font).Width, TextRenderer.MeasureText(__msg3.Text, __msg3.font).Height);

                // determination de la chaine la plus langue pour recadrer le message avec la taille aproprié
                int a1 = TextRenderer.MeasureText(__msg1.Text, __msg1.font).Width;
                int a2 = TextRenderer.MeasureText(__msg2.Text, __msg2.font).Width;
                int a3 = TextRenderer.MeasureText(__msg3.Text, __msg3.font).Width;

                int max = Math.Max(a1, a2);
                max = Math.Max(max, a3);

                __helpMenuParent.size.Width = 10 + max;
                (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Width = 8 + max;

                // determination de la longeur du cadre, on prend la position vertical du dernier item + sa longeur vertical
                int leng = 35 + TextRenderer.MeasureText(__msg3.Text, __msg3.font).Height + 5;
                __helpMenuParent.size.Height = leng;
                (__helpMenuParent.Child.Find(f => f.Name() == "__helpMenuRec1") as Rec).size.Height = leng - 2;
                __helpMenuParent.point.X = __iruka.point.X + (__iruka.rectangle.Width / 2) - (__helpMenuParent.size.Width / 2);

                // bouton fermer
                Bmp __closeHelpMenu = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__closeHelpMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 72));
                __closeHelpMenu.point = new Point(__helpMenuParent.size.Width - 1 - __closeHelpMenu.rectangle.Width - 1, 3);
                __closeHelpMenu.MouseOver += CommonCode.CursorHand_MouseMove;
                __closeHelpMenu.MouseOut += CommonCode.CursorDefault_MouseOut;
                __closeHelpMenu.MouseClic += __closeHelpMenu_MouseClic;
                __helpMenuParent.Child.Add(__closeHelpMenu);
                #endregion
                //}
            }
        }

        void __TpRec_MouseClic(Rec rec, MouseEventArgs e)
        {
            __closeHelpMenu_MouseClic(null, null);
            Network.SendMessage("cmd•InteractWithPNJ•iruka•tp•1", true);
        }

        void __msg3_MouseClic(Txt txt, MouseEventArgs e)
        {
            __closeHelpMenu_MouseClic(null, null);
            Network.SendMessage("cmd•InteractWithPNJ•iruka•tp•1", true);
        }
        void __closeHelpMenu_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // effacement du menu help suite au clic sur le bouton fermer
            if (Manager.manager.GfxTopList.Exists(f => f.Name() == "__helpMenuParent"))
            {
                Rec __helpMenuParent = Manager.manager.GfxTopList.Find(f => f.Name() == "__helpMenuParent") as Rec;
                __helpMenuParent.Child.Clear();
                Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "__helpMenuParent");
                indexInteraction = 0;
            }
            // supression de quelque images qui sont généré que sur certain niveau de discution comme la fleche qui montre les endroits
            if (Manager.manager.GfxTopList.Exists(f => f.Name() == "__spell_indicator_anim"))
            {
                Anim __spell_indicator_anim = Manager.manager.GfxTopList.Find(f => f.Name() == "__spell_indicator_anim") as Anim;
                __spell_indicator_anim.Close();
                Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "__spell_indicator_anim");
            }
        }
        void __msg2_MouseClic(Txt txt, MouseEventArgs e)
        {
            __closeHelpMenu_MouseClic(null, null);
            Network.SendMessage("cmd•InteractWithPNJ•iruka•acceptFirstFight", true);
        }
        private void __startBattleRec_MouseClic(Rec rec, MouseEventArgs e)
        {
            __closeHelpMenu_MouseClic(null, null);
            Network.SendMessage("cmd•InteractWithPNJ•iruka•acceptFirstFight", true);
        }
        void __preview_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            addContenteToInteractionMessage("preview");
        }
        void __next_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            addContenteToInteractionMessage("next");
        }
        void contextMenuRecMP3_MouseMove(Rec rec, MouseEventArgs e)
        {
            #region //survole sur menu contextuel / message privé
            CommonCode.CursorHand_MouseMove(null, null);
            #endregion
        }
        void interactionNotProceeded_Tick(object sender, EventArgs e)
        {
            // Message du PNJ qui demande de clicker avec le bouton droit pour lancer le dialogue
            if (Battle.state == Enums.battleState.state.idle)
                CommonCode.ChatMsgFormat("G", "iruka", CommonCode.TranslateText(156));
        }
        void DefaultCursor(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        public void contextMenuRecDefie2_MouseOver(Rec rec, MouseEventArgs e)
        {
            rec.brush = Brushes.GreenYellow;
        }
        void contextMenuRecMP3_MouseOut(Rec rec, MouseEventArgs e)
        {
            #region
            // mouseout sur menu contextuel / message privé
            CommonCode.CursorDefault_MouseOut(null, null);
            rec.brush = Brushes.White;
            #endregion
        }
        public void Network_stat(string stat)
        {
            try
            {
                Manager.manager.mainForm.Invoke(new DelHandel(Handle_Network_Stat), stat);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error 1 " + ex.ToString());
            }
        }
        public void Handle_Network_Stat(string stat)
        {
            string[] cmd = stat.Split('•');

            if (cmd[0] == "internal")
            {
                #region
                if (cmd[1] == "network")
                {
                    if (cmd[2] == "connection")
                    {
                        if (cmd[3] == "failed")
                        {
                            MessageBox.Show(CommonCode.TranslateText(4) + "\n" + cmd[4], "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MainForm.DrawDisconnectImg(true);
                            GameStateManager.ChangeState(new LoginMap());
                            GameStateManager.CheckState();
                        }
                        else if (cmd[3] == "aborted")
                        {
                            MessageBox.Show(CommonCode.TranslateText(2) + "\n" + cmd[4], "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MainForm.DrawDisconnectImg(true);
                            GameStateManager.ChangeState(new LoginMap());
                            GameStateManager.CheckState();
                        }
                    }
                }
                #endregion
            }
        }
        public void Update()
        {
            
        }
        public void CleanUp()
        {
            Manager.manager.Clear();
            System.GC.Collect();
            interactionNotProceeded.Stop();     // arret de timer au cas ou il est toujour activé
        }
        public static bool isFreeCellToWalk (Point p)
        {
            // contient tous les tuiles non accessible sur la map pour le mode marche
            // partie obstacles du map
            if (p.X >= 540 && p.X < 570 && p.Y >= 90 && p.Y < 120)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 180 && p.X < 210 && p.Y >= 330 && p.Y < 360)    // obstacles 1 posés sur le map
                return false;
            else if (p.X >= 330 && p.X < 360 && p.Y >= 120 && p.Y < 150)    // obstacles 2 posés sur le map
                return false;
            // ligne 1
            else if (p.X >= 0 && p.X < 30 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne vertical 2
            else if (p.X >= 30 && p.X < 60 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne vertical 3
            else if (p.X >= 60 && p.X < 90 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 4
            else if (p.X >= 90 && p.X < 120 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            // ligne 5
            else if (p.X >= 960 && p.X < 990 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 6
            else if (p.X >= 930 && p.X < 960 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 7
            else if (p.X >= 900 && p.X < 930 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 8
            else if (p.X >= 870 && p.X < 900 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            else
            {
                // verification si le joueur est en combat vus que les participant sont concidérés comme un obstacle aussi
                if (Battle.state == Enums.battleState.state.idle)
                    return true;
                else
                {
                    // recherche parmie les jouers persent s'il coincide avec la meme pose
                    bool found = false;
                    for (int cnt = 0; cnt < Battle.AllPlayersByOrder.Count; cnt++)
                    {
                        Point p2 = Battle.AllPlayersByOrder[cnt].realPosition;
                        p2.X *= 30;
                        p2.Y *= 30;

                        if (p.X >= p2.X && p.X < p2.X + 30 && p.Y >= p2.Y && p.Y < p2.Y + 30)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        return false;
                    else
                        return true;
                }
            }
        }
        public static bool isFreeCellToSpell(Point p)
        {
            // contient tous les tuiles non accessible sur la map pour les sorts
            // partie obstacles du map
            if (p.X >= 540 && p.X < 570 && p.Y >= 90 && p.Y < 120)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 180 && p.X < 210 && p.Y >= 330 && p.Y < 360)    // obstacles 1 posés sur le map
                return false;
            else if (p.X >= 330 && p.X < 360 && p.Y >= 120 && p.Y < 150)    // obstacles 2 posés sur le map
                return false;
            // contient tous les tuiles non accessible sur la map pour le mode marche
            // partie obstacles du map
            if (p.X >= 540 && p.X < 570 && p.Y >= 90 && p.Y < 120)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 180 && p.X < 210 && p.Y >= 330 && p.Y < 360)    // obstacles 1 posés sur le map
                return false;
            else if (p.X >= 330 && p.X < 360 && p.Y >= 120 && p.Y < 150)    // obstacles 2 posés sur le map
                return false;
            // ligne 1
            else if (p.X >= 0 && p.X < 30 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 0 && p.X < 30 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne vertical 2
            else if (p.X >= 30 && p.X < 60 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 30 && p.X < 60 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne vertical 3
            else if (p.X >= 60 && p.X < 90 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 60 && p.X < 90 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 4
            else if (p.X >= 90 && p.X < 120 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 90 && p.X < 120 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            // ligne 5
            else if (p.X >= 960 && p.X < 990 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 960 && p.X < 990 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 6
            else if (p.X >= 930 && p.X < 960 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 930 && p.X < 960 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 7
            else if (p.X >= 900 && p.X < 930 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 900 && p.X < 930 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            // ligne 8
            else if (p.X >= 870 && p.X < 900 && p.Y >= 0 && p.Y < 30)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 30 && p.Y < 60)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 60 && p.Y < 90)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 90 && p.Y < 120)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 120 && p.Y < 150)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 150 && p.Y < 180)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 180 && p.Y < 210)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 210 && p.Y < 240)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 240 && p.Y < 270)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 270 && p.Y < 300)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 300 && p.Y < 330)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 330 && p.Y < 360)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 360 && p.Y < 390)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 390 && p.Y < 420)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 420 && p.Y < 450)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 450 && p.Y < 480)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 480 && p.Y < 510)          // arbres
                return false;
            else if (p.X >= 870 && p.X < 900 && p.Y >= 510 && p.Y < 540)          // arbres
                return false;
            else
            {
                // verification si le joueur est en combat vus que les participant sont concidéré comme un obstacle aussi
                if (Battle.state == Enums.battleState.state.idle)
                    return true;
                else
                {
                    // recherche parmie les jouers persent s'il coincide avec la meme pose
                    bool found = false;
                    for (int cnt = 0; cnt < Battle.AllPlayersByOrder.Count; cnt++)
                    {
                        Point p2 = new Point();
                        p2.X = Battle.AllPlayersByOrder[cnt].realPosition.X * 30;
                        p2.Y = Battle.AllPlayersByOrder[cnt].realPosition.Y * 30;

                        if (p.X >= p2.X && p.X < p2.X + 30 && p.Y >= p2.Y && p.Y < p2.Y + 30)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        return false;
                    else
                        return true;
                }
            }
        }
    }
}
