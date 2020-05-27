using MELHARFI.Gfx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MELHARFI;
using MMORPG.Net.Messages.Request;

namespace MMORPG
{
    public static class network_Command_Dispatcher
    {
        // elle contiens tous les données recus par le serveur et qui sont appliqués a tous les map
        public static void cmd(string stat)
        {
            string[] args = stat.Split('•');
            if(args[1] == "spellTileGranted")
            {
                
            }

            if (Battle.state == Enums.battleState.state.started && CommonCode.blockNetFlow)
            {
                bool blockCmd = true;
                if (args.Length == 5 && args[1] == "ChatMessage")
                    blockCmd = false;
                else if (args.Length == 3 && args[1] == "CheckAndUpdateStates")
                    blockCmd = false;
                else if (args.Length == 2 && args[1] == "spellTileNotAllowed")
                    blockCmd = false;

                // bloquer les cmd quand un joueur lance un sort pour ne pas mélanger les animations
                if (blockCmd)
                    CommonCode.cmdBlockedBySpellInPRogress.Add(stat);
                return;
            }

            if (CommonCode.debug)
                CommonCode.ChatMsgFormat("S", "null", stat);

            if (Manager.manager.mainForm.IsDisposed)
                return;

            if (args.Length == 5 && args[1] == "ChatMessage")
            {
                #region
                // cmd•ChatMessage•canal•user•message
                CommonCode.ChatMsgFormat(args[2], args[3], args[4]);
                #endregion
            }
            else if (args.Length == 3 && args[1] == "askingToChallenge")
            {
                #region
                if (!CommonCode.isBusy())
                {
                    // reception d'une demande de duel
                    // affichage d'une fenetre d'acceptation ou refus du duel
                    CommonCode.DrawAskingToChallengeMeMenu(args[2]);
                }
                #endregion
            }
            else if (args.Length == 3 && args[1] == "waitingToChallenge")
            {
                CommonCode.DrawAskingToChallengeHimMenu(args[2]);
            }
            else if (args.Length == 3 && args[1] == "CancelChallengeRespond")
            {
                #region
                // le jouer qui va etre défié annule cette demande
                if (CommonCode.ChallengeTo == args[2])
                {
                    CommonCode.annulerChallengeHimDlg.visible = false;
                    Manager.manager.GfxTopList.Remove(CommonCode.annulerChallengeHimDlg);
                    CommonCode.annulerChallengeHimDlg = null;
                    CommonCode.ChallengeTo = "";
                }
                #endregion
            }
            else if (args.Length == 2 && args[1] == "playerBusyToChallengeYou")
            {
                #region
                // le joueur est occupé
                // effacement du demande de défie
                MessageBox.Show(CommonCode.TranslateText(105));
                CommonCode.ChallengeTo = "";
                #endregion
            }

            else if (args.Length == 4 && args[1] == "battleIniNewPos")
            {
                #region changement de position d'un joueur lors du temps d'attente du combat
                // verification si state=initialisation
                if (Battle.state == Enums.battleState.state.initialisation)
                {
                    // check si l'utilisateur se trouve parmie les joueurs sideA & sideB
                    if (Battle.SideA.Exists(e => e.pseudo == args[2]))
                    {
                        Point p = new Point(int.Parse(args[3].Split('/')[0]) * 30, int.Parse(args[3].Split('/')[1]) * 30);
                        Bmp ibPlayer = Battle.SideA.Find(e => e.pseudo == args[2]).ibPlayer;
                        ibPlayer.point = new Point(p.X + 15 - (ibPlayer.rectangle.Width / 2), p.Y + 15 - ibPlayer.rectangle.Height);
                        (ibPlayer.tag as Actor).realPosition = new Point(p.X / 30, p.Y / 30);
                        Actor piib = Battle.SideA.Find(f => f.pseudo == args[2]);
                        piib.realPosition = new Point(p.X / 30, p.Y / 30);
                        CommonCode.VerticalSyncZindex(ibPlayer);
                    }
                    else if (Battle.SideB.Exists(e => e.pseudo == args[2]))
                    {
                        Point p = new Point(int.Parse(args[3].Split('/')[0]) * 30, int.Parse(args[3].Split('/')[1]) * 30);
                        Bmp ibPlayer = Battle.SideB.Find(e => e.pseudo == args[2]).ibPlayer;
                        ibPlayer.point = new Point(p.X + 15 - (ibPlayer.rectangle.Width / 2), p.Y + 15 - ibPlayer.rectangle.Height);
                        (ibPlayer.tag as Actor).realPosition = new Point(p.X / 30, p.Y / 30);
                        Actor piib = Battle.SideB.Find(f => f.pseudo == args[2]);
                        piib.realPosition = new Point(p.X / 30, p.Y / 30);
                        CommonCode.VerticalSyncZindex(ibPlayer);
                    }
                }
                #endregion
            }
            else if (args.Length == 4 && args[1] == "CloseBattle")
            {
                #region
                HudHandle.UpdateHealth();
                Battle.CloseBattle(stat);
                CommonCode.blockNetFlow = false;       // liberer le flux au cas ou il été bloqué
                CommonCode.ChatMsgFormat("S", "null", "blockNetFlow7 = false");
                //Network.SendMessage("cmd•CheckAndUpdateStates", true);
                SyncFeaturesRequestMessage syncFeaturesRequestMessage = new SyncFeaturesRequestMessage();
                syncFeaturesRequestMessage.Serialize();
                syncFeaturesRequestMessage.Send();
                #endregion
            }
            else if (args.Length == 3 && args[1] == "change map")
            {
                CommonCode.ChangeMap(args[2]);
            }
            else if (args.Length == 6 && args[1] == "BattleStarted")
            {
                #region
                // cmd•BattleStats•data (data = info des 2 jours des 2 teams)• + player qui a la main__TimeLeftToPlayRec2
                // data = data += _battle.sideA[cnt2].Pseudo + "#" + BattleStatsPDB.reader["classe"].ToString() + "#" + BattleStatsPDB.reader["village"].ToString() + "#" + BattleStatsPDB.reader["MaskColors"].ToString() + "#" + BattleStatsPDB.reader["level"].ToString() + "#" + BattleStatsPDB.reader["rang"].ToString() + "#" + BattleStatsPDB.reader["CurrentPdv"].ToString() + "#" + BattleStatsPDB.reader["TotalPdv"].ToString() + "#" + BattleStatsPDB.reader["doton"].ToString() + "#" + BattleStatsPDB.reader["katon"].ToString() + "#" + BattleStatsPDB.reader["futon"].ToString() + "#" + BattleStatsPDB.reader["raiton"].ToString() + "#" + BattleStatsPDB.reader["suiton"].ToString() + "#" + BattleStatsPDB.reader["usingDoton"].ToString() + "#" + BattleStatsPDB.reader["usingKaton"].ToString() + "#" + BattleStatsPDB.reader["usingFuton"].ToString() + "#" + BattleStatsPDB.reader["usingRaiton"].ToString() + "#" + BattleStatsPDB.reader["usingSuiton"].ToString() + "#" + BattleStatsPDB.reader["equipedDoton"].ToString() + "#" + BattleStatsPDB.reader["equipedKaton"].ToString() + "#" + BattleStatsPDB.reader["equipedFuton"].ToString() + "#" + BattleStatsPDB.reader["equipedRaiton"].ToString() + "#" + BattleStatsPDB.reader["suitonEquiped"].ToString() + "#" + BattleStatsPDB.reader["pc"].ToString() + "#" + BattleStatsPDB.reader["pm"].ToString() + "#" + BattleStatsPDB.reader["pe"].ToString() + "#" + BattleStatsPDB.reader["cd"].ToString() + "#" + BattleStatsPDB.reader["invoc"].ToString() + "#" + BattleStatsPDB.reader["initiative"].ToString() + "#" + BattleStatsPDB.reader["esquivePC"].ToString() + "#" + BattleStatsPDB.reader["esquivePM"].ToString() + "#" + BattleStatsPDB.reader["esquivePE"].ToString() + "#" + BattleStatsPDB.reader["esquiveCD"].ToString() + "#" + BattleStatsPDB.reader["retraitPC"].ToString() + "#" + BattleStatsPDB.reader["retraitPM"].ToString() + "#" + BattleStatsPDB.reader["retraitPE"].ToString() + "#" + BattleStatsPDB.reader["retraitCD"].ToString() + "#" + BattleStatsPDB.reader["evasion"].ToString() + "#" + BattleStatsPDB.reader["blocage"].ToString() + "#" + BattleStatsPDB.reader["puissance"].ToString() + "#" + BattleStatsPDB.reader["puissanceEquiped"].ToString() + "#" + BattleStatsPDB.reader["sorts"] + "|"  + sideB.pseudo ....
                CommonCode.RefreshPlayersDataInBattle(args);
                //verification si les 2 learders ont la meme initiative, si oui, le variable cmd[4] contiens le nom de l'utilisateur séléctionné aléatoirement
                Battle.PlayerTurn = args[4];
                CommonCode.ReorderAvatarPlayers(args[4]);

                #region
                // le serveur annonce le commancement du combat et fournie le temps d'attente pour le déplacement
                // selection du joueur qui dois jouer en 1er
                Battle.TimeToPlayInBattle = Convert.ToInt32(args[5]);
                Battle.TimeLeftToPlay = Convert.ToInt32(args[5]);

                // affichage du TimeLeftToPlayRec1
                HudHandle.TimeLeftToPlayRec1 = new Rec(Brushes.Black, new Point(HudHandle.HealthBarRec1.point.X, HudHandle.HealthBarRec1.point.Y - 38), new Size(38, 38), "__TimeLeftToPlayRec1", Manager.TypeGfx.Top, true);
                Rec TimeLeftToPlayRec2 = new Rec(Brushes.White, new Point(1, 1), new Size(36, 36), "__TimeLeftToPlayRec2", Manager.TypeGfx.Top, true);
                TimeLeftToPlayRec2.zindex = 1;
                HudHandle.TimeLeftToPlayRec1.Child.Add(TimeLeftToPlayRec2);
                Rec TimeLeftToPlayBar = new Rec(Brushes.Silver, new Point(2, 2), new Size(34, 34), "__TimeLeftToPlayBar", Manager.TypeGfx.Top, true);
                TimeLeftToPlayBar.zindex = 2;
                HudHandle.TimeLeftToPlayRec1.Child.Add(TimeLeftToPlayBar);
                Manager.manager.GfxTopList.Add(HudHandle.TimeLeftToPlayRec1);

                // affichage du text du temps restant pour jouer "en seconds"
                HudHandle.TimeLeftToPlayTxt1 = new Txt(Battle.TimeToPlayInBattle.ToString(), new Point(0, HudHandle.TimeLeftToPlayRec1.point.Y + 10), "__TimeLeftToPlayTxt1", Manager.TypeGfx.Top, false, new Font("verdana", 9, FontStyle.Bold), Brushes.Black);
                Manager.manager.GfxTopList.Add(HudHandle.TimeLeftToPlayTxt1);

                // effacement des images de positions et du compteur, des fois les cases reste pour des raisons que j'ignore
                Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__validePosT1Rec");
                Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__validePosT2Rec");
                Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "Chrono_TimeOut");
                Battle.TimeLeftLabel.visible = false;

                // éffacement des images de vérrouillage de la position du personnage
                Manager.manager.GfxObjList.RemoveAll(f => f.Name().Length > 12 && f.Name().Substring(0, 12) == "__posLocked_");

                // un thread qui vérifie tout le temps si les animations liés au lancement du sort sont terminé + l'achévement du joueur. .. pour executer les cmd en instance
                new Thread(new ThreadStart(() =>
                {
                    Thread.CurrentThread.Name = "__Purge_CMD_Thread";
                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        if (Battle.state == Enums.battleState.state.idle)
                        {
                            if (CommonCode.cmdBlockedBySpellInPRogress.Count == 0)
                                break;
                            else
                            {
                                while (CommonCode.cmdBlockedBySpellInPRogress.Count > 0 && !Manager.manager.mainForm.IsDisposed)
                                {
                                    // reinvocation du thread principale pour reprendre la reléve
                                    // verification que la fenetre na pas quité
                                    Manager.manager.mainForm.BeginInvoke((Action)(() =>
                                    {
                                        network_Command_Dispatcher.cmd(CommonCode.cmdBlockedBySpellInPRogress[0]);
                                        CommonCode.cmdBlockedBySpellInPRogress.RemoveAt(0);
                                    }));
                                    Thread.Sleep(500);
                                }
                            }
                        }
                        else if (!CommonCode.blockNetFlow)
                        {
                            // executer les cmd en instance tant qu'aucun sort n'est lancé ou encours
                            while (CommonCode.cmdBlockedBySpellInPRogress.Count > 0 && !Manager.manager.mainForm.IsDisposed)
                            {
                                if (!CommonCode.blockNetFlow)
                                {
                                    Manager.manager.mainForm.BeginInvoke((Action)(() =>
                                    {
                                        if (CommonCode.cmdBlockedBySpellInPRogress.Count > 0)
                                        {
                                            network_Command_Dispatcher.cmd(CommonCode.cmdBlockedBySpellInPRogress[0]);
                                            CommonCode.cmdBlockedBySpellInPRogress.RemoveAt(0);
                                        }
                                        else
                                            return;
                                    }));

                                    Thread.Sleep(500);
                                }
                                else
                                    break;
                            }
                        }

                        Thread.Sleep(500);
                    }
                })).Start();

                // lancement du thread qui calcule le temps d'attente du jouer
                if (Battle.TimeLeftToPlayT.Enabled == true)
                    Battle.TimeLeftToPlayT.Stop();
                Battle.TimeLeftToPlayT.Start();
                Battle.state = Enums.battleState.state.started;

                // thread qui affiche un petit indicateur au dessus du personnage en focus
                Thread focusPlayer = new Thread(new ThreadStart(CommonCode.FocusPlayerByArrow));
                focusPlayer.Start();

                CommonCode.turnPast(Battle.PlayerTurn, false);
                #endregion
                #endregion
            }
            else if (args.Length == 3 && args[1] == "BattleTurnPast")
            {
                #region
                CommonCode.turnPast(args[2], true);
                #endregion
            }
            else if (args.Length == 2 && args[1] == "obstacleFound")
            {
                #region
                // impossible de jouer le sort
                CommonCode.ChatMsgFormat("S", "", CommonCode.TranslateText(109));
                // annulation du waypoint en attente

                Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).animatedAction = Enums.AnimatedActions.Name.idle;
                Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).wayPoint.Clear();
                #endregion
            }
            else if (args.Length == 10 && args[1] == "spellTileGranted")
            {
                #region sorts
                // diminution des pc
                //PlayerInfo CurPlayerInfo = Battle.AllPlayersByOrder.Find(f => f.Pseudo == commune.MyPlayerInfo.instance.Pseudo);

                // cmd des sorts lancées
                // cmd•spellTileGranted•roxor•sortID•posX•posY•spellColor•spellLvl•Dom(jet:x|cd:true ou false|chakra:futon...|dom:total|roxed des dom et # comme séparateur sur plusieurs instance de rox simultanés|deadList:joueurMort1:joueurMort2...|closedBattle:true ou false    séparé par # s'il sagit de plusieurs rox en meme temps•PcUsed
                spells.animSpellAction(args[2], new Point(Convert.ToInt16(args[4]), Convert.ToInt16(args[5])), Convert.ToInt16(args[3]), args[6], Convert.ToInt16(args[7]), args[8], args[9]);
                #endregion
            }
            else if (args.Length == 2 && args[1] == "spellTileNotAllowed")
            {
                #region
                // impossible de jouer le sort
                CommonCode.ChatMsgFormat("S", "", CommonCode.TranslateText(116));
                // annulation du waypoint en attente
                Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).animatedAction = Enums.AnimatedActions.Name.idle;
                Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).wayPoint.Clear();
                #endregion
            }
            else if (args.Length == 2 && args[1] == "spellPointNotEnoughPe")
            {
                #region
                // impossible de jouer le sort
                CommonCode.ChatMsgFormat("S", "", CommonCode.TranslateText(117));
                // annulation du waypoint en attente
                Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).animatedAction = Enums.AnimatedActions.Name.idle;
                Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).wayPoint.Clear();
                #endregion
            }
            else if (args.Length == 2 && args[1] == "spellNotEnoughPc")
            {
                #region
                // impossible de jouer le sort
                CommonCode.ChatMsgFormat("S", "", CommonCode.TranslateText(122));
                #endregion
            }
            else if (args.Length == 4 && args[1] == "CheckAndUpdateStates")
            {
                #region
                // cmd pour update des states apres la cloture du combat et les states de mon joueur seulement
                // "cmd•CheckAndUpdateStates•"Pseudo#ClasseName#Spirit#SpiritLvl#Pvp#village#MaskColors#Orientation#Level#map#rang
                //#CurrentPdv#totalPdv#xp#TotalXp#doton#katon#futon#raiton#suiton#chakralvl2
                //#chakralvl3#chakralvl4#chakralvl5#chakralvl6#usingDoton#usingKaton#usingFuton#usingRaiton#usingSuiton#equipedDoton
                //#equipedKaton#equipedFuton#equipedRaiton#suitonEquiped#pc#pm#pe#cd#invoc#Initiative
                //#job1#job2#specialite1#specialite2#TotalPoid#CurrentPoid#Ryo#resiDoton#resiKaton#resiFuton
                //#resiRaiton#resiSuiton#esquivePC#esquivePM#esquivePE#esquiveCD#retraitPC#retraitPM#retraitPE#retraitCD
                //#evasion#blocage#_sorts#resiDotonFix#resiKatonFix#resiFutonFix#resiRaitonFix#resiSuitonFix#resiFix#domDotonFix
                //#domKatonFix#domFutonFix#domRaitonFix#domSuitonFix#domFix#puissance#puissanceEquiped"•quete•null•pi.spellPointLeft
                string[] states = args[2].Split('#');

                string actorName = states[0];
                Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
                Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit.Name), states[2]);
                int spiritLevel = int.Parse(states[3]);
                bool pvpEnabled = bool.Parse(states[4]);
                Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[5]);
                string maskColorsString = states[6];
                string[] maskColors = states[6].Split('/');
                int orientation = int.Parse(states[7]);
                int level = int.Parse(states[8]);
                string map = states[9];
                Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), states[10]);
                int currentHealth = int.Parse(states[11]);
                int maxHealth = int.Parse(states[12]);
                int currentXp = int.Parse(states[13]);
                int maxXp = int.Parse(states[14]);
                int doton = int.Parse(states[15]);
                int katon = int.Parse(states[16]);
                int futon = int.Parse(states[17]);
                int raiton = int.Parse(states[18]);
                int suiton = int.Parse(states[19]);
                int chakra1Level = int.Parse(states[20]);
                int chakra2Level = int.Parse(states[21]);
                int chakra3Level = int.Parse(states[22]);
                int chakra4Level = int.Parse(states[23]);
                int chakra5Level = int.Parse(states[24]);
                int usingDoton = int.Parse(states[25]);
                int usingKaton = int.Parse(states[26]);
                int usingFuton = int.Parse(states[27]);
                int usingRaiton = int.Parse(states[28]);
                int usingSuiton = int.Parse(states[29]);
                int equipedDoton = int.Parse(states[30]);
                int equipedKaton = int.Parse(states[31]);
                int equipedFuton = int.Parse(states[32]);
                int equipedRaiton = int.Parse(states[33]);
                int equipedSuiton = int.Parse(states[34]);
                int originalPc = int.Parse(states[35]);
                int originalPm = int.Parse(states[36]);
                int pe = int.Parse(states[37]);
                int cd = int.Parse(states[38]);
                int summons = int.Parse(states[39]);
                int initiative = int.Parse(states[40]);
                string job1 = states[41];
                string job2 = states[42];
                string specialty1 = states[43];
                string specialty2 = states[44];
                int maxWeight = int.Parse(states[45]);
                int currentWeight = int.Parse(states[46]);
                int ryo = int.Parse(states[47]);
                int resiDotonPercent = int.Parse(states[48]);
                int resiKatonPercent = int.Parse(states[49]);
                int resiFutonPercent = int.Parse(states[50]);
                int resiRaitonPercent = int.Parse(states[51]);
                int resiSuitonPercent = int.Parse(states[52]);
                int dodgePc = int.Parse(states[53]);
                int dodgePm = int.Parse(states[54]);
                int dodgePe = int.Parse(states[55]);
                int dodgeCd = int.Parse(states[56]);
                int removePc = int.Parse(states[57]);
                int removePm = int.Parse(states[58]);
                int removePe = int.Parse(states[59]);
                int removeCd = int.Parse(states[60]);
                int escape = int.Parse(states[61]);
                int blocage = int.Parse(states[62]);
                string spells = states[63];
                int resiDotonFix = int.Parse(states[64]);
                int resiKatonFix = int.Parse(states[65]);
                int resiFutonFix = int.Parse(states[66]);
                int resiRaitonFix = int.Parse(states[67]);
                int resiSuitonFix = int.Parse(states[68]);
                int resiFix = int.Parse(states[69]);
                int domDotonFix = int.Parse(states[70]);
                int domKatonFix = int.Parse(states[71]);
                int domFutonFix = int.Parse(states[72]);
                int domRaitonFix = int.Parse(states[73]);
                int domSuitonFix = int.Parse(states[74]);
                int domFix = int.Parse(states[75]);
                int power = int.Parse(states[76]);
                int equipedPower = int.Parse(states[77]);

                string questString = args[3];

                int spellPointLeft = int.Parse(args[4]);

                // creation d'une instance Bmp vide just pour contenir les infos dans le tag
                CommonCode.MyPlayerInfo.instance.ibPlayer = new Bmp();
                CommonCode.MyPlayerInfo.instance.ibPlayer.tag = new Actor();
                Actor pi = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
                pi.pseudo = actorName;
                pi.className = className;
                pi.spirit = spirit;
                pi.spiritLevel = spiritLevel;
                pi.pvpEnabled = pvpEnabled;
                pi.hiddenVillage = hiddenVillage;
                pi.maskColorString = maskColorsString;
                pi.directionLook = orientation;
                pi.level = level;
                pi.map = map;
                pi.officialRang = officialRang;
                pi.currentHealth = currentHealth;
                pi.maxHealth = maxHealth;
                pi.currentXp = currentXp;
                pi.maxXp = maxXp;
                pi.doton = doton;
                pi.katon = katon;
                pi.futon = futon;
                pi.raiton = raiton;
                pi.suiton = suiton;

                // association des données des chakralvl2,3,4,5
                CommonCode.chakra1Level = chakra1Level;
                CommonCode.chakra2Level = chakra2Level;
                CommonCode.chakra3Level = chakra3Level;
                CommonCode.chakra4Level = chakra4Level;
                CommonCode.chakra5Level = chakra5Level;

                pi.usingDoton = usingDoton;
                pi.usingKaton = usingKaton;
                pi.usingFuton = usingFuton;
                pi.usingRaiton = usingRaiton;
                pi.usingSuiton = usingSuiton;
                pi.equipedDoton = equipedDoton;
                pi.equipedKaton = equipedKaton;
                pi.equipedFuton = equipedFuton;
                pi.equipedRaiton = equipedRaiton;
                pi.equipedSuiton = equipedSuiton;
                pi.originalPc = originalPc;
                pi.originalPm = originalPm;
                pi.pe = pe;
                pi.cd = cd;
                pi.summons = summons;
                pi.initiative = initiative;
                pi.job1 = job1;
                pi.job2 = job2;
                pi.specialty1 = specialty1;
                pi.specialty2 = specialty2;
                pi.maxWeight = maxWeight;
                pi.currentWeight = currentWeight;
                pi.ryo = ryo;
                pi.resiDotonPercent = resiDotonPercent;
                pi.resiKatonPercent = resiKatonPercent;
                pi.resiFutonPercent = resiFutonPercent;
                pi.resiRaitonPercent = resiRaitonPercent;
                pi.resiSuitonPercent = resiSuitonPercent;
                pi.dodgePc = dodgePc;
                pi.dodgePm = dodgePm;
                pi.dodgePe = dodgePe;
                pi.dodgeCd = dodgeCd;
                pi.removePc = removePc;
                pi.removePm = removePm;
                pi.removePe = removePe;
                pi.removeCd = removeCd;
                pi.escape = escape;
                pi.blocage = blocage;

                if (spells != "")
                {
                    for (int cnt = 0; cnt < spells.Split('|').Length; cnt++)
                    {
                        string tmp_data = spells.Split('|')[cnt];
                        Actor.SpellsInformations _info_sorts = new Actor.SpellsInformations();
                        _info_sorts.sortID = Convert.ToInt32(tmp_data.Split(':')[0].ToString());
                        _info_sorts.emplacement = Convert.ToInt32(tmp_data.Split(':')[1].ToString());
                        _info_sorts.level = Convert.ToInt32(tmp_data.Split(':')[2]);
                        _info_sorts.colorSort = Convert.ToInt32(tmp_data.Split(':')[3]);
                        pi.spells.Add(_info_sorts);
                    }
                }

                pi.resiDotonFix = resiDotonFix;
                pi.resiKatonFix = resiKatonFix;
                pi.resiFutonFix = resiFutonFix;
                pi.resiRaitonFix = resiRaitonFix;
                pi.resiSuitonFix = resiSuitonFix;
                pi.resiFix = resiFix;
                pi.domDotonFix = domDotonFix;
                pi.domKatonFix = domKatonFix;
                pi.domFutonFix = domFutonFix;
                pi.domRaitonFix = domRaitonFix;
                pi.domSuitonFix = domSuitonFix;
                pi.domFix = domFix;
                pi.power = power;
                pi.equipedPower = equipedPower;

                // convertir la list de quete en string
                if (questString != "")
                    for (int cnt = 0; cnt < questString.Split('/').Length; cnt++)
                    {
                        string _quest = questString.Split('/')[cnt];
                        Actor.QuestInformations qi = new Actor.QuestInformations();
                        qi.nom_quete = _quest.Split(':')[0];
                        qi.totalSteps = Convert.ToInt16(_quest.Split(':')[1]);
                        qi.currentStep = Convert.ToInt16(_quest.Split(':')[2]);
                        qi.submited = Convert.ToBoolean(_quest.Split(':')[3]);
                        pi.Quests.Add(qi);
                    }

                CommonCode.CurMap = pi.map;

                // level
                MenuStats.StatsLevel.Text = CommonCode.TranslateText(50) + " " + pi.level;

                // affichage du rang général
                MenuStats.Rang.Text = CommonCode.officialRangToCurrentLangTranslation(pi.officialRang);

                // affichage du level Pvp
                MenuStats.LevelPvp.Text = pi.spiritLevel.ToString();

                // affichage du grade Pvp
                if (spirit != Enums.Spirit.Name.neutral)
                {
                    MenuStats.GradePvp = new Bmp(@"gfx\general\obj\2\" + pi.spirit + @"\" + MenuStats.LevelPvp.Text + ".dat", new Point(276 + (15 - Convert.ToInt16(MenuStats.LevelPvp.Text)), 2), new Size(40 + Convert.ToInt16(MenuStats.LevelPvp.Text), 20 + Convert.ToInt16(MenuStats.LevelPvp.Text)), "PlayerStats." + pi.spirit, Manager.TypeGfx.Top, true, 1);
                    MenuStats.StatsImg.Child.Add(MenuStats.GradePvp);
                }

                // update des pdv
                HudHandle.UpdateHealth();

                MenuStats.Flag = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", new Point(240, 8), "__Flag", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("pays_" + pi.hiddenVillage + "_thumbs", 0));
                MenuStats.StatsImg.Child.Add(MenuStats.Flag);
                MenuStats.LFlag.Text = hiddenVillage.ToString();
                MenuStats.Fusion1.Text = CommonCode.TranslateText(75);
                MenuStats.Fusion2.Text = CommonCode.TranslateText(75);
                MenuStats.NiveauGaugeTxt.Text = CommonCode.TranslateText(50) + " " + pi.level;

                // NiveauGaugeRecPercent, barre de progression du niveau
                // calcule du pourcentage du niveau en progression
                int CurrentProgressLevel = pi.currentXp;
                int TotalProgressLevel = pi.maxXp;
                int PercentProgressLevel = 0;

                if (TotalProgressLevel != 0)
                    PercentProgressLevel = (CurrentProgressLevel * 100) / TotalProgressLevel;
                else
                    PercentProgressLevel = 100;

                MenuStats.NiveauGaugeRecPercent.size.Width = (258 * PercentProgressLevel) / 100;

                // affichage du label progression lvl
                MenuStats.NiveauGaugeTxtCurrent.Text = CurrentProgressLevel + "/" + TotalProgressLevel + " (" + PercentProgressLevel + "%)";
                MenuStats.NiveauGaugeTxtCurrent.point = new Point(MenuStats.NiveauGaugeRec2.point.X + (MenuStats.NiveauGaugeRec2.size.Width / 2) - (TextRenderer.MeasureText(MenuStats.NiveauGaugeTxtCurrent.Text, MenuStats.NiveauGaugeTxtCurrent.font).Width / 2), MenuStats.NiveauGaugeRec2.point.Y);

                // raffrechissement du text pour des mesures de changement de langue
                MenuStats.AffiniteElementaireTxt.Text = CommonCode.TranslateText(76);
                MenuStats.terreStats.Text = "(" + CommonCode.TranslateText(77) + ")";
                MenuStats.FeuStats.Text = "(" + CommonCode.TranslateText(78) + ")";
                MenuStats.VentStats.Text = "(" + CommonCode.TranslateText(79) + ")";
                MenuStats.FoudreStats.Text = "(" + CommonCode.TranslateText(80) + ")";
                MenuStats.EauStats.Text = "(" + CommonCode.TranslateText(81) + ")";

                MenuStats.TerrePuissance.Text = "(" + pi.doton + "+" + pi.equipedDoton + ")=" + (pi.doton + pi.equipedDoton);
                MenuStats.FeuPuissance.Text = "(" + pi.katon + "+" + pi.equipedKaton + ")=" + (pi.katon + pi.equipedKaton);
                MenuStats.VentPuissance.Text = "(" + pi.futon + "+" + pi.equipedFuton + ")=" + (pi.futon + pi.equipedFuton);
                MenuStats.FoudrePuissance.Text = "(" + pi.raiton + "+" + pi.equipedRaiton + ")=" + (pi.raiton + pi.equipedRaiton);
                MenuStats.EauPuissance.Text = "(" + pi.suiton + "+" + pi.equipedSuiton + ")=" + (pi.suiton + pi.equipedSuiton);

                MenuStats.Lvl1RegleTxt.Text = CommonCode.TranslateText(82);
                MenuStats.Lvl2RegleTxt.Text = CommonCode.TranslateText(83);
                MenuStats.Lvl3RegleTxt.Text = CommonCode.TranslateText(84);
                MenuStats.Lvl4RegleTxt.Text = CommonCode.TranslateText(85);
                MenuStats.Lvl5RegleTxt.Text = CommonCode.TranslateText(86);
                MenuStats.Lvl6RegleTxt.Text = CommonCode.TranslateText(87);

                // affichage de la gauge lvl chakra selon les points
                MenuStats.Lvl2ReglePts.Text = CommonCode.chakra1Level.ToString();
                MenuStats.Lvl3ReglePts.Text = CommonCode.chakra2Level.ToString();
                MenuStats.Lvl4ReglePts.Text = CommonCode.chakra3Level.ToString();
                MenuStats.Lvl5ReglePts.Text = CommonCode.chakra4Level.ToString();
                MenuStats.Lvl6ReglePts.Text = CommonCode.chakra5Level.ToString();

                // modification du lvl de l'utilisation de l'element
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.doton, pi.usingDoton);
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.katon, pi.usingKaton);
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.futon, pi.usingFuton);
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.raiton, pi.usingRaiton);
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.suiton, pi.equipedSuiton);

                MenuStats.DotonLvl.Text = pi.usingDoton.ToString();
                MenuStats.KatonLvl.Text = pi.usingKaton.ToString();
                MenuStats.FutonLvl.Text = pi.usingFuton.ToString();
                MenuStats.RaitonLvl.Text = pi.usingRaiton.ToString();
                MenuStats.SuitonLvl.Text = pi.usingSuiton.ToString();

                // bar de vie selon les pdv 11 current, 12 total
                int TotalPdv = pi.maxHealth;
                int CurrentPdv = pi.currentHealth;
                int X = 0;
                if (TotalPdv != 0)
                    X = (CurrentPdv * 100) / TotalPdv;
                MenuStats.VieBar.size.Width = (236 * X) / 100;

                // point de vie dans Menustats
                MenuStats.VieLabel.Text = CommonCode.TranslateText(88);
                MenuStats.ViePts.Text = CurrentPdv.ToString() + " / " + TotalPdv + " (" + X + "%)";
                //MenuStats.PCLabel.Text = common1.TranslateText(89);
                MenuStats.PC.Text = pi.originalPc.ToString();
                //MenuStats.PMLabel.Text = common1.TranslateText(90);
                MenuStats.PM.Text = pi.originalPm.ToString();
                //MenuStats.PELabel.Text = common1.TranslateText(91);
                MenuStats.PE.Text = pi.pe.ToString();
                //MenuStats.CDLabel.Text = common1.TranslateText(92);
                MenuStats.CD.Text = pi.cd.ToString();
                //MenuStats.InvocLabel.Text = common1.TranslateText(93);
                MenuStats.Invoc.Text = pi.summons.ToString();
                //MenuStats.InitiativeLabel.Text = common1.TranslateText(94);
                MenuStats.Initiative.Text = pi.initiative.ToString();
                MenuStats.Job1Label.Text = CommonCode.TranslateText(95) + " 1";
                MenuStats.Specialite1Label.Text = CommonCode.TranslateText(96) + " 1";
                MenuStats.Job2Labe1.Text = CommonCode.TranslateText(95) + " 2";
                MenuStats.Specialite2Label.Text = CommonCode.TranslateText(96) + " 2";
                //////// playerData[41] = job1
                //////// playerdata[42] = job2
                //////// playerdata[43] = specialite1
                //////// playerdata[44] = specialite2
                MenuStats.PoidLabel.Text = CommonCode.TranslateText(97);
                int TotalPoid = pi.maxWeight;
                int CurrentPoid = pi.currentWeight;
                int PercentPoid = (CurrentPoid * 100) / TotalPoid;
                MenuStats.PoidRec.size.Width = (116 * PercentPoid) / 100;
                MenuStats.Poid.Text = CurrentPoid + " / " + TotalPoid + " (" + PercentPoid + "%)";
                MenuStats.Poid.point.X = MenuStats.PoidRec.point.X + 58 - (TextRenderer.MeasureText(MenuStats.Poid.Text, MenuStats.Poid.font).Width / 2);
                MenuStats.Ryo.Text = CommonCode.MoneyThousendSeparation(pi.ryo.ToString());
                MenuStats.resiDotonTxt.Text = pi.resiDotonPercent.ToString() + "%";
                MenuStats.resiKatonTxt.Text = pi.resiKatonPercent.ToString() + "%";
                MenuStats.resiFutonTxt.Text = pi.resiFutonPercent.ToString() + "%";
                MenuStats.resiRaitonTxt.Text = pi.resiRaitonPercent.ToString() + "%";
                MenuStats.resiSuitonTxt.Text = pi.resiSuitonPercent.ToString() + "%";
                MenuStats.__esquivePC_Txt.Text = pi.dodgePc.ToString();
                MenuStats.__esquivePM_Txt.Text = pi.dodgePm.ToString();
                MenuStats.__retraitPC_Txt.Text = pi.removePc.ToString();
                MenuStats.__retraitPM_Txt.Text = pi.removePm.ToString();

                pi.spellPointLeft = spellPointLeft;
                #endregion
            }
            else if (args.Length == 6 && args[1] == "recheckPosition")    // not used
            {
                #region
                /*/cmd•recheckPosition•pseudo•posX,posY•orientatuin•pm rstant si on combat
                string[] p = cmd[3].Split(',');
                if (Battle.state == "Started")
                {
                    PlayerInfo piib = Battle.AllPlayersByOrder.Find(f => f.Pseudo == cmd[2]);
                    if(piib != null)
                    {
                        Point playerPosition = new Point(Convert.ToInt16(p[0]), Convert.ToInt16(p[1]));
                        if (piib.realPosition.X != playerPosition.X || piib.realPosition.Y != playerPosition.Y)
                        {
                            MessageBox.Show("la position du client n'est pas la meme avec le serveur, en combat\n" + cmd[3]);
                        }
                    }
                }
                else
                {
                    if (commune.AllPlayers.Exists(i => (i.tag as PlayerInfo).Pseudo == cmd[2]))
                    {
                        Bmp ibPlayer = commune.AllPlayers.Find(i => (i.tag as PlayerInfo).Pseudo == cmd[2]);
                        Point playerPosition = new Point(Convert.ToInt16(p[0]), Convert.ToInt16(p[1]));
                        if ((ibPlayer.tag as PlayerInfo).realPosition.X != playerPosition.X || (ibPlayer.tag as PlayerInfo).realPosition.Y != playerPosition.Y)
                        {
                            MessageBox.Show("la position du client n'est pas la meme avec le serveur, hors combat");
                        }
                    }
                }*/
                #endregion
            }
            else if (args.Length == 2 && args[1] == "spellIntervalNotReached")
            {
                #region sort ne peux pas être lancé dans le tour actuelle
                CommonCode.ChatMsgFormat("B", "null", CommonCode.TranslateText(114));
                #endregion
            }
            else if (args.Length == 2 && args[1] == "spellRelanceParTourReached")
            {
                #region
                CommonCode.ChatMsgFormat("B", "null", CommonCode.TranslateText(150));
                #endregion
            }
            else if (args.Length == 2 && args[1] == "spellRelanceParJoueurReached")
            {
                #region
                CommonCode.ChatMsgFormat("B", "null", CommonCode.TranslateText(151));
                #endregion
            }
            else if (args.Length == 4 && args[1] == "BattlePosLocked")
            {
                #region joueur qui block sa position en étant en combat et en mode initialisation
                //cmd•BattlePosLocked•player•true or false pour lancer le combat
                Bmp __posLocked = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__posLocked_" + args[2], Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 61));
                List<Actor> Lpiib = new List<Actor>();
                Lpiib.AddRange(Battle.SideA);
                Lpiib.AddRange(Battle.SideB);
                Actor targetedPlayer = Lpiib.Find(f => f.pseudo == args[2]);
                if (targetedPlayer == null)
                    return;
                Point p = new Point(targetedPlayer.ibPlayer.point.X + (targetedPlayer.ibPlayer.rectangle.Width / 2) - (__posLocked.rectangle.Width / 2), targetedPlayer.ibPlayer.point.Y - __posLocked.rectangle.Height - 5);
                __posLocked.zindex = targetedPlayer.ibPlayer.zindex;
                __posLocked.point = p;
                Manager.manager.GfxObjList.Add(__posLocked);

                if (args[3] == "True")
                {
                    // le combat commance suite au validation des pos
                    Battle.timeleft = 0;
                }
                #endregion
            }
            else if (args.Length == 3 && args[1] == "BattlePosUnlocked")
            {
                #region debloquage de la position en mode initialisation
                Manager.manager.GfxObjList.RemoveAll(f => f.Name().Length > 12 && f.Name() == "__posLocked_" + args[2]);
                #endregion
            }
            else if (args.Length == 2 && args[1] == "lockedPosition")
            {
                #region position verrouillé, quand un joueur a verrouillé sa pos et apres demande de la changer
                CommonCode.ChatMsgFormat("B", "null", CommonCode.TranslateText(152));
                #endregion
            }
            else if (args.Length == 2 && args[1] == "spellNotEnoughInvoc")
            {
                #region pas assez de points d'invocation
                CommonCode.ChatMsgFormat("S", "", CommonCode.TranslateText(154));
                #endregion
            }
            else if (args.Length == 2 && args[1] == "spellNeedEtatSennin")
            {
                #region pas assez de points d'invocation
                CommonCode.ChatMsgFormat("S", "", CommonCode.TranslateText(155));
                #endregion
            }
            else if (args.Length > 2 && args[1] == "submitedQuest")
            {
                #region on cherche la quete chez le joueur
                // "cmd•submitedQuest•FirstFight•submited"
                Actor.QuestInformations qi = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).Quests.Find(f => f.nom_quete == args[2]);
                if (qi != null)
                {
                    qi.submited = true;
                    qi.currentStep = qi.totalSteps;
                    if (args[2] == "FirstFight")
                    {
                        // 1ere quete contre iruka
                        CommonCode.ChatMsgFormat("I", "System", CommonCode.TranslateText(184).Replace("%q%", qi.nom_quete));
                    }
                }
                else
                    MessageBox.Show("une incoherance est trouvé sur le systeme des quete.");
                #endregion
            }
            else if (args.Length > 2 && args[1] == "beganQuete")
            {
                #region ajouter une quete au joueur
                //"cmd•beganQuete•nom quete"
                Actor.QuestInformations qi = new Actor.QuestInformations();
                qi.nom_quete = args[2];
                (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).Quests.Add(qi);
                CommonCode.ChatMsgFormat("I", "System", CommonCode.TranslateText(183).Replace("%q%", qi.nom_quete));
                #endregion
            }
            else if (args.Length == 5 && args[1] == "upgradedSpell")
            {
                #region augementation du level d'un sort
                //cmd•upgradedSpell•sortID•level•spellPointsLeft
                Actor pi = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
                int sortID = Convert.ToInt16(args[2]);
                int sortLvl = Convert.ToInt16(args[3]);
                pi.spells.Find(f => f.sortID == sortID).level = sortLvl;
                pi.spellPointLeft = Convert.ToInt16(args[4]);
                // on check si le panneau affichage de sort est present pour augementer le sort visuellement
                if (Manager.manager.GfxTopList.Exists(f => f.Name() == "__showSpellsParent"))
                {
                    MainForm.drawSpellsMenuInParent();

                    // mise à jours des points de sorts disponible au cas ou un changement à eu lieu
                    // cadre général parent
                    Rec showSpellsParent = Manager.manager.GfxTopList.Find(f => f.Name() == "__showSpellsParent") as Rec;
                    Txt spellPointsLeftValue = showSpellsParent.Child.Find(f => f.Name() == "__spellPointsLeftValue") as Txt;
                    spellPointsLeftValue.Text = pi.spellPointLeft.ToString();
                }
                #endregion
            }
            else if(args.Length == 3 && args[1] == "spellHandlerNotFound")
            {
                // cmd•spellHandlerNotFound•" + spell.spellName
                CommonCode.ChatMsgFormat("I", "System", CommonCode.TranslateText(191) + " " + args[2]);
            }
            else if (args.Length == 2 && args[1] == "spellTargetNotAllowed")
            {
                // cmd•spellHandlerNotFound•" + spell.spellName
                CommonCode.ChatMsgFormat("I", "System", CommonCode.TranslateText(192) + " " + args[1]);
            }
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
        static void _passer_La_Main_btn_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // cmd qui termine un combat
            Network.SendMessage("cmd•finishTurn", true);
        }
        
    }
}
