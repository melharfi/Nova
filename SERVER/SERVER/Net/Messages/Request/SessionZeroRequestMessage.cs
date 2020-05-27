using System.Collections.Generic;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class SessionZeroRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private Actor _actor;
        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)nc.Tag;
        }

        public bool Check()
        {
            #region
            return _actor.Pseudo != "" && _actor.map != "" && _actor.inBattle != 1;
            #endregion
        }

        public void Apply()
        {
            // informer tout les abonnées que le client viens de se désabonner du map
            List<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll(p => ((Actor)p.Tag).map == _actor.map && ((Actor)p.Tag).Pseudo != _actor.Pseudo && ((Actor)p.Tag).inBattle == 0);
            foreach (NetConnection t in abonnedPlayers)
            {
                ActorDisconnectedResponseMessage playerDisconnectedResponseMessage =
                    new ActorDisconnectedResponseMessage();
                playerDisconnectedResponseMessage.Initialize(new[] {_actor.Pseudo}, t);
                playerDisconnectedResponseMessage.Serialize();
                playerDisconnectedResponseMessage.Send();
            }
            abonnedPlayers.Clear();

            // modification de la bdd
            mysql.connected connected = ((List<mysql.connected>)DataBase.DataTables.connected).Find(f => f.pseudo == _actor.Pseudo);
            connected.pseudo = "";
            connected.timestamp = 0;
            connected.map = "";
            connected.map_position = "";

            // remise a zero tous les données du client
            _actor.SessionZero();
        }
    }
}
