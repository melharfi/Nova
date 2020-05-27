using MELHARFI;
using MELHARFI.Gfx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MMORPG;

namespace MMORPG.Net.Messages.Response.Battle
{
    class StartDuelBattleResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            // vérifier la chronologie des infos commandStrings
            #region commencement du combat en mode initialisation "préparation"
            if (MMORPG.Battle.state == Enums.battleState.state.idle)
            {
                // cmd[8] dois correspondre a BattleType selon la cmd, mais ici ca coincide avec BattleState ?!!
                string[] playersData = commandStrings[1].Split('|');
                string[] startPositions = commandStrings[2].Split('|');
                MMORPG.Battle.TimeLeftLabel.tag = commandStrings[3];       // cmd[4] = max timeleft
                MMORPG.Battle.BattleType = commandStrings[4];   // il faut changer BattleTypede string à Enum
                Enums.battleState.state battleState = (Enums.battleState.state)Enum.Parse(typeof(Enums.battleState.state), commandStrings[4]);
                List<string> sideA = commandStrings[5].Split('#').ToList();
                List<string> sideB = commandStrings[6].Split('#').ToList();
                // il reste 3 autres variable que le client ne prend pas en charge a voir pourquoi

                MMORPG.Battle.state = battleState;
                // il y a 2 affectation a ce variable MMORPG.Battle.state ici et dans la ligne 781, a verifier si cette valeur change lors du passage entre ces 2 lignes
                //StartDuelBattleResponseMessage
                //pseudo#classe#level#village#MaskColors#TotalPdv#CurrentPdv#rang|pseudo#classe#level#village                    //#MaskColors#TotalPdv#CurrentPdv#rang
                //ValidePos
                //waitTime
                //BattleType
                //sideA(p1t1#position1t1X,position1t1Y)
                //sideB(p1t2#position1t2X,position1t2Y)

                //pos du sideA comme 10/5#15/5
                //pos du sideB comme 10/5#15/5#13/3 séparés par #

                // effacement du menu tamisé
                // effacer le menu de demande de défie SI le menu vraiment affiché, au cas ou un défie est lancé avec un PNJ, il ya pas de menu tamisé vus qu'on attend pas que le joueur accepte ou pas
                if (CommonCode.annulerChallengeMeDlg != null)
                {
                    CommonCode.annulerChallengeMeDlg.visible = false;
                    Manager.manager.GfxTopList.Remove(CommonCode.annulerChallengeMeDlg);
                    // supression du case a coché
                    if (Manager.manager.mainForm.Controls.Find("ignorerCB", false).Count() != 0)
                    {
                        Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0].Visible = false;
                        Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0] = null;
                        Manager.manager.mainForm.Controls.Remove(Manager.manager.mainForm.Controls.Find("ignorerCB", false)[0]);
                    }
                    CommonCode.annulerChallengeMeDlg.Child.Clear();
                    CommonCode.annulerChallengeMeDlg = null;
                }
                else if (CommonCode.annulerChallengeHimDlg != null)
                {
                    // effacement du rec parent de la liste graphics
                    CommonCode.annulerChallengeHimDlg.visible = false;
                    Manager.manager.GfxTopList.Remove(CommonCode.annulerChallengeHimDlg);
                    CommonCode.annulerChallengeHimDlg.Child.Clear();
                    CommonCode.annulerChallengeHimDlg = null;
                }

                // supression des joueurs present dans le map
                List<Bmp> allPlayersExceptChallenged = CommonCode.AllActorsInMap.FindAll(f => (f.tag as Actor).pseudo != CommonCode.ChallengeTo && (f.tag as Actor).pseudo != CommonCode.MyPlayerInfo.instance.pseudo);
                for (int cnt = allPlayersExceptChallenged.Count; cnt > 0; cnt--)
                {
                    allPlayersExceptChallenged[cnt - 1].visible = false;
                    CommonCode.AllActorsInMap.Remove(allPlayersExceptChallenged[cnt - 1]);
                    allPlayersExceptChallenged.RemoveAt(cnt - 1);
                }
                CommonCode.annulerChallengeHimDlg = null;
                CommonCode.annulerChallengeMeDlg = null;
                CommonCode.ChallengeTo = "";

                // initialisation du combat
                // affichage des joueurs
                
                //pseudo#classe#level#village#MaskColors#TotalPdv#CurrentPdv#rang#initiative#doton#katon
                //#futon#raiton#siuton#usingDoton#usingKaton#usingFuton#usingRaiton#usingSuiton#equipedDoton#equipedKaton
                //#equipedFuton#equipedRaiton#suitonEquiped#pc#pm#pe#cd#invoc#resiDoton#resiKaton
                //#resiFuton#resiRaiton#resiSuiton#esquivePC#esquivePM#esquivePE#esquiveCD#retraitCD#retraitPM#retraitPE
                //#retraitCD#evasion#blocage
                // | séparateur entre les teams, / séparateur entre les membres d'une meme team

                //////////////  Team A ////////////////////////////
                for (int cnt = 0; cnt < playersData[0].Split(':').Count(); cnt++)
                {
                    string[] states = playersData[0].Split(':');

                    /////////// dispatch states
                    string actorName = states[0];
                    Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
                    int level = int.Parse(states[2]);
                    Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[3]);
                    string maskColorsString = states[4];
                    string[] maskColors = states[4].Split('/');
                    int maxHealth = int.Parse(states[5]);
                    int currentHealth = int.Parse(states[6]);
                    Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), states[7]);
                    int initiative = int.Parse(states[8]);
                    int doton = int.Parse(states[9]);
                    int katon = int.Parse(states[10]);
                    int futon = int.Parse(states[11]);
                    int raiton = int.Parse(states[12]);
                    int suiton = int.Parse(states[13]);
                    int usingDoton = int.Parse(states[14]);
                    int usingKaton = int.Parse(states[15]);
                    int usingFuton = int.Parse(states[16]);
                    int usingRaiton = int.Parse(states[17]);
                    int usingSuiton = int.Parse(states[18]);
                    int equipedDoton = int.Parse(states[19]);
                    int equipedKaton = int.Parse(states[20]);
                    int equipedFuton = int.Parse(states[21]);
                    int equipedRaiton = int.Parse(states[22]);
                    int equipedSuiton = int.Parse(states[23]);
                    int originalPc = int.Parse(states[24]);
                    int originalPm = int.Parse(states[25]);
                    int pe = int.Parse(states[26]);
                    int cd = int.Parse(states[27]);
                    int summons = int.Parse(states[28]);
                    int resiDotonPercent = int.Parse(states[29]);
                    int resiKatonPercent = int.Parse(states[30]);
                    int resiFutonPercent = int.Parse(states[31]);
                    int resiRaitonPercent = int.Parse(states[32]);
                    int resiSuitonPercent = int.Parse(states[33]);
                    int dodgePc = int.Parse(states[34]);
                    int dodgePm = int.Parse(states[35]);
                    int dodgePe = int.Parse(states[36]);
                    int dodgeCd = int.Parse(states[37]);
                    int removePc = int.Parse(states[38]);
                    int removePm = int.Parse(states[39]);
                    int removePe = int.Parse(states[40]);
                    int removeCd = int.Parse(states[41]);
                    int escape = int.Parse(states[42]);
                    int blocage = int.Parse(states[43]);
                    Enums.Species.Name species = (Enums.Species.Name)Enum.Parse(typeof(Enums.Species.Name), states[44]);
                    int orientation = int.Parse(states[45]);
                    //////////////////////////////////////////////////////////

                    Actor pit1 = new Actor();
                    pit1.teamSide = Enums.Team.Side.A;
                    pit1.pseudo = actorName;
                    pit1.className = className;
                    pit1.level = level;
                    pit1.hiddenVillage = hiddenVillage;
                    pit1.maskColorString = maskColorsString;
                    pit1.maxHealth = maxHealth;
                    pit1.currentHealth = currentHealth;
                    pit1.officialRang = officialRang;
                    pit1.initiative = initiative;
                    pit1.doton = doton;
                    pit1.katon = futon;
                    pit1.futon = futon;
                    pit1.raiton = raiton;
                    pit1.suiton = suiton;
                    pit1.usingDoton = usingDoton;
                    pit1.usingFuton = usingFuton;
                    pit1.usingKaton = usingKaton;
                    pit1.usingRaiton = usingRaiton;
                    pit1.usingSuiton = usingSuiton;
                    pit1.equipedDoton = equipedDoton;
                    pit1.equipedKaton = equipedKaton;
                    pit1.equipedFuton = equipedFuton;
                    pit1.equipedRaiton = equipedRaiton;
                    pit1.equipedSuiton = equipedSuiton;
                    pit1.originalPc = originalPc;
                    pit1.originalPm = originalPm;
                    pit1.pe = pe;
                    pit1.cd = cd;
                    pit1.summons = summons;
                    pit1.resiDotonPercent = resiDotonPercent;
                    pit1.resiKatonPercent = resiKatonPercent;
                    pit1.resiFutonPercent = resiFutonPercent;
                    pit1.resiRaitonPercent = resiRaitonPercent;
                    pit1.resiSuitonPercent = resiSuitonPercent;
                    pit1.dodgePc = dodgePc;
                    pit1.dodgePm = dodgePm;
                    pit1.dodgePe = dodgePe;
                    pit1.dodgeCd = dodgeCd;
                    pit1.removePc = removePc;
                    pit1.removePm = removePm;
                    pit1.removePe = removePe;
                    pit1.removeCd = removeCd;
                    pit1.escape = escape;
                    pit1.blocage = blocage;
                    pit1.species = species;
                    pit1.directionLook = orientation;

                    // si le joueur n'est pas humain comme PNJ, alors il faut créer à nouveau l'image ou dupliquer celle déja présante sur la map, ou meme utiliser celle sur la map
                    if (pit1.species != Enums.Species.Name.Human)
                    {
                        // le joueur étant déja supprimé de la liste common1.AllPlayers lors du nétoyage de l'ecran pour le combat, il faut recréer le pointeur, cad l'ajouter a la liste
                        // j'ai intentionnelement laissé le joueur se supprimer au lieu de faire une exeption pour garder un code uniforme et qui dépand pas des situations, donc vaux mieux recréer le pointeur
                        Bmp __pnj = new Bmp(@"gfx\general\classes\" + pit1.className + ".dat", Point.Empty, "__pnj", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet(pit1.className.ToString(), 0));
                        __pnj.MouseMove += CommonCode.CursorHand_MouseMove;
                        __pnj.MouseOver += CommonCode.ibPlayers_MouseOver;
                        __pnj.MouseOut += CommonCode.ibPlayers_MouseOut;
                        __pnj.tag = pit1;
                        CommonCode.AllActorsInMap.Add(__pnj);
                        CommonCode.VerticalSyncZindex(__pnj);
                        Manager.manager.GfxObjList.Add(__pnj);
                    }
                    else if (pit1.species == Enums.Species.Name.Summon)
                    {
                        // le joueur étant déja supprimé de la liste common1.AllPlayers lors du nétoyage de l'ecran pour le combat, il faut recréer le pointeur, cad l'ajouter a la liste
                        // j'ai intentionnelement laissé le joueur se supprimer au lieu de faire une exeption pour garder un code uniforme et qui dépand pas des situations, donc vaux mieux recréer le pointeur
                        Bmp __invok = new Bmp(@"gfx\general\classes\" + pit1.className + ".dat", Point.Empty, "__pnj", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet(pit1.className.ToString(), 0));
                        __invok.MouseMove += CommonCode.CursorHand_MouseMove;
                        __invok.MouseOver += CommonCode.ibPlayers_MouseOver;
                        __invok.MouseOut += CommonCode.ibPlayers_MouseOut;
                        __invok.tag = pit1;
                        CommonCode.AllActorsInMap.Add(__invok);
                        CommonCode.VerticalSyncZindex(__invok);
                        Manager.manager.GfxObjList.Add(__invok);
                    }

                    pit1.ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == pit1.pseudo);
                    (pit1.ibPlayer.tag as Actor).directionLook = pit1.directionLook;
                    CommonCode.AdjustPositionAndDirection(pit1.ibPlayer, pit1.ibPlayer.point);     // remet le joueur bien aligné sur la grille et lui applique un mask de couleur

                    MMORPG.Battle.SideA.Add(pit1);
                }

                ///////////////////// Team B ///////////////////////////////////
                for (int cnt = 0; cnt < playersData[1].Split(':').Count(); cnt++)
                {
                    string[] states = playersData[1].Split(':');

                    /////////// dispatch states
                    string actorName = states[0];
                    Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
                    int level = int.Parse(states[2]);
                    Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[3]);
                    string maskColorsString = states[4];
                    string[] maskColors = states[4].Split('/');
                    int maxHealth = int.Parse(states[5]);
                    int currentHealth = int.Parse(states[6]);
                    Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), states[7]);
                    int initiative = int.Parse(states[8]);
                    int doton = int.Parse(states[9]);
                    int katon = int.Parse(states[10]);
                    int futon = int.Parse(states[11]);
                    int raiton = int.Parse(states[12]);
                    int suiton = int.Parse(states[13]);
                    int usingDoton = int.Parse(states[14]);
                    int usingKaton = int.Parse(states[15]);
                    int usingFuton = int.Parse(states[16]);
                    int usingRaiton = int.Parse(states[17]);
                    int usingSuiton = int.Parse(states[18]);
                    int equipedDoton = int.Parse(states[19]);
                    int equipedKaton = int.Parse(states[20]);
                    int equipedFuton = int.Parse(states[21]);
                    int equipedRaiton = int.Parse(states[22]);
                    int equipedSuiton = int.Parse(states[23]);
                    int originalPc = int.Parse(states[24]);
                    int originalPm = int.Parse(states[25]);
                    int pe = int.Parse(states[26]);
                    int cd = int.Parse(states[27]);
                    int summons = int.Parse(states[28]);
                    int resiDotonPercent = int.Parse(states[29]);
                    int resiKatonPercent = int.Parse(states[30]);
                    int resiFutonPercent = int.Parse(states[31]);
                    int resiRaitonPercent = int.Parse(states[32]);
                    int resiSuitonPercent = int.Parse(states[33]);
                    int dodgePc = int.Parse(states[34]);
                    int dodgePm = int.Parse(states[35]);
                    int dodgePe = int.Parse(states[36]);
                    int dodgeCd = int.Parse(states[37]);
                    int removePc = int.Parse(states[38]);
                    int removePm = int.Parse(states[39]);
                    int removePe = int.Parse(states[40]);
                    int removeCd = int.Parse(states[41]);
                    int escape = int.Parse(states[42]);
                    int blocage = int.Parse(states[43]);
                    Enums.Species.Name species = (Enums.Species.Name)Enum.Parse(typeof(Enums.Species.Name), states[44]);
                    int orientation = int.Parse(states[45]);
                    //////////////////////////////////////////////////////

                    Actor pit2 = new Actor();
                    pit2.teamSide = Enums.Team.Side.A;
                    pit2.pseudo = actorName;
                    pit2.className = className;
                    pit2.level = level;
                    pit2.hiddenVillage = hiddenVillage;
                    pit2.maskColorString = maskColorsString;
                    pit2.maxHealth = maxHealth;
                    pit2.currentHealth = currentHealth;
                    pit2.officialRang = officialRang;
                    pit2.initiative = initiative;
                    pit2.doton = doton;
                    pit2.katon = futon;
                    pit2.futon = futon;
                    pit2.raiton = raiton;
                    pit2.suiton = suiton;
                    pit2.usingDoton = usingDoton;
                    pit2.usingFuton = usingFuton;
                    pit2.usingKaton = usingKaton;
                    pit2.usingRaiton = usingRaiton;
                    pit2.usingSuiton = usingSuiton;
                    pit2.equipedDoton = equipedDoton;
                    pit2.equipedKaton = equipedKaton;
                    pit2.equipedFuton = equipedFuton;
                    pit2.equipedRaiton = equipedRaiton;
                    pit2.equipedSuiton = equipedSuiton;
                    pit2.originalPc = originalPc;
                    pit2.originalPm = originalPm;
                    pit2.pe = pe;
                    pit2.cd = cd;
                    pit2.summons = summons;
                    pit2.resiDotonPercent = resiDotonPercent;
                    pit2.resiKatonPercent = resiKatonPercent;
                    pit2.resiFutonPercent = resiFutonPercent;
                    pit2.resiRaitonPercent = resiRaitonPercent;
                    pit2.resiSuitonPercent = resiSuitonPercent;
                    pit2.dodgePc = dodgePc;
                    pit2.dodgePm = dodgePm;
                    pit2.dodgePe = dodgePe;
                    pit2.dodgeCd = dodgeCd;
                    pit2.removePc = removePc;
                    pit2.removePm = removePm;
                    pit2.removePe = removePe;
                    pit2.removeCd = removeCd;
                    pit2.escape = escape;
                    pit2.blocage = blocage;
                    pit2.species = species;
                    pit2.directionLook = orientation;

                    // si le joueur n'est humain comme PNJ, alors il faut créer à nouveau l'image ou dupliquer celle déja présante sur la map, ou meme utiliser celle sur la map
                    if (pit2.species == Enums.Species.Name.Pnj)
                    {
                        // le joueur étant déja supprimé de la liste common1.AllPlayers lors du nétoyage de l'ecran pour le combat, il faut recréer le pointeur, cad l'ajouter a la liste
                        // j'ai intentionnelement laissé le joueur se supprimer au lieu de faire une exeption pour garder un code uniforme et qui dépand pas des situations, donc vaux mieux recréer le pointeur
                        Bmp __pnj = new Bmp(@"gfx\general\classes\" + pit2.className + ".dat", Point.Empty, "__pnj", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet(pit2.className.ToString(), 0));
                        __pnj.MouseMove += CommonCode.CursorHand_MouseMove;
                        __pnj.MouseOver += CommonCode.ibPlayers_MouseOver;
                        __pnj.MouseOut += CommonCode.ibPlayers_MouseOut;
                        __pnj.tag = pit2;
                        CommonCode.AllActorsInMap.Add(__pnj);
                        CommonCode.VerticalSyncZindex(__pnj);
                        Manager.manager.GfxObjList.Add(__pnj);
                    }
                    else if (pit2.species == Enums.Species.Name.Summon)
                    {
                        // le joueur étant déja supprimé de la liste common1.AllPlayers lors du nétoyage de l'ecran pour le combat, il faut recréer le pointeur, cad l'ajouter a la liste
                        // j'ai intentionnelement laissé le joueur se supprimer au lieu de faire une exeption pour garder un code uniforme et qui dépand pas des situations, donc vaux mieux recréer le pointeur
                        Bmp __invok = new Bmp(@"gfx\general\classes\" + pit2.className + ".dat", Point.Empty, "__pnj", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet(pit2.className.ToString(), 0));
                        __invok.MouseMove += CommonCode.CursorHand_MouseMove;
                        __invok.MouseOver += CommonCode.ibPlayers_MouseOver;
                        __invok.MouseOut += CommonCode.ibPlayers_MouseOut;
                        __invok.tag = pit2;
                        CommonCode.AllActorsInMap.Add(__invok);
                        CommonCode.VerticalSyncZindex(__invok);
                        Manager.manager.GfxObjList.Add(__invok);
                    }

                    pit2.ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == pit2.pseudo);
                    (pit2.ibPlayer.tag as Actor).directionLook = pit2.directionLook;
                    CommonCode.AdjustPositionAndDirection(pit2.ibPlayer, pit2.ibPlayer.point);

                    MMORPG.Battle.SideB.Add(pit2);
                }
                ///////////////////////////////////////////////////////////////////

                // affichage des positions valide pour les 2 teams
                string[] dataT1 = startPositions[0].Split('#');
                for (int cnt1 = 0; cnt1 < dataT1.Count(); cnt1++)
                {
                    string[] data1 = dataT1[cnt1].Split('/');
                    MMORPG.Battle.ValidePosT1.Add(new Point(Convert.ToInt16(data1[0]) * 30, Convert.ToInt16(data1[1]) * 30));
                }

                string[] dataT2 = startPositions[1].Split('#');
                for (int cnt1 = 0; cnt1 < dataT2.Count(); cnt1++)
                {
                    string[] data1 = dataT2[cnt1].Split('/');
                    MMORPG.Battle.ValidePosT2.Add(new Point(Convert.ToInt16(data1[0]) * 30, Convert.ToInt16(data1[1]) * 30));
                }

                

                // dessin des positions valides dans le map
                MMORPG.Battle.DrawBattleValidePos();

                // image du chronometre timeout
                Bmp Chrono_TimeOut = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ScreenManager.WindowWidth - 100, 50), "Chrono_TimeOut", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 23));
                Manager.manager.GfxTopList.Add(Chrono_TimeOut);

                //timer de temps restant
                MMORPG.Battle.TimeLeftLabel.zindex = Chrono_TimeOut.zindex + 1;
                Manager.manager.GfxTopList.Add(MMORPG.Battle.TimeLeftLabel);

                //lancement du thread du la barre de progression du timeleft
                Thread timeLeftT = new Thread(new ThreadStart(CommonCode.timeLeftForBattle));

                
                timeLeftT.Start();

                

                for (int cnt = 0; cnt < sideA.Count; cnt++)
                {
                    Bmp bmp = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == sideA[cnt].Split('|')[0]);
                    Point p = new Point(Convert.ToInt16(sideA[cnt].Split('|')[1].Split('/')[0]), Convert.ToInt16(sideA[cnt].Split('|')[1].Split('/')[1]));
                    bmp.point = new Point((p.X * 30) + 15 - (bmp.rectangle.Width / 2), (p.Y * 30) + 15 - bmp.rectangle.Height);
                    CommonCode.ApplyMaskColorToClasse(bmp);
                    (bmp.tag as Actor).realPosition = p;
                    Actor piib = MMORPG.Battle.SideA.Find(f => f.pseudo == sideA[cnt].Split('|')[0]);
                    piib.realPosition = p;
                    CommonCode.VerticalSyncZindex(bmp);
                }

                for (int cnt = 0; cnt < sideB.Count; cnt++)
                {
                    Bmp bmp = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == sideB[cnt].Split('|')[0]);
                    Point p = new Point(Convert.ToInt16(sideB[cnt].Split('|')[1].Split('/')[0]), Convert.ToInt16(sideB[cnt].Split('|')[1].Split('/')[1]));
                    bmp.point = new Point((p.X * 30) + 15 - (bmp.rectangle.Width / 2), (p.Y * 30) + 15 - bmp.rectangle.Height);
                    CommonCode.ApplyMaskColorToClasse(bmp);
                    (bmp.tag as Actor).realPosition = p;
                    Actor piib = MMORPG.Battle.SideB.Find(f => f.pseudo == sideB[cnt].Split('|')[0]);
                    piib.realPosition = p;
                    CommonCode.VerticalSyncZindex(bmp);
                }
                MMORPG.Battle.state = battleState;

                // il faut ajouter les 2 bouton, pour quiter le combat et pour valider sa position
                HudHandle._passer_La_Main_btn = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(HudHandle.ChatTextBox.Location.X + HudHandle.ChatTextBox.Width + 50, 578), "_passer_La_Main_btn", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 25));
                HudHandle._passer_La_Main_btn.point = new Point(HudHandle.HealthBarRec1.point.X - HudHandle._passer_La_Main_btn.rectangle.Size.Width - 10, ScreenManager.WindowHeight - HudHandle._passer_La_Main_btn.rectangle.Height - 3);
                HudHandle._passer_La_Main_btn.EscapeGfxWhileMouseMove = true;
                HudHandle._passer_La_Main_btn.EscapeGfxWhileMouseClic = true;
                HudHandle._passer_La_Main_btn.MouseMove += CommonCode.CursorHand_MouseMove;
                HudHandle._passer_La_Main_btn.MouseOut += CommonCode.CursorDefault_MouseOut;
                HudHandle._passer_La_Main_btn.MouseClic += _passer_La_Main_btn_MouseClic;
                Manager.manager.GfxTopList.Add(HudHandle._passer_La_Main_btn);

                // label pour passer la main
                Txt _passer_La_Main_Lbl = new Txt(CommonCode.TranslateText(110), new Point(0, 2), "_passer_La_Main_Lbl", Manager.TypeGfx.Top, true, new Font("verdana", 9), Brushes.Yellow);
                _passer_La_Main_Lbl.point.X = 5 + (HudHandle._passer_La_Main_btn.rectangle.Width - TextRenderer.MeasureText(_passer_La_Main_Lbl.Text, _passer_La_Main_Lbl.font).Width) / 2;
                HudHandle._passer_La_Main_btn.Child.Add(_passer_La_Main_Lbl);

                // bouton pour quiter le combat
                HudHandle._quiter_le_combat = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(HudHandle.ChatTextBox.Location.X + HudHandle.ChatTextBox.Width + 30, 580), "_quiter_le_combat", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 26));
                HudHandle._quiter_le_combat.point = new Point(HudHandle._passer_La_Main_btn.point.X - HudHandle._quiter_le_combat.rectangle.Width + 5, ScreenManager.WindowHeight - HudHandle._quiter_le_combat.rectangle.Height - 3);
                HudHandle._quiter_le_combat.MouseMove += CommonCode.CursorHand_MouseMove;
                HudHandle._quiter_le_combat.MouseOut += CommonCode.CursorDefault_MouseOut;
                HudHandle._quiter_le_combat.MouseClic += _quiter_le_combat_MouseClic;
                Manager.manager.GfxTopList.Add(HudHandle._quiter_le_combat);
            }
            #endregion
        }

        static void _passer_La_Main_btn_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // cmd qui termine un combat
            Network.SendMessage("cmd•finishTurn", true);
        }

        static void _quiter_le_combat_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            DialogResult dr = MessageBox.Show(CommonCode.TranslateText(147), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.Yes)
            {
                // cmd qui annule un combat
                Network.SendMessage("cmd•leaveBattle", true);
            }
        }
    }
}
