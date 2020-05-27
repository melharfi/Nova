using System;
using System.Collections.Generic;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class AuthentificationRequestMessage : IRequestMessage
    {
        private string _username;
        private string _password;
        private string _clientVersion;
        private Actor _actor;
        //private OverridePreviousConnexion _overridePreviousConnexion;
        public enum OverridePreviousConnexion
        {
            Normal, Replace
        }

        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            // AuthentificationRequestMessage•username•password crypted with md5•version like 1.1•normal ou replace
            #region extracting data
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)nc.Tag;
            _username = commandStrings[1].ToString();
            _password = commandStrings[2].ToString();
            _clientVersion = commandStrings[3].ToString();
            #endregion
        }

        public bool Check()
        {
            #region vérification de la longeur de la chaine
            if (CommandStrings.Length != 5)
                return false;
            #endregion
            #region vérifier si le joueur est déjà connecté, pour cela on vérifie si il nous as déjà fournie son nom d'utilisateur et si une map lui a été attribué
            if (_actor.Username != "" || _actor.map != "")
                //Security.User_banne("identification", im);
                // il faut informer le joueur que ceci ne peux pas arriver et qu'il dois soumettre un bugReporte
                return false;
            #endregion
            #region verification de la version
            // ReSharper disable once PossibleNullReferenceException
            var verServer = (DataBase.DataTables.version as List<mysql.version>).Find(f => f.app == "app")._version.Split('.');

            // la requette retourne 1 seul enregistrement qui correspond a l'application
            // si un loader.exe est prévus, cette requette va retourner 2 enregistrement,app puis loader
            // il faut ajouter un while si c'est le ca
            var verClient = _clientVersion.Split('.');

            // une version dois imperativement se composer de 2 indentifiants, numéro version majeur, puis révision
            if (verClient.Length < 2)
                return false;       // il faut informer le client que sa version est érronnée
            try
            {
                int.Parse(verClient[0]);
                int.Parse(verClient[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + " \n\t" + _username + " client tente de passer une version non numerique");
                return false;
            }
            /////////// comparaison du 1er index
            if (Convert.ToInt32(verServer[0]) != Convert.ToInt32(verClient[0]))
            {
                ///// version majeur inferieur, a proposer la MAJ
                VersionResponseMessage versionResponseMessage = new VersionResponseMessage();
                versionResponseMessage.Initialize(new[] { Enums.Version.version.MAJOR_LESS.ToString() }, Nc);
                versionResponseMessage.Send();
                return false;
            }
            ////// client a la derniere version majeur
            //  verification de la version secondaire
            if (Convert.ToInt32(verServer[1]) != Convert.ToInt32(verClient[1]))
            {
                /////// version secondaire inferieur,a proposer la MAJ
                VersionResponseMessage versionResponseMessage = new VersionResponseMessage();
                versionResponseMessage.Initialize(new[] { Enums.Version.version.REVISION_LESS.ToString() }, Nc);
                versionResponseMessage.Send();
                return false;
            }

            #endregion
            #region Verification du nom d'utilisateur contre les injections}
            ///////////////////////////////////////////////
            if (!Security.check_valid_user(_username) || !Security.check_valid_pwd(_password))
            {
                // si arrivé ici c'est que le client a modifié son application pour contourner la methode de securité
                AuthentificationUserNotAllowedResponseMessage authentificationUserNotAllowedResponseMessage = new AuthentificationUserNotAllowedResponseMessage();
                authentificationUserNotAllowedResponseMessage.Initialize(CommandStrings, Nc);
                authentificationUserNotAllowedResponseMessage.Serialize();
                authentificationUserNotAllowedResponseMessage.Send();
                return false;
            }
            #endregion
            #region comparaison des identifiants avec la bdd
            int resultQuery = ((List<mysql.users>)DataBase.DataTables.users).FindAll(f => f.username == _username && f.password == _password.ToLower()).Count;

            switch (resultQuery)
            {
                case 0:
                    // check flooding access try
                    int curTimestamp = CommonCode.ReturnTimeStamp();
                    int resultUserLogin = ((List<mysql.logcounter>)DataBase.DataTables.logcounter).FindAll(f => f.timestamp >= (curTimestamp - (60 * 30))).Count;

                    if (resultUserLogin >= 10)
                    {
                        // supression des anciens enregistrement qui dates de plus de 30min
                        ((List<mysql.logcounter>)DataBase.DataTables.logcounter).RemoveAll(f => f.user == _username);

                        // ban de l'ip
                        mysql.bannedip bannedIp = new mysql.bannedip
                        {
                            ip = Nc.RemoteEndPoint.Address.ToString(),
                            reson = "too many wrong login tries",
                            censure = curTimestamp + (60 * 30)
                        };

                        ((List<mysql.bannedip>)DataBase.DataTables.bannedip).Add(bannedIp);
                        Nc.Disconnect(Enums.DisconnectReason.disconnectReason.IP_BANNED.ToString());
                        return false;
                    }
                    mysql.logcounter logCounter = new mysql.logcounter
                    {
                        user = _username,
                        timestamp = curTimestamp
                    };

                    ((List<mysql.logcounter>)DataBase.DataTables.logcounter).Add(logCounter);

                    // on supprime tous les instances dons le timestamp est supérieur à 30 min
                    ((List<mysql.logcounter>)DataBase.DataTables.logcounter).RemoveAll(f => f.timestamp < curTimestamp - (60 * 30));

                    // utilisateur ou mot de passe incorrecte
                    // il faut que le message passé en référence sois une Enum.disconnect
                    Nc.Disconnect(Enums.DisconnectReason.disconnectReason.WRONG_CREDENTIALS.ToString());
                    return false;
                case 1:
                    // identification confirmé
                    /////////  verification si l'utilisateur est bannée
                    int banned = ((List<mysql.banneduser>)DataBase.DataTables.banneduser).FindAll(f => f.user == _username).Count;

                    if (banned == 0)
                    {
                        /////// l'utilisateur n'est pas banni
                        // verification si l'utilisateur a confirmé sa boite email

                        if (((List<mysql.users>)DataBase.DataTables.users).Exists(f => f.username == _username && f.confirmation_email == 0))
                        {
                            // il faut envoyer au client la bonne commande, il faut aussi deconnecter le client, parce la on lui envoie juste l'information mais il reste connecté, peux etre que le client se deconnecte depuis le client mais c'est unsafe
                            AuthentificationEmailNotValidatedResponseMessage authentificationEmailValidationResponseMessage = new AuthentificationEmailNotValidatedResponseMessage();
                            authentificationEmailValidationResponseMessage.Initialize(CommandStrings, Nc);
                            authentificationEmailValidationResponseMessage.Serialize();
                            authentificationEmailValidationResponseMessage.Send();
                            return false;
                        }
                        return true;
                    }
                    /////// l'utilisateur est banni, il faut que le message passé en référence sois une Enum.Disconnect
                    Nc.Disconnect(Enums.DisconnectReason.disconnectReason.USER_BANNED.ToString());
                    return false;
            }
            return true;
            #endregion
        }

        public void Apply()
        {
            AuthentificationResponseMessage authentificationResponseMessage = new AuthentificationResponseMessage();
            authentificationResponseMessage.Initialize(CommandStrings, Nc);
        }
    }
}
