using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    class WayPointConfirmationRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private Actor _actor;
        private List<Point> _wayPointList;
        private string _wayPointString;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)nc.Tag;
            // CommandStrings = ConfirmWaypointRequestMessage•wayPointString ex = 10,8 : 11,8 : 12,8
        }

        public bool Check()
        {
            #region
            if (CommandStrings.Length != 2)
                return false;

            if (_actor.Pseudo == string.Empty || _actor.map == string.Empty)
            {
                Security.User_banne("waypoint fail", Nc);
                return false;
            }

            Battle battle = Battle.Battles.Find(f => f.IdBattle == _actor.idBattle);
            Actor actorInBattleInstance = null;
            if (_actor.inBattle == 1)
                actorInBattleInstance = battle.AllPlayersByOrder.Find(f => f.Pseudo == _actor.Pseudo);

            _wayPointString = CommandStrings[1].ToString();

            // cmd•waypoint•list•540,240:570,240:600,240
            // check si le joueur a déja demandé un nouveau chemain, pour eviter un spam
            // il y à 2 variable qui determine si le joueur est en combat, actorBattleInstance != null et  _actor.inBattle == 1
            // il se peux que l'un sera vrai et pas l'autre, donc ce cas on dois fair un controle pour voir pourquoi ceci est arrivé

            if ((actorInBattleInstance != null && _actor.inBattle == 0) || (actorInBattleInstance == null && _actor.inBattle == 1))
            {
                // impossible d'atteindre ce code puisque l'un des 2 variable de controle nous dis que le joueur est en combat mais pas l'autre se qui est impossible
                // il faut penser a supprimer l'un des 2 si c'est tjr fonctionnel ou laisser pour plus de sureté
            }

            if (actorInBattleInstance != null && _actor.inBattle == 1)
                if (actorInBattleInstance.wayPoint.Count > 0)
                    return false;

            // check si le joueur est en combat en mode initialisation, se qui est interdis pour bouger
            if (_actor.inBattle == 1 && Battle.Battles.Find(f => f.IdBattle == _actor.idBattle).State == Enums.battleState.state.initialisation)
            {
                Battle.Battles.Remove(battle);
                return false;
            }

            // check si le joueur avais demandé un waypoint il ya 2 seconds pour eviter un spam
            if (_actor.timeBeforeNextWaypoint != 0 && CommonCode.ReturnTimeStamp() - _actor.timeBeforeNextWaypoint < 1)
            {
                Console.WriteLine("spaming waypoint");
                return false;
            }
            _actor.timeBeforeNextWaypoint = CommonCode.ReturnTimeStamp();

            // check si le joueur est en combat et a la main
            if (_actor.inBattle == 1)
            {
                if (battle != null)
                {
                    if (battle.AllPlayersByOrder[battle.Turn].Pseudo != _actor.Pseudo)
                    {
                        Console.WriteLine("Error 0x025, le joueur na pas la main et demande un waypoint");
                        return false;
                    }
                }
            }

            // le joueur annonce un déplacement
            _wayPointList = new List<Point>();
            string[] tmpWayPointData = _wayPointString.Split(':');

            // verification du wayPoint qui devera etre plus grand que 0
            if (tmpWayPointData.Length < 1)
            {
                Security.User_banne("waypoint equal zero", Nc);
                return false;
            }

            // création de la liste wayPointList
            foreach (string t in tmpWayPointData)
                try
                {
                    _wayPointList.Add(new Point(Convert.ToInt32(t.Split(',')[0]), Convert.ToInt32(t.Split(',')[1])));
                }
                catch
                {
                    // le joueur essai d'injecter un code qui ne correspoand pas a un waypoint Int
                    Security.User_banne("waypoint type not numbers", Nc);
                    return false;
                }

            // verification si le wayPointList.Count ne depasse pas 100 positions pour eviter une longeurs importante
            if (_wayPointList.Count > GlobalVariable.MaxStepsInWayPoint)
            {
                Security.User_banne("waypoint too long : [" + _wayPointList.Count + "]", Nc);
                return false;
            }

            // check si le joueur est en combat et si oui check si le nombre de case n'est pas supérieur au pm disposé
            if (_actor.inBattle == 1 && _wayPointList.Count > actorInBattleInstance.currentPm)
            {
                // le joueur demande d'utiliser plus de pm qu'il en as
                return false;
            }

            // check si le joueur est ou sera tacler par un autre joueur
            if (_actor.inBattle == 1)
            {
                ///////////////////////////////////////////////////////////
                // check contre le blocage pour la case actuel
                // check si le rectangle en cours est entouré par un joueur ou invoc
                // tourné sur tous les objets et joueur dans la liste
                int cumuleBlocage1 = 0;
                Point[] validePos1 = new Point[4];
                Point curPlayerPos1 = actorInBattleInstance.map_position;
                validePos1[0] = new Point(curPlayerPos1.X, curPlayerPos1.Y - 1);    // case en haut
                validePos1[1] = new Point(curPlayerPos1.X + 1, curPlayerPos1.Y);    // case a droite
                validePos1[2] = new Point(curPlayerPos1.X, curPlayerPos1.Y + 1);    // case en bas
                validePos1[3] = new Point(curPlayerPos1.X - 1, curPlayerPos1.Y);    // case a gauche

                if (battle != null)
                {
                    // check si un joueur se trouve sur l'une des cases cités en haut
                    // this loop is replaced with the LINK-Expression cumuleBlocage1 += (from t in battle.AllPlayersByOrder where t.TeamBattle != actorInBattleInstance.TeamBattle && t.visible let pi = t let p = new Point(t.map_position.X, t.map_position.Y) where (p.X == validePos1[0].X && p.Y == validePos1[0].Y) || (p.X == validePos1[1].X && p.Y == validePos1[1].Y) || (p.X == validePos1[2].X && p.Y == validePos1[2].Y) || (p.X == validePos1[3].X && p.Y == validePos1[3].Y) select pi.blocage).Sum();
                    /*for (int cnt1 = 0; cnt1 < battle.AllPlayersByOrder.Count; cnt1++)
                    {
                        if (battle.AllPlayersByOrder[cnt1].TeamBattle != actorInBattleInstance.TeamBattle && battle.AllPlayersByOrder[cnt1].visible)
                        {
                            Actor pi = battle.AllPlayersByOrder[cnt1];
                            Point p = new Point(battle.AllPlayersByOrder[cnt1].map_position.X, battle.AllPlayersByOrder[cnt1].map_position.Y);
                            if ((p.X == validePos1[0].X && p.Y == validePos1[0].Y) || (p.X == validePos1[1].X && p.Y == validePos1[1].Y) || (p.X == validePos1[2].X && p.Y == validePos1[2].Y) || (p.X == validePos1[3].X && p.Y == validePos1[3].Y))
                                cumuleBlocage1 += pi.blocage;
                        }
                    }*/
                    cumuleBlocage1 += (from t in battle.AllPlayersByOrder where t.teamSide != actorInBattleInstance.teamSide && t.visible let pi = t let p = new Point(t.map_position.X, t.map_position.Y) where (p.X == validePos1[0].X && p.Y == validePos1[0].Y) || (p.X == validePos1[1].X && p.Y == validePos1[1].Y) || (p.X == validePos1[2].X && p.Y == validePos1[2].Y) || (p.X == validePos1[3].X && p.Y == validePos1[3].Y) select pi.blocage).Sum();

                    // on a ajouté une tolerance de 10 pour esquive, a modifier le calcule de esquive ou rendre ce variable globale généré depuis la bdd ou par un fichier ini
                    if (cumuleBlocage1 > actorInBattleInstance.escape + 10)
                    {
                        // le joueur na pas assé de point d'evasion pour esquiver
                        // supprimer tous les autre iteration du tableau qui sont des position blocké par un autre joueur
                        return false;
                    }
                    ///////////////////////////////////////////////////////////
                }
                else
                    Console.WriteLine("what 1");
            }

            // verification de l'attachement des points du waypoint l'un avec l'autre et si ils dépasses les bords du map
            for (int cnt = 0; cnt < _wayPointList.Count; cnt++)
            {
                Point clientPoint;

                if (_actor.inBattle == 1)
                    clientPoint = actorInBattleInstance.map_position;
                else
                    clientPoint = _actor.map_position;

                if (cnt == 0)
                {
                    // 1ere comparaison avec la position d'origine du joueur
                    Point point2 = new Point(_wayPointList[0].X / 30, _wayPointList[0].Y / 30);
                    if (clientPoint.X == point2.X && ((clientPoint.Y > point2.Y) && clientPoint.Y - point2.Y == 1) || (point2.Y > clientPoint.Y && point2.Y - clientPoint.Y == 1))
                    {
                        /* rien a faire tant que le waypoint est correcte*/
                    }
                    else if (clientPoint.Y == point2.Y && ((clientPoint.X > point2.X && clientPoint.X - point2.X == 1) || point2.X > clientPoint.X && point2.X - clientPoint.X == 1))
                    {
                        /* rien a faire tant que le waypoint est correcte*/
                    }
                    else
                    {
                        // le waypoints ne commence pas par la position vrai du joueur, cela arrive quand le joueur est dans une case chez le client mais que sur le serveur il est pas
                        // cette cmd annule le waypoint en cours et met le joueur sur sa position + orientation;
                        object[] param = new object[2];
                        param[0] = clientPoint;
                        param[1] = _actor.directionLook;

                        WayPointNotSameAsMemorisedResponseMessage wayPointNotSameAsMemorisedResponseMessage = new WayPointNotSameAsMemorisedResponseMessage();
                        wayPointNotSameAsMemorisedResponseMessage.Initialize(param, Nc);
                        wayPointNotSameAsMemorisedResponseMessage.Serialize();
                        wayPointNotSameAsMemorisedResponseMessage.Send();
                        return false;
                    }
                }
                else
                {
                    // comparaison entre la position actuel "cnt" et la precedente "cnt - 1"
                    Point point1 = new Point(_wayPointList[cnt - 1].X / 30, _wayPointList[cnt - 1].Y / 30);
                    Point point2 = new Point(_wayPointList[cnt].X / 30, _wayPointList[cnt].Y / 30);

                    if (point1.X == point2.X && ((point1.Y > point2.Y) && point1.Y - point2.Y == 1) || (point2.Y > point1.Y && point2.Y - point1.Y == 1))
                    { }
                    else if (point1.Y == point2.Y && ((point1.X > point2.X && point1.X - point2.X == 1) || point2.X > point1.X && point2.X - point1.X == 1))
                    { }
                    else
                    {
                        // wayPoint Invalide, possibilité de bannir le player ou d'ignorer si c'est un bug non détécté
                        _actor.Disconnect("0x19");
                        return false;
                    }
                }

                // verification si la tuile se se trouve sur une case libre,
                // normalement le client stop le joueur sur la tuile non accessible
                // donc si on arrive la c'est que le client a modifié le code
                if (!_actor.IsFreeCellToWalk(_wayPointList[cnt]))
                {
                    //l'utilisateur a demandé de marcher sur une tuile non libre
                    WayPointRejectedResponseMessage newWayPointRejectedResponseMessage = new WayPointRejectedResponseMessage();
                    newWayPointRejectedResponseMessage.Initialize(null, Nc);
                    newWayPointRejectedResponseMessage.Serialize();
                    newWayPointRejectedResponseMessage.Send();
                    return false;
                }

                //////////////////////////////////////////////
                if (_actor.inBattle == 1)
                {
                    // check si un joueur se trouve sur cette pos, normalement géré par le client a moin que sa soit invisible
                    Point point2 = new Point(_wayPointList[cnt].X / 30, _wayPointList[cnt].Y / 30);
                    for (int cnt2 = 0; cnt2 < battle.AllPlayersByOrder.Count; cnt2++)
                    {
                        // verification des pos de chaque membre avec la pose demandé par le client
                        if (battle.AllPlayersByOrder[cnt2].map_position.X == point2.X && battle.AllPlayersByOrder[cnt2].map_position.Y == point2.Y)
                        {
                            // le client veux marcher sur une case occupé par un joueur
                            WayPointRejectedResponseMessage newWayPointRejectedResponseMessage = new WayPointRejectedResponseMessage();
                            newWayPointRejectedResponseMessage.Initialize(null, Nc);
                            newWayPointRejectedResponseMessage.Serialize();
                            newWayPointRejectedResponseMessage.Send();
                            return false;
                        }
                    }
                }
            }
            return true;
            #endregion
        }

        public void Apply()
        {
            Battle battle = Battle.Battles.Find(f => f.IdBattle == _actor.idBattle);
            Actor actorInBattleInstance = null;
            if (_actor.inBattle == 1)
                actorInBattleInstance = battle.AllPlayersByOrder.Find(f => f.Pseudo == _actor.Pseudo);

            _actor.timeBeforeNextWaypoint = CommonCode.ReturnTimeStamp();
            int blockedPlayerInPos = -1;

            ////////////////////////////////////////
            if (_actor.inBattle == 1)
                for (int cnt = 0; cnt < _wayPointList.Count; cnt++)
                {
                
                    // check contre le blocage
                    // check si le rectangle en cours est entouré par un joueur ou invoc
                    // tourné sur tous les objets et joueur dans la liste
                    Point[] validePos = new Point[4];
                    validePos[0] = new Point(_wayPointList[cnt].X, _wayPointList[cnt].Y - 30);    // case en haut
                    validePos[1] = new Point(_wayPointList[cnt].X + 30, _wayPointList[cnt].Y);    // case a droite
                    validePos[2] = new Point(_wayPointList[cnt].X, _wayPointList[cnt].Y + 30);    // case en bas
                    validePos[3] = new Point(_wayPointList[cnt].X - 30, _wayPointList[cnt].Y);    // case a gauche

                    // code up replaced by Link-Expression
                    int cumuleBlocage = (from t in battle.AllPlayersByOrder where t.teamSide != actorInBattleInstance.teamSide && t.visible let pi = t let p = new Point(t.map_position.X * 30, t.map_position.Y * 30) where (p.X == validePos[0].X && p.Y == validePos[0].Y) || (p.X == validePos[1].X && p.Y == validePos[1].Y) || (p.X == validePos[2].X && p.Y == validePos[2].Y) || (p.X == validePos[3].X && p.Y == validePos[3].Y) select pi.blocage).Sum();

                    if (cumuleBlocage > actorInBattleInstance.escape + 10)
                    {
                        // le joueur na pas assé de point d'evasion pour esquiver
                        // supprimer tous les autre iteration du tableau qui sont des positions blocké par un autre joueur pour lui affecter un nouveau waypoint
                        if (_wayPointList.Count - 1 > cnt)
                        {
                            _wayPointList.RemoveRange(cnt + 1, _wayPointList.Count - (cnt + 1));
                            blockedPlayerInPos = cnt;

                            // réaffectation des waypoints sur le variable cmd[2]
                            _wayPointString = "";
                            foreach (Point t in _wayPointList)
                                _wayPointString += t.X + "," + t.Y + ":";

                            _wayPointString = _wayPointString.Substring(0, _wayPointString.Length - 1);
                        }
                    }
                }
            
            // attribution des données action = running, waypoint = waypointList
            if (_actor.inBattle == 0)
            {
                _actor.wayPoint = _wayPointList;
                _actor.animatedAction = (_wayPointList.Count > GlobalVariable.WalkingMaxSteps) ? Enums.AnimatedActions.Name.run : Enums.AnimatedActions.Name.walk;
            }
            else
            {
                actorInBattleInstance.wayPoint = _wayPointList;
                actorInBattleInstance.animatedAction = (_wayPointList.Count > GlobalVariable.WalkingMaxSteps) ? Enums.AnimatedActions.Name.run : Enums.AnimatedActions.Name.walk;
            }

            List<NetConnection> abonnedClient;

            // informer tous les abonnées au map
            if (_actor.inBattle == 0)
                abonnedClient = MainClass.netServer.Connections.FindAll(f => ((Actor)f.Tag).map == _actor.map && ((Actor)f.Tag).Pseudo != _actor.Pseudo);
            else
                abonnedClient = MainClass.netServer.Connections.FindAll(f => ((Actor)f.Tag).map == _actor.map && ((Actor)f.Tag).Pseudo != _actor.Pseudo && ((Actor)f.Tag).idBattle == _actor.idBattle);

            foreach (NetConnection t in abonnedClient)
            {
                WayPointReplacedResponseMessage wayPointGeneratedResponseMessage = new WayPointReplacedResponseMessage();
                wayPointGeneratedResponseMessage.Initialize(new []{ _actor.Pseudo , _wayPointString }, t);
                wayPointGeneratedResponseMessage.Serialize();
                wayPointGeneratedResponseMessage.Send();

                
            }
            // envoyer au client qui demande de bouger l'accord de son mouvement
            if (_actor.inBattle != 0 && blockedPlayerInPos != -1)
            {
                object[] o = new object[1];
                o[0] = blockedPlayerInPos + 1;

                WayPointBlockedByAnotherActorResponseMessage newWayPointBlockedByAnotherActorResponseMessage = new WayPointBlockedByAnotherActorResponseMessage();
                newWayPointBlockedByAnotherActorResponseMessage.Initialize(o, Nc);
                newWayPointBlockedByAnotherActorResponseMessage.Serialize();
                newWayPointBlockedByAnotherActorResponseMessage.Send();
            }
            else
            {
                WayPointReplacedResponseMessage wayPointReplacedResponseMessage = new WayPointReplacedResponseMessage();
                wayPointReplacedResponseMessage.Initialize(new[] { _actor.Pseudo, _wayPointString }, Nc);
                wayPointReplacedResponseMessage.Serialize();
                wayPointReplacedResponseMessage.Send();
            }

            // orientation du joueur selon le waypoint
            // determination du derniere orientation
            int orientation = 0;

            if (_actor.inBattle == 0)
            {
                if (_actor.map_position.X > (_wayPointList[0].X / 30))
                    orientation = 3;
                else if (_actor.map_position.X < (_wayPointList[0].X / 30))
                    orientation = 1;
                else if (_actor.map_position.Y > (_wayPointList[0].Y / 30))
                    orientation = 0;
                else
                    orientation = 2;
            }
            else
            {
                if (actorInBattleInstance.map_position.X > (_wayPointList[0].X / 30))
                    orientation = 3;
                else if (actorInBattleInstance.map_position.X < (_wayPointList[0].X / 30))
                    orientation = 1;
                else if (actorInBattleInstance.map_position.Y > (_wayPointList[0].Y / 30))
                    orientation = 0;
                else
                    orientation = 2;
            }

            if (_wayPointList.Count > 1)
            {
                for (int cnt = 1; cnt < _wayPointList.Count; cnt++)
                {
                    if (_wayPointList[cnt - 1].X > _wayPointList[cnt].X)
                        orientation = 3;
                    else if (_wayPointList[cnt - 1].X < _wayPointList[cnt].X)
                        orientation = 1;
                    else if (_wayPointList[cnt - 1].Y > _wayPointList[cnt].Y)
                        orientation = 0;
                    else if (_wayPointList[cnt - 1].Y < _wayPointList[cnt].Y)
                        orientation = 2;
                }
            }

            if (_actor.inBattle == 0)
                _actor.directionLook = orientation;
            else
                actorInBattleInstance.directionLook = orientation;

            // changement d'orientation
            if (_actor.inBattle == 0)
                ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _actor.Pseudo).directionLook = (sbyte)orientation;

            // attribution d'un timestamp pour comparer chaque deplacement avec son precedent
            if (_actor.inBattle == 0)
            {
                _actor.wayPointTimeStamp = CommonCode.ReturnTimeStamp();
                _actor.wayPointCnt = _wayPointList.Count;
            }
            else
            {
                actorInBattleInstance.wayPointTimeStamp = CommonCode.ReturnTimeStamp();
                actorInBattleInstance.wayPointCnt = _wayPointList.Count;
            }
        }
    }
}
