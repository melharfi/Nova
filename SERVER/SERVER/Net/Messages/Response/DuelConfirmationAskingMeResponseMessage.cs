using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class DuelConfirmationAskingMeResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private Actor _actor;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            //commandStrings[0]= nom de celui qui a lancé le defie
            Nc = nc;
            CommandStrings = commandStrings;
            _actor = nc.Tag as Actor;
            _serialized = false;
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
