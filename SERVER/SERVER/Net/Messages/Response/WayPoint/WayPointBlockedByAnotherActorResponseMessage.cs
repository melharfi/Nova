using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class WayPointBlockedByAnotherActorResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private int blockedPlayerInPos;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            _serialized = false;
            Nc = nc;
            blockedPlayerInPos = (int)commandStrings[1];
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + blockedPlayerInPos;
            _serialized = true;
        }
    }
}
