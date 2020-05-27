using System;
using System.Drawing;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class WayPointInteruptedByActorResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            string actorName = commandStrings[1];
            Point newLocation = new Point(int.Parse(commandStrings[2].Split(',')[0]), int.Parse(commandStrings[2].Split(',')[1]));
            
            #region
            // le serveur nous informe qu'un client s'est arrété de son waypoint
            // reinitialisation des infos de mouvement des clients
            if (CommonCode.AllActorsInMap.Exists(i => ((Actor)i.tag).pseudo == actorName))
            {
                Bmp stopedPlayer = CommonCode.AllActorsInMap.Find(i => ((Actor)i.tag).pseudo == actorName);
                Actor stopedPlayerActor = (Actor) stopedPlayer.tag;

                // il faut arreter le thread responsable du waypoint
                CommonCode.abortAnimActionThread = true;

                // remise en place du joueur
                Point p = new Point(newLocation.X * 30, newLocation.Y * 30);
                CommonCode.AdjustPositionAndDirection(stopedPlayer, p);

                // reinitialisations des données de mouvement
                stopedPlayerActor.animatedAction = Enums.AnimatedActions.Name.idle;
                stopedPlayerActor.wayPoint.Clear();
                stopedPlayerActor.realPosition = p;
            }
            #endregion
        }
    }
}
