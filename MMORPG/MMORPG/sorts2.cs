using MELHARFI.Gfx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMORPG
{
    public static class sorts
    {
        public static sort_Stats sort(int _sortID)
        {
            #region liste de tous les sorts et leurs states
            // classe qui contiens les infos des sort
            // sorts de class naruto
            if (_sortID == 0)
            {
                sort_Stats ss = new sort_Stats();
                ss.title = "rasengan";
                ss.classID = 0;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = "futon";
                ss.positionPlayer = 0;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = false;
                isbllvl1.domMin = 5;
                isbllvl1.domMax = 8;
                isbllvl1.cd = 40;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = false;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 10;
                isbllvl2.cd = 35;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = false;
                isbllvl3.domMin = 9;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = false;
                isbllvl4.domMin = 11;
                isbllvl4.domMax = 14;
                isbllvl4.cd = 25;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = false;
                isbllvl5.domMin = 13;
                isbllvl5.domMax = 16;
                isbllvl5.cd = 20;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
            }
            else if (_sortID == 1)
            {
                sort_Stats ss = new sort_Stats();
                ss.title = "shuriken";
                ss.classID = 1;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = "doton";
                ss.positionPlayer = 0;

                // initialisation des stats du sorts selon le lvl
                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level ();
				isbllvl1.etendu = 2;
				isbllvl1.etenduModifiable = true;
				isbllvl1.domMin = 5;
				isbllvl1.domMax = 8;
				isbllvl1.cd = 40;
				ss.isbl.Add (isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 10;
                isbllvl2.cd = 35;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.domMin = 9;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.domMin = 11;
                isbllvl4.domMax = 14;
                isbllvl4.cd = 25;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.domMin = 13;
                isbllvl5.domMax = 16;
                isbllvl5.cd = 20;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
            }
            else if (_sortID == 2)
            {
                sort_Stats ss = new sort_Stats();
                ss.title = "rasen shuriken";
                ss.classID = 2;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = "futon";
                ss.positionPlayer = 0;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.domMin = 5;
                isbllvl1.domMax = 8;
                isbllvl1.cd = 40;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 10;
                isbllvl2.cd = 35;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.domMin = 9;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.domMin = 11;
                isbllvl4.domMax = 14;
                isbllvl4.cd = 25;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.domMin = 13;
                isbllvl5.domMax = 16;
                isbllvl5.cd = 20;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
            }
            else if (_sortID == 3)
            {
                sort_Stats ss = new sort_Stats();
                ss.title = "kage bunshin no jutsu";
                ss.classID = 3;
                ss.technique = "ninjutsu";
                ss.rang = "b";
                ss.element = "neutre";

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 1;
                isbllvl1.etenduModifiable = false;
                isbllvl1.domMin = 10;
                isbllvl1.domMax = 12;
                isbllvl1.cd = 40;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 2;
                isbllvl2.etenduModifiable = false;
                isbllvl2.domMin = 15;
                isbllvl2.domMax = 17;
                isbllvl2.cd = 35;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 3;
                isbllvl3.etenduModifiable = false;
                isbllvl3.domMin = 0;
                isbllvl3.domMax = 0;
                isbllvl3.cd = 30;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 4;
                isbllvl4.etenduModifiable = false;
                isbllvl4.domMin = 19;
                isbllvl4.domMax = 21;
                isbllvl4.cd = 25;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 5;
                isbllvl5.etenduModifiable = false;
                isbllvl5.domMin = 23;
                isbllvl5.domMax = 25;
                isbllvl5.cd = 20;
                ss.isbl.Add(isbllvl5);
                ///////////////////////////////////////////////////
                return ss;
            }
            else if (_sortID == 4)
            {
                // sort dom terre de l'invocation clone naruto
                sort_Stats ss = new sort_Stats();
                ss.title = "pounch";
                ss.classID = 4;
                ss.technique = "taijutsu";
                ss.rang = "d";
                ss.element = "doton";

                // l'invoc ne peux utiliser que le lvl 1 de ses sorts, pour le moment
                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 1;
                isbllvl1.etenduModifiable = false;
                isbllvl1.domMin = 4;
                isbllvl1.domMax = 6;
                isbllvl1.cd = 40;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 1;
                isbllvl2.etenduModifiable = false;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 9;
                isbllvl2.cd = 35;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 1;
                isbllvl3.etenduModifiable = false;
                isbllvl3.domMin = 10;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 1;
                isbllvl4.etenduModifiable = false;
                isbllvl4.domMin = 13;
                isbllvl4.domMax = 15;
                isbllvl4.cd = 25;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 1;
                isbllvl5.etenduModifiable = false;
                isbllvl5.domMin = 16;
                isbllvl5.domMax = 18;
                isbllvl5.cd = 20;
                ss.isbl.Add(isbllvl5);

                return ss;
            }
            else
                throw new Exception("nan");
            #endregion
        }
        public static List<Point> spellPositions = new List<Point>();
        //public static List<spellInfo> spellData = new List<spellInfo>();
        static sorts()
        {
            // CONSTRUCTEUR qui contiens la position des sort pour qu'ils sois biens allignés
            spellPositions.Add(new Point(6, 4));
            spellPositions.Add(new Point(40, 4));
            spellPositions.Add(new Point(72, 4));
            spellPositions.Add(new Point(106, 4));
        }
        public static void animSpellAction(string player, Point TargetPlayer, int sortID, string colorID, Int16 sortLvl, string domString)
        {
            PlayersInfosInBattle playerOfSpell = Battle.AllPlayersByOrder.Find(f => f.Name == player);
            //PlayersInfosInBattle playerTargeted = Battle.AllPlayersByOrder.Find(f => f.Name == player_roxed);
            // calculateDom(typeRox:rox ou heal|jet:x|cd:true ou false|chakra:futon...|dom:x|deadList:joueurMort1:joueurMort2...|roxed    séparé par # s'il sagit de plusieurs
            // methode qui anime les sort, leurs deplacement / animation / changement de l'image du joueur ...

            if (playerOfSpell == null || TargetPlayer == null)
                return;

            commune.blockNetFlow = true;
            if (sortID == 0)
            {
                #region sort rasengan
                // sort rasengan, class naruto
                // changement de l'image du joueur pour la position du lancemnt du sort
                // determination de la direction du sort
                string directionOfSpell;
                
                if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X == TargetPlayer.X && (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y > TargetPlayer.Y)
                {
                    // changement de la direction du joueur vers le haut
                    playerOfSpell.Orientation = 3;
                    (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 3;
                    playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    directionOfSpell = "up";
                }
                else if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X == TargetPlayer.X && (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y < TargetPlayer.Y)
                {
                    // changement de la direction du joueur vers le bas
                    playerOfSpell.Orientation = 0;
                    (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 0;
                    playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    directionOfSpell = "down";
                }
                else if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X > TargetPlayer.X && (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y == TargetPlayer.Y)
                {
                    // changement de la direction du joueur vers la gauche
                    playerOfSpell.Orientation = 1;
                    (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 1;
                    playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    directionOfSpell = "left";
                    if (playerOfSpell.Classe == "naruto")
                        playerOfSpell.ibPlayer.point.X -= 8;
                }
                else
                {
                    // changement de la direction du joueur vers la droite
                    playerOfSpell.Orientation = 2;
                    (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 2;
                    playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    directionOfSpell = "right";
                }              

                // diminution des pv du joueur roxé
                if (domString != "null")
                {
                    for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                    {
                        string data = domString.Split('#')[cnt];
                        string[] DomString = data.Split('|');
                        string typeRox = DomString[0].Split(':')[1];
                        int jet = Convert.ToInt16(DomString[1].Split(':')[1]);
                        bool cd = Convert.ToBoolean(DomString[2].Split(':')[1]);
                        string chakra = DomString[3].Split(':')[1];
                        int dom = Convert.ToInt32(DomString[4].Split(':')[1]);
                        List<string> deadList = DomString[5].Split(':').ToList();
                        deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                        if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                            deadList.Clear();
                        string roxed = DomString[6];
                        PlayersInfosInBattle playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.Name == roxed);

                        // affichage des données du damages sur la zone chat
                        Manager.manager.mainForm.BeginInvoke((Action)(() =>
                        {
                            commune.ChatMsgFormat("B", player + ":" + roxed, domString);
                        }));

                        if (typeRox == "rox")
                        {
                            playerTargeted2.CurrentPdv -= dom;
                            // retrait de 5% des dom au total pdv du joueur roxé comme érosion
                            int ero = (dom * 5) / 100;
                            playerTargeted2.TotalPdv -= ero;
                            if (playerTargeted2.CurrentPdv <= 0)
                                playerTargeted2.CurrentPdv = 0;

                            // s'il sagit de notre joueur, actualiser la barre de ses points
                            HudeHandle.UpdateHealth();
                        }
                    }
                }

                // affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__redraw_Spell_Thread";
                    Anim __rasengan = new Anim(30, true);
                    long startTime = Environment.TickCount;                     // contien le temps pour le timer créer avec une la boucle whiel
                    bool createSpellOnce = false;                               // variable de controle pour la création du spell 1 seul fois dans la boucle while
                    bool moveSpell = true;                                      // variable de controle pour faire avancer le sort jusqu'au joueur attaqu;
                    System.Media.SoundPlayer rasengan_concentration, rasengan_hit;

                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        if (Environment.TickCount <= startTime + 1000 && !createSpellOnce)
                        {
                            #region
                            // lancement de sort rasengan_concentration.wav
                             rasengan_concentration = new System.Media.SoundPlayer(@"sfx\spell\rasengan_concentration.dat");
                            rasengan_concentration.Play();

                            // dessin du sort
                            Point pp = (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition;

                            // dessin d'une animation de rasengan
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\0.dat", 0, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\1.dat", 1, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\2.dat", 2, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\3.dat", 3, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\4.dat", 4, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\5.dat", 5, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\6.dat", 6, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\7.dat", 7, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\8.dat", 8, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\9.dat", 9, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\10.dat", 10, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.AddCell(@"gfx\general\sorts\" + sortID + @"\11.dat", 11, 0, 0, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1));
                            __rasengan.Ini(Manager.typeGfx.obj, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_spell" + sortID + colorID, sortLvl - 1), "__rasengan");
                            __rasengan.PointOfParent = true;
                            // mise en position du sort
                            if (directionOfSpell == "up")
                                __rasengan.img.point = new Point((pp.X * 30) + 15 - (__rasengan.img.rectangle.Width / 2), (pp.Y * 30) - 40);
                            else if (directionOfSpell == "down")
                                __rasengan.img.point = new Point((pp.X * 30) + 15 - (__rasengan.img.rectangle.Width / 2), (pp.Y * 30) + 20);
                            else if (directionOfSpell == "right")
                                __rasengan.img.point = new Point(((pp.X + 1) * 30) + 15 - (__rasengan.img.rectangle.Width / 2), (pp.Y * 30) - (__rasengan.img.rectangle.Height / 2) - 10);
                            else
                                __rasengan.img.point = new Point(((pp.X - 1) * 30) + 15 - (__rasengan.img.rectangle.Width / 2), (pp.Y * 30) - (__rasengan.img.rectangle.Height / 2) - 10);
                            Manager.manager.GfxObjList.Add(__rasengan);
                            commune.VerticalSyncZindex(__rasengan.img);

                            if (directionOfSpell == "up")
                                __rasengan.img.zindex--;

                            __rasengan.Start();
                            createSpellOnce = true;
                            #endregion
                        }
                        else if (Environment.TickCount > startTime + 1000 && moveSpell)
                        {
                            #region // mouvement de sort jusqu'a atteindre l'adversaire en question
                            if (directionOfSpell == "left")
                            {
                                __rasengan.img.point.X -= 10;
                                if ((__rasengan.img.point.X / 30) <= TargetPlayer.X)
                                {
                                    //arrivé du sort pret du joueur
                                    //centrage et mise en place du sort sur la case
                                    if (__rasengan.img.point.X + (__rasengan.img.rectangle.Width / 2) <= ((TargetPlayer.X) * 30) + 15)
                                        moveSpell = false;
                                }
                            }
                            else if (directionOfSpell == "right")
                            {
                                __rasengan.img.point.X += 10;
                                if ((__rasengan.img.point.X / 30) + 1 >= TargetPlayer.X)
                                {
                                    //arrivé du sort pret du joueur
                                    //centrage et mise en place du sort sur la case
                                    if (__rasengan.img.point.X + (__rasengan.img.rectangle.Width / 2) >= ((TargetPlayer.X) * 30) + 15)
                                        moveSpell = false;
                                }
                            }
                            else if (directionOfSpell == "up")
                            {
                                __rasengan.img.point.Y -= 10;
                                if ((__rasengan.img.point.Y / 30) + 1 <= TargetPlayer.Y)
                                    moveSpell = false;
                            }
                            else if (directionOfSpell == "down")
                            {
                                __rasengan.img.point.Y += 10;
                                if ((__rasengan.img.point.Y / 30) >= TargetPlayer.Y)
                                    moveSpell = false;
                            }
                            #endregion
                        }
                        else if (createSpellOnce && !moveSpell)
                        {
                            #region changement de l'image du joueur roxé pour faire semblement de recevoir des dom
                            // faire disparaitre le resengan
                            __rasengan.Visible(false);
                            __rasengan.Close();
                            Manager.manager.GfxObjList.Remove(__rasengan);
                            __rasengan = null;

                            // affichage de l'animation de recovoir des dom
                            //////////////////////
                            if (domString != "null")
                            {
                                // affichage des dom en haut de personnage
                                new Thread(new ThreadStart(() =>
                                {
                                    Thread.CurrentThread.Name = "__Dom_Acros_Player_Thread";
                                    for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                                    {
                                        string[] data = domString.Split('#');
                                        string tmp = data[cnt].ToString();

                                        new Thread(new ThreadStart(() =>
                                        {
                                            string roxed = tmp.Split('|')[6];
                                            PlayersInfosInBattle playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.Name == roxed);
                                            Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                            //commune.showAnimDamageAbove(playerTargeted2, tmp);
                                            commune.showAnimDamageAbove(playerTargeted2.realPosition, playerTargeted2.ibPlayer.rectangle, playerTargeted2.ibPlayer.zindex, tmp);
                                        })).Start();

                                        string[] DomString = tmp.Split('|');
                                        List<string> deadList = DomString[5].Split(':').ToList();
                                        deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                                        if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                            deadList.Clear();

                                        // check si un ou plusieurs joueurs sont mort
                                        if (deadList.Count > 0)
                                        {
                                            for (int i = 0; i < deadList.Count; i++)
                                            {
                                                Bmp ibPlayer = commune.AllPlayers.Find(f => (f.tag as PlayerInfo).Pseudo == deadList[i]);
                                                if (ibPlayer != null)
                                                {
                                                    new Thread(new ThreadStart(() =>
                                                    {
                                                        Thread.CurrentThread.Name = "__Animate_Dead_Player_Thread";
                                                        commune.animDeadPlayer(ibPlayer);
                                                    })).Start();
                                                }
                                                else
                                                {
                                                    // code impossible a atteindre puisqu'il dois y avoir un joueur mort,donc une non synchronisation entre le serveur et le client
                                                    // il faut peux etre penser a demander au client une cmd de resynchronisation des states
                                                }
                                            }
                                        }

                                        // supprimer notre joueur si la liste deadList est plus grande que 0
                                        if(deadList.Count > 0)
                                        {
                                            for(int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                            {
                                                Battle.DeadPlayers.Add((PlayersInfosInBattle)(Battle.AllPlayersByOrder.Find(f => f.Name == deadList[cnt2])).Clone());
                                                Battle.Team1.RemoveAll(f => f.Name == deadList[cnt2]);
                                                Battle.Team2.RemoveAll(f => f.Name == deadList[cnt2]);
                                                Battle.AllPlayersByOrder.RemoveAll(f => f.Name == deadList[cnt2]);
                                            }

                                            // on actualise la timeline
                                            Battle.refreshTimeLine();
                                        }

                                        Thread.Sleep(100);
                                    }
                                    commune.blockNetFlow = false;
                                })).Start();
                            }
                            else
                                commune.blockNetFlow = false;

                            // playe rasengan_hit.wav
                            rasengan_hit = new System.Media.SoundPlayer(@"sfx\spell\rasengan_hit.dat");
                            rasengan_hit.Play();

                            Anim __rasengan_dom_effect = new Anim(60, true);
                            __rasengan_dom_effect.AddCell(@"gfx\general\obj\3\ora\2\color" + colorID + @"\" + sortLvl + @"\1.dat", 1, 100, 100);
                            __rasengan_dom_effect.AddCell(@"gfx\general\obj\3\ora\2\color" + colorID + @"\" + sortLvl + @"\2.dat", 2, 100, 100);
                            __rasengan_dom_effect.AddCell(@"gfx\general\obj\3\ora\2\color" + colorID + @"\" + sortLvl + @"\3.dat", 3, 100, 100);
                            __rasengan_dom_effect.AddCell(@"gfx\general\obj\3\ora\2\color" + colorID + @"\" + sortLvl + @"\4.dat", 4, 100, 100);
                            __rasengan_dom_effect.AddCell(@"gfx\general\obj\3\ora\2\color" + colorID + @"\" + sortLvl + @"\5.dat", 5, 100, 100);
                            __rasengan_dom_effect.AddCell(@"gfx\general\obj\3\ora\2\color" + colorID + @"\" + sortLvl + @"\6.dat", 6, 100, 100);
                            __rasengan_dom_effect.Ini(Manager.typeGfx.obj, "__rasengan_dom_effect");
                            __rasengan_dom_effect.img.point.X = (TargetPlayer.X * 30) - (__rasengan_dom_effect.img.rectangle.Width / 2);
                            __rasengan_dom_effect.img.point.Y = (TargetPlayer.Y * 30) - (__rasengan_dom_effect.img.rectangle.Height - 30) + 5 + sortLvl;
                            __rasengan_dom_effect.img.zindex = playerOfSpell.ibPlayer.zindex + 1;
                            __rasengan_dom_effect.PointOfParent = true;
                            __rasengan_dom_effect.AutoResetAnim = false;
                            __rasengan_dom_effect.HideAtLastFrame = true;
                            __rasengan_dom_effect.DestroyAfterLastFrame = true;
                            __rasengan_dom_effect.Start();
                            Manager.manager.GfxObjList.Add(__rasengan_dom_effect);
                            //////////////////////
                            break;
                            #endregion
                        }
                        Thread.Sleep(20);
                    }
                })).Start();

                // compteur pour remetre le joueur sur sa sprite d'origine apres le lancement du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__redraw_Player_After_Spell_Thread";
                    long startTime = Environment.TickCount;
                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        if (Environment.TickCount >= startTime + 2000)
                        {
                            // invocation du thread principale pour modifier l'image du joueur
                            Manager.manager.mainForm.BeginInvoke((Action)(() =>
                            {
                                Point pp = new Point((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X, (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y);
                                playerOfSpell.ibPlayer.point.X = (pp.X * 30) + 15 - (playerOfSpell.ibPlayer.rectangle.Width / 2);
                                playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\" + playerOfSpell.Classe + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe, playerOfSpell.Orientation * 4));
                            }));
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                })).Start();
                #endregion
            }
            else if (sortID == 1)
            {
                #region sort shuriken
                // changement de position du joueur
                if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X == TargetPlayer.X)
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y > TargetPlayer.Y)
                    {
                        // changement de la direction du joueur vers le haut
                        playerOfSpell.Orientation = 3;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 3;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers le bas
                        playerOfSpell.Orientation = 0;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 0;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }
                else if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y == TargetPlayer.Y)
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X > TargetPlayer.X)
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 1;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 1;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers la droite
                        playerOfSpell.Orientation = 2;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 2;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }
                else
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X > TargetPlayer.X)
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 1;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 1;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 2;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 2;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }

                // sort shuriken
                int xDistance = (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X - TargetPlayer.X;
                int yDistance = (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y - TargetPlayer.Y;

                //commune.blockNetFlow = false;
                // diminution des pv du joueur roxé
                if (domString != "null")
                {
                    for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                    {
                        string data = domString.Split('#')[cnt];
                        string[] DomString = data.Split('|');
                        string typeRox = DomString[0].Split(':')[1];
                        int jet = Convert.ToInt16(DomString[1].Split(':')[1]);
                        bool cd = Convert.ToBoolean(DomString[2].Split(':')[1]);
                        string chakra = DomString[3].Split(':')[1];
                        int dom = Convert.ToInt32(DomString[4].Split(':')[1]);
                        List<string> deadList = DomString[5].Split(':').ToList();
                        deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                        if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                            deadList.Clear();
                        string roxed = DomString[6];
                        PlayersInfosInBattle playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.Name == roxed);

                        // affichage des données du damages sur la zone chat
                        Manager.manager.mainForm.BeginInvoke((Action)(() =>
                        {
                            commune.ChatMsgFormat("B", player + ":" + playerTargeted2.Name, domString);
                        }));

                        if (typeRox == "rox")
                        {
                            playerTargeted2.CurrentPdv -= dom;
                            // retrait de 5% des dom au total pdv du joueur roxé comme érosion
                            int ero = (dom * 5) / 100;
                            playerTargeted2.TotalPdv -= ero;
                            if (playerTargeted2.CurrentPdv <= 0)
                                playerTargeted2.CurrentPdv = 0;

                            // s'il sagit de notre joueur, actualiser la barre de ses points
                            HudeHandle.UpdateHealth();
                        }
                    }
                }

                // affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__redraw_Shuriken_Spell_Thread";
                    long startTime = Environment.TickCount;                     // contien le temps pour le timer créer avec une la boucle whiel

                    Anim __shurikenSpell = new Anim(15, true);
                    __shurikenSpell.AddCell(@"gfx\general\sorts\" + sortID + @"\0.dat", 0, 0, 0, SpriteSheet.GetSpriteSheet("shuriken", 0));
                    __shurikenSpell.AddCell(@"gfx\general\sorts\" + sortID + @"\0.dat", 1, 0, 0, SpriteSheet.GetSpriteSheet("shuriken", 1));
                    __shurikenSpell.AddCell(@"gfx\general\sorts\" + sortID + @"\0.dat", 2, 0, 0, SpriteSheet.GetSpriteSheet("shuriken", 2));
                    __shurikenSpell.AddCell(@"gfx\general\sorts\" + sortID + @"\0.dat", 3, 0, 0, SpriteSheet.GetSpriteSheet("shuriken", 3));
                    __shurikenSpell.AddCell(@"gfx\general\sorts\" + sortID + @"\0.dat", 4, 0, 0, SpriteSheet.GetSpriteSheet("shuriken", 4));
                    __shurikenSpell.AddCell(@"gfx\general\sorts\" + sortID + @"\0.dat", 5, 0, 0, SpriteSheet.GetSpriteSheet("shuriken", 5));
                    __shurikenSpell.Ini(Manager.typeGfx.obj, SpriteSheet.GetSpriteSheet("shuriken", 0), "__shuriken");
                    __shurikenSpell.img.point.X = ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X * 30) + 15 - (__shurikenSpell.img.rectangle.Width / 2);
                    __shurikenSpell.img.point.Y = ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y * 30) - (__shurikenSpell.img.rectangle.Height - 30);
                    commune.VerticalSyncZindex(__shurikenSpell.img);
                    __shurikenSpell.PointOfParent = true;
                    __shurikenSpell.AutoResetAnim = true;
                    __shurikenSpell.Start();
                    Manager.manager.GfxObjList.Add(__shurikenSpell);

                    Point start = new Point(((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X * 30) + 15 - (__shurikenSpell.img.rectangle.Width / 2), playerOfSpell.ibPlayer.point.Y + (playerOfSpell.ibPlayer.rectangle.Height / 2) - (__shurikenSpell.img.rectangle.Height / 2));
                    Point end = new Point((TargetPlayer.X * 30) + 15 - (__shurikenSpell.img.rectangle.Width / 2), (TargetPlayer.Y * 30) + 15 - (__shurikenSpell.img.rectangle.Height / 2));

                    int speed = (int)Math.Sqrt(Math.Max(Math.Abs(xDistance), Math.Abs(yDistance)) * 30) / 2;
                    List<PointF> waypoint = commune.calculTrajectory(start, end, speed);
                    waypoint.Add(end);
                    for (int cnt = 0; cnt < waypoint.Count && !Manager.manager.mainForm.IsDisposed; cnt++)
                    {
                        __shurikenSpell.img.point.X = (int)Math.Round(waypoint[cnt].X);
                        __shurikenSpell.img.point.Y = (int)Math.Round(waypoint[cnt].Y);
                        commune.VerticalSyncZindex(__shurikenSpell.img);
                        Thread.Sleep(50);
                    }

                    // supression de l'image
                    __shurikenSpell.Visible(false);
                    __shurikenSpell.Close();
                    Manager.manager.GfxObjList.Remove(__shurikenSpell);
                    __shurikenSpell = null;

                    // affichage des dom en haut de personnage
                    if (domString != "null")
                    {
                        new Thread(new ThreadStart(() =>
                        {
                            Thread.CurrentThread.Name = "__Dom_Acros_Player_Thread";
                            for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                            {
                                string[] data = domString.Split('#');
                                string tmp = data[cnt].ToString();

                                string roxed = tmp.Split('|')[6];
                                PlayersInfosInBattle playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.Name == roxed);

                                Point playerTargetedPoint = playerTargeted2.realPosition;
                                Rectangle playerTargetedRectangle = playerTargeted2.ibPlayer.rectangle;
                                int playerTargetedZindex = playerTargeted2.ibPlayer.zindex;

                                new Thread(new ThreadStart(() =>
                                {
                                    Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                    commune.showAnimDamageAbove(playerTargetedPoint, playerTargetedRectangle, playerTargetedZindex, tmp);
                                })).Start();


                                string[] DomString = tmp.Split('|');
                                List<string> deadList = DomString[5].Split(':').ToList();
                                deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                                if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                    deadList.Clear();

                                // check si un ou plusieurs joueurs sont mort
                                if (deadList.Count > 0)
                                {
                                    for (int i = 0; i < deadList.Count; i++)
                                    {
                                        Bmp ibPlayer = commune.AllPlayers.Find(f => (f.tag as PlayerInfo).Pseudo == deadList[i]);
                                        if (ibPlayer != null)
                                        {
                                            //new Thread(new ThreadStart(() =>
                                            //{
                                                //Thread.CurrentThread.Name = "__Animate_Dead_Player_Thread";
                                                commune.animDeadPlayer(ibPlayer);
                                            //})).Start();
                                        }
                                        else
                                        {
                                            // code impossible a atteindre puisqu'il dois y avoir un joueur mort,donc une non synchronisation entre le serveur et le client
                                            // il faut peux etre penser a demander au client une cmd de resynchronisation des states
                                        }
                                    }
                                }

                                // supprimer notre joueur si la liste deadList est plus grande que 0
                                if (deadList.Count > 0)
                                {
                                    for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                    {
                                        // supprimer le joueur de la time line
                                        IGfx cadron = Manager.manager.GfxTopList.Find(f => f.Name() == "_cadre_p_" + deadList[cnt2]);
                                        if (cadron != null)
                                        {
                                            (cadron as Bmp).Visible = false;
                                            (cadron as Bmp).Child = null;
                                            Manager.manager.GfxTopList.Remove(cadron);
                                        }

                                        // supprimer le joueur des listes
                                        Battle.DeadPlayers.Add((PlayersInfosInBattle)(Battle.AllPlayersByOrder.Find(f => f.Name == deadList[cnt2])).Clone());
                                        Battle.Team1.RemoveAll(f => f.Name == deadList[cnt2]);
                                        Battle.Team2.RemoveAll(f => f.Name == deadList[cnt2]);
                                        Battle.AllPlayersByOrder.RemoveAll(f => f.Name == deadList[cnt2]);
                                    }

                                    // on actualise la timeline
                                    Battle.refreshTimeLine();
                                }
                                Thread.Sleep(100);
                            }
                            commune.blockNetFlow = false;
                        })).Start();
                    }
                    else
                        commune.blockNetFlow = false;

                    // annimation sur le personnage qui recois les dom
                    if (domString != "null")
                    {
                        Anim __animPlayerDom = new Anim(20, true);
                        for (int cnt = 1; cnt <= 40; cnt++)
                            __animPlayerDom.AddCell(@"gfx\general\obj\3\ora\1\" + cnt + ".dat", cnt, 100, 100);
                        __animPlayerDom.Ini(Manager.typeGfx.obj, "__animPlayerDom");
                        __animPlayerDom.img.point.X = (TargetPlayer.X * 30) + 15 - (__animPlayerDom.img.rectangle.Width / 2);
                        __animPlayerDom.img.point.Y = (TargetPlayer.Y * 30) - (__animPlayerDom.img.rectangle.Height - 30);
                        __animPlayerDom.img.zindex = (TargetPlayer.Y * 100) + 99;
                        __animPlayerDom.PointOfParent = true;
                        __animPlayerDom.AutoResetAnim = false;
                        __animPlayerDom.HideAtLastFrame = true;
                        __animPlayerDom.DestroyAfterLastFrame = true;
                        __animPlayerDom.Start();
                        Manager.manager.GfxObjList.Add(__animPlayerDom);
                    }

                    // invocation du thread principale pour modifier l'image du joueur
                    Manager.manager.mainForm.BeginInvoke((Action)(() =>
                    {
                        Point pp = new Point((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X, (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y);
                        playerOfSpell.ibPlayer.point.X = (pp.X * 30) + 15 - (playerOfSpell.ibPlayer.rectangle.Width / 2);
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\" + playerOfSpell.Classe + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe, playerOfSpell.Orientation * 4));
                    }));
                })).Start();
                #endregion
            }
            else if (sortID == 2)
            {
                #region sort rasen shuriken
                // changement de position du joueur
                if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X == TargetPlayer.X)
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y > TargetPlayer.Y)
                    {
                        // changement de la direction du joueur vers le haut
                        playerOfSpell.Orientation = 3;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 3;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers le bas
                        playerOfSpell.Orientation = 0;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 0;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }
                else if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y == TargetPlayer.Y)
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X > TargetPlayer.X)
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 1;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 1;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers la droite
                        playerOfSpell.Orientation = 2;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 2;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }
                else
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X > TargetPlayer.X)
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 1;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 1;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 2;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 2;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }

                // sort shuriken
                int xDistance = (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X - TargetPlayer.X;
                int yDistance = (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y - TargetPlayer.Y;

                //commune.blockNetFlow = false;
                // diminution des pv du joueur roxé
                if (domString != "null")
                {
                    // on desassemble le variable domString afin de trouver les données qui au rox pour les afficher dans la zone de chat
                    for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                    {
                        string data = domString.Split('#')[cnt];
                        string[] DomString = data.Split('|');
                        string typeRox = DomString[0].Split(':')[1];
                        int jet = Convert.ToInt16(DomString[1].Split(':')[1]);
                        bool cd = Convert.ToBoolean(DomString[2].Split(':')[1]);
                        string chakra = DomString[3].Split(':')[1];
                        int dom = Convert.ToInt32(DomString[4].Split(':')[1]);
                        List<string> deadList = DomString[5].Split(':').ToList();
                        deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                        if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                            deadList.Clear();
                        string roxed = DomString[6];
                        PlayersInfosInBattle playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.Name == roxed);

                        // affichage des données du damages sur la zone chat
                        Manager.manager.mainForm.BeginInvoke((Action)(() =>
                        {
                            commune.ChatMsgFormat("B", player + ":" + playerTargeted2.Name, domString);
                        }));

                        if (typeRox == "rox")
                        {
                            playerTargeted2.CurrentPdv -= dom;
                            // retrait de 5% des dom au total pdv du joueur roxé comme érosion
                            int ero = (dom * 5) / 100;
                            playerTargeted2.TotalPdv -= ero;
                            if (playerTargeted2.CurrentPdv <= 0)
                                playerTargeted2.CurrentPdv = 0;

                            // s'il sagit de notre joueur, actualiser la barre de ses points
                            HudeHandle.UpdateHealth();
                        }
                    }
                }
                // affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__redraw_rasen_shuriken_Spell_Thread";
                    long startTime = Environment.TickCount;                     // contien le temps pour le timer créer avec une la boucle whiel

                    Anim __rasen_shuriken_Spell = new Anim(15, true);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\0.dat", 0, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\1.dat", 1, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\2.dat", 2, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\3.dat", 3, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\4.dat", 4, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\5.dat", 5, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\6.dat", 6, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\7.dat", 7, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\8.dat", 8, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\9.dat", 9, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\10.dat", 10, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\11.dat", 11, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\12.dat", 12, 0, 0);
                    __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\13.dat", 13, 0, 0);

                    __rasen_shuriken_Spell.Ini(Manager.typeGfx.obj);
                    commune.VerticalSyncZindex(__rasen_shuriken_Spell.img);
                    __rasen_shuriken_Spell.PointOfParent = true;
                    __rasen_shuriken_Spell.AutoResetAnim = true;
                    __rasen_shuriken_Spell.Start();
                    Manager.manager.GfxObjList.Add(__rasen_shuriken_Spell);

                    Point start = new Point(((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X * 30) + 30 - (__rasen_shuriken_Spell.img.rectangle.Width / 2), playerOfSpell.ibPlayer.point.Y - __rasen_shuriken_Spell.img.rectangle.Height - 10);
                    Point end = new Point((TargetPlayer.X * 30) + 15 - (__rasen_shuriken_Spell.img.rectangle.Width / 2), (TargetPlayer.Y * 30) + 15 - (__rasen_shuriken_Spell.img.rectangle.Height / 2));
                    __rasen_shuriken_Spell.img.point = start;

                    int speed = (int)Math.Sqrt(Math.Max(Math.Abs(xDistance), Math.Abs(yDistance)) * 30) / 2;
                    List<PointF> waypoint = commune.calculTrajectory(start, end, speed);
                    waypoint.Add(end);

                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        if (Environment.TickCount >= startTime + 1000)
                            break;
                        Thread.Sleep(20);
                    }

                    for (int cnt = 0; cnt < waypoint.Count && !Manager.manager.mainForm.IsDisposed; cnt++)
                    {
                        __rasen_shuriken_Spell.img.point.X = (int)Math.Round(waypoint[cnt].X);
                        __rasen_shuriken_Spell.img.point.Y = (int)Math.Round(waypoint[cnt].Y);
                        commune.VerticalSyncZindex(__rasen_shuriken_Spell.img);
                        Thread.Sleep(50);
                    }

                    // supression de l'image
                    __rasen_shuriken_Spell.Visible(false);
                    __rasen_shuriken_Spell.Close();
                    Manager.manager.GfxObjList.Remove(__rasen_shuriken_Spell);
                    __rasen_shuriken_Spell = null;

                    if (domString != "null")
                    {
                        // affichage des dom en haut de personnage
                        //new Thread(new ThreadStart(() =>
                        //{
                            //Thread.CurrentThread.Name = "__Dom_Acros_Player_Thread";
                            for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                            {
                                string[] data = domString.Split('#');
                                string tmp = data[cnt].ToString();

                                string roxed = tmp.Split('|')[6];
                                PlayersInfosInBattle playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.Name == roxed);

                                Point playerTargetedPoint = playerTargeted2.realPosition;
                                Rectangle playerTargetedRectangle = playerTargeted2.ibPlayer.rectangle;
                                int playerTargetedZindex = playerTargeted2.ibPlayer.zindex;

                                new Thread(new ThreadStart(() =>
                                {
                                    Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                    commune.showAnimDamageAbove(playerTargetedPoint, playerTargetedRectangle, playerTargetedZindex, tmp);
                                })).Start();

                                string[] DomString = tmp.Split('|');
                                List<string> deadList = DomString[5].Split(':').ToList();
                                deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                                if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                    deadList.Clear();

                                // check si un ou plusieurs joueurs sont mort
                                if (deadList.Count > 0)
                                {
                                    for (int i = 0; i < deadList.Count; i++)
                                    {
                                        Bmp ibPlayer = commune.AllPlayers.Find(f => (f.tag as PlayerInfo).Pseudo == deadList[i]);
                                        if (ibPlayer != null)
                                        {
                                            //new Thread(new ThreadStart(() =>
                                            //{
                                                //Thread.CurrentThread.Name = "__Animate_Dead_Player_Thread";
                                                commune.animDeadPlayer(ibPlayer);
                                            //})).Start();
                                        }
                                        else
                                        {
                                            // code impossible a atteindre puisqu'il dois y avoir un joueur mort,donc une non synchronisation entre le serveur et le client
                                            // il faut peux etre penser a demander au client une cmd de resynchronisation des states
                                        }
                                    }
                                }

                                // supprimer notre joueur si la liste deadList est plus grande que 0
                                if (deadList.Count > 0)
                                {
                                    for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                    {
                                        Battle.DeadPlayers.Add((PlayersInfosInBattle)(Battle.AllPlayersByOrder.Find(f => f.Name == deadList[cnt2])).Clone());
                                        Battle.Team1.RemoveAll(f => f.Name == deadList[cnt2]);
                                        Battle.Team2.RemoveAll(f => f.Name == deadList[cnt2]);
                                        Battle.AllPlayersByOrder.RemoveAll(f => f.Name == deadList[cnt2]);
                                        commune.ChatMsgFormat("S", "null", "player has been dead");
                                    }

                                    // on actualise la timeline
                                    Battle.refreshTimeLine();
                                }
                                Thread.Sleep(100);
                            }
                        //})).Start();
                    }

                    // annimation sur le personnage qui recois les dom
                    Anim __animPlayerDom = new Anim(50, true);
                    for (int cnt = 1; cnt < 13; cnt++)
                        __animPlayerDom.AddCell(@"gfx\general\obj\3\ora\3\" + cnt + ".dat", cnt, 100, 100);
                    __animPlayerDom.Ini(Manager.typeGfx.obj, "__animPlayerDom");
                    __animPlayerDom.img.point.X = (TargetPlayer.X * 30) + 15 - (__animPlayerDom.img.rectangle.Width / 2);
                    __animPlayerDom.img.point.Y = (TargetPlayer.Y * 30) - (__animPlayerDom.img.rectangle.Height - 30) + 10;

                    commune.VerticalSyncZindex(__animPlayerDom.img);

                    __animPlayerDom.PointOfParent = true;
                    __animPlayerDom.AutoResetAnim = false;
                    __animPlayerDom.HideAtLastFrame = true;
                    __animPlayerDom.DestroyAfterLastFrame = true;
                    __animPlayerDom.Start();
                    Manager.manager.GfxObjList.Add(__animPlayerDom);

                    // invocation du thread principale pour modifier l'image du joueur
                    Manager.manager.mainForm.BeginInvoke((Action)(() =>
                    {
                        Point pp = new Point((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X, (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y);
                        playerOfSpell.ibPlayer.point.X = (pp.X * 30) + 15 - (playerOfSpell.ibPlayer.rectangle.Width / 2);
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\" + playerOfSpell.Classe + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe, playerOfSpell.Orientation * 4));
                    }));
                })).Start();
                #endregion
            }
            else if (sortID == 3)
            {
                #region invocation 1 de naruto
                //commun.SendMessage("cmd•spellTileGranted•" + pi.Pseudo + "•3•" + spellPos.X + "•" + spellPos.Y + "•0•" + spellLvl + "•" + dom(typeRox:addInvoc|name:x|cd:x|totalPdv:x), nim, true);
                string nameOfInvoc = domString.Split('|')[1].Split(':')[1];
                bool cd = Convert.ToBoolean(domString.Split('|')[2].Split(':')[1]);
                
                // création d'une invocation
                // orientation
                // determination de l'orientation selon les coordonées x et y
                int x = Math.Abs(TargetPlayer.X - playerOfSpell.realPosition.X);
                int y = Math.Abs(TargetPlayer.Y - playerOfSpell.realPosition.Y);
                short orientation = 0;

                // longeur horizontal plus grande que longeur verticale
                if (x > y)
                {
                    // determination de l'orientation horizontal, à droite ou à gauche
                    if (TargetPlayer.X > playerOfSpell.realPosition.X)
                        orientation = 1;    // orientation à droite
                    else
                        orientation = 3;    // orientation à gauche
                }
                else
                {
                    // determination de l'orientation horizontal, à droite ou à gauche
                    if (TargetPlayer.Y > playerOfSpell.realPosition.Y)
                        orientation = 2;    // orientation en bas
                    else
                        orientation = 0;    // orientation en haut
                }
                //////
                Bmp ibPlayers = new Bmp(@"gfx\general\classes\" + playerOfSpell.Classe + ".dat", Point.Empty, nameOfInvoc, 0, true, true, SpriteSheet.GetSpriteSheet(playerOfSpell.Classe, commune.ConvertToClockWizeOrientation(orientation)));
                ibPlayers.MouseOver += commune.ibPlayers_MouseOver;
                ibPlayers.MouseOut += commune.ibPlayers_MouseOut;
                ibPlayers.MouseMove += commune.CursorHand_MouseMove;
                ibPlayers.MouseClic += commune.ibPlayers_MouseClic;
                Manager.manager.GfxObjList.Add(ibPlayers);

                // attachement des données
                Bmp tmpPi = commune.AllPlayers.Find(f => (f.tag as PlayerInfo).Pseudo == playerOfSpell.Name);
                if (tmpPi == null)
                    MessageBox.Show("impossible de trouver un joueur avec ce nom, bizzard");

                ibPlayers.tag = (tmpPi.tag as PlayerInfo).Clone();
                (ibPlayers.tag as PlayerInfo).realPosition = TargetPlayer;
                int lvlOfSpell = Convert.ToInt32(domString.Split('|')[3].Split(':')[1]);
                (ibPlayers.tag as PlayerInfo).TotalPdv = lvlOfSpell;
                (ibPlayers.tag as PlayerInfo).CurrentPdv = lvlOfSpell;
                (ibPlayers.tag as PlayerInfo).Pseudo = nameOfInvoc;
                (ibPlayers.tag as PlayerInfo).isHuman = false;
                
                commune.AdjustPositionAndDirection(ibPlayers, new Point(TargetPlayer.X * 30, TargetPlayer.Y * 30));
                commune.VerticalSyncZindex(ibPlayers);

                // affichage des ailles
                if ((ibPlayers.tag as PlayerInfo).Pvp == true)
                {
                    if ((ibPlayers.tag as PlayerInfo).Spirit != "neutre")
                    {
                        Bmp spirit = new Bmp(@"gfx\general\obj\2\" + (ibPlayers.tag as PlayerInfo).Spirit + @"\" + (ibPlayers.tag as PlayerInfo).SpiritLvl + ".dat", Point.Empty, "spirit_" + ibPlayers.name, Manager.typeGfx.obj, false, true);
                        spirit.point = new Point((ibPlayers.rectangle.Width / 2) - (spirit.rectangle.Width / 2), -spirit.rectangle.Height);
                        ibPlayers.Child.Add(spirit);

                        Txt lPseudo = new Txt("", Point.Empty, "lPseudo_" + ibPlayers.name, Manager.typeGfx.obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                        // check si c'est une invoc pour afficher que la 1ere partie de son nom qui est séparé par '$' comme naruto$5d11d
                        if (!(ibPlayers.tag as PlayerInfo).isHuman)
                            lPseudo.Text = (ibPlayers.tag as PlayerInfo).Pseudo.Split('$')[0];
                        else
                            lPseudo.Text = (ibPlayers.tag as PlayerInfo).Pseudo;
                        lPseudo.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -spirit.rectangle.Height - 15);
                        ibPlayers.Child.Add(lPseudo);

                        Txt lLvlSpirit = new Txt((ibPlayers.tag as PlayerInfo).SpiritLvl.ToString(), Point.Empty, "lLvlSpirit_" + ibPlayers.name, Manager.typeGfx.obj, false, new Font("Verdana", 10, FontStyle.Bold), Brushes.Red);
                        lLvlSpirit.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Width / 2) + 2, -spirit.rectangle.Y - (spirit.rectangle.Height / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Height / 2));
                        ibPlayers.Child.Add(lLvlSpirit);

                        Bmp village = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", Point.Empty, "village_" + (ibPlayers.tag as PlayerInfo).Village, Manager.typeGfx.obj, false, true, SpriteSheet.GetSpriteSheet("pays_" + (ibPlayers.tag as PlayerInfo).Village + "_thumbs", 0));
                        village.point = new Point((ibPlayers.rectangle.Width / 2) - (village.rectangle.Width / 2), lPseudo.point.Y - village.rectangle.Height + 2);
                        ibPlayers.Child.Add(village);
                    }
                }
                else
                {
                    Txt lPseudo = new Txt((ibPlayers.tag as PlayerInfo).Pseudo, Point.Empty, "lPseudo_" + ibPlayers.name, Manager.typeGfx.obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                    lPseudo.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -15);
                    ibPlayers.Child.Add(lPseudo);

                    Bmp village = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", Point.Empty, "village_" + (ibPlayers.tag as PlayerInfo).Village, Manager.typeGfx.obj, false, true, SpriteSheet.GetSpriteSheet("pays_" + (ibPlayers.tag as PlayerInfo).Village + "_thumbs", 0));
                    village.point = new Point((ibPlayers.rectangle.Width / 2) - (village.rectangle.Width / 2), lPseudo.point.Y - village.rectangle.Height + 2);
                    ibPlayers.Child.Add(village);
                }

                // coloriage selon le MaskColors
                if (playerOfSpell.MaskColors.Split('/')[0] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[0].Split('-')[0]), Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[0].Split('-')[1]), Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[0].Split('-')[2]));
                    commune.SetPixelToClass(playerOfSpell.Classe, tmpColor, 1, ibPlayers);
                }

                if (playerOfSpell.MaskColors.Split('/')[1] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[1].Split('-')[0]), Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[1].Split('-')[1]), Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[1].Split('-')[2]));
                    commune.SetPixelToClass(playerOfSpell.Classe, tmpColor, 2, ibPlayers);
                }

                if (playerOfSpell.MaskColors.Split('/')[2] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[2].Split('-')[0]), Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[2].Split('-')[1]), Convert.ToInt16(playerOfSpell.MaskColors.Split('/')[2].Split('-')[2]));
                    commune.SetPixelToClass(playerOfSpell.Classe, tmpColor, 3, ibPlayers);
                }

                // ajout du joueur dans la liste des joueurs
                commune.ApplyMaskColorToClasse(ibPlayers);
                commune.AllPlayers.Add(ibPlayers);

                // ajouter le joueurs dans la liste de combat
                PlayersInfosInBattle piibt = (PlayersInfosInBattle)playerOfSpell.Clone();
                // modification des données du clone comme le nom, vita, isInvoc = false ...
                piibt.Name = nameOfInvoc;
                piibt.isHuman = false;
                piibt.pm2 = piibt.pm;
                piibt.pc2 = piibt.pc;
                piibt.TotalPdv = lvlOfSpell;
                piibt.CurrentPdv = lvlOfSpell;
                piibt.ibPlayer = commune.AllPlayers.Find(f => (f.tag as PlayerInfo).Pseudo == nameOfInvoc);
                piibt.ibPlayer.name = nameOfInvoc;
                (piibt.ibPlayer.tag as PlayerInfo).Pseudo = nameOfInvoc;

                if (piibt.Team == "team1")
                {
                    Battle.Team1.Add(piibt);
                }
                else if (piibt.Team == "team2")
                {
                    Battle.Team2.Add(piibt);
                }
                else
                    MessageBox.Show("impossible de ne pas appartenir a une team tout de meme");

                // ajouter l'invocation a la liste des joueurs en ordre
                // determination de la position de notre joueur
                int index = Battle.AllPlayersByOrder.FindIndex(f => f.Name == playerOfSpell.Name);
                Battle.AllPlayersByOrder.Insert(index + 1, piibt);

                // effacement des cadront des joueurs dans la timeline
                Battle.refreshTimeLine();
                // libération et purgage de la liste des cmd
                commune.blockNetFlow = false;
                #endregion
            }
            else if(sortID == 4)
            {
                #region sort pounch de l'invoc 1 de naruto kagebunshin no jutsu
                // changement de position du joueur
                if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X == TargetPlayer.X)
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y > TargetPlayer.Y)
                    {
                        // changement de la direction du joueur vers le haut
                        playerOfSpell.Orientation = 3;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 3;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers le bas
                        playerOfSpell.Orientation = 0;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 0;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }
                else if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y == TargetPlayer.Y)
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X > TargetPlayer.X)
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 1;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 1;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers la droite
                        playerOfSpell.Orientation = 2;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 2;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }
                else
                {
                    if ((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X > TargetPlayer.X)
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 1;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 1;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                    else
                    {
                        // changement de la direction du joueur vers la gauche
                        playerOfSpell.Orientation = 2;
                        (playerOfSpell.ibPlayer.tag as PlayerInfo).Orientation = 2;
                        playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.Classe + "_AttackSprite" + sorts.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe + "_attackSprite" + sorts.sort(sortID).positionPlayer, playerOfSpell.Orientation));
                    }
                }

                // sort pounch
                int xDistance = (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X - TargetPlayer.X;
                int yDistance = (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y - TargetPlayer.Y;

                // diminution des pv du joueur roxé
                if (domString != "null")
                {
                    for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                    {
                        string data = domString.Split('#')[cnt];
                        string[] DomString = data.Split('|');
                        string typeRox = DomString[0].Split(':')[1];
                        int jet = Convert.ToInt16(DomString[1].Split(':')[1]);
                        bool cd = Convert.ToBoolean(DomString[2].Split(':')[1]);
                        string chakra = DomString[3].Split(':')[1];
                        int dom = Convert.ToInt32(DomString[4].Split(':')[1]);
                        List<string> deadList = DomString[5].Split(':').ToList();
                        deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                        if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                            deadList.Clear();
                        string roxed = DomString[6];
                        PlayersInfosInBattle playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.Name == roxed);

                        // affichage des données du damages sur la zone chat
                        Manager.manager.mainForm.BeginInvoke((Action)(() =>
                        {
                            commune.ChatMsgFormat("B", player + ":" + playerTargeted2.Name, domString);
                        }));

                        if (typeRox == "rox")
                        {
                            playerTargeted2.CurrentPdv -= dom;
                            // retrait de 5% des dom au total pdv du joueur roxé comme érosion
                            int ero = (dom * 5) / 100;
                            playerTargeted2.TotalPdv -= ero;
                            if (playerTargeted2.CurrentPdv <= 0)
                                playerTargeted2.CurrentPdv = 0;

                            // s'il sagit de notre joueur, actualiser la barre de ses points
                            HudeHandle.UpdateHealth();
                        }
                    }
                }

                // affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__draw_ID4_Spell_Thread";
                    long startTime = Environment.TickCount;                     // contien le temps pour le timer créer avec une la boucle whiel

                    // affichage des dom en haut de personnage
                    if (domString != "null")
                    {
                        new Thread(new ThreadStart(() =>
                        {
                            Thread.CurrentThread.Name = "__Dom_Acros_Player_Thread";
                            for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                            {
                                string[] data = domString.Split('#');
                                string tmp = data[cnt].ToString();

                                new Thread(new ThreadStart(() =>
                                {
                                    string roxed = tmp.Split('|')[6];
                                    PlayersInfosInBattle playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.Name == roxed);
                                    Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                    commune.showAnimDamageAbove(playerTargeted2.realPosition, playerTargeted2.ibPlayer.rectangle, playerTargeted2.ibPlayer.zindex, tmp);
                                })).Start();

                                string[] DomString = tmp.Split('|');
                                List<string> deadList = DomString[5].Split(':').ToList();
                                deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                                if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                    deadList.Clear();
                                if (deadList.Count == 1 && deadList[0] == "")
                                    deadList.Clear();

                                // check si un ou plusieurs joueurs sont mort
                                if (deadList.Count > 0)
                                {
                                    for (int i = 0; i < deadList.Count; i++)
                                    {
                                        Bmp ibPlayer = commune.AllPlayers.Find(f => (f.tag as PlayerInfo).Pseudo == deadList[i]);
                                        if (ibPlayer != null)
                                        {
                                            new Thread(new ThreadStart(() =>
                                            {
                                                Thread.CurrentThread.Name = "__Animate_Dead_Player_Thread";
                                                commune.animDeadPlayer(ibPlayer);
                                            })).Start();
                                        }
                                        else
                                        {
                                            // code impossible a atteindre puisqu'il dois y avoir un joueur mort,donc une non synchronisation entre le serveur et le client
                                            // il faut peux etre penser a demander au client une cmd de resynchronisation des states
                                            MessageBox.Show("code impossible a atteindre puisqu'il dois y avoir un joueur mort,donc une non synchronisation entre le serveur et le client\nil faut peux etre penser a demander au client une cmd de resynchronisation des states");
                                        }
                                    }
                                }

                                // supprimer notre joueur si la liste deadList est plus grande que 0
                                if (deadList.Count > 0)
                                {
                                    for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                    {
                                        Battle.DeadPlayers.Add((PlayersInfosInBattle)(Battle.AllPlayersByOrder.Find(f => f.Name == deadList[cnt2])).Clone());
                                        Battle.Team1.RemoveAll(f => f.Name == deadList[cnt2]);
                                        Battle.Team2.RemoveAll(f => f.Name == deadList[cnt2]);
                                        Battle.AllPlayersByOrder.RemoveAll(f => f.Name == deadList[cnt2]);
                                    }

                                    // on actualise la timeline
                                    Battle.refreshTimeLine();
                                }

                                Thread.Sleep(100);
                            }
                            commune.blockNetFlow = false;

                            Thread.Sleep(500);
                            // invocation du thread principale pour modifier l'image du joueur
                            Manager.manager.mainForm.BeginInvoke((Action)(() =>
                            {
                                Point pp = new Point((playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.X, (playerOfSpell.ibPlayer.tag as PlayerInfo).realPosition.Y);
                                playerOfSpell.ibPlayer.point.X = (pp.X * 30) + 15 - (playerOfSpell.ibPlayer.rectangle.Width / 2);
                                playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\" + playerOfSpell.Classe + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.Classe, playerOfSpell.Orientation * 4));
                            }));
                        })).Start();
                    }
                    else
                        commune.blockNetFlow = false;

                    // annimation sur le personnage qui recois les dom
                    if (domString != "null")
                    {
                        Anim __animPlayerDom = new Anim(20, true);
                        for (int cnt = 1; cnt <= 40; cnt++)
                            __animPlayerDom.AddCell(@"gfx\general\obj\3\ora\1\" + cnt + ".dat", cnt, 100, 100);
                        __animPlayerDom.Ini(Manager.typeGfx.obj, "__animPlayerDom");
                        __animPlayerDom.img.point.X = (TargetPlayer.X * 30) + 15 - (__animPlayerDom.img.rectangle.Width / 2);
                        __animPlayerDom.img.point.Y = (TargetPlayer.Y * 30) - (__animPlayerDom.img.rectangle.Height - 30);
                        __animPlayerDom.img.zindex = (TargetPlayer.Y * 100) + 99;
                        __animPlayerDom.PointOfParent = true;
                        __animPlayerDom.AutoResetAnim = false;
                        __animPlayerDom.HideAtLastFrame = true;
                        __animPlayerDom.DestroyAfterLastFrame = true;
                        __animPlayerDom.Start();
                        Manager.manager.GfxObjList.Add(__animPlayerDom);
                    }
                })).Start();
                commune.blockNetFlow = false;
                #endregion
            }
        }
        public static List<sort_tuile_info> isAllowedSpellArea(int pe, int cacDistance, bool Obstacle)
        {
            #region
            Point playerPosition = (commune.MyPlayerInfo.instance.ibPlayer.tag as PlayerInfo).realPosition;
            List<Point> allTuiles = new List<Point>();      // liste qui contiens tous les tuiles affecté par le sort y compris un obstacle ou pas
            List<sort_tuile_info> allTuilesInfo = new List<sort_tuile_info>();
            // allimentation de la liste allTuiles avec tous les cases atteignable par le sort

            // 1ere partie consiste a afficher toute la grille de tuiles
            for (int line = 0; line <= pe; line++)
            {
                // insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le bas
                if (line != 0 && playerPosition.Y + line < ScreenManager.TileHeight)
                {
                    Point p = new Point(playerPosition.X, playerPosition.Y + line);
                    allTuiles.Add(p);

                    if (!commune.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                    {
                        sort_tuile_info sti = new sort_tuile_info();
                        sti.TuilePoint = p;
                        if (Battle.AllPlayersByOrder.FindAll(f => (f.ibPlayer.tag as PlayerInfo).realPosition.X == p.X && (f.ibPlayer.tag as PlayerInfo).realPosition.Y == p.Y).Count > 0)
                        {
                            sti.isWalkable = true;
                            sti.isBlockingView = true;
                        }
                        else
                        {
                            sti.isWalkable = false;
                            sti.isBlockingView = true;
                        }
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        sort_tuile_info sti = new sort_tuile_info();
                        sti.TuilePoint = p;
                        sti.isWalkable = true;
                        sti.isBlockingView = false;
                        allTuilesInfo.Add(sti);
                    }
                }

                // insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le haut
                if (line != 0 && playerPosition.Y - line >= 0)
                {
                    Point p = new Point(playerPosition.X, playerPosition.Y - line);
                    allTuiles.Add(p);

                    if (!commune.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                    {
                        sort_tuile_info sti = new sort_tuile_info();
                        sti.TuilePoint = p;
                        if (Battle.AllPlayersByOrder.FindAll(f => (f.ibPlayer.tag as PlayerInfo).realPosition.X == p.X && (f.ibPlayer.tag as PlayerInfo).realPosition.Y == p.Y).Count > 0)
                        {
                            sti.isWalkable = true;
                            sti.isBlockingView = true;
                        }
                        else
                        {
                            sti.isWalkable = false;
                            sti.isBlockingView = true;
                        }
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        sort_tuile_info sti = new sort_tuile_info();
                        sti.TuilePoint = p;
                        sti.isWalkable = true;
                        sti.isBlockingView = false;
                        allTuilesInfo.Add(sti);
                    }
                }

                if (pe == line)
                    break;

                for (int side = 1; side <= pe; side++)
                {
                    // ajouter des tuiles coté en bas a droite
                    if (playerPosition.X + side < 26 && playerPosition.Y + line < 16)
                    {
                        Point p = new Point(playerPosition.X + side, playerPosition.Y + line);
                        allTuiles.Add(p);
                        
                        if (!commune.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            if (Battle.AllPlayersByOrder.FindAll(f => (f.ibPlayer.tag as PlayerInfo).realPosition.X == p.X && (f.ibPlayer.tag as PlayerInfo).realPosition.Y == p.Y).Count > 0)
                            {
                                sti.isWalkable = true;
                                sti.isBlockingView = true;
                            }
                            else
                            {
                                sti.isWalkable = false;
                                sti.isBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            sti.isWalkable = true;
                            sti.isBlockingView = false;
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en bas a gauche
                    if (playerPosition.X - side < 26 && playerPosition.Y + line < 16)
                    {
                        Point p = new Point(playerPosition.X - side, playerPosition.Y + line);
                        allTuiles.Add(p);
                        
                        if (!commune.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            if (Battle.AllPlayersByOrder.FindAll(f => (f.ibPlayer.tag as PlayerInfo).realPosition.X == p.X && (f.ibPlayer.tag as PlayerInfo).realPosition.Y == p.Y).Count > 0)
                            {
                                sti.isWalkable = true;
                                sti.isBlockingView = true;
                            }
                            else
                            {
                                sti.isWalkable = false;
                                sti.isBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            sti.isWalkable = true;
                            sti.isBlockingView = false;
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en haut a droite
                    if (playerPosition.X + side < 26 && playerPosition.Y - line < 16 && line > 0)
                    {
                        Point p = new Point(playerPosition.X + side, playerPosition.Y - line);
                        allTuiles.Add(p);
                        
                        if (!commune.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            if (Battle.AllPlayersByOrder.FindAll(f => (f.ibPlayer.tag as PlayerInfo).realPosition.X == p.X && (f.ibPlayer.tag as PlayerInfo).realPosition.Y == p.Y).Count > 0)
                            {
                                sti.isWalkable = true;
                                sti.isBlockingView = true;
                            }
                            else
                            {
                                sti.isWalkable = false;
                                sti.isBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            sti.isWalkable = true;
                            sti.isBlockingView = false;
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en haut a gauche
                    if (playerPosition.X - side < 26 && playerPosition.Y - line < 16 && line > 0)
                    {
                        Point p = new Point(playerPosition.X - side, playerPosition.Y - line);
                        allTuiles.Add(p);
                        
                        if (!commune.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            if (Battle.AllPlayersByOrder.FindAll(f => (f.ibPlayer.tag as PlayerInfo).realPosition.X == p.X && (f.ibPlayer.tag as PlayerInfo).realPosition.Y == p.Y).Count > 0)
                            {
                                sti.isWalkable = true;
                                sti.isBlockingView = true;
                            }
                            else
                            {
                                sti.isWalkable = false;
                                sti.isBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            sti.isWalkable = true;
                            sti.isBlockingView = false;
                            allTuilesInfo.Add(sti);
                        }
                    }
                    // determiner si le nombre de tuiles attein à cause de la notion qui vaux 1 tuile diagonale vaux 2 tuile, donc on déduit 1pe de chaque ligne
                    if (side + line == pe)
                        break;
                }
            }
            
            // mise en mode isWalkable = false des tuiles obstacle mais qui laisse la ligne de vue comme meme comme de l'eau ...
            List<sort_tuile_info> lsti = allTuilesInfo.FindAll(f => f.isWalkable == true && f.isBlockingView == false);
            for (int i = 0; i < lsti.Count; i++)
                if (!commune.CurMapFreeCellToWalk(new Point(lsti[i].TuilePoint.X * 30, lsti[i].TuilePoint.Y * 30)) && lsti[i].isWalkable == true && lsti[i].isBlockingView == false)
                    allTuilesInfo.Find(f => f.TuilePoint == lsti[i].TuilePoint && f.isWalkable == true && lsti[i].isBlockingView == false).isWalkable = false;

            if (Obstacle)
            {
                //////////////////// algo pour ligne de vue pour les obstacles
                // determination de la liste de tous les obstacles
                List<sort_tuile_info> block_view_tile = allTuilesInfo.FindAll(f => f.isBlockingView == true);

                // calcules préliminaires
                for (int i = 0; i < block_view_tile.Count; i++)
                {
                    // determination de la distance entre la position du joueur et l'obstacle
                    int xDistance = Math.Abs(playerPosition.X - block_view_tile[i].TuilePoint.X);
                    int yDistance = Math.Abs(playerPosition.Y - block_view_tile[i].TuilePoint.Y);

                    // determiner le niveau d'envergure de l'angle par tuile
                    float AngleA = 0;
                    float AngleB = 0;

                    // determination de ladirection de l'angle
                    bool rightDirection = false;
                    bool leftDirection = false;
                    bool downDirection = false;
                    bool upDirection = false;

                    if (playerPosition.X > block_view_tile[i].TuilePoint.X)
                        leftDirection = true;
                    else if (playerPosition.X < block_view_tile[i].TuilePoint.X)
                        rightDirection = true;

                    if (playerPosition.Y > block_view_tile[i].TuilePoint.Y)
                        upDirection = true;
                    else if (playerPosition.Y < block_view_tile[i].TuilePoint.Y)
                        downDirection = true;

                    // point de départ du polygone
                    Point pointAOfObstacle = new Point();
                    Point pointBOfObstacle = new Point();

                    // coordonnées de l'obstacle en cours de vérifications selons la position du joueur
                    if (upDirection && playerPosition.X != block_view_tile[i].TuilePoint.X && playerPosition.Y != block_view_tile[i].TuilePoint.Y)
                    {
                        // calcule des point d'intersection avec l'obstacle pour tracer la ligne
                        pointAOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X : block_view_tile[i].TuilePoint.X + 1;
                        pointAOfObstacle.Y = block_view_tile[i].TuilePoint.Y;

                        pointBOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X + 1 : block_view_tile[i].TuilePoint.X;
                        pointBOfObstacle.Y = block_view_tile[i].TuilePoint.Y + 1;
                    }
                    else if (downDirection && playerPosition.X != block_view_tile[i].TuilePoint.X && playerPosition.Y != block_view_tile[i].TuilePoint.Y)
                    {
                        // calcule des point d'intersection avec l'obstacle pour tracer la ligne
                        pointAOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X + 1 : block_view_tile[i].TuilePoint.X;
                        pointAOfObstacle.Y = block_view_tile[i].TuilePoint.Y;

                        pointBOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X : block_view_tile[i].TuilePoint.X + 1;
                        pointBOfObstacle.Y = block_view_tile[i].TuilePoint.Y + 1;
                    }
                    else if ((upDirection || downDirection) && !rightDirection && !leftDirection)
                    {
                        // le joueur est aligné horizontalement
                        pointAOfObstacle.X = block_view_tile[i].TuilePoint.X;
                        pointAOfObstacle.Y = (upDirection) ? block_view_tile[i].TuilePoint.Y + 1 : block_view_tile[i].TuilePoint.Y;

                        pointBOfObstacle.X = block_view_tile[i].TuilePoint.X + 1;
                        pointBOfObstacle.Y = (upDirection) ? block_view_tile[i].TuilePoint.Y + 1 : block_view_tile[i].TuilePoint.Y;
                    }
                    else if ((rightDirection || leftDirection) && !upDirection && !downDirection)
                    {
                        // le joueur est aligné horizontalement
                        pointAOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X : block_view_tile[i].TuilePoint.X + 1;
                        pointAOfObstacle.Y = block_view_tile[i].TuilePoint.Y;

                        pointBOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X : block_view_tile[i].TuilePoint.X + 1;
                        pointBOfObstacle.Y = block_view_tile[i].TuilePoint.Y + 1;
                    }

                    // calcule de l'envergure de l'angle par tuile passé
                    if (upDirection && (rightDirection || leftDirection))
                    {
                        AngleA = ((yDistance * 30) + 15) / (xDistance - 0.5F);
                        AngleB = (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F);
                    }
                    else if (downDirection && (rightDirection || leftDirection))
                    {
                        AngleA = (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F);
                        AngleB = ((yDistance * 30) + 15) / (xDistance - 0.5F);
                    }
                    else if (!downDirection && !upDirection && (rightDirection || leftDirection))
                    {
                        AngleA = ((yDistance * 30) + 15) / ((xDistance + 1) - 0.5F);   // direction vers la gauche
                        AngleB = ((yDistance * 30) + 15) / ((xDistance + 1) - 0.5F);   // direction vers la droite
                    }
                    else if (!rightDirection && !leftDirection && (upDirection || downDirection))
                    {
                        AngleA = ((yDistance * 30) + 15) / (yDistance - 0.5F);   // direction vers la gauche
                        AngleB = ((yDistance * 30) + 15) / (yDistance - 0.5F);   // direction vers la droite
                    }

                    AngleA = Math.Abs((upDirection) ? ((yDistance * 30) + 15) / (xDistance - 0.5F) : (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F));
                    AngleB = Math.Abs((upDirection) ? (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F) : ((yDistance * 30) + 15) / (xDistance - 0.5F));

                    List<Point> nextPointAL = new List<Point>();
                    List<Point> nextPointBL = new List<Point>();

                    for (int j = 1; j <= pe; j++)
                    {
                        if (upDirection && rightDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (upDirection && leftDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (downDirection && rightDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (downDirection && leftDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (upDirection && !rightDirection && !leftDirection && !downDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (downDirection && !rightDirection && !leftDirection && !upDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (rightDirection && !downDirection && !upDirection && !leftDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (leftDirection && !downDirection && !upDirection && !rightDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                    }

                    for (int j = 1; j <= pe; j++)
                    {
                        if (upDirection && rightDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30) + 30, (pointBOfObstacle.Y * 30) - (int)(AngleB * j)));
                        else if (upDirection && leftDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) - (j * 30) - 30, (pointBOfObstacle.Y * 30) - (int)(AngleB * j)));
                        else if (downDirection && rightDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleB * j)));
                        else if (downDirection && leftDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) - (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleB * j)));
                        else if (upDirection && !rightDirection && !leftDirection && !downDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (downDirection && !rightDirection && !leftDirection && !upDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (rightDirection && !upDirection && !downDirection && !leftDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (leftDirection && !upDirection && !downDirection && !rightDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
                    }

                    // tracage du cadre qui délimite les tuiles entre le joueur et le champ de vision
                    for (int a = 0; a < allTuilesInfo.Count; a++)
                    {
                        /*if (allTuilesInfo[a].TuilePoint.X == 17 && allTuilesInfo[a].TuilePoint.Y == 12)
                            MessageBox.Show("pop");
                        else if (allTuilesInfo[a].TuilePoint.X == 17 && allTuilesInfo[a].TuilePoint.Y == 11)
                            MessageBox.Show("opo");*/

                        if (allTuilesInfo[a].TuilePoint == block_view_tile[i].TuilePoint)
                            continue;
                        // check si la tuile en cours se trouve entre les angles A et B
                        if (upDirection && rightDirection && !leftDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X >= block_view_tile[i].TuilePoint.X && allTuilesInfo[a].TuilePoint.Y <= block_view_tile[i].TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle superieur à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en haut de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y < ((AngleA * (x + 1)) / 30))
                                    if ((y + 1) > Math.Floor(((AngleB * x) / 30)))
                                    {
                                        allTuilesInfo[a].isBlockingView = true;
                                        allTuilesInfo[a].isWalkable = false;
                                    }
                            }
                        }
                        else if (upDirection && !rightDirection && leftDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X <= block_view_tile[i].TuilePoint.X && allTuilesInfo[a].TuilePoint.Y <= block_view_tile[i].TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle superieur à gauche du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en haut de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y < ((AngleA * (x + 1)) / 30))
                                    if ((y + 1) > Math.Floor(((AngleB * x) / 30)))
                                    {
                                        allTuilesInfo[a].isBlockingView = true;
                                        allTuilesInfo[a].isWalkable = false;
                                    }
                            }
                        }
                        else if (downDirection && !rightDirection && leftDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X <= block_view_tile[i].TuilePoint.X && allTuilesInfo[a].TuilePoint.Y >= block_view_tile[i].TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle inférieure à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)(y) < ((AngleB * (x + 1)) / 30))
                                    if ((float)(y + 1) > Math.Floor(((AngleA * (x)) / (float)30)))
                                    {
                                        allTuilesInfo[a].isBlockingView = true;
                                        allTuilesInfo[a].isWalkable = false;
                                    }
                            }
                        }
                        else if (downDirection && rightDirection && !leftDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X >= block_view_tile[i].TuilePoint.X && allTuilesInfo[a].TuilePoint.Y >= block_view_tile[i].TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle inférieure à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)(y) < ((AngleB * (x + 1)) / 30))
                                    if ((float)(y + 1) > Math.Floor(((AngleA * (x)) / (float)30)))
                                    {
                                        allTuilesInfo[a].isBlockingView = true;
                                        allTuilesInfo[a].isWalkable = false;
                                    }
                            }
                        }
                        else if (rightDirection && !leftDirection && !upDirection && !downDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X >= block_view_tile[i].TuilePoint.X)
                            {
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y < ((AngleB * (x + 1)) / 30))
                                {
                                    allTuilesInfo[a].isBlockingView = true;
                                    allTuilesInfo[a].isWalkable = false;
                                }
                            }
                        }
                        else if (leftDirection && !rightDirection && !upDirection && !downDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X <= block_view_tile[i].TuilePoint.X)
                            {
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y < ((AngleB * (x + 1)) / 30))
                                {
                                    allTuilesInfo[a].isBlockingView = true;
                                    allTuilesInfo[a].isWalkable = false;
                                }
                            }
                        }
                        else if (upDirection && !downDirection && !leftDirection && !rightDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.Y <= block_view_tile[i].TuilePoint.Y)
                            {
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y >= ((AngleB * (x)) / 30))
                                {
                                    allTuilesInfo[a].isBlockingView = true;
                                    allTuilesInfo[a].isWalkable = false;
                                }
                            }
                        }
                        else if (downDirection && !upDirection && !leftDirection && !rightDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.Y >= block_view_tile[i].TuilePoint.Y)
                            {
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y >= ((AngleA * (x)) / 30))
                                {
                                    allTuilesInfo[a].isBlockingView = true;
                                    allTuilesInfo[a].isWalkable = false;
                                }
                            }
                        }
                    }
                }
            }
            // sustraction des cases qui ne sont pas accessible et qui sont a coté du joueur
            if (cacDistance == 2)
                allTuilesInfo.RemoveAll(f => (f.TuilePoint.X == playerPosition.X + 1 && f.TuilePoint.Y == playerPosition.Y) || (f.TuilePoint.X == playerPosition.X - 1 && f.TuilePoint.Y == playerPosition.Y) || (f.TuilePoint.X == playerPosition.X + 2 && f.TuilePoint.Y == playerPosition.Y) || (f.TuilePoint.X == playerPosition.X - 2 && f.TuilePoint.Y == playerPosition.Y) || (f.TuilePoint.X == playerPosition.X + 1 && f.TuilePoint.Y == playerPosition.Y + 1) || (f.TuilePoint.X == playerPosition.X - 1 && f.TuilePoint.Y == playerPosition.Y + 1) || (f.TuilePoint.X == playerPosition.X + 1 && f.TuilePoint.Y == playerPosition.Y - 1) || (f.TuilePoint.X == playerPosition.X - 1 && f.TuilePoint.Y == playerPosition.Y - 1) || (f.TuilePoint.X == playerPosition.X && f.TuilePoint.Y == playerPosition.Y + 1) || (f.TuilePoint.X == playerPosition.X && f.TuilePoint.Y == playerPosition.Y - 1) || (f.TuilePoint.X == playerPosition.X && f.TuilePoint.Y == playerPosition.Y + 2) || (f.TuilePoint.X == playerPosition.X && f.TuilePoint.Y == playerPosition.Y - 2));
            return allTuilesInfo;
            #endregion
        }
    }
    public class sort_Stats
    {
        public string title;
        public int classID;
        public string technique;
        public string rang;
        public string element;
        public int positionPlayer;          // id d'un type de position lorsqu'un joueur lance un sort pour faire samblant que le joueur a fait un truc, du genre tandre sa main ...
        public List<info_sort_by_level> isbl = new List<info_sort_by_level>();
        public class info_sort_by_level
        {
            // contiens les infos des sorts selons leurs niveau qui sont ensuite stoqué dans la liste isbl, donc sort a la position 0 = lvl1 ...
            public bool etenduModifiable;
            public int domMin, domMax, cd, etendu;
        }
    }
    public class sort_tuile_info
    {
        // classe utilisé lors de l'affichage de la porté d'un sort, pour vérifier si une tuile est un obstacle ou non et si oui si sa dois bloquer la ligne de vus pour faciliter le controle
        public Point TuilePoint;
        public bool isWalkable;
        public bool isBlockingView;
    }
}