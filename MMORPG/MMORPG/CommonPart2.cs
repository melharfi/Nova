using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using MELHARFI.Gfx;
using MELHARFI.AStarAlgo;
using System.Windows.Forms;
using MELHARFI;
using MMORPG.Net.Messages.Request;

namespace MMORPG
{
    public partial class CommonCode
    {
        public static void AnimAction(Bmp ibplayer, IList<Point> wayPointList, int speed)
        {
            #region mouvement d'un personnage
            abortAnimActionThread = false;
            // bloquer le flux des données recus pour attendre l'arret du joueur avant de lancer une cmd comme un sort ou autre
            if (Battle.state == Enums.battleState.state.started)
            {
                blockNetFlow = true;
                ChatMsgFormat("S", "null", "blockNetFlow3 = true");
            }
            
            Actor actor = (Actor)ibplayer.tag;
            //créée une animation en ce basons sur le waypoint
            if (Thread.CurrentThread.Name == null)
                Thread.CurrentThread.Name = "WayPoint_AnimThread_" + actor.pseudo;

            // copie du way point vus qu'il va s'éffacer et on dois le passer en référence sur la methode qui affiche les pm perdus
            List<Point> wayPointListCopy = wayPointList.Select(item => item).ToList();

            // un bug arrive quand le joueur spam le click sur la map, du coup un waypoint se regénére mais vide bizarement
            if (wayPointList.Count == 0)
                return;

            // tracé du trajectoire selon le wayPointList
            List<string> wayDirection = new List<string>(); // contiens la direction des trajectoires de la liste wayPointList2,ils doivents avoir le même nombre d'element, 2 en générale (le waypoint ne support pas plus de 1 obstacle)

            for (int cnt = 0; cnt < wayPointList.Count; cnt++)
            {
                if (cnt == 0)
                {
                    if ((ibplayer.point.X / 30 * 30) > wayPointList[cnt].X)
                        wayDirection.Add("left");
                    else if ((ibplayer.point.X / 30 * 30) < wayPointList[cnt].X)
                        wayDirection.Add("right");
                    else if (((ibplayer.point.Y + ibplayer.rectangle.Height) / 30 * 30) > wayPointList[cnt].Y)
                        wayDirection.Add("up");
                    else if (((ibplayer.point.Y + ibplayer.rectangle.Height) / 30 * 30) < wayPointList[cnt].Y)
                        wayDirection.Add("down");
                }
                else
                {
                    if (wayPointList[cnt - 1].X > wayPointList[cnt].X)
                        wayDirection.Add("left");
                    else if (wayPointList[cnt - 1].X < wayPointList[cnt].X)
                        wayDirection.Add("right");
                    else if (wayPointList[cnt - 1].Y < wayPointList[cnt].Y)
                        wayDirection.Add("down");
                    else if (wayPointList[cnt - 1].Y > wayPointList[cnt].Y)
                        wayDirection.Add("up");
                }
            }

            int curDirIndex = 0;              // pour basculer entre les itérations de la liste wayDirection
            int curSprite = 1;                // contiens l'id de la spriteSheet a afficher
            int intervalChangeSprite = 2;     // combiens de temps pour l'incrementation du curSprite qui change de figure
            int curTuile = 0;              // contient le nombre de tuiles passés lors du deplacement du joueurs
            int timestamp = ReturnTimeStamp();

            //try
            //{
                while (!Manager.manager.mainForm.IsDisposed)
                {
                    // deplacement du joueurs
                    if (wayDirection.Count < curDirIndex)
                        return;

                // un bug arrive quand le joueur spam le click sur la map, du coup un waypoint se regénére mais vide bizarement
                if (wayPointList.Count == 0)
                    return;

                if (wayDirection[curDirIndex] == "right")
                    {
                        if ((ibplayer.point.X / 30 * 30) < wayPointList[curDirIndex].X)
                        {
                            if (intervalChangeSprite == 2)
                            {
                                ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(actor.className.ToString(), 8 + curSprite));

                                curSprite++;
                                if (curSprite == 4)
                                    curSprite = 0;
                                intervalChangeSprite = 0;
                                ibplayer.point.X += 6;
                            }
                            else
                                intervalChangeSprite++;
                            ibplayer.point.X += 2;
                        }
                        else
                        {
                            if (curDirIndex == wayPointList.Count - 1)
                                break;
                            curDirIndex++;
                        }
                    }
                    else if (wayDirection[curDirIndex] == "left")
                    {
                        if ((ibplayer.point.X / 30 * 30) > wayPointList[curDirIndex].X)
                        {
                            if (intervalChangeSprite == 2)
                            {
                                ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(actor.className.ToString(), 4 + curSprite));

                                curSprite++;
                                if (curSprite == 4)
                                    curSprite = 0;
                                intervalChangeSprite = 0;
                                ibplayer.point.X -= 6;
                            }
                            else
                                intervalChangeSprite++;
                            ibplayer.point.X -= 2;
                        }
                        else
                        {
                            if (curDirIndex == wayPointList.Count - 1)
                                break;
                            else
                                curDirIndex++;
                        }
                    }
                    else if (wayDirection[curDirIndex] == "down")
                    {
                        if (ibplayer.point.Y + ibplayer.rectangle.Height - 15 < wayPointList[curDirIndex].Y)
                        {
                            if (intervalChangeSprite == 2)
                            {
                                ibplayer.point.X = (ibplayer.point.X / 30 * 30) + 15 - (ibplayer.rectangle.Width / 2);
                                ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(actor.className.ToString(), 0 + curSprite));

                                curSprite++;
                                if (curSprite == 4)
                                    curSprite = 0;
                                intervalChangeSprite = 0;
                                ibplayer.point.Y += 6;
                            }
                            else
                                intervalChangeSprite++;
                            ibplayer.point.Y += 2;
                        }
                        else
                        {
                            if (curDirIndex == wayPointList.Count - 1)
                                break;
                            else
                                curDirIndex++;
                        }
                    }
                    else if (wayDirection[curDirIndex] == "up")
                    {
                        if (ibplayer.point.Y + ibplayer.rectangle.Height - 15 > wayPointList[curDirIndex].Y)
                        {
                            if (intervalChangeSprite == 2)
                            {
                                ibplayer.point.X = (ibplayer.point.X / 30 * 30) + 15 - (ibplayer.rectangle.Width / 2);
                                ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(actor.className.ToString(), 12 + curSprite));

                                curSprite++;
                                if (curSprite == 4)
                                    curSprite = 0;
                                intervalChangeSprite = 0;
                                ibplayer.point.Y -= 6;
                            }
                            else
                                intervalChangeSprite++;
                            ibplayer.point.Y -= 2;
                        }
                        else
                        {
                            if (curDirIndex == wayPointList.Count - 1)
                                break;
                            else
                                curDirIndex++;
                        }
                    }

                    // envoie au serveur si le client viens de passer une tuile, seulement s'il sagit de notre joueur
                    // si le joueur en action est notre joueur

                    if (ibplayer == MyPlayerInfo.instance.ibPlayer && curTuile < wayPointList.Count)
                    {
                        // determiner si le player a passé une case ou non pour informer le serveur afin qu'il controle si le temps entre chaque passage de tuile est conforme
                        if ((curTuile < wayPointList.Count) && (new Point(ibplayer.point.X / 30 * 30, (ibplayer.point.Y + ibplayer.rectangle.Height) / 30 * 30) == wayPointList[curTuile]))
                        {
                            WayPointTilePassedRequestMessage wayPointTilePassedRequestMessage = new WayPointTilePassedRequestMessage(wayPointList[curTuile].X + "," + wayPointList[curTuile].Y);
                            wayPointTilePassedRequestMessage.Serialize();
                            wayPointTilePassedRequestMessage.Send();
                            curTuile++;

                            VerticalSyncZindex(ibplayer);

                            if (Battle.state == Enums.battleState.state.started)
                            {
                                Battle.AllPlayersByOrder.Find(f => f.pseudo == actor.pseudo).currentPm--;
                                HudHandle.UpdatePm();
                            }
                        }
                    }
                    else
                    {
                        if ((curTuile < wayPointList.Count) && (new Point(ibplayer.point.X / 30 * 30, (ibplayer.point.Y + ibplayer.rectangle.Height) / 30 * 30) == wayPointList[curTuile]))
                        {
                            curTuile++;

                            VerticalSyncZindex(ibplayer);

                            if (Battle.state == Enums.battleState.state.started)
                            {
                                Battle.AllPlayersByOrder.Find(f => f.pseudo == actor.pseudo).currentPm--;
                                HudHandle.UpdatePm(ibplayer.name);
                            }
                        }
                    }
                    // maj du zindex selon la position horizontal
                    VerticalSyncZindex(ibplayer);
                    if (abortAnimActionThread)
                        break;
                    Thread.Sleep(speed);
                }
            //}
            /*catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }*/

            // mettre le joueur sur la position debout "ne marche pas" selon l'orientation
            if (wayDirection[wayDirection.Count - 1] == "up")
                ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(actor.className.ToString(), 12));
            else if (wayDirection[wayDirection.Count - 1] == "down")
                ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(actor.className.ToString(), 0));
            else if (wayDirection[wayDirection.Count - 1] == "right")
                ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(actor.className.ToString(), 8));
            else
                ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(actor.className.ToString(), 4));

            // un bug arrive quand le joueur spam le click sur la map, du coup un waypoint se regénére mais vide bizarement
            if (wayPointList.Count == 0)
                return;
            
            // recalibrage de la position du joueur
            ibplayer.point.X = wayPointList[wayPointList.Count - 1].X + 15 - (ibplayer.rectangle.Width / 2);
            ibplayer.point.Y = wayPointList[wayPointList.Count - 1].Y + 15 - ibplayer.rectangle.Height;

            // reinitialisation des informations de mouvement
            if(Battle.state == Enums.battleState.state.idle)
            {
                actor.animatedAction = Enums.AnimatedActions.Name.idle;
                actor.realPosition = new Point(wayPointList[wayPointList.Count - 1].X / 30, wayPointList[wayPointList.Count - 1].Y / 30);
                actor.wayPoint.Clear();
            }
            else
            {
                Battle.AllPlayersByOrder.Find(f => f.pseudo == actor.pseudo).realPosition = new Point(wayPointList[wayPointList.Count - 1].X / 30, wayPointList[wayPointList.Count - 1].Y / 30);
                Battle.AllPlayersByOrder.Find(f => f.pseudo == actor.pseudo).animatedAction = Enums.AnimatedActions.Name.idle;
                Battle.AllPlayersByOrder.Find(f => f.pseudo == actor.pseudo).wayPoint.Clear();

                // animation de diminution des pm
                Point playerOfPoint = new Point(wayPointListCopy[wayPointListCopy.Count - 1].X / 30, wayPointListCopy[wayPointListCopy.Count - 1].Y / 30);
                Rectangle playerOfRectangle = ibplayer.rectangle;
                int playerOfSpellZindex = ibplayer.zindex;

                new Thread(() =>
                {
                    showAnimUsedPm(wayPointListCopy.Count, playerOfPoint, playerOfRectangle, playerOfSpellZindex);
                }).Start();
            }
            
            // modification du realposition de l'objet en question créer sur la classe battle
            // recoloriage apres chaque changement du bitmap
            ApplyMaskColorToClasse(ibplayer);

            ChatMsgFormat("S", "null", "walk finished");
            // libération du flux des données recus
            blockNetFlow = false;
            ChatMsgFormat("S", "null", "blockNetFlow4 = false");
            #endregion
        }
        public static void DrawPlayerContextMenu(Bmp ibplayer, MouseEventArgs e)
        {
            #region
            // affiche le menu contextuel du joueur
            if (e.Button == MouseButtons.Right)
            {
                int distance = 0;
                // affichage du menu contextuel du joueur
                Rec contextMenuRecParent = new Rec(Brushes.Black, new Point(e.X, e.Y), new Size(86, 50), "__contextMenuRec1", Manager.TypeGfx.Top, true);
                contextMenuRecParent.MouseMove += contextMenuRecParent_MouseMove;
                Manager.manager.GfxTopList.Add(contextMenuRecParent);
                RemoveGfxWhenClicked = contextMenuRecParent;

                Rec contextMenuRecMP2 = new Rec(Brushes.Beige, new Point(1, 1), new Size(84, 48), "__contextMenuRec2", Manager.TypeGfx.Top, true);
                contextMenuRecParent.Child.Add(contextMenuRecMP2);

                if (((Actor)ibplayer.tag).pseudo != CommonCode.MyPlayerInfo.instance.pseudo)
                {
                    ///////// menu message privé
                    Rec contextMenuRecMP3 = new Rec(Brushes.White, new Point(4, 4), new Size(78, 12), "contextMenuRecPM2_" + (ibplayer.tag as Actor).pseudo, Manager.TypeGfx.Top, true);
                    contextMenuRecMP3.tag = (ibplayer.tag as Actor).pseudo;
                    contextMenuRecMP3.EscapeGfxWhileMouseClic = true;
                    contextMenuRecMP3.MouseClic += contextMenuRecMP3_MouseClic;
                    contextMenuRecMP3.MouseMove += contextMenuRecMP3_MouseMove;
                    contextMenuRecMP3.MouseOver += contextMenuRecMP3_MouseOver;
                    contextMenuRecMP3.MouseOut += contextMenuRecMP3_MouseOut;
                    contextMenuRecMP3.EscapeGfxWhileMouseMove = true;
                    contextMenuRecParent.Child.Add(contextMenuRecMP3);

                    Txt contextMenuTxtPM = new Txt(TranslateText(98), new Point(5, 4), "__contextMenuTxtPM", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
                    contextMenuRecParent.Child.Add(contextMenuTxtPM);

                    distance += 15;

                    //////////////// menu défier
                    Rec contextMenuRecDefie2 = new Rec(Brushes.White, new Point(4, distance + 4), new Size(78, 12),
                        "contextMenuRecDefie2_" + (ibplayer.tag as Actor).pseudo, Manager.TypeGfx.Top, true)
                    {
                        tag = contextMenuRecParent,
                        EscapeGfxWhileMouseClic = true
                    };
                    contextMenuRecDefie2.MouseClic += contextMenuRecDefie2_MouseClic;
                    contextMenuRecDefie2.MouseMove += contextMenuRecMP3_MouseMove;
                    contextMenuRecDefie2.MouseOver += contextMenuRecDefie2_MouseOver;
                    contextMenuRecDefie2.MouseOut += contextMenuRecMP3_MouseOut;
                    contextMenuRecDefie2.EscapeGfxWhileMouseMove = true;
                    contextMenuRecParent.Child.Add(contextMenuRecDefie2);

                    Txt contextMenuTxtDefie = new Txt(TranslateText(99), new Point(5, distance + 3), "__contextMenuTxtDefie", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
                    contextMenuRecParent.Child.Add(contextMenuTxtDefie);
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
        public static void DrawBattleContextMenu(Bmp flag, MouseEventArgs e)
        {
            #region // Menu contextuel du drapeau du combat
            // tag = battleID#sideA ou sideB#pseudo#classID#Village#Class Level#Spirit#Level Alignement Séparé par |

            // affiche le menu contextuel du joueur
            if (e.Button == MouseButtons.Left)
            {
                int distance = 0;
                // affichage du menu contextuel du joueur, cadra 1 (conteneur)
                Rec __contextMenuBattleFlag = new Rec(Brushes.Black, new Point(e.X, e.Y), new Size(146, 50), "__contextMenuBattleFlag", Manager.TypeGfx.Top, true);
                __contextMenuBattleFlag.MouseMove += contextMenuRecParent_MouseMove;
                Manager.manager.GfxTopList.Add(__contextMenuBattleFlag);
                RemoveGfxWhenClicked = __contextMenuBattleFlag;

                string tmpTeam = flag.tag.ToString().Split('|')[0].Split('#')[1];
                Enums.Team.Side team = (Enums.Team.Side)Enum.Parse(typeof(Enums.Team.Side), tmpTeam);

                // cadre 2 child
                Rec contextMenuRecMP2 = new Rec((team == Enums.Team.Side.A) ? new Pen(Color.FromArgb(142, 191, 247)).Brush : new Pen(Color.FromArgb(248, 183, 173)).Brush, new Point(1, 1), new Size(144, 48), "__contextMenuRec2", Manager.TypeGfx.Top, true);
                __contextMenuBattleFlag.Child.Add(contextMenuRecMP2);

                for (int cnt = 0; cnt < flag.tag.ToString().Split('|').Length; cnt++)
                {
                    //int battleID = Convert.ToInt32(Flag.tag.ToString().Split('|')[cnt].Split('#')[0]);
                    //string currentTeam = Flag.tag.ToString().Split('|')[cnt].Split('#')[1];
                    string actorNameInstance = flag.tag.ToString().Split('|')[cnt].Split('#')[2];
                    int classID = Convert.ToInt32(flag.tag.ToString().Split('|')[cnt].Split('#')[3]);
                    string village = flag.tag.ToString().Split('|')[cnt].Split('#')[4];
                    int level = Convert.ToInt32(flag.tag.ToString().Split('|')[cnt].Split('#')[5]);
                    string spiritString = flag.tag.ToString().Split('|')[cnt].Split('#')[6];
                    Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit.Name), spiritString);
                    int spiritLvl = Convert.ToInt32(flag.tag.ToString().Split('|')[cnt].Split('#')[7]);

                    ///////// menu message privé
                    Rec contextMenuRecFlag =
                        new Rec(
                            team == Enums.Team.Side.A
                                ? new Pen(Color.FromArgb(138, 191, 247)).Brush
                                : new Pen(Color.FromArgb(248, 183, 173)).Brush, new Point(4, 4), new Size(140, 12),
                            "contextMenuRecFlag_" + actorNameInstance, Manager.TypeGfx.Top, true)
                        {
                            tag = __contextMenuBattleFlag,
                            EscapeGfxWhileMouseClic = true
                        };
                    contextMenuRecFlag.MouseClic += contextMenuBattleFlag_MouseClic;
                    contextMenuRecFlag.MouseMove += contextMenuRecMP3_MouseMove;
                    contextMenuRecFlag.MouseOut += contextMenuBattleFlag_MouseOut;
                    contextMenuRecFlag.EscapeGfxWhileMouseMove = true;
                    contextMenuRecFlag.tag = actorNameInstance;
                    __contextMenuBattleFlag.Child.Add(contextMenuRecFlag);

                    // pseudo
                    Txt contextMenuTxtPseudo = new Txt(actorNameInstance[0].ToString().ToUpper() + actorNameInstance.Substring(1,actorNameInstance.Length - 1), new Point(5, 4), "__contextMenuTxtPseudo", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
                    __contextMenuBattleFlag.Child.Add(contextMenuTxtPseudo);
                    
                    // longeur maximum de 10 caractère
                    int separation1 = TextRenderer.MeasureText("Azertyuiop", new Font("Verdana", 7)).Width - 10;

                    // level
                    Txt contextMenuTxtLevel = new Txt(level.ToString(), new Point(5 + separation1, 4), "__contextMenuTxtLevel", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
                    __contextMenuBattleFlag.Child.Add(contextMenuTxtLevel);

                    int separation2 = TextRenderer.MeasureText("200", new Font("Verdana", 7)).Width;

                    // class image
                    Bmp classBmp = new Bmp(@"gfx\general\classes\classes_thumbs\" + IdToClassName(classID) + ".dat", new Point(5 + separation1 + separation2, 4), new Size(10, 10), 1);
                    __contextMenuBattleFlag.Child.Add(classBmp);

                    // village image
                    Bmp villageBmp = new Bmp(@"gfx\general\obj\1\pays_thumbs\" + village + ".dat", new Point(classBmp.point.X + classBmp.rectangle.Width, 4), new Size(10, 10), 1);
                    __contextMenuBattleFlag.Child.Add(villageBmp);

                    // Spirite image
                    if (spirit != Enums.Spirit.Name.neutral)
                    {
                        Bmp spiritBmp = new Bmp(@"gfx\general\obj\2\" + spiritString + @"\15.dat", new Point(villageBmp.point.X + villageBmp.rectangle.Width, 4), new Size(10, 10), 1);
                        __contextMenuBattleFlag.Child.Add(spiritBmp);

                        // Spirit level
                        Txt contextMenuTxtSpiritLevel = new Txt("G " + spiritLvl.ToString(), new Point(spiritBmp.point.X + spiritBmp.rectangle.Width, 4), "__contextMenuTxtLevel", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
                        __contextMenuBattleFlag.Child.Add(contextMenuTxtSpiritLevel);
                    }
                    distance += 15;
                }
                contextMenuRecMP2.size.Height = distance + 2;
                __contextMenuBattleFlag.size.Height = distance + 4;
            }
            else if(e.Button == MouseButtons.Right)
            {
                // menu pour rejoindre le combat
            }
            #endregion
        }
        public static void DrawAskingToChallengeMeMenu(string actorName)
        {
            #region reception d'une demande de défie
            // menue pour celui qui a été demandé
            Bmp _bg_datcmm = new Bmp(@"gfx\general\obj\1\bg_tamise1.dat", new Point(0, 0), "_bg_datcmm", Manager.TypeGfx.Top, true, 1);
            Manager.manager.GfxTopList.Add(_bg_datcmm);

            Rec _atcmm1 = new Rec(Brushes.Black, Point.Empty, new Size(200, 80), "__atcmm1", Manager.TypeGfx.Top, true);
            _atcmm1.point = new Point((ScreenManager.WindowWidth - _atcmm1.size.Width) / 2, ((ScreenManager.WindowHeight - _atcmm1.size.Height) / 2) - 50);
            _bg_datcmm.Child.Add(_atcmm1);

            Rec _atcmm2 = new Rec(Brushes.WhiteSmoke, new Point(_atcmm1.point.X + 1, _atcmm1.point.Y + 1), new Size(198, 78), "__atcmm2", Manager.TypeGfx.Top, true);
            _bg_datcmm.Child.Add(_atcmm2);

            Txt _atcmm3 = new Txt(actorName + " " + CommonCode.TranslateText(100), Point.Empty, "__atcmm4", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Black);
            _atcmm3.point = new Point(_atcmm1.point.X + ((_atcmm1.size.Width - TextRenderer.MeasureText(_atcmm3.Text, _atcmm3.font).Width) / 2), _atcmm1.point.Y + 5);
            _bg_datcmm.Child.Add(_atcmm3);

            Bmp _btnok_datcmm = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(_atcmm1.point.X + 5, _atcmm1.point.Y + 30), "_btnok_datcmm", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 19));
            _btnok_datcmm.MouseOver += CommonCode.CursorHand_MouseMove;
            _btnok_datcmm.MouseOut += CursorDefault_MouseOut;
            _btnok_datcmm.EscapeGfxWhileMouseClic = true;
            _btnok_datcmm.MouseClic += _btnok_datcmm_MouseClic;
            _bg_datcmm.Child.Add(_btnok_datcmm);

            Txt _txtok_datcmm = new Txt(CommonCode.TranslateText(101), new Point(_atcmm1.point.X + 40, _atcmm1.point.Y + 33), "_txtok_datcmm", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Green);
            _bg_datcmm.Child.Add(_txtok_datcmm);

            Bmp _btnannuler_datcmm = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(_atcmm1.point.X + 105, _atcmm1.point.Y + 30), "_btnannuler_datcmm", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 19));
            _btnannuler_datcmm.tag = _bg_datcmm;        // association du parent pour le supprimer lors du click
            _btnannuler_datcmm.MouseOver += CursorDefault_MouseOut;
            _btnannuler_datcmm.MouseOut += CommonCode.CursorDefault_MouseOut;
            _btnannuler_datcmm.MouseClic += _btnannuler_datcmm_MouseClic;
            _btnannuler_datcmm.EscapeGfxWhileMouseClic = true;
            _bg_datcmm.Child.Add(_btnannuler_datcmm);

            Txt _txtannuler_datcmm = new Txt(CommonCode.TranslateText(102), new Point(_atcmm1.point.X + 145, _atcmm1.point.Y + 33), "_txtannuler_datcmm", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Red);
            _bg_datcmm.Child.Add(_txtannuler_datcmm);

            CheckBox _ignorerCB_datcmm = new CheckBox();
            _ignorerCB_datcmm.Checked = false;
            _ignorerCB_datcmm.Name = "ignorerCB";
            _ignorerCB_datcmm.Text = CommonCode.TranslateText(103);
            _ignorerCB_datcmm.Location = new Point(_atcmm1.point.X + 8, _atcmm1.point.Y + _atcmm1.size.Height - 22);
            _ignorerCB_datcmm.BackColor = Color.Transparent;
            _ignorerCB_datcmm.Click += _ignorerCB_datcmm_Click;
            Manager.manager.GfxCtrlList.Add(_ignorerCB_datcmm);
            Manager.manager.mainForm.Controls.Add(_ignorerCB_datcmm);

            ChallengeTo = actorName;
            annulerChallengeMeDlg = _bg_datcmm;
            annulerChallengeHimDlg = null;
            #endregion
        }
        public static void DrawAskingToDuelMeMenu(string actorName)
        {
            #region reception d'une demande de défie
            // menue pour celui qui a été demandé
            Bmp _bg_datcmm = new Bmp(@"gfx\general\obj\1\bg_tamise1.dat", new Point(0, 0), "_bg_datcmm", Manager.TypeGfx.Top, true, 1);
            Manager.manager.GfxTopList.Add(_bg_datcmm);

            Rec _atcmm1 = new Rec(Brushes.Black, Point.Empty, new Size(200, 80), "__atcmm1", Manager.TypeGfx.Top, true);
            _atcmm1.point = new Point((ScreenManager.WindowWidth - _atcmm1.size.Width) / 2, ((ScreenManager.WindowHeight - _atcmm1.size.Height) / 2) - 50);
            _bg_datcmm.Child.Add(_atcmm1);

            Rec _atcmm2 = new Rec(Brushes.WhiteSmoke, new Point(_atcmm1.point.X + 1, _atcmm1.point.Y + 1), new Size(198, 78), "__atcmm2", Manager.TypeGfx.Top, true);
            _bg_datcmm.Child.Add(_atcmm2);

            Txt _atcmm3 = new Txt(actorName + " " + CommonCode.TranslateText(100), Point.Empty, "__atcmm4", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Black);
            _atcmm3.point = new Point(_atcmm1.point.X + ((_atcmm1.size.Width - TextRenderer.MeasureText(_atcmm3.Text, _atcmm3.font).Width) / 2), _atcmm1.point.Y + 5);
            _bg_datcmm.Child.Add(_atcmm3);

            Bmp _btnok_datcmm = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(_atcmm1.point.X + 5, _atcmm1.point.Y + 30), "_btnok_datcmm", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 19));
            _btnok_datcmm.MouseOver += CommonCode.CursorHand_MouseMove;
            _btnok_datcmm.MouseOut += CursorDefault_MouseOut;
            _btnok_datcmm.EscapeGfxWhileMouseClic = true;
            _btnok_datcmm.MouseClic += _btnok_datcmm_MouseClic;
            _bg_datcmm.Child.Add(_btnok_datcmm);

            Txt _txtok_datcmm = new Txt(CommonCode.TranslateText(101), new Point(_atcmm1.point.X + 40, _atcmm1.point.Y + 33), "_txtok_datcmm", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Green);
            _bg_datcmm.Child.Add(_txtok_datcmm);

            Bmp _btnannuler_datcmm = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(_atcmm1.point.X + 105, _atcmm1.point.Y + 30), "_btnannuler_datcmm", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 19));
            _btnannuler_datcmm.tag = _bg_datcmm;        // association du parent pour le supprimer lors du click
            _btnannuler_datcmm.MouseOver += CursorDefault_MouseOut;
            _btnannuler_datcmm.MouseOut += CommonCode.CursorDefault_MouseOut;
            _btnannuler_datcmm.MouseClic += _btnannuler_datcmm_MouseClic;
            _btnannuler_datcmm.EscapeGfxWhileMouseClic = true;
            _bg_datcmm.Child.Add(_btnannuler_datcmm);

            Txt _txtannuler_datcmm = new Txt(CommonCode.TranslateText(102), new Point(_atcmm1.point.X + 145, _atcmm1.point.Y + 33), "_txtannuler_datcmm", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Red);
            _bg_datcmm.Child.Add(_txtannuler_datcmm);

            CheckBox _ignorerCB_datcmm = new CheckBox();
            _ignorerCB_datcmm.Checked = false;
            _ignorerCB_datcmm.Name = "ignorerCB";
            _ignorerCB_datcmm.Text = CommonCode.TranslateText(103);
            _ignorerCB_datcmm.Location = new Point(_atcmm1.point.X + 8, _atcmm1.point.Y + _atcmm1.size.Height - 22);
            _ignorerCB_datcmm.BackColor = Color.Transparent;
            _ignorerCB_datcmm.Click += _ignorerCB_datcmm_Click;
            Manager.manager.GfxCtrlList.Add(_ignorerCB_datcmm);
            Manager.manager.mainForm.Controls.Add(_ignorerCB_datcmm);

            ChallengeTo = actorName;
            annulerChallengeMeDlg = _bg_datcmm;
            annulerChallengeHimDlg = null;
            #endregion
        }
        static void _ignorerCB_datcmm_Click(object sender, EventArgs e)
        {
            #region
            if (((CheckBox)sender).Checked)
            {
                // envoie au serveur une demande d'ignorer ce joueur
                Network.SendMessage("cmd•IgnorePlayerChallenge•" + ChallengeTo, true);
                annulerChallengeMeDlg.visible = false;
                Manager.manager.GfxTopList.Remove(annulerChallengeMeDlg);
                annulerChallengeMeDlg = null;

                // supression du case a coché
                Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0].Visible = false;
                Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0] = null;
                Manager.manager.mainForm.Controls.Remove(Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0]);

                ChallengeTo = "";
            }
            #endregion
        }
        static void _btnok_datcmm_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            #region
            // envoie au serveur pour le lancement du défit
            Network.SendMessage("cmd•AcceptChallenge•" + ChallengeTo, true);
            #endregion
        }
        public static void DrawAskingToChallengeHimMenu(string challenged)
        {
            #region
            // menu pour celui qui demande
            Bmp _bg_datchm = new Bmp(@"gfx\general\obj\1\bg_tamise1.dat", new Point(0, 0), "_bg_datchm", Manager.TypeGfx.Top, true, 1);
            Manager.manager.GfxTopList.Add(_bg_datchm);

            Rec _datchm1 = new Rec(Brushes.Black, Point.Empty, new Size(200, 80), "__datchm1", Manager.TypeGfx.Top, true);
            _datchm1.point = new Point((ScreenManager.WindowWidth - _datchm1.size.Width) / 2, (ScreenManager.WindowHeight - _datchm1.size.Height) / 2 - 50);
            _bg_datchm.Child.Add(_datchm1);

            Rec _datchm2 = new Rec(Brushes.WhiteSmoke, new Point(_datchm1.point.X + 1, _datchm1.point.Y + 1), new Size(198, 78), "__datchm2", Manager.TypeGfx.Top, true);
            _bg_datchm.Child.Add(_datchm2);

            Txt _datchm3 = new Txt(challenged + " " + CommonCode.TranslateText(104), Point.Empty, "__datchm3", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Black);
            _datchm3.point = new Point(_datchm1.point.X + (_datchm1.size.Width - TextRenderer.MeasureText(_datchm3.Text, _datchm3.font).Width) / 2, _datchm1.point.Y + 10);
            _bg_datchm.Child.Add(_datchm3);

            Bmp _btnannuler_datchm = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "_btnannuler_datchm", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 19));
            _btnannuler_datchm.point = new Point(_datchm1.point.X + (_datchm1.size.Width - _btnannuler_datchm.rectangle.Size.Width) / 2, _datchm1.point.Y + 40);
            _btnannuler_datchm.tag = _bg_datchm;
            _btnannuler_datchm.MouseClic += _btnannuler_datchm_MouseClic;
            _bg_datchm.Child.Add(_btnannuler_datchm);

            Txt _annuler_datchm1 = new Txt(CommonCode.TranslateText(34), Point.Empty, "_annuler_datchm1", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Red);
            _annuler_datchm1.point = new Point(_datchm1.point.X + (_datchm1.size.Width - TextRenderer.MeasureText(_annuler_datchm1.Text, _annuler_datchm1.font).Width) / 2, _datchm1.point.Y + 42);
            _bg_datchm.Child.Add(_annuler_datchm1);
            ChallengeTo = challenged;
            annulerChallengeHimDlg = _bg_datchm;
            annulerChallengeMeDlg = null;
            #endregion
        }
        public static void _btnannuler_datchm_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            #region
            Bmp _ParentMenu = (bmp.tag as Bmp);
            _ParentMenu.visible = false;
            Manager.manager.GfxTopList.Remove(_ParentMenu);
            // supression du parent

            // envoie au serveur pour que le client efface sa fenetre de demande
            Network.SendMessage("cmd•CancelChallengeAsking•" + ChallengeTo, true);
            ChallengeTo = "";
            #endregion
        }
        public static void _btnannuler_datcmm_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            #region
            // supression du parent
            Bmp _ParentMenu = (bmp.tag as Bmp);
            _ParentMenu.visible = false;
            Manager.manager.GfxTopList.Remove(_ParentMenu);

            // supression du case a coché
            Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0].Visible = false;
            Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0] = null;
            Manager.manager.mainForm.Controls.Remove(Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0]);

            // envoie au serveur pour que le client efface sa fenetre de demande
            Network.SendMessage("cmd•CancelChallengeRespond•" + ChallengeTo, true);
            ChallengeTo = "";
            #endregion
        }
        static void contextMenuRecDefie2_MouseOver(Rec rec, MouseEventArgs e)
        {
            rec.brush = Brushes.GreenYellow;
        }
        static void contextMenuRecDefie2_MouseClic(Rec rec, MouseEventArgs e)
        {
            #region
            // envoie d'une requette pour defie
            DuelConfirmationRequestMessage duelConfirmationRequestMessage = new DuelConfirmationRequestMessage(rec.name.Substring(21, rec.name.Length - 21));
            duelConfirmationRequestMessage.Serialize();
            duelConfirmationRequestMessage.Send();

            ChallengeTo = rec.name.Substring(21, rec.name.Length - 21);
            rec.visible = false;
            /////////// supression du menu contextuel
            (rec.tag as Rec).Child.Clear();
            Manager.manager.GfxTopList.Remove((rec.tag as Rec));
            #endregion
        }
        static void contextMenuRecMP3_MouseOver(Rec rec, MouseEventArgs e)
        {
            rec.brush = Brushes.GreenYellow;
        }
        static void contextMenuRecMP3_MouseOut(Rec rec, MouseEventArgs e)
        {
            #region
            // mouseout sur menu contextuel / message privé
            CommonCode.CursorDefault_MouseOut(null, null);
            rec.brush = Brushes.White;
            #endregion
        }
        static void contextMenuRecMP3_MouseMove(Rec rec, MouseEventArgs e)
        {
            #region //survole sur menu contextuel / message privé
            CursorHand_MouseMove(null, null);
            #endregion
        }
        static void contextMenuRecMP3_MouseClic(Rec rec, MouseEventArgs e)
        {
            #region
            // click sur message privé sur le menu contextuel d'un joueur
            // passage au mode private
            HudHandle.ChannelState("P");

            // insertion du nom de l'user qui sera contacté par mp
            HudHandle.ChatTextBox.Text = rec.tag + @"#";
            HudHandle.ChatTextBox.Focus();
            HudHandle.ChatTextBox.SelectionStart = HudHandle.ChatTextBox.TextLength;

            Manager.manager.GfxTopList.Find(f => f.Name() == "__contextMenuRec1").Visible(false);
            Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "__contextMenuRec1");
            #endregion
        }
        static void contextMenuBattleFlag_MouseOut(Rec rec, MouseEventArgs e)
        {
            #region
            // mouseout sur menu contextuel / message privé
            CursorDefault_MouseOut(null, null);
            #endregion
        }
        static void contextMenuRecParent_MouseMove(Rec rec, MouseEventArgs e)
        {
            #region
            // survole menu contextuel
            CursorDefault_MouseOut(null, null);
            #endregion
        }
        static void contextMenuBattleFlag_MouseClic(Rec rec, MouseEventArgs e)
        {
            #region
            // click sur la ligne states des joueurs dans un combat
            // passage au mode private
            HudHandle.ChannelState("P");

            // insertion du nom de l'user qui sera contacté par mp
            HudHandle.ChatTextBox.Text = rec.tag + "#";
            HudHandle.ChatTextBox.Focus();
            HudHandle.ChatTextBox.SelectionStart = HudHandle.ChatTextBox.TextLength;

            Manager.manager.GfxTopList.Find(f => f.Name() == "__contextMenuBattleFlag").Visible(false);
            Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "__contextMenuBattleFlag");
            #endregion
        }
        public static void ibPlayers_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            #region survole sur un joueur
            // affichages des childes nom + ailles + village
            for (int cnt = 0; cnt < bmp.Child.Count(); cnt++)
            {
                if (bmp.Child[cnt].GetType() == typeof(Bmp))
                {
                    Bmp tempBmp = bmp.Child[cnt] as Bmp;
                    tempBmp.zindex = bmp.zindex;
                    tempBmp.visible = true;
                }
                else if (bmp.Child[cnt].GetType() == typeof(Txt))
                {
                    Txt tempTxt = bmp.Child[cnt] as Txt;
                    tempTxt.zindex = bmp.zindex;
                    tempTxt.visible = true;
                }
            }

            // si on ai en combat affichage des states
            if (Battle.state == Enums.battleState.state.started)
            {
                #region
                // pointeur pour le joueur dans la liste
                Actor piib = Battle.AllPlayersByOrder.Find(f => f.pseudo == (bmp.tag as Actor).pseudo);

                /*Rec __playerStatesInBattleRecParent = new Rec(Brushes.WhiteSmoke, Point.Empty, new Size(HudHandle.all_sorts.rectangle.Size.Width, HudHandle.all_sorts.rectangle.Size.Height), "__playerStatesInBattleRecParent", Manager.TypeGfx.Top, true);
                __playerStatesInBattleRecParent.point = new Point(HudHandle.all_sorts.point.X + 1, HudHandle.all_sorts.point.Y);
                __playerStatesInBattleRecParent.zindex = HudHandle.all_sorts.zindex + 1000;
                Manager.manager.GfxTopList.Add(__playerStatesInBattleRecParent);*/

                Bmp __playerStatesInBattleParent = new Bmp(@"gfx\general\obj\1\statesPlayer.dat", new Point(6, 6), "__playerStatesInBattleParent", Manager.TypeGfx.Top, true, 1);
                __playerStatesInBattleParent.point = new Point(HudHandle.all_sorts.point.X + 1, HudHandle.all_sorts.point.Y);
                __playerStatesInBattleParent.zindex = HudHandle.all_sorts.zindex + 1000;
                Manager.manager.GfxTopList.Add(__playerStatesInBattleParent);

                /*Bmp __PlayerStatesPC2Bmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(6, 6), "__PlayerStatesPC2Bmp", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 27));
                __playerStatesInBattleRecParent.Child.Add(__PlayerStatesPC2Bmp);*/

                Txt __PC2_ValueTxt = new Txt(piib.currentPc.ToString(), new Point(15, 3), "__PC2_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Blue);
                __playerStatesInBattleParent.Child.Add(__PC2_ValueTxt);

                /*Bmp __PlayerStatesPM2Bmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(50, 6), "__PlayerStatesPM2Bmp", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 28));
                __playerStatesInBattleRecParent.Child.Add(__PlayerStatesPM2Bmp);*/

                Txt __PM2_ValueTxt = new Txt(piib.currentPm.ToString(), new Point(67, 3), "__PM2_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Green);
                __playerStatesInBattleParent.Child.Add(__PM2_ValueTxt);

                //Bmp __PlayerStatesPEBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(100, 7), "__PlayerStatesPEBmp", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 29));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesPEBmp);

                Txt __PE_ValueTxt = new Txt(piib.pe.ToString(), new Point(115, 3), "__PE_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), CommonCode.spellAreaNotAllowedColor);
                __playerStatesInBattleParent.Child.Add(__PE_ValueTxt);

                //Bmp __PlayerStatesCDBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(150, 6), "__PlayerStatesCDBmp", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 30));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesCDBmp);

                Txt __CD_ValueTxt = new Txt(piib.cd.ToString(), new Point(150, 3), "__CD_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Red);
                __playerStatesInBattleParent.Child.Add(__CD_ValueTxt);

                //Bmp __PlayerStatesInvocBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(190, 6), "__PlayerStatesInvocBmp", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 31));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesInvocBmp);

                Txt __Invoc_ValueTxt = new Txt(piib.summons.ToString(), new Point(190, 3), "__Invoc_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), CommonCode.spellAreaNotAllowedColor);
                __playerStatesInBattleParent.Child.Add(__Invoc_ValueTxt);

                //Bmp __PlayerStatesIniBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(240, 6), "__PlayerStatesIniBmp", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 32));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesIniBmp);

                Txt __Ini_ValueTxt = new Txt(piib.initiative.ToString(), new Point(230, 3), "__Ini_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Brown);
                __playerStatesInBattleParent.Child.Add(__Ini_ValueTxt);


                Txt __Puissance_ValueTxt = new Txt((piib.power + piib.equipedPower).ToString(), new Point(282, 3), "__Puissance_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Brown);
                __playerStatesInBattleParent.Child.Add(__Puissance_ValueTxt);


                Txt __DomFix_ValueTxt = new Txt(piib.domFix.ToString(), new Point(326, 3), "__DomFix_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Brown);
                __playerStatesInBattleParent.Child.Add(__DomFix_ValueTxt);

                // stats element
                //doton
                //Bmp __PlayerStatesDotonBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(6, 20), "__PlayerStatesDotonBmp", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 33));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesDotonBmp);

                // valeur de l'element
                Txt __Doton_ValueTxt = new Txt((piib.doton + piib.equipedDoton).ToString(), new Point(18, 25), "__Doton_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(142, 91, 21)));
                __playerStatesInBattleParent.Child.Add(__Doton_ValueTxt);

                // calcule du niveau de l'element
                int tmpLabel = 0;
                if (CalculateElementLvl(piib.usingDoton) == 1)
                    tmpLabel = 82;
                else if (CalculateElementLvl(piib.usingDoton) == 2)
                    tmpLabel = 83;
                else if (CalculateElementLvl(piib.usingDoton) == 3)
                    tmpLabel = 84;
                else if (CalculateElementLvl(piib.usingDoton) == 4)
                    tmpLabel = 85;
                else if (CalculateElementLvl(piib.usingDoton) == 5)
                    tmpLabel = 86;
                else if (CalculateElementLvl(piib.usingDoton) == 6)
                    tmpLabel = 87;

                // valeur du niveau
                Txt __Doton_Lvl_Txt = new Txt(TranslateText(tmpLabel), new Point(43, 25), "__Doton_Lvl_Txt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(142, 91, 21)));
                __playerStatesInBattleParent.Child.Add(__Doton_Lvl_Txt);
                //////////////////////////////////////////////
                //katon
                //Bmp __PlayerStatesKatonBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(100, 20), "__PlayerStatesKatonBmp", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 34));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesKatonBmp);

                // valeur de l'element
                Txt __Katon_ValueTxt = new Txt((piib.katon + piib.equipedKaton).ToString(), new Point(88, 25), "__Katon_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(198, 0, 0)));
                __playerStatesInBattleParent.Child.Add(__Katon_ValueTxt);

                // calcule du niveau de l'element
                if (CalculateElementLvl(piib.usingKaton) == 1)
                    tmpLabel = 82;
                else if (CalculateElementLvl(piib.usingKaton) == 2)
                    tmpLabel = 83;
                else if (CalculateElementLvl(piib.usingKaton) == 3)
                    tmpLabel = 84;
                else if (CalculateElementLvl(piib.usingKaton) == 4)
                    tmpLabel = 85;
                else if (CalculateElementLvl(piib.usingKaton) == 5)
                    tmpLabel = 86;
                else if (CalculateElementLvl(piib.usingKaton) == 6)
                    tmpLabel = 87;

                // valeur du niveau
                Txt __Katon_Lvl_Txt = new Txt(TranslateText(tmpLabel), new Point(113, 25), "__Katon_Lvl_Txt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(198, 0, 0)));
                __playerStatesInBattleParent.Child.Add(__Katon_Lvl_Txt);
                ///////////////////////////////////////////////////////
                //futon
                //Bmp __PlayerStatesFutonBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(200, 20), "__PlayerStatesFutonBmp", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 35));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesFutonBmp);

                // valeur de l'element
                Txt __Futon_ValueTxt = new Txt((piib.futon + piib.equipedFuton).ToString(), new Point(158, 25), "__Futon_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(0, 197, 125)));
                __playerStatesInBattleParent.Child.Add(__Futon_ValueTxt);

                // calcule du niveau de l'element
                if (CalculateElementLvl(piib.usingFuton) == 1)
                    tmpLabel = 82;
                else if (CalculateElementLvl(piib.usingFuton) == 2)
                    tmpLabel = 83;
                else if (CalculateElementLvl(piib.usingFuton) == 3)
                    tmpLabel = 84;
                else if (CalculateElementLvl(piib.usingFuton) == 4)
                    tmpLabel = 85;
                else if (CalculateElementLvl(piib.usingFuton) == 5)
                    tmpLabel = 86;
                else if (CalculateElementLvl(piib.usingFuton) == 6)
                    tmpLabel = 87;

                // valeur du niveau
                Txt __Futon_Lvl_Txt = new Txt(TranslateText(tmpLabel), new Point(183, 25), "__Futon_Lvl_Txt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(0, 197, 125)));
                __playerStatesInBattleParent.Child.Add(__Futon_Lvl_Txt);
                //////////////////////////////////////////////////////////
                //raiton
                //Bmp __PlayerStatesRaitonBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(6, 40), "__PlayerStatesRaitonBmp", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 36));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesRaitonBmp);

                // valeur de l'element
                Txt __Raiton_ValueTxt = new Txt((piib.raiton + piib.equipedRaiton).ToString(), new Point(228, 25), "__Raiton_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(215, 203, 0)));
                __playerStatesInBattleParent.Child.Add(__Raiton_ValueTxt);

                // calcule du niveau de l'element
                tmpLabel = 0;
                if (CommonCode.CalculateElementLvl(piib.usingRaiton) == 1)
                    tmpLabel = 82;
                else if (CommonCode.CalculateElementLvl(piib.usingRaiton) == 2)
                    tmpLabel = 83;
                else if (CommonCode.CalculateElementLvl(piib.usingRaiton) == 3)
                    tmpLabel = 84;
                else if (CommonCode.CalculateElementLvl(piib.usingRaiton) == 4)
                    tmpLabel = 85;
                else if (CommonCode.CalculateElementLvl(piib.usingRaiton) == 5)
                    tmpLabel = 86;
                else if (CommonCode.CalculateElementLvl(piib.usingRaiton) == 6)
                    tmpLabel = 87;

                // valeur du niveau
                Txt __Raiton_Lvl_Txt = new Txt(TranslateText(tmpLabel), new Point(253, 25), "__Raiton_Lvl_Txt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(215, 203, 0)));
                __playerStatesInBattleParent.Child.Add(__Raiton_Lvl_Txt);
                //////////////////////////////////////////////////////
                //suiton
                //Bmp __PlayerStatesSuitonBmp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(100, 40), "__PlayerStatesSuitonBmp", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 37));
                //__playerStatesInBattleRecParent.Child.Add(__PlayerStatesSuitonBmp);

                // valeur de l'element
                Txt __Suiton_ValueTxt = new Txt((piib.suiton + piib.equipedSuiton).ToString(), new Point(298, 25), "__Suiton_ValueTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(12, 133, 255)));
                __playerStatesInBattleParent.Child.Add(__Suiton_ValueTxt);

                // calcule du niveau de l'element
                if (CalculateElementLvl(piib.usingSuiton) == 1)
                    tmpLabel = 82;
                else if (CalculateElementLvl(piib.usingSuiton) == 2)
                    tmpLabel = 83;
                else if (CalculateElementLvl(piib.usingSuiton) == 3)
                    tmpLabel = 84;
                else if (CalculateElementLvl(piib.usingSuiton) == 4)
                    tmpLabel = 85;
                else if (CalculateElementLvl(piib.usingSuiton) == 5)
                    tmpLabel = 86;
                else if (CalculateElementLvl(piib.usingSuiton) == 6)
                    tmpLabel = 87;

                // valeur du niveau
                Txt __Suiton_Lvl_Txt = new Txt(TranslateText(tmpLabel), new Point(323, 25), "__Suiton_Lvl_Txt", Manager.TypeGfx.Top, true, new Font("Verdana", 7), new SolidBrush(Color.FromArgb(12, 133, 255)));
                __playerStatesInBattleParent.Child.Add(__Suiton_Lvl_Txt);

                ////////////////////////
                // affichage des pdv
                //Bmp __pdv_coeur = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(186, 46), "__pdv_coeur", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 38));
                //__playerStatesInBattleRecParent.Child.Add(__pdv_coeur);

                Rec __pdv_rec1 = new Rec(Brushes.Black, new Point(12, 44), new Size(100, 16), "__pdv_rec1", Manager.TypeGfx.Top, true);
                __playerStatesInBattleParent.Child.Add(__pdv_rec1);

                Rec __pdv_rec2 = new Rec(Brushes.White, new Point(13, 45), new Size(98, 14), "__pdv_rec2", Manager.TypeGfx.Top, true);
                __playerStatesInBattleParent.Child.Add(__pdv_rec2);

                Rec __pdv_percent = new Rec(Brushes.Red, new Point(14, 46), new Size(0, 12), "__pdv_percent", Manager.TypeGfx.Top, true);
                __playerStatesInBattleParent.Child.Add(__pdv_percent);

                // modification de la gauge pdv selon le %
                int percent = (piib.currentHealth * 100) / piib.maxHealth;
                __pdv_percent.size.Width = (96 * percent) / 100;

                Txt __pdv_value_percent = new Txt(piib.currentHealth + " / " + piib.maxHealth + " [" + percent + "%]", new Point(0, 47), "__pdv_value_percent", Manager.TypeGfx.Top, true, new Font("verdana", 6), Brushes.Black);
                __pdv_value_percent.point.X = 14 + (96 - TextRenderer.MeasureText(__pdv_value_percent.Text, __pdv_value_percent.font).Width) / 2;
                __playerStatesInBattleParent.Child.Add(__pdv_value_percent);
                //////////////////////////////////////////////////
                // bobo doton
                //Bmp resiDoton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(10, 60), "__resiDoton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 40));
                //__playerStatesInBattleRecParent.Child.Add(resiDoton);

                Txt resiDotonTxt = new Txt(piib.resiDotonPercent + "%", new Point(130, 45), "__resiDotonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(resiDotonTxt);

                // bobo katon
                //Bmp resiKaton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(70, 60), "resiKaton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 41));
                //__playerStatesInBattleRecParent.Child.Add(resiKaton);

                Txt resiKatonTxt = new Txt(piib.resiKatonPercent + "%", new Point(180, 45), "__resiKatonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(resiKatonTxt);

                // bobo futon
                //Bmp resiFuton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(130, 60), "resiFuton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 42));
                //__playerStatesInBattleRecParent.Child.Add(resiFuton);

                Txt resiFutonTxt = new Txt(piib.resiFutonPercent + "%", new Point(225, 45), "__resiFutonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(resiFutonTxt);

                // bobo raiton
                //Bmp resiRaiton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(190, 60), "resiRaiton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 43));
                //__playerStatesInBattleRecParent.Child.Add(resiRaiton);

                Txt resiRaitonTxt = new Txt(piib.resiRaitonPercent + "%", new Point(275, 45), "__resiRaitonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(resiRaitonTxt);

                // bobo suiton
                //Bmp resiSuiton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(250, 60), "resiSuiton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 44));
                //__playerStatesInBattleRecParent.Child.Add(resiSuiton);

                Txt resiSuitonTxt = new Txt(piib.resiSuitonPercent + "%", new Point(325, 45), "__resiSuitonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(resiSuitonTxt);

                // esquive PC
                //Bmp __esquivePC = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(10, 80), "__esquivePC", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 45));
                //__playerStatesInBattleRecParent.Child.Add(__esquivePC);

                Txt __esquivePC_Txt = new Txt(piib.dodgePc.ToString(), new Point(20, 67), "__esquivePC_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__esquivePC_Txt);

                // esquive PM
                //Bmp __esquivePM = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(10, 95), "__esquivePM", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 46));
                //__playerStatesInBattleRecParent.Child.Add(__esquivePM);

                Txt __esquivePM_Txt = new Txt(piib.dodgePm.ToString(), new Point(85, 67), "__esquivePM_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__esquivePM_Txt);

                // esquive PE
                //Bmp __esquivePE = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(70, 80), "__esquivePE", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 47));
                //__playerStatesInBattleRecParent.Child.Add(__esquivePE);

                Txt __esquivePE_Txt = new Txt(piib.dodgePe.ToString(), new Point(145, 67), "__esquivePE_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__esquivePE_Txt);

                // esquive CD
                //Bmp __esquiveCD = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(70, 95), "__esquiveCD", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 48));
                //__playerStatesInBattleRecParent.Child.Add(__esquiveCD);

                Txt __esquiveCD_Txt = new Txt(piib.dodgeCd.ToString(), new Point(200, 67), "__esquiveCD_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__esquiveCD_Txt);

                ////////////////////////////////////////////////////////////////////////////////
                // retrait PC
                //Bmp __retraitPC = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(140, 80), "__retraitPC", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 49));
                //__playerStatesInBattleRecParent.Child.Add(__retraitPC);

                Txt __retraitPC_Txt = new Txt(piib.removePc.ToString(), new Point(20, 90), "__retraitPC_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__retraitPC_Txt);

                // retrait PM
                //Bmp __retraitPM = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(140, 95), "__retraitPM", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 50));
                //__playerStatesInBattleRecParent.Child.Add(__retraitPM);

                Txt __retraitPM_Txt = new Txt(piib.removePm.ToString(), new Point(85, 90), "__retraitPM_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__retraitPM_Txt);

                // retrait PE
                //Bmp __retraitPE = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(200, 80), "__retraitPE", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 51));
                //__playerStatesInBattleRecParent.Child.Add(__retraitPE);

                Txt __retraitPE_Txt = new Txt(piib.removePe.ToString(), new Point(145, 90), "__retrait_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__retraitPE_Txt);

                // retrait CD
                //Bmp __retraitCD = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(200, 95), "__retraitCD", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 52));
                //__playerStatesInBattleRecParent.Child.Add(__retraitCD);

                Txt __retraitCD_Txt = new Txt(piib.removeCd.ToString(), new Point(200, 90), "__retraitCD_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__retraitCD_Txt);

                // evasion
                //Bmp __evasion = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(260, 80), "__evasion", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 53));
                //__playerStatesInBattleRecParent.Child.Add(__evasion);

                Txt __evasion_Txt = new Txt(piib.escape.ToString(), new Point(264, 67), "__evasion_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__evasion_Txt);

                // blockage
                //Bmp __blockage = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(260, 95), "__blockage", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 54));
                //__playerStatesInBattleRecParent.Child.Add(__blockage);

                Txt __blockage_Txt = new Txt(piib.blocage.ToString(), new Point(316, 67), "__blockage_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
                __playerStatesInBattleParent.Child.Add(__blockage_Txt);
                #endregion
            }
            #endregion
        }
        public static void ibPlayers_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            #region
            for (int cnt = 0; cnt < bmp.Child.Count(); cnt++)
            {
                if (bmp.Child[cnt].GetType() == typeof(Bmp))
                {
                    Bmp tempBmp = bmp.Child[cnt] as Bmp;
                    tempBmp.visible = false;
                }
                else if (bmp.Child[cnt].GetType() == typeof(Txt))
                {
                    Txt tempTxt = bmp.Child[cnt] as Txt;
                    tempTxt.visible = false;
                }
            }

            if (Battle.state == Enums.battleState.state.started)
            {
                // si le joueur est en combat, effacer le menue qui affiche les states des joueurs
                //List<IGfx> __playerStatesInBattleParent = Manager.manager.GfxTopList.FindAll(f => f.GetType() == typeof(Bmp) && (f as Bmp).name == "__playerStatesInBattleRecParent");
                List<IGfx> __playerStatesInBattleParent = Manager.manager.GfxTopList.FindAll(f => f.Name() == "__playerStatesInBattleParent");
                if (__playerStatesInBattleParent != null)
                    for (int cnt = __playerStatesInBattleParent.Count; cnt > 0; cnt--)
                        DisposeIGfxAndChild(__playerStatesInBattleParent[cnt - 1]);
            }
            #endregion
        }
        public static void ibPlayers_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            #region clic sur un personnage
            if (Battle.state == Enums.battleState.state.idle)
                CommonCode.DrawPlayerContextMenu(bmp, e);
            else
            {
                if(Battle.currentCursor != "")
                {
                    // si le joueur avais un sort a lancé
                    Network.SendMessage("cmd•spellTuiles•" + Battle.infos_sorts.sortID + "•" + (bmp.tag as Actor).realPosition.X + "•" + (bmp.tag as Actor).realPosition.Y, true);
                    // changement du curseur a son état initiale
                    CursorHand_MouseMove(null, null);

                    // effacement des tuiles de selection
                    Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles");
                    // effacement de tous les images du sort rasen shuriken
                    Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles_child");

                    // supression de certains element dépondant des sorts
                    if (Battle.infos_sorts.sortID == 3)
                    {
                        IGfx __clone_jutsu = Manager.manager.GfxObjList.Find(f => f.Name() == "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto");
                        if (__clone_jutsu == null)
                            MessageBox.Show("error2");
                        else
                        {
                            (__clone_jutsu as Bmp).visible = false;
                            Manager.manager.GfxObjList.Remove(__clone_jutsu);
                        }
                    }

                    // reinitialisation du stat du sort en instance
                    Battle.infos_sorts = null;
                }
                Battle.currentCursor = "";
                CursorDefault_MouseOut(null, null);
                Bmp __spellCursor = (Bmp)Manager.manager.GfxTopList.Find(f => f.GetType() == typeof(Bmp) && ((Bmp)f).name == "__spellCursor");
                if (__spellCursor == null)
                    return;
                __spellCursor.visible = false;
                Manager.manager.GfxTopList.Remove(__spellCursor);
            }
            #endregion
        }
        public static void RefreshPlayersDataInBattle(string[] data)
        {
            #region
            // data[2] = sideA stats,   data[3] = sideB stats
            // pseudo#classe#village#MaskColors#level#rang#CurrentPdv#TotalPdv#doton#katon#futon#raiton#suiton#usingDoton#usingKaton#usingFuton#usingRaiton#usingSuiton#equipedDoton#equipedKaton#equipedFuton#equipedRaiton#suitonEquiped#pc#pm#pe#cd#invoc#initiative#esquivePC#esquivePM#esquivePE#esquiveCD#retraitPC#retraitPM#retraitPE#retraitCD#evasion#blocage#puissance#puissanceEquiped#sorts   séparé par | entre les joueurs
            
            // mise a jours des données des joueurs
            string[] sideAPlayersInfo = data[2].Split('|');
            foreach (string t in sideAPlayersInfo)
            {
                string actorName = sideAPlayersInfo[0];
                Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), sideAPlayersInfo[1]);
                Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), sideAPlayersInfo[2]);
                string maskColors = sideAPlayersInfo[3];
                int level = int.Parse(sideAPlayersInfo[4]);
                Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), sideAPlayersInfo[5]);
                int currentHealth = int.Parse(sideAPlayersInfo[6]);
                int maxHealth = int.Parse(sideAPlayersInfo[7]);
                int doton = int.Parse(sideAPlayersInfo[8]);
                int katon = int.Parse(sideAPlayersInfo[9]);
                int futon = int.Parse(sideAPlayersInfo[10]);
                int raiton = int.Parse(sideAPlayersInfo[11]);
                int suiton = int.Parse(sideAPlayersInfo[12]);
                int usingDoton = int.Parse(sideAPlayersInfo[13]);
                int usingKaton = int.Parse(sideAPlayersInfo[14]);
                int usingFuton = int.Parse(sideAPlayersInfo[15]);
                int usingRaiton = int.Parse(sideAPlayersInfo[16]);
                int usingSuiton = int.Parse(sideAPlayersInfo[17]);
                int equipedDoton = int.Parse(sideAPlayersInfo[18]);
                int equipedKaton = int.Parse(sideAPlayersInfo[19]);
                int equipedFuton = int.Parse(sideAPlayersInfo[20]);
                int equipedRaiton = int.Parse(sideAPlayersInfo[21]);
                int equipedSuiton = int.Parse(sideAPlayersInfo[22]);
                int originalPc = int.Parse(sideAPlayersInfo[23]);
                int originalPm = int.Parse(sideAPlayersInfo[24]);
                int pe = int.Parse(sideAPlayersInfo[25]);
                int cd = int.Parse(sideAPlayersInfo[26]);
                int summons = int.Parse(sideAPlayersInfo[27]);
                int initiative = int.Parse(sideAPlayersInfo[28]);
                int dodgePc = int.Parse(sideAPlayersInfo[29]);
                int dodgePm = int.Parse(sideAPlayersInfo[30]);
                int dodgePe = int.Parse(sideAPlayersInfo[31]);
                int dodgeCd = int.Parse(sideAPlayersInfo[32]);
                int removePc = int.Parse(sideAPlayersInfo[33]);
                int removePm = int.Parse(sideAPlayersInfo[34]);
                int removePe = int.Parse(sideAPlayersInfo[35]);
                int removeCd = int.Parse(sideAPlayersInfo[36]);
                int escape = int.Parse(sideAPlayersInfo[37]);
                int blocage = int.Parse(sideAPlayersInfo[38]);
                int power = int.Parse(sideAPlayersInfo[39]);
                int equipedPower = int.Parse(sideAPlayersInfo[40]);
                string[] spellsString = sideAPlayersInfo[41].Split('/');

                Actor piibt1 = Battle.SideA.Find(f => f.pseudo == actorName);
                piibt1.className = className;
                piibt1.hiddenVillage = hiddenVillage;
                piibt1.maskColorString = maskColors;
                piibt1.level = level;
                piibt1.officialRang = officialRang;
                piibt1.currentHealth = currentHealth;
                piibt1.maxHealth = maxHealth;
                piibt1.doton = doton;
                piibt1.katon = katon;
                piibt1.futon = futon;
                piibt1.raiton = raiton;
                piibt1.suiton = suiton;
                piibt1.usingDoton = usingDoton;
                piibt1.usingKaton = usingKaton;
                piibt1.usingFuton = usingFuton;
                piibt1.usingRaiton = usingRaiton;
                piibt1.usingSuiton = usingSuiton;
                piibt1.equipedDoton = equipedDoton;
                piibt1.equipedKaton = equipedKaton;
                piibt1.equipedFuton = equipedFuton;
                piibt1.equipedRaiton = equipedRaiton;
                piibt1.equipedSuiton = equipedSuiton;
                piibt1.originalPc = originalPc;           // contiens le total fix des pc
                piibt1.currentPc = originalPc;          // contiens les pc courante, le reliqua apres utilisation
                piibt1.originalPm = originalPm;           // ...
                piibt1.currentPm = originalPm;          // ...
                piibt1.pe = pe;
                piibt1.cd = cd;
                piibt1.summons = summons;
                piibt1.initiative = initiative;
                piibt1.dodgePc = dodgePc;
                piibt1.dodgePm = dodgePm;
                piibt1.dodgePe = dodgePe;
                piibt1.dodgeCd = dodgeCd;
                piibt1.removePc = removePc;
                piibt1.removePm = removePm;
                piibt1.removePe = removePe;
                piibt1.removeCd = removeCd;
                piibt1.escape = escape;
                piibt1.blocage = blocage;
                piibt1.power = power;
                piibt1.equipedPower = equipedPower;

                if (spellsString.Length > 0)
                {
                    for (int cnt2 = 0; cnt2 < spellsString.Length; cnt2++)
                    {
                        Actor.SpellsInformations _info_sorts = new Actor.SpellsInformations();
                        _info_sorts.sortID = Convert.ToInt32(spellsString[cnt2].Split(':')[0]);
                        _info_sorts.emplacement = Convert.ToInt32(spellsString[cnt2].Split(':')[1]);
                        _info_sorts.level = Convert.ToInt32(spellsString[cnt2].Split(':')[2]);
                        _info_sorts.colorSort = Convert.ToInt32(spellsString[cnt2].Split(':')[3]);
                        piibt1.spells.Add(_info_sorts);
                    }
                }
            }
            /// team 2
            string[] sideBPlayersInfo = data[3].Split('|');
            foreach (string t in sideBPlayersInfo)
            {
                string actorName = sideBPlayersInfo[0];
                Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), sideBPlayersInfo[1]);
                Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), sideBPlayersInfo[2]);
                string maskColors = sideBPlayersInfo[3];
                int level = int.Parse(sideBPlayersInfo[4]);
                Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), sideBPlayersInfo[5]);
                int currentHealth = int.Parse(sideBPlayersInfo[6]);
                int maxHealth = int.Parse(sideBPlayersInfo[7]);
                int doton = int.Parse(sideBPlayersInfo[8]);
                int katon = int.Parse(sideBPlayersInfo[9]);
                int futon = int.Parse(sideBPlayersInfo[10]);
                int raiton = int.Parse(sideBPlayersInfo[11]);
                int suiton = int.Parse(sideBPlayersInfo[12]);
                int usingDoton = int.Parse(sideBPlayersInfo[13]);
                int usingKaton = int.Parse(sideBPlayersInfo[14]);
                int usingFuton = int.Parse(sideBPlayersInfo[15]);
                int usingRaiton = int.Parse(sideBPlayersInfo[16]);
                int usingSuiton = int.Parse(sideBPlayersInfo[17]);
                int equipedDoton = int.Parse(sideBPlayersInfo[18]);
                int equipedKaton = int.Parse(sideBPlayersInfo[19]);
                int equipedFuton = int.Parse(sideBPlayersInfo[20]);
                int equipedRaiton = int.Parse(sideBPlayersInfo[21]);
                int equipedSuiton = int.Parse(sideBPlayersInfo[22]);
                int originalPc = int.Parse(sideBPlayersInfo[23]);
                int originalPm = int.Parse(sideBPlayersInfo[24]);
                int pe = int.Parse(sideBPlayersInfo[25]);
                int cd = int.Parse(sideBPlayersInfo[26]);
                int summons = int.Parse(sideBPlayersInfo[27]);
                int initiative = int.Parse(sideBPlayersInfo[28]);
                int dodgePc = int.Parse(sideBPlayersInfo[29]);
                int dodgePm = int.Parse(sideBPlayersInfo[30]);
                int dodgePe = int.Parse(sideBPlayersInfo[31]);
                int dodgeCd = int.Parse(sideBPlayersInfo[32]);
                int removePc = int.Parse(sideBPlayersInfo[33]);
                int removePm = int.Parse(sideBPlayersInfo[34]);
                int removePe = int.Parse(sideBPlayersInfo[35]);
                int removeCd = int.Parse(sideBPlayersInfo[36]);
                int escape = int.Parse(sideBPlayersInfo[37]);
                int blocage = int.Parse(sideBPlayersInfo[38]);
                int power = int.Parse(sideBPlayersInfo[39]);
                int equipedPower = int.Parse(sideBPlayersInfo[40]);
                string[] spellsString = sideBPlayersInfo[41].Split('/');

                Actor piibt2 = Battle.SideB.Find(f => f.pseudo == actorName);
                piibt2.className = className;
                piibt2.hiddenVillage = hiddenVillage;
                piibt2.maskColorString = maskColors;
                piibt2.level = level;
                piibt2.officialRang = officialRang;
                piibt2.currentHealth = currentHealth;
                piibt2.maxHealth = maxHealth;
                piibt2.doton = doton;
                piibt2.katon = katon;
                piibt2.futon = futon;
                piibt2.raiton = raiton;
                piibt2.suiton = suiton;
                piibt2.usingDoton = usingDoton;
                piibt2.usingKaton = usingKaton;
                piibt2.usingFuton = usingFuton;
                piibt2.usingRaiton = usingRaiton;
                piibt2.usingSuiton = usingSuiton;
                piibt2.equipedDoton = equipedDoton;
                piibt2.equipedKaton = equipedKaton;
                piibt2.equipedFuton = equipedFuton;
                piibt2.equipedRaiton = equipedRaiton;
                piibt2.equipedSuiton = equipedSuiton;
                piibt2.originalPc = originalPc;           // contiens le total fix des pc
                piibt2.currentPc = originalPc;          // contiens les pc courante, le reliqua apres utilisation
                piibt2.originalPm = originalPm;           // ...
                piibt2.currentPm = originalPm;          // ...
                piibt2.pe = pe;
                piibt2.cd = cd;
                piibt2.summons = summons;
                piibt2.initiative = initiative;
                piibt2.dodgePc = dodgePc;
                piibt2.dodgePm = dodgePm;
                piibt2.dodgePe = dodgePe;
                piibt2.dodgeCd = dodgeCd;
                piibt2.removePc = removePc;
                piibt2.removePm = removePm;
                piibt2.removePe = removePe;
                piibt2.removeCd = removeCd;
                piibt2.escape = escape;
                piibt2.blocage = blocage;
                piibt2.power = power;
                piibt2.equipedPower = equipedPower;

                if (spellsString.Length > 0)
                {
                    foreach (string t1 in spellsString)
                    {
                        Actor.SpellsInformations _info_sorts = new Actor.SpellsInformations();
                        _info_sorts.sortID = Convert.ToInt32(t1.Split(':')[0]);
                        _info_sorts.emplacement = Convert.ToInt32(t1.Split(':')[1]);
                        _info_sorts.level = Convert.ToInt32(t1.Split(':')[2]);
                        _info_sorts.colorSort = Convert.ToInt32(t1.Split(':')[3]);
                        piibt2.spells.Add(_info_sorts);
                    }
                }
            }
            #endregion
        }
        public static void ReorderAvatarPlayers(string leader)
        {
            #region
            // reorganisation des joueurs selon l'initiative
            // determination de celui qui a le plus de l'initiative du sideA
            PiibSortByInitiative _sort = new PiibSortByInitiative();
            Battle.SideA.Sort(_sort);
            Battle.SideB.Sort(_sort);

            // vidage de la liste des joueur Battle.AllPlayersByOrder a cause d'un problème inconnu qui crée des doublons du meme compte
            // peux etre parce qu'il y à plusieurs instance du meme client sur la meme machine
            Battle.AllPlayersByOrder.Clear();
            // reorganization de tous les joueurs selon l'initiative et basculation entre les team
            int sideACnt = 0;
            int sideBCnt = 0;
            if (leader == "")
            {
                if (Battle.SideA[0].initiative > Battle.SideB[0].initiative)
                {
                    sideACnt = 1;
                    Battle.AllPlayersByOrder.Add(Battle.SideA[0]);
                }
                else if (Battle.SideA[0].initiative < Battle.SideB[0].initiative)
                {
                    sideBCnt = 1;
                    Battle.AllPlayersByOrder.Add(Battle.SideB[0]);
                }
            }
            else
            {
                // les 2 leaders ont la meme ini, le serveur a choisie un leader aléatoirement
                // recherche du leader depuis la list des teams
                Actor piib = Battle.SideA.Find(f => f.pseudo == leader);
                if (piib != null)
                {
                    sideACnt = 1;
                    Battle.AllPlayersByOrder.Add(Battle.SideA[0]);
                }
                else
                {
                    sideBCnt = 1;
                    Battle.AllPlayersByOrder.Add(Battle.SideB[0]);
                }
            }

            // insertion des joueurs dans la liste Battle.AllPlayersByOrder
            while (sideACnt < Battle.SideA.Count || sideBCnt < Battle.SideB.Count)
            {
                if (Battle.AllPlayersByOrder.Last().teamSide == Enums.Team.Side.A)
                {
                    if (Battle.SideB.Count >= sideBCnt)
                    {
                        Battle.AllPlayersByOrder.Add(Battle.SideB[sideBCnt]);
                        sideBCnt++;
                    }
                }
                else if (Battle.AllPlayersByOrder.Last().teamSide == Enums.Team.Side.B)
                {
                    if (Battle.SideA.Count >= sideACnt)
                    {
                        Battle.AllPlayersByOrder.Add(Battle.SideA[sideACnt]);
                        sideACnt++;
                    }
                }
            }
            Battle.TimelineRec = new Rec(Brushes.Transparent, new Point(0, (ScreenManager.TilesHeight * 30) - 88), new Size(0, 88), "TimelineRec", Manager.TypeGfx.Top, true);
            Manager.manager.GfxTopList.Add(Battle.TimelineRec);
            Battle._Selected_Player.zindex = 0;
            Battle._Selected_Player.visible = true;
            Battle._bar_to_move.zindex = 0;
            Battle._bar_to_move.visible = true;
            Battle.TimelineRec.Child.Add(Battle._Selected_Player);
            Battle.TimelineRec.Child.Add(Battle._bar_to_move);
            Battle._bar_to_move.MouseMove += CursorHand_MouseMove;
            Battle._bar_to_move.MouseDown += Battle._bar_to_move_MouseDown;
            Battle._bar_to_move.MouseOut += CursorDefault_MouseOut;
            Battle.RefreshTimeLine();
            #endregion
        }
        public static void DrawSpellTiles()
        {
            // affichage des cases affecté par le sort
            Point playerPosition = Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).realPosition;
            
            if (Battle.infos_sorts.sortID == 0)
            {
                #region rasengan
                bool objFoundInTheRight = false;                // pour determiner si un obstacle se trouve entre le joueur et la case du sort pour la desactivé
                bool objFoundInTheLeft = false;                 // pour determiner si un obstacle se trouve entre le joueur et la case du sort pour la desactivé
                bool objFoundInTheDown = false;                 // pour determiner si un obstacle se trouve entre le joueur et la case du sort pour la desactivé
                bool objFoundInTheUp = false;                   // pour determiner si un obstacle se trouve entre le joueur et la case du sort pour la desactivé
                short allowFirstOpponentBlockingInLeft = 0;     // ne pas bloque la ligne de vue de gauche sur le 1er adversaire rencontré puisque le systeme le considere comme obstacle alors qu'il dois avoir le focus sur sa case, pas comme les obstacle du map
                short allowFirstOpponentBlockingInRight = 0;    // ne pas bloque la ligne de vue de droite le 1er adversaire rencontré puisque le systeme le considere comme obstacle alors qu'il dois avoir le focus sur sa case, pas comme les obstacle du map
                short allowFirstOpponentBlockingInUp = 0;       // ne pas bloque la ligne de vue du haut le 1er adversaire rencontré puisque le systeme le considere comme obstacle alors qu'il dois avoir le focus sur sa case, pas comme les obstacle du map
                short allowFirstOpponentBlockingInDown = 0;     // ne pas bloque la ligne de vue du bas le 1er adversaire rencontré puisque le systeme le considere comme obstacle alors qu'il dois avoir le focus sur sa case, pas comme les obstacle du map

                // a ajouter apres "isbl[Battle.infos_sorts.lvl - 1].etendu" + x d'ou x = la portée fix ajouté
                for (int cnt = 0; cnt < spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu + 2; cnt++)
                {
                    // check si la case à droite qu'on veux dessiner ne sort pas des bord
                    /////////////// right //////////////
                    if (playerPosition.X + 1 + cnt < ScreenManager.TilesWidth && playerPosition.Y < ScreenManager.TilesHeight)
                    {
                        // affichage des cases du sorts a droite
                        
                        Rec __spellTuiles_right = new Rec(!objFoundInTheRight ? spellAreaAllowedColor : spellAreaNotAllowedColor, new Point((playerPosition.X + 1 + cnt) * 30 + 1, ((playerPosition.Y) * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                        __spellTuiles_right.MouseClic += __spellTuiles_MouseClic;
                        __spellTuiles_right.MouseOut += __spellTuiles_MouseOut;
                        __spellTuiles_right.MouseMove += __spellTuiles_MouseMove;

                        // check s'il y à un obstacle non joueur
                        if (CurMapFreeCellToWalk(new Point((playerPosition.X + 1 + cnt) * 30, playerPosition.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X + 1 + cnt && f.realPosition.Y == playerPosition.Y))
                            if (cnt > 1)
                                Manager.manager.GfxBgrList.Add(__spellTuiles_right);

                        // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                        __spellTuiles_right.tag = !objFoundInTheRight ? spellAreaAllowedColor : spellAreaNotAllowedColor;

                        // check si un joueur se trouve sur la pos à droite pour bloquer les prochain tuiles
                        List<Actor> ChkObjFoundInTheRight = Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == (playerPosition.X + 1 + cnt) && f.realPosition.Y == playerPosition.Y);
                        if (ChkObjFoundInTheRight.Count > 0)
                        {
                            if (allowFirstOpponentBlockingInRight == 0)
                                allowFirstOpponentBlockingInRight = 1;
                            objFoundInTheRight = true;
                        }

                        // check si un obstacle se trouve sur le map (NON JOUEUR)
                        if (!CurMapFreeCellToSpell(new Point((playerPosition.X + 1 + cnt) * 30, (playerPosition.Y * 30))))
                        {
                            objFoundInTheRight = true;
                            __spellTuiles_right.brush = spellAreaNotAllowedColor;
                            __spellTuiles_right.tag = spellAreaNotAllowedColor;
                        }

                        if (allowFirstOpponentBlockingInRight == 1)
                        {
                            // un joueur se met en obstacle sur le coté droit du joueur, et du coup on autorise le focus sur son case, se qui n'est pas pareil pour un obstacle non joueur
                            __spellTuiles_right.brush = spellAreaAllowedColor;
                            __spellTuiles_right.tag = spellAreaAllowedColor;
                            allowFirstOpponentBlockingInRight = -1;
                        }
                    }
                    ////////////////////////////////////

                    // check si la case à gauche qu'on veux dessiner ne sort pas des bord
                    ////////////// left  ///////////////
                    if (playerPosition.X - 1 - cnt < ScreenManager.TilesWidth && playerPosition.Y < ScreenManager.TilesHeight)
                    {
                        Rec __spellTuiles_left = new Rec(!objFoundInTheLeft ? spellAreaAllowedColor : spellAreaNotAllowedColor, new Point((playerPosition.X - 1 - cnt) * 30 + 1, (playerPosition.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                        __spellTuiles_left.MouseClic += __spellTuiles_MouseClic;
                        __spellTuiles_left.MouseOut += __spellTuiles_MouseOut;
                        __spellTuiles_left.MouseMove += __spellTuiles_MouseMove;

                        // check s'il y à un obstacle non joueur
                        if (CurMapFreeCellToWalk(new Point((playerPosition.X - 1 - cnt) * 30, playerPosition.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X - 1 - cnt && f.realPosition.Y == playerPosition.Y))
                            if (cnt > 1)
                                Manager.manager.GfxBgrList.Add(__spellTuiles_left);

                        // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                        __spellTuiles_left.tag = !objFoundInTheLeft ? spellAreaAllowedColor : spellAreaNotAllowedColor;

                        // check si un joueur se trouve sur la pos à gauche pour bloquer les prochain tuiles
                        List<Actor> ChkObjFoundInTheLeft = Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == (playerPosition.X - 1 - cnt) && f.realPosition.Y == playerPosition.Y);
                        if (ChkObjFoundInTheLeft.Count > 0)
                        {
                            objFoundInTheLeft = true;
                            if (allowFirstOpponentBlockingInLeft == 0)
                                allowFirstOpponentBlockingInLeft = 1;
                        }

                        // check si un obstacle se trouve sur le map (NON JOUEUR)
                        if (!CurMapFreeCellToSpell(new Point((playerPosition.X - 1 - cnt) * 30, (playerPosition.Y) * 30)))
                        {
                            objFoundInTheLeft = true;
                            __spellTuiles_left.brush = spellAreaNotAllowedColor;
                            __spellTuiles_left.tag = spellAreaNotAllowedColor;
                        }

                        if (allowFirstOpponentBlockingInLeft == 1)
                        {
                            // un joueur se met en obstacle sur le coté gauche du joueur, et du coup on autorise le focus sur son case, se qui n'est pas pareil pour un obstacle non joueur
                            __spellTuiles_left.brush = spellAreaAllowedColor;
                            __spellTuiles_left.tag = spellAreaAllowedColor;
                            allowFirstOpponentBlockingInLeft = -1;
                        }
                    }
                    ////////////////////////////////////

                    // check si la case en bas qu'on veux dessiner ne sort pas des bord
                    ///////////// down /////////////////
                    if (playerPosition.X < ScreenManager.TilesWidth && playerPosition.Y + 1 + cnt < ScreenManager.TilesHeight)
                    {
                        Rec __spellTuiles_down = new Rec(!objFoundInTheDown ? spellAreaAllowedColor : spellAreaNotAllowedColor, new Point(playerPosition.X * 30 + 1, ((playerPosition.Y + 1 + cnt) * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                        __spellTuiles_down.MouseClic += __spellTuiles_MouseClic;
                        __spellTuiles_down.MouseMove += __spellTuiles_MouseMove;
                        __spellTuiles_down.MouseOut += __spellTuiles_MouseOut;

                        // check s'il y à un obstacle non joueur
                        if (CurMapFreeCellToWalk(new Point(playerPosition.X * 30, (playerPosition.Y + 1 + cnt) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X && f.realPosition.Y == playerPosition.Y + 1 + cnt))
                            if (cnt > 1)
                                Manager.manager.GfxBgrList.Add(__spellTuiles_down);

                        // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                        __spellTuiles_down.tag = !objFoundInTheDown ? CommonCode.spellAreaAllowedColor : CommonCode.spellAreaNotAllowedColor;

                        // check si un joueur se trouve sur la pos de bas pour bloquer les prochain tuiles
                        List<Actor> ChkObjFoundInTheDown = Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == playerPosition.X && f.realPosition.Y == playerPosition.Y + 1 + cnt);
                        if (ChkObjFoundInTheDown.Count > 0)
                        {
                            if (allowFirstOpponentBlockingInDown == 0)
                                allowFirstOpponentBlockingInDown = 1;
                            objFoundInTheDown = true;
                        }

                        // check si un obstacle se trouve sur le map (NON JOUEUR)
                        if (!CurMapFreeCellToSpell(new Point((playerPosition.X * 30), (playerPosition.Y + 1 + cnt) * 30)))
                        {
                            objFoundInTheDown = true;
                            __spellTuiles_down.brush = spellAreaNotAllowedColor;
                            __spellTuiles_down.tag = spellAreaNotAllowedColor;
                        }

                        if (allowFirstOpponentBlockingInDown == 1)
                        {
                            // un joueur se met en obstacle sur le coté bas du joueur, et du coup on autorise le focus sur son case, se qui n'est pas pareil pour un obstacle non joueur
                            __spellTuiles_down.brush = spellAreaAllowedColor;
                            __spellTuiles_down.tag = spellAreaAllowedColor;
                            allowFirstOpponentBlockingInDown = -1;
                        }
                    }
                    /////////////////////////////////////

                    // check si la case en haut qu'on veux dessiner ne sort pas des bord
                    //////////// up /////////////////////
                    if (playerPosition.X < ScreenManager.TilesWidth && playerPosition.Y - 1 - cnt < ScreenManager.TilesHeight)
                    {

                        Rec __spellTuiles_up = new Rec(!objFoundInTheUp ? spellAreaAllowedColor : spellAreaNotAllowedColor, new Point(playerPosition.X * 30 + 1, ((playerPosition.Y - 1 - cnt) * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                        __spellTuiles_up.MouseClic += __spellTuiles_MouseClic;
                        __spellTuiles_up.MouseMove += __spellTuiles_MouseMove;
                        __spellTuiles_up.MouseOut += __spellTuiles_MouseOut;

                        // check s'il y à un obstacle non joueur
                        if (CurMapFreeCellToWalk(new Point(playerPosition.X * 30, (playerPosition.Y - 1 - cnt) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X && f.realPosition.Y == playerPosition.Y - 1 - cnt))
                            if (cnt > 1)
                                Manager.manager.GfxBgrList.Add(__spellTuiles_up);

                        // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                        __spellTuiles_up.tag = !objFoundInTheUp ? spellAreaAllowedColor : spellAreaNotAllowedColor;

                        // check si un joueur se trouve sur le haut de bas pour bloquer les prochain tuiles
                        List<Actor> ChkObjFoundInTheUp = Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == playerPosition.X && f.realPosition.Y == playerPosition.Y - 1 - cnt);
                        if (ChkObjFoundInTheUp.Count > 0)
                        {
                            if (allowFirstOpponentBlockingInUp == 0)
                                allowFirstOpponentBlockingInUp = 1;
                            objFoundInTheUp = true;
                        }

                        // check si un obstacle se trouve sur la map (NON JOUEUR)
                        if (!CurMapFreeCellToSpell(new Point((playerPosition.X * 30), (playerPosition.Y - 1 - cnt) * 30)))
                        {
                            objFoundInTheUp = true;
                            __spellTuiles_up.brush = spellAreaNotAllowedColor;
                            __spellTuiles_up.tag = spellAreaNotAllowedColor;
                        }

                        if (allowFirstOpponentBlockingInUp == 1)
                        {
                            // un joueur se met en obstacle sur le coté haut du joueur, et du coup on autorise le focus sur son case, se qui n'est pas pareil pour un obstacle non joueur
                            __spellTuiles_up.brush = spellAreaAllowedColor;
                            __spellTuiles_up.tag = spellAreaAllowedColor;
                            allowFirstOpponentBlockingInUp = -1;
                        }
                    }
                    /////////////////////////////////////
                }
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 1)
            {
                #region Shuriken
                // determination de la porté du sort
                int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu + Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).pe;
                List<sort_tuile_info> allTuilesInfo = spells.isAllowedSpellInSquareArea(pe, 0, true);

                for (int i = 0; i < allTuilesInfo.Count; i++)
                {
                    if (allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView == false)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaAllowedColor;
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView)
                    {
                        // zones non accessible et qui bloque la ligne de vus
                        // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                        // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort
                        Brush br;
                        if (Battle.AllPlayersByOrder.Exists(f => f.realPosition == allTuilesInfo[i].TuilePoint))
                            br = spellAreaAllowedColor;
                        else
                            br = spellAreaNotAllowedColor;

                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            Rec __spellTuiles = new Rec(br, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = br;
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable == false && allTuilesInfo[i].isBlockingView == true)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaNotAllowedColor;
                        }
                    }
                }
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 2)
            {
                #region rasen shuriken
                // determination de la porté du sort
                int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu + Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).pe + 2;
                List<sort_tuile_info> allTuilesInfo = spells.isAllowedSpellInSquareArea(pe, 2, true);

                for (int i = 0; i < allTuilesInfo.Count; i++)
                {
                    if (allTuilesInfo[i].isWalkable && !allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaAllowedColor;
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones non accessible et qui bloque la ligne de vus
                            // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                            // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort
                            Brush br;
                            if (Battle.AllPlayersByOrder.Exists(f => f.realPosition == allTuilesInfo[i].TuilePoint))
                                br = spellAreaAllowedColor;
                            else
                                br = spellAreaNotAllowedColor;

                            Rec __spellTuiles = new Rec(br, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = br;
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable == false && allTuilesInfo[i].isBlockingView == true)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaNotAllowedColor;
                        }
                    }
                }
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 3)
            {
                #region Kage bunshin no jutsu
                // determination de la porté du sort
                int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu;
                List<sort_tuile_info> allTuilesInfo = spells.isAllowedSpellInSquareArea(pe, 0, false);

                for (int i = 0; i < allTuilesInfo.Count; i++)
                {
                    if (allTuilesInfo[i].isWalkable && !allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaAllowedColor;
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessible mais qui bloque la ligne de vus
                            // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                            // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort s'il est déja en ligne de vue
                            Brush br;
                            if (Battle.AllPlayersByOrder.Exists(f => f.realPosition == allTuilesInfo[i].TuilePoint))
                                br = spellAreaNotAllowedColor;
                            else
                                br = spellAreaAllowedColor;
                            Rec __spellTuiles = new Rec(br, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = br;
                        }
                    }
                    else if (!allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaNotAllowedColor;
                        }
                    }
                }

                // dessin de notre clone du personnage en mode invisible, et se met en visible pour ajustement de l'orientation lorsque le joueur survole une case
                // determination de l'orientation selon les coordonées x et y
                //...
                ///

                // affichage du personnage + position + orientation
                Bmp __clone_jutsu = new Bmp(@"gfx\general\classes\naruto.dat", Point.Empty, "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto", Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), CommonCode.ConvertToClockWizeOrientation(1)));
                Manager.manager.GfxObjList.Add(__clone_jutsu);
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 5)
            {
                #region Gamabunta
                // determination de la portée du sort
                //int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu;
                List<sort_tuile_info> allTuilesInfo = new List<sort_tuile_info>();
                
                // check si c'est la case en haut n'est pas bloqué par un mur
                if(CurMapFreeCellToSpell(new Point(playerPosition.X * 30, (playerPosition.Y - 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X && f.realPosition.Y == playerPosition.Y - 1))
                {
                    sort_tuile_info sti = new sort_tuile_info();
                    sti.isBlockingView = false;
                    sti.isWalkable = true;
                    sti.TuilePoint = new Point(playerPosition.X, playerPosition.Y - 1);
                    sti.data = "up";
                    allTuilesInfo.Add(sti);
                }

                // check si c'est la case en bas n'est pas bloqué par un mur
                if (CurMapFreeCellToSpell(new Point(playerPosition.X * 30, (playerPosition.Y + 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X && f.realPosition.Y == playerPosition.Y + 1))
                {
                    sort_tuile_info sti = new sort_tuile_info
                    {
                        isBlockingView = false,
                        isWalkable = true,
                        TuilePoint = new Point(playerPosition.X, playerPosition.Y + 1),
                        data = "down"
                    };

                    allTuilesInfo.Add(sti);
                }

                // check si c'est la case à droite n'est pas bloqué par un mur
                if (CurMapFreeCellToSpell(new Point((playerPosition.X + 1) * 30, playerPosition.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X + 1 && f.realPosition.Y == playerPosition.Y))
                {
                    sort_tuile_info sti = new sort_tuile_info
                    {
                        isBlockingView = false,
                        isWalkable = true,
                        TuilePoint = new Point(playerPosition.X + 1, playerPosition.Y),
                        data = "right"
                    };
                    allTuilesInfo.Add(sti);
                }

                // check si c'est la case à gauche n'est pas bloqué par un mur
                if (CurMapFreeCellToSpell(new Point((playerPosition.X - 1) * 30, playerPosition.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X - 1 && f.realPosition.Y == playerPosition.Y))
                {
                    sort_tuile_info sti = new sort_tuile_info
                    {
                        isBlockingView = false,
                        isWalkable = true,
                        TuilePoint = new Point(playerPosition.X - 1, playerPosition.Y),
                        data = "left"
                    };
                    allTuilesInfo.Add(sti);
                }

                for (int i = 0; i < allTuilesInfo.Count; i++)
                {
                    if (allTuilesInfo[i].isWalkable && !allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = spellAreaAllowedColor;
                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            Txt __orientation = new Txt(allTuilesInfo[i].data, Point.Empty, "__orientation", Manager.TypeGfx.Bgr, false, new Font("verdana", 8), Brushes.Blue);
                            __spellTuiles.Child.Add(__orientation);

                            // memorisation de la position du joueur pour en savoir la position des cases concernées sur le code du survole
                            Txt __point = new Txt(playerPosition.X + "-" + playerPosition.Y, Point.Empty, "__point", Manager.TypeGfx.Bgr, false, new Font("verdana", 8), Brushes.Blue);
                            __spellTuiles.Child.Add(__point);
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessible mais qui bloque la ligne de vus
                            // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                            // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort s'il est déja en ligne de vue
                            Brush br;
                            br = Battle.AllPlayersByOrder.Exists(f => f.realPosition == allTuilesInfo[i].TuilePoint) ? spellAreaNotAllowedColor : spellAreaAllowedColor;
                            Rec __spellTuiles = new Rec(br, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = br;
                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            Txt __orientation = new Txt(allTuilesInfo[i].data, Point.Empty);
                            __orientation.name = "__orientation";
                            __spellTuiles.Child.Add(__orientation);

                            // memorisation de la position du joueur pour en savoir la position des cases concernées sur le code du survole
                            Txt __point = new Txt(playerPosition.X + "-" + playerPosition.Y, Point.Empty, "__point", Manager.TypeGfx.Bgr, false, new Font("verdana", 8), Brushes.Blue);
                            __spellTuiles.Child.Add(__point);
                        }
                    }
                    else if (!allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = CommonCode.spellAreaNotAllowedColor;
                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            Txt __orientation = new Txt(allTuilesInfo[i].data, Point.Empty);
                            __orientation.name = "__orientation";
                            __spellTuiles.Child.Add(__orientation);

                            // memorisation de la position du joueur pour en savoir la position des cases concernées sur le code du survole
                            Txt __point = new Txt(playerPosition.X + "-" + playerPosition.Y, Point.Empty, "__point", Manager.TypeGfx.Bgr, false, new Font("verdana", 8), Brushes.Blue);
                            __spellTuiles.Child.Add(__point);
                        }
                    }
                }

                // dessin de notre clone du personnage en mode invisible, et se met en visible pour ajustement de l'orientation lorsque le joueur survole une case
                // determination de l'orientation selon les coordonées x et y
                //...
                ///

                // affichage du personnage + position + orientation
                Bmp __clone_jutsu = new Bmp(@"gfx\general\classes\naruto.dat", Point.Empty, "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto", Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), CommonCode.ConvertToClockWizeOrientation(1)));
                Manager.manager.GfxObjList.Add(__clone_jutsu);
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 6)
            {
                #region transfert de vie
                // determination de la porté du sort
                int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu + Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).pe;
                List<sort_tuile_info> allTuilesInfo = spells.isAllowedSpellInSquareArea(pe, 0, true);

                for (int i = 0; i < allTuilesInfo.Count(); i++)
                {
                    if (allTuilesInfo[i].isWalkable && !allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaAllowedColor;
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones non accessible et qui bloque la ligne de vus
                            // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                            // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort
                            Brush br = Battle.AllPlayersByOrder.Exists(f => f.realPosition == allTuilesInfo[i].TuilePoint) ? spellAreaAllowedColor : spellAreaNotAllowedColor;

                            Rec __spellTuiles = new Rec(br, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = br;
                        }
                    }
                    else if (!allTuilesInfo[i].isWalkable && allTuilesInfo[i].isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaNotAllowedColor;
                        }
                    }
                }
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 7)
            {
                #region transfert de pc
                // determination de la porté du sort
                int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu + Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).pe;
                List<sort_tuile_info> allTuilesInfo = spells.isAllowedSpellInSquareArea(pe, 0, true);

                foreach (sort_tuile_info t in allTuilesInfo)
                {
                    if (t.isWalkable && !t.isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaAllowedColor;
                        }
                    }
                    else if (t.isWalkable && t.isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones non accessible et qui bloque la ligne de vus
                            // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                            // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort
                            Brush br;
                            br = Battle.AllPlayersByOrder.Exists(f => f.realPosition == t.TuilePoint) ? spellAreaAllowedColor : spellAreaNotAllowedColor;

                            Rec __spellTuiles = new Rec(br, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = br;
                        }
                    }
                    else if (t.isWalkable == false && t.isBlockingView == true)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaNotAllowedColor;
                        }
                    }
                }
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 8)
            {
                #region transfert de pm
                // determination de la porté du sort
                int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu + Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).pe;
                List<sort_tuile_info> allTuilesInfo = spells.isAllowedSpellInSquareArea(pe, 0, true);

                foreach (sort_tuile_info t in allTuilesInfo)
                {
                    if (t.isWalkable && !t.isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaAllowedColor;
                        }
                    }
                    else if (t.isWalkable && t.isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones non accessible et qui bloque la ligne de vus
                            // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                            // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort
                            Brush br = Battle.AllPlayersByOrder.Exists(f => f.realPosition == t.TuilePoint) ? spellAreaAllowedColor : spellAreaNotAllowedColor;

                            Rec __spellTuiles = new Rec(br, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = br;
                        }
                    }
                    else if (!t.isWalkable && t.isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaNotAllowedColor;
                        }
                    }
                }
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 9)
            {
                #region transfert de puissance
                // determination de la porté du sort
                int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu + Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).pe;
                List<sort_tuile_info> allTuilesInfo = spells.isAllowedSpellInSquareArea(pe, 0, true);

                foreach (sort_tuile_info t in allTuilesInfo)
                {
                    if (t.isWalkable && !t.isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaAllowedColor;
                        }
                    }
                    else if (t.isWalkable && t.isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones non accessible et qui bloque la ligne de vus
                            // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                            // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort
                            Brush br;
                            if (Battle.AllPlayersByOrder.Exists(f => f.realPosition == t.TuilePoint))
                                br = spellAreaAllowedColor;
                            else
                                br = spellAreaNotAllowedColor;

                            Rec __spellTuiles = new Rec(br, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = br;
                        }
                    }
                    else if (!t.isWalkable && t.isBlockingView)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == t.TuilePoint.X && f.realPosition.Y == t.TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((t.TuilePoint.X * 30) + 1, (t.TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            __spellTuiles.tag = spellAreaNotAllowedColor;
                        }
                    }
                }
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 10)
            {
                #region Etat Sennin
                Actor pi = Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo);
                Rec __spellTuiles = new Rec(spellAreaAllowedColor, new Point((pi.realPosition.X * 30) + 1, (pi.realPosition.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                __spellTuiles.tag = spellAreaAllowedColor;
                Manager.manager.GfxBgrList.Add(__spellTuiles);
                #endregion
            }
            else if (Battle.infos_sorts.sortID == 11)
            {
                #region katas des crapauds
                // determination de la portée du sort
                //int pe = spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].etendu;
                List<sort_tuile_info> allTuilesInfo = new List<sort_tuile_info>();

                // check si c'est la case en haut à droite n'est pas bloqué par un mur
                if (CurMapFreeCellToSpell(new Point((playerPosition.X + 1) * 30, (playerPosition.Y - 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X + 1 && f.realPosition.Y == playerPosition.Y - 1))
                {
                    sort_tuile_info sti = new sort_tuile_info();
                    sti.isBlockingView = false;
                    sti.isWalkable = true;
                    sti.TuilePoint = new Point(playerPosition.X + 1, playerPosition.Y - 1);
                    sti.data = "up right";
                    allTuilesInfo.Add(sti);
                }

                // check si c'est la case en haut à gauche n'est pas bloqué par un mur
                if (CurMapFreeCellToSpell(new Point((playerPosition.X - 1) * 30, (playerPosition.Y - 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X - 1 && f.realPosition.Y == playerPosition.Y - 1))
                {
                    sort_tuile_info sti = new sort_tuile_info();

                    sti.isBlockingView = false;
                    sti.isWalkable = true;
                    sti.TuilePoint = new Point(playerPosition.X - 1, playerPosition.Y - 1);
                    sti.data = "up left";
                    allTuilesInfo.Add(sti);
                }

                // check si c'est la case en bas à droite n'est pas bloqué par un mur
                if (CurMapFreeCellToSpell(new Point((playerPosition.X + 1) * 30, (playerPosition.Y + 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X + 1 && f.realPosition.Y == playerPosition.Y + 1))
                {
                    sort_tuile_info sti = new sort_tuile_info();
                    sti.isBlockingView = false;
                    sti.isWalkable = true;
                    sti.TuilePoint = new Point(playerPosition.X + 1, playerPosition.Y + 1);
                    sti.data = "down right";
                    allTuilesInfo.Add(sti);
                }

                // check si c'est la case en bas à gauche n'est pas bloqué par un mur
                if (CurMapFreeCellToSpell(new Point((playerPosition.X - 1) * 30, (playerPosition.Y + 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == playerPosition.X - 1 && f.realPosition.Y == playerPosition.Y + 1))
                {
                    sort_tuile_info sti = new sort_tuile_info();
                    sti.isBlockingView = false;
                    sti.isWalkable = true;
                    sti.TuilePoint = new Point(playerPosition.X - 1, playerPosition.Y + 1);
                    sti.data = "down left";
                    allTuilesInfo.Add(sti);
                }

                for (int i = 0; i < allTuilesInfo.Count(); i++)
                {
                    if (allTuilesInfo[i].isWalkable == true && allTuilesInfo[i].isBlockingView == false)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(CommonCode.spellAreaAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = CommonCode.spellAreaAllowedColor;
                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            Txt __orientation = new Txt(allTuilesInfo[i].data, Point.Empty, "__orientation", Manager.TypeGfx.Bgr, false, new Font("verdana", 8), Brushes.Blue);
                            __spellTuiles.Child.Add(__orientation);

                            // memorisation de la position du joueur pour en savoir la position des cases concernées sur le code du survole
                            Txt __point = new Txt(playerPosition.X + "-" + playerPosition.Y, Point.Empty, "__point", Manager.TypeGfx.Bgr, false, new Font("verdana", 8), Brushes.Blue);
                            __spellTuiles.Child.Add(__point);
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable == true && allTuilesInfo[i].isBlockingView == true)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessible mais qui bloque la ligne de vus
                            // il faut tout de meme ajouter un code pour savoir si cette position correspond à un obstacle ordinaire ou un joueur
                            // parsque un joueur dois avoir la zone en bleu puisqu'il est accessible par le sort s'il est déja en ligne de vue
                            Brush br;
                            if (Battle.AllPlayersByOrder.Exists(f => f.realPosition == allTuilesInfo[i].TuilePoint))
                                br = CommonCode.spellAreaNotAllowedColor;
                            else
                                br = CommonCode.spellAreaAllowedColor;
                            Rec __spellTuiles = new Rec(br, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = br;
                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            Txt __orientation = new Txt(allTuilesInfo[i].data, Point.Empty);
                            __orientation.name = "__orientation";
                            __spellTuiles.Child.Add(__orientation);

                            // memorisation de la position du joueur pour en savoir la position des cases concernées sur le code du survole
                            Txt __point = new Txt(playerPosition.X + "-" + playerPosition.Y, Point.Empty, "__point", Manager.TypeGfx.Bgr, false, new Font("verdana", 8), Brushes.Blue);
                            __spellTuiles.Child.Add(__point);
                        }
                    }
                    else if (allTuilesInfo[i].isWalkable == false && allTuilesInfo[i].isBlockingView == true)
                    {
                        // check s'il y à un obstacle non joueur pour ne pas colorer la zone
                        if (CurMapFreeCellToWalk(new Point(allTuilesInfo[i].TuilePoint.X * 30, allTuilesInfo[i].TuilePoint.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == allTuilesInfo[i].TuilePoint.X && f.realPosition.Y == allTuilesInfo[i].TuilePoint.Y))
                        {
                            // zones accessibles qui ne contiens pas d'obstacles
                            Rec __spellTuiles = new Rec(spellAreaNotAllowedColor, new Point((allTuilesInfo[i].TuilePoint.X * 30) + 1, (allTuilesInfo[i].TuilePoint.Y * 30) + 1), new Size(29, 29), "__spellTuiles", Manager.TypeGfx.Bgr, true);
                            __spellTuiles.MouseClic += __spellTuiles_MouseClic;
                            __spellTuiles.MouseOut += __spellTuiles_MouseOut;
                            __spellTuiles.MouseMove += __spellTuiles_MouseMove;
                            Manager.manager.GfxBgrList.Add(__spellTuiles);

                            __spellTuiles.tag = CommonCode.spellAreaNotAllowedColor;
                            // memorisation de la couleur de la tuile pour la remettre sur sa couleur d'origine quand sa change lors d'un survole
                            Txt __orientation = new Txt(allTuilesInfo[i].data, Point.Empty);
                            __orientation.name = "__orientation";
                            __spellTuiles.Child.Add(__orientation);

                            // memorisation de la position du joueur pour en savoir la position des cases concernées sur le code du survole
                            Txt __point = new Txt(playerPosition.X + "-" + playerPosition.Y, Point.Empty, "__point", Manager.TypeGfx.Bgr, false, new Font("verdana", 8), Brushes.Blue);
                            __spellTuiles.Child.Add(__point);
                        }
                    }
                }

                // dessin de notre clone du personnage en mode invisible, et se met en visible pour ajustement de l'orientation lorsque le joueur survole une case
                // determination de l'orientation selon les coordonées x et y
                //...
                ///

                // affichage du personnage + position + orientation
                Bmp __clone_jutsu = new Bmp(@"gfx\general\classes\naruto.dat", Point.Empty, "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto", Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), CommonCode.ConvertToClockWizeOrientation(1)));
                Manager.manager.GfxObjList.Add(__clone_jutsu);
                #endregion
            }
        }
        static void __spellTuiles_MouseMove(Rec rec,MouseEventArgs e)
        {
            #region
            // survole sur les tuiles representant la porté d'un sort
            if (Battle.currentCursor != "")
            {
                Color c = new Pen(rec.brush).Color;
                // remise en couleurs par defaut de tout les tuiles affecté par le sort
                List<IGfx> __spellTuiles = Manager.manager.GfxBgrList.FindAll(f => f.Name() == "__spellTuiles");
                for (int cnt = 0; cnt < __spellTuiles.Count; cnt++)
                    if (new Pen((__spellTuiles[cnt] as Rec).brush).Color.R != 128 && new Pen((__spellTuiles[cnt] as Rec).brush).Color.G != 128 && new Pen((__spellTuiles[cnt] as Rec).brush).Color.B != 128)
                        (__spellTuiles[cnt] as Rec).brush = CommonCode.spellAreaAllowedColor;

                if (rec.brush == CommonCode.spellAreaAllowedColor)
                {
                    // check si la tuile sort des bords de l'ecran (ce sort na pas besoin de ce controle puisqu'il s'agit d'une suel case qui ne peux pas sortir des bord de tt façon)
                    // if (rec.point.X / 30 < ScreenManager.TilesWidth && (rec.point.Y + (line * 30)) / 30 < ScreenManager.TilesHeight)
                    // coloriage du tuile survolé
                    if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.doton)
                        rec.brush = dotonColor;
                    else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.katon)
                        rec.brush = katonColor;
                    else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.futon)
                        rec.brush = futonColor;
                    else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.raiton)
                        rec.brush = raitonColor;
                    else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.suiton)
                        rec.brush = suitonColor;
                    else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.neutral)
                        rec.brush = neutralColor;
                }
                // sorts qui cible 1 seul case qui na pas besoin d'avoir le controle qui cible les tuiles hors zone d'affichage
                if (rec.brush != spellAreaNotAllowedColor && (Battle.infos_sorts.sortID == 0 || Battle.infos_sorts.sortID == 1 || Battle.infos_sorts.sortID == 3 || Battle.infos_sorts.sortID == 10 || Battle.infos_sorts.sortID == 11))
                {
                    #region rasengan ou shuriken ou kagebunshin no jutsu, Etat Sennin, katas des crapauds

                    ///////////////////////////////////////////////////////////////////////////
                    // affichage de certains elements dépondant des sorts comme les invocations
                    if (Battle.infos_sorts.sortID == 3)
                    {
                        // clone jutsu
                        IGfx __clone_jutsu = Manager.manager.GfxObjList.Find(f => f.Name() == "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto");
                        if (__clone_jutsu == null)
                        {
                            MessageBox.Show("error");
                            __clone_jutsu = new Bmp(@"gfx\general\classes\naruto.dat", Point.Empty, "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto", Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), CommonCode.ConvertToClockWizeOrientation(1)));
                            Manager.manager.GfxObjList.Add(__clone_jutsu);
                        }
                        else
                        {
                            // determination de l'orientation selon les coordonées x et y
                            int x = Math.Abs(rec.point.X - (Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).realPosition.X * 30));
                            int y = Math.Abs(rec.point.Y - (Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).realPosition.Y * 30));

                            // longeur horizontal plus grande que longeur verticale
                            if (x > y)
                            {
                                // determination de l'orientation horizontal, à droite ou à gauche
                                if (rec.point.X > Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).realPosition.X * 30)
                                {
                                    // orientation à droite
                                    (__clone_jutsu as Bmp).ChangeBmp(@"gfx\general\classes\naruto.dat", SpriteSheet.GetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), CommonCode.ConvertToClockWizeOrientation(1)));
                                }
                                else
                                {
                                    // orientation à gauche
                                    (__clone_jutsu as Bmp).ChangeBmp(@"gfx\general\classes\naruto.dat", SpriteSheet.GetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), CommonCode.ConvertToClockWizeOrientation(3)));
                                }
                            }
                            else
                            {
                                // determination de l'orientation horizontal, à droite ou à gauche
                                if (rec.point.Y > Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).realPosition.Y * 30)
                                {
                                    // orientation en bas
                                    (__clone_jutsu as Bmp).ChangeBmp(@"gfx\general\classes\naruto.dat", SpriteSheet.GetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), CommonCode.ConvertToClockWizeOrientation(2)));
                                }
                                else
                                {
                                    // orientation en haut
                                    (__clone_jutsu as Bmp).ChangeBmp(@"gfx\general\classes\naruto.dat", SpriteSheet.GetSpriteSheet(Enums.ActorClass.ClassName.naruto.ToString(), CommonCode.ConvertToClockWizeOrientation(0)));
                                }
                            }
                            (__clone_jutsu as Bmp).point = new Point(rec.point.X + 15 - ((__clone_jutsu as Bmp).rectangle.Width / 2), rec.point.Y - (__clone_jutsu as Bmp).rectangle.Height + 15);
                            (__clone_jutsu as Bmp).visible = true;

                            CommonCode.VerticalSyncZindex((__clone_jutsu as Bmp));
                            (__clone_jutsu as Bmp).newColorMap = MyPlayerInfo.instance.ibPlayer.newColorMap;
                        }
                    }
                    #endregion
                }
                else if (rec.brush != spellAreaNotAllowedColor && Battle.infos_sorts.sortID == 2)
                {
                    #region rasen shuriken sort de zone
                    // cellule du centre
                    //rec.brush = new SolidBrush(Color.FromArgb(0, 197, 125));

                    // ajouter des tuiles pour faires un sort de zone
                    int pe = 0;
                    if (Battle.infos_sorts.level == 3)
                        pe = 1;
                    else if (Battle.infos_sorts.level == 5)
                        pe = 2;

                    for (int line = 0; line <= pe; line++)
                    {
                        // ajout des tuiles verticales
                        if (line > 0)
                        {
                            // check si la tuile sort des bords de l'ecran
                            if (rec.point.X / 30 < ScreenManager.TilesWidth && (rec.point.Y + (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child1 = new Rec(CommonCode.futonColor, new Point(rec.point.X, rec.point.Y + (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }

                            // check si la tuile sort des bords de l'ecran
                            if (rec.point.X / 30 < ScreenManager.TilesWidth && (rec.point.Y - (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child2 = new Rec(CommonCode.futonColor, new Point(rec.point.X, rec.point.Y - (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child2.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child2.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child2);
                            }
                        }

                        // ajout des tuiles horizontal
                        for (int cols = 0; cols < pe - line; cols++)
                        {
                            // check si la tuile sort des bords de l'ecran
                            if ((rec.point.X + 30 + (cols * 30)) / 30 < ScreenManager.TilesWidth && (rec.point.Y + (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                // 1 seul ligne, centrale
                                // ligne centrale
                                Rec __spellTuiles_child1 = new Rec(CommonCode.futonColor, new Point(rec.point.X + 30 + (cols * 30), rec.point.Y + (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }

                            // check si la tuile sort des bords de l'ecran
                            if ((rec.point.X - 30 - (cols * 30)) / 30 < ScreenManager.TilesWidth && (rec.point.Y + (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child2 = new Rec(CommonCode.futonColor, new Point(rec.point.X - 30 - (cols * 30), rec.point.Y + (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child2.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child2.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child2);
                            }

                            // check si la tuile sort des bords de l'ecran
                            if ((rec.point.X + 30 + (cols * 30)) / 30 < ScreenManager.TilesWidth && (rec.point.Y - (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child3 = new Rec(CommonCode.futonColor, new Point(rec.point.X + 30 + (cols * 30), rec.point.Y - (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child3.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child3.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child3);
                            }

                            // check si la tuile sort des bords de l'ecran
                            if ((rec.point.X - 30 - (cols * 30)) / 30 < ScreenManager.TilesWidth && (rec.point.Y - (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child4 = new Rec(CommonCode.futonColor, new Point(rec.point.X - 30 - (cols * 30), rec.point.Y - (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child4.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child4.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child4);
                            }
                        }
                    }
                    #endregion
                }
                else if (rec.brush != spellAreaNotAllowedColor && Battle.infos_sorts.sortID == 5)
                {
                    #region gamabunta
                    // cellule du centre

                    if (rec.Child.Exists(f => f.Name() == "__orientation") && rec.Child.Exists(f => f.Name() == "__point"))
                    {
                        Txt __orientation = rec.Child.Find(f => f.Name() == "__orientation") as Txt;
                        Txt __point = rec.Child.Find(f => f.Name() == "__point") as Txt;

                        if (__orientation.Text == "up")
                        {
                            Point centre = new Point(Convert.ToInt32(__point.Text.Split('-')[0]), Convert.ToInt32(__point.Text.Split('-')[1]) - 1);
                            // on verifie si les 2 cases sont accessibles

                            if (CommonCode.CurMapFreeCellToSpell(new Point((centre.X - 1) * 30, centre.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == centre.X - 1 && f.realPosition.Y == centre.Y))
                            {
                                // check si la tuile à droite du centre est accessible
                                Rec __spellTuiles_child1 = new Rec(rec.brush, new Point(rec.point.X - 30, rec.point.Y), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }

                            if (CommonCode.CurMapFreeCellToSpell(new Point((centre.X + 1) * 30, centre.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == centre.X + 1 && f.realPosition.Y == centre.Y))
                            {
                                // check si la tuile à droite du centre est accessible
                                Rec __spellTuiles_child1 = new Rec(rec.brush, new Point(rec.point.X + 30, rec.point.Y), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }
                        }
                        else if (__orientation.Text == "down")
                        {
                            Point centre = new Point(Convert.ToInt32(__point.Text.Split('-')[0]), Convert.ToInt32(__point.Text.Split('-')[1]) + 1);
                            // on verifie si les 2 cases sont accessibles

                            if (CommonCode.CurMapFreeCellToSpell(new Point((centre.X - 1) * 30, centre.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == centre.X - 1 && f.realPosition.Y == centre.Y))
                            {
                                // check si la tuile à droite du centre est accessible
                                Rec __spellTuiles_child1 = new Rec(rec.brush, new Point(rec.point.X - 30, rec.point.Y), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }

                            if (CommonCode.CurMapFreeCellToSpell(new Point((centre.X + 1) * 30, centre.Y * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == centre.X + 1 && f.realPosition.Y == centre.Y))
                            {
                                // check si la tuile à droite du centre est accessible
                                Rec __spellTuiles_child1 = new Rec(rec.brush, new Point(rec.point.X + 30, rec.point.Y), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }
                        }
                        else if (__orientation.Text == "right")
                        {
                            Point centre = new Point(Convert.ToInt32(__point.Text.Split('-')[0] + 1), Convert.ToInt32(__point.Text.Split('-')[1]));
                            // on verifie si les 2 cases sont accessibles

                            if (CommonCode.CurMapFreeCellToSpell(new Point(centre.X * 30, (centre.Y - 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == centre.X && f.realPosition.Y == centre.Y - 1))
                            {
                                // check si la tuile à droite du centre est accessible
                                Rec __spellTuiles_child1 = new Rec(rec.brush, new Point(rec.point.X, rec.point.Y - 30), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }

                            if (CommonCode.CurMapFreeCellToSpell(new Point(centre.X * 30, (centre.Y + 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == centre.X && f.realPosition.Y == centre.Y + 1))
                            {
                                // check si la tuile à droite du centre est accessible
                                Rec __spellTuiles_child1 = new Rec(rec.brush, new Point(rec.point.X, rec.point.Y + 30), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }
                        }
                        else if (__orientation.Text == "left")
                        {
                            Point centre = new Point(Convert.ToInt32(__point.Text.Split('-')[0]) - 1, Convert.ToInt32(__point.Text.Split('-')[1]));
                            // on verifie si les 2 cases sont accessibles

                            if (CommonCode.CurMapFreeCellToSpell(new Point(centre.X * 30, (centre.Y - 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == centre.X && f.realPosition.Y == centre.Y - 1))
                            {
                                // check si la tuile à droite du centre est accessible
                                Rec __spellTuiles_child1 = new Rec(rec.brush, new Point(rec.point.X, rec.point.Y - 30), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }

                            if (CommonCode.CurMapFreeCellToSpell(new Point(centre.X * 30, (centre.Y + 1) * 30)) || Battle.AllPlayersByOrder.Exists(f => f.realPosition.X == centre.X && f.realPosition.Y == centre.Y + 1))
                            {
                                // check si la tuile à droite du centre est accessible
                                Rec __spellTuiles_child1 = new Rec(rec.brush, new Point(rec.point.X, rec.point.Y + 30), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }
                        }
                    }
                    #endregion
                }
                else if (rec.brush != spellAreaNotAllowedColor && (Battle.infos_sorts.sortID == 6 || Battle.infos_sorts.sortID == 7 || Battle.infos_sorts.sortID == 8 || Battle.infos_sorts.sortID == 9))
                {
                    #region transfert de vie ou pc,pm,puissance
                    // cellule du centre

                    // ajouter des tuiles pour faires un sort de zone
                    int pe = 1;

                    for (int line = 0; line <= pe; line++)
                    {
                        // ajout des tuiles verticales
                        if (line > 0)
                        {
                            // check si la tuile sort des bords de l'ecran
                            if (rec.point.X / 30 < ScreenManager.TilesWidth && (rec.point.Y + (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child1 = new Rec(CommonCode.neutralColor, new Point(rec.point.X, rec.point.Y + (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }

                            // check si la tuile sort des bords de l'ecran
                            if (rec.point.X / 30 < ScreenManager.TilesWidth && (rec.point.Y - (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child2 = new Rec(CommonCode.neutralColor, new Point(rec.point.X, rec.point.Y - (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child2.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child2.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child2);
                            }
                        }

                        // ajout des tuiles horizontal
                        for (int cols = 0; cols < pe - line; cols++)
                        {
                            // check si la tuile sort des bords de l'ecran
                            if ((rec.point.X + 30 + (cols * 30)) / 30 < ScreenManager.TilesWidth && (rec.point.Y + (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                // 1 seul ligne, centrale
                                // ligne centrale
                                Rec __spellTuiles_child1 = new Rec(CommonCode.neutralColor, new Point(rec.point.X + 30 + (cols * 30), rec.point.Y + (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child1.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child1.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child1);
                            }

                            // check si la tuile sort des bords de l'ecran
                            if ((rec.point.X - 30 - (cols * 30)) / 30 < ScreenManager.TilesWidth && (rec.point.Y + (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child2 = new Rec(CommonCode.neutralColor, new Point(rec.point.X - 30 - (cols * 30), rec.point.Y + (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child2.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child2.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child2);
                            }

                            // check si la tuile sort des bords de l'ecran
                            if ((rec.point.X + 30 + (cols * 30)) / 30 < ScreenManager.TilesWidth && (rec.point.Y - (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child3 = new Rec(CommonCode.neutralColor, new Point(rec.point.X + 30 + (cols * 30), rec.point.Y - (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child3.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child3.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child3);
                            }

                            // check si la tuile sort des bords de l'ecran
                            if ((rec.point.X - 30 - (cols * 30)) / 30 < ScreenManager.TilesWidth && (rec.point.Y - (line * 30)) / 30 < ScreenManager.TilesHeight)
                            {
                                Rec __spellTuiles_child4 = new Rec(CommonCode.neutralColor, new Point(rec.point.X - 30 - (cols * 30), rec.point.Y - (line * 30)), new Size(29, 29), "__spellTuiles_child", Manager.TypeGfx.Bgr, true);
                                __spellTuiles_child4.MouseClic += __spellTuiles_MouseClic;
                                __spellTuiles_child4.MouseOut += __spellTuiles_MouseOut;
                                Manager.manager.GfxBgrList.Add(__spellTuiles_child4);
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }
        static void __spellTuiles_MouseClic(Rec rec, MouseEventArgs e)
        {
            #region Clic sur une tuile pour un sort lancé
            // lancement d'un sort lors d'un clic sur l'image
            if (rec.brush == spellAreaNotAllowedColor)
                return;

            // check si on a les pc necessaires
            if (Battle.currentCursor == "nopc")
            {
                ChatMsgFormat("S", "", TranslateText(122));
                ChatAreaCursorInTheEnd();
                return;
            }

            // check si on a les Poits d'invoc necessaires
            // comptage des invocs sur la map qui nous appartiens
            int sumOfInvoc = Battle.AllPlayersByOrder.FindAll(f => f.pseudo.IndexOf(MyPlayerInfo.instance.pseudo + "$") != -1).Count;
            if (spells.sort_d_invocation.Exists(f => f == Battle.infos_sorts.sortID) && Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).summons <= sumOfInvoc)
            {
                ChatMsgFormat("S", "", TranslateText(154));
                ChatAreaCursorInTheEnd();
                return;
            }

            Actor roxed = Battle.AllPlayersByOrder.Find(f => f.realPosition.X == (e.X / 30) && f.realPosition.Y == (e.Y / 30));

            // check des envoutements si on ai autorisé a jouer ce sort
            Actor pi = Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo);
            if (pi.BuffsList.Exists(f => f.SortID == Battle.infos_sorts.sortID))
            {
                Actor.Buff piEnv = pi.BuffsList.Find(f => f.SortID == Battle.infos_sorts.sortID);

                // check contre relanceInterval s'il a déja été lancé dans un tour précédent, d'où la relanceInterval qui ne dois pas être égale a celui attribué la 1er fois, pour laisser le joueur lancer le sort plusieurs fois le tour actuel
                if (piEnv.relanceInterval > 0 && piEnv.relanceInterval < spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].relanceInterval)
                {
                    // pas encore atteint le délais de relance
                    ChatMsgFormat("B", "null", TranslateText(114));
                    return;
                }
                else if (spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].relanceParTour > 0 && piEnv.playerRoxed.Count >= spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].relanceParTour/* && (roxed != null || Battle.infos_sorts.sortID == 2 || Battle.infos_sorts.sortID == 3)*/)
                {
                    // délais de relance par tour atteint, si = 0 donc illimité
                    ChatMsgFormat("B", "null", TranslateText(150));
                    return;
                }

                if(roxed != null)
                {
                    if (spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].relanceParJoueur > 0 && piEnv.playerRoxed.FindAll(f => f == roxed.pseudo).Count >= spells.sort(Battle.infos_sorts.sortID).isbl[Battle.infos_sorts.level - 1].relanceParJoueur)
                    {
                        // relance par sur le meme joueur atteint, si = 0 donc illimité
                        ChatMsgFormat("B", "null", TranslateText(151));
                        return;
                    }
                }
            }

            // check s'il sagit d'un sort qui necessite un mode précis comme Futon rasen shuriken qui necessite l'état Sennin
            if ((Battle.infos_sorts.sortID == 2 || Battle.infos_sorts.sortID == 11) && !pi.BuffsList.Exists(f => f.StateList.Exists(t => t == Enums.Buff.Name.Senin)))
            {
                // client qui n'est pas en mode sennin
                ChatMsgFormat("B", "null", TranslateText(155));
                return;
            }

            Network.SendMessage("cmd•spellTuiles•" + Battle.infos_sorts.sortID + "•" + (e.X / 30) + "•" + (e.Y / 30), true);

            // changement du curseur a son état initiale
            CursorHand_MouseMove(null, null);

            // effacement des tuiles de selection
            Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles");
            // effacement de tous les images du sort rasen shuriken
            Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles_child");
            
            // supression de certains element dépondant des sorts
            if (Battle.infos_sorts.sortID == 3)
            {
                // supression du clone qui apparait lors de la selection de la case
                IGfx __clone_jutsu = Manager.manager.GfxObjList.Find(f => f.Name() == "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto");
                if (__clone_jutsu == null)
                    MessageBox.Show("error2");
                else
                {
                    (__clone_jutsu as Bmp).visible = false;
                    Manager.manager.GfxObjList.Remove(__clone_jutsu);
                }
            }

            // reinitialisation du stat du sort en instance
            Battle.infos_sorts = null;
            Battle.currentCursor = "";
            #endregion
        }
        static void __spellTuiles_MouseOut(Rec rec, MouseEventArgs e)
        {
            #region
            // remet les couleurs a leurs origine quand une cellule est coloré par la couleurs de l'element de l'attaque
            if (Battle.currentCursor != "")
            {
                // remise en couleurs par defaut de tout les tuiles affecté par le sort
                List<IGfx> __spellTuiles = Manager.manager.GfxBgrList.FindAll(f => f.Name() == "__spellTuiles" && (f as Rec).brush != CommonCode.spellAreaNotAllowedColor && (f as Rec).brush != CommonCode.spellAreaAllowedColor);
                for (int cnt = 0;   cnt < __spellTuiles.Count; cnt++)
                    (__spellTuiles[cnt] as Rec).brush = (__spellTuiles[cnt] as Rec).tag as Brush;

                // supression des tuiles supplémentaires pour les sorts de zones
                Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles_child");
            }
            #endregion
        }
        public static void cleanMapFromSpellTiles()
        {
            #region
            if (Battle.currentCursor != "")
            {
                List<IGfx> __spellTuiles = Manager.manager.GfxBgrList.FindAll(f => f.Name() == "__spellTuiles");
                for (int cnt = __spellTuiles.Count; cnt > 0; cnt--)
                {
                    (__spellTuiles[cnt - 1] as Rec).visible = false;
                    Manager.manager.GfxBgrList.Remove(__spellTuiles[cnt - 1]);
                }

                // si le sort spellID4 kage bunshin no jutso a été joué qui a laissé un clone affiché
                IGfx __clone_jutsu2 = Manager.manager.GfxObjList.Find(f => f.Name() == "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto");
                if (__clone_jutsu2 != null)
                {
                    (__clone_jutsu2 as Bmp).visible = false;
                    Manager.manager.GfxObjList.Remove(__clone_jutsu2);
                }
            }
            #endregion
        }
        static void wayPointRec_MouseClic(Rec rec, MouseEventArgs e)
        {
            #region
            IGfx map = Manager.manager.GfxBgrList.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).name == "__map");
            map_MouseClic((map as Bmp), e);
            #endregion
        }
        static void wayPointRec_MouseMove(Rec rec, MouseEventArgs e)
        {
            #region
            // affichage du chemain que le joueur aimerai prendre en positionant la souris sur le map dépondament des point de mouvement
            // methode qui supprime l'ancien waypoint tracé et le retrace pour netiyer l'ecrans
            DelWayPointCallBack isFreeCell = rec.tag as DelWayPointCallBack;
            if (isFreeCell(e.Location))
                CursorHand_MouseMove(null, null);
            else
                CursorDefault_MouseOut(null, null);

            // si le joueur est en combat, on trace le chemain que l'utilisateur veux choisir
            // avant cela on nétoie l'ecran des anciens cases colorés par le chemain

            // effacement de tous les chemain tracés avant
            if (Manager.manager.GfxBgrList.FindAll(f => f.Name() == "__wayPointRec").Count > 0)
            {
                (Manager.manager.GfxBgrList.Find(f => f.Name() == "__wayPointRec") as Rec).visible = false;
                Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__wayPointRec");
            }

            // on dessine le chemain, si on ai on combat, si on ai en statut Started, si on a la main
            if (Battle.state == Enums.battleState.state.started && Battle.PlayerTurn == MyPlayerInfo.instance.pseudo)
            {
                // generer un waypoint
                /////////////// pathfinding   ////////////////////
                List<Point> wayPointList = new List<Point>();
                MapPoint startPoint = new MapPoint(MyPlayerInfo.instance.ibPlayer.point.X / 30, (MyPlayerInfo.instance.ibPlayer.point.Y + CommonCode.MyPlayerInfo.instance.ibPlayer.rectangle.Height) / 30);
                MapPoint endPoint = new MapPoint(e.Location.X / 30, e.Location.Y / 30);
                byte[,] byteMap = new byte[ScreenManager.TilesWidth, ScreenManager.TilesHeight];
                for (int i = 0; i < ScreenManager.TilesWidth; i++)
                    for (int j = 0; j < ScreenManager.TilesHeight; j++)
                    {
                        if (!isFreeCell(new Point(i * 30, j * 30)))
                            byteMap[i, j] = 3;
                        else
                            byteMap[i, j] = 0;
                    }

                Map _map = new Map(ScreenManager.TilesWidth, ScreenManager.TilesHeight, startPoint, endPoint, byteMap);

                if (_map != null && _map.StartPoint != MapPoint.InvalidPoint && _map.EndPoint != MapPoint.InvalidPoint)
                {
                    AStar astart = new AStar(_map);
                    List<MapPoint> sol = astart.CalculateBestPath();
                    if (sol != null)
                        sol.Reverse();
                    else
                    {
                        // impossible de determiner le chemain, peux etre que la case ciblé est un obstacle
                        CommonCode.ChatMsgFormat("S", null, CommonCode.TranslateText(118));
                        return;
                    }
                    // conversion de la liste MapPoint a une liste Point
                    for (int i = 0; i < sol.Count; i++)
                        wayPointList.Add(new Point(sol[i].X * 30, sol[i].Y * 30));
                }
                ////////////////////////////////////////////
                if (wayPointList.Count > 0 && wayPointList.Count <= Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).currentPm)
                {
                    // check si notre waypoint se croise avec l'une des position des joueur dans le combat
                    bool blocked = false;           // variable qui nous permet de savoir si un joueur tacle une case pour que tous les autres cases soit en rouge
                    Brush brushes = Brushes.Green;

                    // premiere comparésons avec la position actuel
                    ///////////////////////////////////////////////////////////
                    // check contre le blocage pour la case actuel
                    // check si le rectangle en cours est entouré par un joueur ou invoc
                    // tourné sur tous les objets et joueur dans la liste
                    int cumuleBlocage1 = 0;
                    Point[] validePos1 = new Point[4];
                    Point curPlayerPos1 = Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).realPosition;
                    validePos1[0] = new Point(curPlayerPos1.X, curPlayerPos1.Y - 1);    // case en haut
                    validePos1[1] = new Point(curPlayerPos1.X + 1, curPlayerPos1.Y);    // case a droite
                    validePos1[2] = new Point(curPlayerPos1.X, curPlayerPos1.Y + 1);    // case en bas
                    validePos1[3] = new Point(curPlayerPos1.X - 1, curPlayerPos1.Y);    // case a gauche

                    // check si un joueur se trouve sur l'une des cases cités en haut
                    for (int cnt1 = 0; cnt1 < Battle.AllPlayersByOrder.Count; cnt1++)
                    {
                        if (Battle.AllPlayersByOrder[cnt1].teamSide != Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).teamSide && Battle.AllPlayersByOrder[cnt1].visible)
                        {
                            Actor pi = Battle.AllPlayersByOrder[cnt1];
                            Point p = new Point(Battle.AllPlayersByOrder[cnt1].realPosition.X, Battle.AllPlayersByOrder[cnt1].realPosition.Y);
                            if ((p.X == validePos1[0].X && p.Y == validePos1[0].Y) || (p.X == validePos1[1].X && p.Y == validePos1[1].Y) || (p.X == validePos1[2].X && p.Y == validePos1[2].Y) || (p.X == validePos1[3].X && p.Y == validePos1[3].Y))
                                cumuleBlocage1 += pi.blocage;
                        }
                    }
                    bool alreadyBlocked = false;
                    if (cumuleBlocage1 > Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).escape + 10)
                    {
                        brushes = Brushes.Red;
                        alreadyBlocked = true;
                    }
                    ////////////////////////////////////

                    // tracage du chemain
                    for (int cntWayPoint = 0; cntWayPoint < wayPointList.Count(); cntWayPoint++)
                    {
                        if (!isFreeCell(new Point(wayPointList[cntWayPoint].X, wayPointList[cntWayPoint].Y)))
                            break;
                        else
                        {
                            // check si le rectangle en cours est entouré par un joueur ou invoc
                            // tourné sur tous les objets et joueur dans la liste
                            int cumuleBlocage = cumuleBlocage1;

                            if (!alreadyBlocked)
                            {
                                Point[] validePos = new Point[4];
                                validePos[0] = new Point(wayPointList[cntWayPoint].X, wayPointList[cntWayPoint].Y - 30);    // case en haut
                                validePos[1] = new Point(wayPointList[cntWayPoint].X + 30, wayPointList[cntWayPoint].Y);    // case a droite
                                validePos[2] = new Point(wayPointList[cntWayPoint].X, wayPointList[cntWayPoint].Y + 30);    // case en bas
                                validePos[3] = new Point(wayPointList[cntWayPoint].X - 30, wayPointList[cntWayPoint].Y);    // case a gauche

                                // check si on ai bloqué par un joueur
                                for (int cnt = 0; cnt < Battle.AllPlayersByOrder.Count; cnt++)
                                {
                                    if (Battle.AllPlayersByOrder[cnt].teamSide != Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).teamSide && Battle.AllPlayersByOrder[cnt].visible)
                                    {
                                        Actor piib = Battle.AllPlayersByOrder[cnt];
                                        Point p = new Point((piib.ibPlayer.point.X - 15 + piib.ibPlayer.rectangle.Width), piib.ibPlayer.point.Y + piib.ibPlayer.rectangle.Height);
                                        p.X = (p.X / 30) * 30;
                                        p.Y = (p.Y / 30) * 30;
                                        Point p2 = piib.realPosition;
                                        if (p == validePos[0] || p == validePos[1] || p == validePos[2] || p == validePos[3])
                                            cumuleBlocage += piib.blocage;
                                    }
                                }
                            }

                            Rec wayPointRec = new Rec(brushes, new Point(wayPointList[cntWayPoint].X + 1, wayPointList[cntWayPoint].Y + 1), new Size(28, 28), "__wayPointRec", Manager.TypeGfx.Bgr, true);
                            wayPointRec.tag = isFreeCell;
                            wayPointRec.MouseClic += wayPointRec_MouseClic;
                            wayPointRec.MouseMove += wayPointRec_MouseMove;
                            wayPointRec.EscapeGfxWhileMouseClic = true;
                            Manager.manager.GfxBgrList.Add(wayPointRec);

                            Txt wayPointTxt = new Txt((cntWayPoint + 1).ToString(), new Point(0, 10), "__waypointTxt", Manager.TypeGfx.Bgr, true, new Font("verdana", 6), Brushes.Black);
                            wayPointTxt.point.X = 15 - (TextRenderer.MeasureText(wayPointTxt.Text, wayPointTxt.font).Width / 2);
                            wayPointRec.Child.Add(wayPointTxt);

                            // determination si le cumuleBlocage est plus grand que celui de notre joueur, on ajoute 10 points d'evasion a notre joueur comme tolerance
                            if (cumuleBlocage > Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).escape + 10)
                            {
                                brushes = Brushes.Red;
                                blocked = true;
                            }
                            else if (!blocked)
                                brushes = Brushes.Green;
                        }
                    }
                }
            }
            #endregion
        }
        public static void showAnimDamageAbove(Point playerPoint, Rectangle playerRec, int playerIndex, string domString)
        {
            #region affichage des dom ... en haut du personnage
            // diminution des pv du joueur roxé
            if (domString != "null")
            {
                string[] DomString = domString.Split('|');
                string typeRox = DomString[0].Split(':')[1];
                // on mémorises certaines donénes de l'objet playerTargeted puisque cette objet va se détruire quand le joueurs sera mort, du coup il s'éfface de la liste des joueurs et il deviens accessible et provoque une erreur

                if (typeRox == "rox")
                {
                    bool cd = Convert.ToBoolean(DomString[2].Split(':')[1]);
                    int dom = Convert.ToInt32(DomString[4].Split(':')[1]);

                    Txt __labelOfDom = new Txt(MoneyThousendSeparation(dom.ToString()), Point.Empty, "__labelOfDom", Manager.TypeGfx.Obj, true, new Font("Arial Black", 16, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 205, 69, 25)));
                    __labelOfDom.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfDom.Text, __labelOfDom.font).Width / 2);
                    __labelOfDom.point.Y = (playerPoint.Y * 30) - playerRec.Height - 10;
                    __labelOfDom.zindex = playerIndex;
                    Manager.manager.GfxObjList.Add(__labelOfDom);

                    // animation pour afficher le text des dom
                    int counter = 0;
                    int opacity = 255;
                    int fontsize = 16;
                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        if (counter > 25)
                        {
                            __labelOfDom.visible = false;
                            Manager.manager.GfxObjList.Remove(__labelOfDom);
                            break;
                        }
                        else
                        {
                            counter++;
                            __labelOfDom.point.Y -= 2;

                            if (counter > 20)
                            {
                                if (fontsize <= 1)
                                    fontsize = 1;
                                else
                                    fontsize--;

                                if (opacity < 6)
                                    opacity = 0;
                                else
                                    opacity -= 25;
                            }

                            __labelOfDom.font = new Font("Arial Black", fontsize, FontStyle.Bold);
                            __labelOfDom.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfDom.Text, __labelOfDom.font).Width / 2);
                            __labelOfDom.brush = new SolidBrush(Color.FromArgb(opacity, 205, 69, 25));
                            Thread.Sleep(50);
                        }
                    }
                }
            }
            #endregion
        }
        public static void showAnimUsedPC(int UsedPC, Point playerPoint, Rectangle playerRec, int playerIndex)
        {
            #region animation des PC utilisés lors d'un sort au dessus du lanceur
            // animation pour afficher le text des pc utilisés

            Txt __labelOfUsedPC = new Txt("-" + UsedPC.ToString(), Point.Empty, "__labelOfUsedPC_" + playerPoint.X + "-" + playerPoint.Y, Manager.TypeGfx.Obj, true, new Font("Arial Black", 12, FontStyle.Bold), Brushes.Blue);
            __labelOfUsedPC.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfUsedPC.Text, __labelOfUsedPC.font).Width / 2);
            __labelOfUsedPC.point.Y = (playerPoint.Y * 30) - playerRec.Height - 10;
            __labelOfUsedPC.zindex = playerIndex;
            Manager.manager.GfxObjList.Add(__labelOfUsedPC);

            int counter = 0;
            int opacity = 255;
            int fontsize = 12;
            while (!Manager.manager.mainForm.IsDisposed)
            {
                if (counter > 25)
                {
                    __labelOfUsedPC.visible = false;
                    Manager.manager.GfxObjList.RemoveAll(f => f.Name() == "__labelOfUsedPC_" + playerPoint.X + "-" + playerPoint.Y);

                    // libération des cmd en instance
                    CommonCode.blockNetFlow = false;
                    CommonCode.ChatMsgFormat("S", "null", "blockNetFlow5 = false");
                    break;
                }
                else
                {
                    counter++;
                    __labelOfUsedPC.point.Y -= 2;

                    if (counter > 20)
                    {
                        if (fontsize <= 1)
                            fontsize = 1;
                        else
                            fontsize--;

                        if (opacity < 6)
                            opacity = 0;
                        else
                            opacity -= 25;
                    }

                    __labelOfUsedPC.font = new Font("Arial Black", fontsize, FontStyle.Bold);
                    __labelOfUsedPC.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfUsedPC.Text, __labelOfUsedPC.font).Width / 2);
                    __labelOfUsedPC.brush = Brushes.Blue;
                    Thread.Sleep(50);
                }
            }
            #endregion
        }
        public static void showAnimUsedPm(int UsedPC, Point playerPoint, Rectangle playerRec, int playerIndex)
        {
            #region animation pour afficher le text des pm utilisés

            Txt __labelOfUsedPM = new Txt("-" + UsedPC.ToString(), Point.Empty, "__labelOfUsedPM", Manager.TypeGfx.Obj, true, new Font("Arial Black", 12, FontStyle.Bold), Brushes.Green);
            __labelOfUsedPM.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfUsedPM.Text, __labelOfUsedPM.font).Width / 2);
            __labelOfUsedPM.point.Y = (playerPoint.Y * 30) - playerRec.Height - 10;
            __labelOfUsedPM.zindex = playerIndex;
            Manager.manager.GfxObjList.Add(__labelOfUsedPM);

            int counter = 0;
            int opacity = 255;
            int fontsize = 12;
            while (!Manager.manager.mainForm.IsDisposed)
            {
                if (counter > 25)
                {
                    __labelOfUsedPM.visible = false;
                    Manager.manager.GfxObjList.Remove(__labelOfUsedPM);

                    // libération des cmd en instance
                    CommonCode.blockNetFlow = false;
                    CommonCode.ChatMsgFormat("S", "null", "blockNetFlow6 = false");
                    break;
                }
                else
                {
                    counter++;
                    __labelOfUsedPM.point.Y -= 2;

                    if (counter > 20)
                    {
                        if (fontsize <= 1)
                            fontsize = 1;
                        else
                            fontsize--;

                        if (opacity < 6)
                            opacity = 0;
                        else
                            opacity -= 25;
                    }

                    __labelOfUsedPM.font = new Font("Arial Black", fontsize, FontStyle.Bold);
                    __labelOfUsedPM.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfUsedPM.Text, __labelOfUsedPM.font).Width / 2);
                    __labelOfUsedPM.brush = Brushes.Green;
                    Thread.Sleep(50);
                }
            }
            #endregion
        }
        public static void showAnimUpgradeVita(int UpgradeVita, Point playerPoint, Rectangle playerRec, int playerIndex)
        {
            #region animation des PC utilisés lors d'un sort au dessus du lanceur
            // animation pour afficher le text des pc utilisés

            Txt __labelOfUsedPC = new Txt("+" + UpgradeVita.ToString(), Point.Empty, "__labelOfUsedPC_" + playerPoint.X + "-" + playerPoint.Y, Manager.TypeGfx.Obj, true, new Font("Arial Black", 12, FontStyle.Bold), Brushes.DarkRed);
            __labelOfUsedPC.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfUsedPC.Text, __labelOfUsedPC.font).Width / 2);
            __labelOfUsedPC.point.Y = (playerPoint.Y * 30) - playerRec.Height - 10;
            __labelOfUsedPC.zindex = playerIndex;
            Manager.manager.GfxObjList.Add(__labelOfUsedPC);

            int counter = 0;
            int opacity = 255;
            int fontsize = 18;
            while (!Manager.manager.mainForm.IsDisposed)
            {
                if (counter > 25)
                {
                    __labelOfUsedPC.visible = false;
                    Manager.manager.GfxObjList.RemoveAll(f => f.Name() == "__labelOfUsedPC_" + playerPoint.X + "-" + playerPoint.Y);

                    // libération des cmd en instance
                    CommonCode.blockNetFlow = false;
                    CommonCode.ChatMsgFormat("S", "null", "blockNetFlow1 = false");
                    break;
                }
                else
                {
                    counter++;
                    __labelOfUsedPC.point.Y -= 2;

                    if (counter > 20)
                    {
                        if (fontsize <= 1)
                            fontsize = 1;
                        else
                            fontsize--;

                        if (opacity < 6)
                            opacity = 0;
                        else
                            opacity -= 25;
                    }

                    __labelOfUsedPC.font = new Font("Arial Black", fontsize, FontStyle.Bold);
                    __labelOfUsedPC.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfUsedPC.Text, __labelOfUsedPC.font).Width / 2);
                    __labelOfUsedPC.brush = Brushes.DarkRed;
                    Thread.Sleep(50);
                }
            }
            #endregion
        }
        public static void showAnimUpgrade(int UpgradePm, Point playerPoint, Rectangle playerRec, int playerIndex, string element)
        {
            #region animation qui affiche le bonnus ou malus affiché au dessus de la tête du concerné

            Brush brush = Brushes.Black;

            // il faut ajouter une enum States ou features qui contiens tous les caractéristiques, et les elements (chakra) sont déjà dans les enums
            if (element == "pc")
                brush = Brushes.Blue;
            else if (element == "pm")
                brush = Brushes.Green;
            else if (element == "puissance")
                brush = CommonCode.puissanceColor;
            else if (element == Enums.Chakra.Element.doton.ToString())
                brush = CommonCode.dotonColor;
            else if (element == Enums.Chakra.Element.katon.ToString())
                brush = CommonCode.katonColor;
            else if (element == Enums.Chakra.Element.futon.ToString())
                brush = CommonCode.futonColor;
            else if (element == Enums.Chakra.Element.raiton.ToString())
                brush = CommonCode.raitonColor;
            else if (element == Enums.Chakra.Element.suiton.ToString())
                brush = CommonCode.suitonColor;

            Txt __labelOfUsedPm = new Txt("+" + UpgradePm.ToString(), Point.Empty, "__labelOfUsedPm_" + playerPoint.X + "-" + playerPoint.Y, Manager.TypeGfx.Obj, true, new Font("Arial Black", 12, FontStyle.Bold), brush);
            __labelOfUsedPm.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfUsedPm.Text, __labelOfUsedPm.font).Width / 2);
            __labelOfUsedPm.point.Y = (playerPoint.Y * 30) - playerRec.Height - 10;
            __labelOfUsedPm.zindex = playerIndex;
            Manager.manager.GfxObjList.Add(__labelOfUsedPm);

            int counter = 0;
            int opacity = 255;
            int fontsize = 18;
            while (!Manager.manager.mainForm.IsDisposed)
            {
                if (counter > 25)
                {
                    __labelOfUsedPm.visible = false;
                    Manager.manager.GfxObjList.RemoveAll(f => f.Name() == "__labelOfUsedPm_" + playerPoint.X + "-" + playerPoint.Y);

                    // libération des cmd en instance
                    CommonCode.blockNetFlow = false;
                    CommonCode.ChatMsgFormat("S", "null", "blockNetFlow2 = false");
                    break;
                }
                else
                {
                    counter++;
                    __labelOfUsedPm.point.Y -= 2;

                    if (counter > 20)
                    {
                        if (fontsize <= 1)
                            fontsize = 1;
                        else
                            fontsize--;

                        if (opacity < 6)
                            opacity = 0;
                        else
                            opacity -= 25;
                    }

                    __labelOfUsedPm.font = new Font("Arial Black", fontsize, FontStyle.Bold);
                    __labelOfUsedPm.point.X = (playerPoint.X * 30) + 15 - (TextRenderer.MeasureText(__labelOfUsedPm.Text, __labelOfUsedPm.font).Width / 2);
                    __labelOfUsedPm.brush = brush;
                    Thread.Sleep(50);
                }
            }
            #endregion
        }
        public static void animDeadPlayer(Bmp ibPlayer)
        {
            #region simuler la mort d'un joueur
            ibPlayer.visible = false;
            
            if (Battle.state == Enums.battleState.state.started)
            {
                if (Battle.AllPlayersByOrder.Exists(f => f.pseudo == (ibPlayer.tag as Actor).pseudo))
                    Battle.AllPlayersByOrder.Find(f => f.pseudo == (ibPlayer.tag as Actor).pseudo).isAlive = false;
                else if (Battle.DeadPlayers.Exists(f => f.pseudo == (ibPlayer.tag as Actor).pseudo))
                {
                    Actor deadPlayer = Battle.DeadPlayers.Find(f => f.pseudo == (ibPlayer.tag as Actor).pseudo);
                    deadPlayer.isAlive = false;
                }
            }
            #endregion
        }
        public static List<PointF> calculeTrajectoire(PointF depart, PointF cible, int step)
        {
            #region retourne les points entre 2 position
            List<PointF> points = new List<PointF>();
            PointF distance = new PointF(cible.X - depart.X, cible.Y - depart.Y);
            for (int i = 1; i < step; i++)
                points.Add(new PointF(depart.X + (distance.X / step * i), depart.Y + (distance.Y / step * i)));
            return points;
            #endregion
        }
        public static void LaunchAcceptChallenge(Actor p1)
        {

        }
        public static void CancelChallengeRespond(string player)
        {
            #region
            // le jouer qui va etre défier annule cette demande
            if (CommonCode.ChallengeTo == player)
            {
                CommonCode.annulerChallengeHimDlg.visible = false;
                Manager.manager.GfxTopList.Remove(CommonCode.annulerChallengeHimDlg);
                CommonCode.annulerChallengeHimDlg = null;
                CommonCode.ChallengeTo = "";
            }
            #endregion
        }
        public static void CancelChallengeAsking(string player)
        {
            #region
            // le jouer qui veux défier annule cette demande
            if (CommonCode.ChallengeTo == player)
            {
                // supression du case a coché
                Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0].Visible = false;
                Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0] = null;
                Manager.manager.mainForm.Controls.Remove(Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0]);

                // supression du parent
                CommonCode.annulerChallengeMeDlg.visible = false;
                CommonCode.annulerChallengeMeDlg.Child.Clear();

                // effacement du recparent de la liste graphics
                Manager.manager.GfxTopList.Remove(CommonCode.annulerChallengeMeDlg);
                CommonCode.annulerChallengeMeDlg = null;

                CommonCode.ChallengeTo = "";
            }
            #endregion
        }
    }
}