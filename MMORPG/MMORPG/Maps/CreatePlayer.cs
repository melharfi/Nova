using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using MELHARFI;
using MELHARFI.Gfx;
using MMORPG.Net.Messages.Request;

namespace MMORPG.GameStates
{
    public class CreatePlayer : IGameState
    {
        delegate void DelHandel(string s);   // delegate pour faire du cross threading
        public Enums.ActorClass.ClassName selected_Class;
        public Enums.HiddenVillage.Names selectedHiddenVillage;
        Txt tPlayerName, playerName, paysName, paysDescription, bonusPays;
        Bmp dockdown, ibValider, ibPlayers, terrain, illustration, no_color_1, no_color_2, no_color_3, refresh;
        TextBox name = new TextBox();
        Rec infosPlayer, cadre_Select_Player, cadre_Select_Pays, cadre_Select_Color1, cadre_Select_Color2, cadre_Select_Color3;
        Point[] classes_thumbs_points = new Point[8], pays_thumbs_points = new Point[5];
        ColorDialog cd = new ColorDialog();

        public CreatePlayer()
        {
            
        }
        public void Init()
        {
            cd.FullOpen = true;
            // arriere plant image
            Bmp map = new Bmp(@"gfx\map\CreatePlayer\bg.dat", new Point(0, 0), "map", Manager.TypeGfx.Bgr, true, 1);
            Manager.manager.GfxBgrList.Add(map);

            // dessin du dock-up Creer un personnage
            Bmp banniere1 = new Bmp(@"gfx\map\CreatePlayer\banniere1.dat", new Point(251, 0), "banniere1", Manager.TypeGfx.Bgr, true, 1);
            banniere1.point = new Point((ScreenManager.WindowWidth / 2) - (banniere1.rectangle.Width / 2));
            Manager.manager.GfxBgrList.Add(banniere1);

            // label creer un joueur
            Txt lcreatePlayer = new Txt(CommonCode.TranslateText(19), Point.Empty, "lcreatePlayer", Manager.TypeGfx.Obj, true, new Font("Verdana", 12), Brushes.White);
            lcreatePlayer.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(lcreatePlayer.Text, lcreatePlayer.font).Width / 2);
            Manager.manager.GfxObjList.Add(lcreatePlayer);

            // dessin d'un dock-down comme menu des joueurs
            dockdown = new Bmp(@"gfx\map\CreatePlayer\dock_down.dat", new Point(0, ScreenManager.WindowHeight - 164), "dockdown", Manager.TypeGfx.Obj, true, 1);
            
            Manager.manager.GfxObjList.Add(dockdown);

            Txt namePlayer = new Txt(CommonCode.TranslateText(25), new Point(0, 400), "namePlayer", Manager.TypeGfx.Obj, true, new Font("Verdana", 10), Brushes.Red);
            namePlayer.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(namePlayer.Text, namePlayer.font).Width / 2);
            Manager.manager.GfxObjList.Add(namePlayer);

            tPlayerName = new Txt(string.Empty, new Point(350, 410), "tPlayerName", Manager.TypeGfx.Obj, true, new Font("Verdana", 20), Brushes.Red);
            Manager.manager.GfxObjList.Add(tPlayerName);

            name.Name = "name";
            name.Size = new Size(100, 20);
            name.Location = new Point(350, 420);
            name.TabIndex = ZOrder.Obj();
            name.Focus();
            name.BackColor = Color.DarkSlateGray;
            name.ForeColor = Color.White;
            Manager.manager.GfxCtrlList.Add(name);
            
            Manager.manager.mainForm.Controls.Add(name);

            ibValider = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(320, 500), "ibRetour", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 9));
            ibValider.MouseDown += ibRetour_MouseDown;
            ibValider.MouseUp += ibRetour_MouseUp;
            ibValider.MouseClic += ibValider_MouseClic;
            ibValider.MouseOver += HandCursor;
            ibValider.MouseOut += DefaultCursor;
            Manager.manager.GfxObjList.Add(ibValider);

            Txt lValider = new Txt(CommonCode.TranslateText(26), new Point(5, 505), "lsubmit", Manager.TypeGfx.Obj, true, new Font("Verdana", 10), Brushes.FloralWhite);
            lValider.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(lValider.Text, lValider.font).Width / 2);
            lValider.MouseClic += lValider_MouseClic;
            Manager.manager.GfxObjList.Add(lValider);

            Rec playersTab = new Rec(Brushes.Chocolate, new Point(30, 50), new Size(233, 119), "playerTab", Manager.TypeGfx.Obj, true);
            Manager.manager.GfxObjList.Add(playersTab);

            cadre_Select_Player = new Rec(Brushes.DarkRed, new Point(3, 3), new Size(56, 56), "cadre_Select_Player", Manager.TypeGfx.Obj, true);
            playersTab.Child.Add(cadre_Select_Player);

            classes_thumbs_points[0] = new Point(3, 3);
            classes_thumbs_points[1] = new Point(60, 3);
            classes_thumbs_points[2] = new Point(117, 3);
            classes_thumbs_points[3] = new Point(174, 3);

            classes_thumbs_points[4] = new Point(3, 60);
            classes_thumbs_points[5] = new Point(60, 60);
            classes_thumbs_points[6] = new Point(117, 60);
            classes_thumbs_points[7] = new Point(174, 60);

            // ---- 1ere ligne
            Bmp naruto_thumb = new Bmp(@"gfx\map\CreatePlayer\obj\class_thumbs\" + Enums.ActorClass.ClassName.naruto + ".dat", new Point(5, 5), Enums.ActorClass.ClassName.naruto.ToString(), Manager.TypeGfx.Obj, true, 1);
            naruto_thumb.MouseOver += HandCursor;
            naruto_thumb.MouseOut += DefaultCursor;
            naruto_thumb.MouseClic += classe_thumb_MouseClic;
            playersTab.Child.Add(naruto_thumb);

            Bmp choji_thumb = new Bmp(@"gfx\map\CreatePlayer\obj\class_thumbs\" + Enums.ActorClass.ClassName.choji + ".dat", new Point(62, 5), Enums.ActorClass.ClassName.choji.ToString(), Manager.TypeGfx.Obj, true, 1);
            choji_thumb.MouseOver += HandCursor;
            choji_thumb.MouseOut += DefaultCursor;
            choji_thumb.MouseClic += classe_thumb_MouseClic;
            playersTab.Child.Add(choji_thumb);

            Bmp kabuto_thumb = new Bmp(@"gfx\map\CreatePlayer\obj\class_thumbs\" + Enums.ActorClass.ClassName.kabuto + ".dat", new Point(119, 5), Enums.ActorClass.ClassName.kabuto.ToString(), Manager.TypeGfx.Obj, true, 1);
            kabuto_thumb.MouseOver += HandCursor;
            kabuto_thumb.MouseOut += DefaultCursor;
            kabuto_thumb.MouseClic += classe_thumb_MouseClic;
            playersTab.Child.Add(kabuto_thumb);

            Bmp ino_thumb = new Bmp(@"gfx\map\CreatePlayer\obj\class_thumbs\" + Enums.ActorClass.ClassName.ino + ".dat", new Point(176, 5), Enums.ActorClass.ClassName.ino.ToString(), Manager.TypeGfx.Obj, true, 1);
            ino_thumb.MouseOver += HandCursor;
            ino_thumb.MouseOut += DefaultCursor;
            ino_thumb.MouseClic += classe_thumb_MouseClic;
            playersTab.Child.Add(ino_thumb);

            //-----------
            Bmp lee_thumb = new Bmp(@"gfx\map\CreatePlayer\obj\class_thumbs\" + Enums.ActorClass.ClassName.lee + ".dat", new Point(5, 62), Enums.ActorClass.ClassName.lee.ToString(), Manager.TypeGfx.Obj, true, 1);
            //lee_thumb.MouseOver += classe_thumb_MouseOver;
            //lee_thumb.MouseOut += classe_thumb_MouseOut;
            //lee_thumb.MouseClic += classe_thumb_MouseClic;
            playersTab.Child.Add(lee_thumb);

            Bmp kankura_thumb = new Bmp(@"gfx\map\CreatePlayer\obj\class_thumbs\" + Enums.ActorClass.ClassName.kankura + ".dat", new Point(62, 62), Enums.ActorClass.ClassName.kankura.ToString(), Manager.TypeGfx.Obj, true, 1);
            //kankura_thumb.MouseOver += classe_thumb_MouseOver;
            //kankura_thumb.MouseOut += classe_thumb_MouseOut;
            //kankura_thumb.MouseClic += classe_thumb_MouseClic;
            playersTab.Child.Add(kankura_thumb);

            Bmp shikamaru_thumb = new Bmp(@"gfx\map\CreatePlayer\obj\class_thumbs\" + Enums.ActorClass.ClassName.shikamaru + ".dat", new Point(119, 62), Enums.ActorClass.ClassName.shikamaru.ToString(), Manager.TypeGfx.Obj, true, 1);
            //shikamaru_thumb.MouseOver += classe_thumb_MouseOver;
            //shikamaru_thumb.MouseOut += classe_thumb_MouseOut;
            //shikamaru_thumb.MouseClic += classe_thumb_MouseClic;
            playersTab.Child.Add(shikamaru_thumb);

            Bmp sakura_thumb = new Bmp(@"gfx\map\CreatePlayer\obj\class_thumbs\" + Enums.ActorClass.ClassName.sakura + ".dat", new Point(176, 62), Enums.ActorClass.ClassName.sakura.ToString(), Manager.TypeGfx.Obj, true, 1);
            //sakura_thumb.MouseOver += classe_thumb_MouseOver;
            //sakura_thumb.MouseOut += classe_thumb_MouseOut;
            //sakura_thumb.MouseClic += classe_thumb_MouseClic;
            playersTab.Child.Add(sakura_thumb);

            terrain = new Bmp(@"gfx\map\SelectPlayer\obj\2.dat", new Point(0, 350), "terrain", Manager.TypeGfx.Obj, true, 1);
            terrain.point.X = (ScreenManager.WindowWidth / 2) - (terrain.rectangle.Width / 2);
            Manager.manager.GfxObjList.Add(terrain);

            // image du personnage
            ibPlayers = new Bmp(@"gfx\general\classes\" + Enums.ActorClass.ClassName.naruto + ".dat", new Point(0, 0), 1);
            ibPlayers.name = "ibPlayers";
            ibPlayers.zindex = ZOrder.Obj();
            ibPlayers.visible = false;
            ibPlayers.Crypted = true;
            Manager.manager.GfxObjList.Add(ibPlayers);

            // image de l'illustration
            illustration = new Bmp(@"gfx\general\classes\illustrations\" + Enums.ActorClass.ClassName.naruto + ".dat", new Point(0, 0), 1);
            illustration.name = "illustration";
            illustration.zindex = ZOrder.Obj();
            illustration.visible = false;
            illustration.Crypted = true;
            Manager.manager.GfxObjList.Add(illustration);

            infosPlayer = new Rec(Brushes.CadetBlue, new Point((ScreenManager.WindowWidth / 2) - 75, 50), new Size(180, 100), "infosPlayer", Manager.TypeGfx.Obj, true);
            Manager.manager.GfxObjList.Add(infosPlayer);

            playerName = new Txt("", new Point(5, 5), "playerName", Manager.TypeGfx.Obj, true, new Font("Verdana", 9), Brushes.White);
            infosPlayer.Child.Add(playerName);

            ///////////////// pays tab
            Rec paysTab = new Rec(Brushes.NavajoWhite, new Point(30, 300), new Size(240, 52), "paysTab", Manager.TypeGfx.Obj, true);
            Manager.manager.GfxObjList.Add(paysTab);

            paysName = new Txt("", new Point(5, 20), "paysName", Manager.TypeGfx.Obj, true, new Font("Verdana", 9), Brushes.White);
            infosPlayer.Child.Add(paysName);

            cadre_Select_Pays = new Rec(Brushes.DarkRed, new Point(191, 3), new Size(46, 46), "cadre_Select_Pays", Manager.TypeGfx.Obj, true);
            paysTab.Child.Add(cadre_Select_Pays);

            pays_thumbs_points[0] = new Point(3, 3);
            pays_thumbs_points[1] = new Point(50, 3);
            pays_thumbs_points[2] = new Point(97, 3);
            pays_thumbs_points[3] = new Point(144, 3);
            pays_thumbs_points[4] = new Point(191, 3);

            ////////////// pays thumbs
            Bmp konoha = new Bmp(@"gfx\general\obj\1\pays.dat", new Point(5, 5), Enums.HiddenVillage.Names.konoha.ToString(), Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("pays_konoha", 0));
            konoha.MouseOver += HandCursor;
            konoha.MouseOut += DefaultCursor;
            konoha.MouseClic += pays_MouseClic;
            paysTab.Child.Add(konoha);

            Bmp iwa = new Bmp(@"gfx\general\obj\1\pays.dat", new Point(52, 5), Enums.HiddenVillage.Names.iwa.ToString(), Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("pays_iwa", 0));
            iwa.MouseOver += HandCursor;
            iwa.MouseOut += DefaultCursor;
            iwa.MouseClic += pays_MouseClic;
            paysTab.Child.Add(iwa);

            Bmp kiri = new Bmp(@"gfx\general\obj\1\pays.dat", new Point(99, 5), Enums.HiddenVillage.Names.kiri.ToString(), Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("pays_kiri", 0));
            kiri.MouseOver += HandCursor;
            kiri.MouseOut += DefaultCursor;
            kiri.MouseClic += pays_MouseClic;
            paysTab.Child.Add(kiri);

            Bmp kumo = new Bmp(@"gfx\general\obj\1\pays.dat", new Point(146, 5), Enums.HiddenVillage.Names.kumo.ToString(), Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("pays_kumo", 0));
            kumo.MouseOver += HandCursor;
            kumo.MouseOut += DefaultCursor;
            kumo.MouseClic += pays_MouseClic;
            paysTab.Child.Add(kumo);

            Bmp suna = new Bmp(@"gfx\general\obj\1\pays.dat", new Point(193, 5), Enums.HiddenVillage.Names.suna.ToString(), Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("pays_suna", 0));
            suna.MouseOver += HandCursor;
            suna.MouseOut += DefaultCursor;
            suna.MouseClic += pays_MouseClic;
            paysTab.Child.Add(suna);
            ///////////////////////////////////////////////
            
            /////////////// affichage des bonus liés au pays selectionnée
            bonusPays = new Txt("", new Point(10, 37), "bonusPays", Manager.TypeGfx.Obj, true, new Font("Verdana", 9), Brushes.White);
            infosPlayer.Child.Add(bonusPays);
            ////////////////////////////////////

            /////////// description du pays
            paysDescription = new Txt("", new Point(paysTab.point.X, paysTab.point.Y + paysTab.size.Height + 10), "paysDescription", Manager.TypeGfx.Obj, true, new Font("Verdana", 9), Brushes.Black);
            Manager.manager.GfxObjList.Add(paysDescription);
            //////////////////////////////

            ///////// selection de couleurs
            Txt partie1 = new Txt(CommonCode.TranslateText(64), new Point(50, 200), "partie1", Manager.TypeGfx.Obj, true, new Font("Verdana", 9), Brushes.Red);
            Manager.manager.GfxObjList.Add(partie1);

            Txt partie2 = new Txt(CommonCode.TranslateText(65), new Point(50, 220), "partie2", Manager.TypeGfx.Obj, true, new Font("Verdana", 9), Brushes.Red);
            Manager.manager.GfxObjList.Add(partie2);

            Txt partie3 = new Txt(CommonCode.TranslateText(66), new Point(50, 240), "partie3", Manager.TypeGfx.Obj, true, new Font("Verdana", 9), Brushes.Red);
            Manager.manager.GfxObjList.Add(partie3);

            Rec partie1Cadre = new Rec(Brushes.Black, new Point(160, 200), new Size(14, 14), "partie1Cadre", Manager.TypeGfx.Obj, true);
            Manager.manager.GfxObjList.Add(partie1Cadre);

            Rec partie2Cadre = new Rec(Brushes.Black, new Point(160, 220), new Size(14, 14), "partie2Cadre", Manager.TypeGfx.Obj, true);
            Manager.manager.GfxObjList.Add(partie2Cadre);

            Rec partie3Cadre = new Rec(Brushes.Black, new Point(160, 240), new Size(14, 14), "partie3Cadre", Manager.TypeGfx.Obj, true);
            Manager.manager.GfxObjList.Add(partie3Cadre);

            cadre_Select_Color1 = new Rec(Brushes.Red, new Point(1, 1), new Size(12, 12), "cadre_Select_color1", Manager.TypeGfx.Obj, true);
            cadre_Select_Color1.MouseOver += cadre_Select_Color_MouseOver;
            cadre_Select_Color1.MouseOut += cadre_Select_Color_MouseOut;
            cadre_Select_Color1.MouseClic += cadre_Select_Color_MouseClic;
            cadre_Select_Color1.ChangedBrush += cadre_Select_Color1_ChangedBrush;
            partie1Cadre.Child.Add(cadre_Select_Color1);

            cadre_Select_Color2 = new Rec(Brushes.Blue, new Point(1, 1), new Size(12, 12), "cadre_Select_color2", Manager.TypeGfx.Obj, true);
            cadre_Select_Color2.MouseOver += cadre_Select_Color_MouseOver;
            cadre_Select_Color2.MouseOut += cadre_Select_Color_MouseOut;
            cadre_Select_Color2.MouseClic += cadre_Select_Color_MouseClic;
            cadre_Select_Color2.ChangedBrush += cadre_Select_Color2_ChangedBrush;
            partie2Cadre.Child.Add(cadre_Select_Color2);

            cadre_Select_Color3 = new Rec(Brushes.BurlyWood, new Point(1, 1), new Size(12, 12), "cadre_Select_color3", Manager.TypeGfx.Obj, true);
            cadre_Select_Color3.MouseOver += cadre_Select_Color_MouseOver;
            cadre_Select_Color3.MouseOut += cadre_Select_Color_MouseOut;
            cadre_Select_Color3.MouseClic += cadre_Select_Color_MouseClic;
            cadre_Select_Color3.ChangedBrush += cadre_Select_Color3_ChangedBrush;
            partie3Cadre.Child.Add(cadre_Select_Color3);

            no_color_1 = new Bmp(@"gfx\map\CreatePlayer\obj\no_color.dat", new Point(partie1Cadre.point.X + 1, partie1Cadre.point.Y + 1), "no_color_1", Manager.TypeGfx.Obj, true, 1);
            no_color_1.MouseOut += DefaultCursor;
            no_color_1.MouseOver += HandCursor;
            no_color_1.MouseClic += no_color_1_MouseClic;
            Manager.manager.GfxObjList.Add(no_color_1);

            no_color_2 = new Bmp(@"gfx\map\CreatePlayer\obj\no_color.dat", new Point(partie2Cadre.point.X + 1, partie2Cadre.point.Y + 1), "no_color_2", Manager.TypeGfx.Obj, true, 1);
            no_color_2.MouseOut += DefaultCursor;
            no_color_2.MouseOver += HandCursor;
            no_color_2.MouseClic += no_color_2_MouseClic;
            Manager.manager.GfxObjList.Add(no_color_2);

            no_color_3 = new Bmp(@"gfx\map\CreatePlayer\obj\no_color.dat", new Point(partie3Cadre.point.X + 1, partie3Cadre.point.Y + 1), "no_color_3", Manager.TypeGfx.Obj, true, 1);
            no_color_3.MouseOut += DefaultCursor;
            no_color_3.MouseOver += HandCursor;
            no_color_3.MouseClic += no_color_3_MouseClic;
            Manager.manager.GfxObjList.Add(no_color_3);

            refresh = new Bmp(@"gfx\map\CreatePlayer\refresh.dat", new Point(no_color_2.point.X + 50, no_color_2.point.Y), "refresh", Manager.TypeGfx.Obj, true, 1);
            refresh.MouseClic += refresh_MouseClic;
            refresh.MouseOver += HandCursor;
            refresh.MouseOut += DefaultCursor;
            Manager.manager.GfxObjList.Add(refresh);

            //------- selection automatique du personnage naruto
            naruto_thumb.FireMouseClic(null);
            konoha.FireMouseClic(null);
        }

        void lValider_MouseClic(Txt txt, MouseEventArgs e)
        {
            ibValider_MouseClic();
        }
        void refresh_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            ibPlayers.ChangeBmp(ibPlayers.path, SpriteSheet.GetSpriteSheet(selected_Class.ToString(), 0));
            no_color_1.visible = true;
            no_color_2.visible = true;
            no_color_3.visible = true;
        }
        void DefaultCursor(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        void HandCursor(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        void no_color_3_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            cadre_Select_Color3.FireMouseClic(null);
        }
        void no_color_2_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            cadre_Select_Color2.FireMouseClic(null);
        }
        void no_color_1_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            cadre_Select_Color1.FireMouseClic(null);
        }
        void cadre_Select_Color1_ChangedBrush(Rec rec, MouseEventArgs e)
        {
            CommonCode.SetPixelToClass2(selected_Class, new Pen(rec.brush).Color, 1, ibPlayers);
            no_color_1.visible = false;
        }
        void cadre_Select_Color2_ChangedBrush(Rec rec, MouseEventArgs e)
        {
            CommonCode.SetPixelToClass2(selected_Class, new Pen(rec.brush).Color, 2, ibPlayers);
            no_color_2.visible = false;
        }
        void cadre_Select_Color3_ChangedBrush(Rec rec, MouseEventArgs e)
        {
            CommonCode.SetPixelToClass2(selected_Class, new Pen(rec.brush).Color, 3, ibPlayers);
            no_color_3.visible = false;
        }
        void cadre_Select_Color_MouseClic(Rec rec, MouseEventArgs e)
        {
            cd.ShowDialog();
            rec.brush = new SolidBrush(cd.Color);
        }
        void cadre_Select_Color_MouseOut(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        void cadre_Select_Color_MouseOver(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        void pays_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            Enums.HiddenVillage.Names hidden_Village = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), bmp.name);

            if (hidden_Village == Enums.HiddenVillage.Names.konoha)
            {
                cadre_Select_Pays.point = pays_thumbs_points[0];
                paysName.Text = CommonCode.TranslateText(41) + " : " + Enums.HiddenVillage.Names.konoha;
                paysDescription.Text = CommonCode.TranslateText(45);
                bonusPays.Text = CommonCode.TranslateText(52);
                selectedHiddenVillage = Enums.HiddenVillage.Names.konoha;
            }
            else if (hidden_Village == Enums.HiddenVillage.Names.iwa)
            {
                cadre_Select_Pays.point = pays_thumbs_points[1];
                paysName.Text = CommonCode.TranslateText(41) + " : " + Enums.HiddenVillage.Names.iwa;
                paysDescription.Text = CommonCode.TranslateText(46);
                bonusPays.Text = CommonCode.TranslateText(53);
                selectedHiddenVillage = Enums.HiddenVillage.Names.iwa;
            }
            else if (hidden_Village == Enums.HiddenVillage.Names.kiri)
            {
                cadre_Select_Pays.point = pays_thumbs_points[2];
                paysName.Text = CommonCode.TranslateText(41) + " : " + Enums.HiddenVillage.Names.kiri;
                paysDescription.Text = CommonCode.TranslateText(47);
                bonusPays.Text = CommonCode.TranslateText(54);
                selectedHiddenVillage = Enums.HiddenVillage.Names.kiri;
            }
            else if (hidden_Village == Enums.HiddenVillage.Names.kumo)
            {
                cadre_Select_Pays.point = pays_thumbs_points[3];
                paysName.Text = CommonCode.TranslateText(41) + " : " + Enums.HiddenVillage.Names.kumo;
                paysDescription.Text = CommonCode.TranslateText(48);
                bonusPays.Text = CommonCode.TranslateText(55);
                selectedHiddenVillage = Enums.HiddenVillage.Names.kumo;
            }
            else if (hidden_Village == Enums.HiddenVillage.Names.suna)
            {
                cadre_Select_Pays.point = pays_thumbs_points[4];
                paysName.Text = CommonCode.TranslateText(41) + " : " + Enums.HiddenVillage.Names.suna;
                paysDescription.Text = CommonCode.TranslateText(49);
                bonusPays.Text = CommonCode.TranslateText(56);
                selectedHiddenVillage = Enums.HiddenVillage.Names.suna;
            }
            paysName.point.X = (infosPlayer.size.Width / 2) - (TextRenderer.MeasureText(paysName.Text, paysName.font).Width / 2);
        }
        void classe_thumb_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), bmp.name);

            ibPlayers.ChangeBmp(@"gfx\general\classes\" + bmp.name + ".dat", SpriteSheet.GetSpriteSheet(bmp.name, 0));
            ibPlayers.point = new Point(ScreenManager.WindowWidth / 2 - (ibPlayers.rectangle.Width / 2), terrain.point.Y - ibPlayers.rectangle.Height + 10);
            ibPlayers.visible = true;

            illustration.ChangeBmp(@"gfx\general\classes\illustrations\" + bmp.name + ".dat", 0.5F);
            illustration.point = new Point(ScreenManager.WindowWidth - 50 - illustration.rectangle.Width, 20);
            illustration.visible = true;

            playerName.Text = CommonCode.TranslateText(40) + " : " + bmp.name;
            playerName.point.X = (infosPlayer.size.Width / 2) - (TextRenderer.MeasureText(playerName.Text, playerName.font).Width / 2);

            cadre_Select_Player.point = classes_thumbs_points[CommonCode.ClassNameToId(className)];
            selected_Class = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), bmp.name);

            no_color_1.visible = true;
            no_color_2.visible = true;
            no_color_3.visible = true;

            //if (allowFirstLaunch)
                //MainForm.ForeSound.URL = @"sfx\drum1.mp3";
            //else
                //allowFirstLaunch = true;
            CommonCode.foresound.SoundLocation = @"sfx\drum1.wav";
            CommonCode.foresound.LoadAsync();
            CommonCode.foresound.Play();
        }
        void ibValider_MouseClic()
        {
            if (name.Text == string.Empty)
            {
                MessageBox.Show(CommonCode.TranslateText(36), "Pseudo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (name.TextLength < 3 || name.TextLength > 15)
            {
                MessageBox.Show(CommonCode.TranslateText(120), "Pseudo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (Security.check_valid_pseudo(name.Text.ToLower()) && name.Text != string.Empty)
            {
                string tmpColor;

                if (!no_color_1.visible)
                {
                    Color tmp1Color = new Pen(cadre_Select_Color1.brush).Color;
                    tmpColor = tmp1Color.R.ToString() + "-" + tmp1Color.G.ToString() + "-" + tmp1Color.B.ToString() + "/";
                }
                else
                    tmpColor = "null/";

                if (!no_color_2.visible)
                {
                    Color tmp1Color = new Pen(cadre_Select_Color2.brush).Color;
                    tmpColor += tmp1Color.R.ToString() + "-" + tmp1Color.G.ToString() + "-" + tmp1Color.B.ToString() + "/";
                }
                else
                    tmpColor += "null/";

                if (!no_color_3.visible)
                {
                    Color tmp1Color = new Pen(cadre_Select_Color3.brush).Color;
                    tmpColor += tmp1Color.R.ToString() + "-" + tmp1Color.G.ToString() + "-" + tmp1Color.B.ToString();
                }
                else
                    tmpColor += "null";

                //Network.SendMessage("cmd•CreatePlayer•new player•" + name.Text.ToLower() + "•" + selected_Class + "•" + selectedHiddenVillage + "•" + tmpColor, true);

                CreateNewActorRequestMessage createNewActorRequestMessage = new CreateNewActorRequestMessage(name.Text.ToLower(), selected_Class, selectedHiddenVillage, tmpColor);
                createNewActorRequestMessage.Serialize();
                createNewActorRequestMessage.Send();


                ibValider.visible = false;
                name.Text = string.Empty;
            }
        }
        void ibValider_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            ibValider_MouseClic();
        }
        void ibRetour_MouseUp(Bmp bmp, MouseEventArgs e)
        {
            ibValider.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 9));
        }
        void ibRetour_MouseDown(Bmp bmp, MouseEventArgs e)
        {
            ibValider.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 10));
        }
        public void Network_stat(string stat)
        {
            Manager.manager.mainForm.Invoke(new DelHandel(Handle_Network_Stat), stat);
        }  
        public void Handle_Network_Stat(string stat)
        {
            string[] cmd = stat.Split('•');

            if (cmd.Length == 5 && cmd[0] == "internal")
            {
                if (cmd[1] == "network")
                {
                    if (cmd[2] == "connection")
                    {
                        if (cmd[3] == "failed")
                        {
                            MessageBox.Show(CommonCode.TranslateText(2), "Connexion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            GameStateManager.ChangeState(new LoginMap());
                            GameStateManager.CheckState();
                        }
                        else if (cmd[3] == "aborted")
                        {
                            MessageBox.Show(CommonCode.TranslateText(4), "Connexion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            GameStateManager.ChangeState(new LoginMap());
                            GameStateManager.CheckState();
                        }
                    }
                }
            }
        }
        public void KeyBoardHandleEvents()
        {

        }
        public void CleanUp()
        {
            Manager.manager.Clear();
        }
        public void Update()
        {

        } 
    }
}
