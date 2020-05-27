using System;
using System.Threading;
using Lidgren.Network;
using System.Collections.Generic;
using SERVER.Enums;
using SERVER.Net.Messages.Response;
using SERVER.Net.Messages.Response.Map;

namespace SERVER
{
    class MainClass
    {
        public static NetServer netServer;
        public static INetEncryption algo = new NetXtea("the-morpher");

        public static Int32 Port;
        public static Int32 MaximumConnections;
        public static Int32 ConnectionTimeout;
        public static Int32 MaxTimeAfk;

        public static string db;
        public static string user;
        public static string pass;
        public static string hostdb;

        public static int LvlStart;
        public static int StartPdv;
        public static int AutoUpdatePdv;                // contiens les pdv a ajouter chaque 2 seconds
        public static Int32 chakralvl1, chakralvl2, chakralvl3, chakralvl4, chakralvl5, InitialisationBattleWaitTime;
        public static int TimeToPlayInBattle;
        public static int TimeToSaveDB;                 // temps d'interval pour sauveguarder la BDD

        public static void Main(string[] args)
        {
            // initialisation PetaPoco

            DataBase.DataTables.IniPetaPoco();
            Console.WriteLine("Initialisation PetaPoco [ok]");        // BDD en interne sous forme de liste au lieu de se connecter a chaque fois à la BDD

            CommonCode.ReadConfigFile();

            NetPeerConfiguration config = new NetPeerConfiguration("the-morpher");

            config.Port = Port;
            config.MaximumConnections = MaximumConnections;
            //config.PingInterval = 100;
            //config.ConnectionTimeout = 600;
            //config.PingInterval = 1F;
            config.ConnectionTimeout = ConnectionTimeout;

            netServer = new NetServer(config);
            netServer.Start();

            Console.WriteLine("Initialisatoin network [ok]");

            // Thread pour supprimer les utilisateurs resté bloqués dans la table Connected
            Thread tCleanTableConnected = new Thread(new ThreadStart(CleanTableConnected));
            tCleanTableConnected.Start();

            // Thread pour la supression des utilisateurs dans ClientData.Client qui ont dépassé le time stamp de 15min
            Thread tChkConnUsr = new Thread(new ThreadStart(chkConnUsr));
            tChkConnUsr.Start();

            // thread pour l'augementation des pdv apres chaque 2 seconds
            Thread tUpdatePdv = new Thread(new ThreadStart(UpdatePdv));
            tUpdatePdv.Start();

            // thread pour le lancement du combat apres x temps seulement si un combat est en mode initialisation
            Thread tBattleLauncher = new Thread(new ThreadStart(BattleLauncher));
            tBattleLauncher.Start();

            Thread tCleanePlayersStats = new Thread(new ThreadStart(CleanPlayersStats));
            tCleanePlayersStats.Start();

            // nétoyage de la bdd
            DataBase.DataTables.dataContext.Execute("TRUNCATE connected");
            DataBase.DataTables.dataContext.Execute("TRUNCATE Logerror");
            DataBase.DataTables.dataContext.Execute("TRUNCATE logCounter");
            DataBase.DataTables.dataContext.Execute("delete from MapObj where state='dynamic'");
            DataBase.DataTables.dataContext.Execute("update players set inBattle ='0', inBattleType ='', inBattleID=''");

            Console.WriteLine("Nétoyage de la BDD [ok]");

            Console.WriteLine((DataBase.DataTables.classes as List<mysql.classes>).Count + " classes chargés");
            /*foreach (mysql.classes a in DataBase.DataTables.classes)
                Console.WriteLine("{0} - {1}", a.id, a.classeName);*/

            Thread saveDB = new Thread(new ThreadStart(UpdateDB));
            saveDB.Start();
            Console.WriteLine("Boucle regénération Pdv [ok]");

            Console.WriteLine("Lancement boucle réseau [ok]");
            Console.WriteLine("____________________________________");
            while (!Console.KeyAvailable)
            {
                NetIncomingMessage im;
                while ((im = netServer.ReadMessage()) != null)
                {
                    // handle incoming message
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        //Console.WriteLine(im.ReadString());
                        //break;
                        case NetIncomingMessageType.ErrorMessage:
                        //Console.WriteLine(im.ReadString());
                        //break;
                        case NetIncomingMessageType.WarningMessage:
                        //Console.WriteLine(im.ReadString());
                        //break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                        //Console.WriteLine(im.ReadString());
                        //break;
                        case NetIncomingMessageType.StatusChanged:
                            try
                            {
                                NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                                string reason = im.ReadString();
                                if (status.ToString() == "Connected")
                                    server_NewConnection(im);
                                else if (status.ToString() == "Disconnected")
                                    server_LostConnection(im, reason);
                            }
                            catch
                            {
                                //Console.WriteLine (ex.ToString ());
                            }
                            break;
                        case NetIncomingMessageType.Data:
                            im.Decrypt(algo);
                            Network.GetData(im);
                            break;
                        default:
                            Console.WriteLine("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                            break;
                    }
                }
                Thread.Sleep(200);
            }
            Console.WriteLine("Le programme a sortie de la boucle réseau !!!!!!!!!!!\nRelancer le programme si c'est pas planifié");
            tChkConnUsr.Abort();
            tCleanTableConnected.Abort();
            tUpdatePdv.Abort();
            tBattleLauncher.Abort();
            tCleanePlayersStats.Abort();
        }

        private static void UpdateDB()
        {
            while(true)
            {
                Thread.Sleep(TimeToSaveDB);
                DataBase.DataTables.Update();
            }
        }

        static void server_NewConnection(NetIncomingMessage im)
        {
            Console.WriteLine("New client was connected : " + im.SenderConnection.RemoteUniqueIdentifier);
            im.SenderConnection.Tag = new Actor() { Timestamp = CommonCode.ReturnTimeStamp() };
        }

        static void server_LostConnection(NetIncomingMessage im, string reason)
        {
            Console.WriteLine("client " + im.SenderConnection.RemoteUniqueIdentifier + " was diconnected " + reason);
            Actor pi = im.SenderConnection.Tag as Actor;
            if (pi.Username != string.Empty)
                ((List<mysql.connected>)DataBase.DataTables.connected).RemoveAll(f => f.user == pi.Username);

            // il faut informer tous les autre clients abonnées au map de la deconnexion du client
            // seulement si le client n'est pas dans un combat ou dans un combat FreeChallenge
            if (pi.inBattle == 0)
            {
                List<NetConnection> abonnedPlayers = netServer.Connections.FindAll(p => ((Actor)p.Tag).map == pi.map && ((Actor)p.Tag).Pseudo != pi.Pseudo && pi.inBattle == 0);
                for (int cnt = 0; cnt < abonnedPlayers.Count; cnt++)
                {
                    ActorDisconnectedResponseMessage playerDisconnectedResponseMessage = new ActorDisconnectedResponseMessage();
                    playerDisconnectedResponseMessage.Initialize(new[] { pi.Pseudo }, abonnedPlayers[cnt]);
                    playerDisconnectedResponseMessage.Serialize();
                    playerDisconnectedResponseMessage.Send();
                }
                abonnedPlayers.Clear();
            }
            else
            {
                List<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll(p => ((Actor)p.Tag).map == pi.map && ((Actor)p.Tag).Pseudo != pi.Pseudo && ((Actor)p.Tag).idBattle == pi.idBattle);
                foreach (NetConnection t in abonnedPlayers)
                {
                    ActorDisconnectedResponseMessage playerDisconnectedResponseMessage = new ActorDisconnectedResponseMessage();
                    playerDisconnectedResponseMessage.Initialize(new[] { pi.Pseudo }, t);
                    playerDisconnectedResponseMessage.Serialize();
                    playerDisconnectedResponseMessage.Send();
                }
                abonnedPlayers.Clear();
            }

            if (pi.inBattle == 1 && Battle.Battles.Find(f => f.IdBattle == pi.idBattle).BattleType == BattleType.Type.FreeChallenge)
            {
                // le joueur été en combat, il faut annuler le combat s'il ya seulement 2 joueurs
                // check s'il le combat est du type FreeChallenge
                Battle battle = Battle.Battles.Find(f => f.IdBattle == pi.idBattle);
                if (battle != null && battle.BattleType == BattleType.Type.FreeChallenge)
                {
                    // effacement de notre joueur de la combat afin de verifier apres
                    battle.SideA.Remove(battle.SideA.Find(f => f.Pseudo == pi.Pseudo));
                    battle.SideB.Remove(battle.SideB.Find(f => f.Pseudo == pi.Pseudo));
                    // il faut informer les abonnés du map du combat et qui ne sont pas en combat que le combat en cours a été annulé pour supprimer l'objet qui represente le combat
                    // informer les joueurs abonnées au map de combat et qui ne sont pas en combat de la supression des 2 objets
                    List<NetConnection> nc = netServer.Connections.FindAll(f => ((Actor)f.Tag).inBattle == 0 && ((Actor)f.Tag).map == battle.Map);
                    foreach (NetConnection t in nc)
                    {
                        MapObjetRemovedResponseMessage mapObjetRemovedResponseMessage = new MapObjetRemovedResponseMessage();
                        object[] o = new object[2];
                        o[0] = Enums.BattleType.Type.FreeChallenge;
                        o[1] = battle.IdBattle;

                        mapObjetRemovedResponseMessage.Initialize(o, t);
                        mapObjetRemovedResponseMessage.Serialize();
                        mapObjetRemovedResponseMessage.Send();
                    }
                        
                }
                if (CommonCode.IsClosedBattle(battle, true))
                {
                    battle.State = battleState.state.closed;
                    Console.WriteLine("battle closed");
                    foreach (Actor t in battle.AllPlayersByOrder)
                    {
                        t.inBattle = 0;
                        ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == t.Pseudo).inBattle = 0;
                    }
                }
            }

            if (pi.PlayerChallengeYou != "")
            {
                //check si le joueur est connecté
                NetConnection nt = netServer.Connections.Find(f => ((Actor)f.Tag).Pseudo == pi.PlayerChallengeYou);
                if (nt != null)
                {
                    ((Actor)nt.Tag).YouChallengePlayer = "";
                    ((Actor)nt.Tag).PlayerChallengeYou = "";
                }
            }
            else if (pi.YouChallengePlayer != "")
            {
                //check si le joueur est connecté
                NetConnection nt = netServer.Connections.Find(f => ((Actor)f.Tag).Pseudo == pi.PlayerChallengeYou);
                if (nt != null)
                {
                    ((Actor)nt.Tag).YouChallengePlayer = "";
                    ((Actor)nt.Tag).PlayerChallengeYou = "";
                }
            }
        }

        public static void chkConnUsr()
        {
            #region
            Console.WriteLine("Thread counter timeout lancé [ok]");
            while (true)
            {
                try
                {
                    List<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll(p => p.Tag != null && CommonCode.ReturnTimeStamp() - (p.Tag as Actor).Timestamp > MaxTimeAfk);
                    for (int cnt = 0; cnt < abonnedPlayers.Count; cnt++)
                    {
                        Console.WriteLine("\n" + (abonnedPlayers[cnt].Tag as Actor).Username + " was kicked due to time out");
                        abonnedPlayers[cnt].Disconnect(Enums.DisconnectReason.disconnectReason.TIME_OUT.ToString());
                    }
                    abonnedPlayers.Clear();
                    abonnedPlayers = null;
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Thread chkConnUsr time out est arreté " + ex.ToString());
                }
            }
            #endregion
        }

        public static void CleanPlayersStats()
        {
            Console.WriteLine("Thread reinitialisation stats players lancé [ok]");
            // netoie la bdd des variables qui sont resté affecté alors qu'il deverons se reinitialiser quand le joueur deco pour eviter un blocage du joueur
            while (true)
            {
                try
                {
                    // remise a zero des champ quand l'utilisateur est deconnecte lors d'un combat FreeChallenge
                    foreach (mysql.players cleanStats in (DataBase.DataTables.players as List<mysql.players>).FindAll(f => f.inBattle == 1))
                        if (!Battle.Battles.Exists(f => f.IdBattle == cleanStats.inBattleID))
                        {
                            (DataBase.DataTables.players as List<mysql.players>).Find(f => f.pseudo == cleanStats.pseudo).inBattle = 0;
                            (DataBase.DataTables.players as List<mysql.players>).Find(f => f.pseudo == cleanStats.pseudo).inBattleType = "";
                            (DataBase.DataTables.players as List<mysql.players>).Find(f => f.pseudo == cleanStats.pseudo).inBattleID = 0;
                        }

                    // le contraire du code avant, cad, chercher les utilisateur qui sont en combat dans leurs tag mais aucun combat ne correspond a leurs battleId
                    List<NetConnection> nt = MainClass.netServer.Connections.FindAll(f => f.Tag != null && (f.Tag as Actor).inBattle == 1);
                    if (nt != null)
                    {
                        for (int cnt = nt.Count; cnt > 0; cnt--)
                        {
                            if (!Battle.Battles.Exists(f => (f.IdBattle == (nt[cnt - 1].Tag as Actor).idBattle)))
                            {
                                (nt[cnt - 1].Tag as Actor).inBattle = 0;
                                (nt[cnt - 1].Tag as Actor).idBattle = -1;
                                (nt[cnt - 1].Tag as Actor).teamSide = Enums.Team.Side.None;
                            }
                        }
                    }

                    nt.Clear();
                    nt = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Thread reinitialisation stats players arreté " + ex.ToString());
                }
                Thread.Sleep(500);
            }
        }

        public static void CleanTableConnected()
        {
            Console.WriteLine("Thread pour nétoyage la bdd des utilisateurs deconnectés lancé [ok]");
            while (true)
            {
                // recherche des utilisateurs qui ont déco et qui sont restés connectés sur la table connect
                // sans etre intersepté par la methode Server_LostConnection, comme lors de l'arret du serveur
                // les utilisateurs reste enregistré dans la table connect
                (DataBase.DataTables.connected as List<mysql.connected>).RemoveAll(f => f.timestamp < (CommonCode.ReturnTimeStamp() - MaxTimeAfk));
                Thread.Sleep(1000);
            }
        }

        public static void UpdatePdv()
        {
            Console.WriteLine("Thread UpdatePDV lancé [ok]");
            while (true)
            {
                try
                {
                    foreach (mysql.players player in DataBase.DataTables.players)
                    {
                        // augementation des pdv "initiales",si le joueurs a les pdv inférieurs
                        if (player.currentHealth < player.maxHEalth)
                        {
                            player.currentHealth += AutoUpdatePdv;
                            // regularisation si il depasse le max des pdv
                            if (player.currentHealth > player.maxHEalth)
                                player.currentHealth = player.maxHEalth;

                            (MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == player.pseudo).Tag as Actor).currentHealth = player.currentHealth;
                            // informer le client que ses pdv ont été changés
                            //CommonCode.SendMessage("cmd•PlayerStats•" + player.TotalPdv + "#" + player.CurrentPdv, MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == player.pseudo), true);
                            //Console.WriteLine("<--cmd•PlayerStats to " + (MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == player.pseudo).Tag as Actor).Pseudo);

                            // tester ce code si ça marche sur le client
                            UpdateStatsResponseMessage updateSpecificStatsResponseMessage = new UpdateStatsResponseMessage();
                            object[] o = new object[1];
                            
                            o[0] = "maxHealth#" + player.maxHEalth + "|" + "currentHealth#" + player.currentHealth;
                            updateSpecificStatsResponseMessage.Initialize(o, MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == player.pseudo));
                            updateSpecificStatsResponseMessage.Serialize();
                            updateSpecificStatsResponseMessage.Send();

                        }
                    }

                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Thread UpdatePDV arreté " + ex.ToString());
                }
            }
        }
        public static void BattleLauncher()
        {
            // lanceur de combat
            Console.WriteLine("Thread d'activation des combats [ok]");
            // lancement des combat et changement des status si le timestamp depasse MainClass.InitialisationBattleWaitTime
            while (true)
            {
                try
                {
                    for (int cnt = Battle.Battles.Count; cnt > 0; cnt--)
                    {
                        Battle _battle = Battle.Battles[cnt - 1];
                        if (_battle.State == Enums.battleState.state.initialisation && CommonCode.ReturnTimeStamp() - _battle.Timestamp > MainClass.InitialisationBattleWaitTime)
                        {
                            #region état initialisation
                            // supprimer les 2 images qui represente le combat dans le map
                            (DataBase.DataTables.mapobj as List<mysql.mapobj>).RemoveAll(f => f.idBattle == _battle.IdBattle);

                            // informer les joueurs abonnées au map de combat et qui ne sont pas en combat de la supression des 2 objets
                            List<NetConnection> nc = MainClass.netServer.Connections.FindAll(f => (f.Tag as Actor).inBattle == 0 && (f.Tag as Actor).map == _battle.Map);
                            for (int cnt2 = 0; cnt2 < nc.Count; cnt2++)
                            {
                                MapObjetRemovedResponseMessage mapObjetRemovedResponseMessage = new MapObjetRemovedResponseMessage();
                                object[] o = new object[2];
                                o[0] = _battle.BattleType;
                                o[1] = _battle.IdBattle;

                                mapObjetRemovedResponseMessage.Initialize(o, nc[cnt2]);
                                mapObjetRemovedResponseMessage.Serialize();
                                mapObjetRemovedResponseMessage.Send();
                            }
                            // etape final, informer les joueurs du combat pour le commencement
                            // envoyer tous les infos des states au joueurs abonnés au combat
                            string data = "";
                            for (int cnt2 = 0; cnt2 < _battle.SideA.Count; cnt2++)
                            {
                                string sorts = "";
                                for (int cnt3 = 0; cnt3 < _battle.SideA[cnt2].sorts.Count; cnt3++)
                                    sorts += _battle.SideA[cnt2].sorts[cnt3].SpellId + ":" + _battle.SideA[cnt2].sorts[cnt3].SpellPlace + ":" + _battle.SideA[cnt2].sorts[cnt3].Level + ":" + _battle.SideA[cnt2].sorts[cnt3].SpellColor + "/";
                                if (sorts != "")
                                    sorts = sorts.Substring(0, sorts.Length - 1);

                                data += _battle.SideA[cnt2].Pseudo + "#" + _battle.SideA[cnt2].classeName + "#" + _battle.SideA[cnt2].hiddenVillage + "#" + _battle.SideA[cnt2].maskColorString + "#" + _battle.SideA[cnt2].level + "#" + _battle.SideA[cnt2].officialRang + "#" + _battle.SideA[cnt2].currentHealth + "#" + _battle.SideA[cnt2].maxHealth + "#" + _battle.SideA[cnt2].doton + "#" + _battle.SideA[cnt2].katon + "#" + _battle.SideA[cnt2].futon + "#" + _battle.SideA[cnt2].raiton + "#" + _battle.SideA[cnt2].suiton + "#" + _battle.SideA[cnt2].usingDoton + "#" + _battle.SideA[cnt2].usingKaton + "#" + _battle.SideA[cnt2].usingFuton + "#" + _battle.SideA[cnt2].usingRaiton + "#" + _battle.SideA[cnt2].usingSuiton + "#" + _battle.SideA[cnt2].equipedDoton + "#" + _battle.SideA[cnt2].equipedKaton + "#" + _battle.SideA[cnt2].equipedFuton + "#" + _battle.SideA[cnt2].equipedRaiton + "#" + _battle.SideA[cnt2].equipedSuiton + "#" + _battle.SideA[cnt2].originalPc + "#" + _battle.SideA[cnt2].originalPm + "#" + _battle.SideA[cnt2].pe + "#" + _battle.SideA[cnt2].cd + "#" + _battle.SideA[cnt2].summons + "#" + _battle.SideA[cnt2].initiative + "#" + _battle.SideA[cnt2].dodgePC + "#" + _battle.SideA[cnt2].dodgePM + "#" + _battle.SideA[cnt2].dodgePE + "#" + _battle.SideA[cnt2].dodgeCD + "#" + _battle.SideA[cnt2].removePC + "#" + _battle.SideA[cnt2].removePM + "#" + _battle.SideA[cnt2].removePE + "#" + _battle.SideA[cnt2].removeCD + "#" + _battle.SideA[cnt2].escape + "#" + _battle.SideA[cnt2].blocage + "#" + _battle.SideA[cnt2].power + "#" + _battle.SideA[cnt2].equipedPower + "#" + sorts + "|";
                            }
                            if (data != "")
                                data = data.Substring(0, data.Length - 1) + "•";

                            for (int cnt2 = 0; cnt2 < _battle.SideB.Count; cnt2++)
                            {
                                string sorts = "";
                                for (int cnt3 = 0; cnt3 < _battle.SideB[cnt2].sorts.Count; cnt3++)
                                    sorts += _battle.SideB[cnt2].sorts[cnt3].SpellId + ":" + _battle.SideB[cnt2].sorts[cnt3].SpellPlace + ":" + _battle.SideB[cnt2].sorts[cnt3].Level + ":" + _battle.SideB[cnt2].sorts[cnt3].SpellColor + "/";
                                if (sorts != "")
                                    sorts = sorts.Substring(0, sorts.Length - 1);

                                data += _battle.SideB[cnt2].Pseudo + "#" + _battle.SideB[cnt2].classeName + "#" + _battle.SideB[cnt2].hiddenVillage + "#" + _battle.SideB[cnt2].maskColorString + "#" + _battle.SideB[cnt2].level + "#" + _battle.SideB[cnt2].officialRang + "#" + _battle.SideB[cnt2].currentHealth + "#" + _battle.SideB[cnt2].maxHealth + "#" + _battle.SideB[cnt2].doton + "#" + _battle.SideB[cnt2].katon + "#" + _battle.SideB[cnt2].futon + "#" + _battle.SideB[cnt2].raiton + "#" + _battle.SideB[cnt2].suiton + "#" + _battle.SideB[cnt2].usingDoton + "#" + _battle.SideB[cnt2].usingKaton + "#" + _battle.SideB[cnt2].usingFuton + "#" + _battle.SideB[cnt2].usingRaiton + "#" + _battle.SideB[cnt2].usingSuiton + "#" + _battle.SideB[cnt2].equipedDoton + "#" + _battle.SideB[cnt2].equipedKaton + "#" + _battle.SideB[cnt2].equipedFuton + "#" + _battle.SideB[cnt2].equipedRaiton + "#" + _battle.SideB[cnt2].equipedSuiton + "#" + _battle.SideB[cnt2].originalPc + "#" + _battle.SideB[cnt2].originalPm + "#" + _battle.SideB[cnt2].pe + "#" + _battle.SideB[cnt2].cd + "#" + _battle.SideB[cnt2].summons + "#" + _battle.SideB[cnt2].initiative + "#" + _battle.SideB[cnt2].dodgePC + "#" + _battle.SideB[cnt2].dodgePM + "#" + _battle.SideB[cnt2].dodgePE + "#" + _battle.SideB[cnt2].dodgeCD + "#" + _battle.SideB[cnt2].removePC + "#" + _battle.SideB[cnt2].removePM + "#" + _battle.SideB[cnt2].removePE + "#" + _battle.SideB[cnt2].removeCD + "#" + _battle.SideB[cnt2].escape + "#" + _battle.SideB[cnt2].blocage + "#" + _battle.SideB[cnt2].power + "#" + _battle.SideB[cnt2].equipedPower + "#" + sorts + "|";
                            }
                            if (data != "")
                                data = data.Substring(0, data.Length - 1);

                            // verifier si les 2 leaders du team1 et team2 ont la meme initiative, si oui,choisir un aléatoirement
                            // reorganisation des joueurs selons leurs initiative
                            ReorderPlayersInTeam ropit = new ReorderPlayersInTeam();
                            _battle.SideA.Sort(ropit);
                            _battle.SideB.Sort(ropit);

                            // contien le nom du joueur qui a le plus d'initiative, quand et seulement quand les 2 adversaires ont la meme initiative
                            string ranSelectedPlayer = "";
                            if (_battle.SideA[0].initiative == _battle.SideB[0].initiative)
                            {
                                // selection aléatoire de l'un des 2 team
                                Random rand = new Random();
                                int r = rand.Next(0, 2);
                                if (r == 0)
                                    ranSelectedPlayer = _battle.SideA[0].Pseudo;
                                else
                                    ranSelectedPlayer = _battle.SideB[0].Pseudo;
                            }
                            else
                            {
                                if (_battle.SideA[0].initiative > _battle.SideB[0].initiative)
                                    ranSelectedPlayer = _battle.SideA[0].Pseudo;
                                else
                                    ranSelectedPlayer = _battle.SideB[0].Pseudo;
                            }
                            // reorganisation et alimentaion de la liste qui contiens tous les joueurs
                            // reorganization de tous les joueurs selon l'initiative et basculation entre les team
                            int team1Cnt = 0;
                            int team2Cnt = 0;

                            if (_battle.SideA[0].initiative > _battle.SideB[0].initiative)
                            {
                                team1Cnt = 1;
                                _battle.AllPlayersByOrder.Add(_battle.SideA[0]);
                            }
                            else if (_battle.SideA[0].initiative < _battle.SideB[0].initiative)
                            {
                                team2Cnt = 1;
                                _battle.AllPlayersByOrder.Add(_battle.SideB[0]);
                            }
                            else
                            {
                                // les 2 leaders ont la meme initiative
                                Actor piib = _battle.SideA.Find(f => f.Pseudo == ranSelectedPlayer);
                                if (piib != null)
                                {
                                    team1Cnt = 1;
                                    _battle.AllPlayersByOrder.Add(_battle.SideA[0]);
                                }
                                else
                                {
                                    team2Cnt = 1;
                                    _battle.AllPlayersByOrder.Add(_battle.SideB[0]);
                                }
                            }

                            // insertion des joueurs dans la liste Battle.AllPlayersByOrder
                            while (team1Cnt < _battle.SideA.Count || team2Cnt < _battle.SideB.Count)
                            {
                                int last = _battle.AllPlayersByOrder.Count - 1;
                                if (_battle.AllPlayersByOrder[last].teamSide == Enums.Team.Side.A)
                                {
                                    if (_battle.SideB.Count >= team2Cnt)
                                    {
                                        _battle.AllPlayersByOrder.Add(_battle.SideB[team2Cnt]);
                                        team2Cnt++;
                                    }
                                }
                                else if (_battle.AllPlayersByOrder[last].teamSide == Enums.Team.Side.A)
                                {
                                    if (_battle.SideA.Count >= team1Cnt)
                                    {
                                        _battle.AllPlayersByOrder.Add(_battle.SideA[team1Cnt]);
                                        team1Cnt++;
                                    }
                                }
                            }
                            //////////////////////////////////////////////////////

                            // envoie les données des joueurs a tous les joueurs dans le combat
                            for (int cnt2 = 0; cnt2 < _battle.SideA.Count; cnt2++)
                            {
                                NetConnection nc2 = MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == _battle.SideA[cnt2].Pseudo);
                                if (nc2 != null)
                                {
                                    CommonCode.SendMessage("cmd•BattleStarted•" + data + "•" + ranSelectedPlayer + "•" + MainClass.TimeToPlayInBattle, nc2, true);
                                    Console.WriteLine("<--cmd•BattleStarted•" + data + "•" + ranSelectedPlayer + " to " + (nc2.Tag as Actor).Pseudo);
                                }
                            }
                            for (int cnt2 = 0; cnt2 < _battle.SideB.Count; cnt2++)
                            {
                                NetConnection nc2 = MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == _battle.SideB[cnt2].Pseudo);
                                if (nc2 != null)
                                {
                                    CommonCode.SendMessage("cmd•BattleStarted•" + data + "•" + ranSelectedPlayer + "•" + MainClass.TimeToPlayInBattle, nc2, true);
                                    Console.WriteLine("<--cmd•BattleStarted•" + data + "•" + ranSelectedPlayer + "•" + MainClass.TimeToPlayInBattle + " to " + (nc2.Tag as Actor).Pseudo);
                                }
                            }

                            // mise a jour du timestamp du joueur qui a la main
                            _battle.TimeLeftToPlay = CommonCode.ReturnTimeStamp();
                            // changement de state =Started, pour eviter tous changement de position ou accée au combat
                            _battle.State = Enums.battleState.state.started;
                            CommonCode.FinishTurn(_battle, false);
                            #endregion
                        }
                        else if (_battle.State == Enums.battleState.state.started)
                        {
                            #region started
                            // code pour passer la main a l'autre joueur
                            if (Battle.Battles.Count > 0 && CommonCode.ReturnTimeStamp() - _battle.TimeLeftToPlay > TimeToPlayInBattle)
                            {
                                CommonCode.FinishTurn(_battle, true);
                            }

                            if (_battle.SideA.FindAll(f => f.species == Species.Name.Human).Count + _battle.SideB.FindAll(f => f.species == Species.Name.Human).Count < 0)
                            {
                                // le combat est términé
                                CommonCode.IsClosedBattle(_battle, true);
                            }
                            #endregion
                        }
                        else if (_battle.State == Enums.battleState.state.closed)
                        {
                            #region closed
                            // le combat a été cloturé suite a la deconnexion de quelqu'un, ou un joueur est mort apres un lancement de sort
                            // supprimer les 2 images qui represente le combat dans le map
                            (DataBase.DataTables.mapobj as List<mysql.mapobj>).RemoveAll(f => f.idBattle == _battle.IdBattle);

                            // mise du statut inBattle = 0, idBattle = -1 pour les joueurs
                            foreach (mysql.players player in (DataBase.DataTables.players as List<mysql.players>).FindAll(f => f.inBattleID == _battle.IdBattle))
                            {
                                player.inBattle = 0;
                                player.inBattleID = 0;
                                player.inBattleType = "";
                            }

                            // mise du statut inBattle = 0;, idBattle = -1 dans les sessions PlayerInfo de chaque joueur
                            List<Actor> bl = Battle.Battles[cnt - 1].AllPlayersByOrder.FindAll(f => f.species == Species.Name.Human);
                            bl.InsertRange(0, Battle.Battles[cnt - 1].DeadPlayers.FindAll(f => f.species == Species.Name.Human));

                            foreach (Actor t in bl)
                            {
                                NetConnection ncCnt2 = MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == t.Pseudo);

                                if (ncCnt2 != null)
                                {
                                    Actor piTag = ncCnt2.Tag as Actor;
                                    piTag.inBattle = 0;
                                    piTag.idBattle = -1;
                                    piTag.teamSide = Team.Side.None;
                                    piTag.isAlive = true;
                                    piTag.animatedAction = AnimatedActions.Name.idle;
                                    piTag.PlayerChallengeYou = "";
                                    piTag.YouChallengePlayer = "";
                                    piTag.BuffsList.Clear();
                                }
                            }

                            // Informer les utilisateurs abonnées au map et qui ne sont pas en combat de l'affichage des joueurs qui étés en combat
                            foreach (Actor pi in bl)
                            {
                                // un controle inutile mais par précaution

                                var pi1 = pi;
                                NetConnection nt = netServer.Connections.Find(f => ((Actor)f.Tag).Pseudo == pi1.Pseudo);
                                if (nt != null)
                                {
                                    CommonCode.RefreshStats(nt);

                                    // pi2 = la vrais instance qui pointe vers le joueur en itérations, pi pointe vers le clone qui as les donnée playerInfo qui correspand au combat, comme la position dans le combat, ses stats differente apres un boost ou retrait..
                                    // si non on envois au abonnées du map la mauvaises position des joueurs avec leurs position lorsqu'il été en combat et non la position réel
                                    Actor pi2 = nt.Tag as Actor;

                                    // envoie d'une requette de réaparition des joueurs qui étés en combats
                                    List<NetConnection> abonnedMap = MainClass.netServer.Connections.FindAll(f => f.Tag != null && (f.Tag as Actor).Pseudo != pi2.Pseudo && (f.Tag as Actor).map == pi2.map);
                                    foreach (NetConnection t in abonnedMap)
                                    {
                                        #region
                                        // pseudo#classe#pvp:spirit:spiritLvl#village#MaskColors#map_position#orientation#level#action#waypoint   - separateur entre plusieurs joueurs
                                        string data = pi2.Pseudo + "#" + pi2.classeName + "#";
                                        if (pi2.Pvp == false)
                                            data += "0:null:null#";
                                        else
                                            data += "1:" + pi2.spirit + ":" + pi2.spiritLvl.ToString() + "#";
                                        data += pi2.hiddenVillage + "#" + pi2.maskColorString + "#" + pi2.map_position.X + "/" + pi2.map_position.Y + "#" + pi2.directionLook.ToString() + "#" + pi2.level.ToString() + "##";

                                        SpawnActorResponseMessage spawnActorResponseMessage = new SpawnActorResponseMessage();
                                        spawnActorResponseMessage.Initialize(new[] { data }, t);
                                        spawnActorResponseMessage.Serialize();
                                        spawnActorResponseMessage.Send();
                                        #endregion
                                    }
                                }
                            }

                            // netoyage des listes
                            Battle.Battles[cnt - 1].SideA.Clear();
                            Battle.Battles[cnt - 1].SideB.Clear();
                            Battle.Battles[cnt - 1].AllPlayersByOrder.Clear();
                            // supression du combat de la liste
                            Battle.Battles.RemoveAt(cnt - 1);
                            Console.WriteLine("battle finished");
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Thread.Sleep(1000);
            }
        }
    }
}