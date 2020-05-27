using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class VersionResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        
        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            CommandStrings = commandStrings;
            Nc = nc;
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
            // VersionResponseMessage•major less•Network.downloadMajorLink
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + CommandStrings[0] + CommandDelimitterChar.Delimitter + Network.downloadMajorLink;
            _serialized = true;
        }
    }
}