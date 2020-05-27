using System;
using System.Collections.Generic;
using Lidgren.Network;
using mysql;

namespace SERVER.Net.Messages.Response
{
    internal class AuthentificationGrantedResponseMessage : IResponseMessage
    {
        private string _username;
        //private string _password;
        //private string _clientVersion;
        private Actor _actor;
        //private AuthentificationRequestMessage.OverridePreviousConnexion _overridePreviousConnexion;
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            #region extracting data
            Nc = nc;
            CommandStrings = commandStrings;
            _actor = (Actor)Nc.Tag;
            _username = commandStrings[1].ToString();
            //_password = commandStrings[2];
            //_clientVersion = commandStrings[3];

            /*AuthentificationRequestMessage.OverridePreviousConnexion overridePreviouseConnexion;
            if (!Enum.TryParse(commandStrings[4], out overridePreviouseConnexion))
            {
                // tester un autre parametre au lieu de normal ou replace pour voir si ce code sera exécuter
                overridePreviouseConnexion = AuthentificationRequestMessage.OverridePreviousConnexion.Normal;
                return;
            }
            _overridePreviousConnexion = overridePreviouseConnexion;*/
            #endregion
            #region affectation
            _actor.Username = _username;
            _actor.SignedIn = true;
            _actor.Nc = Nc;
            _actor.SubstituteUid = 0;
            #endregion
            #region [Database] integration in connected Table
            connected newConnection = new connected
            {
                ip = Nc.RemoteEndPoint.Address.ToString(),
                user = _username,
                uid = Nc.RemoteUniqueIdentifier.ToString(),
                date = DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture),
                timestamp = CommonCode.ReturnTimeStamp()
            };
            ((List<connected>)DataBase.DataTables.connected).Add(newConnection);
            #endregion
            #region [database] supression des tentative de connexion qui servé pour éviter le flood
            ((List<logcounter>)DataBase.DataTables.logcounter).RemoveAll(f => f.user == _username);
            #endregion
        }

        public void Send()
        {
            if (!_serialized)
                throw new NotImplementedException("buffer not serialized yet, you should call Serialize() method first");
            CommonCode.SendMessage(_buffer, Nc, true);
            Console.WriteLine("(SEND)(" + _username + ")" + _buffer.Replace(CommandDelimitterChar.Delimitter, '.'));
        }

        public void Serialize()
        {
            _serialized = true;
            _buffer = GetType().Name;
        }
    }
}
