using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVER.Enums;

namespace SERVER
{
    class Tactic
    {
        public static List<Point> best_Waypoint(Battle _battle, Actor currentPlayer,Point pointToReach)
        {
            // methode qui retourne le meilleur waypoints vers une position on prenant en considération le bloquage
            // ici est la logique pour les invocations
            // variable de controle pour verifier la présence d'un adversaire acoté de l'invoc
            // cheque si l'invoc est a coté d'un adversaire, si non on recherche
            Point up = new Point(_battle.AllPlayersByOrder[_battle.Turn].map_position.X, _battle.AllPlayersByOrder[_battle.Turn].map_position.Y - 1);
            Point down = new Point(_battle.AllPlayersByOrder[_battle.Turn].map_position.X, _battle.AllPlayersByOrder[_battle.Turn].map_position.Y + 1);
            Point right = new Point(_battle.AllPlayersByOrder[_battle.Turn].map_position.X + 1, _battle.AllPlayersByOrder[_battle.Turn].map_position.Y);
            Point left = new Point(_battle.AllPlayersByOrder[_battle.Turn].map_position.X - 1, _battle.AllPlayersByOrder[_battle.Turn].map_position.Y);
            List<Actor> _cacOpponent = new List<Actor>();

            // variable qui dois contenir le nombre total des points de bloquages qui va permettre de savoir si le joueur va pouvoir se liberer si un autre adversaire se trouve quelque part
            int cacBlocage = 0;
            if (up.Y >= 0 && _battle.AllPlayersByOrder.Exists(f => f.map_position.X == up.X && f.map_position.Y == up.Y && f.teamSide != _battle.AllPlayersByOrder[_battle.Turn].teamSide))
            {
                Actor pi = _battle.AllPlayersByOrder.Find(f => f.map_position.X == up.X && f.map_position.Y == up.Y);
                if (pi != null)
                {
                    cacBlocage += pi.blocage;
                    _cacOpponent.Add(pi);
                }
            }

            if (down.Y < ScreenManager.TileHeight && _battle.AllPlayersByOrder.Exists(f => f.map_position.X == down.X && f.map_position.Y == down.Y && f.teamSide != _battle.AllPlayersByOrder[_battle.Turn].teamSide))
            {
                Actor pi = _battle.AllPlayersByOrder.Find(f => f.map_position.X == down.X && f.map_position.Y == down.Y);
                if (pi != null)
                {
                    cacBlocage += pi.blocage;
                    _cacOpponent.Add(pi);
                }
            }

            if (right.X < ScreenManager.TileWidth && _battle.AllPlayersByOrder.Exists(f => f.map_position.X == right.X && f.map_position.Y == right.Y && f.teamSide != _battle.AllPlayersByOrder[_battle.Turn].teamSide))
            {
                Actor pi = _battle.AllPlayersByOrder.Find(f => f.map_position.X == right.X && f.map_position.Y == right.Y);
                if (pi != null)
                {
                    cacBlocage += pi.blocage;
                    _cacOpponent.Add(pi);
                }
            }

            if (left.X >= 0 && _battle.AllPlayersByOrder.Exists(f => f.map_position.X == left.X && f.map_position.Y == left.Y && f.teamSide != _battle.AllPlayersByOrder[_battle.Turn].teamSide))
            {
                Actor pi = _battle.AllPlayersByOrder.Find(f => f.map_position.X == left.X && f.map_position.Y == left.Y);
                if (pi != null)
                {
                    cacBlocage += pi.blocage;
                    _cacOpponent.Add(pi);
                }
            }

            // recherche si un joueur a été trouvé au cac
            // on recherche une cible s'il y à eu aucun adversaire au cac, ou s'il y à des adversaires au cac mais ils sont pas humains (invoc)
            // on check a la fin si un joueur a été trouvé si non on prend une invoc qui été au cac, found est le variable de controle
            List<Point> wayPointList = new List<Point>();

            if (_cacOpponent.Count == 0 || (_cacOpponent.Count > 0 && _cacOpponent.FindAll(f => f.species == Species.Name.Human).Count == 0 && cacBlocage <= _battle.AllPlayersByOrder[_battle.Turn].escape))
            {
                // aucun joueur na été trouvé au cac, mise en recherche
                // check si l'invoc a plus que 0 pm
                if (_battle.AllPlayersByOrder[_battle.Turn].currentPm > 0)
                {
                    Enums.Team.Side team = _battle.AllPlayersByOrder[_battle.Turn].teamSide;
                    List<FiltrePlayers> fpiiL = new List<FiltrePlayers>();

                    // liste qui dois contenir tous les points qui sont a proximités de notre joueurs
                    List<Actor> piL = (team == Enums.Team.Side.A) ? _battle.SideB : _battle.SideA;
                    List<NearestPoint> tacledPoint = new List<NearestPoint>();      // contiens les points aproximités des joueurs

                    for (int cnt = 0; cnt < piL.Count; cnt++)
                    {
                        Point _up = new Point(piL[cnt].map_position.X, piL[cnt].map_position.Y - 1);
                        Point _down = new Point(piL[cnt].map_position.X, piL[cnt].map_position.Y + 1);
                        Point _right = new Point(piL[cnt].map_position.X + 1, piL[cnt].map_position.Y);
                        Point _left = new Point(piL[cnt].map_position.X - 1, piL[cnt].map_position.Y);

                        // on check si cette cases est occupé déja par un joueur
                        if (!_battle.AllPlayersByOrder.Exists(f => f.map_position.X == _up.X && f.map_position.Y == _up.Y))
                        {
                            NearestPoint np_up = new NearestPoint();
                            np_up.index = piL[cnt].blocage;
                            np_up.point = _up;
                            np_up.actor = piL[cnt];
                            tacledPoint.Add(np_up);
                        }
                        if (!_battle.AllPlayersByOrder.Exists(f => f.map_position.X == _down.X && f.map_position.Y == _down.Y))
                        {
                            NearestPoint np_down = new NearestPoint();
                            np_down.index = piL[cnt].blocage;
                            np_down.point = _down;
                            np_down.actor = piL[cnt];
                            tacledPoint.Add(np_down);
                        }
                        if (!_battle.AllPlayersByOrder.Exists(f => f.map_position.X == _right.X && f.map_position.Y == _right.Y))
                        {
                            NearestPoint np_right = new NearestPoint();
                            np_right.index = piL[cnt].blocage;
                            np_right.point = _right;
                            np_right.actor = piL[cnt];
                            tacledPoint.Add(np_right);
                        }
                        if (!_battle.AllPlayersByOrder.Exists(f => f.map_position.X == _left.X && f.map_position.Y == _left.Y))
                        {
                            NearestPoint np_left = new NearestPoint();
                            np_left.index = piL[cnt].blocage;
                            np_left.point = _left;
                            np_left.actor = piL[cnt];
                            tacledPoint.Add(np_left);
                        }
                    }

                    /////////////// pathfinding   ////////////////////
                    
                    MELHARFI.AStarAlgo.MapPoint startPoint = new MELHARFI.AStarAlgo.MapPoint(_battle.AllPlayersByOrder[_battle.Turn].map_position.X, _battle.AllPlayersByOrder[_battle.Turn].map_position.Y);
                    MELHARFI.AStarAlgo.MapPoint endPoint = new MELHARFI.AStarAlgo.MapPoint(pointToReach.X, pointToReach.Y);
                    Actor target2 = _battle.AllPlayersByOrder.Find(f => f.map_position.X == endPoint.X && f.map_position.Y == endPoint.Y);
                    if (target2 == null)
                    {
                        // si on arrive ici c'est que le joueur n'est pas synchro avec le serveur
                        // le joueur essai de frapper un adversaire qui est existant chez le client mais pas sur le serveur, a voir le client
                        Console.WriteLine("team1 has " + _battle.SideA.Count + " team2 has " + _battle.SideB.Count);
                        Console.WriteLine("can't reach that code unless if client is not sync with server, no player was found to hit");
                        /*finishTurn(_battle, true);
                        return;*/
                    }

                    byte[,] byteMap = new byte[ScreenManager.TileWidth, ScreenManager.TileHeight];
                    for (int i = 0; i < ScreenManager.TileWidth; i++)
                    {
                        for (int j = 0; j < ScreenManager.TileHeight; j++)
                        {
                            if (!_battle.IsFreeCellToWalk(new Point(i * 30, j * 30)) || _battle.AllPlayersByOrder.Exists(f => f.map_position.X == i && f.map_position.Y == j && f.Pseudo != _battle.AllPlayersByOrder[_battle.Turn].Pseudo && (target2 != null && f.Pseudo != target2.Pseudo)))
                                byteMap[i, j] = 3;
                            else if (tacledPoint.FindAll(f => f.point.X == i && f.point.Y == j && f.actor.Pseudo != _battle.AllPlayersByOrder[_battle.Turn].Pseudo && f.actor.Pseudo != target2.Pseudo).Count > 0)
                            {
                                // il ya plusieurs cases qui sont affecté par un joueur, on calcule le nombre de point de blocage
                                int npTotal = 0;
                                List<NearestPoint> np = tacledPoint.FindAll(f => f.point.X == i && f.point.Y == j && f.actor.Pseudo != _battle.AllPlayersByOrder[_battle.Turn].Pseudo && f.actor.Pseudo != target2.Pseudo);
                                for (int cnt3 = 0; cnt3 < np.Count; cnt3++)
                                    npTotal += np[cnt3].actor.blocage;

                                if (npTotal > _battle.AllPlayersByOrder[_battle.Turn].blocage)
                                    byteMap[i, j] = 3;
                                else
                                    byteMap[i, j] = 0;
                            }
                            else
                                byteMap[i, j] = 0;
                        }
                    }
                    // mise en obstacle de tous les cases qui devais tacler l'invoc lorsqu'il passe a coté

                    MELHARFI.AStarAlgo.Map _map = new MELHARFI.AStarAlgo.Map(ScreenManager.TileWidth, ScreenManager.TileHeight, startPoint, endPoint, byteMap);

                    if (_map != null && _map.StartPoint != MELHARFI.AStarAlgo.MapPoint.InvalidPoint && _map.EndPoint != MELHARFI.AStarAlgo.MapPoint.InvalidPoint)
                    {
                        MELHARFI.AStarAlgo.AStar astart = new MELHARFI.AStarAlgo.AStar(_map);
                        List<MELHARFI.AStarAlgo.MapPoint> sol = astart.CalculateBestPath();
                        if (sol != null)
                            sol.Reverse();
                        else
                        {
                            // impossible de determiner le chemain, peux etre que la case ciblé est un obstacle
                        }
                        // conversion de la liste MapPoint a une liste Point
                        for (int i = 0; i < sol.Count; i++)
                            wayPointList.Add(new Point(sol[i].X * 30, sol[i].Y * 30));
                    }

                    // soustraction de la derniere case si elle correspand a la case ciblé, puisque le joueur ne peux pas occuper une case déja occupé
                    if (wayPointList.Count > 0)
                        if (wayPointList[wayPointList.Count - 1].X == endPoint.X * 30 && wayPointList[wayPointList.Count - 1].Y == endPoint.Y * 30)
                            wayPointList.RemoveAt(wayPointList.Count - 1);

                }
                    ////////////////////////////
            }

            return wayPointList;
        }
        public static List<Actor> oppenent_By_Lower_Pdv( Battle _battle, Enums.Team.Side team_To_Seek_For_Oppenents)
        {
            // classement des adversaires selon leurs pdv
            List<Actor> oppenent_Ordered_By_Lower_Pdv = new List<Actor>();
            if(team_To_Seek_For_Oppenents == Enums.Team.Side.A)
                for (int cnt=0; cnt < _battle.SideA.Count; cnt++)
                    oppenent_Ordered_By_Lower_Pdv.Add(_battle.SideA[cnt]);
            else
                for (int cnt = 0; cnt < _battle.SideB.Count; cnt++)
                    oppenent_Ordered_By_Lower_Pdv.Add(_battle.SideB[cnt]);

            oppenent_Ordered_By_Lower_Pdv.OrderBy(f => f.currentHealth);

            return oppenent_Ordered_By_Lower_Pdv;
        }
        public static string is_Spell_Allowed_From_Current_Pos(Battle _battle, Actor playerTargeted, Actor pi, int spellID)
        {
            // on vérifie selon le sort, si c'est un cac ou à distance
            if (spellID == 4)
            {
                // sort qui se lance au cac
                // on vérifie si l'adversaire en question est au cac, si non on vérifie si on peux l'atteindre
                if (((pi.map_position.X == playerTargeted.map_position.X + 1 || pi.map_position.X == playerTargeted.map_position.X - 1) && pi.map_position.Y == playerTargeted.map_position.Y) || ((pi.map_position.Y == playerTargeted.map_position.Y + 1 || pi.map_position.Y == playerTargeted.map_position.Y - 1) && (pi.map_position.X == playerTargeted.map_position.X)))
                {
                    // on vérifie si le sort en question peux etre lancé avec les envoutement
                    if (pi.BuffsList.Exists(f => f.SortID == spellID && f.system))
                    {
                        Actor.Buff piEnv = pi.BuffsList.Find(f => f.SortID == spellID && f.system);
                        Actor.SpellsInformations piis = pi.sorts.Find(f => f.SpellId == spellID);
                        mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == piis.Level - 1);
                        if (piEnv.relanceInterval < spell.relanceInterval)
                        {
                            // check contre relanceInterval si il a déja été lance dans un tours précédent, d'où le relanceInterval ne dois pas égale celui attribué la 1er fois, pour laisser le joueur lancer le sort plusieurs fois le tour actuel
                            return "spellIntervalNotReached";
                        }
                        else if (spell.relanceParTour > 0 /*&& playerTargeted != null*/ && piEnv.playerRoxed.Count >= spell.relanceParTour)
                        {
                            // check contre le max de ralance par tour "relanceParTour", si = 0 donc ilimité
                            return "spellRelanceParTourReached";
                        }
                        else if (spell.relanceParJoueur > 0 && playerTargeted != null && piEnv.playerRoxed.FindAll(f => f == playerTargeted.Pseudo).Count >= spell.relanceParJoueur)
                        {
                            // check contre relanceParJoueur s'il a déja été lancé sur le même adversaire, si = 0 donc illimité
                            return "spellRelanceParJoueurReached";
                        }
                    }
                    return "allowed";
                }
                else
                    return "denied";
            }
            return "denied";
        }
        
    }
}
