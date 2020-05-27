using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using SERVER.Enums;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class GrabingPlayersInformationRequestMessage : IRequestMessage
    {   
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        Actor _actor;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            Nc = nc;
            CommandStrings = commandStrings;
            _actor = (Actor)Nc.Tag;
        }

        public bool Check()
        {
            if (_actor.Username == "" || _actor.Pseudo != "" || _actor.map != "")
            {
                Security.User_banne("Actor without generic stats.GrabingPlayersInformationRequestMessage", Nc);
                return false;
            }
            return true;
        }

        public void Apply()
        {
            string tmp = ((List<mysql.players>) DataBase.DataTables.players).FindAll(f => f.user == _actor.Username).Aggregate(string.Empty, (current, p) => current + (p.pseudo + "#" + p.level + "#" + p.spirit + "#" + p.classe + "#" + p.pvpEnabled + "#" + p.spiritLevel + "#" + p.hiddenVillage + "#" + p.maskColorString + "#null#null|"));

            // verification si le joueur a créée des personnages
            if (((List<mysql.players>)DataBase.DataTables.players).Exists(f => f.user == _actor.Username))
            {
                // on check si l'utilisateur est en combat, si oui, verifier si le combat existe, si oui, verifier s'il reste plus d'1 joueur non invoc dans les 2 teams
                // un probleme arrive rarement qui fait qu'un client se deconnecte d'un combat, et apres il co et il bug sur l'état qu'il été en combat, alors qu'il s'est deconnecté mais le serveur garde la session de combat, donc il cloture pas
                if (_actor.inBattle == 1)
                {
                    if (!Battle.Battles.Exists(f => f.IdBattle == _actor.idBattle))
                    {
                        _actor.inBattle = 0;
                        // mise a jours sur la bdd
                        // mise du statut inBattle = 0, idBattle = -1 pour les joueurs
                        mysql.players p = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _actor.Pseudo);
                        p.inBattle = 0;
                        p.inBattleID = 0;
                        p.inBattleType = "";
                    }
                    else
                    {
                        Battle battle = Battle.Battles.Find(f => f.IdBattle == _actor.idBattle);
                        if (battle.SideA.FindAll(f => f.species == Species.Name.Human).Count + battle.SideB.FindAll(f => f.species == Species.Name.Human).Count < 2)
                        {
                            // le combat dois etre cloturé puisqu'il reste qu'un seul joueur dans l'une des 2 team
                            if (CommonCode.IsClosedBattle(battle, false))
                            {
                                // il faut cloturer le combat
                                battle.State = battleState.state.closed;
                                //Console.WriteLine ("1client qui été en combat alors qu'il viens just de co peux etre, peux etre que ca va faire beuguer le client avec les cmd de cloture");
                                _actor.inBattle = 0;
                            }
                        }
                    }
                }

                
                // verification si le joueur été en combat avec un personnage
                mysql.players actorAlreadyConnectedInBattle = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.inBattle == 1 && f.user == _actor.Username);

                // check si un des joueurs est en combat

                // verifier si il faut utiliser la valeur "actor.inBattle == 1" ou bien "actorAlreadyConnectedInBattle != null"
                // un check si le jouer est en combat est deja fait en haut, mais celui d'en haut est pour réctifier un problème qui arrive
                // du coup on répare ce bug et dans cette 2eme condition on revérifie, il faut peux etre penser a réunir cette 2éme partie dans la 1ere "celle d'en haut"
                // au lieu de revérifier apres avoir déjà fait le control
                //if (actorAlreadyConnectedInBattle != null)
                if (_actor.inBattle == 1)
                {
                    #region ///////// DECO RECO pas encore testé
                    // l'un des joueur est en combat, recuperation du map 
                    Console.WriteLine("player " + actorAlreadyConnectedInBattle.pseudo + " inBattle-map: " + actorAlreadyConnectedInBattle.map);
                    _actor.Pseudo = actorAlreadyConnectedInBattle.pseudo;
                    CommonCode.RefreshStats(Nc);
                    ////////////////////////////////
                    string spells = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == actorAlreadyConnectedInBattle.pseudo).sorts;
                    // recuperer le totalXp du niveau en cours
                    int maxXp;

                    maxXp = ((List<mysql.xplevel>)DataBase.DataTables.xplevel).Count == _actor.level ? ((List<mysql.xplevel>)DataBase.DataTables.xplevel)[((List<mysql.xplevel>)DataBase.DataTables.xplevel).Count - 1].xp : ((List<mysql.xplevel>)DataBase.DataTables.xplevel).Find(f => f.level == (_actor.level + 1)).xp;

                    // convertir la list de quete en string, ce code se trouve sur 2 endroit, quand le joueur selectionne un player et quand le joueur été déja dans un combat et que apres un co qui précédé une deco, le player se selectionne tous seul
                    string quete = "";
                    foreach (Actor.QuestInformation t in _actor.Quests)
                        quete += t.QuestName + ":" + t.MaxSteps + ":" + t.CurrentStep + ":" + t.Submited + "/";

                    if (quete != "")
                        quete = quete.Substring(0, quete.Length - 1);

                    // si une modification sur cette cmd a été faite, il faut vérifier tous les cmd avec cmd•SelectedPlayer et aussi checkandupdatestates
                    string buffer = _actor.Pseudo + "#" + _actor.classeName + "#" + _actor.spirit + "#" +
                        _actor.spiritLvl.ToString() + "#" + _actor.Pvp.ToString() + "#" + _actor.hiddenVillage + "#" + _actor.maskColorString + "#" +
                        _actor.directionLook.ToString() + "#" + _actor.level.ToString() + "#" + _actor.map + "#" + _actor.officialRang.ToString() +
                        "#" + _actor.currentHealth.ToString() + "#" + _actor.maxHealth.ToString() + "#" + _actor.xp.ToString() +
                        "#" + maxXp + "#" + _actor.doton.ToString() + "#" + _actor.katon.ToString() + "#" +
                        _actor.futon.ToString() + "#" + _actor.raiton.ToString() + "#" + _actor.suiton.ToString() + "#" +
                        MainClass.chakralvl1 + "#" + MainClass.chakralvl2 + "#" + MainClass.chakralvl3 + "#" +
                        MainClass.chakralvl4 + "#" + MainClass.chakralvl5 + "#" + _actor.usingDoton.ToString() + "#" +
                        _actor.usingKaton.ToString() + "#" + _actor.usingFuton.ToString() + "#" + _actor.usingRaiton.ToString() +
                        "#" + _actor.usingSuiton.ToString() + "#" + _actor.equipedDoton.ToString() + "#" +
                        _actor.equipedKaton.ToString() + "#" + _actor.equipedFuton.ToString() + "#" +
                        _actor.equipedRaiton.ToString() + "#" + _actor.equipedSuiton.ToString() + "#" +
                        _actor.originalPc.ToString() + "#" + _actor.originalPm.ToString() + "#" + _actor.pe.ToString() + "#" +
                        _actor.cd.ToString() + "#" + _actor.summons.ToString() + "#" + _actor.initiative.ToString() + "#" +
                        _actor.job1 + "#" + _actor.job2 + "#" + _actor.specialty1 + "#" +
                        _actor.specialty2 + "#" + _actor.maxWeight.ToString() + "#" + _actor.currentWeight.ToString() +
                        "#" + _actor.ryo.ToString() + "#" + _actor.resiDotonPercent.ToString() + "#" +
                        _actor.resiKatonPercent.ToString() + "#" + _actor.resiFutonPercent.ToString() + "#" +
                        _actor.resiRaitonPercent.ToString() + "#" + _actor.resiSuitonPercent.ToString() + "#" +
                        _actor.dodgePC.ToString() + "#" + _actor.dodgePM.ToString() + "#" + _actor.dodgePE.ToString() + "#" +
                        _actor.dodgeCD.ToString() + "#" + _actor.removePC.ToString() + "#" + _actor.removePM.ToString() + "#" +
                        _actor.removePE.ToString() + "#" + _actor.removeCD.ToString() + "#" + _actor.escape.ToString() + "#" +
                        _actor.blocage.ToString() + "#" + spells + "#" + _actor.resiDotonFix + "#" + _actor.resiKatonFix + "#" +
                        _actor.resiFutonFix + "#" + _actor.resiRaitonFix + "#" + _actor.resiSuitonFix + "#" + _actor.resiFix + "#" +
                        _actor.domDotonFix + "#" + _actor.domKatonFix + "#" + _actor.domFutonFix + "#" + _actor.domRaitonFix + "#" +
                        _actor.domSuitonFix + "#" + _actor.domFix + "#" + _actor.power + "#" + _actor.equipedPower + "•" + quete +
                        "•inBattle•" + _actor.spellPointLeft;

                    AutoSelectActorInBattleResponseMessage selectedActorGrantedResponseMessage = new AutoSelectActorInBattleResponseMessage();
                    selectedActorGrantedResponseMessage.Initialize(new[] { buffer}, Nc);
                    selectedActorGrantedResponseMessage.Serialize();
                    selectedActorGrantedResponseMessage.Send();

                    mysql.connected newConnection = ((List<mysql.connected>)DataBase.DataTables.connected).Find(f => f.user == _actor.Username);
                    newConnection.pseudo = _actor.Pseudo;
                    newConnection.timestamp = CommonCode.ReturnTimeStamp();
                    newConnection.map = _actor.map;
                    newConnection.map_position = _actor.map_position.X + "/" + _actor.map_position.Y;

                    // mise en combat du joueur
                    _actor.inBattle = 1;
                    Battle battle = Battle.Battles.Find(f => f.IdBattle == _actor.idBattle);
                    _actor.teamSide = battle.SideA.Exists(f => f.Pseudo == _actor.Pseudo) ? Team.Side.A : Team.Side.B;

                    // envoie d'une cmd au client qui contiens tous les infos du combat

                    // collecte des données des 2 joueurs
                    /*
                    // envoie au client la confirmation du challenge
                    commun.SendMessage("cmd•challengeBegan•" + playersData + "•" + battleStartPositions.Map(pi.map) + "•" + MainClass.InitialisationBattleWaitTime + "•" + _battle.BattleType + "•" + team1 + "•" + team2 + "•" + _battle.State + "•" + battleStartPositions.Start().Split('|')[0] + "•" + battleStartPositions.Start().Split('|')[1], im, true);*/

                    // modification des position des joueurs selon les position valide du map aléatoirement
                    /*string[] team1ValidePos = battleStartPositions.Map(_battle.Map, _battle.BattleType).Split('|')[0].Split('#');
                    string[] team2ValidePos = battleStartPositions.Map(_battle.Map, _battle.BattleType).Split('|')[1].Split('#');

                    _battle.team1ValidePos = battleStartPositions.Map(_battle.Map, _battle.BattleType).Split('|')[0];
                    _battle.team2ValidePos = battleStartPositions.Map(_battle.Map, _battle.BattleType).Split('|')[1];

                    string playersData = "";

                    for (int cnt = 0; cnt < _battle.Team1.Count; cnt++)
                    {
                        PlayerInfo pit1 = _battle.Team1[cnt];
                        playersData += pit1.Pseudo + "#" + pit1.ClasseName + "#" + pit1.Level + "#" + pit1.village + "#" + pit1.MaskColors + "#" + pit1.totalPdv + "#" + pit1.CurrentPdv + "#" + pit1.rang + "#" + pit1.Initiative + "#" + pit1.doton + "#" + pit1.katon + "#" + pit1.futon + "#" + pit1.raiton + "#" + pit1.suiton + "#" + pit1.usingDoton + "#" + pit1.usingKaton + "#" + pit1.usingFuton + "#" + pit1.usingRaiton + "#" + pit1.usingSuiton + "#" + pit1.dotonEquiped + "#" + pit1.katonEquiped + "#" + pit1.futonEquiped + "#" + pit1.raitonEquiped + "#" + pit1.suitonEquiped + "#" + pit1.pc + "#" + pit1.pm + "#" + pit1.pe + "#" + pit1.cd + "#" + pit1.invoc + "#" + pit1.resiDoton + "#" + pit1.resiKaton + "#" + pit1.resiFuton + "#" + pit1.resiRaiton + "#" + pit1.resiSuiton + "#" + pit1.esquivePC + "#" + pit1.esquivePM + "#" + pit1.esquivePE + "#" + pit1.esquiveCD + "#" + pit1.retraitPC + "#" + pit1.retraitPM + "#" + pit1.retraitPE + "#" + pit1.retraitCD + "#" + pit1.evasion + "#" + pit1.blocage + "#" + pit1.genre.ToString() + "#" + pit1.Orientation + ":";

                        Random random = new Random();
                        int rand = random.Next(team2ValidePos.Length);
                        pit1.map_position = new Point(Convert.ToInt32(team1ValidePos[rand].Split('/')[0]), Convert.ToInt32(team1ValidePos[rand].Split('/')[1]));
                    }

                    if (playersData != "")
                        playersData = playersData.Substring(0, playersData.Length - 1);

                    playersData += "|";

                    for (int cnt = 0; cnt < _battle.Team2.Count; cnt++)
                    {
                        PlayerInfo pit2 = _battle.Team2[cnt];
                        playersData += pit2.Pseudo + "#" + pit2.ClasseName + "#" + pit2.Level + "#" + pit2.village + "#" + pit2.MaskColors + "#" + pit2.totalPdv + "#" + pit2.CurrentPdv + "#" + pit2.rang + "#" + pit2.Initiative + "#" + pit2.doton + "#" + pit2.katon + "#" + pit2.futon + "#" + pit2.raiton + "#" + pit2.suiton + "#" + pit2.usingDoton + "#" + pit2.usingKaton + "#" + pit2.usingFuton + "#" + pit2.usingRaiton + "#" + pit2.usingSuiton + "#" + pit2.dotonEquiped + "#" + pit2.katonEquiped + "#" + pit2.futonEquiped + "#" + pit2.raitonEquiped + "#" + pit2.suitonEquiped + "#" + pit2.pc + "#" + pit2.pm + "#" + pit2.pe + "#" + pit2.cd + "#" + pit2.invoc + "#" + pit2.resiDoton + "#" + pit2.resiKaton + "#" + pit2.resiFuton + "#" + pit2.resiRaiton + "#" + pit2.resiSuiton + "#" + pit2.esquivePC + "#" + pit2.esquivePM + "#" + pit2.esquivePE + "#" + pit2.esquiveCD + "#" + pit2.retraitPC + "#" + pit2.retraitPM + "#" + pit2.retraitPE + "#" + pit2.retraitCD + "#" + pit2.evasion + "#" + pit2.blocage + "#" + pit2.genre.ToString() + "#" + pit2.Orientation;

                        Random random = new Random();
                        int rand = random.Next(team2ValidePos.Length);
                        pit2.map_position = new Point(Convert.ToInt32(team2ValidePos[rand].Split('/')[0]), Convert.ToInt32(team2ValidePos[rand].Split('/')[1]));
                    }
                    ////////////////////////////////////
                    string team1 = "";
                    string team2 = "";

                    foreach (PlayerInfo piib in _battle.Team1)
                        team1 += piib.Pseudo + "|" + piib.map_position.X + "/" + piib.map_position.Y + "#";
                    team1 = team1.Substring(0, team1.Length - 1);

                    foreach (PlayerInfo piib in _battle.Team2)
                        team2 += piib.Pseudo + "|" + piib.map_position.X + "/" + piib.map_position.Y + "#";
                    team2 = team2.Substring(0, team2.Length - 1);

                    commun.SendMessage("cmd•challengeBegan•" + playersData + "•" + battleStartPositions.Map(_battle.Map, _battle.BattleType) + "•" + MainClass.InitialisationBattleWaitTime + "•" + _battle.BattleType + "•" + team1 + "•" + team2 + "•" + _battle.State + "•" + battleStartPositions.Map(_battle.Map, _battle.BattleType).Split('|')[0] + "•" + battleStartPositions.Map(_battle.Map, _battle.BattleType).Split('|')[1], im, true);
                    Console.WriteLine("<--cmd•challengeBegan•" + playersData + "•" + battleStartPositions.Map(_battle.Map, _battle.BattleType) + "•" + MainClass.InitialisationBattleWaitTime + "•" + _battle.BattleType + "•" + team1 + "•" + team2 + "•" + _battle.State + "•" + battleStartPositions.Map(_battle.Map, _battle.BattleType).Split('|')[0] + "•" + battleStartPositions.Map(_battle.Map, _battle.BattleType).Split('|')[1] + " to " + pi.Pseudo);*/
                    #endregion
                }
                else
                {
                    tmp = tmp.Substring(0, tmp.Length - 1);
                    GrabingActorsInformationResponseMessage grabingPlayersInformationResponseMessage = new GrabingActorsInformationResponseMessage();
                    grabingPlayersInformationResponseMessage.Initialize(new [] { tmp }, Nc);
                    grabingPlayersInformationResponseMessage.Serialize();
                    grabingPlayersInformationResponseMessage.Send();
                }
            }
            else
            {
                // l'utilisateur na pas de joueurs, il faut le basculer vers le map CreatePlayer
                CreateNewActorResponseMessage createNewPlayerResponseMessage = new CreateNewActorResponseMessage();
                createNewPlayerResponseMessage.Initialize(CommandStrings, Nc);
                createNewPlayerResponseMessage.Serialize();
                createNewPlayerResponseMessage.Send();
            }
        }
    }
}
