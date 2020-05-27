using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class AuthentificationUserBusyResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        //private string _username;
        //private string _password;
        //private string _clientVersion;
        //private Actor _actor;
        //private AuthentificationRequestMessage.OverridePreviousConnexion _overridePreviousConnexion;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            #region extracting data
            Nc = nc;
            CommandStrings = commandStrings;
            /*_actor = nim.SenderConnection.Tag as Actor;
            _username = commandStrings[1];
            _password = commandStrings[2];
            _clientVersion = commandStrings[3];

            /*AuthentificationRequestMessage.OverridePreviousConnexion overridePreviouseConnexion;
            if (!Enum.TryParse(commandStrings[4], out overridePreviouseConnexion))
            {
                // tester un autre parametre au lieu de normal ou replace pour voir si ce code sera exécuter
                overridePreviouseConnexion = AuthentificationRequestMessage.OverridePreviousConnexion.Normal;
                return;
            }
            _overridePreviousConnexion = overridePreviouseConnexion;*/
            #endregion
        }

        public void Send()
        {
            if (!_serialized)
                throw new NotImplementedException("buffer not serialized yet, you should call Serialize() method first");
            CommonCode.SendMessage(_buffer, Nc, true);
            Console.WriteLine("(SEND)" + _buffer.Replace(CommandDelimitterChar.Delimitter, '.'));
        }

        public void Serialize()
        {
            // s'il y à un autre utilisateur sur le meme compte on envoie au client une requete qui lui permet sois de le deconnecter et se connecter à la place sois de se deconnecter
            // il se peux que le client spam plusieurs connexion avec different thread et qui reste en attente et garde tjr une connexion ouverte se qui va bloquer le serveur
            // il faut trouver un script qui enregistre ces demandes dans une fil d'attente et apres un x timestamp il le deconnecte automatiquement, mais il si se connecte il faut le supprimer de cette fil d'attente
            _buffer = GetType().Name;
            _serialized = true;
        }
    }
}
