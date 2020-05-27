using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using MELHARFI;
using MELHARFI.Gfx;
using MMORPG.Net.Messages.Request;

namespace MMORPG.GameStates
{
    public class LoginMap : IGameState
    {
        Bmp ConnexionBtn, selectedLangue;
        Txt connexion_stat;
        TextBox username, password;
        Anim Feu, MenuLangue;
        delegate void DelHandel(string s);   // delegate pour faire du cross threading
        public OptionForm of = new OptionForm();
        public static Rec CloseFormMenu;
        Txt CloseFormDecoTxt, CloseAppTxt, CloseSelectPlayerTxt, annulerMenuTxt;

        public LoginMap()
        {
            
        }

        #region IGameState Methodes
        public void Init()
        {
            //MainForm.BackSound.settings.setMode("loop", true);
            //MainForm.BackSound.URL = @"sfx\openning.mp3";
            Manager.manager.Clear(true);
            System.GC.Collect();
            HudHandle.chatboxCleaner();             // pour supprimer l'objet si l'utilisateur été déjà connecté est a deconnecté apres, afin de recréer l'objet lors une nouvelle connexion
            HudHandle.hudCleaner();

            Bmp __optionForm = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ScreenManager.WindowWidth - 17, 2), "__optionForm", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 0));
            Manager.manager.GfxTopList.Add(__optionForm);
            Manager.manager.GfxFixedList.Add(__optionForm);
            __optionForm.MouseOver += optionForm_MouseOver;
            __optionForm.MouseOut += optionForm_MouseOut;
            __optionForm.MouseClic += option_MouseClic;

            // bouton fermer
            Bmp __CloseForm = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ScreenManager.WindowWidth - 17, 20), "__CloseForm", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 3));
            Manager.manager.GfxTopList.Add(__CloseForm);
            Manager.manager.GfxFixedList.Add(__CloseForm);
            __CloseForm.MouseOver += CloseForm_MouseOver;
            __CloseForm.MouseOut += CloseForm_MouseOut;
            __CloseForm.MouseClic += CloseForm_MouseClic;

            // menu "cadre qui encercle le menu"
            CloseFormMenu = new Rec(Brushes.LightSteelBlue, new Point((Manager.manager.mainForm.Width / 2) - 125, (Manager.manager.mainForm.Height / 2) - 170), new Size(250, 300), "__CloseFormMenu", Manager.TypeGfx.Top, false);
            CloseFormMenu.MouseOver += CloseFormMenu_MouseOver;
            Manager.manager.GfxTopList.Add(CloseFormMenu);
            Manager.manager.GfxFixedList.Add(CloseFormMenu);

            // 2eme cadre qui encercle le menu
            Rec CloseFormCadre = new Rec(Brushes.CadetBlue, new Point(10, 10), new Size(230, 280), "__CloseFormCadre", Manager.TypeGfx.Top, true);
            CloseFormCadre.MouseOver += CloseFormCadre_MouseOver;
            CloseFormMenu.Child.Add(CloseFormCadre);

            /////////////// Deconexion
            Bmp CloseFormDeco = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(20, 23), "__CloseFormDeco", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 5));
            CloseFormDeco.MouseClic += CloseFormDeco_MouseClic;
            CloseFormDeco.MouseOver += CloseFormDeco_MouseOver;
            CloseFormDeco.MouseOut += CloseFormDeco_MouseOut;
            CloseFormMenu.Child.Add(CloseFormDeco);

            CloseFormDecoTxt = new Txt(CommonCode.TranslateText(51), new Point(0, 24), "__CloseFormDecoTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 10, FontStyle.Bold), Brushes.White);
            CloseFormDecoTxt.point.X = (CloseFormMenu.size.Width / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(51), CloseFormDecoTxt.font).Width / 2) + 2;
            CloseFormDecoTxt.MouseClic += CloseFormDecoTxt_MouseClic;
            CloseFormMenu.Child.Add(CloseFormDecoTxt);

            ////////////// fermeture
            Bmp CloseApp = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(20, 60), "__CloseApp", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 5));
            CloseApp.MouseClic += CloseApp_MouseClic;
            CloseApp.MouseOver += CloseApp_MouseOver;
            CloseApp.MouseOut += CloseApp_MouseOut;
            CloseFormMenu.Child.Add(CloseApp);

            CloseAppTxt = new Txt(CommonCode.TranslateText(44), new Point(0, 62), "__CloseFormDecoTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 10, FontStyle.Bold), Brushes.White);
            CloseAppTxt.point.X = (CloseFormMenu.size.Width / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(44), CloseAppTxt.font).Width / 2) + 2;
            CloseAppTxt.MouseClic += CloseAppTxt_MouseClic;
            CloseFormMenu.Child.Add(CloseAppTxt);

            ////////////// retour map selection de joueur
            Bmp CloseSelectPlayer = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(20, 100), "__CloseSelectPlayer", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 5));
            CloseSelectPlayer.MouseClic += CloseSelectPlayer_MouseClic;
            CloseSelectPlayer.MouseOver += CloseSelectPlayer_MouseOver;
            CloseSelectPlayer.MouseOut += CloseSelectPlayer_MouseOut;
            CloseFormMenu.Child.Add(CloseSelectPlayer);

            CloseSelectPlayerTxt = new Txt(CommonCode.TranslateText(19), new Point(0, 102), "__CloseFormDecoTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 10, FontStyle.Bold), Brushes.White);
            CloseSelectPlayerTxt.point.X = (CloseFormMenu.size.Width / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(19), CloseSelectPlayerTxt.font).Width / 2) + 2;
            CloseSelectPlayerTxt.MouseClic += CloseSelectPlayerTxt_MouseClic;
            CloseFormMenu.Child.Add(CloseSelectPlayerTxt);

            ////////////// annuler pour supprimer le menu
            Bmp annulerMenu = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(20, 140), "__annulerMenu", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 5));
            annulerMenu.MouseClic += annulerMenu_MouseClic;
            annulerMenu.MouseOver += CloseSelectPlayer_MouseOver;
            annulerMenu.MouseOut += CloseSelectPlayer_MouseOut;
            CloseFormMenu.Child.Add(annulerMenu);

            annulerMenuTxt = new Txt(CommonCode.TranslateText(34), new Point(0, 142), "__annulerMenuTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 10, FontStyle.Bold), Brushes.White);
            annulerMenuTxt.point.X = (CloseFormMenu.size.Width / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(34), annulerMenuTxt.font).Width / 2) + 2;
            annulerMenuTxt.MouseClic += annulerMenuTxt_MouseClic;
            CloseFormMenu.Child.Add(annulerMenuTxt);
            CloseFormMenu.visible = false;

            Bmp map = new Bmp(@"gfx\map\login\bg\1.dat", new Point(0, 0), "map1", Manager.TypeGfx.Bgr, true, 1);
            map.MouseOver += map_MouseOver;

            Manager.manager.GfxBgrList.Add(map);
            CommonCode.CurMap = "LoginMap";

            username = new TextBox
            {
                Name = "username",
                Size = new Size(100, 20),
                Location = new Point(520, 242),
                Text = "admin",
                TabIndex = ZOrder.Ctrl()
            };
            username.Focus();
            username.KeyUp += username_KeyUp;
            username.Visible = true;
            Manager.manager.GfxCtrlList.Add(username);
            Manager.manager.mainForm.Controls.Add(username);

            password = new TextBox
            {
                Size = new Size(100, 20),
                Location = new Point(520, 270),
                Name = "password",
                Text = "123456",
                UseSystemPasswordChar = true
            };
            password.KeyUp += password_KeyUp;
            password.TabIndex = ZOrder.Ctrl();
            Manager.manager.mainForm.Controls.Add(password);
            Manager.manager.GfxCtrlList.Add(password);

            ConnexionBtn = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(440, 330), "__ConnexionBtn", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 9));
            ConnexionBtn.MouseClic += ConnexionBtn_MouseClic;
            ConnexionBtn.MouseOver += ConnexionBtn_MouseOver;
            ConnexionBtn.MouseDown += ConnexionBtn_MouseDown;
            ConnexionBtn.MouseUp += ConnexionBtn_MouseUp;
            ConnexionBtn.MouseOut += ConnexionBtn_MouseOut;
            ConnexionBtn.MouseMove += ConnexionBtn_MouseMove;
            Manager.manager.GfxObjList.Add(ConnexionBtn);

            Txt connexionBtnLabel = new Txt(CommonCode.TranslateText(145), Point.Empty, "__connexionBtnLabel", Manager.TypeGfx.Obj, true, new Font("Verdana", 9, FontStyle.Bold), Brushes.White);
            connexionBtnLabel.point = new Point(ConnexionBtn.point.X + (ConnexionBtn.rectangle.Width / 2) - (TextRenderer.MeasureText(connexionBtnLabel.Text, connexionBtnLabel.font).Width / 2), ConnexionBtn.point.Y + (ConnexionBtn.rectangle.Height / 2) - (TextRenderer.MeasureText(connexionBtnLabel.Text, connexionBtnLabel.font).Height / 2) - 5);
            connexionBtnLabel.MouseClic += connexionBtnLabel_MouseClic;
            Manager.manager.GfxObjList.Add(connexionBtnLabel);

            connexion_stat = new Txt(string.Empty, new Point(0, 306), "__connexion_stat", Manager.TypeGfx.Obj, true, new Font("Verdana", 8, FontStyle.Regular), Brushes.Red);
            Manager.manager.GfxObjList.Add(connexion_stat);

            Feu = new Anim(50, 1);
            for (int cnt = 1; cnt < 26; cnt++)
                Feu.AddCell(@"gfx\map\login\anim\1\" + cnt + ".dat", cnt, 0, ScreenManager.WindowHeight - 200);
            Feu.Ini(Manager.TypeGfx.Obj, "Fire", true);
            Feu.Start();
            Manager.manager.GfxObjList.Add(Feu);

            Int16 langueIndex = 0;
            switch (CommonCode.langue)
            {
                case 0:
                    langueIndex = 4;
                    break;
                case 1:
                    langueIndex = 5;
                    break;
                case 2:
                    langueIndex = 3;
                    break;
            }

            selectedLangue = new Bmp(@"gfx\map\login\obj\" + langueIndex + ".dat", new Point(8, 3), "__selectedLangue", Manager.TypeGfx.Bgr, true, 1);
            selectedLangue.MouseClic += selectedLangue_MouseClic;
            selectedLangue.MouseOver += selectedLangue_MouseOver;
            Manager.manager.GfxBgrList.Add(selectedLangue);

            MenuLangue = new Anim(50, 1);
            for (int cnt = 0; cnt < 4; cnt++)
                MenuLangue.AddCell(@"gfx\map\login\obj\6.dat", cnt, 4, cnt * 10);
            MenuLangue.Ini(Manager.TypeGfx.Obj, true);
            MenuLangue.visible(false);
            MenuLangue.AutoResetAnim = false;
            Manager.manager.GfxObjList.Add(MenuLangue);

            Bmp ArFlag = new Bmp(@"gfx\map\login\obj\3.dat", new Point(4, 4), "__ArFlag", Manager.TypeGfx.Obj, true, 1);
            ArFlag.MouseClic += ArFlag_MouseClic;
            ArFlag.MouseOver += ArFlag_MouseOver;
            MenuLangue.Child.Add(ArFlag);

            Bmp FrFlag = new Bmp(@"gfx\map\login\obj\4.dat", new Point(4, 30), "__FrFlag", Manager.TypeGfx.Obj, true, 1);
            FrFlag.MouseClic += FrFlag_MouseClic;
            FrFlag.MouseOver += FrFlag_MouseOver;
            MenuLangue.Child.Add(FrFlag);

            Bmp EnFlag = new Bmp(@"gfx\map\login\obj\5.dat", new Point(4, 56), "__EnFlag", Manager.TypeGfx.Obj, true, 1);
            EnFlag.MouseClic += EnFlag_MouseClic;
            EnFlag.MouseOver += EnFlag_MouseOver;
            MenuLangue.Child.Add(EnFlag);

            // affichage du menuStats et hud
            // affichage du hud chat

            
            System.GC.Collect();
            // dessin du menuStats
            MainForm.DrawMenuStats();
            // reinitialisation des variable
            CommonCode.blockNetFlow = false;
            Manager.manager.mainForm.Cursor = Cursors.Default;
            Battle.currentCursor = "";
            Battle.Clear();
        }

        void optionForm_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 1));
        }
        void optionForm_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 0));
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        void option_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            of.ReloadString();
            of.Show();
            Manager.manager.mainForm.Enabled = false;
        }
        void CloseForm_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 4));
        }
        void CloseForm_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 3));
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        void CloseForm_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            if (!CloseFormMenu.visible)
            {
                // check si le joueur est connecté, vus que ce menu est valide seulement en cas de connexion
                if (Network.netClient.Status.ToString() == "Running")
                {
                    CloseFormMenu.zindex = 1000;
                    CloseFormMenu.visible = true;

                    Manager.manager.HideGfxCtrlListOnlyVisible(true);
                    CloseFormDecoTxt.Text = CommonCode.TranslateText(51);
                    CloseFormDecoTxt.point.X = (CloseFormMenu.size.Width / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(51), CloseFormDecoTxt.font).Width / 2) + 2;

                    CloseAppTxt.Text = CommonCode.TranslateText(44);
                    CloseAppTxt.point.X = (CloseFormMenu.size.Width / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(44), CloseAppTxt.font).Width / 2) + 2;

                    CloseSelectPlayerTxt.Text = CommonCode.TranslateText(19);
                    CloseSelectPlayerTxt.point.X = (CloseFormMenu.size.Width / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(19), CloseSelectPlayerTxt.font).Width / 2) + 2;
                }
            }
            else
            {
                CloseFormMenu.visible = false;
                Manager.manager.HideGfxCtrlListOnlyVisible(false);
            }
        }
        void CloseFormMenu_MouseOver(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        void CloseFormCadre_MouseOver(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        void CloseFormDeco_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            CloseFormDeco_MouseClic();
        }
        void CloseFormDeco_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 6));
        }
        void CloseFormDeco_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 5));
        }
        void CloseFormDecoTxt_MouseClic(Txt txt, MouseEventArgs e)
        {
            CloseFormDeco_MouseClic();
        }
        void CloseApp_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            Manager.manager.mainForm.Close();
        }
        void CloseApp_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 6));
        }
        void CloseApp_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 5));
        }
        void CloseAppTxt_MouseClic(Txt txt, MouseEventArgs e)
        {
            Manager.manager.mainForm.Close();
        }
        void CloseSelectPlayer_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 6));
        }
        void CloseSelectPlayer_MouseClic()
        {
            if (Network.netClient.Status == MELHARFI.Lidgren.Network.NetPeerStatus.Running)
            {
                // check si on ai en combat, si oui dire auc lient si on souhaite abondonner le combat
                if (Battle.state == Enums.battleState.state.started)
                    MessageBox.Show(CommonCode.TranslateText(111), "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    // informer le serveur que le client souhaite changer de personnage pour reinitialiser les données du clients
                    //Network.SendMessage("cmd•SessionZero", true);
                    SessionZeroRequestMessage sessionZeroRequestMessage = new SessionZeroRequestMessage();
                    sessionZeroRequestMessage.Serialize();
                    sessionZeroRequestMessage.Send();

                    GameStateManager.ChangeState(new SelectPlayer());
                    GameStateManager.CheckState();
                }
            }
            CloseFormMenu.visible = false;
            Manager.manager.HideGfxCtrlListOnlyVisible(false);
        }
        void CloseSelectPlayer_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            CloseSelectPlayer_MouseClic();
        }
        void CloseSelectPlayerTxt_MouseClic(Txt txt, MouseEventArgs e)
        {
            CloseSelectPlayer_MouseClic();
        }
        void CloseSelectPlayer_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 5));
        }
        void annulerMenu_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            CloseFormMenu.visible = false;
        }
        void annulerMenuTxt_MouseClic(Txt txt, MouseEventArgs e)
        {
            CloseFormMenu.visible = false;
        }
        public void CloseFormDeco_MouseClic()
        {
            if (Network.netClient.Status.ToString() == "Running")
            {
                Network.Shutdown();
                GameStateManager.ChangeState(new LoginMap());
                GameStateManager.CheckState();
            }

            CloseFormMenu.visible = false;
            Manager.manager.HideGfxCtrlList(false);
        }
        void ConnexionBtn_MouseMove(Bmp bmp, MouseEventArgs e)
        {
            // pour contrer un probleme qui fait que lors d'un MouseDown, l'image change en bouton enfoncé, mais quand on garde la sourie enfoncé et qu'on déplace le curseur ailleur, l'image reviens a son état original MAIS quand on remet le curseur sur le bouton il ne redeviens pas enfoncé
            // ce code fait un controle du bouton de la sourie si il est enfoncé ou pas pour remetre la bonne image
            if (e.Button == MouseButtons.Left)
                bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 10));
        }
        void ConnexionBtn_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 9));
        }
        void ConnexionBtn_MouseUp(Bmp bmp, MouseEventArgs e)
        {
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 9));
        }

        void ConnexionBtn_MouseDown(Bmp bmp, MouseEventArgs e)
        {
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 10));
        }
        void ConnexionBtn_MouseClic()
        {
            username.Text = username.Text.ToLower();

            if (username.Text == "")
            {
                connexion_stat.point = new Point(0, 306);
                connexion_stat.Text = CommonCode.TranslateText(0);
                connexion_stat.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(0), connexion_stat.font).Width / 2);
                username.Focus();
            }
            else if (password.Text == "")
            {
                connexion_stat.point = new Point(0, 306);
                connexion_stat.Text = CommonCode.TranslateText(1);
                connexion_stat.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(1), connexion_stat.font).Width / 2);
                password.Focus();
            }
            else if (username.Text.Length < 5)
            {
                connexion_stat.point = new Point(312, 306);
                connexion_stat.Text = CommonCode.TranslateText(6);
                connexion_stat.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(6), connexion_stat.font).Width / 2);
                username.Focus();
            }
            else if (password.Text.Length < 5)
            {
                connexion_stat.point = new Point(0, 306);
                connexion_stat.Text = CommonCode.TranslateText(7);
                connexion_stat.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(7), connexion_stat.font).Width / 2);
                password.Focus();
            }
            else if (username.Text.Length > 15)
            {
                connexion_stat.point = new Point(0, 306);
                connexion_stat.Text = CommonCode.TranslateText(21);
                connexion_stat.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(21), connexion_stat.font).Width / 2);
                username.Focus();
            }
            else if (password.Text.Length > 15)
            {
                connexion_stat.point = new Point(0, 306);
                connexion_stat.Text = CommonCode.TranslateText(22);
                connexion_stat.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(22), connexion_stat.font).Width / 2);
                password.Focus();
            }
            else if (!Security.check_valid_user(username.Text))
            {
                MessageBox.Show(CommonCode.TranslateText(23), "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
                username.Focus();

            }
            else if (!Security.check_valid_pwd(password.Text))
            {
                connexion_stat.point = new Point(0, 306);
                connexion_stat.Text = CommonCode.TranslateText(31);
                connexion_stat.point.X = (ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(CommonCode.TranslateText(31), connexion_stat.font).Width / 2);
                password.Focus();
            }
            else
            {
                ///// desactivation du boutton connexion //////
                ConnexionBtn.visible = false;
                username.Enabled = false;
                password.Enabled = false;
                IGfx __connexionBtnLabel = Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel");
                if (__connexionBtnLabel != null)
                    Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel").Visible(false);

                // 1ere étape de connexion
                connexion_stat.Text = string.Empty;
                if (Network.netClient.Status.ToString() == "NotRunning")
                    Network.Connect(Network.ip, Network.port);
            }
        }
        void connexionBtnLabel_MouseClic(Txt txt, MouseEventArgs e)
        {
            ConnexionBtn_MouseClic();
        }
        void ConnexionBtn_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            ConnexionBtn_MouseClic();
        }
        void EnFlag_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        void FrFlag_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        void ArFlag_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        void selectedLangue_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        void ConnexionBtn_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        void EnFlag_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            selectedLangue.ChangeBmp(@"gfx\map\login\obj\5.dat");
            CommonCode.langue = 1;
            MenuLangue.Reverse = true;
            MenuLangue.HideAtLastFrame = true;
            MenuLangue.Start();
            saveOptions();
            // changement des labels dans StatsImg
            ChatArea.RightToLeft = RightToLeft.No;
            HudHandle.ChatTextBox.RightToLeft = RightToLeft.No;
        }
        void FrFlag_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            selectedLangue.ChangeBmp(@"gfx\map\login\obj\4.dat");
            CommonCode.langue = 0;
            MenuLangue.Reverse = true;
            MenuLangue.HideAtLastFrame = true;
            MenuLangue.Start();
            saveOptions();

            ChatArea.RightToLeft = RightToLeft.No;
            HudHandle.ChatTextBox.RightToLeft = RightToLeft.No;
        }
        void ArFlag_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            selectedLangue.ChangeBmp(@"gfx\map\login\obj\3.dat");
            CommonCode.langue = 2;
            MenuLangue.Reverse = true;
            MenuLangue.HideAtLastFrame = true;
            MenuLangue.Start();
            saveOptions();

            ChatArea.RightToLeft = RightToLeft.Yes;
            HudHandle.ChatTextBox.RightToLeft = RightToLeft.Yes;
        }
        void selectedLangue_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            if (MenuLangue.img.visible == false)
            {
                MenuLangue.visible(true);
                MenuLangue.Reverse = false;
                MenuLangue.HideAtLastFrame = false;
                MenuLangue.Start();
            }
            else
            {
                MenuLangue.Reverse = true;
                MenuLangue.HideAtLastFrame = true;
                MenuLangue.Start();
            }
        }
        void map_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        void password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ConnexionBtn_MouseClic(null, null);
        }
        void username_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ConnexionBtn_MouseClic(null, null);
        }       
        private void saveOptions()
        {
            List<string> configFile = new List<string>();
            using (StreamReader sr = new StreamReader(@"Config.ini"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    configFile.Add(line);
                sr.Close();         
            }
            string[] data = new string[configFile.Count];

            for (int cnt = 0; cnt < configFile.Count; cnt++)
            {
                if (configFile[cnt] != string.Empty && configFile[cnt].Substring(0, 2) != "//" && configFile[cnt].IndexOf(':') != -1)
                {
                    string[] dataLine = configFile[cnt].Split(':');
                    dataLine[0] = dataLine[0].Replace(" ", "");
                    dataLine[1] = dataLine[1].Replace(" ", "");

                    if (dataLine[0] == "Remote_Ip")
                        data[cnt] = "Remote_Ip:" + Network.host;
                    else if (dataLine[0] == "Remote_Port")
                        data[cnt] = "Remote_Port:" + Network.port;
                    else if (dataLine[0] == "lang")
                        data[cnt] = "lang:" + CommonCode.langue;
                    else
                        data[cnt] = dataLine[0] + ":" + dataLine[1];
                }
            }

            using (StreamWriter sw = new StreamWriter(@"Config.ini"))
            {
                foreach (string s in data)
                    sw.WriteLine(s);
                sw.Close();
            }
        }
        public void Disconenct()
        {
            // deconnexion avec le serveur
            username.Enabled = true;
            password.Enabled = true;
            ConnexionBtn.visible = true;
            IGfx __connexionBtnLabel = Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel");
            if (__connexionBtnLabel != null)
                Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel").Visible(true);
        }
        public void Handle_Network_Stat(string stat)
        {
            //string[] cmd = stat.Split('•');
        }
        public void Network_stat(string stat)
        {
            try
            {
                Manager.manager.mainForm.Invoke(new DelHandel(Handle_Network_Stat), stat);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }  
        public void CleanUp()
        {
            //MainForm.BackSound.controls.stop();
            Manager.manager.Clear();
            Battle.Clear();
        }
        public void Update()
        {
           
        }
        #endregion
    }
}