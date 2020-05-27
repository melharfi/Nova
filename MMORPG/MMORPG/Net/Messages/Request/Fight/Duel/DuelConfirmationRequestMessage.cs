using MELHARFI.Lidgren.Network;
using MMORPG.Cryptography.Algo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPG.Net.Messages.Request
{
    internal class DuelConfirmationRequestMessage : IRequestMessage
    {
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private string _actorChallenged;

        public DuelConfirmationRequestMessage(string actorChallenged)
        {
            _actorChallenged = actorChallenged;
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
            _serialized = true;
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _actorChallenged;
        }
    }
}
