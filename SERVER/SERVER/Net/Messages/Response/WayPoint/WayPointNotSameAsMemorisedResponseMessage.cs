using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class WayPointNotSameAsMemorisedResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private Point _newPoint;                            // nouvelle poisition du joueur
        private int _orientation;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            _serialized = false;
            Nc = nc;
            _newPoint = (Point)commandStrings[0];
            _orientation = (int) commandStrings[1];
        }
        public void Send()
        {
            if (!_serialized)
                throw new NotImplementedException("buffer not serialized yet, you should call Serialize() method first");
            CommonCode.SendMessage(_buffer + CommandDelimitterChar.Delimitter + _newPoint.X + CommandDelimitterChar.Delimitter + _newPoint.Y + CommandDelimitterChar.Delimitter + _orientation, Nc, true);
            Console.WriteLine("(SEND)" + _buffer.Replace(CommandDelimitterChar.Delimitter, '.'));
        }

        public void Serialize()
        {
            _buffer = GetType().Name;
            _serialized = true;
        }
    }
}
