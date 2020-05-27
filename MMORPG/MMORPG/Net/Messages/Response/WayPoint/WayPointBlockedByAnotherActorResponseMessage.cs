using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class WayPointBlockedByAnotherActorResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            // le serveur na pas autorisé notre waypoint parsqu'il est bloqué par unjoueur mais il a renvoyé un waypoint jusqu'a l'obstacle
            int blockedPlayerInPos = int.Parse(commandStrings[1]);
            // decrementation du waypoint jusqu'a la pos valide
            MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).wayPoint.RemoveRange(Convert.ToInt32(blockedPlayerInPos), MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).wayPoint.Count - Convert.ToInt32(blockedPlayerInPos));

            // effacement de tous les chemain tracés avant
            List<IGfx> bgrL = Manager.manager.GfxBgrList.FindAll(f => f.Name() == "__wayPointRec");
            for (int cntGfxBgr = bgrL.Count - 1; cntGfxBgr > 0; cntGfxBgr--)
            {
                ((Rec)bgrL[cntGfxBgr]).visible = false;
                ((Rec)bgrL[cntGfxBgr]).Child.Clear();
                Manager.manager.GfxBgrList.Remove(bgrL[cntGfxBgr]);
            }

            Thread tAnimAction = new Thread(() => CommonCode.AnimAction(CommonCode.MyPlayerInfo.instance.ibPlayer, MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).wayPoint, 20));
            MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).animatedAction = Enums.AnimatedActions.Name.run;
            tAnimAction.Start();
            #endregion
        }
    }
}
