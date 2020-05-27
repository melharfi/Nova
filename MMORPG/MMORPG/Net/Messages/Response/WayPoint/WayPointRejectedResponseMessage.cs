using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPG.Net.Messages.Response
{
    class WayPointRejectedResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            // le serveur n'approuve pas le déplacement
            CommonCode.ChatMsgFormat("S", "", CommonCode.TranslateText(109));
            Actor actor = (Actor)CommonCode.MyPlayerInfo.instance.ibPlayer.tag;

            // annulation du waypoint en attente
            if (MMORPG.Battle.state == Enums.battleState.state.idle)
            {
                actor.animatedAction = Enums.AnimatedActions.Name.idle;
                actor.wayPoint.Clear();
            }
            else
            {
                MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).animatedAction = Enums.AnimatedActions.Name.idle;
                MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).wayPoint.Clear();
            }
            //il faut jouer un son qui montre que ce n'est pas possible
            #endregion
        }
    }
}