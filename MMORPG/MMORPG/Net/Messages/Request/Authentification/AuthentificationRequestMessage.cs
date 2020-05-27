using System;
using MELHARFI.Lidgren.Network;
using MMORPG.Cryptography.Algo;

namespace MMORPG.Net.Messages.Request
{
    internal class AuthentificationRequestMessage : IRequestMessage
    {
        private readonly string _username;
        private readonly string _password;
        private readonly OverridePreviousConnexion _overridePreviousConnexion;
        public enum OverridePreviousConnexion
        {
            Normal, Replace
        }

        public AuthentificationRequestMessage(string username, string password, OverridePreviousConnexion overridePreviousConnexion)
        {
            _username = username;
            _password = password;
            _overridePreviousConnexion = overridePreviousConnexion;
        }

        public bool _serialized { get; set; }
        public string _buffer { get; set; }

        public void Send()
        {
            if (!_serialized)
                throw new NotImplementedException("buffer not serialized yet, you should call Serialize() method first");
            NetOutgoingMessage ogMessage = NetworkEncryption.Encrypt(_buffer);
            CommandCaster.Send(ogMessage);
        }

        public void Serialize()
        {
            _serialized = true;
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _username +
                     CommandDelimitterChar.Delimitter + Security.CalculateMD5Hash(_password) +
                     CommandDelimitterChar.Delimitter + MainForm.version + CommandDelimitterChar.Delimitter +
                     _overridePreviousConnexion;
        }
    }
}
