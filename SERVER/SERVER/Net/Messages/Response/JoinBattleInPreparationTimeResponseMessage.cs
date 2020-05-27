using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class JoinBattleInPreparationTimeResponseMessage : IResponseMessage
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
            // CommandString[0] = sideAData
                // currentPlayerInfo.Pseudo + "#" + currentPlayerInfo.classeName + "#" + currentPlayerInfo.Level + "#" + currentPlayerInfo.village + "#" + currentPlayerInfo.MaskColors + "#" + currentPlayerInfo.totalHealth + "#" + currentPlayerInfo.currentHealth + "#" + currentPlayerInfo.officialRang + "|";     | séparateur entre les states des joueurs

            // CommandString[1] = sideBData
                // currentPlayerInfo.Pseudo + "#" + currentPlayerInfo.classeName + "#" + currentPlayerInfo.Level + "#" + currentPlayerInfo.village + "#" + currentPlayerInfo.MaskColors + "#" + currentPlayerInfo.totalHealth + "#" + currentPlayerInfo.currentHealth + "#" + currentPlayerInfo.officialRang + "|";     | séparateur entre les states des joueurs

            // CommandString[2] = battleStartPositions.Map (_actor.map, _battle.BattleType)

            // CommandString[3] = MainClass.InitialisationBattleWaitTime.ToString()

            // CommandString[4] = _battle.timestamp.ToString()
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + CommandStrings[0] + CommandDelimitterChar.Delimitter + CommandStrings[1] + CommandDelimitterChar.Delimitter + CommandStrings[2] + CommandDelimitterChar.Delimitter + CommandStrings[3] + CommandDelimitterChar.Delimitter + CommandStrings[4];
            _serialized = true;
        }
    }
}
