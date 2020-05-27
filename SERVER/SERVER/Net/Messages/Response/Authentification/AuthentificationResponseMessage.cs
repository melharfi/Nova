using System;
using System.Collections.Generic;
using Lidgren.Network;
using mysql;
using SERVER.Net.Messages.Request;

namespace SERVER.Net.Messages.Response
{
    internal class AuthentificationResponseMessage : IResponseMessage
    {
        private string _username;
        //private string _password;
        //private string _clientVersion;
        private Actor _actor;
        private AuthentificationRequestMessage.OverridePreviousConnexion _overridePreviousConnexion;
        private NetConnection _sessionParent;                    // pointeur vers celui qui détien la session et qui va etre écrasé par la suite
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            #region extracting data
            Nc = nc;
            CommandStrings = commandStrings;
            _actor = (Actor)nc.Tag;
            _username = commandStrings[1].ToString();
            //_password = commandStrings[2];
            //_clientVersion = commandStrings[3];

            _sessionParent = MainClass.netServer.Connections.Find(f => ((Actor)f.Tag).Username == _username);

            AuthentificationRequestMessage.OverridePreviousConnexion overridePreviouseConnexion;
            if (!Enum.TryParse(commandStrings[4].ToString(), out overridePreviouseConnexion))
            {
                // tester un autre parametre au lieu de normal ou replace pour voir si ce code sera exécuter
                overridePreviouseConnexion = AuthentificationRequestMessage.OverridePreviousConnexion.Normal;
            }
            _overridePreviousConnexion = overridePreviouseConnexion;
            #endregion
            #region affectation des données à la connexion en cours
            _actor.Username = _username;
            _actor.SignedIn = true;
            _actor.Nc = Nc;
            #endregion
            #region traitement
            switch (_overridePreviousConnexion)
            {
                case AuthentificationRequestMessage.OverridePreviousConnexion.Normal:
                    int connected = ((List<connected>)DataBase.DataTables.connected).FindAll(f => f.user == _username).Count;

                    if (connected > 0)
                    {
                        // on autorise d'ecraser l'ancienne connexion SI personnes n'été connecté dessus ou si celui qui demande de reprendre le joueur est celui qui a fait la 1ere demande
                        if(((Actor)_sessionParent.Tag).SubstituteUid == 0 || _actor.SubstituteUid == ((Actor)_sessionParent.Tag).SubstituteUid)
                        {
                            // un utilisateur est déja connecté, demande de le deconnecté
                            AuthentificationUserBusyResponseMessage authentificationUserBusyResponseMessage =
                                new AuthentificationUserBusyResponseMessage();

                            // pas la paine d'appeler les methode Serialize et Send puisqu'ils ne font rien dans cette class
                            _actor.SubstituteUid = Nc.RemoteUniqueIdentifier;
                            authentificationUserBusyResponseMessage.Initialize(CommandStrings, Nc);
                            authentificationUserBusyResponseMessage.Serialize();    // cette methode ne fait rien parce que Send() envoie une commande a l'ancienne, il faut réctifier cela sur Send()
                            authentificationUserBusyResponseMessage.Send();
                        }
                        else
                        {
                            // un autre utilisateur essai de prendre le controle, on dois annuler tous les autres tentatives
                            _actor.Disconnect(Enums.DisconnectReason.disconnectReason.ANOTHER_USER_OVERRIDE_CONNEXION);
                        }
                    }
                    else
                    {
                        // coordonnées d'identification correcte, client connecté
                        AuthentificationGrantedResponseMessage authentificationGrantedResponseMessage = new AuthentificationGrantedResponseMessage();
                        authentificationGrantedResponseMessage.Initialize(CommandStrings, Nc);
                        authentificationGrantedResponseMessage.Serialize();
                        authentificationGrantedResponseMessage.Send();
                    }
                    break;
                case AuthentificationRequestMessage.OverridePreviousConnexion.Replace:
                    // ceci est seuelement autorisé quand il y a déjà un personnage qui joue avec ce joueurs et qu'on souhaite le remplacer
                    if (_actor.SubstituteUid != ((Actor)_sessionParent.Tag).SubstituteUid)
                    {
                        // normalement l'utilisateur ne dois pas attérir ici puisque le serveur le deconnect lors de l'étape de l'authentification (1ere étape avant de répondre par un replace ou normal)
                        // si on ai attérie ici c'est que le client triche en envoyons directement au sérveur une requête d'authentification avec comme flag "replace" au lieu de "normal"
                        _actor.Disconnect(Enums.DisconnectReason.disconnectReason.ANOTHER_USER_OVERRIDE_CONNEXION);
                    }
                        // suppression depuis connected
                        ((List<connected>)DataBase.DataTables.connected).RemoveAll(f => f.user == _username);

                    // deconnexion de l'ancien joueur
                    Console.WriteLine(_username + " was deconnected by another user");
                    // un controle s'oblige pour un probleme qui arrive, quand un joueur deco grace a un bug de connexion ou débogage, et que le serveur garde toujours la session de ce joueur, et que le client renouvel une connexion,
                    //le serveur lui dis qu'un autre joueur été connécté, souhaitez vous le deconnecter, du coup quand vous le deconnecter, et que le serveur a deja supprimer celui qui avais initialement le jeton mais qui a expiré,
                    //une erreur survien sur l'objet qui référencé le 1er deteneur de connexion, donc il faut verifier si le joueur qui detené le 1er jeton existe toujour

                    if (MainClass.netServer.Connections.Exists(f => ((Actor)f.Tag).Username == _username))
                        MainClass.netServer.Connections.Find(f => ((Actor)f.Tag).Username == _username).Disconnect(Enums.DisconnectReason.disconnectReason.ANOTHER_USER_OVERRIDE_CONNEXION.ToString());

                    // suppression des logCount table
                    ((List<logcounter>)DataBase.DataTables.logcounter).RemoveAll(f => f.user == _username);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Authentification param differ than normal or replace");
            }
            #endregion
        }

        [Obsolete("This method is empty, no need to use it")]
        public void Send()
        {
            // AuthentificationRequestMessage•admin•ljk56ljk65lkj65lkj65lkj6•1.1•normal ou replace
            throw new NotImplementedException("This method is empty, no need to use it");
        }

        [Obsolete("This method is empty, no need to use it")]
        public void Serialize()
        {
            throw new NotImplementedException("This method is empty, no need to use it");
        }
    }
}
