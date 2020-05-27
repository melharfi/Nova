using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class ConfirmSelectActorRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private string _playerName;
        private Actor _actor;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            #region extracting data
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)Nc.Tag;
            #endregion
        }

        public bool Check()
        {
            if (CommandStrings.Length != 2)
                return false;
            if (_actor.Username == "" || _actor.Pseudo != "" || _actor.map != "")
                return false;

            _playerName = CommandStrings[1].ToString();

            return ((List<mysql.players>)DataBase.DataTables.players).Exists(f => f.pseudo == _playerName && f.user == _actor.Username);
        }

        public void Apply()
        {
            _actor.Pseudo = _playerName;
            CommonCode.RefreshStats(Nc);
            string spells = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _playerName).sorts;

            // recuperer le totalXp du niveau en cours
            int totalXp = ((List<mysql.xplevel>)DataBase.DataTables.xplevel).Exists(f => f.level == _actor.level + 1) ? ((List<mysql.xplevel>)DataBase.DataTables.xplevel).Find(f => f.level == (_actor.level + 1)).xp : 0;

            // convertir la list de quete en string, ce code se trouve sur 2 endroit, quand le joueur selectionne un player et quand le joueur été déja dans un combat et que apres un co qui précédé une deco, le player se selectionne tous seul
            string quete = _actor.Quests.Aggregate("", (current, t) => current + (t.QuestName + ":" + t.MaxSteps + ":" + t.CurrentStep + ":" + t.Submited.ToString() + "/"));

            if (quete != "")
                quete = quete.Substring(0, quete.Length - 1);

            // si cette cmd est modifié, il faut aussi modifier les autre cmd cmd•SelectedPlayer• et checkandupdatestates
            string buffer = _actor.Pseudo + "#" + _actor.classeName + "#" + _actor.spirit + "#" + _actor.spiritLvl +
                             "#" + _actor.Pvp + "#" + _actor.hiddenVillage + "#" + _actor.maskColorString + "#" +
                             _actor.directionLook + "#" + _actor.level + "#" + _actor.map + "#" + _actor.officialRang + "#" +
                             _actor.currentHealth + "#" + _actor.maxHealth + "#" + _actor.xp + "#" + totalXp + "#" +
                             _actor.doton + "#" + _actor.katon + "#" + _actor.futon + "#" + _actor.raiton + "#" +
                             _actor.suiton + "#" + MainClass.chakralvl1 + "#" + MainClass.chakralvl2 + "#" +
                             MainClass.chakralvl3 + "#" + MainClass.chakralvl4 + "#" + MainClass.chakralvl5 + "#" +
                             _actor.usingDoton + "#" + _actor.usingKaton + "#" + _actor.usingFuton + "#" +
                             _actor.usingRaiton + "#" + _actor.usingSuiton + "#" + _actor.equipedDoton + "#" +
                             _actor.equipedKaton + "#" + _actor.equipedFuton + "#" + _actor.equipedRaiton + "#" +
                             _actor.equipedSuiton + "#" + _actor.originalPc + "#" + _actor.originalPm + "#" +
                             _actor.pe + "#" + _actor.cd + "#" + _actor.summons + "#" + _actor.initiative + "#" +
                             _actor.job1 + "#" + _actor.job2 + "#" + _actor.specialty1 + "#" + _actor.specialty2 + "#" +
                             _actor.maxWeight + "#" + _actor.currentWeight + "#" + _actor.ryo + "#" +
                             _actor.resiDotonPercent + "#" + _actor.resiKatonPercent + "#" + _actor.resiFutonPercent +
                             "#" + _actor.resiRaitonPercent + "#" + _actor.resiSuitonPercent + "#" + _actor.dodgePC +
                             "#" + _actor.dodgePM + "#" + _actor.dodgePE + "#" + _actor.dodgeCD + "#" + _actor.removePC +
                             "#" + _actor.removePM + "#" + _actor.removePE + "#" + _actor.removeCD + "#" + _actor.escape +
                             "#" + _actor.blocage + "#" + spells + "#" + _actor.resiDotonFix + "#" + _actor.resiKatonFix +
                             "#" + _actor.resiFutonFix + "#" + _actor.resiRaitonFix + "#" + _actor.resiSuitonFix + "#" +
                             _actor.resiFix + "#" + _actor.domDotonFix + "#" + _actor.domKatonFix + "#" +
                             _actor.domFutonFix + "#" + _actor.domRaitonFix + "#" + _actor.domSuitonFix + "#" +
                             _actor.domFix + "#" + _actor.power + "#" + _actor.equipedPower + "•" + quete + "•" +
                             _actor.spellPointLeft;

            ConfirmSelectActorResponseMessage confirmSelectPlayerResponseMessage = new ConfirmSelectActorResponseMessage();
            confirmSelectPlayerResponseMessage.Initialize(new []{ buffer }, Nc);
            confirmSelectPlayerResponseMessage.Serialize();
            confirmSelectPlayerResponseMessage.Send();


            mysql.connected newConnetion = ((List<mysql.connected>)DataBase.DataTables.connected).Find(f => f.user == _actor.Username);
            newConnetion.pseudo = _playerName;
            newConnetion.timestamp = CommonCode.ReturnTimeStamp();
            newConnetion.map = _actor.map;
            newConnetion.map_position = _actor.map_position.X + "/" + _actor.map_position.Y;

            // il faut informer les autres de la connexion
            // pseudo#classe#pvp:spirit:spiritLvl#village#MaskColors#map_position#orientation#level#action#waypoint   - separateur entre plusieurs joueurs

            string data = _actor.Pseudo + "#" + _actor.classeName + "#";
            if (!_actor.Pvp)
                data += "false:null:null#";
            else
                data += "true:" + _actor.spirit + ":" + _actor.spiritLvl + "#";  // il faut renvoyer que True ou False au lieu de 0 ou 1, il faut aussi voir de l'autre coté "client MMORPG"
            // ici on passe pas les dernier donné qui corespond à action(walk,idle...) et waypoint
            data += _actor.hiddenVillage + "#" + _actor.maskColorString + "#" + _actor.map_position.X + "/" + _actor.map_position.Y + "#" + _actor.directionLook + "#" + _actor.level + "##";

            IList<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll(f => ((Actor)f.Tag).map == _actor.map && ((Actor)f.Tag).Pseudo != _actor.Pseudo && ((Actor)f.Tag).inBattle == 0);
            foreach (NetConnection t in abonnedPlayers)
            {
                SpawnActorResponseMessage spawnActorResponseMessage = new SpawnActorResponseMessage();
                spawnActorResponseMessage.Initialize(new object[]{data}, t);
                spawnActorResponseMessage.Serialize();
                spawnActorResponseMessage.Send();
            }

            abonnedPlayers.Clear();
        }
    }
}
