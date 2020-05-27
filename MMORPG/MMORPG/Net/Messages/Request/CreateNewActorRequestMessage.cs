using System;
using MELHARFI.Lidgren.Network;
using MMORPG.Cryptography.Algo;

namespace MMORPG.Net.Messages.Request
{
    internal class CreateNewActorRequestMessage : IRequestMessage
    {
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        readonly string _nameOfNewActor;
        readonly Enums.ActorClass.ClassName _selectedClass;
        readonly Enums.HiddenVillage.Names _selectedHiddenVillage;
        readonly string _maskColor;

        public CreateNewActorRequestMessage(string nameOfNewActor, Enums.ActorClass.ClassName selectedClass, Enums.HiddenVillage.Names selectedHiddenVillage, string maskColor)
        {
            _nameOfNewActor = nameOfNewActor;
            _selectedClass = selectedClass;
            _selectedHiddenVillage = selectedHiddenVillage;
            _maskColor = maskColor;
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
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter + _nameOfNewActor + CommandDelimitterChar.Delimitter + _selectedClass + CommandDelimitterChar.Delimitter + _selectedHiddenVillage + CommandDelimitterChar.Delimitter + _maskColor;
        }
    }
}
