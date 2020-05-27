using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;
using System.Drawing;
using System;

namespace MMORPG.Net.Messages.Response
{
    class WayPointNotSameAsMemorisedResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            // WayPointNotSameAsMemorisedResponseMessage•pointX•PointY•position
            // le joueur a demandé un chemain qui débute d'une case difference que la sienne,
            // le serveur répond pour annuler le waypoint et recadrer le joueur sur sa position
            Actor actor = (Actor)CommonCode.MyPlayerInfo.instance.ibPlayer.tag;
            Point truePoint = new Point(Convert.ToInt16(commandStrings[1]) * 30, Convert.ToInt16(commandStrings[2]) * 30);
            actor.directionLook = CommonCode.ConvertToClockWizeOrientation(Convert.ToInt16(commandStrings[3]));
            CommonCode.AdjustPositionAndDirection(CommonCode.MyPlayerInfo.instance.ibPlayer, truePoint);

            if (MMORPG.Battle.state == Enums.battleState.state.idle)
            {

                // annulation du waypoint en attente
                actor.animatedAction = Enums.AnimatedActions.Name.idle;
                actor.wayPoint.Clear();
                actor.realPosition = truePoint;
            }
            else
            {
                MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).directionLook = CommonCode.ConvertToClockWizeOrientation(Convert.ToInt16(commandStrings[3]));
                // annulation du waypoint en attente
                MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).animatedAction = Enums.AnimatedActions.Name.idle;
                MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).wayPoint.Clear();
                MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).realPosition = truePoint;
            }
            #endregion
        }
    }
}
