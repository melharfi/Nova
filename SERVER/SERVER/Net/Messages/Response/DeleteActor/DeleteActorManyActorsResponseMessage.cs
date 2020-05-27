using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class DeleteActorManyActorsResponseMessage
    {
        // arrive quand l'utilisateur supprimer un peronnage mais sur le serveur il y a plusieurs instance de ce meme personnage se qui est impossible
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            CommandStrings = commandStrings;
            Nc = nc;
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
            _buffer = GetType().Name;
            _serialized = true;
        }
    }
}
