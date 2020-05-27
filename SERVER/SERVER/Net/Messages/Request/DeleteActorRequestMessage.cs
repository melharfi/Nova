using System.Collections.Generic;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class DeleteActorRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private Actor _actor;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            #region extracting data
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)nc.Tag;
            #endregion
        }

        public bool Check()
        {
            if (CommandStrings.Length != 3)
                return false;

            if (_actor.Username == "" || _actor.Pseudo != "" || _actor.map != "")
            {
                Security.User_banne("Actor without generic stats.DeletePlayerRequestMessage", Nc);
                return false;
            }
            return true;
        }

        public void Apply()
        {
            string reponseSecrette = ((List<mysql.users>)DataBase.DataTables.users).Find(f => f.username == _actor.Username).reponse_secrette;

            string userSecreteQuestionResponse = CommandStrings[2].ToString();
            string deleteActorName = CommandStrings[1].ToString();

            if (reponseSecrette == userSecreteQuestionResponse)
            {
                int deletedPlayer = ((List<mysql.players>)DataBase.DataTables.players).RemoveAll(f => f.user == _actor.Username && f.pseudo == deleteActorName);

                if (deletedPlayer == 0)
                {
                    // impossible de supprimer un joueur qui n'existe pas, donc le client triche
                    DeleteActorNotFoundResponseMessage deletePlayerActorNotFoundResponseMessage = new DeleteActorNotFoundResponseMessage();
                    deletePlayerActorNotFoundResponseMessage.Initialize(CommandStrings, Nc);
                    deletePlayerActorNotFoundResponseMessage.Serialize();
                    deletePlayerActorNotFoundResponseMessage.Send();
                }
                else if (deletedPlayer > 1)
                {
                    // impossible d'avoir plusieurs personnages avec le meme nom
                    DeleteActorManyActorsResponseMessage deletePlayerManyActorsResponseMessage = new DeleteActorManyActorsResponseMessage();
                    deletePlayerManyActorsResponseMessage.Initialize(CommandStrings, Nc);
                    deletePlayerManyActorsResponseMessage.Serialize();
                    deletePlayerManyActorsResponseMessage.Send();
                }

                DeleteActorGrantedResponseMessage deletePlayerGrantedResponseMessage = new DeleteActorGrantedResponseMessage();
                deletePlayerGrantedResponseMessage.Initialize(CommandStrings, Nc);
                deletePlayerGrantedResponseMessage.Serialize();
                deletePlayerGrantedResponseMessage.Send();
            }
            else
            {
                DeleteActorIncorrectSecretAnswerResponseMessage deletePlayerIncorrectSecretAnswerResponseMessage = new DeleteActorIncorrectSecretAnswerResponseMessage();
                deletePlayerIncorrectSecretAnswerResponseMessage.Initialize(CommandStrings, Nc);
                deletePlayerIncorrectSecretAnswerResponseMessage.Serialize();
                deletePlayerIncorrectSecretAnswerResponseMessage.Send();
            }
        }
    }
}
