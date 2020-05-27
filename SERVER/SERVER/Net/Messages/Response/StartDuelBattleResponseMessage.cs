using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class StartDuelBattleResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private Actor _actor;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
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
