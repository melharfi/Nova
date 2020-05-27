using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response.Map
{
    class MapObjetRemovedResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private Actor _actor;
        private Enums.BattleType.Type _battleType;
        private int _idBattle;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            //commandStrings[0] = Enums.BattleType.Type like FreeChallenge
            //commandStrings[1] = _battle.IdBattle
            _battleType = (Enums.BattleType.Type)commandStrings[0];
            _idBattle = (int)commandStrings[1];

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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + CommandStrings[0] + CommandDelimitterChar.Delimitter + CommandStrings[1];
            _serialized = true;
        }
    }
}
