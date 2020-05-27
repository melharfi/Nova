using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class GrabingActorsInformationResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            // CommandStrings[0] = "narutox#30#ange#naruto#1#15#kumo#52-153-201/253-197-134/12-46-88#null#null|player2#1#neutre#choji#0#1#suna#38-62-157/81-171-238/172-239-253#null#null|player3#1#neutre#kabuto#0#1#suna#225-214-66/228-141-63/151-103-4#null#null|player5#1#neutre#naruto#0#1#kiri#null/null/null#null#null"
            _serialized = false;
            CommandStrings = commandStrings;
            Nc = nc;
        }

        public void Send()
        {
            if (!_serialized)
                throw new NotImplementedException("buffer not serialized yet, you should call Serialize() method first");
            CommonCode.SendMessage(_buffer, Nc, true);
            Console.WriteLine("(SEND)" + _buffer.Replace(CommandDelimitterChar.Delimitter,'.'));
        }

        public void Serialize()
        {
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + CommandStrings[0];
            _serialized = true;
        }
    }
}
