using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class ConfirmDeleteActorResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            _serialized = false;
            Nc = nc;
            CommandStrings = commandStrings;
        }

        public void Send()
        {
            // _buffer = mapobj.obj + "•" + mapobj.map_position + "•" + mapobj.idBattle + "•" + mapobj.assoc
            if (!_serialized)
                throw new NotImplementedException("buffer not serialized yet, you should call Serialize() function first");
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
