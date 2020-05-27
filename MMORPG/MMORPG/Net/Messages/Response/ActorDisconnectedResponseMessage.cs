using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class ActorDisconnectedResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            // deconnexion du joueur
            // suppression de ibPlayer si on ai pas en combat

            string _actor = commandStrings[1];

            if (MMORPG.Battle.state == Enums.battleState.state.idle)
            {
                Bmp allPlayers = CommonCode.AllActorsInMap.Find(f => ((Actor)f.tag).pseudo == _actor);
                if (allPlayers != null)
                {
                    allPlayers.visible = false;
                    CommonCode.AllActorsInMap.Remove(allPlayers);
                }
            }
            else
            {
                CommonCode.ChatMsgFormat("S", "null", _actor + " " + CommonCode.TranslateText(119));
            }

            //check si nous avons recus une demande de combat ou c'est nous qui ont envoyé cette demande
            if (CommonCode.ChallengeTo == _actor)
            {
                if (CommonCode.annulerChallengeMeDlg != null)
                    CommonCode.CancelChallengeAsking(_actor);
                else
                    CommonCode.CancelChallengeRespond(_actor);
            }
            #endregion
        }
    }
}
