using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class WayPointReplacedResponseMessage : IResponseMessage
    {
        private string _actorPseudo;
        private string _wayPointString;         //ex 10,8:10,9 ...10 = x et 8 = y et : séparateur entre les points

        public void Fetch(string[] commandStrings)
        {
            #region
            _actorPseudo = commandStrings[1];
            _wayPointString = commandStrings[2];

            //verification si le joueur en question se trouve dans la liste des joueurs, si non c'est qu'il ya une désynchronisation avec le serveur, donc nous envoyer une cmd de réctification
            if (CommonCode.AllActorsInMap.Exists(f => ((Actor)f.tag).pseudo == _actorPseudo))
            {
                CommonCode.blockNetFlow = true;
                CommonCode.ChatMsgFormat("S", "null", "blockNetFlow8 = true");
                Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => ((Actor)f.tag).pseudo == _actorPseudo);
                string[] tmpWayPointData = _wayPointString.Split(':');
                List<Point> wayPointList = tmpWayPointData.Select(t => new Point(Convert.ToInt32(t.Split(',')[0]), Convert.ToInt32(t.Split(',')[1]))).ToList();

                //mouvement du personnage avec un thread
                if (MMORPG.Battle.state == Enums.battleState.state.idle)
                {
                    ((Actor)ibPlayer.tag).animatedAction = (wayPointList.Count > 5) ? Enums.AnimatedActions.Name.run : Enums.AnimatedActions.Name.walk;
                    ((Actor)ibPlayer.tag).wayPoint = wayPointList;

                    Thread tAnimAction = new Thread(() => CommonCode.AnimAction(ibPlayer, wayPointList, ((wayPointList.Count > 5) ? 20 : 35)));
                    tAnimAction.Start();
                }
                else
                {
                    MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == _actorPseudo).animatedAction = (wayPointList.Count > 5) ? Enums.AnimatedActions.Name.run : Enums.AnimatedActions.Name.walk;
                    MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == _actorPseudo).wayPoint = wayPointList;

                    // s'il s'agit de notre pérsonnage, il faut supprimer les cases qui représente la portés de notre sort
                    if (((Actor)ibPlayer.tag).pseudo == CommonCode.MyPlayerInfo.instance.pseudo)
                    {
                        // effacement de tous les chemain tracés avant
                        if (MMORPG.Battle.state == Enums.battleState.state.started)
                        {
                            for (int cntGfxBgr = Manager.manager.GfxBgrList.Count - 1; cntGfxBgr > 0; cntGfxBgr--)
                            {
                                if (Manager.manager.GfxBgrList[cntGfxBgr].Name() == "__wayPointRec")
                                {
                                    Rec rec = (Rec)Manager.manager.GfxBgrList[cntGfxBgr];
                                    rec.visible = false;
                                    rec.Child.Clear();
                                    Manager.manager.GfxBgrList.RemoveAt(cntGfxBgr);
                                }
                            }
                        }
                    }
                    // decrementation des pm2
                    Thread tAnimAction = new Thread(() => CommonCode.AnimAction(ibPlayer, wayPointList, 20));
                    MMORPG.Battle.AllPlayersByOrder.Find(f => f.pseudo == _actorPseudo).animatedAction = wayPointList.Count > 5 ? Enums.AnimatedActions.Name.run : Enums.AnimatedActions.Name.walk;
                    tAnimAction.Start();
                }
            }
            #endregion
        }
    }
}
