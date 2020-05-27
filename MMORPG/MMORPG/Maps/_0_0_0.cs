using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;
using MMORPG.Net.Messages.Request;

namespace MMORPG.GameStates
{
    public class _0_0_0 : IGameState    //_PosX_PosY_Profondeur
    {
        delegate void DelHandel(string s);   // delegate pour faire du cross threading
        public delegate void DelAnimAction(Bmp bmp, IList<Point> wayPointList, int speed);
        public void Init()
        {
            //////// chaque map dois avoir ce boup de code si son map a un evenemeny
            Bmp map = new Bmp(@"gfx\map\_0_0_0\bg\map.dat", new Point(0, 0), "__map", Manager.TypeGfx.Bgr, true, 1);
            CommonCode.DelWayPointCallBack isFreeCell = _0_0_0.isFreeCellToWalk;
            map.tag = isFreeCell;
            map.MouseMove += CommonCode.map_MouseMove;
            map.MouseOut += DefaultCursor;
            map.MouseClic += CommonCode.map_MouseClic;
            Manager.manager.GfxBgrList.Add(map);
            CommonCode.CurMapFreeCellToSpell = _0_0_0.isFreeCellToSpell;                        // mémoriser la méthode qui check les obstacles du map + les joueurs en combats pour accée a la methode isFreeCellToSpell
            CommonCode.CurMapFreeCellToWalk = _0_0_0.isFreeCellToWalk;                          // // mémoriser la méthode qui check les obstacles du map + les joueurs en combats pour accée a la methode isFreeCellToWalk
            CommonCode.CurMap = "_0_0_0";                                                       // memorisation du nom de la classe pour l'accé du waypoint
            ///////////////////////////////////////////////////////////////////

            if (MainForm.grid)
            {
                Bmp grid = new Bmp(@"gfx\map\grille.dat", new Point(0, 0), "__grille", Manager.TypeGfx.Obj, true);
                grid.zindex = 1000;
                Manager.manager.GfxObjList.Add(grid);
            }

            // affichage du 1er obstacle arbre1.dat
            Bmp __statue_naruto = new Bmp(@"gfx\map\_0_0_0\obj\statue naruto.dat", new Point(30, 30), "__statue_naruto", 0, true, 1);
            __statue_naruto.point = new Point(405, 300 - __statue_naruto.rectangle.Height + 50);
            __statue_naruto.zindex = ((__statue_naruto.point.Y + __statue_naruto.rectangle.Height) / 30) * 100;
            __statue_naruto.TypeGfx = Manager.TypeGfx.Obj;
            Manager.manager.GfxObjList.Add(__statue_naruto);

            // demande des informations des joueurs
            GrabingMapActorsInformationRequestMessage grabingMapInformationRequestMessage = new GrabingMapActorsInformationRequestMessage();
            grabingMapInformationRequestMessage.Serialize();
            grabingMapInformationRequestMessage.Send();

            // demande des obj sur le map
            GrabingMapObjectsInformationRequestMessage grabinMapObjectsInformationRequestMessage =
                new GrabingMapObjectsInformationRequestMessage();
            grabinMapObjectsInformationRequestMessage.Serialize();
            grabinMapObjectsInformationRequestMessage.Send();

            HudHandle.ChatTextBox.Focus();

            // reinitialisation des variable
            CommonCode.blockNetFlow = false;
            CommonCode.ChatMsgFormat("S", "null", "blockNetFlow22 = false");
            Manager.manager.mainForm.Cursor = Cursors.Default;

            // check si le joueur été déjà en combat
            if (CommonCode.MyPlayerInfo.instance.Event == "inBattle")       // ce systeme de DecoReco en combat dois etre implementé da, sma class AutoSelectActorInBattleResponseMessage
            {
                // on recréer l'image du joueur
                CommonCode.RedrawPlayerAfterRespawnInBattle();
                Network.SendMessage("cmd•requireBattleData", true);
            }
        }

        void DefaultCursor(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        public void Network_stat(string stat)
        {
            try
            {
                Manager.manager.mainForm.Invoke(new DelHandel(Handle_Network_Stat), stat);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error 1 " + ex.ToString());
            }
        }
        public void Handle_Network_Stat(string stat)
        {
            string[] cmd = stat.Split('•');

            if (cmd[0] == "internal")
            {
                #region
                if (cmd[1] == "network")
                {
                    if (cmd[2] == "connection")
                    {
                        if (cmd[3] == "failed")
                        {
                            MessageBox.Show(CommonCode.TranslateText(4) + "\n" + cmd[4], "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MainForm.DrawDisconnectImg(true);
                            GameStateManager.ChangeState(new LoginMap());
                            GameStateManager.CheckState();
                        }
                        else if (cmd[3] == "aborted")
                        {
                            MessageBox.Show(CommonCode.TranslateText(2) + "\n" + cmd[4], "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MainForm.DrawDisconnectImg(true);
                            GameStateManager.ChangeState(new LoginMap());
                            GameStateManager.CheckState();
                        }
                    }
                }
                #endregion
            }
        }
        public void Update()
        {

        }
        public void CleanUp()
        {
            Manager.manager.Clear();
        }
        public static bool isFreeCellToWalk(Point p)
        {
            // contient tous les tuiles non accessible sur la map pour le mode marche
            // partie obstacles du map, statue naruto
            if (p.X >= 420 && p.X < 450 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 420 && p.X < 450 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 450 && p.X < 480 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 450 && p.X < 480 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 510 && p.X < 540 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 510 && p.X < 540 && p.Y >= 330 && p.Y < 360)
                return false;
            else
            {
                // verification si le joueur est en combat vus que les participant sont concidérés comme un obstacle aussi
                if (Battle.state == Enums.battleState.state.idle)
                    return true;
                else
                {
                    // recherche parmie les jouers persent s'il coincide avec la meme pose
                    bool found = false;
                    for (int cnt = 0; cnt < Battle.AllPlayersByOrder.Count; cnt++)
                    {
                        Point p2 = Battle.AllPlayersByOrder[cnt].realPosition;
                        p2.X *= 30;
                        p2.Y *= 30;

                        if (p.X >= p2.X && p.X < p2.X + 30 && p.Y >= p2.Y && p.Y < p2.Y + 30)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        return false;
                    else
                        return true;
                }
            }
        }
        public static bool isFreeCellToSpell(Point p)
        {
            // contient tous les tuiles non accessible sur la map pour les sorts
            // partie obstacles du map
            if (p.X >= 420 && p.X < 450 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 420 && p.X < 450 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 450 && p.X < 480 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 450 && p.X < 480 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 480 && p.X < 510 && p.Y >= 330 && p.Y < 360)
                return false;
            else if (p.X >= 510 && p.X < 540 && p.Y >= 300 && p.Y < 330)
                return false;
            else if (p.X >= 510 && p.X < 540 && p.Y >= 330 && p.Y < 360)
                return false;
            else
            {
                // verification si le joueur est en combat vus que les participant sont concidéré comme un obstacle aussi
                if (Battle.state == Enums.battleState.state.idle)
                    return true;
                else
                {
                    // recherche parmie les jouers persent s'il coincide avec la meme pose
                    bool found = false;
                    for (int cnt = 0; cnt < Battle.AllPlayersByOrder.Count; cnt++)
                    {
                        Point p2 = new Point();
                        p2.X = Battle.AllPlayersByOrder[cnt].realPosition.X * 30;
                        p2.Y = Battle.AllPlayersByOrder[cnt].realPosition.Y * 30;

                        if (p.X >= p2.X && p.X < p2.X + 30 && p.Y >= p2.Y && p.Y < p2.Y + 30)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        return false;
                    else
                        return true;
                }
            }
        }
    }
}