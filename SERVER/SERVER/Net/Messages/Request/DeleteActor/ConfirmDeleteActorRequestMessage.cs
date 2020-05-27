using System.Collections.Generic;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class ConfirmDeleteActorRequestMessage : IRequestMessage
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
            if (_actor.Username != "" && _actor.Pseudo == "" && _actor.map == "") return true;
            Security.User_banne("Actor without generic stats.ConfirmDeletePlayerRequestMessage", Nc);
            return false;
        }

        public void Apply()
        {
            string questionSecrette = ((List<mysql.users>)DataBase.DataTables.users).Find(f => f.username == ((Actor)Nc.Tag).Username).question_secrette;
            ConfirmDeleteActorResponseMessage askDeletePlayerResponseMessage = new ConfirmDeleteActorResponseMessage();
            askDeletePlayerResponseMessage.Initialize(new[] { questionSecrette }, Nc);
            askDeletePlayerResponseMessage.Serialize();
            askDeletePlayerResponseMessage.Send();
        }
    }
}
