using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class ActorDisconnectedResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private string _actorPseudo;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            _actorPseudo = commandStrings[0].ToString();
            Nc = nc;
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _actorPseudo;
            _serialized = true;
        }
    }
}
