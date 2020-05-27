using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class WayPointReplacedResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private string _pseudo;
        private string _wayPointString;         //ex 10,8:10,9 ...10 = x et 8 = y et : séparateur entre les points

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            _serialized = false;
            Nc = nc;
            CommandStrings = commandStrings;
            _pseudo = commandStrings[0].ToString();
            _wayPointString = commandStrings[1].ToString();
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _pseudo + CommandDelimitterChar.Delimitter + _wayPointString;
            _serialized = true;
        }
    }
}
