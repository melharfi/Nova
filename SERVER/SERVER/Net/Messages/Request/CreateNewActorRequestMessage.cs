using System;
using System.Collections.Generic;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class CreateNewActorRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private Actor _actor;
        string _nameOfNewActor;
        Enums.ActorClass.ClassName _selectedClass;
        Enums.HiddenVillage.Names _selectedHiddenVillage;
        string _maskColorString;
        string[] _maskColor;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)Nc.Tag;
        }

        public bool Check()
        {
            if (_actor.Username == "" || _actor.Pseudo != "" || _actor.map != "")   // client dois être en session_zero
                return false;

            if (CommandStrings.Length != 5)
                return false;
            
            _nameOfNewActor = CommandStrings[1].ToString();

            // vérifier si le client envoie un nom de classe indisponible pour voir se que le parse va retourner, peux etre qu'il selectionne tjr la 1ere occurance
            if (!Enum.TryParse(CommandStrings[2].ToString(), out _selectedClass))
                return false; // class name introuvable

            if (!Enum.TryParse(CommandStrings[3].ToString(), out _selectedHiddenVillage))
            {
                CreateNewActorVillageNotSelectedResponseMessage createNewActorVillageNotSelectedResponseMessage = new CreateNewActorVillageNotSelectedResponseMessage();
                createNewActorVillageNotSelectedResponseMessage.Initialize(null, Nc);
                createNewActorVillageNotSelectedResponseMessage.Serialize();
                createNewActorVillageNotSelectedResponseMessage.Send();
                return false; // village introuvable
            }

            _maskColorString = CommandStrings[4].ToString();

            _maskColor = _maskColorString.Split('/');

            if (_maskColorString.Length < 14 || _maskColorString.Length > 35)
            {
                // MaskColor incorrect
                return false;
            }
            if (_maskColor.Length != 3)
            {
                // MaskColor incorrect
                return false;
            }

            // verification de la syntax du pseudo contre les caractères non autorisés
            // verification si le pseudo est déja utilisé
            mysql.players newPlayer = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _nameOfNewActor);

            if (newPlayer != null)
            {
                // traitement quand l'utilisateur tente de créer un joueur avec un pseudo déja utilisé
                CreateNewActorNameAlreadyUsedResponseMessage newActorNameAlreadyUsedResponseMessage = new CreateNewActorNameAlreadyUsedResponseMessage();
                newActorNameAlreadyUsedResponseMessage.Initialize(null, Nc);
                newActorNameAlreadyUsedResponseMessage.Serialize();
                newActorNameAlreadyUsedResponseMessage.Send();
                return false;
            }
            if (_nameOfNewActor.Length < 3 || _nameOfNewActor.Length > 10)
            {
                // traitement quand l'utilisateur tente de créer un joueur avec un pseudo déja utilisé
                CreateNewActorNameWrongSizeResponseMessage newActorPseudoWrongSizeResponseMessage = new CreateNewActorNameWrongSizeResponseMessage();
                newActorPseudoWrongSizeResponseMessage.Initialize(null, Nc);
                newActorPseudoWrongSizeResponseMessage.Serialize();
                newActorPseudoWrongSizeResponseMessage.Send();
                return false;
            }
            if (!Security.check_valid_pseudo(_nameOfNewActor.ToLower()))
            {
                CreateNewActorNameNotAllowedResponseMessage newActorNameNotAllowedResponseMessage = new CreateNewActorNameNotAllowedResponseMessage();
                newActorNameNotAllowedResponseMessage.Initialize(null, Nc);
                newActorNameNotAllowedResponseMessage.Serialize();
                newActorNameNotAllowedResponseMessage.Send();
                return false;
            }

            // verification si le joueurs a attein le nombre maximum des joueurs (5)
            List<mysql.players> player = ((List<mysql.players>)DataBase.DataTables.players).FindAll(f => f.user == _actor.Username);

            if (player.Count == 5)
            {
                CreateNewActorMaxCharactersReachedReponseMessage newActorMaxActorReachedReponseMessage = new CreateNewActorMaxCharactersReachedReponseMessage();
                newActorMaxActorReachedReponseMessage.Initialize(null, Nc);
                newActorMaxActorReachedReponseMessage.Serialize();
                newActorMaxActorReachedReponseMessage.Send();
                return false;
            }

            for (int cnt = 0; cnt < 3; cnt++)
            {
                if (_maskColor[cnt] == "null") continue;
                string[] flag = _maskColor[cnt].Split('-');

                if (flag.Length != 3)
                {
                    // maskColor incorrecte
                    return false;
                }
                for (int cnt2 = 0; cnt2 < 3; cnt2++)
                {
                    int tmpInt;
                    bool result = int.TryParse(flag[cnt2], out tmpInt);

                    if (!result)
                    {
                        // MaskColor incorrect
                        return false;
                    }
                    if (tmpInt < 0 || tmpInt > 255)
                    {
                        // MaskColor incorrect
                        return false;
                    }
                }
                // end for
            }

            return true;
        }

        public void Apply()
        {
            // insertion d'un nouveau champ pour un nouveau player
            mysql.players newPlayer2 = new mysql.players
            {
                pseudo = _nameOfNewActor,
                user = _actor.Username,
                classe = _selectedClass.ToString(),
                hiddenVillage = _selectedHiddenVillage.ToString(),
                maskColorString = _maskColorString,
                level = MainClass.LvlStart,
                maxHEalth = MainClass.StartPdv,
                currentHealth = MainClass.StartPdv,
                spirit = Enums.Spirit.Name.neutral.ToString()
            };
            
            ((List<mysql.players>)DataBase.DataTables.players).Add(newPlayer2);

            CreatedActorSuccessfullyResponseMessage createdActorSuccessfullyResponseMessage = new CreatedActorSuccessfullyResponseMessage();
            createdActorSuccessfullyResponseMessage.Initialize(null, Nc);
            createdActorSuccessfullyResponseMessage.Serialize();
            createdActorSuccessfullyResponseMessage.Send();
        }
    }
}
