using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace SERVER.Net.Messages.Request
{
    internal class DuelConfirmationRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private Actor _actor;
        private Actor _actorChallenged;
        private NetConnection ncChallenged;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            Nc = nc;
            CommandStrings = commandStrings;
            _actor = (Actor)Nc.Tag;
            string _actorChallengedName = commandStrings[1].ToString();
            ncChallenged = MainClass.netServer.Connections.Find(f => ((Actor)f.Tag).Pseudo == _actorChallengedName);
            _actorChallenged = ncChallenged.Tag as Actor;
            
        }

        public bool Check()
        {
            if (_actor.Pseudo == "" || _actor.map == "" || _actor.inBattle == 1)
            {
                Security.User_banne("challenge", Nc);
                return false;
            }

            // verification si le joueur existe
            if (ncChallenged != null && _actorChallenged.IgnoredPlayersChallenge.IndexOf(_actor.Pseudo) == -1)
            {
                // verification si les 2 joueurs sont dans la meme map
                if (_actor.map != _actorChallenged.map)
                {
                    Console.WriteLine("error# 0x025 : client " + _actor.Pseudo + " challenge player " + _actorChallenged + " in different map");
                    return false;
                }

                if (_actor.inBattle == 0 && _actorChallenged.inBattle == 0)
                {
                    // verification si les 2 joueunts ne sont pas en attente ou demande un défie d'une autre personne
                    if (_actor.YouChallengePlayer == "" && _actorChallenged.PlayerChallengeYou == "" && _actor.PlayerChallengeYou == "" && _actorChallenged.YouChallengePlayer == "")
                    {
                        return true;
                    }
                    else
                    {
                        // sois celui demandé en défie est occupé sois le demandeur triche en envoyons une demande de défie alors qu'il est déja entrain de defier ou demande un autre défie
                        CommonCode.SendMessage("cmd•playerBusyToChallengeYou", Nc, true);
                        Console.WriteLine("<--cmd•playerBusyToChallengeYou");
                    }
                }

                return false;
            }
            else
                return false;
        }

        public void Apply()
        {
            // les 2 utilisateurs sont libre
            // demander au jouer défié si il accepte le défie ou pas
            CommonCode.SendMessage("cmd•askingToChallenge•" + _actor.Pseudo, ncChallenged, true);
            Console.WriteLine("<--cmd•askingToChallenge to " + _actor.Pseudo);
            (ncChallenged.Tag as Actor).PlayerChallengeYou = _actor.Pseudo;

            CommonCode.SendMessage("cmd•waitingToChallenge•" + _actorChallenged.Pseudo, Nc, true);
            Console.WriteLine("<--cmd•waitingToChallenge•" + _actorChallenged.Pseudo);
            (Nc.Tag as Actor).YouChallengePlayer = _actorChallenged.Pseudo;

        }
    }
}
