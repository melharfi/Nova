using System;
using MELHARFI.Lidgren.Network;
using MMORPG.Cryptography.Algo;

namespace MMORPG.Net.Messages.Request
{
    internal class DeleteActorRequestMessage : IRequestMessage
    {
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private readonly string _selectedDeletPlayer;
        private readonly string _secretQuestionTb;

        public DeleteActorRequestMessage(string selectedDeletPlayer, string secretQuestionTb)
        {
            _selectedDeletPlayer = selectedDeletPlayer;
            _secretQuestionTb = secretQuestionTb;
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _selectedDeletPlayer +
                     CommandDelimitterChar.Delimitter + _secretQuestionTb;
        }
    }
}
