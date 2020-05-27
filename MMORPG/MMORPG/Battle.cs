using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using MELHARFI.Gfx;
using System.Drawing;
using MELHARFI;

namespace MMORPG
{
    public static class Battle
    {
        // classe qui lance le combat, menus + huds ...
        public static List<Actor> SideA = new List<Actor>();
        public static List<Actor> SideB = new List<Actor>();
        public static Enums.battleState.state state = Enums.battleState.state.idle;                         // lorsque le combat commence 
        public static int IdBattle;                                     // contiens l'id du combat
        public static string BattleType;
        public static List<Point> ValidePosT1 = new List<Point>();        // contiens la liste des points accessible lors du début d'un combat pour SideA
        public static List<Point> ValidePosT2 = new List<Point>();        // contiens la liste des points accessible lors du début d'un combat pour SideB
        public static List<Actor> AllPlayersByOrder = new List<Actor>();      // contiens tous els joueurs classé par ordre de jeu
        public static List<Actor> DeadPlayers = new List<Actor>();			// contiens la liste des joueurs mort dans un combat pour les reinvoquer peux etre ou pour le bonus fin de combat
        public static string PlayerTurn = "";                       // le pseudo du joueur qui a la main
        public static int TimeToPlayInBattle;                       // temps pour un tour
        public static int TimeLeftToPlay;                           // temps restant pour un tour
        public static System.Windows.Forms.Timer TimeLeftToPlayT = new System.Windows.Forms.Timer();    // timer qui calcule le % du temps restant pour un jouer avant de passer la main
        public static Txt TimeLeftLabel = new Txt("", new Point(0, 28), "__timeLeft", Manager.TypeGfx.Top, true, new Font("verdana", 17, FontStyle.Bold), Brushes.SteelBlue);
        public static Bmp Chrono_TimeOut = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ScreenManager.WindowWidth - 100, 50), "Chrono_TimeOut", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 23));
        public static Rec TimelineRec;
        public static Bmp _Selected_Player = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(100,50), "__Selected_Player", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 24));
        public static Bmp _bar_to_move = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(0, 0), "_bar_to_move", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 55));
        /////////////// variables de controles pour changement de curseurs en spell ou arme
        public static string currentCursor = "";                 // contiens le nom du curseur a savoir spell ou arme
        public static Actor.SpellsInformations infos_sorts;   // contiens les données du sort lancé
        public static bool isLeftButtonPressed = false;
        public static int timeleft;                     // variable de control pour la methode common1.timeLeftForBattle() qui est égale au temps d'attente lors des déplacement, pareil que TimeLeftToPlay mais celui la se decrémente jusqu'a = 0 pour commancer le combat, quand tous les joueurs vérroue leurs pos, ce variable se met a 0 pour que le combat commance
        public static bool ShowEnvoutement = false;
        //////////// methode static
        public static void DrawBattleValidePos()
        {
            // affichage des positions valide que le joueur dois choisir lors du commencement d'un combat
            if (state == Enums.battleState.state.initialisation && ValidePosT1.Count > 0 && ValidePosT2.Count > 0)
            {
                foreach (Point t in ValidePosT1)
                {
                    Rec validePosT1Rec = new Rec(Brushes.Blue, new Point(t.X + 1, t.Y + 1), new Size(28, 28), "__validePosT1Rec", Manager.TypeGfx.Bgr, true);
                    validePosT1Rec.MouseClic += validePosT1Rec_MouseClic;
                    validePosT1Rec.MouseOut += validePosTRec_MouseOut;
                    // si c'est notre team on affiche la main sur le curseur lors du survole
                    if(SideA.Exists(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo))
                        validePosT1Rec.MouseMove += validePosRec_MouseMove;
                    Manager.manager.GfxBgrList.Add(validePosT1Rec);
                }

                foreach (Point t in ValidePosT2)
                {
                    Rec validePosT2Rec = new Rec(Brushes.Red, new Point(t.X + 1, t.Y + 1), new Size(28, 28), "__validePosT2Rec", Manager.TypeGfx.Bgr, true);
                    validePosT2Rec.MouseClic += validePosT2Rec_MouseClic;
                    validePosT2Rec.MouseOut += validePosTRec_MouseOut;
                    // si c'est notre team on affiche la main sur le curseur lors du survole
                    if (Battle.SideB.Exists(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo))
                        validePosT2Rec.MouseMove += validePosRec_MouseMove;
                    Manager.manager.GfxBgrList.Add(validePosT2Rec);
                }
            }
        }
        static void validePosRec_MouseMove(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        static void validePosTRec_MouseOut(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        static void validePosT2Rec_MouseClic(Rec rec, MouseEventArgs e)
        {
            Network.SendMessage("cmd•battleIniNewPos•" + (e.X / 30) + "/" + (e.Y / 30), true);
        }
        static void validePosT1Rec_MouseClic(Rec rec, MouseEventArgs e)
        {
            Network.SendMessage("cmd•battleIniNewPos•" + (e.X / 30) + "/" + (e.Y / 30), true);
        }
        public static void Clear()
        {
            IdBattle = -1;
            state = Enums.battleState.state.idle;
            AllPlayersByOrder.Clear();
            SideA.Clear();
            SideB.Clear();
            DeadPlayers.Clear();
            ValidePosT1.Clear();
            ValidePosT2.Clear();
            TimeToPlayInBattle = 0;
            TimeLeftToPlay = 0;
            TimeLeftToPlayT.Stop();
            Chrono_TimeOut.visible = false;
            TimeLeftLabel.visible = false;
            //turn = 0;
            PlayerTurn = "";
            // supression des cadre du timeout
            CommonCode.DisposeIGfxAndChild(HudHandle.TimeLeftToPlayRec1);
            CommonCode.DisposeIGfxAndChild(HudHandle._passer_La_Main_btn);
            CommonCode.DisposeIGfxAndChild(HudHandle._quiter_le_combat);
        }
        public static void RefreshTimeLine()
        {
            // supression des cadrons des joueurs dans la timeline
            for(int cnt = TimelineRec.Child.Count; cnt > 0; cnt--)
            {
                if ((TimelineRec.Child[cnt - 1] as Bmp).name != "__Selected_Player" && (TimelineRec.Child[cnt - 1] as Bmp).name != "_bar_to_move")
                {
                    (TimelineRec.Child[cnt - 1] as Bmp).visible = false;
                    TimelineRec.Child.Remove(TimelineRec.Child[cnt - 1]);
                }
            }
            // affichage de la nouvelle disposition des joueurs dans les cadres

            // on determine la team d'en haut, celle qui a un joueur qui as le plus d'initiative
            int decalageVerticale = 4 + _bar_to_move.rectangle.Width + 2;       // 4 = marge du cadron qui entoure le joueur qui a la main, + la largeur du panneau de déplacement + 2 de distance
            const int decalageHorizontal = 4;
            for (int cnt = 0; cnt < AllPlayersByOrder.Count; cnt++)
            {
                Bmp _cadre_p = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty,
                    "_cadre_p_" + AllPlayersByOrder[cnt].pseudo, Manager.TypeGfx.Top, true, 1,
                    SpriteSheet.GetSpriteSheet("_Main_option", (AllPlayersByOrder[cnt].teamSide == Enums.Team.Side.A) ? 21 : 22))
                {
                    zindex = 1
                };
                _cadre_p.MouseMove += CommonCode.CursorHand_MouseMove;
                _cadre_p.MouseClic += _cadre_p_MouseClic;
                _cadre_p.tag = AllPlayersByOrder[cnt].pseudo;

                // on determine si c'est le 1er joueur a afficher
                Point p = new Point();
                if (cnt == 0)
                {
                    p.X = decalageVerticale + (cnt * (_cadre_p.rectangle.Width + 4));
                    p.Y = decalageHorizontal;
                }
                else
                {
                    // on recupere la position de l'ancienne image
                    Bmp _cadre_p_before = TimelineRec.Child.Find(f => f.Name() == "_cadre_p_" + AllPlayersByOrder[cnt - 1].pseudo) as Bmp;
                    if (_cadre_p_before == null)
                        MessageBox.Show("impossible d'avoir ce message");

                    // on verifie si ce joueur est dans la meme team que la precedante
                    if (AllPlayersByOrder[cnt - 1].teamSide == AllPlayersByOrder[cnt].teamSide)
                    {
                        // le joueur doit etre aligné comme son precedant
                        p.X = _cadre_p_before.point.X + 4 + _cadre_p.rectangle.Width;
                        p.Y = (AllPlayersByOrder[cnt - 1].teamSide == AllPlayersByOrder[0].teamSide) ? decalageHorizontal : 36 + decalageHorizontal;
                    }
                    else
                    {
                        // il s'agit d'un joueur different
                        // on dois determiner la position de la team d'avant si ils sont en haut ou en bas
                        p.X = _cadre_p_before.point.X + (_cadre_p.rectangle.Width / 2) + 4;
                        p.Y = (AllPlayersByOrder[cnt].teamSide == AllPlayersByOrder[0].teamSide) ? decalageHorizontal : 36 + decalageHorizontal;
                    }
                }

                _cadre_p.point = p;
                TimelineRec.Child.Add(_cadre_p);

                // affichage de l'avatar du joueur sur ce cadre
                Bmp _avatar_p = new Bmp(@"gfx\general\classes\" + AllPlayersByOrder[cnt].className + ".dat",
                    Point.Empty, "_avatar_p" + (cnt + 1) + "_battle_" + AllPlayersByOrder[cnt].pseudo,
                    Manager.TypeGfx.Top, true, 1,
                    SpriteSheet.GetSpriteSheet("avatar_" + AllPlayersByOrder[cnt].className, 0)) {zindex = 2};
                _avatar_p.point.X = _cadre_p.point.X + (_cadre_p.rectangle.Width / 2)  - (_avatar_p.rectangle.Width / 2);
                _avatar_p.point.Y = _cadre_p.point.Y + (_cadre_p.rectangle.Height / 2) - (_avatar_p.rectangle.Height / 2);
                _avatar_p.tag = new Actor { className = AllPlayersByOrder[cnt].className, maskColorString = AllPlayersByOrder[cnt].maskColorString, pseudo = AllPlayersByOrder[cnt].pseudo };
                _avatar_p.MouseMove += CommonCode.CursorHand_MouseMove;
                _avatar_p.MouseClic += _avatar_p_MouseClic;
                CommonCode.ApplyMaskColorToClasse(_avatar_p);
                TimelineRec.Child.Add(_avatar_p);
            }

            // image en triangle qui met la barre timeline en haut
            Bmp _cadre_dock_up = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(_bar_to_move.point.X, _bar_to_move.point.Y + 1), "_cadre_dock_up", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 56));
            _cadre_dock_up.zindex = _bar_to_move.zindex + 1;
            _cadre_dock_up.MouseClic += _cadre_dock_up_MouseClic;
            _cadre_dock_up.MouseMove += CommonCode.CursorHand_MouseMove;
            _cadre_dock_up.MouseOut += CommonCode.CursorDefault_MouseOut;
            TimelineRec.Child.Add(_cadre_dock_up);

            // image en triangle qui met la barre timeline en bas
            Bmp _cadre_dock_down = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(_bar_to_move.point.X, _bar_to_move.point.Y + _bar_to_move.rectangle.Height - 10), "_cadre_dock_up", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 57));
            _cadre_dock_down.zindex = _bar_to_move.zindex + 1;
            _cadre_dock_down.MouseMove += CommonCode.CursorHand_MouseMove;
            _cadre_dock_down.MouseOut += CommonCode.CursorDefault_MouseOut;
            _cadre_dock_down.MouseClic += _cadre_dock_down_MouseClic;
            TimelineRec.Child.Add(_cadre_dock_down);

            // on met le cadre qui selectionne le joueur qui a le tour de jouer
            Bmp _cadre_current_player = (TimelineRec.Child.Find(f => f.Name() == "_cadre_p_" + PlayerTurn) as Bmp);
            _Selected_Player.point = new Point(_cadre_current_player.point.X - 4, _cadre_current_player.point.Y - 5);

            // on remet a jour la taille horizontal du cadre
            TimelineRec.size.Width = (TimelineRec.Child.Find(f => f.Name() == "_cadre_p_" + AllPlayersByOrder[AllPlayersByOrder.Count - 1].pseudo) as Bmp).point.X + (Battle.TimelineRec.Child.Find(f => f.Name() == "_cadre_p_" + AllPlayersByOrder[AllPlayersByOrder.Count - 1].pseudo) as Bmp).rectangle.Width + 4;
        }
        private static void _avatar_p_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // clic sur le l'image d'un personnage dans la timeline, meme code sur la methode _cadre_p_MouseClic
            string pseudo = bmp.name.Substring(bmp.name.LastIndexOf('_') + 1, bmp.name.Length - (bmp.name.LastIndexOf('_') + 1));
            ShowEnvoutementOverTimeLine(pseudo);
        }
        private static void _cadre_p_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // clic sur le cadre qui encercle l'image d'un personnage dans la timeline
            string pseudo = bmp.name.Substring(bmp.name.LastIndexOf('_') + 1, bmp.name.Length - (bmp.name.LastIndexOf('_') + 1));
            ShowEnvoutementOverTimeLine(pseudo);
        }
        private static void ShowEnvoutementOverTimeLine(string player)
        {
            // affichage de l'image du sort de l'envoutement en question + un triangle qui contien un index de bonus / malus comme child
            if (!ShowEnvoutement)
            {
                ShowEnvoutement = true;
                List<Actor.Buff> envoutements = AllPlayersByOrder.Find(f => f.pseudo == player).BuffsList.FindAll(f => !f.systeme);

                // determiner s'il y à de l'espace en haut de la timeline
                int Xdistance = TimelineRec.point.X;
                int Ydistance;

                if (TimelineRec.point.Y - 31 - 2 > 0)
                    Ydistance = TimelineRec.point.Y - 31 - 2;
                else
                    Ydistance = TimelineRec.point.Y + TimelineRec.size.Height + 2;

                for (int cnt = 0; cnt < envoutements.Count; cnt++)
                {
                    // image du sort
                    Bmp SpellIcon1 = new Bmp(@"gfx\general\icons\spells\" + envoutements[cnt].SortID + ".dat", new Point(Xdistance + (32 * cnt), Ydistance), "SpellIcon1_" + envoutements[cnt].SortID, Manager.TypeGfx.Top, true, 1);
                    SpellIcon1.MouseMove += CommonCode.CursorHand_MouseMove;
                    SpellIcon1.MouseOver += SpellIcon1_MouseOver;
                    SpellIcon1.MouseOut += SpellIcon1_MouseOut;
                    SpellIcon1.tag = envoutements[cnt];
                    Manager.manager.GfxTopList.Add(SpellIcon1);

                    // trangle qui contien l'index de tour pour le bonus/malus
                    Bmp BonusRoundLeftBmp = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "BonusRoundLeftBmp_" + envoutements[cnt].SortID, Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 63));
                    BonusRoundLeftBmp.point = new Point(SpellIcon1.point.X + (SpellIcon1.rectangle.Width / 2) - (BonusRoundLeftBmp.rectangle.Width / 2), SpellIcon1.point.Y - BonusRoundLeftBmp.rectangle.Height - 1);
                    Manager.manager.GfxTopList.Add(BonusRoundLeftBmp);

                    // interval
                    Txt BonusRoundLeft = new Txt(envoutements[cnt].BonusRoundLeft.ToString(), Point.Empty, "BonusRoundLeft_" + envoutements[cnt].SortID, Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Black);
                    BonusRoundLeft.point = new Point((BonusRoundLeftBmp.rectangle.Width / 2) - (TextRenderer.MeasureText(BonusRoundLeft.Text, BonusRoundLeft.font).Width / 2) + 1, (BonusRoundLeftBmp.rectangle.Height / 2) - (TextRenderer.MeasureText(BonusRoundLeft.Text, BonusRoundLeft.font).Height / 2));
                    BonusRoundLeftBmp.Child.Add(BonusRoundLeft);
                }
            }
            else
            {
                // cacher les envoutement visibles
                HideEnvoutementOverTimeLine();
            }
        }
        static void SpellIcon1_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            // supression du descrptif de l'envoutement
            Actor.Buff envoutement = bmp.tag as Actor.Buff;

            IGfx EnvoutementRec1 = Manager.manager.GfxTopList.Find(f => f.Name() == "EnvoutementRec1_" + envoutement.SortID);
            if (EnvoutementRec1 != null)
            {
                (EnvoutementRec1 as Rec).Child.Clear();
                Manager.manager.GfxTopList.Remove(EnvoutementRec1);
            }
        }
        static void SpellIcon1_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            // survole sur l'envoutement pour afficher les detail dans un rectangle semie transparent
            Actor.Buff envoutement = bmp.tag as Actor.Buff;

            Rec EnvoutementRec1 = new Rec(new Pen(Color.FromArgb(180, Color.White)).Brush, Point.Empty, new Size(150, 45), "EnvoutementRec1_" + envoutement.SortID, Manager.TypeGfx.Top, true);

            Point p = new Point(bmp.point.X + (bmp.rectangle.Width / 2) - (EnvoutementRec1.size.Width / 2), bmp.point.Y - EnvoutementRec1.size.Height - 20);
            if (p.X < 0)
                p.X = 1;
            EnvoutementRec1.point = p;
            Manager.manager.GfxTopList.Add(EnvoutementRec1);

            // image du sort
            Bmp spellIcon = (Bmp)(bmp.Clone());
            spellIcon.name = "spellIcon_" + envoutement.SortID;
            spellIcon.point = new Point(1, 1);
            EnvoutementRec1.Child.Add(spellIcon);

            // nom de sort
            Txt spellName = new Txt(spells.sort(envoutement.SortID).title, new Point(spellIcon.point.X + spellIcon.rectangle.Width + 5, 2), "spellName_" + envoutement.SortID, Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            EnvoutementRec1.Child.Add(spellName);

            // traitement pour chaque sort
            switch (envoutement.SortID)
            {
                case 7:
                {
                    Txt bonus = new Txt("+2 PC", new Point(30, 14), "__bonus", Manager.TypeGfx.Top, true, new Font("Verdana", 6, FontStyle.Bold), Brushes.Blue);
                    EnvoutementRec1.Child.Add(bonus);
                }
                    break;
                case 8:
                {
                    Txt bonus = new Txt("+2 PM", new Point(30, 14), "__bonus", Manager.TypeGfx.Top, true, new Font("Verdana", 6, FontStyle.Bold), Brushes.Blue);
                    EnvoutementRec1.Child.Add(bonus);
                }
                    break;
                case 9:
                {
                    Actor currentPlayer = Battle.AllPlayersByOrder.Find(f => f.pseudo == envoutement.player);
                    int sortLvl = currentPlayer.spells.Find(f => f.sortID == envoutement.SortID).level;
                    int puissance = spells.sort(envoutement.SortID).isbl[sortLvl - 1].piBonus.power;

                    Txt bonus = new Txt("+" + puissance + " Puissance", new Point(30, 14), "__bonus", Manager.TypeGfx.Top, true, new Font("Verdana", 6, FontStyle.Bold), Brushes.Blue);
                    EnvoutementRec1.Child.Add(bonus);
                }
                    break;
                case 10:
                {
                    Actor currentPlayer = Battle.AllPlayersByOrder.Find(f => f.pseudo == envoutement.player);
                    int sortLvl = currentPlayer.spells.Find(f => f.sortID == envoutement.SortID).level;
                    int doton = spells.sort(envoutement.SortID).isbl[sortLvl - 1].piBonus.doton;

                    Txt bonus = new Txt("+" + doton + " Doton", new Point(30, 14), "__bonus", Manager.TypeGfx.Top, true, new Font("Verdana", 6, FontStyle.Bold), CommonCode.dotonColor);
                    EnvoutementRec1.Child.Add(bonus);
                }
                    break;
            }
        }
        private static void HideEnvoutementOverTimeLine()
        {
            // cacher les envoutement visibles
            ShowEnvoutement = false;
            if (Manager.manager.GfxTopList.Exists(f => f.Name() != null && f.Name().Length > 11 && f.Name().Substring(0, 11) == "SpellIcon1_"))
            {
                Manager.manager.GfxTopList.Find(f => f.Name() != null && f.Name().Length > 11 && f.Name().Substring(0, 11) == "SpellIcon1_").Visible(false);
                Manager.manager.GfxTopList.RemoveAll(f => f.Name() != null && f.Name().Length > 11 && f.Name().Substring(0, 11) == "SpellIcon1_");
            }
            if (Manager.manager.GfxTopList.Exists(f => f.Name() != null && f.Name().Length > 18 && f.Name().Substring(0, 18) == "BonusRoundLeftBmp_"))
            {
                Manager.manager.GfxTopList.Find(f => f.Name() != null && f.Name().Length > 18 && f.Name().Substring(0, 18) == "BonusRoundLeftBmp_").Visible(false);
                Manager.manager.GfxTopList.RemoveAll(f => f.Name() != null && f.Name().Length > 18 && f.Name().Substring(0, 18) == "BonusRoundLeftBmp_");
            }
            if (Manager.manager.GfxTopList.Exists(f => f.Name() != null && f.Name().Length > 18 && f.Name().Substring(0, 15) == "BonusRoundLeft_"))
            {
                Manager.manager.GfxTopList.Find(f => f.Name() != null && f.Name().Length > 18 && f.Name().Substring(0, 15) == "BonusRoundLeft_").Visible(false);
                Manager.manager.GfxTopList.RemoveAll(f => f.Name() != null && f.Name().Length > 18 && f.Name().Substring(0, 15) == "BonusRoundLeft_");
            }
        }
        static void _cadre_dock_down_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // mettre la barre de timeline en bas de la fenettre
            if (TimelineRec.point.X > 0 || TimelineRec.point.Y < (ScreenManager.TilesHeight * 30) - 88)
            {
                int xDistance = TimelineRec.point.X;
                int yDistance = (ScreenManager.TilesHeight * 30) - TimelineRec.point.Y;

                Point start = TimelineRec.point;
                Point end = new Point(0, (ScreenManager.TilesHeight * 30) - 88);

                int speed = (int)Math.Sqrt(Math.Max(Math.Abs(xDistance), Math.Abs(yDistance)) * 10);
                List<PointF> waypoint = CommonCode.calculeTrajectoire(start, end, speed);
                waypoint.Add(end);

                // devision du waypoint a 3 étapes
                int step1 = (waypoint.Count * 90) / 100;
                int step2 = (waypoint.Count * 95) / 100;

                new Thread(new ThreadStart(() =>
                {
                    Thread.CurrentThread.Name = "__cadre_dock_down_MouseClic_TimelineRec";
                    // determination du waypoint
                    for (int cnt = 0; cnt < waypoint.Count && !Manager.manager.mainForm.IsDisposed; cnt++)
                    {
                        if (cnt < step1)
                        {
                            Battle.TimelineRec.point.X = (int)Math.Round(waypoint[cnt].X);
                            Battle.TimelineRec.point.Y = (int)Math.Round(waypoint[cnt].Y);
                            Thread.Sleep(10);
                        }
                        else if (cnt < step2)
                        {
                            Battle.TimelineRec.point.X = (int)Math.Round(waypoint[cnt].X);
                            Battle.TimelineRec.point.Y = (int)Math.Round(waypoint[cnt].Y);
                            Thread.Sleep(30);
                        }
                        else if (cnt > 0)
                        {
                            Battle.TimelineRec.point.X = (int)Math.Round(waypoint[cnt].X);
                            Battle.TimelineRec.point.Y = (int)Math.Round(waypoint[cnt].Y);
                            Thread.Sleep(100);
                        }
                    }
                })).Start();
            }
        }
        static void _cadre_dock_up_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // mettre la barre de timeline en haut de la fenettre
            if (TimelineRec.point.X > 0 || TimelineRec.point.Y > 0)
            {
                int xDistance = TimelineRec.point.X;
                int yDistance = TimelineRec.point.Y;

                Point start = TimelineRec.point;
                Point end = new Point(0, 0);

                int speed = (int)Math.Sqrt(Math.Max(Math.Abs(xDistance), Math.Abs(yDistance)) * 10);
                List<PointF> waypoint = CommonCode.calculeTrajectoire(start, end, speed);
                waypoint.Add(end);
                
                // devision du waypoint a 3 étapes
                int step1 = (waypoint.Count * 90) / 100;
                int step2 = (waypoint.Count * 95) / 100;

                new Thread(new ThreadStart(() =>
                {
                    Thread.CurrentThread.Name = "__cadre_dock_up_TimelineRec";
                    // determination du waypoint
                    for (int cnt = 0; cnt < waypoint.Count && !Manager.manager.mainForm.IsDisposed; cnt++)
                    {
                        if (cnt < step1)
                        {
                            Battle.TimelineRec.point.X = (int)Math.Round(waypoint[cnt].X);
                            Battle.TimelineRec.point.Y = (int)Math.Round(waypoint[cnt].Y);
                            Thread.Sleep(10);
                        }
                        else if (cnt < step2)
                        {
                            Battle.TimelineRec.point.X = (int)Math.Round(waypoint[cnt].X);
                            Battle.TimelineRec.point.Y = (int)Math.Round(waypoint[cnt].Y);
                            Thread.Sleep(30);
                        }
                        else if(cnt > 0)
                        {
                            Battle.TimelineRec.point.X = (int)Math.Round(waypoint[cnt].X);
                            TimelineRec.point.Y = (int)Math.Round(waypoint[cnt].Y);
                            Thread.Sleep(100);
                        }
                    }
                })).Start();
            }
        }
        public static void UpdateSelectedPlayerInBattle()
        {
            #region
            if (state == Enums.battleState.state.started)
            {
                // selection le joueur qui a la main par un cadre noir parmis la liste des joueurs et monstres
                Bmp _cadre_current_player = (TimelineRec.Child.Find(f => f.Name() == "_cadre_p_" + PlayerTurn) as Bmp);
                _Selected_Player.point = new Point(_cadre_current_player.point.X - 4, _cadre_current_player.point.Y - 5);
            }
            #endregion
        }
        public static void CloseBattle(string cmd)
        {
            #region
            // fermeture de combat et notoyage de l'ecran
            // cmd•CloseBattle•BattleType comme FreeChallenge ...•winnerTeam

            Bmp _bg_datcmm = new Bmp(@"gfx\general\obj\1\bg_tamise1.dat", new Point(0, 0), "_bg_datcmm", Manager.TypeGfx.Top, true, 1);
            Manager.manager.GfxTopList.Add(_bg_datcmm);

            // arret du timer du timeout du combat par la modification du statut du combat
            state = Enums.battleState.state.closed;
            string[] data = cmd.Split('•');

            List<Actor> piibT1 = SideA.FindAll(f => f.species != Enums.Species.Name.Summon);
            piibT1.AddRange(DeadPlayers.FindAll(f => f.teamSide == Enums.Team.Side.A && f.species != Enums.Species.Name.Summon));

            List<Actor> piibT2 = SideB.FindAll(f => f.species != Enums.Species.Name.Summon);
            piibT2.AddRange(DeadPlayers.FindAll(f => f.teamSide == Enums.Team.Side.B && f.species != Enums.Species.Name.Summon));

            // affichage du tableau de résultat
            Rec finalResultRecParent = new Rec(CommonCode.spellAreaNotAllowedColor, new Point(150, 100), new Size(500, (piibT1.Count * 31) + 100 + (piibT2.FindAll(f => f.species == Enums.Species.Name.Human).Count * 31) / 2), "__finalResultRecParent", Manager.TypeGfx.Top, true);
            finalResultRecParent.point = new Point(((ScreenManager.TilesWidth * 30) / 2) - (finalResultRecParent.size.Width / 2), ((ScreenManager.TilesHeight * 30) / 2) - (finalResultRecParent.size.Height / 2));
            Manager.manager.GfxTopList.Add(finalResultRecParent);

            Rec FinalResultRecChild1 = new Rec(Brushes.GhostWhite, new Point(1, 1), new Size(498, (piibT1.Count * 31) + 100 + (piibT2.Count * 31) - 2), "__FinalResultRecChild1", Manager.TypeGfx.Top, true);
            finalResultRecParent.Child.Add(FinalResultRecChild1);

            Txt FinalResultTxtChild1 = new Txt(CommonCode.TranslateText(108), new Point(0, 5), "__FinalResultTxtChild1", Manager.TypeGfx.Top, true, new Font("Verdana", 12, FontStyle.Bold), Brushes.Red);
            FinalResultTxtChild1.point.X = 250 - (TextRenderer.MeasureText(FinalResultTxtChild1.Text, FinalResultTxtChild1.font).Width / 2);
            finalResultRecParent.Child.Add(FinalResultTxtChild1);

            Enums.Team.Side team = (Enums.Team.Side)Enum.Parse(typeof(Enums.Team.Side), data[3]);

            int distance = (team == Enums.Team.Side.B) ? ((Battle.SideA.FindAll(f => f.species == Enums.Species.Name.Human).Count + Battle.DeadPlayers.FindAll(f => f.teamSide == Enums.Team.Side.A && f.species == Enums.Species.Name.Human).Count) * 22) + 20 : 0;
            // affichage des states des joueurs SideA

            int heightOfVS = 0;

            for (int cnt = 0; cnt < piibT1.Count; cnt++)
            {
                Rec FinalResultRec1PT1 = new Rec(Brushes.Black, new Point(25, 30 + (cnt * 22) + distance), new Size(468, 20), "__FinalResultRec1PT1", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultRec1PT1);

                Rec FinalResultRec2PT1 = new Rec(Brushes.PapayaWhip, new Point(26, 31 + (cnt * 22) + distance), new Size(100, 18), "__FinalResultRec2PT1", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultRec2PT1);

                // affichage du pseudo
                Txt FinalResultTxt1PT1 = new Txt(piibT1[cnt].pseudo, new Point(26, 34 + (cnt * 22) + distance), "__FinalResultTxt1PT1", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Regular), Brushes.Black);
                finalResultRecParent.Child.Add(FinalResultTxt1PT1);

                // s'il sagit d'une invoc on soustrait just le nom sans l'id
                // pas la paine puisqu'on affiche pas les invocs
                if (piibT1[cnt].pseudo.IndexOf('$') == -1)
                    FinalResultTxt1PT1.Text = piibT1[cnt].pseudo;
                else
                    FinalResultTxt1PT1.Text = piibT1[cnt].pseudo.Substring(0, piibT1[cnt].pseudo.IndexOf('$'));

                // affichage du lvl
                Rec FinalResultRecLvlPT1 = new Rec(Brushes.PapayaWhip, new Point(127, 31 + (cnt * 22) + distance), new Size(100, 18), "__FinalResultRecLvlPT1", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultRecLvlPT1);

                Txt FinalResultTxtLvlPT1 = new Txt(piibT1[cnt].level.ToString(), new Point(127, 34 + (cnt * 22) + distance), "__FinalResultTxtLvlPT1", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Black);
                finalResultRecParent.Child.Add(FinalResultTxtLvlPT1);

                Bmp FinalResultWinIconT1;
                // si le joueur est gagnon
                if (team == piibT1[cnt].teamSide && DeadPlayers.Exists(f => f.pseudo == piibT1[cnt].pseudo))  // gagnon et a terminé le combat sans mourir
                    FinalResultWinIconT1 = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(154, 31 + (cnt * 22) + distance), "__FinalResultWinIconT1", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 60));
                else if (team == piibT1[cnt].teamSide && !DeadPlayers.Exists(f => f.pseudo == piibT1[cnt].pseudo))    // gagnon mais est mort lors d'un combat
                    FinalResultWinIconT1 = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(154, 31 + (cnt * 22) + distance), "__FinalResultWinIconT1", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 59));
                else    // mort est perdant
                    FinalResultWinIconT1 = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(154, 31 + (cnt * 22) + distance), "__FinalResultWinIconT1", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 58));
                finalResultRecParent.Child.Add(FinalResultWinIconT1);

                // affichage de l'avatar du joueur sur ce cadre
                Bmp FinalResultAvatar_t1_p = new Bmp(@"gfx\general\classes\" + piibT1[cnt].className + ".dat", new Point(2, 29 + (cnt * 22) + distance), "__FinalResultavatar_t1_p" + (cnt + 1) + "_battle", Manager.TypeGfx.Top, true, 1, new Rectangle(SpriteSheet.GetSpriteSheet("avatar_" + piibT1[0].className, 0).Location, new Size(SpriteSheet.GetSpriteSheet("avatar_" + piibT1[0].className, 0).Width, SpriteSheet.GetSpriteSheet("avatar_" + piibT1[0].className, 0).Height - 4)));
                FinalResultAvatar_t1_p.tag = new Actor() { className = piibT1[cnt].className, maskColorString = piibT1[cnt].maskColorString, };
                CommonCode.ApplyMaskColorToClasse(FinalResultAvatar_t1_p);
                finalResultRecParent.Child.Add(FinalResultAvatar_t1_p);

                // affichage du village
                Rec FinalResultVillageRecPT1 = new Rec(Brushes.PapayaWhip, new Point(228, 31 + (cnt * 22) + distance), new Size(50, 18), "__FinalResultVillageRecPT1", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultVillageRecPT1);

                Bmp FinalResultvillagePT1 = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", new Point(240, 30 + (cnt * 22) + distance), "__village_" + piibT1[cnt].hiddenVillage, Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("pays_" + piibT1[cnt].hiddenVillage + "_thumbs", 0));
                finalResultRecParent.Child.Add(FinalResultvillagePT1);

                // affichage des elements dropés
                Rec FinalResultDropePT1 = new Rec(Brushes.PapayaWhip, new Point(279, 31 + (cnt * 22) + distance), new Size(213, 18), "__FinalResultDropePT1", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultDropePT1);

                heightOfVS = 31 + (cnt * 22) + distance;
            }

            if (distance == 0)
                distance = (piibT1.Count * 22) + 42;
            else
                distance = 0;

            // image vs
            Bmp FinalResultVS = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(0, 0), "__vs", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("vs", 0));
            FinalResultVS.point = new Point((finalResultRecParent.size.Width / 2) - (FinalResultVS.rectangle.Width / 2), heightOfVS + 20);
            finalResultRecParent.Child.Add(FinalResultVS);

            // affichage des states des joueurs SideB
            for (int cnt = 0; cnt < piibT2.Count; cnt++)
            {
                // cadre
                Rec FinalResultRec1PT2 = new Rec(Brushes.Black, new Point(25, 30 + (cnt * 22) + distance), new Size(468, 20), "__FinalResultRec1PT2", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultRec1PT2);

                Rec FinalResultRec2PT2 = new Rec(Brushes.PapayaWhip, new Point(26, 31 + (cnt * 22) + distance), new Size(100, 18), "__FinalResultRec2PT2", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultRec2PT2);

                // affichage du pseudo
                Txt FinalResultTxt1PT2 = new Txt(piibT2[cnt].pseudo, new Point(26, 34 + (cnt * 22) + distance), "__FinalResultTxt1PT2", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Regular), Brushes.Black);
                finalResultRecParent.Child.Add(FinalResultTxt1PT2);

                // s'il sagit d'une invoc on soustrait just le nom sans l'id
                if (piibT2[cnt].pseudo.IndexOf('$') == -1)
                    FinalResultTxt1PT2.Text = piibT2[cnt].pseudo;
                else
                    FinalResultTxt1PT2.Text = piibT2[cnt].pseudo.Substring(0, piibT2[cnt].pseudo.IndexOf('$'));

                // affichage du lvl
                Rec FinalResultRecLvlPT2 = new Rec(Brushes.PapayaWhip, new Point(127, 31 + (cnt * 22) + distance), new Size(100, 18), "__FinalResultRecLvlPT2", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultRecLvlPT2);

                Txt FinalResultTxtLvlPT2 = new Txt(piibT2[cnt].level.ToString(), new Point(127, 34 + (cnt * 22) + distance), "__FinalResultTxtLvlPT2", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Black);
                finalResultRecParent.Child.Add(FinalResultTxtLvlPT2);

                Bmp FinalResultWinIconT2;
                // si le joueur est gagnon
                if (team == piibT2[cnt].teamSide && DeadPlayers.Exists(f => f.pseudo == piibT2[cnt].pseudo))  // gagnon et a terminé le combat sans mourir
                    FinalResultWinIconT2 = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(154, 31 + (cnt * 22) + distance), "__FinalResultWinIconT2", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 60));
                else if (team == piibT2[cnt].teamSide && !DeadPlayers.Exists(f => f.pseudo == piibT2[cnt].pseudo))    // gagnon mais est mort lors d'un combat
                    FinalResultWinIconT2 = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(154, 31 + (cnt * 22) + distance), "__FinalResultWinIconT2", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 59));
                else    // mort est perdant
                    FinalResultWinIconT2 = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(154, 31 + (cnt * 22) + distance), "__FinalResultWinIconT2", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 58));
                finalResultRecParent.Child.Add(FinalResultWinIconT2);

                // affichage de l'avatar du joueur sur ce cadre
                Bmp FinalResultAvatar_t2_p = new Bmp(@"gfx\general\classes\" + piibT2[cnt].className + ".dat", new Point(2, 29 + (cnt * 22) + distance), "__FinalResultavatar_t2_p" + (cnt + 1) + "_battle", Manager.TypeGfx.Top, true, 1, new Rectangle(SpriteSheet.GetSpriteSheet("avatar_" + piibT2[0].className, 0).Location, new Size(SpriteSheet.GetSpriteSheet("avatar_" + piibT2[0].className, 0).Width, SpriteSheet.GetSpriteSheet("avatar_" + piibT2[0].className, 0).Height - 4)));
                FinalResultAvatar_t2_p.tag = new Actor() { className = piibT2[cnt].className, maskColorString = piibT2[cnt].maskColorString, };
                CommonCode.ApplyMaskColorToClasse(FinalResultAvatar_t2_p);
                finalResultRecParent.Child.Add(FinalResultAvatar_t2_p);

                // affichage du village
                Rec FinalResultVillageRecPT2 = new Rec(Brushes.PapayaWhip, new Point(228, 31 + (cnt * 22) + distance), new Size(50, 18), "__FinalResultVillageRecPT2", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultVillageRecPT2);

                Bmp FinalResultvillagePT2 = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", new Point(240, 30 + (cnt * 22) + distance), "__village_" + piibT2[cnt].hiddenVillage, Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("pays_" + piibT2[cnt].hiddenVillage + "_thumbs", 0));
                finalResultRecParent.Child.Add(FinalResultvillagePT2);

                // affichage des elements dropés
                Rec FinalResultDropePT2 = new Rec(Brushes.PapayaWhip, new Point(279, 31 + (cnt * 22) + distance), new Size(213, 18), "__FinalResultDropePT2", Manager.TypeGfx.Top, true);
                finalResultRecParent.Child.Add(FinalResultDropePT2);
            }

            // affichage du boutton terminé
            Bmp FinalResult_btnok_datcmm = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(200, ((piibT1.Count * 31) + 80 + (piibT2.Count * 31) / 2)), "__FinalResult_btnok_datcmm", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 19));
            FinalResult_btnok_datcmm.EscapeGfxWhileMouseClic = true;
            FinalResult_btnok_datcmm.EscapeGfxWhileMouseMove = true;
            FinalResult_btnok_datcmm.MouseClic += FinalResult_btnok_datcmm_MouseClic;
            FinalResult_btnok_datcmm.MouseMove += CommonCode.CursorHand_MouseMove;
            FinalResult_btnok_datcmm.MouseOut += CommonCode.CursorDefault_MouseOut;
            finalResultRecParent.Child.Add(FinalResult_btnok_datcmm);

            // text ok
            Txt FinalResult_txtok_datcmm = new Txt(CommonCode.TranslateText(101), new Point(0, ((piibT1.Count * 31) + 82 + (piibT2.Count * 31) / 2)), "_txtok_datcmm", Manager.TypeGfx.Top, true, new Font("Verdana", 8), Brushes.Green);
            FinalResult_txtok_datcmm.point.X = 170 + ((FinalResult_btnok_datcmm.rectangle.Width + TextRenderer.MeasureText(FinalResult_txtok_datcmm.Text, FinalResult_txtok_datcmm.font).Width) / 2);
            finalResultRecParent.Child.Add(FinalResult_txtok_datcmm);

            if (Battle.TimelineRec != null)
            {
                for (int cnt = Battle.TimelineRec.Child.Count; cnt > 0; cnt--)
                    (Battle.TimelineRec.Child[cnt - 1] as Bmp).visible = false;
                Battle.TimelineRec.Child.Clear();
                Battle.TimelineRec.visible = false;
                Manager.manager.GfxTopList.Remove(Battle.TimelineRec);
            }

            // réactivation de tous les sorts
            List<Actor.SpellsInformations> iSort = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).spells;
            for (int cnt = 0; cnt < iSort.Count; cnt++)
            {
                // pointeur vers l'image du sort
                Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f != null && f.GetType() == typeof(Bmp) && (f as Bmp).tag != null && f.Tag().GetType() == typeof(Actor.SpellsInformations) && (f.Tag() as Actor.SpellsInformations).sortID == iSort[cnt].sortID));
                Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;
                // changement de l'image de sort en image en normale "accessible"
                spellIcon.ChangeBmp(@"gfx\general\obj\1\spells.dat", SpriteSheet.GetSpriteSheet(spellIS.sortID + "_spell", 0));
            }

            // supression des label pour relanceInterval affichés sur les sorts
            HudHandle.all_sorts.Child.RemoveAll(f => f != null && f.Name().Length > 16 && f.Name().Substring(0, 16) == "relanceInterval_");

            // supression des envoutements
            // pour cela, notre personnage risque d'etre dans 3 liste differentes, SideA, SideB , deadlist, donc on dois créer une nouvelle liste avec tous les joueurs dedons
            List<Actor> piL = new List<Actor>();
            piL.AddRange(Battle.SideA);
            piL.AddRange(Battle.SideB);
            piL.AddRange(Battle.DeadPlayers);

            Actor pi = piL.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo);
            
            //pi?.BuffsList.Clear();
            if(pi != null)
                pi.BuffsList.Clear();

            Clear();
            #endregion
        }
        public static void FinalResult_btnok_datcmm_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            #region
            // effacement du tableau du résultat du combat
            IGfx finalResultRecParent = Manager.manager.GfxTopList.Find(f => f.GetType() == typeof(Rec) && f.Name() == "__finalResultRecParent");
            Rec r = (Rec)finalResultRecParent;
            r.visible = false;
            r.Child.Clear();
            Manager.manager.GfxFixedList.Remove(finalResultRecParent);
            Manager.manager.GfxTopList.Remove(r);

            IGfx _bg_datcmm = Manager.manager.GfxTopList.Find(f => f.Name() == "_bg_datcmm");
            if (_bg_datcmm != null)
            {
                ((Bmp)_bg_datcmm).visible = false;
                Manager.manager.GfxTopList.Remove(_bg_datcmm);
            }

            // on recharge la map si on ai toujours connecté
            if (Network.netClient.ConnectionStatus == MELHARFI.Lidgren.Network.NetConnectionStatus.Connected)
                CommonCode.ChangeMap(((Actor)CommonCode.MyPlayerInfo.instance.ibPlayer.tag).map);
            #endregion
        }
        public static void _bar_to_move_MouseDown(Bmp bmp, MouseEventArgs e)
        {
            isLeftButtonPressed = true;
            Manager.manager.mainForm.MouseMove += mainForm_MouseMove;
            Manager.manager.mainForm.MouseUp += mainForm_MouseUp;
        }
        static void mainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (state == Enums.battleState.state.started && e.Button == MouseButtons.Left && isLeftButtonPressed)
            {
                // quand le joueur est en combat et qu'il a relaché le bouton gauche et que la souris qui déplacé la time line
                isLeftButtonPressed = false;
                Manager.manager.mainForm.MouseMove -= mainForm_MouseMove;
                Manager.manager.mainForm.MouseUp += mainForm_MouseUp;
            }
        }
        static void mainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (state == Enums.battleState.state.started && e.Button == MouseButtons.Left && isLeftButtonPressed)
            {
                // quand le joueur est en combat et qu'il a appuyé sur le bouton gauche et que la souris été sur l'image _bar_to_move qui déplace la time line
                // on verifie si la souris ne sort pas des limites autorisées sur la fenetre
                if (e.Location.Y <= ((ScreenManager.TilesHeight * 30) - TimelineRec.size.Height) && e.Location.Y >= 0 && e.Location.X >= 0 && e.Location.X <= ((ScreenManager.TilesWidth * 30) - TimelineRec.size.Width))
                    TimelineRec.point = e.Location;
                else if ((e.Location.Y > ((ScreenManager.TilesHeight * 30) - TimelineRec.size.Height) || e.Location.Y < 0) && e.Location.X >= 0 && e.Location.X <= ((ScreenManager.TilesWidth * 30) - TimelineRec.size.Width))
                    TimelineRec.point.X = e.Location.X;
                else if ((e.Location.X > ((ScreenManager.TilesWidth * 30) - TimelineRec.size.Width) || e.Location.X < 0) && e.Location.Y >= 0 && e.Location.Y <= ((ScreenManager.TilesHeight * 30) - TimelineRec.size.Height))
                    TimelineRec.point.Y = e.Location.Y;
            }
        }
        public static void __spell_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            #region lorsqu'un joueur clic sur un sort
            // check si on ai en combat, si on a la main, , si on ai pas en mode actio = marche || courir

            // infos sur le sort passé par l'objet bmp
            Actor.SpellsInformations infos_sorts = (bmp.tag as Actor.SpellsInformations);
            Battle.infos_sorts = (bmp.tag as Actor.SpellsInformations);       // pour stocker en memoire les données du sort qui a été choisie pour les exploiter sur la methode commune.drawSpellTiles();
            // controle si on pas assez de pc
            Actor myPlayerInfo = AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo);

            if (state == Enums.battleState.state.started && PlayerTurn == CommonCode.MyPlayerInfo.instance.pseudo && AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).animatedAction == Enums.AnimatedActions.Name.idle)
            {
                CommonCode.cleanMapFromSpellTiles();
                if (myPlayerInfo.originalPc < spells.sort(infos_sorts.sortID).isbl[infos_sorts.level - 1].pi.originalPc)
                {
                    Bmp __spellCursor = new Bmp(@"gfx\general\obj\1\nopc.dat", new Point(-20, -20), "__spellCursor", Manager.TypeGfx.Top, true, 1);
                    Manager.manager.GfxTopList.Add(__spellCursor);
                    Manager.manager.mainForm.Cursor = new Cursor(__spellCursor.bmp.GetHicon());
                    Battle.currentCursor = "nopc";
                }
                else
                {
                    Bmp __spellCursor = new Bmp(@"gfx\general\obj\1\spellArrow.dat", new Point(-20, -20), "__spellCursor", Manager.TypeGfx.Top, true, 1);
                    Manager.manager.GfxTopList.Add(__spellCursor);
                    Manager.manager.mainForm.Cursor = new Cursor(__spellCursor.bmp.GetHicon());
                    Battle.currentCursor = "spell";
                }

                // affichage des cases affectés par le sort
                CommonCode.DrawSpellTiles();
            }
            #endregion
        }
        public static void __spell_MouseMove(Bmp bmp, MouseEventArgs e)
        {
            #region survole sur un sort dans la barre de hud
            // infos sur le sort passé par l'objet bmp
            Actor.SpellsInformations infos_sorts = (bmp.tag as Actor.SpellsInformations);
            
            Actor myPlayerInfo = AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo);

            if (state == Enums.battleState.state.started)
            {
                if (myPlayerInfo.currentPc < spells.sort(infos_sorts.sortID).isbl[infos_sorts.level - 1].pi.originalPc)
                {
                    // check si on pas assez de pc
                    Bmp __spellCursor = new Bmp(@"gfx\general\obj\1\nopc.dat", new Point(-20, -20), "__spellCursor", Manager.TypeGfx.Top, true, 1);
                    Manager.manager.GfxTopList.Add(__spellCursor);
                    Manager.manager.mainForm.Cursor = new Cursor(__spellCursor.bmp.GetHicon());
                }
                else if (spells.sort_d_invocation.Exists(f => f == infos_sorts.sortID) && myPlayerInfo.summons <= AllPlayersByOrder.FindAll(f => f.pseudo.IndexOf(myPlayerInfo.pseudo + "$", StringComparison.Ordinal) != -1).Count)
                {
                    // pas assez de point d'invoc pour les sorts d'invocation
                    Bmp __spellCursor = new Bmp(@"gfx\general\obj\1\nopc.dat", new Point(-20, -20), "__spellCursor", Manager.TypeGfx.Top, true, 1);
                    Manager.manager.GfxTopList.Add(__spellCursor);
                    Manager.manager.mainForm.Cursor = new Cursor(__spellCursor.bmp.GetHicon());
                }
                else
                {
                    if (myPlayerInfo.BuffsList.Exists(f => f.SortID == infos_sorts.sortID))
                    {
                        Actor.Buff piEnv = myPlayerInfo.BuffsList.Find(f => f.SortID == infos_sorts.sortID);

                        if (piEnv != null && (piEnv.relanceInterval > 1 && piEnv.relanceInterval < spells.sort(infos_sorts.sortID).isbl[infos_sorts.level - 1].relanceInterval) || piEnv.relanceParTour == piEnv.playerRoxed.Count)
                        {
                            // check si le sort ne peux pas etre lancé a cause du relanceParTour ou relanceInterval
                            Bmp __spellCursor = new Bmp(@"gfx\general\obj\1\nopc.dat", new Point(-20, -20), "__spellCursor", Manager.TypeGfx.Top, true, 1);
                            Manager.manager.GfxTopList.Add(__spellCursor);
                            Manager.manager.mainForm.Cursor = new Cursor(__spellCursor.bmp.GetHicon());
                        }
                        else
                        {
                            CommonCode.CursorHand_MouseMove(null, null);
                        }
                    }
                    else
                    {
                        CommonCode.CursorHand_MouseMove(null, null);
                    }
                }
            }
            #endregion
        }
    }

    public class TagedBattleForSpectators
    {
        // cette mini class dérivée de Battle contiens que des champs pour contenir les infos des combats lancés sur une map
        public int IdBattle;                                     // contiens l'id du combat
        public Enums.BattleType.Type BattleType;
        public List<Actor> AllPlayersByOrder = new List<Actor>();
        public Point MapPoint;
        public Enums.Team.Side TeamSide;
    }
}