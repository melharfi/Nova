using System;
using MELHARFI.Lidgren.Network;
using MMORPG.Cryptography.Algo;

namespace MMORPG.Net.Messages.Request
{
    internal class WayPointConfirmationRequestMessage : IRequestMessage
    {
        public bool _serialized { get; set; }
        public string _buffer { get; set; }     // buffer = ConfirmWaypointRequestMessage•wayPointString ex = 10,8 : 11,8 : 12,8
        private readonly string _waypoint;

        public WayPointConfirmationRequestMessage(string waypoint)
        {
            _waypoint = waypoint;
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _waypoint;
        }
    }
}
