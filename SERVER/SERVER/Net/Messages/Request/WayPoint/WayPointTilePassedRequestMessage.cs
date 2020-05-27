using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace SERVER.Net.Messages.Request
{
    class WayPointTilePassedRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private Actor _actor;
        private string _locationString;
        private Point _location;

        // lorsque le joueurs "client" avance, il envoie cette cmd pour indiquer qu'il a passer une tuile
        // a fin de décrémonter le wayPointList, et de comparer le timeStamp entre chaque mouvement
        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)nc.Tag;
        }

        public bool Check()
        {
            Battle b = Battle.Battles.Find(f => f.IdBattle == _actor.idBattle);
            Actor actorBattleInstance = null;
            if (_actor.inBattle == 1)
                actorBattleInstance = b.AllPlayersByOrder.Find(f => f.Pseudo == _actor.Pseudo);

            if (CommandStrings.Length != 2)
                return false;

            _locationString = CommandStrings[1].ToString();

            // incrementation du timestamp contre le spam waypoint qui se libere apres 2seconds
            _actor.timeBeforeNextWaypoint = CommonCode.ReturnTimeStamp();

            // verifier si le joueur a un wayPointList
            if ((_actor.inBattle == 0 && _actor.wayPoint.Count > 0) || (_actor.inBattle == 1 && actorBattleInstance.wayPoint.Count > 0))
            {
                int x, y;
                if (!int.TryParse(_locationString.Split(',')[0], out x))
                {
                    Security.User_banne("client inject a non int 'waypoint past'", Nc);
                    return false;
                }

                if (!int.TryParse(_locationString.Split(',')[1], out y))
                {
                    Security.User_banne("client inject a non int 'waypoint past'", Nc);
                    return false;
                }

                _location = new Point(x, y);

                // arrondissement
                Point p = new Point(_location.X / 30 * 30, _location.Y / 30 * 30);

                // verification si c'est la derniere offset pour mettre a jour la position du joueur
                if ((_actor.inBattle == 0 && p.X == _actor.wayPoint[0].X && p.Y == _actor.wayPoint[0].Y) || (_actor.inBattle == 1 && p.X == actorBattleInstance.wayPoint[0].X && p.Y == actorBattleInstance.wayPoint[0].Y))
                {
                    // modification du way point sur la bd seulement si le joueur n'est pas dans un combat
                    if (_actor.inBattle == 0)
                    {
                        ((List<mysql.connected>)DataBase.DataTables.connected).Find(f => f.pseudo == _actor.Pseudo).map_position = (_actor.wayPoint[0].X / 30) + "/" + (_actor.wayPoint[0].Y / 30);
                        ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _actor.Pseudo).map_position = (_actor.wayPoint[0].X / 30) + "/" + (_actor.wayPoint[0].Y / 30);
                    }
                    else
                    {
                        // sustraction de 1 du nombre des pm2
                        actorBattleInstance.currentPm--;
                    }

                    if (_actor.inBattle == 0)
                    {
                        _actor.map_position = new Point((_actor.wayPoint[0].X / 30), (_actor.wayPoint[0].Y / 30));
                        _actor.wayPoint.RemoveAt(0);
                    }
                    else
                    {
                        actorBattleInstance.map_position = new Point((actorBattleInstance.wayPoint[0].X / 30), (actorBattleInstance.wayPoint[0].Y / 30));
                        actorBattleInstance.wayPoint.RemoveAt(0);
                    }

                    // supression de la 1ere position depuis le way point
                    // verification du temps passé apres le passage du waypoint
                    if (_actor.inBattle == 0)
                    {
                        switch (_actor.wayPoint.Count)
                        {
                            case 0:
                                if (_actor.wayPointCnt > 1)
                                {
                                    int elapsedTime = CommonCode.ReturnTimeStamp() - _actor.wayPointTimeStamp;

                                    if (_actor.animatedAction == Enums.AnimatedActions.Name.walk)
                                    {
                                        //double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
                                        int trueTime = (int)(Math.Round(_actor.wayPointCnt * 0.2));

                                        if (elapsedTime - trueTime < -1)
                                        {
                                            // seuille de 5 seconds tolléré (1 = 0.2 * 5 seconds)
                                            // le joueur a triché en diminuons le temps de relance pour chaque tuile passé
                                            Security.User_banne("time too short between 2 tile", Nc);
                                            //Nc.Disconnect("0x17");
                                            return false;
                                        }
                                    }
                                    else if (_actor.animatedAction == Enums.AnimatedActions.Name.run)
                                    {
                                        //double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
                                        int trueTime = (int)(Math.Round(_actor.wayPointCnt * 0.2));

                                        if (elapsedTime - trueTime < ((_actor.wayPointCnt < 10) ? -1 : -3))
                                        {
                                            // le joueur a triché en diminuons le temps de relance pour chaque tuille passé
                                            Security.User_banne("time too short between 2 tile", Nc);
                                            return false;
                                        }
                                    }
                                }

                                _actor.wayPointTimeStamp = 0;
                                _actor.animatedAction = Enums.AnimatedActions.Name.idle;
                                _actor.wayPoint.Clear();
                                _actor.wayPointCnt = 0;
                                break;
                        }
                    }
                    else
                    {
                        switch (actorBattleInstance.wayPoint.Count)
                        {
                            case 0:
                                if (actorBattleInstance.wayPointCnt > 1)
                                {
                                    int elapsedTime = CommonCode.ReturnTimeStamp() - actorBattleInstance.wayPointTimeStamp;

                                    switch (actorBattleInstance.animatedAction)
                                    {
                                        case Enums.AnimatedActions.Name.walk:
                                        {
                                            //double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
                                            int trueTime = (int)(Math.Round(actorBattleInstance.wayPointCnt * 0.2));

                                            if (elapsedTime - trueTime < -1)
                                            {
                                                // seuille de 5 seconds toléré
                                                // le joueur a triché en diminuons le temps de relance pour chaque tuille passé
                                                Security.User_banne("too short time between 2 tiles, err2", Nc);
                                                return false;
                                            }
                                        }
                                            break;
                                        case Enums.AnimatedActions.Name.run:
                                        {
                                            //double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
                                            int trueTime = (int)(Math.Round(actorBattleInstance.wayPointCnt * 0.2));

                                            if (elapsedTime - trueTime < ((actorBattleInstance.wayPointCnt < 10) ? -1 : -3))
                                            {
                                                // le joueur a triché en diminuons le temps de relance pour chaque tuille passé
                                                Security.User_banne("too short time between 2 tiles, err3", Nc);
                                                return false;
                                            }
                                        }
                                            break;
                                    }
                                }

                                actorBattleInstance.wayPointTimeStamp = 0;
                                actorBattleInstance.animatedAction = Enums.AnimatedActions.Name.idle;
                                actorBattleInstance.wayPoint.Clear();
                                actorBattleInstance.wayPointCnt = 0;
                                break;
                        }
                    }
                }
            }
            else
            {
                // cette condition n'est pas possible vus que le client devera avoir un waypoint > 0
                // mais pour des raisons de sécurité on le bloque si ce cas arrive
                return false;
            }

            return true;
        }

        public void Apply()
        {
            /*Battle b = Battle.Battles.Find(f => f.IdBattle == _actor.idBattle);
            Actor actorBattleInstance = null;
            if (_actor.inBattle == 1)
                actorBattleInstance = b.AllPlayersByOrder.Find(f => f.Pseudo == _actor.Pseudo);

            Point p = new Point(_location.X / 30 * 30, _location.Y / 30 * 30);

            // verification si c'est la derniere offset pour mettre a jour la position du joueur
            if ((_actor.inBattle == 0 && p.X == _actor.wayPoint[0].X && p.Y == _actor.wayPoint[0].Y) || (_actor.inBattle == 1 && p.X == actorBattleInstance.wayPoint[0].X && p.Y == actorBattleInstance.wayPoint[0].Y))
            {
                // modification du way point sur la bd seulement si le joueur n'est pas dans un combat
                if (_actor.inBattle == 0)
                {
                    ((List<mysql.connected>)DataBase.DataTables.connected).Find(f => f.pseudo == _actor.Pseudo).map_position = (_actor.wayPoint[0].X / 30) + "/" + (_actor.wayPoint[0].Y / 30);
                    ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _actor.Pseudo).map_position = (_actor.wayPoint[0].X / 30) + "/" + (_actor.wayPoint[0].Y / 30);
                }
                else
                {
                    // sustraction de 1 du nombre des pm2
                    actorBattleInstance.current_Pm--;
                }

                if (_actor.inBattle == 0)
                {
                    _actor.map_position = new Point((_actor.wayPoint[0].X / 30), (_actor.wayPoint[0].Y / 30));
                    _actor.wayPoint.RemoveAt(0);
                }
                else
                {
                    actorBattleInstance.map_position = new Point((actorBattleInstance.wayPoint[0].X / 30), (actorBattleInstance.wayPoint[0].Y / 30));
                    actorBattleInstance.wayPoint.RemoveAt(0);
                }

                // supression de la 1ere position depuis le way point
                // verification du temps passé apres le passage du waypoint
                if (_actor.inBattle == 0)
                {
                    switch (_actor.wayPoint.Count)
                    {
                        case 0:
                            if (_actor.wayPointCnt > 1)
                            {
                                int elapsedTime = CommonCode.ReturnTimeStamp() - _actor.wayPointTimeStamp;

                                if (_actor.animatedAction == Enums.AnimatedActions.Name.walk)
                                {
                                    //double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
                                    int trueTime = (int)(Math.Round(_actor.wayPointCnt * 0.2));

                                    if (elapsedTime - trueTime < -1)
                                    {
                                        // seuille de 5 seconds tolléré (1 = 0.2 * 5 seconds)
                                        // le joueur a triché en diminuons le temps de relance pour chaque tuile passé
                                        Security.User_banne("time too short between 2 tile", Nc);
                                        //Nc.Disconnect("0x17");
                                        return false;
                                    }
                                }
                                else if (_actor.animatedAction == Enums.AnimatedActions.Name.run)
                                {
                                    //double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
                                    int trueTime = (int)(Math.Round(_actor.wayPointCnt * 0.2));

                                    if (elapsedTime - trueTime < ((_actor.wayPointCnt < 10) ? -1 : -3))
                                    {
                                        // le joueur a triché en diminuons le temps de relance pour chaque tuille passé
                                        Security.User_banne("time too short between 2 tile", Nc);
                                        return false;
                                    }
                                }
                            }

                            _actor.wayPointTimeStamp = 0;
                            _actor.animatedAction = Enums.AnimatedActions.Name.idle;
                            _actor.wayPoint.Clear();
                            _actor.wayPointCnt = 0;
                            break;
                    }
                }
                else
                {
                    switch (actorBattleInstance.wayPoint.Count)
                    {
                        case 0:
                            if (actorBattleInstance.wayPointCnt > 1)
                            {
                                int elapsedTime = CommonCode.ReturnTimeStamp() - actorBattleInstance.wayPointTimeStamp;

                                switch (actorBattleInstance.animatedAction)
                                {
                                    case Enums.AnimatedActions.Name.walk:
                                        {
                                            //double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
                                            int trueTime = (int)(Math.Round(actorBattleInstance.wayPointCnt * 0.2));

                                            if (elapsedTime - trueTime < -1)
                                            {
                                                // seuille de 5 seconds toléré
                                                // le joueur a triché en diminuons le temps de relance pour chaque tuille passé
                                                Security.User_banne("too short time between 2 tiles, err2", Nc);
                                                return false;
                                            }
                                        }
                                        break;
                                    case Enums.AnimatedActions.Name.run:
                                        {
                                            //double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
                                            int trueTime = (int)(Math.Round(actorBattleInstance.wayPointCnt * 0.2));

                                            if (elapsedTime - trueTime < ((actorBattleInstance.wayPointCnt < 10) ? -1 : -3))
                                            {
                                                // le joueur a triché en diminuons le temps de relance pour chaque tuille passé
                                                Security.User_banne("too short time between 2 tiles, err3", Nc);
                                                return false;
                                            }
                                        }
                                        break;
                                }
                            }

                            actorBattleInstance.wayPointTimeStamp = 0;
                            actorBattleInstance.animatedAction = Enums.AnimatedActions.Name.idle;
                            actorBattleInstance.wayPoint.Clear();
                            actorBattleInstance.wayPointCnt = 0;
                            break;
                    }
                }
            }*/
        }
    }
}
