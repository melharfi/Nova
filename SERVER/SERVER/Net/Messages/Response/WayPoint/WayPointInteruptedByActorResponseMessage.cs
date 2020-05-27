using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class WayPointInteruptedByActorResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private string _actorName;
        private string _locationString;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            Nc = nc;
            CommandStrings = commandStrings;
            _serialized = false;
            _actorName = (String)commandStrings[0];
            _locationString = (string) commandStrings[1];
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _actorName + CommandDelimitterChar.Delimitter + _locationString;
            _serialized = true;
        }
    }
}
