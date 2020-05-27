using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Lidgren.Network;
using mysql;
using MELHARFI.AStarAlgo;
using SERVER.Cryptography;
using SERVER.DataBase;
using SERVER.Enums;
using SERVER.Fight;
using SERVER.Net.Messages.Response;

namespace SERVER
{
    public class CommonCode
    {
        public static int ReturnTimeStamp()
        {
            return (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
        public static void SendMessage(string msg, NetIncomingMessage im, bool crypted)
        {
            try
            {
                NetOutgoingMessage om = MainClass.netServer.CreateMessage(msg);
                if (crypted)
                    om.Encrypt(MainClass.algo);
                MainClass.netServer.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                MainClass.netServer.FlushSendQueue();
            }
            catch
            {
                // ignored
            }
        }
        public static void SendMessage(string msg, NetConnection conn, bool crypted)
        {
            try
            {
                NetOutgoingMessage om = MainClass.netServer.CreateMessage(msg);
                if (crypted)
                    om.Encrypt(MainClass.algo);
                MainClass.netServer.SendMessage(om, conn, NetDeliveryMethod.ReliableOrdered);
                MainClass.netServer.FlushSendQueue();
            }
            catch
            {
                // ignored
            }
        }
        public static void ReadConfigFile()
        {
            #region lecture du fichier config.ini
            string[] configFile = File.ReadAllLines(@"Config.ini");
            foreach (string t in configFile)
            {
                if (t != string.Empty && t.Substring(0, 2) != "//" && t.IndexOf(':') != -1)
                {
                    string[] dataLine = t.Split(':');
                    dataLine[0] = dataLine[0].Replace(" ", "");
                    dataLine[1] = dataLine[1].Replace(" ", "");

                    switch (dataLine[0])
                    {
                        case "port":
                            MainClass.Port = Convert.ToInt32(dataLine[1]);
                            break;
                        case "MaximumConnections":
                            MainClass.MaximumConnections = Convert.ToInt32(dataLine[1]);
                            break;
                        case "ConnectionTimeout":
                            MainClass.ConnectionTimeout = Convert.ToInt32(dataLine[1]);
                            break;
                        case "MaxTimeAfk":
                            MainClass.MaxTimeAfk = Convert.ToInt32(dataLine[1]);
                            break;
                        case "downloadMajorLink":
                            Network.downloadMajorLink = dataLine[1];
                            break;
                        case "downloadRevLink":
                            Network.downloadRevLink = dataLine[1];
                            break;
                        case "MaxStepsInWayPoint":
                            GlobalVariable.MaxStepsInWayPoint = Convert.ToInt32(dataLine[1]);
                            break;
                        case "actionCnt":
                            GlobalVariable.WalkingMaxSteps = Convert.ToInt32(dataLine[1]);
                            break;
                        case "db":
                            MainClass.db = dataLine[1];
                            break;
                        case "user":
                            MainClass.user = dataLine[1];
                            break;
                        case "pass":
                            MainClass.pass = dataLine[1];
                            break;
                        case "hostdb":
                            MainClass.hostdb = dataLine[1];
                            break;
                        case "allowedCharPseudo":
                            Security.allowedCharPseudo = dataLine[1];
                            break;
                        case "allowedCharPwd":
                            Security.allowedCharPwd = dataLine[1];
                            break;
                        case "ChatMessageMaxChar":
                            Network.ChatMessageMaxChar = Convert.ToInt32(dataLine[1]);
                            break;
                        case "LvlStart":
                            MainClass.LvlStart = Convert.ToInt32(dataLine[1]);
                            break;
                        case "StartPdv":
                            MainClass.StartPdv = Convert.ToInt32(dataLine[1]);
                            break;
                        case "AutoUpdatePdv":
                            MainClass.AutoUpdatePdv = Convert.ToInt32(dataLine[1]);
                            break;
                        case "chakralvl2":
                            MainClass.chakralvl1 = Convert.ToInt32(dataLine[1]);
                            break;
                        case "chakralvl3":
                            MainClass.chakralvl2 = Convert.ToInt32(dataLine[1]);
                            break;
                        case "chakralvl4":
                            MainClass.chakralvl3 = Convert.ToInt32(dataLine[1]);
                            break;
                        case "chakralvl5":
                            MainClass.chakralvl4 = Convert.ToInt32(dataLine[1]);
                            break;
                        case "chakralvl6":
                            MainClass.chakralvl5 = Convert.ToInt32(dataLine[1]);
                            break;
                        case "InitialisationBattleWaitTime":
                            MainClass.InitialisationBattleWaitTime = Convert.ToInt32(dataLine[1]);
                            break;
                        case "TimeToPlayInBattle":
                            MainClass.TimeToPlayInBattle = Convert.ToInt32(dataLine[1]);
                            break;
                        case "TimeToSaveDB":
                            MainClass.TimeToSaveDB = Convert.ToInt32(dataLine[1]);
                            break;
                    }
                }
            }
            #endregion
            Console.WriteLine("Variables chargés depuis le fichier Config.ini [ok]");
        }
        public static void TimeStampUpdate(NetConnection nc)
        {
            int timestamp = ReturnTimeStamp();
            foreach (connected connected in DataTables.connected)
                connected.timestamp = timestamp;

            ((Actor)nc.Tag).Timestamp = timestamp;
        }
        public static isFreeCellToWalkDel ReturnFreeCellToWalkFunc(string map)
        {
            switch (map)
            {
                case "Start":
                    return isFreeCellToWalk.Start;
                case "_0_0_0":
                    return isFreeCellToWalk._0_0_0;
                default:
                    throw new Exception("null");
            }
        }
        public static isFreeCellToSpellDel ReturnFreeCellToSpellFunc(string map)
        {
            switch (map)
            {
                case "Start":
                    return isFreeCellToSpell.Start;
                case "_0_0_0":
                    return isFreeCellToSpell._0_0_0;
                default:
                    throw new Exception("null");
            }
        }
        public static string ReturnRandomId()
        {
            string id = "";
            string str = "azertyuiopqsdfghjklmwxcvbn1234567890";
            Random rnd = new Random();
            for (int cnt = 0; cnt < 5; cnt++)
                id += str[rnd.Next(0, 36)];
            return id;
        }
        public static void RefreshStats(NetConnection nt)
        {
            // mise a jours des données du joueurs apres le combats
            Actor actor = (Actor)nt.Tag;

            ////////////////////////////
            players mpi = ((List<players>)DataTables.players).FindLast(f => f.pseudo == actor.Pseudo);
            actor.Pseudo = mpi.pseudo;
            actor.map = mpi.map;
            actor.map_position = new Point(Convert.ToInt32(mpi.map_position.Split('/')[0]), Convert.ToInt32(mpi.map_position.Split('/')[1]));
            actor.classeName = (ActorClass.ClassName)Enum.Parse(typeof(ActorClass.ClassName), mpi.classe);
            Spirit.Name spirit = (Spirit.Name)Enum.Parse(typeof(Spirit.Name), mpi.spirit);
            actor.spirit = spirit;
            actor.spiritLvl = mpi.spiritLevel;
            actor.hiddenVillage = mpi.hiddenVillage;
            actor.maskColorString = mpi.maskColorString;
            actor.directionLook = mpi.directionLook;
            actor.inBattle = mpi.inBattle;
            actor.idBattle = mpi.inBattleID;
            actor.initiative = mpi.initiative;
            actor.Pvp = mpi.pvpEnabled == 0 ? false : true;
            actor.level = mpi.level;
            actor.officialRang = (Rang.official)mpi.rang;
            actor.currentHealth = mpi.currentHealth;
            actor.maxHealth = mpi.maxHEalth;
            actor.xp = mpi.xp;
            actor.doton = mpi.doton;
            actor.katon = mpi.katon;
            actor.futon = mpi.futon;
            actor.raiton = mpi.raiton;
            actor.suiton = mpi.suiton;
            actor.power = mpi.puissance;
            actor.usingDoton = mpi.usingDoton;
            actor.usingKaton = mpi.usingKaton;
            actor.usingFuton = mpi.usingFuton;
            actor.usingRaiton = mpi.usingRaiton;
            actor.usingSuiton = mpi.usingSuiton;
            actor.equipedDoton = mpi.dotonEquiped;
            actor.equipedKaton = mpi.katonEquiped;
            actor.equipedFuton = mpi.futonEquiped;
            actor.equipedRaiton = mpi.raitonEquiped;
            actor.equipedSuiton = mpi.suitonEquiped;
            actor.equipedPower = mpi.puissanceEquiped;
            actor.originalPc = mpi.pc;
            actor.currentPc = mpi.pc;
            actor.originalPm = mpi.pm;
            actor.currentPm = mpi.pm;
            actor.pe = mpi.pe;
            actor.cd = mpi.cd;
            actor.summons = mpi.invoc;
            actor.job1 = mpi.job1;
            actor.job2 = mpi.job2;
            actor.specialty1 = mpi.specialite1;
            actor.specialty2 = mpi.specialite2;
            actor.maxWeight = mpi.maxWeight;
            actor.currentWeight = mpi.currentWeight;
            actor.ryo = mpi.ryo;
            actor.resiDotonPercent = mpi.resiDoton;
            actor.resiKatonPercent = mpi.resiKaton;
            actor.resiFutonPercent = mpi.resiFuton;
            actor.resiRaitonPercent = mpi.resiRaiton;
            actor.resiSuitonPercent = mpi.resiSuiton;
            actor.resiDotonFix = mpi.resiDotonFix;
            actor.resiKatonFix = mpi.resiKatonFix;
            actor.resiFutonFix = mpi.resiFutonFix;
            actor.resiRaitonFix = mpi.resiRaitonFix;
            actor.resiSuitonFix = mpi.resiSuitonFix;
            actor.resiFix = mpi.resiFix;
            actor.dodgePC = mpi.esquivePC;
            actor.dodgePM = mpi.esquivePM;
            actor.dodgePE = mpi.esquivePE;
            actor.dodgeCD = mpi.esquiveCD;
            actor.removePC = mpi.retraitPC;
            actor.removePM = mpi.retraitPM;
            actor.removePE = mpi.retraitPE;
            actor.removeCD = mpi.retraitCD;
            actor.escape = mpi.evasion;
            actor.blocage = mpi.blocage;
            
            string spell = mpi.sorts;
            
            if (spell != "")
            {
                actor.sorts.Clear();
                for (int cnt = 0; cnt < spell.Split('/').Length; cnt++)
                {
                    string tmpData = spell.Split('/')[cnt];
                    Actor.SpellsInformations spellsInformations = new Actor.SpellsInformations
                    {
                        SpellId = Convert.ToInt32(tmpData.Split(':')[0]),
                        SpellPlace = Convert.ToInt32(tmpData.Split(':')[1]),
                        Level = Convert.ToInt32(tmpData.Split(':')[2]),
                        SpellColor = Convert.ToInt32(tmpData.Split(':')[3])
                    };
                    spellsInformations.effect = crypted_data.effects_decoder(spellsInformations.SpellId, spellsInformations.Level);
                    actor.sorts.Add(spellsInformations);
                }
            }

            // dom fix
            actor.domDotonFix = mpi.domDotonFix;
            actor.domKatonFix = mpi.domKatonFix;
            actor.domFutonFix = mpi.domFutonFix;
            actor.domRaitonFix = mpi.domRaitonFix;
            actor.domSuitonFix = mpi.domSuitonFix;
            actor.domFix = mpi.domFix;
            actor.IsFreeCellToWalk = ReturnFreeCellToWalkFunc(mpi.map);
            actor.IsFreeCellToSpell = ReturnFreeCellToSpellFunc(mpi.map);
            actor.spellPointLeft = mpi.spellPointLeft;
            
            /*if (actor.usingDoton >= MainClass.chakralvl5)
                actor.dotonChakraLevel = 6;
            else if (actor.usingDoton >= MainClass.chakralvl4)
                actor.dotonChakraLevel = 5;
            else if (actor.usingDoton >= MainClass.chakralvl3)
                actor.dotonChakraLevel = 4;
            else if (actor.usingDoton >= MainClass.chakralvl2)
                actor.dotonChakraLevel = 3;
            else if (actor.usingDoton >= MainClass.chakralvl1)
                actor.dotonChakraLevel = 2;*/
            
            if (actor.usingKaton >= MainClass.chakralvl5)
                actor.katonChakraLevel = 6;
            else if (actor.usingKaton >= MainClass.chakralvl4)
                actor.katonChakraLevel = 5;
            else if (actor.usingKaton >= MainClass.chakralvl3)
                actor.katonChakraLevel = 4;
            else if (actor.usingKaton >= MainClass.chakralvl2)
                actor.katonChakraLevel = 3;
            else if (actor.usingKaton >= MainClass.chakralvl1)
                actor.katonChakraLevel = 2;
            
            if (actor.usingFuton >= MainClass.chakralvl5)
                actor.futonChakraLevel = 6;
            else if (actor.usingFuton >= MainClass.chakralvl4)
                actor.futonChakraLevel = 5;
            else if (actor.usingFuton >= MainClass.chakralvl3)
                actor.futonChakraLevel = 4;
            else if (actor.usingFuton >= MainClass.chakralvl2)
                actor.futonChakraLevel = 3;
            else if (actor.usingFuton >= MainClass.chakralvl1)
                actor.futonChakraLevel = 2;
            
            if (actor.usingRaiton >= MainClass.chakralvl5)
                actor.raitonChakraLevel = 6;
            else if (actor.usingRaiton >= MainClass.chakralvl4)
                actor.raitonChakraLevel = 5;
            else if (actor.usingRaiton >= MainClass.chakralvl3)
                actor.raitonChakraLevel = 4;
            else if (actor.usingRaiton >= MainClass.chakralvl2)
                actor.raitonChakraLevel = 3;
            else if (actor.usingRaiton >= MainClass.chakralvl1)
                actor.raitonChakraLevel = 2;
            
            if (actor.usingSuiton >= MainClass.chakralvl5)
                actor.suitonChakraLevel = 6;
            else if (actor.usingSuiton >= MainClass.chakralvl4)
                actor.suitonChakraLevel = 5;
            else if (actor.usingSuiton >= MainClass.chakralvl3)
                actor.suitonChakraLevel = 4;
            else if (actor.usingSuiton >= MainClass.chakralvl2)
                actor.suitonChakraLevel = 3;
            else if (actor.usingSuiton >= MainClass.chakralvl1)
                actor.suitonChakraLevel = 2;

            ///////////// récupération des quete faites

            foreach (quete quete in DataTables.quete)
            {
                Actor.QuestInformation qi = new Actor.QuestInformation
                {
                    QuestName = quete.nom_quete,
                    MaxSteps = quete.totalSteps,
                    CurrentStep = quete.currentStep,
                    Submited = Convert.ToBoolean(quete.submited)
                };
                actor.Quests.Add(qi);
            }
        }
        public static bool CloseBattle(Battle battle, bool sendFeedBack)
        {
            // code pour cloturer un combat qui peux etre appelé de plusieurs endrois
            // check s'il ya seulement 2 joueurs dans le combat, 1 dans chaque team
            if (battle.SideA.FindAll(f => f.species == Species.Name.Human || f.species == Species.Name.Summon).Count == 0 || battle.SideB.FindAll(f => f.species == Species.Name.Human || f.species == Species.Name.Summon).Count == 0)
            {
                Team.Side winnerTeam = battle.SideA.FindAll(f => f.species == Species.Name.Human).Count == 0 ? Team.Side.B : Team.Side.A;
                List<Actor> bl = new List<Actor>();
                bl.AddRange(battle.SideA.FindAll(f => f.species == Species.Name.Human));
                bl.AddRange(battle.SideB.FindAll(f => f.species == Species.Name.Human));
                bl.InsertRange(0, battle.DeadPlayers.FindAll(f => f.species == Species.Name.Human));

                foreach (Actor t in bl)
                {
                    NetConnection nc = MainClass.netServer.Connections.Find(f => ((Actor)f.Tag).Pseudo == t.Pseudo);
                    if (nc == null)
                        continue;

                    Actor actorIteration = (Actor) nc.Tag;
                    Console.WriteLine("CloseBattle sent to --> " + actorIteration.Pseudo);
                    actorIteration.inBattle = 0;
                    actorIteration.idBattle = -1;
                    actorIteration.teamSide = Team.Side.None;
                    actorIteration.PlayerChallengeYou = "";
                    actorIteration.YouChallengePlayer = "";
                    if (sendFeedBack)
                    {
                        SendMessage("cmd•CloseBattle•" + BattleType.Type.FreeChallenge + "•" + winnerTeam, nc, true);
                        //Console.WriteLine("<--cmd•CloseBattle•" + BattleType.Type.FreeChallenge + "•" + winnerTeam + " to " + (nc.Tag as Actor).Pseudo);
                    }
                }

                return true;
            }
            return false;
        }
        public static bool IsClosedBattle(Battle battle, bool sendFeedBack)
        {
            // determine si le combat est cloturé puisqu'il ya 3 maniere de cloturer un combat, selon une deconnexion, ou lorsque'un joueur quite un combat ou apres le lancement d'un sort qui tue un adversaire et qui laisse une team avec 0 personne
            // deconnexion,quit,sort
            // si cette methode est modifier, il faut aussi modifier sa surchage juste en dessous
            return CloseBattle(battle, sendFeedBack);
        }
        public static bool IsClosedBattle(Battle battle, Actor playerTargeted)
        {
            // determine si le combat est cloturé puisqu'il ya 3 maniere de cloturer un combat, selon une deconnexion en mode FreePlay, ou lorsque'un joueur quite un combat ou apres le lancement d'un sort qui tue un adversaire et qui laisse une team avec 0 personne
            // deconnexion,quit,sort
            // 2éme surcharge de la méthode isClosedBattle
            if (battle == null) return false;
            // check s'il ya seulement 2 joueurs dans le combat, 1 dans chaque team
            if (
                battle.SideA.FindAll(f => f.species != Species.Name.Summon && f.Pseudo != playerTargeted.Pseudo).Count !=
                0 &&
                battle.SideB.FindAll(f => f.species != Species.Name.Summon && f.Pseudo != playerTargeted.Pseudo).Count !=
                0) return false;
            {
                // type du combat
                //string[] BattleType = _battle.BattleType.Split('/');
                Team.Side winnerTeam;

                if (battle.BattleType == BattleType.Type.VsPnj)
                    winnerTeam = (battle.SideA.Count == 0) ? Team.Side.B : Team.Side.A;
                else
                    winnerTeam = (battle.SideA.FindAll(f => f.species == Species.Name.Human).Count == 0) ? Team.Side.B : Team.Side.A;

                List<Actor> bl = battle.AllPlayersByOrder.FindAll(f => f.species == Species.Name.Human);
                bl.InsertRange(0, battle.DeadPlayers.FindAll(f => f.species == Species.Name.Human));

                foreach (Actor t in bl)
                {
                    NetConnection nc = MainClass.netServer.Connections.Find(f => ((Actor)f.Tag).Pseudo == t.Pseudo);
                    if (nc == null)
                        continue;

                    Actor pi = (Actor)nc.Tag;

                    pi.inBattle = 0;
                    pi.idBattle = -1;
                    pi.teamSide = Team.Side.None;
                    pi.PlayerChallengeYou = "";
                    pi.YouChallengePlayer = "";
                    SendMessage("cmd•CloseBattle•" + battle.BattleType + "•" + winnerTeam, nc, true);
                    //Console.WriteLine("<--cmd•CloseBattle•" + battle.BattleType + "•" + winnerTeam + " to " + pi.Pseudo);

                    // determiner si le combat et du type VsPnj pour une eventuel quete
                        
                    if (battle.BattleType == BattleType.Type.VsPnj)
                    {
                        // commbat contre un PNJ
                        string pnj = battle.BattleFlags[0];
                        string quete = battle.BattleFlags[1];
                        switch (pnj)
                        {
                            case "iruka":
                                if (quete == "FirstFight")
                                {
                                    // verifier si la team gagnante est le joueur, sideA = iruka vus qu'il a bc d'initiative
                                    // valider la quete
                                    if (winnerTeam == Team.Side.B)
                                    {
                                        ((List<quete>)DataTables.quete).Find(f => f.pseudo == pi.Pseudo).submited = 1;
                                        // envoie au joueur la validation de la quete
                                        SendMessage("cmd•submitedQuest•FirstFight•submited", nc, true);
                                        Console.WriteLine("<--cmd•updateQuete•FirstFight•submited to " + pi.Pseudo);
                                    }
                                }
                                break;
                        }
                    }
                }

                // objet droppé VsPnj/iruka/FirstFight
                return true;
            }
        }
        public static int scoop_calculator_standard_algo(Point start, Point end)
        {
            // calcule de la porté
            if (start.X > end.X)
            {
                int x = start.X - end.X;
                int y = 0;

                if (start.Y > end.Y)
                    y = start.Y - end.Y;
                else if(start.Y < end.Y)
                    y = end.Y - start.Y;

                return x + y;
            }
            if (start.X < end.X)
            {
                int x = end.X - start.X;
                int y = 0;
                
                if (start.Y > end.Y)
                    y = start.Y - end.Y;
                else if (start.Y < end.Y)
                    y = end.Y - start.Y;

                return x + y;
                
            }

            return 0;
        }
        public static void FinishTurn(Battle battle, bool increment)
        {
            // code pour passer la main qui poura etre appelé depuis 2 endrois, quand le joueur passe la main lui meme ou quand le temps est ecoulé

            // reinitialisation des compteurs de stats du joueur qui viens de perdre la main
            battle.AllPlayersByOrder[battle.Turn].currentPc = battle.AllPlayersByOrder[battle.Turn].originalPc;
            battle.AllPlayersByOrder[battle.Turn].currentPm = battle.AllPlayersByOrder[battle.Turn].originalPm;

            // check des envoutements du fin de tour, appliquer ceux de ce tour et decrementer les autre en cours
            spellsChecker.BuffCheck(battle.AllPlayersByOrder[battle.Turn], battle, Enums.Buff.State.Fin);

            // incrementation de la main
            if (increment)
            {
                if (battle.Turn + 1 < battle.AllPlayersByOrder.Count)
                    battle.Turn++;
                else
                    battle.Turn = 0;
            }

            Actor currentPlayer = battle.AllPlayersByOrder[battle.Turn];

            // informer les joueurs si increment != true
            if (increment)
                foreach (Actor t in battle.AllPlayersByOrder)
                {
                    NetConnection nc = MainClass.netServer.Connections.Find(f => ((Actor)f.Tag).Pseudo == t.Pseudo);
                    if (nc != null && t.species == Species.Name.Human)
                    {
                        SendMessage("cmd•BattleTurnPast•" + currentPlayer.Pseudo, nc, true);
                        Console.WriteLine("<--cmd•BattleTurnPast•" + currentPlayer.Pseudo);
                    }
                }

            // reinitialisation du compteur a 0
            battle.TimeLeftToPlay = ReturnTimeStamp();

            // si le joueur a 0pm et 0 pc on lui fait passer la main
            if (currentPlayer.originalPc == 0 && currentPlayer.originalPm == 0)
            {
                new Thread(() => {
                    Thread.Sleep(1000);
                    FinishTurn(battle, true);
                }).Start();
                return;
            }

            // check si le joueur qui a la main est une invoc
            switch (currentPlayer.species)
            {
                case Species.Name.Summon:

                    #region
                    if (battle.AllPlayersByOrder[battle.Turn].classeName == ActorClass.ClassName.naruto)
                    {
                        // ici est la logique pour les invocations naruto
                        // variable de controle pour verifier la présence d'un adversaire acoté de l'invoc
                        // cheque si l'invoc est a coté d'un adversaire, si non on recherche
                        Point up = new Point(battle.AllPlayersByOrder[battle.Turn].map_position.X, battle.AllPlayersByOrder[battle.Turn].map_position.Y - 1);
                        Point down = new Point(battle.AllPlayersByOrder[battle.Turn].map_position.X, battle.AllPlayersByOrder[battle.Turn].map_position.Y + 1);
                        Point right = new Point(battle.AllPlayersByOrder[battle.Turn].map_position.X + 1, battle.AllPlayersByOrder[battle.Turn].map_position.Y);
                        Point left = new Point(battle.AllPlayersByOrder[battle.Turn].map_position.X - 1, battle.AllPlayersByOrder[battle.Turn].map_position.Y);
                        List<Actor> handToHandOpponent = new List<Actor>();

                        // variable qui dois contenir le nombre total des points de bloquage qui va permettre de savoir si l'invoc va pouvoir se liberer si un autre adversaire se trouve quelque part
                        int cacBlocage = 0;
                        if (up.Y >= 0 && battle.AllPlayersByOrder.Exists(f => f.map_position.X == up.X && f.map_position.Y == up.Y && f.teamSide != battle.AllPlayersByOrder[battle.Turn].teamSide))
                        {
                            Actor pi = battle.AllPlayersByOrder.Find(f => f.map_position.X == up.X && f.map_position.Y == up.Y);
                            if (pi != null)
                            {
                                cacBlocage += pi.blocage;
                                handToHandOpponent.Add(pi);
                            }
                        }

                        if (down.Y < ScreenManager.TileHeight && battle.AllPlayersByOrder.Exists(f => f.map_position.X == down.X && f.map_position.Y == down.Y && f.teamSide != battle.AllPlayersByOrder[battle.Turn].teamSide))
                        {
                            Actor pi = battle.AllPlayersByOrder.Find(f => f.map_position.X == down.X && f.map_position.Y == down.Y);
                            if (pi != null)
                            {
                                cacBlocage += pi.blocage;
                                handToHandOpponent.Add(pi);
                            }
                        }

                        if (right.X < ScreenManager.TileWidth && battle.AllPlayersByOrder.Exists(f => f.map_position.X == right.X && f.map_position.Y == right.Y && f.teamSide != battle.AllPlayersByOrder[battle.Turn].teamSide))
                        {
                            Actor pi = battle.AllPlayersByOrder.Find(f => f.map_position.X == right.X && f.map_position.Y == right.Y);
                            if (pi != null)
                            {
                                cacBlocage += pi.blocage;
                                handToHandOpponent.Add(pi);
                            }
                        }

                        if (left.X >= 0 && battle.AllPlayersByOrder.Exists(f => f.map_position.X == left.X && f.map_position.Y == left.Y && f.teamSide != battle.AllPlayersByOrder[battle.Turn].teamSide))
                        {
                            Actor pi = battle.AllPlayersByOrder.Find(f => f.map_position.X == left.X && f.map_position.Y == left.Y);
                            if (pi != null)
                            {
                                cacBlocage += pi.blocage;
                                handToHandOpponent.Add(pi);
                            }
                        }

                        // recherche si un joueur a été trouvé au cac
                        // variable de controle pour voir si un joueur humain a été trouvé ailleur si il ya aucun humain au cac
                        bool found = false;

                        // on recherche une cible s'il y à eu aucun adversaire au cac, ou s'il y à des adversaires au cac mais ils sont pas humains (invoc)
                        // on check a la fin si un joueur a été trouvé si non on prend une invoc qui été au cac, found est le variable de controle
                        if (handToHandOpponent.Count == 0 || (handToHandOpponent.Count > 0 && handToHandOpponent.FindAll(f => f.species == Species.Name.Human).Count == 0 && cacBlocage <= battle.AllPlayersByOrder[battle.Turn].escape))
                        {
                            // aucun joueur na été trouvé au cac, mise en recherche
                            // check si l'invoc a plus que 0 pm
                            if (battle.AllPlayersByOrder[battle.Turn].currentPm > 0)
                            {
                                Team.Side team = battle.AllPlayersByOrder[battle.Turn].teamSide;
                                List<FiltrePlayers> fpiiL = new List<FiltrePlayers>();

                                // liste qui dois contenir tous les points qui sont a proximités de notre joueurs
                                List<Actor> piL = (team == Team.Side.A) ? battle.SideB : battle.SideA;
                                List<NearestPoint> tacledPoint = new List<NearestPoint>();      // contiens les points aproximités des joueurs

                                foreach (Actor t in piL)
                                {
                                    Point _up = new Point(t.map_position.X, t.map_position.Y - 1);
                                    Point _down = new Point(t.map_position.X, t.map_position.Y + 1);
                                    Point _right = new Point(t.map_position.X + 1, t.map_position.Y);
                                    Point _left = new Point(t.map_position.X - 1, t.map_position.Y);

                                    // on check si cette cases est occupé déja par un joueur
                                    if (!battle.AllPlayersByOrder.Exists(f => f.map_position.X == _up.X && f.map_position.Y == _up.Y))
                                    {
                                        NearestPoint npUp = new NearestPoint
                                        {
                                            index = t.blocage,
                                            point = _up,
                                            actor = t
                                        };
                                        tacledPoint.Add(npUp);
                                    }
                                    if (!battle.AllPlayersByOrder.Exists(f => f.map_position.X == _down.X && f.map_position.Y == _down.Y))
                                    {
                                        NearestPoint npDown = new NearestPoint();
                                        npDown.index = t.blocage;
                                        npDown.point = _down;
                                        npDown.actor = t;
                                        tacledPoint.Add(npDown);
                                    }
                                    if (!battle.AllPlayersByOrder.Exists(f => f.map_position.X == _right.X && f.map_position.Y == _right.Y))
                                    {
                                        NearestPoint npRight = new NearestPoint();
                                        npRight.index = t.blocage;
                                        npRight.point = _right;
                                        npRight.actor = t;
                                        tacledPoint.Add(npRight);
                                    }
                                    if (!battle.AllPlayersByOrder.Exists(f => f.map_position.X == _left.X && f.map_position.Y == _left.Y))
                                    {
                                        NearestPoint npLeft = new NearestPoint();
                                        npLeft.index = t.blocage;
                                        npLeft.point = _left;
                                        npLeft.actor = t;
                                        tacledPoint.Add(npLeft);
                                    }
                                }

                                /////////////// pathfinding   ////////////////////
                                for (int cnt2 = 0; cnt2 < ((team == Team.Side.A) ? battle.SideB.Count : battle.SideA.Count); cnt2++)
                                {
                                    List<Point> wayPointList = new List<Point>();
                                    MapPoint startPoint = new MapPoint(battle.AllPlayersByOrder[battle.Turn].map_position.X, battle.AllPlayersByOrder[battle.Turn].map_position.Y);
                                    MapPoint endPoint = new MapPoint((team == Team.Side.A) ? battle.SideB[cnt2].map_position.X : battle.SideA[cnt2].map_position.X, (team == Team.Side.A) ? battle.SideB[cnt2].map_position.Y : battle.SideA[cnt2].map_position.Y);
                                    Actor target2 = battle.AllPlayersByOrder.Find(f => f.map_position.X == endPoint.X && f.map_position.Y == endPoint.Y);
                                    if (target2 == null)
                                    {
                                        // si on arrive ici c'est que le joueur n'est pas synchro avec le serveur
                                        // le joueur essai de frapper un adversaire qui est existant chez le client mais pas sur le serveur, a voir le client
                                        Console.WriteLine("sideA has " + battle.SideA.Count + " sideB has " + battle.SideB.Count);
                                        Console.WriteLine("can't reach that code unless if client is not sync with server, no player was found to hit");
                                        FinishTurn(battle, true);
                                        return;
                                    }

                                    byte[,] byteMap = new byte[ScreenManager.TileWidth, ScreenManager.TileHeight];
                                    for (int i = 0; i < ScreenManager.TileWidth; i++)
                                    {
                                        for (int j = 0; j < ScreenManager.TileHeight; j++)
                                        {
                                            if (!battle.IsFreeCellToWalk(new Point(i * 30, j * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == i && f.map_position.Y == j && f.Pseudo != battle.AllPlayersByOrder[battle.Turn].Pseudo && f.Pseudo != target2.Pseudo))
                                                byteMap[i, j] = 3;
                                            else if (tacledPoint.FindAll(f => f.point.X == i && f.point.Y == j && f.actor.Pseudo != battle.AllPlayersByOrder[battle.Turn].Pseudo && f.actor.Pseudo != target2.Pseudo).Count > 0)
                                            {
                                                // il ya plusieurs cases qui sont affecté par un joueur, on calcule le nombre de point de blocage
                                                List<NearestPoint> np = tacledPoint.FindAll(f => f.point.X == i && f.point.Y == j && f.actor.Pseudo != battle.AllPlayersByOrder[battle.Turn].Pseudo && f.actor.Pseudo != target2.Pseudo);
                                                int npTotal = np.Sum(t => t.actor.blocage);

                                                if (npTotal > battle.AllPlayersByOrder[battle.Turn].blocage)
                                                    byteMap[i, j] = 3;
                                                else
                                                    byteMap[i, j] = 0;
                                            }
                                            else
                                                byteMap[i, j] = 0;
                                        }
                                    }
                                    // mise en obstacle de tous les cases qui ont été devais taclés l'invoc lorsqu'il passe a coté

                                    Map map = new Map(ScreenManager.TileWidth, ScreenManager.TileHeight, startPoint, endPoint, byteMap);

                                    if (map.StartPoint != MapPoint.InvalidPoint && map.EndPoint != MapPoint.InvalidPoint)
                                    {
                                        AStar astart = new AStar(map);
                                        List<MapPoint> sol = astart.CalculateBestPath();
                                        if (sol != null)
                                            sol.Reverse();
                                        else
                                        {
                                            // impossible de determiner le chemain, peux etre que la case ciblé est un obstacle
                                            continue;
                                        }
                                        // conversion de la liste MapPoint a une liste Point
                                        wayPointList.AddRange(sol.Select(t => new Point(t.X * 30, t.Y * 30)));
                                    }

                                    // soustraction de la derniere case si elle correspand a la case ciblé, puisque le joueur ne peux pas occuper une case déja occupé
                                    if (wayPointList.Count > 0)
                                        if (wayPointList[wayPointList.Count - 1].X == endPoint.X * 30 && wayPointList[wayPointList.Count - 1].Y == endPoint.Y * 30)
                                            wayPointList.RemoveAt(wayPointList.Count - 1);

                                    Actor pi = (team == Team.Side.A) ? battle.SideB[cnt2] : battle.SideA[cnt2];

                                    FiltrePlayers fpii = new FiltrePlayers();
                                    fpii.actor = pi;
                                    fpii.Waypoint = wayPointList;
                                    fpiiL.Add(fpii);
                                }
                                ////////////////////////////

                                if (fpiiL.Count == 0)
                                {
                                    // normalement au moin une cible devera etre selectionné
                                    // songer a choisir un waypoint aléatoire just comme quoi que la cible ne reste pas immobile
                                    Console.WriteLine("impossible de trouver une cible");
                                    FinishTurn(battle, true);
                                }
                                else
                                {
                                    // on trie depuis notre liste qui contiens les infos sur les adversaire
                                    // on check s'il y a des joueurs atténiable avec les pm qu'on as
                                    FiltrePlayers fpiiSelected;
                                    /////////////////////////////////////////////////

                                    if (fpiiL.FindAll(f => f.Waypoint.Count <= battle.AllPlayersByOrder[battle.Turn].currentPm).Count > 0)
                                    {
                                        // il ya des adversaire atténiables humain
                                        List<FiltrePlayers> fpiiL2 = fpiiL.FindAll(f => f.Waypoint.Count <= battle.AllPlayersByOrder[battle.Turn].currentPm && f.actor.species == Species.Name.Human);
                                        if (fpiiL2.Count > 0)
                                        {
                                            // il y à 1 ou plusieurs adversaires atteniable qui ne sont pas des invocations
                                            // on cherche la cible qui a le moin de pdv

                                            // on verifie si le waypoint généré est vraimenent atteignables ou peux etre taclé par un adversaire
                                            // on check chaque cases de waypoint si un adversaire se trouve sur sa portée de 1 case et si oui on cheque le tacle

                                            for (int cnt = fpiiL.Count; cnt > 0; cnt--)
                                            {
                                                foreach (Point t in fpiiL[cnt - 1].Waypoint)
                                                {
                                                    Point _up = new Point(t.X / 30, (t.Y / 30) - 1);
                                                    Point _down = new Point(t.X / 30, (t.Y / 30) + 1);
                                                    Point _right = new Point((t.X / 30) + 1, t.Y / 30);
                                                    Point _left = new Point((t.X / 30) - 1, t.Y / 30);

                                                    // cheque si les cases contiens un adversaires et si cette case ne sort pas des bord
                                                    int totalBlocage = 0;       // variable de controle qui calcule le nombre de point de blocage qui entoure une cellule et qui determine si l'invoque sera taclé ou pas
                                                    // case up
                                                    if (_up.X < ScreenManager.TileHeight && battle.AllPlayersByOrder.Exists(f => f.map_position.X == _up.X && f.map_position.Y == _up.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide))
                                                    {
                                                        Console.WriteLine("le joueur " + battle.AllPlayersByOrder.Find(f => f.map_position.X == _up.X && f.map_position.Y == _up.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide).Pseudo + " se trouve en haut de l'invocation " + battle.AllPlayersByOrder[battle.Turn].Pseudo);
                                                        totalBlocage += battle.AllPlayersByOrder.Find(f => f.map_position.X == _up.X && f.map_position.Y == _up.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide).blocage;
                                                    }
                                                    // case down
                                                    if (_down.X < ScreenManager.TileHeight && battle.AllPlayersByOrder.Exists(f => f.map_position.X == _down.X && f.map_position.Y == _down.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide))
                                                    {
                                                        Console.WriteLine("le joueur " + battle.AllPlayersByOrder.Find(f => f.map_position.X == _down.X && f.map_position.Y == _down.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide).Pseudo + " se trouve en bas de l'invocation " + battle.AllPlayersByOrder[battle.Turn].Pseudo);
                                                        totalBlocage += battle.AllPlayersByOrder.Find(f => f.map_position.X == _down.X && f.map_position.Y == _down.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide).blocage;
                                                    }
                                                    // case left
                                                    if (_left.X < ScreenManager.TileWidth && battle.AllPlayersByOrder.Exists(f => f.map_position.X == _left.X && f.map_position.Y == _left.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide))
                                                    {
                                                        Console.WriteLine("le joueur " + battle.AllPlayersByOrder.Find(f => f.map_position.X == _left.X && f.map_position.Y == _left.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide).Pseudo + " se trouve a gauche de l'invocation " + battle.AllPlayersByOrder[battle.Turn].Pseudo);
                                                        totalBlocage += battle.AllPlayersByOrder.Find(f => f.map_position.X == _left.X && f.map_position.Y == _left.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide).blocage;
                                                    }
                                                    // case right
                                                    if (_right.X < ScreenManager.TileWidth && battle.AllPlayersByOrder.Exists(f => f.map_position.X == _right.X && f.map_position.Y == _right.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide))
                                                    {
                                                        Console.WriteLine("le joueur " + battle.AllPlayersByOrder.Find(f => f.map_position.X == _right.X && f.map_position.Y == _right.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide).Pseudo + " se trouve a droite de l'invocation " + battle.AllPlayersByOrder[battle.Turn].Pseudo);
                                                        totalBlocage += battle.AllPlayersByOrder.Find(f => f.map_position.X == _right.X && f.map_position.Y == _right.Y && f.teamSide == fpiiL[cnt - 1].actor.teamSide).blocage;
                                                    }

                                                    // on cheque si le joueur sera taclé
                                                    // pour le moment si le joueur est inferrieur de 1 ou plus du nombre de bloquage que sont adversaire il est bloqué,
                                                    // peux etre il faut faire un seuille de tolérance qui lui apporte un maluce qui lui reduit ses pm par ex en vertu de la differance de point de bloquage qui ne dois pas comme meme dépasser une limite de tolérance
                                                    // ce systeme na pas encore été mise en place
                                                    if (battle.AllPlayersByOrder[battle.Turn].escape < totalBlocage)
                                                    {
                                                        // le joueur est bloqué
                                                        Console.WriteLine("le joueur est bloqué vus qu'il a " + battle.AllPlayersByOrder[battle.Turn].escape + " et l'ensemble des point de bloquage des adversaire qui l'entoure est " + totalBlocage);
                                                    }
                                                }
                                            }
                                            /////////////////////////////////////////
                                            ReorderPlayersByPdv2 rpbp2 = new ReorderPlayersByPdv2();
                                            fpiiL2.Sort(rpbp2);
                                            fpiiSelected = fpiiL2[0];
                                            fpiiSelected.HandToHand = true;
                                            found = true;
                                        }
                                        else
                                        {
                                            // il n'y à pas d'adversaire humain mais il y à une invocation atteniable, on choisie aléatoirement une cible qui est surement une invocation
                                            List<FiltrePlayers> fpii3 = fpiiL.FindAll(f => f.Waypoint.Count <= battle.AllPlayersByOrder[battle.Turn].currentPm);
                                            Random rnd = new Random();
                                            int r = rnd.Next(0, fpii3.Count - 1);
                                            fpiiSelected = fpii3[r];
                                            fpiiSelected.HandToHand = true;
                                            // il faut peux etre choisir une cible qui a le moin de pdv
                                        }
                                    }
                                    else
                                    {
                                        // on a pas assez de pm nessessaire pour atteindre un adversaire,
                                        // on trie selon le plus proche
                                        ReorderPlayersByPm rpbp = new ReorderPlayersByPm();
                                        fpiiL.Sort(rpbp);
                                        fpiiSelected = fpiiL[0];
                                        fpiiSelected.HandToHand = false;
                                    }

                                    // si aucun humain n'est au cac et qu'un humain a été trouvé aux alongtours avec les pm necessaire pour l'atteindre, on le prend
                                    if (found || (handToHandOpponent.Count == 0))
                                    {
                                        // un variable qui contiel les pm necessaire pour le deplacement vers une cible assez loin
                                        List<Point> allowedWaypoint = (fpiiSelected.Waypoint.Count > battle.AllPlayersByOrder[battle.Turn].currentPm) ? fpiiSelected.Waypoint.GetRange(0, battle.AllPlayersByOrder[battle.Turn].currentPm) : fpiiSelected.Waypoint;

                                        // verifier si un joueur a été trouvé pour envoyer le waypoint
                                        // envoie d'une requette waypoint a tous les abonnées du combat
                                        List<Actor> abonnedClient = battle.AllPlayersByOrder.FindAll(f => f.species == Species.Name.Human);
                                        string way = allowedWaypoint.Aggregate("", (current, t) => current + (t.X + "," + t.Y + ":"));

                                        if (way == "")
                                            FinishTurn(battle, true);
                                        else
                                            way = way.Substring(0, way.Length - 1);

                                        foreach (Actor t in abonnedClient)
                                        {
                                            NetConnection nt = MainClass.netServer.Connections.Find(f =>
                                            {
                                                var actor = f.Tag as Actor;
                                                return actor != null && (f.Tag != null && actor.Pseudo == t.Pseudo);
                                            });
                                            if (nt == null) continue;
                                            
                                            WayPointReplacedResponseMessage newWayPointGeneratedResponseMessage = new WayPointReplacedResponseMessage();
                                            newWayPointGeneratedResponseMessage.Initialize(new[] { battle.AllPlayersByOrder[battle.Turn].Pseudo, way }, nt);
                                            newWayPointGeneratedResponseMessage.Serialize();
                                            newWayPointGeneratedResponseMessage.Send();
                                        }

                                        // action marche ou courir
                                        battle.AllPlayersByOrder[battle.Turn].animatedAction = (allowedWaypoint.Count > GlobalVariable.WalkingMaxSteps) ? AnimatedActions.Name.run : AnimatedActions.Name.walk;
                                        battle.AllPlayersByOrder[battle.Turn].wayPoint = allowedWaypoint;
                                        battle.AllPlayersByOrder[battle.Turn].currentPm -= allowedWaypoint.Count;
                                        battle.AllPlayersByOrder[battle.Turn].map_position = new Point(allowedWaypoint[allowedWaypoint.Count - 1].X / 30, allowedWaypoint[allowedWaypoint.Count - 1].Y / 30);
                                        Console.WriteLine("new pos for invoc is " + battle.AllPlayersByOrder[battle.Turn].map_position.X + "-" + battle.AllPlayersByOrder[battle.Turn].map_position.Y);
                                        // orientation du joueur selon le waypoint
                                        // determination du derniere orientation
                                        int orientation = 0;

                                        // si plus grand que 1 on change d'orientation
                                        if (allowedWaypoint.Count > 1)
                                        {
                                            for (int cnt2 = 1; cnt2 < allowedWaypoint.Count; cnt2++)
                                            {
                                                if (allowedWaypoint[cnt2 - 1].X > allowedWaypoint[cnt2].X)
                                                    orientation = 3;
                                                else if (allowedWaypoint[cnt2 - 1].X < allowedWaypoint[cnt2].X)
                                                    orientation = 1;
                                                else if (allowedWaypoint[cnt2 - 1].Y > allowedWaypoint[cnt2].Y)
                                                    orientation = 0;
                                                else if (allowedWaypoint[cnt2 - 1].Y < allowedWaypoint[cnt2].Y)
                                                    orientation = 2;
                                            }
                                        }
                                        battle.AllPlayersByOrder[battle.Turn].directionLook = orientation;

                                        if (!fpiiSelected.HandToHand)
                                        {
                                            // cible non au cac
                                            Console.WriteLine("not in cac");
                                        }

                                        if (battle.AllPlayersByOrder[battle.Turn].animatedAction == AnimatedActions.Name.walk)
                                            Thread.Sleep((int)(Math.Round(allowedWaypoint.Count * 0.2) * 1000));
                                        else
                                            Thread.Sleep((int)(Math.Round(allowedWaypoint.Count * ((allowedWaypoint.Count < 10) ? 0.2 : 0.3)) * 1000));

                                        if (fpiiSelected.HandToHand)
                                        {
                                            // sort de l'invocation
                                            // check si le client est autorisé a avoir ce sort

                                            // bug dans l'IA
                                            // on change le 1er charactere qui pointe vers le sort 3, qui est le sort Kage Bunshin No Jutsu, par le numéro 4 qui est le sort Pounch qui a le meme lvl que le sort Kagebunshin no jutso
                                            //_spellInfo = "4" + _spellInfo.Substring(1, _spellInfo.Length - 1);
                                            Console.WriteLine("invoc a trouvé une cible");
                                            // un adversaire se trouve au cac
                                            // on rox terre
                                            // cmd = "cmd•spellTuiles•" + sortID + "•" + (e.X / 30) + "•" + (e.Y / 30)
                                            // _spellinfo = sortID:emplacementID:level:couleur
                                            //string dom = Fight.spellsChecker.calculateDom(_battle.AllPlayersByOrder[_battle.turn], _battle, fpiiSelected.pi.map_position, 3);

                                            List<Actor> bl = battle.AllPlayersByOrder.FindAll(f => f.species == Species.Name.Human);
                                            bl.InsertRange(0, battle.DeadPlayers.FindAll(f => f.species == Species.Name.Human));

                                            // envoie d'une cmd a tous les abonnés au combats
                                            foreach (Actor t in bl)
                                            {
                                                // envoie d'une requette a tous les personnages
                                                NetConnection nim = MainClass.netServer.Connections.Find(f => f.Tag != null && ((Actor)f.Tag).Pseudo == t.Pseudo);
                                                if (nim != null)
                                                {
                                                    //commun.SendMessage("cmd•spellTileGranted•" + _battle.AllPlayersByOrder[_battle.turn].Pseudo + "•" + 4 + "•" + fpiiSelected.pi.map_position.X + "•" + fpiiSelected.pi.map_position.Y + "•" + 0 + "•" + 1 + "•" + dom + "•PcUsed:" + sorts.sort(4).isbl[_SpellLvl - 1].pi.original_Pc, nim, true);
                                                    //Console.WriteLine("<--cmd•spellTileGranted•" + _battle.AllPlayersByOrder[_battle.turn].Pseudo + "•" + 4 + "•" + fpiiSelected.pi.map_position.X + "•" + fpiiSelected.pi.map_position.Y + "•" + 0 + "•" + 1 + "•" + dom + "•PcUsed:" + sorts.sort(4).isbl[_SpellLvl - 1].pi.original_Pc + " to " + (nim.Tag as PlayerInfo).Pseudo);
                                                }
                                            }
                                        }

                                        if (battle.State == battleState.state.started)
                                        {
                                            new Thread(() => {
                                                Thread.Sleep(500);
                                                FinishTurn(battle, true);
                                            }).Start();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // pas assé de pm, l'invoc dois passer son tours
                                FinishTurn(battle, true);
                            }
                        }

                        if (handToHandOpponent.Count > 0 && !found)
                        {
                            Console.WriteLine("invoc au cac");
                            // un joueur est au cac, on rox
                            // recherche sur les cases qui entoure l'invoc des adversaire a proximité
                            // si un cac a eu lieu prevoire d'élargire le timeout qui inclus le temps d'anim rox + pousse

                            // IA pour choisir la bonne cible lorsque l'invoc est entouré par plusieurs ardversaires
                            //Actor selectedChoise = _cacOpponent[0];

                            if (handToHandOpponent.Count > 0)
                            {
                                // instance pour IComparer
                                ReorderPlayersByPdv rpbp = new ReorderPlayersByPdv();

                                // on choisie un adversaire non invoc comme 1er choix et on trie par leurs pdv de plus bas au plus haut
                                if (handToHandOpponent.FindAll(f => f.species == Species.Name.Human).Count > 0)
                                {
                                    // check s'il y à plus d'un adversaires non invocation, comme 1er choix, et i oui, choisie la cible qui a le moins de pdv
                                    List<Actor> isHumanCacOpponent = handToHandOpponent.FindAll(f => f.species == Species.Name.Human);

                                    isHumanCacOpponent.Sort(rpbp);  // triage du plus petit au plus grand
                                    //selectedChoise = isHumanCacOpponent[0]; // on choisie la 1ere occurance au cac ou il y à un seul joueur
                                }
                                else
                                {
                                    // il reste que des invocs, on choisie une cible qui a le moin de pdv
                                    handToHandOpponent.Sort(rpbp);
                                    //selectedChoise = _cacOpponent[0];
                                }
                            }

                            // sort de l'invocation
                            // check si le client est autorisé a avoir ce sort
                            /*players actor = ((List<players>)DataTables.players).FindLast(f => f.pseudo == battle.AllPlayersByOrder[battle.Turn].owner);
                            //string spellInfo = string.Empty;

                            if (actor != null)
                            {
                                List<string> spells = new List<string>(actor.sorts.Split('/'));
                                if (spells.Exists(f => f.Split(':')[0] == "3"))
                                    spellInfo = spells.Find(f => f.Split(':')[0] == "3");
                                else
                                {
                                    Console.WriteLine("code impossible, le joueur dois avoir le sort de l'invoc,si non comment il a invoqué");
                                    spellInfo = "4:0:1:0";
                                }
                            }
                            else
                            {
                                Console.WriteLine("error, impossible de trouver le sortID3 d'invocation chez le joueur, se qui est impossible, puisque l'invoque est la et essai de jouer");
                                // on attribue des données par défaut pour contourner le probleme, sortID lvl 1
                                spellInfo = "4:0:1:0";
                            }*/

                            // on change le 1er charactere qui pointe vers le sort 3, qui est le sort Kage Bunshin No Jutsu, par le numéro 4 qui est le sort Pounch qui dois avoir les meme parametres que le sort Kagebunshin no jutso
                            //spellInfo = "4" + ":" + spellInfo.Split(':')[1] + ":" + spellInfo.Split(':')[2] + ":" + spellInfo.Split(':')[3];
                            //int _SpellLvl = Convert.ToInt32(spellInfo.Split(':')[2]);

                            // un adversaire se trouve au cac
                            // on rox terre
                            // cmd = "cmd•spellTuiles•" + sortID + "•" + (e.X / 30) + "•" + (e.Y / 30)
                            // _spellinfo = sortID:emplacementID:level:couleur
                            //string dom = Fight.spellsChecker.calculateDom(_battle.AllPlayersByOrder[_battle.turn], _battle, selectedChoise.map_position, 3);
                            // envoie d'une cmd a tous les abonnés au combats

                            List<Actor> notInvoc = battle.AllPlayersByOrder.FindAll(f => f.species == Species.Name.Human);
                            foreach (Actor t in notInvoc)
                            {
                                // envoie d'une requette a tous les personnages
                                NetConnection nim = MainClass.netServer.Connections.Find(f => f.Tag != null && ((Actor)f.Tag).Pseudo == t.Pseudo);
                                if (nim != null)
                                {
                                    //commun.SendMessage("cmd•spellTileGranted•" + notInvoc[cnt].Pseudo + "•" + 4 + "•" + selectedChoise.map_position.X + "•" + selectedChoise.map_position.Y + "•" + 0 + "•" + 1 + "•" + dom + "•PcUsed:" + sorts.sort(4).isbl[_SpellLvl - 1].pi.original_Pc, nim, true);
                                    //commun.SendMessage("cmd•spellTileGranted•" + _battle.AllPlayersByOrder[_battle.turn].Pseudo + "•" + 4 + "•" + selectedChoise.map_position.X + "•" + selectedChoise.map_position.Y + "•" + 0 + "•" + 1 + "•" + dom + "•PcUsed:" + sorts.sort(4).isbl[_SpellLvl - 1].pi.original_Pc, nim, true);
                                    //Console.WriteLine("<--cmd•spellTileGranted•" + _battle.AllPlayersByOrder[_battle.turn].Pseudo + "•" + 4 + "•" + selectedChoise.map_position.X + "•" + selectedChoise.map_position.Y + "•" + 0 + "•" + 1 + "•" + dom + "•PcUsed:" + sorts.sort(4).isbl[_SpellLvl - 1].pi.original_Pc + " to " + (nim.Tag as PlayerInfo).Pseudo);
                                }
                            }

                            List<Actor> notInvocNorDead = battle.DeadPlayers.FindAll(f => f.species == Species.Name.Human);
                            foreach (Actor t in notInvocNorDead)
                            {
                                // envoie d'une requette a tous les personnages
                                NetConnection nim = MainClass.netServer.Connections.Find(f => f.Tag != null && ((Actor)f.Tag).Pseudo == t.Pseudo);
                                if (nim != null)
                                {
                                    //commun.SendMessage("cmd•spellTileGranted•" + _battle.AllPlayersByOrder[_battle.turn].Pseudo + "•" + 4 + "•" + selectedChoise.map_position.X + "•" + selectedChoise.map_position.Y + "•" + 0 + "•" + 1 + "•" + dom + "•PcUsed:" + sorts.sort(4).isbl[_SpellLvl - 1].pi.original_Pc, nim, true);
                                    //Console.WriteLine("<--cmd•spellTileGranted•" + _battle.AllPlayersByOrder[_battle.turn].Pseudo + "•" + 4 + "•" + selectedChoise.map_position.X + "•" + selectedChoise.map_position.Y + "•" + 0 + "•" + 1 + "•" + dom + "•PcUsed:" + sorts.sort(4).isbl[_SpellLvl - 1].pi.original_Pc + " to " + (nim.Tag as PlayerInfo).Pseudo);
                                }
                            }

                            // voir si un joueur est mort
                            /*string deadPlayer = dom.Split('|')[5].Split(':')[1];
                        if (deadPlayer != "")
                        {
                            // supprimer le joueur de la liste des joueurs actifs
                            if (commun.isClosedBattle(_battle, selectedChoise))
                                _battle.State = "Closed";
                        }*/

                            // on passe la main
                            if (battle.State == battleState.state.started)
                            {
                                new Thread(() =>
                                {
                                    Thread.Sleep(1000);
                                    FinishTurn(battle, true);
                                }).Start();
                            }
                        }
                    }
                    break;

                    #endregion

                case Species.Name.Pnj:

                    #region
                    // on élimine les sorts qui nécessites plus de pc que ceux disponibles
                    List<Actor.SpellsInformations> allowedSpells = (from t in currentPlayer.sorts where ((List<spells>)DataTables.spells).Find(f => f.spellID == t.SpellId && f.level == t.Level - 1).pc <= currentPlayer.originalPc select new Actor.SpellsInformations {SpellId = t.SpellId, Level = t.Level}).ToList();
                    //List<mysql.spells> spells = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == )

                    // classements des adversaires selons les pdv croissant
                    List<Actor> oppenentByLowerPdv = Tactic.oppenent_By_Lower_Pdv(battle, currentPlayer.teamSide == Team.Side.A ? Team.Side.B : Team.Side.A);
                    int pc = currentPlayer.originalPc;      // copie des pc
                    while (allowedSpells.Count > 0)
                    {
                        // selection aléatoire d'un sort
                        Random r = new Random();
                        int rand = r.Next(0, allowedSpells.Count);
                        spells currentSpell = ((List<spells>)DataTables.spells).Find(f => f.spellID == allowedSpells[rand].SpellId && f.level == allowedSpells[rand].Level);
                        if (currentSpell.pc > pc)
                            continue;

                        // on check si le sort peux etre lancé de la ou on ai sur l'adversaire le plus affaiblie
                        int selectedTarget = -1;
                        for (int cnt1 = 0; cnt1 < oppenentByLowerPdv.Count; cnt1++)
                        {
                            if (Tactic.is_Spell_Allowed_From_Current_Pos(battle, oppenentByLowerPdv[cnt1], currentPlayer, currentPlayer.sorts[rand].SpellId) == "allowed")
                            {
                                // adversaire atteignable en cac
                                selectedTarget = cnt1;
                                break;
                            }
                            if (Tactic.is_Spell_Allowed_From_Current_Pos(battle, oppenentByLowerPdv[cnt1], currentPlayer, currentPlayer.sorts[rand].SpellId) == "denied")
                            {
                                // spell not allowed from current pos
                                // on essai de générer un waypoint
                                List<Point> bestWaypoint = Tactic.best_Waypoint(battle, currentPlayer, oppenentByLowerPdv[cnt1].map_position);
                                if (bestWaypoint.Count > 0)
                                {
                                    // adversaire atteignable de loin
                                    selectedTarget = cnt1;
                                    break;
                                }
                            }
                        }

                        if (selectedTarget > -1)
                        {
                            // un joueur est trouvé
                            // on verifie si le sort peux etre lancé de la ou on ai
                            if (Tactic.is_Spell_Allowed_From_Current_Pos(battle, oppenentByLowerPdv[selectedTarget], currentPlayer, currentPlayer.sorts[rand].SpellId) == "allowed")
                            {
                                // adversaire atteignable
                                Console.WriteLine("cac");

                                // sort qui peux etre lancé
                                //sortID:emplacementID:level:couleur
                                // cmd•spellTuiles•sortID•MousePosX•MousePosY
                                //spellsChecker.spells("cmd•spellTuiles•" + currentPlayer.sorts[rand].sortID + "•" + oppenent_By_Lower_Pdv[selectedTarget].map_position.X + "•" + oppenent_By_Lower_Pdv[selectedTarget].map_position.Y, currentPlayer.sorts[rand].sortID +":0:" + currentPlayer.sorts[rand].lvl + ":0", im);
                                string spellinfo = currentPlayer.sorts[rand].SpellId + ":0:" + currentPlayer.sorts[rand].Level + ":0";
                                bool spellDone = spellsChecker.spells_Of_Invocs(spellinfo, battle, currentPlayer, oppenentByLowerPdv[selectedTarget].map_position, oppenentByLowerPdv[selectedTarget]);
                                if (spellDone)
                                {
                                    // on vérifie s'i reste assez de pc pour utiliser un sort
                                    allowedSpells.Clear();
                                    allowedSpells.AddRange(from t in currentPlayer.sorts where ((List<spells>)DataTables.spells).Find(f => f.spellID == t.SpellId && f.level == t.Level - 1).pc <= currentPlayer.currentPc select new Actor.SpellsInformations {SpellId = t.SpellId, Level = t.Level});
                                }
                                else
                                {
                                    Console.WriteLine("Error 0");
                                }
                            }
                            else if (Tactic.is_Spell_Allowed_From_Current_Pos(battle, oppenentByLowerPdv[selectedTarget], currentPlayer, currentPlayer.sorts[rand].SpellId) == "denied")
                            {
                                // adversaire loin, on dois générer un waypoint qui est normalement atteignable
                                Console.WriteLine("away");
                                // on génére un waypoint jusqu'à l'adversaire
                                List<Point> allowedWaypoint = Tactic.best_Waypoint(battle, currentPlayer, oppenentByLowerPdv[selectedTarget].map_position);

                                // au cas ou il n y à pas assez de pm, on soustrait le reste
                                if (currentPlayer.originalPm < allowedWaypoint.Count)
                                    allowedWaypoint.RemoveRange(currentPlayer.originalPm, allowedWaypoint.Count - currentPlayer.originalPm);

                                // verifier si un joueur a été trouvé pour envoyer le waypoint
                                // envoie d'une requette waypoint a tous les abonnées du combat
                                List<Actor> abonnedClient = battle.AllPlayersByOrder.FindAll(f => f.species == Species.Name.Human);
                                string way = allowedWaypoint.Aggregate("", (current, t) => current + (t.X + "," + t.Y + ":"));

                                if (way == "")
                                    FinishTurn(battle, true);
                                else
                                    way = way.Substring(0, way.Length - 1);

                                foreach (Actor t in abonnedClient)
                                {
                                    NetConnection nt = MainClass.netServer.Connections.Find(f => f.Tag != null && ((Actor)f.Tag).Pseudo == t.Pseudo);
                                    if (nt != null)
                                    {
                                        WayPointReplacedResponseMessage newWayPointGeneratedResponseMessage = new WayPointReplacedResponseMessage();
                                        newWayPointGeneratedResponseMessage.Initialize(new[] { battle.AllPlayersByOrder[battle.Turn].Pseudo, way }, nt);
                                        newWayPointGeneratedResponseMessage.Serialize();
                                        newWayPointGeneratedResponseMessage.Send();
                                    }
                                }

                                // action marche ou courir
                                battle.AllPlayersByOrder[battle.Turn].animatedAction = (allowedWaypoint.Count > GlobalVariable.WalkingMaxSteps) ? AnimatedActions.Name.run : AnimatedActions.Name.walk;
                                battle.AllPlayersByOrder[battle.Turn].wayPoint = allowedWaypoint;
                                battle.AllPlayersByOrder[battle.Turn].currentPm -= allowedWaypoint.Count;
                                battle.AllPlayersByOrder[battle.Turn].map_position = new Point(allowedWaypoint[allowedWaypoint.Count - 1].X / 30, allowedWaypoint[allowedWaypoint.Count - 1].Y / 30);
                                Console.WriteLine("new pos for invoc is " + battle.AllPlayersByOrder[battle.Turn].map_position.X + "-" + battle.AllPlayersByOrder[battle.Turn].map_position.Y);
                                // orientation du joueur selon le waypoint
                                // determination du derniere orientation
                                int orientation = 0;

                                // si plus grand que 1 on change d'orientation
                                if (allowedWaypoint.Count > 1)
                                {
                                    for (int cnt2 = 1; cnt2 < allowedWaypoint.Count; cnt2++)
                                    {
                                        if (allowedWaypoint[cnt2 - 1].X > allowedWaypoint[cnt2].X)
                                            orientation = 3;
                                        else if (allowedWaypoint[cnt2 - 1].X < allowedWaypoint[cnt2].X)
                                            orientation = 1;
                                        else if (allowedWaypoint[cnt2 - 1].Y > allowedWaypoint[cnt2].Y)
                                            orientation = 0;
                                        else if (allowedWaypoint[cnt2 - 1].Y < allowedWaypoint[cnt2].Y)
                                            orientation = 2;
                                    }
                                }
                                battle.AllPlayersByOrder[battle.Turn].directionLook = orientation;

                                // on vérifie si le joueur est au cac pour lancer un sort
                                switch (Tactic.is_Spell_Allowed_From_Current_Pos(battle, oppenentByLowerPdv[selectedTarget], currentPlayer, currentPlayer.sorts[rand].SpellId))
                                {
                                    // faire une enumeration allowed et denied
                                    case "allowed":
                                        // sort qui peux etre lancé
                                        //sortID:emplacementID:level:couleur
                                        // cmd•spellTuiles•sortID•MousePosX•MousePosY
                                        //spellsChecker.spells("cmd•spellTuiles•" + currentPlayer.sorts[rand].sortID + "•" + oppenent_By_Lower_Pdv[selectedTarget].map_position.X + "•" + oppenent_By_Lower_Pdv[selectedTarget].map_position.Y, currentPlayer.sorts[rand].sortID +":0:" + currentPlayer.sorts[rand].lvl + ":0", im);
                                        string spellinfo = currentPlayer.sorts[rand].SpellId + ":0:" + currentPlayer.sorts[rand].Level + ":0";
                                        bool spellDone = spellsChecker.spells_Of_Invocs(spellinfo, battle, currentPlayer, oppenentByLowerPdv[selectedTarget].map_position, oppenentByLowerPdv[selectedTarget]);
                                        if (spellDone)
                                        {
                                            // on vérifie s'i reste assez de pc pour utiliser un sort
                                            allowedSpells.Clear();
                                            allowedSpells.AddRange(from t in currentPlayer.sorts where ((List<spells>)DataTables.spells).Find(f => f.spellID == t.SpellId && f.level == t.Level - 1).pc <= currentPlayer.originalPc select new Actor.SpellsInformations {SpellId = t.SpellId, Level = t.Level});
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error 1");
                                        }
                                        break;

                                    //////////////////////////////////////////////
                                    case "denied":
                                        // impossible de lancer le sort, bizard puisque le waypoint nécessaire pour lancer le sort a été généré et appliqué
                                        Console.WriteLine("Error 2");
                                        break;
                                    default:
                                        Console.WriteLine("Error 3");
                                        break;
                                }
                            }
                            else
                            {
                                // sort non autorisé par les réstrictions suivantes, se qui est impossible puisq'une vérification préalable a déja été faite
                                // spellIntervalNotReached
                                // spellRelanceParTourReached
                                // spellRelanceParJoueurReached
                                Console.WriteLine("Error 4");
                            }
                        }
                        else
                        {
                            // aucun joueur trouvé
                            // on passe la main ou on génére un waypoint aléatoire
                            Console.WriteLine("Error 5");
                        }
                    }
                    // le joueur ne lui reste rien à faire, on passe la main
                    if (battle.State == battleState.state.started)
                    {
                        new Thread(() =>
                        {
                            Thread.Sleep(1000);
                            FinishTurn(battle, true);
                        }).Start();
                    }
                    break;

                    #endregion
            }
        }
        public static List<spell_effect_target.targets> Dispatch_effect_target(spell_effect_target.targets target)
        {
            // retourne les elements d'un target
            List<spell_effect_target.targets> l = new List<spell_effect_target.targets>();

            switch (target)
            {
                case spell_effect_target.targets.all:
                    l.AddRange(Enum.GetValues(typeof(spell_effect_target.targets)).Cast<spell_effect_target.targets>());
                    break;
                case spell_effect_target.targets.ally_1:
                    l.Add(spell_effect_target.targets.ally_1);
                    break;
                case spell_effect_target.targets.ally_all:
                    l.Add(spell_effect_target.targets.ally_all);
                    l.Add(spell_effect_target.targets.ally_1);
                    break;
                case spell_effect_target.targets.ally_summon:
                    l.Add(spell_effect_target.targets.ally_summon);
                    break;
                case spell_effect_target.targets.enemy_1:
                    l.Add(spell_effect_target.targets.enemy_1);
                    break;
                case spell_effect_target.targets.enemy_all:
                    l.Add(spell_effect_target.targets.enemy_1);
                    l.Add(spell_effect_target.targets.enemy_all);
                    break;
                case spell_effect_target.targets.enemy_summon:
                    l.Add(spell_effect_target.targets.enemy_summon);
                    break;
                case spell_effect_target.targets.none:
                    l.Add(spell_effect_target.targets.none);
                    break;
                case spell_effect_target.targets.self:
                    l.Add(spell_effect_target.targets.self);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }

            return l;
        }
        public static spell_effect_target.targets TargetParser(Actor spellCaster, Actor target)
        {
            // return l'énumération selon le type effectTarget d'un effet et selon le target allié, enemie, invoc ...
            if (target == null)
                return spell_effect_target.targets.none;

            if (spellCaster.teamSide == target.teamSide && target.species == Species.Name.Human && spellCaster.Pseudo != target.Pseudo)
                return spell_effect_target.targets.ally_1;

            if (spellCaster.teamSide == target.teamSide && target.species == Species.Name.Human && spellCaster.Pseudo == target.Pseudo)
                return spell_effect_target.targets.self;

            if (spellCaster.teamSide == target.teamSide && target.species == Species.Name.Summon && spellCaster.Pseudo != target.Pseudo)
                return spell_effect_target.targets.ally_summon;

            if (spellCaster.teamSide != target.teamSide && target.species == Species.Name.Human)
                return spell_effect_target.targets.enemy_1;

            if (spellCaster.teamSide != target.teamSide && target.species == Species.Name.Summon)
                return spell_effect_target.targets.enemy_summon;

            return spell_effect_target.targets.none;
        }
        public static List<spell_effect_target.targets> SpellTarget(spells spell)
        {
            List<spell_effect_target.targets> targets = new List<spell_effect_target.targets>();
            if (spell.target != null)
            {
                string[] data = spell.target.Split('#');
                spell_effect_target.targets target;
                foreach (string t in data)
                {
                    Enum.TryParse(t, out target);
                    targets.Add(target);
                }
            }
            else
                targets.Add(spell_effect_target.targets.none);
            return targets;
        }
        public static void RequireBattleData(NetIncomingMessage im)
        {
            Actor actor = (Actor)im.SenderConnection.Tag;
            Battle battle = Battle.Battles.Find(f => f.IdBattle == actor.idBattle);
            
            if (battle == null)
                return;

            battle.SideAValidePos = battleStartPositions.Map(battle.Map, battle).Split('|')[0];
            battle.SideBValidePos = battleStartPositions.Map(battle.Map, battle).Split('|')[1];

            string playersData = battle.SideA.Aggregate("", (current, pit1) => current + (pit1.Pseudo + "#" + pit1.classeName + "#" + pit1.level + "#" + pit1.hiddenVillage + "#" + pit1.maskColorString + "#" + pit1.maxHealth + "#" + pit1.currentHealth + "#" + pit1.officialRang + "#" + pit1.initiative + "#" + pit1.doton + "#" + pit1.katon + "#" + pit1.futon + "#" + pit1.raiton + "#" + pit1.suiton + "#" + pit1.usingDoton + "#" + pit1.usingKaton + "#" + pit1.usingFuton + "#" + pit1.usingRaiton + "#" + pit1.usingSuiton + "#" + pit1.equipedDoton + "#" + pit1.equipedKaton + "#" + pit1.equipedFuton + "#" + pit1.equipedRaiton + "#" + pit1.equipedSuiton + "#" + pit1.currentPc + "#" + pit1.currentPm + "#" + pit1.pe + "#" + pit1.cd + "#" + pit1.summons + "#" + pit1.resiDotonPercent + "#" + pit1.resiKatonPercent + "#" + pit1.resiFutonPercent + "#" + pit1.resiRaitonPercent + "#" + pit1.resiSuitonPercent + "#" + pit1.dodgePC + "#" + pit1.dodgePM + "#" + pit1.dodgePE + "#" + pit1.dodgeCD + "#" + pit1.removePC + "#" + pit1.removePM + "#" + pit1.removePE + "#" + pit1.removeCD + "#" + pit1.escape + "#" + pit1.blocage + "#" + pit1.species.ToString() + "#" + pit1.directionLook + ":"));

            if (playersData != "")
                playersData = playersData.Substring(0, playersData.Length - 1);

            playersData += "|";

            playersData = battle.SideB.Aggregate(playersData, (current, pit2) => current + (pit2.Pseudo + "#" + pit2.classeName + "#" + pit2.level + "#" + pit2.hiddenVillage + "#" + pit2.maskColorString + "#" + pit2.maxHealth + "#" + pit2.currentHealth + "#" + pit2.officialRang + "#" + pit2.initiative + "#" + pit2.doton + "#" + pit2.katon + "#" + pit2.futon + "#" + pit2.raiton + "#" + pit2.suiton + "#" + pit2.usingDoton + "#" + pit2.usingKaton + "#" + pit2.usingFuton + "#" + pit2.usingRaiton + "#" + pit2.usingSuiton + "#" + pit2.equipedDoton + "#" + pit2.equipedKaton + "#" + pit2.equipedFuton + "#" + pit2.equipedRaiton + "#" + pit2.equipedSuiton + "#" + pit2.currentPc + "#" + pit2.currentPm + "#" + pit2.pe + "#" + pit2.cd + "#" + pit2.summons + "#" + pit2.resiDotonPercent + "#" + pit2.resiKatonPercent + "#" + pit2.resiFutonPercent + "#" + pit2.resiRaitonPercent + "#" + pit2.resiSuitonPercent + "#" + pit2.dodgePC + "#" + pit2.dodgePM + "#" + pit2.dodgePE + "#" + pit2.dodgeCD + "#" + pit2.removePC + "#" + pit2.removePM + "#" + pit2.removePE + "#" + pit2.removeCD + "#" + pit2.escape + "#" + pit2.blocage + "#" + pit2.species.ToString() + "#" + pit2.directionLook + ":"));

            if (playersData != "")
                playersData = playersData.Substring(0, playersData.Length - 1);
            ////////////////////////////////////
            string sideA = battle.SideA.Aggregate("", (current, piib) => current + (piib.Pseudo + "|" + piib.map_position.X + "/" + piib.map_position.Y + "#"));

            sideA = sideA.Substring(0, sideA.Length - 1);

            string sideB = battle.SideB.Aggregate("", (current, piib) => current + (piib.Pseudo + "|" + piib.map_position.X + "/" + piib.map_position.Y + "#"));
            sideB = sideB.Substring(0, sideB.Length - 1);

            int timeLeftToPlay = MainClass.InitialisationBattleWaitTime - (ReturnTimeStamp() - battle.Timestamp);
            SendMessage("cmd•challengeBegan•" + playersData + "•" + battleStartPositions.Map(battle.Map, battle) + "•" + timeLeftToPlay + "•" + battle.BattleType + "•" + sideA + "•" + sideB + "•" + battle.State + "•" + battleStartPositions.Map(battle.Map, battle).Split('|')[0] + "•" + battleStartPositions.Map(battle.Map, battle).Split('|')[1], im, true);

            switch (battle.State)
            {
                case battleState.state.started:
                    string data = "";
                    foreach (Actor t1 in battle.SideA)
                    {
                        Actor currentPlayerInfo = t1;
                        string sorts = t1.sorts.Aggregate("", (current, t) => current + (t.SpellId + ":" + t.SpellPlace + ":" + t.Level + ":" + t.SpellColor + "/"));
                        if (sorts != "")
                            sorts = sorts.Substring(0, sorts.Length - 1);

                        data += currentPlayerInfo.Pseudo + "#" + currentPlayerInfo.classeName + "#" + currentPlayerInfo.hiddenVillage + "#" + currentPlayerInfo.maskColorString + "#" + currentPlayerInfo.level + "#" + currentPlayerInfo.officialRang + "#" + currentPlayerInfo.currentHealth + "#" + currentPlayerInfo.maxHealth + "#" + currentPlayerInfo.doton + "#" + currentPlayerInfo.katon + "#" + currentPlayerInfo.futon + "#" + currentPlayerInfo.raiton + "#" + currentPlayerInfo.suiton + "#" + currentPlayerInfo.usingDoton + "#" + currentPlayerInfo.usingKaton + "#" + currentPlayerInfo.usingFuton + "#" + currentPlayerInfo.usingRaiton + "#" + currentPlayerInfo.usingSuiton + "#" + currentPlayerInfo.equipedDoton + "#" + currentPlayerInfo.equipedKaton + "#" + currentPlayerInfo.equipedFuton + "#" + currentPlayerInfo.equipedRaiton + "#" + currentPlayerInfo.equipedSuiton + "#" + currentPlayerInfo.originalPc + "#" + currentPlayerInfo.originalPm + "#" + currentPlayerInfo.pe + "#" + currentPlayerInfo.cd + "#" + currentPlayerInfo.summons + "#" + currentPlayerInfo.initiative + "#" + currentPlayerInfo.dodgePC + "#" + currentPlayerInfo.dodgePM + "#" + currentPlayerInfo.dodgePE + "#" + currentPlayerInfo.dodgeCD + "#" + currentPlayerInfo.removePC + "#" + currentPlayerInfo.removePM + "#" + currentPlayerInfo.removePE + "#" + currentPlayerInfo.removeCD + "#" + currentPlayerInfo.escape + "#" + currentPlayerInfo.blocage + "#" + currentPlayerInfo.power + "#" + currentPlayerInfo.equipedPower + "#" + sorts + "|";
                    }
                    if (data != "")
                        data = data.Substring(0, data.Length - 1) + "•";

                    foreach (Actor t1 in battle.SideB)
                    {
                        Actor currentPlayerInfo = t1;
                        string sorts = t1.sorts.Aggregate("", (current, t) => current + (t.SpellId + ":" + t.SpellPlace + ":" + t.Level + ":" + t.SpellColor + "/"));
                        if (sorts != "")
                            sorts = sorts.Substring(0, sorts.Length - 1);

                        data += currentPlayerInfo.Pseudo + "#" + currentPlayerInfo.classeName + "#" + currentPlayerInfo.hiddenVillage + "#" + currentPlayerInfo.maskColorString + "#" + currentPlayerInfo.level + "#" + currentPlayerInfo.officialRang + "#" + currentPlayerInfo.currentHealth + "#" + currentPlayerInfo.maxHealth + "#" + currentPlayerInfo.doton + "#" + currentPlayerInfo.katon + "#" + currentPlayerInfo.futon + "#" + currentPlayerInfo.raiton + "#" + currentPlayerInfo.suiton + "#" + currentPlayerInfo.usingDoton + "#" + currentPlayerInfo.usingKaton + "#" + currentPlayerInfo.usingFuton + "#" + currentPlayerInfo.usingRaiton + "#" + currentPlayerInfo.usingSuiton + "#" + currentPlayerInfo.equipedDoton + "#" + currentPlayerInfo.equipedKaton + "#" + currentPlayerInfo.equipedFuton + "#" + currentPlayerInfo.equipedRaiton + "#" + currentPlayerInfo.equipedSuiton + "#" + currentPlayerInfo.originalPc + "#" + currentPlayerInfo.originalPm + "#" + currentPlayerInfo.pe + "#" + currentPlayerInfo.cd + "#" + currentPlayerInfo.summons + "#" + currentPlayerInfo.initiative + "#" + currentPlayerInfo.dodgePC + "#" + currentPlayerInfo.dodgePM + "#" + currentPlayerInfo.dodgePE + "#" + currentPlayerInfo.dodgeCD + "#" + currentPlayerInfo.removePC + "#" + currentPlayerInfo.removePM + "#" + currentPlayerInfo.removePE + "#" + currentPlayerInfo.removeCD + "#" + currentPlayerInfo.escape + "#" + currentPlayerInfo.blocage + "#" + currentPlayerInfo.power + "#" + currentPlayerInfo.equipedPower + "#" + sorts + "|";
                    }
                    if (data != "")
                        data = data.Substring(0, data.Length - 1);

                    // envoie les données des joueurs a tous les joueurs dans le combat
                    SendMessage("cmd•BattleStarted•" + data + "•" + battle.AllPlayersByOrder[battle.Turn].Pseudo + "•" + MainClass.TimeToPlayInBattle, im, true);
                    //Console.WriteLine("<--cmd•BattleStarted•" + data + "•" + battle.AllPlayersByOrder[battle.Turn].Pseudo + " to " + (im.SenderConnection.Tag as Actor).Pseudo);
                    break;
                case battleState.state.idle:
                    break;
                case battleState.state.closed:
                    break;
                case battleState.state.initialisation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static List<SortTuileInfo> RhombusShapeWithObtsacle(Actor spellCaster, Point spellPoint, spells spell)
        {
            /* allDirections, losange, rhombus
            ///                 [ ]
            ///              [ ][ ][ ]
            ///           [ ][ ][ ][ ][ ]
            ///        [ ][ ][ ][x][ ][ ][ ]
            ///           [ ][ ][ ][ ][ ]
            ///              [ ][ ][ ]
            ///                 [ ]                 */
            // renvoyer tous les tuiles de la forme losange "Rhombus"
            int pe = (Convert.ToBoolean(spell.peModifiable) ? spellCaster.pe + spell.pe : spell.pe) + spell.distanceFromMelee;
            Battle battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);
            // l'information blockLineOfSight peux etre soustraite depuis l'information spell mais il se peux qu'on a besoin de connaitre tous les tuiles disponible dans un champ sans prendre en consideration la valeur blockLineOfSight qui dans ce cas = false
            // utile quand on veux connaitre le champ d'une zone lors se que distanceFromMele > 0 du coup on aimerai connaitre

            //////////////////////////////////////////////////////////////////////////
            //List<Point> allTuiles = new List<Point>();      // liste qui contiens tous les tuiles affecté par le sort y compris un obstacle ou pas
            List<SortTuileInfo> allTuilesInfo = new List<SortTuileInfo>();

            // 1ere partie consiste a afficher toute la grille de tuiles
            for (int line = 0; line <= pe + spell.distanceFromMelee; line++)
            {
                // insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le bas
                if (line != 0 && spellCaster.map_position.Y + line < ScreenManager.TileHeight)
                {
                    Point p = new Point(spellCaster.map_position.X, spellCaster.map_position.Y + line);
                    //allTuiles.Add(p);

                    if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                    {
                        SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                        if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                        {
                            sti.IsWalkable = true;
                            sti.IsBlockingView = true;
                        }
                        else
                        {
                            sti.IsWalkable = false;
                            sti.IsBlockingView = true;
                        }
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        SortTuileInfo sti = new SortTuileInfo();
                        sti.TuilePoint = p;
                        sti.IsWalkable = true;
                        sti.IsBlockingView = false;
                        allTuilesInfo.Add(sti);
                    }
                }

                // insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le haut
                if (line != 0 && spellCaster.map_position.Y - line >= 0)
                {
                    Point p = new Point(spellCaster.map_position.X, spellCaster.map_position.Y - line);
                    //allTuiles.Add(p);

                    if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                    {
                        SortTuileInfo sti = new SortTuileInfo();
                        sti.TuilePoint = p;
                        if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                        {
                            sti.IsWalkable = true;
                            sti.IsBlockingView = true;
                        }
                        else
                        {
                            sti.IsWalkable = false;
                            sti.IsBlockingView = true;
                        }
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = p,
                            IsWalkable = true,
                            IsBlockingView = false
                        };
                        allTuilesInfo.Add(sti);
                    }
                }

                if (pe == line)
                    break;

                for (int side = 1; side <= pe; side++)
                {
                    // ajouter des tuiles coté en bas a droite
                    if (spellCaster.map_position.X + side < ScreenManager.TileWidth && spellCaster.map_position.Y + line < ScreenManager.TileHeight)
                    {
                        Point p = new Point(spellCaster.map_position.X + side, spellCaster.map_position.Y + line);
                        //allTuiles.Add(p);

                        if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                        {
                            SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                            if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                            {
                                sti.IsWalkable = true;
                                sti.IsBlockingView = true;
                            }
                            else
                            {
                                sti.IsWalkable = false;
                                sti.IsBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = p,
                                IsWalkable = true,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en bas a gauche
                    if (spellCaster.map_position.X - side < ScreenManager.TileWidth && spellCaster.map_position.Y + line < ScreenManager.TileHeight)
                    {
                        Point p = new Point(spellCaster.map_position.X - side, spellCaster.map_position.Y + line);
                        //allTuiles.Add(p);

                        if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                        {
                            SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                            if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                            {
                                sti.IsWalkable = true;
                                sti.IsBlockingView = true;
                            }
                            else
                            {
                                sti.IsWalkable = false;
                                sti.IsBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = p,
                                IsWalkable = true,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en haut a droite
                    if (spellCaster.map_position.X + side < ScreenManager.TileWidth && spellCaster.map_position.Y - line < ScreenManager.TileHeight && line > 0)
                    {
                        Point p = new Point(spellCaster.map_position.X + side, spellCaster.map_position.Y - line);
                        //allTuiles.Add(p);

                        if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                        {
                            SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                            if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                            {
                                sti.IsWalkable = true;
                                sti.IsBlockingView = true;
                            }
                            else
                            {
                                sti.IsWalkable = false;
                                sti.IsBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = p,
                                IsWalkable = true,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en haut a gauche
                    if (spellCaster.map_position.X - side < ScreenManager.TileWidth && spellCaster.map_position.Y - line < ScreenManager.TileHeight && line > 0)
                    {
                        Point p = new Point(spellCaster.map_position.X - side, spellCaster.map_position.Y - line);
                        //allTuiles.Add(p);

                        if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                        {
                            SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                            if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                            {
                                sti.IsWalkable = true;
                                sti.IsBlockingView = true;
                            }
                            else
                            {
                                sti.IsWalkable = false;
                                sti.IsBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = p,
                                IsWalkable = true,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                    }
                    // determiner si le nombre de tuiles attein à cause de la notion qui vaux 1 tuile diagonale vaux 2 tuile, donc on déduit 1pe de chaque ligne
                    if (side + line == pe)
                        break;
                }
            }

            // mise en mode isWalkable = false des tuiles obstacle mais qui laisse la ligne de vue comme meme comme de l'eau ...
            if (Convert.ToBoolean(spell.ligneDeVue))
            {
                // si blockLineOfSight = true, on marque les tuiles non accessible comme étant isWalkable = false
                List<SortTuileInfo> lsti = allTuilesInfo.FindAll(f => f.IsWalkable && !f.IsBlockingView);
                foreach (SortTuileInfo t in lsti)
                    if (!spellCaster.IsFreeCellToSpell(new Point(t.TuilePoint.X * 30, t.TuilePoint.Y * 30)) && battle.AllPlayersByOrder.Exists(f => f.map_position.X == t.TuilePoint.X && f.map_position.Y == t.TuilePoint.Y) && t.IsWalkable && t.IsBlockingView == false)
                        allTuilesInfo.Find(f => f.TuilePoint == t.TuilePoint && f.IsWalkable && !t.IsBlockingView).IsWalkable = false;

                // il faut mettre la position des joueur comme accessible mais qui garde toujours le isWalking = true
                // .....
                // .....
                ///////////

                //////////////////// algo pour determiner se que les obstacles peuvent cacher comme ligne de vue
                // determination de la liste de tous les obstacles
                List<SortTuileInfo> blockViewTile = allTuilesInfo.FindAll(f => f.IsBlockingView);

                // calcules préliminaires
                foreach (SortTuileInfo t in blockViewTile)
                {
                    // determination de la distance entre la position du joueur et l'obstacle
                    int xDistance = Math.Abs(spellCaster.map_position.X - t.TuilePoint.X);
                    int yDistance = Math.Abs(spellCaster.map_position.Y - t.TuilePoint.Y);

                    // determiner le niveau d'envergure de l'angle par tuile

                    // determination de ladirection de l'angle
                    bool rightDirection = false;
                    bool leftDirection = false;
                    bool downDirection = false;
                    bool upDirection = false;

                    if (spellCaster.map_position.X > t.TuilePoint.X)
                        leftDirection = true;
                    else if (spellCaster.map_position.X < t.TuilePoint.X)
                        rightDirection = true;

                    if (spellCaster.map_position.Y > t.TuilePoint.Y)
                        upDirection = true;
                    else if (spellCaster.map_position.Y < t.TuilePoint.Y)
                        downDirection = true;

                    // point de départ du polygone
                    Point pointAOfObstacle = new Point();
                    Point pointBOfObstacle = new Point();

                    // coordonnées de l'obstacle en cours de vérifications selons la position du joueur
                    if (upDirection && spellCaster.map_position.X != t.TuilePoint.X && spellCaster.map_position.Y != t.TuilePoint.Y)
                    {
                        // calcule des point d'intersection avec l'obstacle pour tracer la ligne
                        pointAOfObstacle.X = rightDirection ? t.TuilePoint.X : t.TuilePoint.X + 1;
                        pointAOfObstacle.Y = t.TuilePoint.Y;

                        pointBOfObstacle.X = rightDirection ? t.TuilePoint.X + 1 : t.TuilePoint.X;
                        pointBOfObstacle.Y = t.TuilePoint.Y + 1;
                    }
                    else if (downDirection && spellCaster.map_position.X != t.TuilePoint.X && spellCaster.map_position.Y != t.TuilePoint.Y)
                    {
                        // calcule des point d'intersection avec l'obstacle pour tracer la ligne
                        pointAOfObstacle.X = rightDirection ? t.TuilePoint.X + 1 : t.TuilePoint.X;
                        pointAOfObstacle.Y = t.TuilePoint.Y;

                        pointBOfObstacle.X = rightDirection ? t.TuilePoint.X : t.TuilePoint.X + 1;
                        pointBOfObstacle.Y = t.TuilePoint.Y + 1;
                    }
                    else if ((upDirection || downDirection) && !rightDirection && !leftDirection)
                    {
                        // le joueur est aligné horizontalement
                        pointAOfObstacle.X = t.TuilePoint.X;
                        pointAOfObstacle.Y = upDirection ? t.TuilePoint.Y + 1 : t.TuilePoint.Y;

                        pointBOfObstacle.X = t.TuilePoint.X + 1;
                        pointBOfObstacle.Y = upDirection ? t.TuilePoint.Y + 1 : t.TuilePoint.Y;
                    }
                    else if ((rightDirection || leftDirection) && !upDirection && !downDirection)
                    {
                        // le joueur est aligné horizontalement
                        pointAOfObstacle.X = rightDirection ? t.TuilePoint.X : t.TuilePoint.X + 1;
                        pointAOfObstacle.Y = t.TuilePoint.Y;

                        pointBOfObstacle.X = rightDirection ? t.TuilePoint.X : t.TuilePoint.X + 1;
                        pointBOfObstacle.Y = t.TuilePoint.Y + 1;
                    }

                    // calcule de l'envergure de l'angle par tuile passé
                    /*if (upDirection && (rightDirection || leftDirection))
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
                    }*/

                    float angleA = Math.Abs(upDirection ? ((yDistance * 30) + 15) / (xDistance - 0.5F) : (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F));
                    float angleB = Math.Abs(upDirection ? (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F) : ((yDistance * 30) + 15) / (xDistance - 0.5F));

                    /*/List<Point> nextPointAl = new List<Point>();
                    //List<Point> nextPointBl = new List<Point>();

                    for (int j = 1; j <= pe; j++)
                    {
                        if (upDirection && rightDirection)
                            nextPointAl.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(angleA * j)));
                        else if (upDirection && leftDirection)
                            nextPointAl.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) - (int)(angleA * j)));
                        else if (downDirection && rightDirection)
                            nextPointAl.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) + (int)(angleA * j)));
                        else if (downDirection && leftDirection)
                            nextPointAl.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) + (int)(angleA * j)));
                        else if (upDirection && !rightDirection && !leftDirection && !downDirection)
                            nextPointAl.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) - (int)(angleA * j)));
                        else if (downDirection && !rightDirection && !leftDirection && !upDirection)
                            nextPointAl.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) + (int)(angleA * j)));
                        else if (rightDirection && !downDirection && !upDirection && !leftDirection)
                            nextPointAl.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(angleA * j)));
                        else if (leftDirection && !downDirection && !upDirection && !rightDirection)
                            nextPointAl.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(angleA * j)));
                    }

                    for (int j = 1; j <= pe; j++)
                    {
                        if (upDirection && rightDirection)
                            nextPointBl.Add(new Point((pointBOfObstacle.X * 30) + (j * 30) + 30, (pointBOfObstacle.Y * 30) - (int)(angleB * j)));
                        else if (upDirection && leftDirection)
                            nextPointBl.Add(new Point((pointBOfObstacle.X * 30) - (j * 30) - 30, (pointBOfObstacle.Y * 30) - (int)(angleB * j)));
                        else if (downDirection && rightDirection)
                            nextPointBl.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(angleB * j)));
                        else if (downDirection && leftDirection)
                            nextPointBl.Add(new Point((pointBOfObstacle.X * 30) - (j * 30), (pointBOfObstacle.Y * 30) + (int)(angleB * j)));
                        else if (upDirection && !rightDirection && !leftDirection && !downDirection)
                            nextPointBl.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) - (int)(angleA * j)));
                        else if (downDirection && !rightDirection && !leftDirection && !upDirection)
                            nextPointBl.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(angleA * j)));
                        else if (rightDirection && !upDirection && !downDirection && !leftDirection)
                            nextPointBl.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(angleA * j)));
                        else if (leftDirection && !upDirection && !downDirection && !rightDirection)
                            nextPointBl.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(angleA * j)));
                    }*/

                    // tracage du cadre qui délimite les tuiles entre le joueur et le champ de vision
                    foreach (SortTuileInfo t1 in allTuilesInfo)
                    {
                        if (t1.TuilePoint == t.TuilePoint)
                            continue;
                        // check si la tuile en cours se trouve entre les angles A et B
                        if (upDirection && rightDirection && !leftDirection)
                        {
                            if (t1.TuilePoint.X >= t.TuilePoint.X && t1.TuilePoint.Y <= t.TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle superieur à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(t1.TuilePoint.X - t.TuilePoint.X);
                                int y = Math.Abs(t.TuilePoint.Y - t1.TuilePoint.Y);
                                // cheque si la tuile en cours est en haut de l'angle B, puis 2éme check pour l'angle A
                                if (y < (angleA * (x + 1) / 30))
                                    if (y + 1 > Math.Floor((angleB * x) / 30))
                                    {
                                        t1.IsBlockingView = true;
                                        t1.IsWalkable = false;
                                    }
                            }
                        }
                        else if (upDirection && !rightDirection && leftDirection)
                        {
                            if (t1.TuilePoint.X <= t.TuilePoint.X && t1.TuilePoint.Y <= t.TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle superieur à gauche du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(t1.TuilePoint.X - t.TuilePoint.X);
                                int y = Math.Abs(t.TuilePoint.Y - t1.TuilePoint.Y);
                                // cheque si la tuile en cours est en haut de l'angle B, puis 2éme check pour l'angle A
                                if (y < (angleA * (x + 1)) / 30)
                                    if ((y + 1) > Math.Floor(((angleB * x) / 30)))
                                    {
                                        t1.IsBlockingView = true;
                                        t1.IsWalkable = false;
                                    }
                            }
                        }
                        else if (downDirection && !rightDirection && leftDirection)
                        {
                            if (t1.TuilePoint.X <= t.TuilePoint.X && t1.TuilePoint.Y >= t.TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle inférieure à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(t1.TuilePoint.X - t.TuilePoint.X);
                                int y = Math.Abs(t.TuilePoint.Y - t1.TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if (y < ((angleB * (x + 1)) / 30))
                                    if (y + 1 > Math.Floor(((angleA * (x)) / 30)))
                                    {
                                        t1.IsBlockingView = true;
                                        t1.IsWalkable = false;
                                    }
                            }
                        }
                        else if (downDirection && rightDirection && !leftDirection)
                        {
                            if (t1.TuilePoint.X >= t.TuilePoint.X && t1.TuilePoint.Y >= t.TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle inférieure à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(t1.TuilePoint.X - t.TuilePoint.X);
                                int y = Math.Abs(t.TuilePoint.Y - t1.TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if (y < ((angleB * (x + 1)) / 30))
                                    if (y + 1 > Math.Floor(((angleA * (x)) / 30)))
                                    {
                                        t1.IsBlockingView = true;
                                        t1.IsWalkable = false;
                                    }
                            }
                        }
                        else if (rightDirection && !leftDirection && !upDirection && !downDirection)
                        {
                            if (t1.TuilePoint.X >= t.TuilePoint.X)
                            {
                                int x = Math.Abs(t1.TuilePoint.X - t.TuilePoint.X);
                                int y = Math.Abs(t.TuilePoint.Y - t1.TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if (y < ((angleB * (x + 1)) / 30))
                                {
                                    t1.IsBlockingView = true;
                                    t1.IsWalkable = false;
                                }
                            }
                        }
                        else if (leftDirection && !rightDirection && !upDirection && !downDirection)
                        {
                            if (t1.TuilePoint.X <= t.TuilePoint.X)
                            {
                                int x = Math.Abs(t1.TuilePoint.X - t.TuilePoint.X);
                                int y = Math.Abs(t.TuilePoint.Y - t1.TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if (y < ((angleB * (x + 1)) / 30))
                                {
                                    t1.IsBlockingView = true;
                                    t1.IsWalkable = false;
                                }
                            }
                        }
                        else if (upDirection && !downDirection && !leftDirection && !rightDirection)
                        {
                            if (t1.TuilePoint.Y <= t.TuilePoint.Y)
                            {
                                int x = Math.Abs(t1.TuilePoint.X - t.TuilePoint.X);
                                int y = Math.Abs(t.TuilePoint.Y - t1.TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if (y >= ((angleB * (x)) / 30))
                                {
                                    t1.IsBlockingView = true;
                                    t1.IsWalkable = false;
                                }
                            }
                        }
                        else if (downDirection && !upDirection && !leftDirection && !rightDirection)
                        {
                            if (t1.TuilePoint.Y >= t.TuilePoint.Y)
                            {
                                int x = Math.Abs(t1.TuilePoint.X - t.TuilePoint.X);
                                int y = Math.Abs(t.TuilePoint.Y - t1.TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if (y >= ((angleA * (x)) / 30))
                                {
                                    t1.IsBlockingView = true;
                                    t1.IsWalkable = false;
                                }
                            }
                        }
                    }
                }
            }
            return allTuilesInfo;
        }
        public static List<SortTuileInfo> RhombusShapeWithoutObstacle(Actor spellCaster, int pe)
        {
            /* allDirections, losange, rhombus
            ///                 [ ]
            ///              [ ][ ][ ]
            ///           [ ][ ][ ][ ][ ]
            ///        [ ][ ][ ][x][ ][ ][ ]
            ///           [ ][ ][ ][ ][ ]
            ///              [ ][ ][ ]
            ///                 [ ]             */
            
            //List<Point> allTuiles = new List<Point>();      // liste qui contiens tous les tuiles affecté par le sort y compris un obstacle ou pas, a supprimer puisqu'elle sert pas dans cette fonction
            List<SortTuileInfo> allTuilesInfo = new List<SortTuileInfo>();
            Battle battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);

            // 1ere partie consiste a afficher toute la grille de tuiles
            for (int line = 0; line <= pe; line++)
            {
                // insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le bas
                if (line != 0 && spellCaster.map_position.Y + line < ScreenManager.TileHeight)
                {
                    Point p = new Point(spellCaster.map_position.X, spellCaster.map_position.Y + line);
                    //allTuiles.Add(p);

                    if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                    {
                        SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                        if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                        {
                            sti.IsWalkable = true;
                            sti.IsBlockingView = true;
                        }
                        else
                        {
                            sti.IsWalkable = false;
                            sti.IsBlockingView = true;
                        }
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = p,
                            IsWalkable = true,
                            IsBlockingView = false
                        };
                        allTuilesInfo.Add(sti);
                    }
                }

                // insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le haut
                if (line != 0 && spellCaster.map_position.Y - line >= 0)
                {
                    Point p = new Point(spellCaster.map_position.X, spellCaster.map_position.Y - line);
                    //allTuiles.Add(p);

                    if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                    {
                        SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                        if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                        {
                            sti.IsWalkable = true;
                            sti.IsBlockingView = true;
                        }
                        else
                        {
                            sti.IsWalkable = false;
                            sti.IsBlockingView = true;
                        }
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = p,
                            IsWalkable = true,
                            IsBlockingView = false
                        };
                        allTuilesInfo.Add(sti);
                    }
                }

                if (pe == line)
                    break;

                for (int side = 1; side <= pe; side++)
                {
                    // ajouter des tuiles coté en bas a droite
                    if (spellCaster.map_position.X + side < ScreenManager.TileWidth && spellCaster.map_position.Y + line < ScreenManager.TileHeight)
                    {
                        Point p = new Point(spellCaster.map_position.X + side, spellCaster.map_position.Y + line);
                        //allTuiles.Add(p);

                        if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                        {
                            SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                            if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                            {
                                sti.IsWalkable = true;
                                sti.IsBlockingView = true;
                            }
                            else
                            {
                                sti.IsWalkable = false;
                                sti.IsBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = p,
                                IsWalkable = true,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en bas a gauche
                    if (spellCaster.map_position.X - side < ScreenManager.TileWidth && spellCaster.map_position.Y + line < ScreenManager.TileHeight)
                    {
                        Point p = new Point(spellCaster.map_position.X - side, spellCaster.map_position.Y + line);
                        //allTuiles.Add(p);

                        if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                        {
                            SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                            if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                            {
                                sti.IsWalkable = true;
                                sti.IsBlockingView = true;
                            }
                            else
                            {
                                sti.IsWalkable = false;
                                sti.IsBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = p,
                                IsWalkable = true,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en haut a droite
                    if (spellCaster.map_position.X + side < ScreenManager.TileWidth && spellCaster.map_position.Y - line < ScreenManager.TileHeight && line > 0)
                    {
                        Point p = new Point(spellCaster.map_position.X + side, spellCaster.map_position.Y - line);
                        //allTuiles.Add(p);

                        if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                        {
                            SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                            if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                            {
                                sti.IsWalkable = true;
                                sti.IsBlockingView = true;
                            }
                            else
                            {
                                sti.IsWalkable = false;
                                sti.IsBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = p,
                                IsWalkable = true,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en haut a gauche
                    if (spellCaster.map_position.X - side < ScreenManager.TileWidth && spellCaster.map_position.Y - line < ScreenManager.TileHeight && line > 0)
                    {
                        Point p = new Point(spellCaster.map_position.X - side, spellCaster.map_position.Y - line);
                        //allTuiles.Add(p);

                        if (!spellCaster.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
                        {
                            SortTuileInfo sti = new SortTuileInfo {TuilePoint = p};
                            if (battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
                            {
                                sti.IsWalkable = true;
                                sti.IsBlockingView = true;
                            }
                            else
                            {
                                sti.IsWalkable = false;
                                sti.IsBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = p,
                                IsWalkable = true,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                    }
                    // determiner si le nombre de tuiles attein à cause de la notion qui vaux 1 tuile diagonale vaux 2 tuile, donc on déduit 1pe de chaque ligne
                    if (side + line == pe)
                        break;
                }
            }

            return allTuilesInfo;
        }
        public static List<SortTuileInfo> PerpondicularShapeWithObstacle(Actor spellCaster, Point spellPoint, spells spell)
        {
            #region renvoie tous les tuile de la forme perpondiculaire avec la prise en charge de LDV y compris la zone didtanceFromMelee qui devrais etre soustrait apres grace à la methode perpondicularShapeWithoutObstacle
            /*                           [ ]
            ///                          [ ]
            ///                          [ ]
            /// exemple tile    [ ][ ][ ][x][ ][ ][ ]
            /// ///                      [ ]
            ///                          [ ]
            ///                          [ ]                    */
            int pe = (Convert.ToBoolean(spell.peModifiable) ? spellCaster.pe + spell.pe : spell.pe) + spell.distanceFromMelee;
            Battle battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);
            bool inEmptyTileOnly = false;
            List<spell_effect_target.targets> spellTargets = SpellTarget(spell);
            List<SortTuileInfo> allTuilesInfo = new List<SortTuileInfo>();

            if (spellTargets.Exists(f => f == spell_effect_target.targets.none) && spellTargets.Count == 1)
                inEmptyTileOnly = true;
            ///////////////////////////////////////////
            bool allowFirstOpponentBlockingInLeft = true;     // ne pas bloque la ligne de vue de gauche sur le 1er adversaire rencontré puisque le systeme le considere comme obstacle alors qu'il dois avoir le focus sur sa case, pas comme les obstacle du map
            bool allowFirstOpponentBlockingInRight = true;    // ne pas bloque la ligne de vue de droite le 1er adversaire rencontré puisque le systeme le considere comme obstacle alors qu'il dois avoir le focus sur sa case, pas comme les obstacle du map
            bool allowFirstOpponentBlockingInUp = true;       // ne pas bloque la ligne de vue du haut le 1er adversaire rencontré puisque le systeme le considere comme obstacle alors qu'il dois avoir le focus sur sa case, pas comme les obstacle du map
            bool allowFirstOpponentBlockingInDown = true;     // ne pas bloque la ligne de vue du bas le 1er adversaire rencontré puisque le systeme le considere comme obstacle alors qu'il dois avoir le focus sur sa case, pas comme les obstacle du map

            for(int cnt = 0; cnt < pe; cnt++)
            {
                // check si la case à droite qu'on veux vérifier ne sort pas des bord
                /////////////// right //////////////
                if (spellCaster.map_position.X + 1 + cnt < ScreenManager.TileWidth && spellCaster.map_position.Y < ScreenManager.TileHeight)
                {
                    // check si un joueur se trouve sur la pos à droite pour bloquer les prochain tuiles
                    Actor chkObjFoundInTheRight = battle.AllPlayersByOrder.Find(f => f.map_position.X == (spellCaster.map_position.X + 1 + cnt) && f.map_position.Y == spellCaster.map_position.Y);
                    if (chkObjFoundInTheRight != null)
                    {
                        if (allowFirstOpponentBlockingInRight && !inEmptyTileOnly)
                        {
                            //il n y à pas eu d'obstacle avant
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = new Point(spellCaster.map_position.X + 1 + cnt, spellCaster.map_position.Y),
                                IsWalkable = true,
                                IsBlockingView = true
                            };
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = new Point(spellCaster.map_position.X + 1 + cnt, spellCaster.map_position.Y),
                                IsWalkable = false,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                        allowFirstOpponentBlockingInRight = false;
                    }
                    else if (!spellCaster.IsFreeCellToSpell(new Point((spellCaster.map_position.X + 1 + cnt) * 30, spellCaster.map_position.Y * 30)))
                    {
                        // check si un obstacle se trouve sur le map (NON JOUEUR)
                        allowFirstOpponentBlockingInRight = true;
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = new Point(spellCaster.map_position.X + 1 + cnt, spellCaster.map_position.Y),
                            IsWalkable = false,
                            IsBlockingView = true
                        };
                        allTuilesInfo.Add(sti);
                    }
                    else
                    { 
                        SortTuileInfo sti = new SortTuileInfo();
                        sti.TuilePoint = new Point(spellCaster.map_position.X + 1 + cnt, spellCaster.map_position.Y);
                        sti.IsWalkable = true;
                        sti.IsBlockingView = false;
                        allTuilesInfo.Add(sti);
                    }
                }

                /////////////// left //////////////
                if (spellCaster.map_position.X - 1 - cnt < ScreenManager.TileWidth && spellCaster.map_position.Y < ScreenManager.TileHeight)
                {
                    // check si un joueur se trouve sur la pos à droite pour bloquer les prochain tuiles
                    Actor chkObjFoundInTheLeft = battle.AllPlayersByOrder.Find(f => f.map_position.X == (spellCaster.map_position.X - 1 - cnt) && f.map_position.Y == spellCaster.map_position.Y);
                    if (chkObjFoundInTheLeft != null)
                    {
                        if (allowFirstOpponentBlockingInLeft && !inEmptyTileOnly)
                        {
                            //il n y à pas eu d'obstacle avant
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = new Point(spellCaster.map_position.X - 1 - cnt, spellCaster.map_position.Y),
                                IsWalkable = true,
                                IsBlockingView = true
                            };
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = new Point(spellCaster.map_position.X - 1 - cnt, spellCaster.map_position.Y),
                                IsWalkable = false,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                        allowFirstOpponentBlockingInLeft = false;
                    }
                    else if (!spellCaster.IsFreeCellToSpell(new Point((spellCaster.map_position.X - 1 - cnt) * 30, (spellCaster.map_position.Y * 30))))
                    {
                        // check si un obstacle se trouve sur le map (NON JOUEUR)
                        allowFirstOpponentBlockingInLeft = true;
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = new Point(spellCaster.map_position.X - 1 - cnt, spellCaster.map_position.Y),
                            IsWalkable = false,
                            IsBlockingView = true
                        };
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = new Point(spellCaster.map_position.X - 1 - cnt, spellCaster.map_position.Y),
                            IsWalkable = true,
                            IsBlockingView = false
                        };
                        allTuilesInfo.Add(sti);
                    }
                }

                /////////////// up //////////////
                if (spellCaster.map_position.X < ScreenManager.TileWidth && spellCaster.map_position.Y - 1 - cnt < ScreenManager.TileHeight)
                {
                    // check si un joueur se trouve sur la pos à droite pour bloquer les prochain tuiles
                    Actor chkObjFoundInTheUp = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellCaster.map_position.X && f.map_position.Y == spellCaster.map_position.Y - 1 - cnt);
                    if (chkObjFoundInTheUp != null)
                    {
                        if (allowFirstOpponentBlockingInUp && !inEmptyTileOnly)
                        {
                            //il n y à pas eu d'obstacle avant
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y - 1 - cnt),
                                IsWalkable = true,
                                IsBlockingView = true
                            };
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y - 1 - cnt),
                                IsWalkable = false,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                        allowFirstOpponentBlockingInUp = false;
                    }
                    else if (!spellCaster.IsFreeCellToSpell(new Point(spellCaster.map_position.X * 30, (spellCaster.map_position.Y - 1 - cnt)* 30)))
                    {
                        // check si un obstacle se trouve sur le map (NON JOUEUR)
                        allowFirstOpponentBlockingInUp = true;
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y - 1 - cnt),
                            IsWalkable = false,
                            IsBlockingView = true
                        };
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y - 1 - cnt),
                            IsWalkable = true,
                            IsBlockingView = false
                        };
                        allTuilesInfo.Add(sti);
                    }
                }

                /////////////// down //////////////
                if (spellCaster.map_position.X < ScreenManager.TileWidth && spellCaster.map_position.Y + 1 + cnt < ScreenManager.TileHeight)
                {
                    // check si un joueur se trouve sur la pos à droite pour bloquer les prochain tuiles
                    Actor chkObjFoundInTheDown = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellCaster.map_position.X && f.map_position.Y == spellCaster.map_position.Y + 1 + cnt);
                    if (chkObjFoundInTheDown != null)
                    {
                        if (allowFirstOpponentBlockingInDown && !inEmptyTileOnly)
                        {
                            //il n y à pas eu d'obstacle avant
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y + 1 + cnt),
                                IsWalkable = true,
                                IsBlockingView = true
                            };
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            SortTuileInfo sti = new SortTuileInfo
                            {
                                TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y + 1 + cnt),
                                IsWalkable = false,
                                IsBlockingView = false
                            };
                            allTuilesInfo.Add(sti);
                        }
                        allowFirstOpponentBlockingInDown = false;
                    }
                    else if (!spellCaster.IsFreeCellToSpell(new Point(spellCaster.map_position.X * 30, (spellCaster.map_position.Y + 1 + cnt) * 30)))
                    {
                        // check si un obstacle se trouve sur le map (NON JOUEUR)
                        allowFirstOpponentBlockingInDown = true;
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y + 1 + cnt),
                            IsWalkable = false,
                            IsBlockingView = true
                        };
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        SortTuileInfo sti = new SortTuileInfo
                        {
                            TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y + 1 + cnt),
                            IsWalkable = true,
                            IsBlockingView = false
                        };
                        allTuilesInfo.Add(sti);
                    }
                }
            }

            return allTuilesInfo;
            #endregion
        }
        public static List<SortTuileInfo> PerpondicularShapeWithoutObstacle(Actor spellCaster, int pe)
        {
            #region renvoie tous les tuiles de la forme perpondiculaire sans prise en charge de LDV, utilisé pour connaitre les case non accessible au sort comme lors du distanceFromMelee qu'on le soustrait de la liste englobant toute la zone
            /*/                          [ ]
            ///                          [ ]
            ///                          [ ]
            /// exemple tile    [ ][ ][ ][x][ ][ ][ ]
            /// ///                      [ ]
            ///                          [ ]
            ///                          [ ]                */
            List<SortTuileInfo> allTuilesInfo = new List<SortTuileInfo>();

            for (int cnt = 0; cnt < pe; cnt++)
            {
                // check si la case à droite qu'on veux vérifier ne sort pas des bord
                /////////////// right //////////////
                SortTuileInfo stir = new SortTuileInfo
                {
                    TuilePoint = new Point(spellCaster.map_position.X + 1 + cnt, spellCaster.map_position.Y),
                    IsWalkable = true,
                    IsBlockingView = false
                };
                allTuilesInfo.Add(stir);
                
                ////////////// left
                SortTuileInfo stil = new SortTuileInfo
                {
                    TuilePoint = new Point(spellCaster.map_position.X - 1 - cnt, spellCaster.map_position.Y),
                    IsWalkable = true,
                    IsBlockingView = false
                };
                allTuilesInfo.Add(stil);

                /////////////// up //////////////

                SortTuileInfo stiu = new SortTuileInfo
                {
                    TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y - 1 - cnt),
                    IsWalkable = true,
                    IsBlockingView = false
                };
                allTuilesInfo.Add(stiu);

                /////////////// down //////////////
                SortTuileInfo stid = new SortTuileInfo
                {
                    TuilePoint = new Point(spellCaster.map_position.X, spellCaster.map_position.Y + 1 + cnt),
                    IsWalkable = true,
                    IsBlockingView = false
                };
                allTuilesInfo.Add(stid);
            }

            return allTuilesInfo;
            #endregion
        }
    }
    public class FiltrePlayers
    {
        public Actor actor;
        public List<Point> Waypoint;
        public bool HandToHand;
    }
    public class ReorderPlayersInTeam : IComparer<Actor>
    {
        public int Compare(Actor x, Actor y)
        {
            if (x.initiative == y.initiative)
            {
                //les 2 ont la meme initiative
                // classement en aléatoire
                Random random = new Random();
                int r = random.Next(0, 2);
                if (r == 0)
                    return 1;
                return -1;
            }
            if (x.initiative > y.initiative)
                return -1;
            return 1;
        }
    }
    public class ReorderPlayersByPdv : IComparer<Actor>
    {
        public int Compare(Actor x, Actor y)
        {
            if (x.currentHealth == y.currentHealth)
            {
                Random random = new Random();
                int r = random.Next(0, 2);
                if (r == 0)
                    return 1;
                return -1;
            }
            if (x.currentHealth > y.currentHealth)
                return 1;
            return -1;
        }
    }
    public class ReorderPlayersByPdv2 : IComparer<FiltrePlayers>
    {
        // utilisé lors se qu'une invoc éssai de trouver la cible la plus pertinante, selon ses pm / humain puis invoc si non
        public int Compare(FiltrePlayers x, FiltrePlayers y)
        {
            if (x.actor.currentHealth == y.actor.currentHealth)
            {
                Random random = new Random();
                int r = random.Next(0, 2);
                if (r == 0)
                    return 1;
                return -1;
            }
            if (x.actor.currentHealth > y.actor.currentHealth)
                return 1;
            return -1;
        }
    }
    public class ReorderPlayersByPm : IComparer<FiltrePlayers>
    {
        // utilisé lors se q'ne invoc éssai de trouver la cible la plus pertinante, selon ses pm / humain puis invoc si non
        public int Compare(FiltrePlayers x, FiltrePlayers y)
        {
            if (x.Waypoint.Count == y.Waypoint.Count)
            {
                Random random = new Random();
                int r = random.Next(0, 2);
                if (r == 0)
                    return 1;
                return -1;
            }
            if (x.Waypoint.Count > y.Waypoint.Count)
                return 1;
            return -1;
        }
    }
    public static class ScreenManager
    {
        #region
        public static int WindowWidth = 1008;
        public static int WindowHeight = 684;
        public static int TileWidth = 33;
        public static int TileHeight = 18;
        public static string WindowTitle
        {
            get { return "MMORPG Super Heroes"; }
        }
        #endregion
    }
    public class NearestPoint
    {
        // contiens les pos a proximités des joueurs ainsi qu'un indice de blocage
        public Point point;
        public int index;
        public Actor actor;
    }
}
//0x25