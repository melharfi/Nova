using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    class WayPointInteruptedByActorRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private Actor _actor;

        public void Apply()
        {
            // reinitialisation des données de mouvement, Action,WayPoint,WayPointCnt,wayPointTimeStamp
            _actor.animatedAction = Enums.AnimatedActions.Name.idle;
            _actor.wayPoint.Clear();
            _actor.wayPointCnt = 0;
            _actor.wayPointTimeStamp = 0;

            // informer les clients de l'arret du mouvement du joueur et sa nouvelle position y compris le jouer lui meme
            IList<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll(f => ((Actor)f.Tag).map == _actor.map);
            for (int cnt = 0; cnt < abonnedPlayers.Count; cnt++)
            {
                WayPointInteruptedByActorResponseMessage wayPointInteruptedByActorResponseMessage = new WayPointInteruptedByActorResponseMessage();
                object[] o = new object[2];
                o[0] = _actor.Pseudo;
                o[1] = _actor.map_position.X + "," + _actor.map_position.Y;

                wayPointInteruptedByActorResponseMessage.Initialize(o, abonnedPlayers[cnt]);
                wayPointInteruptedByActorResponseMessage.Serialize();
                wayPointInteruptedByActorResponseMessage.Send();
            }
            abonnedPlayers.Clear();
        }

        public bool Check()
        {
            if (_actor.inBattle == 1)
                return false;

            // verification si le client est en mouvemengt reellement ou pas
            // si se n'est pas le cas c'est que le client triche en voulons s'arreter alors qu'il n'été pas en mouvement
            if (_actor.animatedAction == Enums.AnimatedActions.Name.idle)
                return false;

            return true;
        }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)nc.Tag;
        }
    }
}
