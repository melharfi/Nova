using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class GrabingMapActorsInformationRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        Actor _actor;
        //public static int counter = 0;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            Nc = nc;
            CommandStrings = commandStrings;
            _actor = (Actor)Nc.Tag;
        }

        public bool Check()
        {
            if (_actor.Username == "" || _actor.Pseudo == "" || _actor.map == "")
            {
                Security.User_banne("Actor without generic stats.GrabingMapActorsInformationRequestMessage", Nc);
                return false;
            }
            return true;
        }

        public void Apply()
        {
            #region
            if (_actor.Pseudo == "")
            {
                Security.User_banne("Player ask for map information while he does'nt have any actor currently used(maybe sessionZero)", Nc);
                return;
            }
            // on check si l'utilisateur est en combat, si oui, verifier si le combat existe, si oui, verifier s'il reste plus d'1 joueur non invoc dans les 2 teams
            // un bug arrive rarement qui fait qu'un client se deconnecte d'un combat, et apres il co et il big sur l'état qu'il été en combat, alors qu'il s'est deconnecté mais le serveur garde la session de combat, donc il cloture pas
            if (_actor.inBattle == 1)
            {
                if (!Battle.Battles.Exists(f => f.IdBattle == _actor.idBattle))
                {
                    _actor.inBattle = 0;
                    // mise a jours sur la bdd
                    // mise du statut inBattle = 0, idBattle = -1 pour les joueurs
                    mysql.players player = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _actor.Pseudo);
                    player.inBattle = 0;
                    player.inBattleID = 0;
                    player.inBattleType = "";
                }
            }

            // check si l'utilisateur est en combat
            if (_actor.inBattle == 1)
            {
                // recuperation des pseudo de tous les joueurs des 2 teams
                // check s'il y a une session de notre combat, normalement elle dois se trouver mais par mesure
                if (!Battle.Battles.Exists(f => f.IdBattle == _actor.idBattle))
                    return;
                Battle battle = Battle.Battles.Find(f => f.IdBattle == _actor.idBattle);
                
                // récupération des données des utilisateurs du sideA
                string sideAData = battle.SideA.Aggregate("", (current, currentPlayerInfo) => current + (currentPlayerInfo.Pseudo + "#" + currentPlayerInfo.classeName + "#" + currentPlayerInfo.level + "#" + currentPlayerInfo.hiddenVillage + "#" + currentPlayerInfo.maskColorString + "#" + currentPlayerInfo.maxHealth + "#" + currentPlayerInfo.currentHealth + "#" + currentPlayerInfo.officialRang + "|"));

                if (sideAData == "")
                {
                    Console.WriteLine("le joueur est supposé etre en combat mais aucun joueurs ne se trouve dans la liste");
                    // la methode qui cloture le combat na pas fini son travail
                }
                if (sideAData != "")
                    sideAData = sideAData.Substring(0, sideAData.Length - 1);

                // récupération des données des utilisateurs team2
                string sideBData = battle.SideB.Aggregate("", (current, currentPlayerInfo) => current + (currentPlayerInfo.Pseudo + "#" + currentPlayerInfo.classeName + "#" + currentPlayerInfo.level + "#" + currentPlayerInfo.hiddenVillage + "#" + currentPlayerInfo.maskColorString + "#" + currentPlayerInfo.maxHealth + "#" + currentPlayerInfo.currentHealth + "#" + currentPlayerInfo.officialRang + "|"));
                if (sideBData.Length > 0)
                    sideBData = sideBData.Substring(0, sideBData.Length - 1);

                // envoie au client les données du combat
                var joinBattleResponseMessage = new JoinBattleInPreparationTimeResponseMessage();
                joinBattleResponseMessage.Initialize(new[] { sideAData, sideBData, battleStartPositions.Map(_actor.map, battle), MainClass.InitialisationBattleWaitTime.ToString(), battle.Timestamp.ToString() } , Nc);
                joinBattleResponseMessage.Serialize();
                joinBattleResponseMessage.Send();
            }
            else
            {
                // récuperer les données du map comme les joueurs abonnées
                string map = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _actor.Pseudo).map;
                _actor.map = map;

                StringBuilder data = new StringBuilder();
                foreach (mysql.connected connected in ((List<mysql.connected>)DataBase.DataTables.connected).FindAll(f => f.map == _actor.map))
                {
                    mysql.players curPlayer = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == connected.pseudo && f.inBattle == 0);
                    if (curPlayer == null)
                        break;
                    data.Append(connected.pseudo + "#" + curPlayer.classe + "#");

                    if (curPlayer.pvpEnabled == 0)
                        data.Append("0:null:null#");
                    else
                        data.Append(curPlayer.pvpEnabled + ":" + curPlayer.spirit + ":" + curPlayer.spiritLevel + "#");

                    int selectedPlayer = MainClass.netServer.Connections.FindIndex(sp => sp.Tag.GetType() == typeof(Actor) && ((Actor)sp.Tag).Pseudo == connected.pseudo);
                    if (selectedPlayer == -1)
                        return;
                    Enums.AnimatedActions.Name animatedAction;
                    // le serveur a planté sur l'affectaion du variable action bizarement, peux etre que l'autre client viens just de deco quand on essayé d'extraire son action, du coup il est null
                    if (selectedPlayer >= 0 && ((Actor)MainClass.netServer.Connections[selectedPlayer].Tag).animatedAction != Enums.AnimatedActions.Name.idle)
                        animatedAction = ((Actor)MainClass.netServer.Connections[selectedPlayer].Tag).animatedAction;
                    else
                        animatedAction = Enums.AnimatedActions.Name.idle;
                    List<Point> wayPointList = ((Actor)MainClass.netServer.Connections[selectedPlayer].Tag).wayPoint;
                    // il faut allimenter le wayPointString par les infos du joueur est non du celui qui demande les infos
                    string wayPointString = wayPointList.Aggregate("", (current, t) => current + (t.X + "," + t.Y + ':'));

                    if (wayPointString != string.Empty)
                        wayPointString = wayPointString.Substring(0, wayPointString.Length - 1);

                    // (_actor.Pseudo == connected.pseudo) ? curPlayer.level : 0) ?? étrange, on vérifie s'il s'agit de notre personnage, si c le cas on envoie notre niveau si non on envoie la valeur 0, pk envoyer nos states ? il faut juste selectionner les autres, ainsi pour le total pdv ... il faut enlever dans la boucle foreach la selection de notre personnage et envoyer que 0 sur les autre states level pdv ... ou les supprimer carémenet de la cmd
                    data.Append(curPlayer.hiddenVillage + "#" + curPlayer.maskColorString + "#" + curPlayer.map_position + "#" + curPlayer.directionLook + "#" + (_actor.Pseudo == connected.pseudo ? curPlayer.level : 0) + "#" + animatedAction + "#" + wayPointString + "#" + (_actor.Pseudo == connected.pseudo ? curPlayer.maxHEalth : 0) + "#" + (_actor.Pseudo == connected.pseudo ? curPlayer.currentHealth : 0) + "#" + _actor.officialRang + " |");
                    wayPointList.Clear();
                }

                if (data.ToString() == "") return;
                data = new StringBuilder(data.ToString().Substring(0, data.Length - 1));

                GrabingMapInformationResponseMessage grabingMapInformationResponseMessage = new GrabingMapInformationResponseMessage();
                grabingMapInformationResponseMessage.Initialize(new[] { data.ToString() }, Nc);
                grabingMapInformationResponseMessage.Serialize();
                grabingMapInformationResponseMessage.Send();
            }
            #endregion
        }
    }
}
