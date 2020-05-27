using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MELHARFI.Lidgren.Network;
using MMORPG.Cryptography.Algo;

namespace MMORPG.Net.Messages.Request
{
    internal class ConfirmSelectActorRequestMessage : IRequestMessage
    {
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private readonly string _playerName;
        public ConfirmSelectActorRequestMessage(string playerName)
        {
            _playerName = playerName;
        }

        public void Send()
        {
            if (!_serialized)
                throw new NotImplementedException("buffer not serialized yet, you should call Serialize() method first");
            NetOutgoingMessage ogMessage = NetworkEncryption.Encrypt(_buffer);
            CommandCaster.Send(ogMessage);
        }

        public void Serialize()
        {
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _playerName;
            _serialized = true;
        }
    }
}
