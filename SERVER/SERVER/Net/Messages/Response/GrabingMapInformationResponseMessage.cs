using Lidgren.Network;
using System;

namespace SERVER.Net.Messages.Response
{
    internal class GrabingMapInformationResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            // commandStrings[0] (Data) = pseudo # class # pvp:spirit:levelSpirit # hiddenVillage # maskColors # map_position # orientation # level si notre personnage, si non 0 # animatedAction # waypoint # totalPdv si c notre personnage si non 0 # currentPdv si c notre perso si non 0 | <-- est le séparateur entre les donné de chaque acteur
            _serialized = false;
            Nc = nc;
            CommandStrings = commandStrings;
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + CommandStrings[0];
            _serialized = true;
        }
    }
}
