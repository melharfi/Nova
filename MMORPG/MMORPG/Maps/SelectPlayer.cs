using System;
using System.Drawing;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;
using MMORPG.Net.Messages.Request;

namespace MMORPG.GameStates
{
    public class SelectPlayer : IGameState
    {
        public static Txt[] lPseudo = new Txt[5], lLvlPlayer = new Txt[5], lLvlSpirit = new Txt[5];
        public static Txt secretQuestion;
        public static Bmp[] ibPlayers = new Bmp[5], terrain = new Bmp[5], delete = new Bmp[5], village = new Bmp[5];
        Bmp ibCreatePlayer;
        Rec rec1;
        TextBox secretQuestionTB;
        Button annulerBtn, envoyerBtn;
        delegate void DelHandel(string s);
        string selectedDeletPlayer = "";

        public SelectPlayer()
        {
            
        }

        #region IGameState Members
        public void Init()
        {
            Bmp map = new Bmp(@"gfx\map\SelectPlayer\bg\1.dat", new Point(0, 0), "map1", Manager.TypeGfx.Bgr, true, 1);
            map.MouseMove += CommonCode.CursorDefault_MouseOut;
            Manager.manager.GfxBgrList.Add(map);
            
            // mise en cache des element du menu hud s'il a été préalablement affiché
            IGfx __StatsImg = Manager.manager.GfxTopList.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).name == "__StatsImg");
            if (__StatsImg != null && (__StatsImg as Bmp).visible)
                (__StatsImg as Bmp).visible = false;

            for (int cnt = 0; cnt < 5; cnt++)
            {
                terrain[cnt] = new Bmp(@"gfx\map\SelectPlayer\obj\2.dat", new Point(0, 0), "terrain[" + cnt + "]", Manager.TypeGfx.Obj, true, 1);
                Manager.manager.GfxObjList.Add(terrain[cnt]);

                lPseudo[cnt] = new Txt(string.Empty, new Point(0, 0), "lPseudo[" + cnt + "]", Manager.TypeGfx.Obj, true, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                lLvlPlayer[cnt] = new Txt(string.Empty, new Point(0, 0), "lLvlPlayer[" + cnt + "]", Manager.TypeGfx.Obj, true, new Font("Verdana", 10, FontStyle.Regular), Brushes.CadetBlue);

                // image du personnage
                ibPlayers[cnt] = new Bmp
                {
                    point = new Point(0, 0),
                    name = "ibPlayers[" + cnt + "]",
                    zindex = ZOrder.Obj(),
                    visible = true
                };
                ibPlayers[cnt].MouseDoubleClic += SelectPlayer_MouseDoubleClic;
                ibPlayers[cnt].MouseMove += CommonCode.CursorHand_MouseMove;
                ibPlayers[cnt].MouseOut += CommonCode.CursorDefault_MouseOut;
                ibPlayers[cnt].TypeGfx = Manager.TypeGfx.Obj;
                ibPlayers[cnt].Crypted = true;

                delete[cnt] = new Bmp(@"gfx\map\SelectPlayer\obj\delete.dat", new Point(0, 0), "delete." + cnt, Manager.TypeGfx.Obj, false, 1);
                delete[cnt].MouseOver += CommonCode.CursorHand_MouseMove;
                delete[cnt].MouseOut += CommonCode.CursorDefault_MouseOut;
                delete[cnt].MouseClic += Delete_Player_MouseClic;
                ibPlayers[cnt].Child.Add(delete[cnt]);

                lLvlSpirit[cnt] = new Txt(string.Empty, new Point(0, 0), "lLvlSpirit[" + cnt + "]", Manager.TypeGfx.Obj, true, new Font("Verdana", 10, FontStyle.Bold), Brushes.Red);
            }

            //// ombre
            terrain[0].point = new Point(172, 210);
            terrain[1].point = new Point(316, 310);
            terrain[2].point = new Point(460, 390);
            terrain[3].point = new Point(598, 310);
            terrain[4].point = new Point(720, 212);

            //// imagebox player
            ibPlayers[0].point = new Point(209, 170);
            ibPlayers[1].point = new Point(352, 268);
            ibPlayers[2].point = new Point(498, 350);
            ibPlayers[3].point = new Point(637, 270);
            ibPlayers[4].point = new Point(757, 174);

            ibCreatePlayer = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(434, 466), "ibCreatePlayer", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 9));
            ibCreatePlayer.MouseDown += ibRetour_MouseDown;
            ibCreatePlayer.MouseUp += ibRetour_MouseUp;
            ibCreatePlayer.MouseOver += CommonCode.CursorHand_MouseMove;
            ibCreatePlayer.MouseOut += CommonCode.CursorDefault_MouseOut;
            ibCreatePlayer.EscapeGfxWhileMouseUp = true;
            Manager.manager.GfxObjList.Add(ibCreatePlayer);

            Txt CreatePlayer = new Txt(CommonCode.TranslateText(24), new Point(378, 470), "__CreatePlayer", Manager.TypeGfx.Obj, true, new Font("Verdana", 10, FontStyle.Regular), Brushes.Yellow);
            CreatePlayer.point.X = ibCreatePlayer.point.X + (ibCreatePlayer.rectangle.Width / 2) - (TextRenderer.MeasureText(CreatePlayer.Text, CreatePlayer.font).Width / 2);
            Manager.manager.GfxObjList.Add(CreatePlayer);

            ///////////////////// menu pour la supression du joueur
            rec1 = new Rec(Brushes.Beige, new Point((ScreenManager.WindowWidth / 2) - 150, (ScreenManager.WindowHeight / 2) - 100), new Size(300, 200), "rec1", Manager.TypeGfx.Top, false);

            Txt lqs = new Txt(CommonCode.TranslateText(60), new Point(0, 20), "lqs", Manager.TypeGfx.Top, true, new Font("Verdana", 9), Brushes.Black);
            lqs.point.X = (rec1.size.Width / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(60), lqs.font).Width / 2);
            rec1.Child.Add(lqs);

            secretQuestion = new Txt(string.Empty, new Point(0, 50), "qs", Manager.TypeGfx.Top, true, new Font("Verdana", 9), Brushes.Red);
            rec1.Child.Add(secretQuestion);

            secretQuestionTB = new TextBox {Width = 150};
            secretQuestionTB.Location = new Point((ScreenManager.WindowWidth - secretQuestionTB.Width) / 2 + 10, rec1.point.Y + 80);
            secretQuestionTB.Visible = false;
            secretQuestionTB.UseSystemPasswordChar = true;
            secretQuestionTB.Name = "secretQuestionTB";
            Manager.manager.mainForm.Controls.Add(secretQuestionTB);
            Manager.manager.GfxCtrlList.Add(secretQuestionTB);

            envoyerBtn = new Button
            {
                Text = CommonCode.TranslateText(26),
                Visible = false,
                Location = new Point(440, 370),
                Name = "envoyerBtn"
            };
            envoyerBtn.Click += envoyerBtn_Click;
            Manager.manager.GfxCtrlList.Add(envoyerBtn);
            Manager.manager.mainForm.Controls.Add(envoyerBtn);

            annulerBtn = new Button
            {
                Text = CommonCode.TranslateText(34),
                Visible = false,
                Location = new Point(520, 370),
                Name = "annulerBtn"
            };
            annulerBtn.Click += annulerBtn_Click;
            Manager.manager.mainForm.Controls.Add(annulerBtn);
            Manager.manager.GfxCtrlList.Add(annulerBtn);
            Manager.manager.GfxTopList.Add(rec1);

            CommonCode.MyPlayerInfo.instance.map = "SelectPlayer";
            ////////////////////////////////////////////////////////
            GrabingPlayersInformationRequestMessage grabingPlayersInformationRequestMessage = new GrabingPlayersInformationRequestMessage();
            grabingPlayersInformationRequestMessage.Serialize();
            grabingPlayersInformationRequestMessage.Send();

        }
        void SelectPlayer_MouseDoubleClic(Bmp bmp, MouseEventArgs e)
        {
            // demander tous infos concernant le joueur qui a été double cliqué
            CommonCode.MyPlayerInfo.instance.pseudo = bmp.name;
            ConfirmSelectActorRequestMessage confirmSelectPlayerRequestMessage = new ConfirmSelectActorRequestMessage(bmp.name);
            confirmSelectPlayerRequestMessage.Serialize();
            confirmSelectPlayerRequestMessage.Send();
        }
        void envoyerBtn_Click(object sender, EventArgs e)
        {
            DeleteActorRequestMessage deletePlayerRequestMessage = new DeleteActorRequestMessage(selectedDeletPlayer, secretQuestionTB.Text);
            deletePlayerRequestMessage.Serialize();
            deletePlayerRequestMessage.Send();
            annulerBtn_Click(null, null);
        }
        void annulerBtn_Click(object sender, EventArgs e)
        {
            envoyerBtn.Visible = false;
            annulerBtn.Visible = false;
            rec1.visible = false;
            selectedDeletPlayer = "";
            secretQuestionTB.Text = string.Empty;
            secretQuestionTB.Visible = false;
        }
        void Delete_Player_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // demande de suppression de compte
            DialogResult result = MessageBox.Show(CommonCode.TranslateText(59), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                selectedDeletPlayer = ((Actor)ibPlayers[Convert.ToInt16(bmp.name.Split('.')[1])].tag).pseudo;
                ConfirmDeleteActorRequestMessage askDeletePlayerRequestMessage = new ConfirmDeleteActorRequestMessage();
                askDeletePlayerRequestMessage.Serialize();
                askDeletePlayerRequestMessage.Send();
            }
        }
        void ibRetour_MouseUp(Bmp bmp, MouseEventArgs e)
        {
            ibCreatePlayer.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 9));
            GameStateManager.ChangeState(new CreatePlayer());
            GameStateManager.CheckState();
        }
        void ibRetour_MouseDown(Bmp bmp, MouseEventArgs e)
        {
            ibCreatePlayer.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 10));
        }
        public void Network_stat(string stat)
        {
            //Manager.manager.mainForm.Invoke(new DelHandel(Handle_Network_Stat), stat);    
        }
        public void Handle_Network_Stat(string stat)
        {

        }
        public void CleanUp()
        {
            Manager.manager.Clear();
        }
        public void Update()
        {
            
        }
        #endregion
    }
}