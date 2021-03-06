﻿using System;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class AuthentificationUserNotAllowedResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }

        public void Initialize(object[] commandStrings, NetConnection nim)
        {
            // AuthentificationRequestMessage•username•password crypted with md5•version like 1.1•normal ou replace
            // CommandStrings = commandStrings;
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
