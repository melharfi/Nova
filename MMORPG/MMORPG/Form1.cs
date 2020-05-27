using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MMORPG.GameStates;
using System.Net;
using System.IO;
using MELHARFI.Gfx;
using System.Threading;
using System.Diagnostics;
using MELHARFI;
using System.Reflection;

namespace MMORPG
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]   //
        static extern IntPtr GetDC(IntPtr hWnd);    //
        [DllImport("user32.dll")]   //
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);   //
        [DllImport("gdi32.dll")]    //
        static extern int GetPixel(IntPtr hDC, int x, int y);   //
        [DllImport("gdi32.dll")]    //
        static extern int SetPixel(IntPtr hDC, int x, int y, int color);    //
        /// //////////////////////////////////
        public static string version = "1.1";   //
        public static string url = "http://www.mmorpg.com"; //
        //public OptionForm of = new OptionForm();
        //public static Rec CloseFormMenu;
        //Txt CloseFormDecoTxt, CloseAppTxt, CloseSelectPlayerTxt, annulerMenuTxt;
        public static bool grid = true;
        public static Thread tUpdateStats;
        public static bool hudVisible = false;             // pour determiner si la zone de sort + chat (hud) a été déssiné
        public static bool hudDrawnOnce = false;
        public static bool transparentChatBox = false;      // variable modifié depuis le fichier config.ini pour appliquer une image en arriere plant au chatbox
        public static string PictureBgChatBox = "null";     // contiens le chemain vers l'image de l'arrier plant chatbox
        public static string ColorBgChatBox = "255,255,255";    // contiens les couleurs de l'arrier plant du chatbox
        public static NHunspellExtender.NHunspellTextBoxExtender nHunspellTextBoxExtender1; // a supprimer puisque un autre variable est créée dans la form ChatBox
        int originalExStyle = -1;
        bool enableFormLevelDoubleBuffering = true;
        public static List<string> ChatLog = new List<string>();                               // contien l'historique de conversation
        protected override CreateParams CreateParams
        {
            get
            {
                ///////////////////////////////
                if (originalExStyle == -1)
                    originalExStyle = base.CreateParams.ExStyle;

                CreateParams cp = base.CreateParams;
                if (enableFormLevelDoubleBuffering && transparentChatBox)
                {
                    cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                }
                else
                    cp.ExStyle = originalExStyle;
                return cp;
            }
        }
        // double buffering d'un control en particulier
        /*public static void SetDoubleBuffered(Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }*/
        public static int indexSensorOfSpells = 0;                                    // contiens l'index qui determine quel page de sorts a afficher, quand le joueur a plusieurs sorts, et que chaque page de sorts contiens que 10 sort
        public static string spellsSortMethode = "all";                             // all = tous les sorts, classe = sorts de classe, aux = sorts auxiliaires
        public static ChatBox chatBox;                             // form qui contiens le RichTextBoxEx "ChatArea" pour le chat
        #region fonction qui permet de savoir si l'applicatione est en arriere plant ou avant plant
        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        #endregion
        //static System.Windows.Forms.Timer chatBoxVisibilityIssueHandle = new System.Windows.Forms.Timer();
        //bool blockChatBoxVisibilityIssueHandle = false;                 // pour que le timer bool chatBoxVisibilityIssueHandle n'execute pas le code indéfiniment quand la forme est en avant
        Point chatBoxPoint = Point.Empty;                               // se calcule lors du 1er lancement du jeu, s'il y à 1 seul instance, on lui attribue des valeur, et s'il y'en a plusieur on attribue une autre valeur au autres instances, bug lors de FormBorderStyle = None qui ajoute aussi les bordures de la forme étant invisible dans le calcule de position et de size http://stackoverflow.com/questions/4163655/form-height-problem-when-formborderstyle-is-none

        int ppp = 1;
        
        public MainForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.Invalidate();  //
            Initialize();

            Manager.manager = new Manager(this);    //
            Manager.OutputErrorCallBack = ManagerErrorHandler;  //

                Manager.ShowErrorsInMessageBox = false; //
                CommonCode.debug = true;
            
            SpriteSheetData.SSD.Run();  // pour lire le chemain vers les assets
            Manager.manager.Background = Color.White;   //
            
            #region lecture du fichier config.ini
            // verification si le fichier config est corrempu
            CommonCode.repairConfigFile();
            List<string> configFile = new List<string>();
            using (StreamReader sr = new StreamReader(@"Config.ini"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    configFile.Add(line);
                sr.Close();
            }

            for (int cnt = 0; cnt < configFile.Count; cnt++)
            {
                if (configFile[cnt] != string.Empty && configFile[cnt].Substring(0, 2) != "//" && configFile[cnt].IndexOf(':') != -1)
                {
                    string[] dataLine = configFile[cnt].Split(':');
                    dataLine[0] = dataLine[0].Replace(" ", "");
                    dataLine[1] = dataLine[1].Replace(" ", "");

                    if (dataLine[0] == "Remote_Ip")
                    {
                        try
                        {
                            string ip = dataLine[1];
                            // verifier s'il sagit d'un dns ou ip
                            if (dataLine[1].Split('.').Count() == 4)
                            {
                                string[] splittedIP = dataLine[1].Split('.');
                                bool ValidIP;
                                int part1;
                                int part2;
                                int part3;

                                if (int.TryParse(splittedIP[0], out part1))
                                {
                                    if (int.TryParse(splittedIP[0], out part2))
                                    {
                                        if (int.TryParse(splittedIP[0], out part3))
                                        {
                                            ValidIP = true;
                                        }
                                        else
                                            ValidIP = false;
                                    }
                                    else
                                        ValidIP = false;
                                }
                                else
                                    ValidIP = false;

                                if (ValidIP)
                                {
                                    // IP
                                    Network.host = dataLine[1];
                                    Network.ip = dataLine[1];
                                }
                                else
                                {
                                    // DNS
                                    IPAddress[] IPs = Dns.GetHostAddresses(dataLine[1]);
                                    if (IPs.Count() == 0)
                                    {
                                        // impossible de determiner l'ip
                                        MessageBox.Show("Can't resolve Hostname");
                                        Network.host = dataLine[1];
                                        Network.ip = dataLine[1];
                                    }
                                    else
                                    {
                                        Network.host = dataLine[1];
                                        Network.ip = IPs[0].ToString();
                                    }
                                }
                            }
                            else
                            {
                                // DNS
                                IPAddress[] IPs = Dns.GetHostAddresses(dataLine[1]);
                                if (IPs.Count() == 0)
                                {
                                    // impossible de determiner l'ip
                                    MessageBox.Show("Can't resolve Hostname");
                                    Network.host = dataLine[1];
                                    Network.ip = dataLine[1];
                                }
                                else
                                {
                                    Network.host = dataLine[1];
                                    Network.ip = IPs[0].ToString();
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show(CommonCode.TranslateText(148));
                        }
                    }
                    else if (dataLine[0] == "Remote_Port")
                        Network.port = Convert.ToInt32(dataLine[1]);
                    else if (dataLine[0] == "lang")
                        CommonCode.langue = Convert.ToInt32(dataLine[1]);
                    else if (dataLine[0] == "ChatBulleLineMaxLength")
                        CommonCode.ChatBulleLineMaxLength = Convert.ToInt32(dataLine[1]);
                    else if (dataLine[0] == "ChatBulleTimeOut")
                        CommonCode.ChatBulleTimeOut = Convert.ToInt32(dataLine[1]);
                    else if (dataLine[0] == "ChatMessageMaxChar")
                        CommonCode.ChatMessageMaxChar = Convert.ToInt32(dataLine[1]);
                    else if (dataLine[0] == "TransparentChatBox")
                        MainForm.transparentChatBox = Convert.ToBoolean(dataLine[1]);
                    else if (dataLine[0] == "ColorBgChatBox")
                        MainForm.ColorBgChatBox = dataLine[1];
                    else if (dataLine[0] == "PictureBgChatBox")
                        MainForm.PictureBgChatBox = dataLine[1];
                    else if (dataLine[0] == "DefaultLang")
                        CommonCode.DefaultLang = dataLine[1];
                    else if (dataLine[0] == "SpellCheck")
                        CommonCode.SpellCheck = bool.Parse(dataLine[1]);
                }
            }
            #endregion

            // supression
            this.Click += MainForm_Click;
            this.Move += MainForm_Move;
            this.MouseLeave += MainForm_MouseLeave;
            
            //chatBoxVisibilityIssueHandle.Interval = 30;
            //chatBoxVisibilityIssueHandle.Tick += T_chatBoxVisibleHandle_Tick;

            // nhunspell checker
            MainForm.nHunspellTextBoxExtender1 = new NHunspellExtender.NHunspellTextBoxExtender();
            
            ((System.ComponentModel.ISupportInitialize)(MainForm.nHunspellTextBoxExtender1)).BeginInit();
            this.SuspendLayout();
            // 
            // nHunspellTextBoxExtender1
            // 
            MainForm.nHunspellTextBoxExtender1.Language = "English";
            MainForm.nHunspellTextBoxExtender1.MaintainUserChoice = true;
            MainForm.nHunspellTextBoxExtender1.ShortcutKey = System.Windows.Forms.Shortcut.F7;
            MainForm.nHunspellTextBoxExtender1.SpellAsYouType = true;
            ((System.ComponentModel.ISupportInitialize)(MainForm.nHunspellTextBoxExtender1)).EndInit();

            if (Process.GetProcessesByName("MMORPG").Count() + Process.GetProcessesByName("MMORPG.vshost").Count() == 1)
                chatBoxPoint = new Point(8, -22);
            else
                chatBoxPoint = new Point(3, -17);

            CommonCode.installedLang = MainForm.nHunspellTextBoxExtender1.GetAvailableLanguages().ToList();
        }
        private void T_chatBoxVisibleHandle_Tick(object sender, EventArgs e)
        {
            if (ApplicationIsActivated() && hudVisible)
                chatBox.Show();
            else
                chatBox.Hide();
        }
        private void MainForm_Move(object sender, EventArgs e)
        {
            if (MainForm.chatBox != null)
                HudHandle.recalibrateChatBoxPosition();
        }
        void MainForm_MouseLeave(object sender, EventArgs e)
        {
            if (Battle.state == Enums.battleState.state.started && Battle.PlayerTurn == CommonCode.MyPlayerInfo.instance.pseudo)
            {
                // effacement de tous les chemain tracés avant
                if (Manager.manager.GfxBgrList.FindAll(f => f.GetType() == typeof(Rec) && f.Name() == "__wayPointRec").Count > 0)
                {
                    (Manager.manager.GfxBgrList.Find(f => f.GetType() == typeof(Rec) && f.Name() == "__wayPointRec") as Rec).visible = false;
                    (Manager.manager.GfxBgrList.Find(f => f.GetType() == typeof(Rec) && f.Name() == "__wayPointRec") as Rec).Child.Clear();
                    Manager.manager.GfxBgrList.RemoveAll(f => f.GetType() == typeof(Rec) && f.Name() == "__wayPointRec");
                }
            }
        }
        void ManagerErrorHandler(string s)
        {
            CommonCode.ChatMsgFormat("S", "null", s);
        }
        void MainForm_Click(object sender, EventArgs e)
        {
            // cette surcharge est pour bute de supprimer et d'annuler tout les objets Igfx abonnés à la liste commune.RemoveGfxWhenClicked
            // tous element dans cette liste vont etre supprimé avec ses child quand un click se produit ailleur de ce meme objet

            if (CommonCode.RemoveGfxWhenClicked != null && CommonCode.RemoveGfxWhenClicked.GetType() == typeof(Bmp))
            {
                MouseEventArgs me = (MouseEventArgs)e;
                if (!new Rectangle((CommonCode.RemoveGfxWhenClicked as Bmp).rectangle.Location, (CommonCode.RemoveGfxWhenClicked as Bmp).rectangle.Size).Contains(me.X, me.Y))
                {
                    // supression de l'objet parent de la list
                    if ((CommonCode.RemoveGfxWhenClicked as Bmp).TypeGfx == Manager.TypeGfx.Bgr)
                        Manager.manager.GfxBgrList.Remove(CommonCode.RemoveGfxWhenClicked);
                    else if ((CommonCode.RemoveGfxWhenClicked as Bmp).TypeGfx == Manager.TypeGfx.Obj)
                        Manager.manager.GfxObjList.Remove(CommonCode.RemoveGfxWhenClicked);
                    else if ((CommonCode.RemoveGfxWhenClicked as Bmp).TypeGfx == Manager.TypeGfx.Top)
                        Manager.manager.GfxTopList.Remove(CommonCode.RemoveGfxWhenClicked);

                    // supression des childs
                    for (int cnt2 = (CommonCode.RemoveGfxWhenClicked as Bmp).Child.Count() - 1; cnt2 >= 0; cnt2--)
                    {
                        if ((CommonCode.RemoveGfxWhenClicked as Bmp).Child[cnt2].GetType() == typeof(Bmp))
                            (CommonCode.RemoveGfxWhenClicked as Bmp).Child[cnt2].Visible(false);
                        else if ((CommonCode.RemoveGfxWhenClicked as Bmp).Child[cnt2].GetType() == typeof(Rec))
                            (CommonCode.RemoveGfxWhenClicked as Bmp).Child[cnt2].Visible(false);
                        else if ((CommonCode.RemoveGfxWhenClicked as Bmp).Child[cnt2].GetType() == typeof(Anim))
                        {
                            ((CommonCode.RemoveGfxWhenClicked as Bmp).Child[cnt2] as Anim).Close();
                            (CommonCode.RemoveGfxWhenClicked as Bmp).Child[cnt2].Visible(false);
                        }
                    }
                    (CommonCode.RemoveGfxWhenClicked as Bmp).Child.Clear();
                    CommonCode.RemoveGfxWhenClicked = null;
                }
            }
            else if (CommonCode.RemoveGfxWhenClicked != null && CommonCode.RemoveGfxWhenClicked.GetType() == typeof(Rec))
            {
                MouseEventArgs me = (MouseEventArgs)e;
                if (!new Rectangle((CommonCode.RemoveGfxWhenClicked as Rec).point, (CommonCode.RemoveGfxWhenClicked as Rec).size).Contains(me.X, me.Y))
                {
                    // supression de l'objet parent de la list
                    if ((CommonCode.RemoveGfxWhenClicked as Rec).TypeGfx == Manager.TypeGfx.Bgr)
                        Manager.manager.GfxBgrList.Remove(CommonCode.RemoveGfxWhenClicked);
                    else if ((CommonCode.RemoveGfxWhenClicked as Rec).TypeGfx == Manager.TypeGfx.Obj)
                        Manager.manager.GfxObjList.Remove(CommonCode.RemoveGfxWhenClicked);
                    else if ((CommonCode.RemoveGfxWhenClicked as Rec).TypeGfx == Manager.TypeGfx.Top)
                        Manager.manager.GfxTopList.Remove(CommonCode.RemoveGfxWhenClicked);

                    // supression des childs
                    for (int cnt2 = (CommonCode.RemoveGfxWhenClicked as Rec).Child.Count() - 1; cnt2 >= 0; cnt2--)
                    {
                        if ((CommonCode.RemoveGfxWhenClicked as Rec).Child[cnt2].GetType() == typeof(Bmp))
                            (CommonCode.RemoveGfxWhenClicked as Rec).Child[cnt2].Visible(false);
                        else if ((CommonCode.RemoveGfxWhenClicked as Rec).Child[cnt2].GetType() == typeof(Rec))
                            (CommonCode.RemoveGfxWhenClicked as Rec).Child[cnt2].Visible(false);
                        else if ((CommonCode.RemoveGfxWhenClicked as Rec).Child[cnt2].GetType() == typeof(Anim))
                        {
                            ((CommonCode.RemoveGfxWhenClicked as Rec).Child[cnt2] as Anim).Close();
                            (CommonCode.RemoveGfxWhenClicked as Rec).Child[cnt2].Visible(false);
                        }
                    }
                    (CommonCode.RemoveGfxWhenClicked as Rec).Child.Clear();
                    CommonCode.RemoveGfxWhenClicked = null;
                }
            }
            else if (CommonCode.RemoveGfxWhenClicked != null && CommonCode.RemoveGfxWhenClicked.GetType() == typeof(Anim))
            {
                MouseEventArgs me = (MouseEventArgs)e;
                if (!new Rectangle((CommonCode.RemoveGfxWhenClicked as Anim).img.rectangle.Location, (CommonCode.RemoveGfxWhenClicked as Anim).img.rectangle.Size).Contains(me.X, me.Y))
                {
                    // supression de l'objet parent de la list
                    if ((CommonCode.RemoveGfxWhenClicked as Anim).TypeGfx == Manager.TypeGfx.Bgr)
                        Manager.manager.GfxBgrList.Remove(CommonCode.RemoveGfxWhenClicked);
                    else if ((CommonCode.RemoveGfxWhenClicked as Anim).TypeGfx == Manager.TypeGfx.Obj)
                        Manager.manager.GfxObjList.Remove(CommonCode.RemoveGfxWhenClicked);
                    else if ((CommonCode.RemoveGfxWhenClicked as Anim).TypeGfx == Manager.TypeGfx.Top)
                        Manager.manager.GfxTopList.Remove(CommonCode.RemoveGfxWhenClicked);

                    // supression des childs
                    for (int cnt2 = (CommonCode.RemoveGfxWhenClicked as Anim).Child.Count() - 1; cnt2 >= 0; cnt2--)
                    {
                        if ((CommonCode.RemoveGfxWhenClicked as Anim).Child[cnt2].GetType() == typeof(Bmp))
                            (CommonCode.RemoveGfxWhenClicked as Anim).Child[cnt2].Visible(false);
                        else if ((CommonCode.RemoveGfxWhenClicked as Anim).Child[cnt2].GetType() == typeof(Rec))
                            (CommonCode.RemoveGfxWhenClicked as Anim).Child[cnt2].Visible(false);
                        else if ((CommonCode.RemoveGfxWhenClicked as Anim).Child[cnt2].GetType() == typeof(Anim))
                        {
                            ((CommonCode.RemoveGfxWhenClicked as Anim).Child[cnt2] as Anim).Close();
                            (CommonCode.RemoveGfxWhenClicked as Anim).Child[cnt2].Visible(false);
                        }
                    }
                    (CommonCode.RemoveGfxWhenClicked as Anim).Child.Clear();
                    CommonCode.RemoveGfxWhenClicked = null;
                }
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            GameStateManager.ChangeState(new LoginMap());
            GameStateManager.CheckState();

            this.KeyUp += MainForm_KeyUp;
        }
        void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            ChatArea_KeyUp(sender, e);
        }
        protected void Initialize()
        {
            this.Text = ScreenManager.WindowTitle;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Network.Shutdown();
            GameStateManager.Quit();

            // arret des threads
            if (tUpdateStats != null && tUpdateStats.IsAlive)
                tUpdateStats.Abort();

            // reinitialisation du battel pour que le thread focusPlayer s'arret
            Battle.Clear();

            // enregistrement des cmd dans le fichier log
            if(CommonCode.historyCmd.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(@"logs\historyCMD.txt"))
                {
                    for (int cnt = 0; cnt < CommonCode.historyCmd.Count; cnt++)
                    {
                        sw.WriteLine(CommonCode.historyCmd[cnt]);
                    }
                    sw.Close();
                }
            }
        }
        public static void DrawDisconnectImg(bool disconnect)
        {
            if (!Manager.manager.GfxObjList.Exists(f => f.Name() == "__BmpDisconnect"))
            {
                Bmp BmpDisconnect = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ScreenManager.WindowWidth - 60, 4), "__BmpDisconnect", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 2));

                Manager.manager.GfxObjList.Add(BmpDisconnect);
            }
        }
        public static void drawSpellStatesMenuOnce()
        {
            // dessiner le menu des sorts une fois lors de la selection du joueur
            // cadre général parent
            Rec showSpellsParent = new Rec(Brushes.White, new Point(10, 20), new Size(500, 500), "__showSpellsParent", Manager.TypeGfx.Top, false);
            MenuStats.spellrec = showSpellsParent;
            Manager.manager.GfxFixedList.Add(showSpellsParent);
            Manager.manager.GfxTopList.Add(showSpellsParent);

            // cadre qui encercle les sorts
            Rec spellrec2 = new Rec(Brushes.Gray, new Point(1, 1), new Size(254, 441), "__spellrec2", Manager.TypeGfx.Top, true);
            showSpellsParent.Child.Add(spellrec2);

            // dessiner les sorts
            drawSpellsMenuInParent();

            // bar verticale pour faire defiler les sorts
            VScrollBar spellsVscoll = new VScrollBar();
            spellsVscoll.Name = "__spellsVscroll";
            spellsVscoll.Location = new Point(showSpellsParent.point.X + 254, showSpellsParent.point.Y + 1);
            spellsVscoll.Size = new Size(18, 441);
            spellsVscoll.Minimum = 0;
            spellsVscoll.Maximum = returnSortedSpells().Count - 1;
            spellsVscoll.Value = indexSensorOfSpells;
            spellsVscoll.ValueChanged += spellsVscoll_ValueChanged;
            spellsVscoll.Visible = false;
            Manager.manager.mainForm.Controls.Add(spellsVscoll);

            // type de sort
            Txt typeOfSpell = new Txt(CommonCode.TranslateText(179), new Point(280, 5), "__typeOfSpell", Manager.TypeGfx.Top, true, new Font("Verdana", 9, FontStyle.Regular), Brushes.Black);
            showSpellsParent.Child.Add(typeOfSpell);

            // menu déroulant qui contien Tous, Classe, Auxiliaire
            ComboBox typeOfSpellCB = new ComboBox();
            typeOfSpellCB.Name = "__typeOfSpellCB";
            typeOfSpellCB.Items.Add(CommonCode.TranslateText(180));
            typeOfSpellCB.Items.Add(CommonCode.TranslateText(181));
            typeOfSpellCB.Items.Add(CommonCode.TranslateText(182));
            typeOfSpellCB.DropDownStyle = ComboBoxStyle.DropDownList;
            typeOfSpellCB.Location = new Point(showSpellsParent.point.X + typeOfSpell.point.X + TextRenderer.MeasureText(typeOfSpell.Text, typeOfSpell.font).Width, showSpellsParent.point.Y + typeOfSpell.point.Y - 2);
            typeOfSpellCB.SelectedIndex = 0;
            typeOfSpellCB.SelectedIndexChanged += typeOfSpellCB_SelectedIndexChanged;
            typeOfSpellCB.Visible = false;
            Manager.manager.mainForm.Controls.Add(typeOfSpellCB);

            // nombre de points réstant
            Actor pi = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
            string str1 = CommonCode.TranslateText(185).Split('%')[0];
            int spellPointsLeft = pi.spellPointLeft;
            string str2 = CommonCode.TranslateText(185).Split('%')[1];

            Txt spellPointsLeftLabel1 = new Txt(str1, new Point(10, spellrec2.point.Y + spellrec2.size.Height + 10), "__spellPointsLeftLabel1", Manager.TypeGfx.Top, true, new Font("Verdana", 10, FontStyle.Regular), Brushes.Black);
            showSpellsParent.Child.Add(spellPointsLeftLabel1);

            Txt spellPointsLeftValue = new Txt(spellPointsLeft.ToString(), new Point(spellPointsLeftLabel1.point.X + TextRenderer.MeasureText(spellPointsLeftLabel1.Text, spellPointsLeftLabel1.font).Width, spellrec2.point.Y + spellrec2.size.Height + 7), "__spellPointsLeftValue", Manager.TypeGfx.Top, true, new Font("Verdana", 12, FontStyle.Bold), Brushes.Orange);
            showSpellsParent.Child.Add(spellPointsLeftValue);

            Txt spellPointsLeftLabel2 = new Txt(str2, new Point(spellPointsLeftValue.point.X + TextRenderer.MeasureText(spellPointsLeftValue.Text, spellPointsLeftValue.font).Width + 2, spellrec2.point.Y + spellrec2.size.Height + 10), "__spellPointsLeftLabel2", Manager.TypeGfx.Top, true, new Font("Verdana", 10, FontStyle.Regular), Brushes.Black);
            showSpellsParent.Child.Add(spellPointsLeftLabel2);
        }
        static void typeOfSpellCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            // liste de choix de type de sort Tous/Classe/Auxiliaire
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex == 0)
                spellsSortMethode = "all";
            else if (cb.SelectedIndex == 1)
                spellsSortMethode = "classe";
            else if (cb.SelectedIndex == 2)
                spellsSortMethode = "aux";

            indexSensorOfSpells = 0;
            // modifier la VScrollbar pour les nouvelles valeurs
            VScrollBar spellsVscoll = Manager.manager.mainForm.Controls.Find("__spellsVscroll", false)[0] as VScrollBar;
            spellsVscoll.Maximum = returnSortedSpells().Count - 1;
            spellsVscoll.Value = 0;
            drawSpellsMenuInParent();
            // on efface l'ancienne instance déja affichés
            Manager.manager.GfxTopList.RemoveAll(f => f.Name() != null && f.Name().Length > 5 && f.Name().Substring(0, 5) == "DSI__");
        }
        static void spellsVscoll_ValueChanged(object sender, EventArgs e)
        {
            // lorsque la bare VScroll a été décalé pour actualiser les sorts caché
            indexSensorOfSpells = (sender as VScrollBar).Value;
            drawSpellsMenuInParent();
        }
        public static void drawSpellsMenuInParent()
        {
            // liste des sorts qui s'intégre dans le menu des sorts
            Actor pi = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
            List<int> newOrderedSpells = returnSortedSpells();

            Rec showSpellsParent = Manager.manager.GfxTopList.Find(f => f.Name() == "__showSpellsParent") as Rec;

            // nétoyage si les sorts sont déja affichés
            showSpellsParent.Child.RemoveAll(f => f.Name() == "__spell" || f.Name() == "__SpellIcon" || f.Name() == "__spellName" || f.Name() == "__lvlLabel" || f.Name() == "__lvlSort" || f.Name() == "__spellLvl");

            for (int cnt = 0; cnt < 10; cnt++)
            {
                Bmp spell;
                if (newOrderedSpells.Count - 1 >= cnt && pi.spells.Exists(f => f.sortID == newOrderedSpells[cnt]))
                {
                    spell = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(2, (44 * cnt) + 2), "__spell", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 74));
                    showSpellsParent.Child.Add(spell);
                }
                else
                {
                    spell = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(2, (44 * cnt) + 2), "__spell", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 75));
                    showSpellsParent.Child.Add(spell);
                }

                int level = 1;
                if (newOrderedSpells.Count - 1 >= indexSensorOfSpells + cnt)
                {
                    // image du sort
                    Bmp SpellIcon = new Bmp(@"gfx\general\icons\spells\" + newOrderedSpells[indexSensorOfSpells + cnt] + ".dat", new Point(spell.point.X + 5, spell.point.Y + 5), "__SpellIcon", Manager.TypeGfx.Top, true, 1);
                    showSpellsParent.Child.Add(SpellIcon);

                    // nom du sort
                    Txt spellName = new Txt(spells.sort(newOrderedSpells[indexSensorOfSpells + cnt]).title, new Point(SpellIcon.point.X + SpellIcon.rectangle.Width + 5, SpellIcon.point.Y + 3), "__spellName", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
                    showSpellsParent.Child.Add(spellName);

                    // label lvl
                    Txt lvlLabel = new Txt(CommonCode.TranslateText(50), new Point(spellName.point.X, spellName.point.Y + 15), "__lvlLabel", Manager.TypeGfx.Top, true, new Font("Verdana", 6, FontStyle.Italic), Brushes.Black);
                    showSpellsParent.Child.Add(lvlLabel);

                    // lvl, si notre personnage a déja ce sort, on lui affecte son lvl si non on lui donne le lvl 1
                    if (pi.spells.Exists(f => f.sortID == newOrderedSpells[indexSensorOfSpells + cnt]))
                    {
                        Txt lvlSort = new Txt(pi.spells.Find(f => f.sortID == newOrderedSpells[indexSensorOfSpells + cnt]).level.ToString(), new Point(lvlLabel.point.X + TextRenderer.MeasureText(lvlLabel.Text, lvlLabel.font).Width, lvlLabel.point.Y - 2), "__lvlSort", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                        showSpellsParent.Child.Add(lvlSort);
                        level = pi.spells.Find(f => f.sortID == newOrderedSpells[indexSensorOfSpells + cnt]).level;
                    }
                    else
                    {
                        Txt lvlSort = new Txt("1", new Point(lvlLabel.point.X + TextRenderer.MeasureText(lvlLabel.Text, lvlLabel.font).Width, lvlLabel.point.Y - 2), "__lvlSort", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Italic), Brushes.Black);
                        showSpellsParent.Child.Add(lvlSort);
                    }

                    // affichages des niveau disponibles
                    for (int cnt2 = 0; cnt2 < 5; cnt2++)
                    {
                        //bool bold = false;
                        Brush b = Brushes.Black;

                        if (pi.spells.Exists(f => f.sortID == newOrderedSpells[indexSensorOfSpells + cnt]))
                        {
                            if (pi.spells.Exists(f => f.sortID == newOrderedSpells[indexSensorOfSpells + cnt] && f.level == cnt2 + 1))
                            {
                                //bold = true;
                                b = Brushes.Red;
                            }
                        }
                        else
                        {
                            if (cnt2 == 0)
                            {
                                //bold = true;
                                b = Brushes.Black;
                            }
                        }
                        Txt spellLvl = new Txt((cnt2 + 1).ToString(), new Point(200 + (cnt2 * 10), spell.point.Y + 2), "__spellLvl", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), b);
                        showSpellsParent.Child.Add(spellLvl);
                    }
                    
                    // bouton pour augementer le lvl d'un sort
                    if ( pi.spells.Exists(f => f.sortID == newOrderedSpells[indexSensorOfSpells + cnt]))
                    {
                        // verifier si le joueur a le nombre de point necessaire pour augementer le sort et que le sort n'est pas le lvl max (5)
                        int lvlSort = pi.spells.Find(f => f.sortID == newOrderedSpells[indexSensorOfSpells + cnt]).level;
                        if(lvlSort < 5 && pi.spellPointLeft >= lvlSort)
                        {
                            // affichage d'image qui augemente les points
                            Bmp upgradeSpell = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(230, spell.point.Y + 18), "__spell", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 76));
                            upgradeSpell.tag = newOrderedSpells[indexSensorOfSpells + cnt];
                            upgradeSpell.MouseDown += UpgradeSpell_MouseDown;
                            upgradeSpell.MouseMove += CommonCode.CursorHand_MouseMove;
                            upgradeSpell.MouseOut += CommonCode.CursorDefault_MouseOut;
                            upgradeSpell.MouseOut += upgradeSpell_MouseOut;
                            upgradeSpell.MouseUp += upgradeSpell_MouseUp;
                            upgradeSpell.MouseClic += upgradeSpell_MouseClic;
                            showSpellsParent.Child.Add(upgradeSpell);
                        }
                    }
                }

                // associer le SortID au tag de l'image spell pour une utilisation lors d'un clic
                spell.tag = (newOrderedSpells.Count > cnt) ? newOrderedSpells[cnt] + "/" + level : -1 + "/" + level;
                spell.MouseClic += spell_MouseClic;
            }
        }
        static void spell_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // clic sur le cadre d'un sort pour afficher le détail du sort
            // on efface l'ancienne instance déja affichés
            Manager.manager.GfxTopList.RemoveAll(f => f.Name() != null && f.Name().Length > 5 && f.Name().Substring(0, 5) == "DSI__");
            int spellID = Convert.ToInt16(bmp.tag.ToString().Split('/')[0]);
            int level = Convert.ToInt16(bmp.tag.ToString().Split('/')[1]);
            Rec SpellInfo = DisplaySpellInfo(spellID, level);
            Rec showSpellsParent = Manager.manager.GfxTopList.Find(f => f.Name() == "__showSpellsParent") as Rec;
            SpellInfo.zindex = showSpellsParent.zindex + 1;
            Manager.manager.GfxTopList.Add(SpellInfo);
        }
        public static Rec DisplaySpellInfo(int sortID, int level)
        {
            // methode qui retourne la fiche technique d'un sort a un level demandé qui se superpos au menu 
            Actor pi = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
            Actor.SpellsInformations si = pi.spells.Find(f => f.sortID == sortID);

            Rec SpellInfo = new Rec(Brushes.AliceBlue, new Point(290, 70), new Size(212, 300), "DSI__SpellInfo", Manager.TypeGfx.Top, true);

            // image du sort
            Bmp SpellIcon = new Bmp(@"gfx\general\icons\spells\" + sortID + ".dat", new Point(10, 10), "DSI__SpellIcon", Manager.TypeGfx.Top, true, 1);
            SpellInfo.Child.Add(SpellIcon);

            // nom du sort
            Txt spellName = new Txt(spells.sort(sortID).title, new Point(SpellIcon.point.X + SpellIcon.rectangle.Width + 5, SpellIcon.point.Y + 9), "DSI__spellName", Manager.TypeGfx.Top, true, new Font("Verdana", 9, FontStyle.Bold), Brushes.Black);
            SpellInfo.Child.Add(spellName);

            // label lvl
            Txt lvlSort = new Txt(si.level.ToString(), new Point(180, 2), "DSI__lvlSort", Manager.TypeGfx.Top, true, new Font("Verdana", 12, FontStyle.Bold), Brushes.Red);
            SpellInfo.Child.Add(lvlSort);

            // on check l'element du sort, feut, terre, eau, vent, foudre, neutral
            if (spells.sort(sortID).element != Enums.Chakra.Element.neutral)
            {
                int idElement = 0;
                if (spells.sort(sortID).element == Enums.Chakra.Element.doton)
                    idElement = 33;
                else if (spells.sort(sortID).element == Enums.Chakra.Element.katon)
                    idElement = 34;
                else if (spells.sort(sortID).element == Enums.Chakra.Element.futon)
                    idElement = 35;
                else if (spells.sort(sortID).element == Enums.Chakra.Element.suiton)
                    idElement = 36;
                else if (spells.sort(sortID).element == Enums.Chakra.Element.raiton)
                    idElement = 37;

                Bmp ElementIcon = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "DSI__ElementIcon", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", idElement));
                ElementIcon.point = new Point(SpellIcon.point.X, SpellIcon.point.Y + SpellIcon.rectangle.Height + 5);
                SpellInfo.Child.Add(ElementIcon);
            }

            // ligne de vue ou pas
            if (spells.sort(sortID).isbl[si.level - 1].ligneDeVue)
            {
                Bmp LigneDeVue = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "DSI__LigneDeVue", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 86));
                LigneDeVue.point = new Point(SpellIcon.point.X + 40, SpellIcon.point.Y + SpellIcon.rectangle.Height + 9);
                SpellInfo.Child.Add(LigneDeVue);
            }
            else
            {
                // pas de ligne de vue
                Bmp pasDeLigneDeVue = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "DSI__pasDeLigneDeVue", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 83));
                pasDeLigneDeVue.point = new Point(SpellIcon.point.X + 40, SpellIcon.point.Y + SpellIcon.rectangle.Height + 9);
                SpellInfo.Child.Add(pasDeLigneDeVue);
            }

            // PE modifable
            if (spells.sort(sortID).isbl[si.level - 1].ligneDeVue)
            {
                Bmp peModifiable = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "DSI__peModifiable", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 84));
                peModifiable.point = new Point(SpellIcon.point.X + 70, SpellIcon.point.Y + SpellIcon.rectangle.Height + 7);
                SpellInfo.Child.Add(peModifiable);
            }
            else
            {
                // PE non modifiable
                Bmp peNonModifiable = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "DSI__peNonModifiable", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 85));
                peNonModifiable.point = new Point(SpellIcon.point.X + 70, SpellIcon.point.Y + SpellIcon.rectangle.Height + 7);
                SpellInfo.Child.Add(peNonModifiable);
            }

            // dessin des cadres qui contiens les infos sur les sorts
            // cadre lvl
            for (int cnt = 0; cnt < 5; cnt++)
            {
                // cadre des level
                Bmp sortRec = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(10, 70), "DSI__sortRec", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 78 + cnt));
                sortRec.zindex = (level == cnt + 1) ? 600 : cnt * 100;
                sortRec.tag = sortID + "/" + (cnt + 1);          // contien sortID/Level
                sortRec.MouseClic += sortRec_MouseClic;
                SpellInfo.Child.Add(sortRec);

                // label de l'anglet numéroté du 1 au 5 (cnt +1)
                Txt labelanglet = new Txt((cnt + 1).ToString(), new Point(((cnt + 1) * 30) + 2, sortRec.point.Y), "DSI__labelanglet", Manager.TypeGfx.Top, true, new Font("Verdana", 9, FontStyle.Bold), Brushes.Black);
                labelanglet.zindex = sortRec.zindex + 1;
                SpellInfo.Child.Add(labelanglet);

                // image CD
                Bmp cd = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(20, 90), "DSI__cd", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 30));
                cd.zindex = sortRec.zindex + 1;
                SpellInfo.Child.Add(cd);

                // vérification si le nombre de points CD sont >= au nombre de CD requise pour le sort en question, pour afficher toujours un status de 1/2 maximum
                int cdLeft = spells.sort(sortID).isbl[cnt].cd - pi.cd;
                if (cdLeft <= 0)
                    cdLeft = 1;
                cdLeft++;

                Txt cdLabel = new Txt(pi.cd + "/" + spells.sort(sortID).isbl[cnt].cd + "    [1/" + cdLeft + "]", new Point(cd.point.X + cd.rectangle.Width + 10, cd.point.Y + 2), "DSI__cdLabel", Manager.TypeGfx.Top, true, new Font("Verdana", 9, FontStyle.Regular), Brushes.Black);
                cdLabel.zindex = cd.zindex;
                SpellInfo.Child.Add(cdLabel);

                // Label dommage min
                Txt domMinLabel = new Txt(CommonCode.TranslateText(186), new Point(cd.point.X, cd.point.Y + 20), "DSI__domMinLabel", Manager.TypeGfx.Top, true, new Font("Verdana", 9, FontStyle.Regular), Brushes.Black);
                domMinLabel.zindex = cd.zindex;
                SpellInfo.Child.Add(domMinLabel);

                SolidBrush color = CommonCode.neutralColor;

                if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.doton)
                    color = CommonCode.dotonColor;
                else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.katon)
                    color = CommonCode.katonColor;
                else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.futon)
                    color = CommonCode.futonColor;
                else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.raiton)
                    color = CommonCode.raitonColor;
                else if (spells.sort(Battle.infos_sorts.sortID).element == Enums.Chakra.Element.suiton)
                    color = CommonCode.suitonColor;

                Txt domMin = new Txt(spells.sort(sortID).isbl[cnt].domMin + "  ->  " + spells.sort(sortID).isbl[cnt].domMax, new Point(domMinLabel.point.X + TextRenderer.MeasureText(domMinLabel.Text, domMinLabel.font).Width + 10, domMinLabel.point.Y), "DSI__domMin", Manager.TypeGfx.Top, true, new Font("Verdana", 9, FontStyle.Bold), color);
                domMin.zindex = cd.zindex;
                SpellInfo.Child.Add(domMin);
            }
            return SpellInfo;
        }
        static void sortRec_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // clic sur l'anglet du level d'un sort sur la partie détail d'un sort
            int spellID = Convert.ToInt16(bmp.tag.ToString().Split('/')[0]);
            int level = Convert.ToInt16(bmp.tag.ToString().Split('/')[1]);

            //on efface et réaffiche les infos des sorts selon le spellID et leve
            Manager.manager.GfxTopList.RemoveAll(f => f.Name() != null && f.Name().Length > 5 && f.Name().Substring(0, 5) == "DSI__");

            Rec SpellInfo = DisplaySpellInfo(spellID, level);
            Rec showSpellsParent = Manager.manager.GfxTopList.Find(f => f.Name() == "__showSpellsParent") as Rec;
            SpellInfo.zindex = showSpellsParent.zindex + 1;
            Manager.manager.GfxTopList.Add(SpellInfo);
        }
        static void upgradeSpell_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // click sur le boutton "+" qui permet d'augementer le lvl d'unj sort
            int sortID = Convert.ToInt16(bmp.tag);
            Network.SendMessage("cmd•upgradeSpell•" + sortID, true);
        }
        static void upgradeSpell_MouseUp(Bmp bmp, MouseEventArgs e)
        {
            upgradeSpell_MouseOut(bmp, e);
        }
        static void upgradeSpell_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            // remetre l'image d'origine sur le boutton "+" qui ajoute des points de sort lors du survol
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 76));
        }
        private static void UpgradeSpell_MouseDown(Bmp bmp, MouseEventArgs e)
        {
            // état bouton upgradeSpell enfoncé, on le remet sur l'état non enfoncé
            bmp.ChangeBmp(@"gfx\general\obj\1\all1.dat", SpriteSheet.GetSpriteSheet("_Main_option", 77));
        }
        static List<int> returnSortedSpells()
        {
            Actor pi = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
            List<int> newOrderedSpells = new List<int>();
            if (spellsSortMethode == "all")
            {
                newOrderedSpells = spells.returnClassSpells(pi.className).ToArray().ToList();
                for (int cnt = 0; cnt < pi.spells.Count; cnt++)
                    if (!newOrderedSpells.Exists(f => f == pi.spells[cnt].sortID))
                        newOrderedSpells.Add(pi.spells[cnt].sortID);
            }
            else if (spellsSortMethode == "classe")
                newOrderedSpells = spells.returnClassSpells(pi.className).ToArray().ToList();
            else if (spellsSortMethode == "aux")
            {
                List<int> classSort = spells.returnClassSpells(pi.className).ToArray().ToList();
                for (int cnt = 0; cnt < pi.spells.Count; cnt++)
                    if (!classSort.Exists(f => f == pi.spells[cnt].sortID))
                        newOrderedSpells.Add(pi.spells[cnt].sortID);
            }
            return newOrderedSpells;
        }
        public static void DrawMenuStats()
        {
            if(Manager.manager.GfxTopList.Exists(f => f.Name() == "__StatsImg"))
                return;
            #region affichage du menu stats
            // image stats
            Bmp StatsImg = new Bmp(@"gfx\general\obj\1\stats.dat", new Point(5, 100), "__StatsImg", Manager.TypeGfx.Top, false, 1);
            Manager.manager.GfxTopList.Add(StatsImg);
            Manager.manager.GfxFixedList.Add(StatsImg);
            MenuStats.StatsImg = StatsImg;

            // affichage des composants du tableau stats
            Bmp ThumbsAvatar = new Bmp();
            MenuStats.ThumbsAvatar = ThumbsAvatar;
            Manager.manager.GfxFixedList.Add(ThumbsAvatar);

            // label nom du player
            Txt StatsPlayerName = new Txt("", new Point(46, 4), "__StatsPlayerName", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.MediumBlue);
            StatsImg.Child.Add(StatsPlayerName);
            Manager.manager.GfxFixedList.Add(StatsPlayerName);
            MenuStats.StatsPlayerName = StatsPlayerName;

            // label level
            Txt StatsLevel = new Txt("", new Point(46, 16), "__StatsLevel", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Green);
            StatsImg.Child.Add(StatsLevel);
            Manager.manager.GfxFixedList.Add(StatsLevel);
            MenuStats.StatsLevel = StatsLevel;

            // rang
            Txt Rang = new Txt("", new Point(46, 28), "__Rang", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Brown);
            StatsImg.Child.Add(Rang);
            Manager.manager.GfxFixedList.Add(StatsImg);
            MenuStats.Rang = Rang;

            // lvl pvp
            Txt LevelPvp = new Txt("", new Point(295, 32), "__LevelPvp", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
            StatsImg.Child.Add(LevelPvp);
            Manager.manager.GfxFixedList.Add(LevelPvp);
            MenuStats.LevelPvp = LevelPvp;

            // grade pvp
            Bmp GradePvp = new Bmp();
            Manager.manager.GfxFixedList.Add(GradePvp);
            MenuStats.GradePvp = GradePvp;

            // flag
            Bmp Flag = new Bmp();
            Manager.manager.GfxFixedList.Add(Flag);
            MenuStats.Flag = Flag;

            // label flag
            Txt LFlag = new Txt("", new Point(235, 28), "__LFlag", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
            StatsImg.Child.Add(LFlag);
            Manager.manager.GfxFixedList.Add(LFlag);
            MenuStats.LFlag = LFlag;

            // label fusion 1
            Txt Fusion1 = new Txt(CommonCode.TranslateText(75) + " 1", new Point(128, 30), "__Fusion1", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
            StatsImg.Child.Add(Fusion1);
            Manager.manager.GfxFixedList.Add(Fusion1);
            MenuStats.Fusion1 = Fusion1;

            // label fusion 2
            Txt Fusion2 = new Txt(CommonCode.TranslateText(75) + " 2", new Point(184, 30), "__Fusion2", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
            StatsImg.Child.Add(Fusion2);
            Manager.manager.GfxFixedList.Add(Fusion2);
            MenuStats.Fusion2 = Fusion2;

            // label niveau
            Txt NiveauGaugeTxt = new Txt(CommonCode.TranslateText(50), new Point(4, 52), "__NiveauGauge", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(NiveauGaugeTxt);
            Manager.manager.GfxFixedList.Add(NiveauGaugeTxt);
            MenuStats.NiveauGaugeTxt = NiveauGaugeTxt;

            // rectangle gauge 1
            Rec NiveauGaugeRec1 = new Rec(Brushes.Black, new Point(68, 52), new Size(260, 12), "__NiveauGaugeRec1", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(NiveauGaugeRec1);
            Manager.manager.GfxFixedList.Add(NiveauGaugeRec1);

            // rectangle gauge 2
            Rec NiveauGaugeRec2 = new Rec(Brushes.White, new Point(69, 53), new Size(258, 10), "__NiveauGaugeRec2", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(NiveauGaugeRec2);
            Manager.manager.GfxFixedList.Add(NiveauGaugeRec2);
            MenuStats.NiveauGaugeRec2 = NiveauGaugeRec2;

            // rectangle gauge 3 selon le pourcentage
            Rec NiveauGaugeRecPercent = new Rec(Brushes.BlueViolet, new Point(69, 53), new Size(0, 10), "__NiveauGaugePercent", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(NiveauGaugeRecPercent);
            Manager.manager.GfxFixedList.Add(NiveauGaugeRecPercent);
            MenuStats.NiveauGaugeRecPercent = NiveauGaugeRecPercent;

            // nombre de point gauge
            Txt NiveauGaugeTxtCurrent = new Txt("0", new Point(0, 0), "__NiveauGaugeTxtCurrent", Manager.TypeGfx.Top, true, new Font("Verdana", 6, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(NiveauGaugeTxtCurrent);
            Manager.manager.GfxFixedList.Add(NiveauGaugeTxtCurrent);
            MenuStats.NiveauGaugeTxtCurrent = NiveauGaugeTxtCurrent;

            // affinité elementaire
            Txt AffiniteElementaireTxt = new Txt(CommonCode.TranslateText(76), new Point(8, 65), "__AffiniteElementaireTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), CommonCode.spellAreaNotAllowedColor);
            StatsImg.Child.Add(AffiniteElementaireTxt);
            Manager.manager.GfxFixedList.Add(AffiniteElementaireTxt);
            MenuStats.AffiniteElementaireTxt = AffiniteElementaireTxt;

            // (terre)
            Txt terreStats = new Txt(CommonCode.TranslateText(77), new Point(70, 90), "__terreTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 8), new SolidBrush(Color.FromArgb(142, 91, 21)));
            StatsImg.Child.Add(terreStats);
            Manager.manager.GfxFixedList.Add(terreStats);
            MenuStats.terreStats = terreStats;

            // (feu)
            Txt FeuStats = new Txt(CommonCode.TranslateText(78), new Point(70, 108), "__FeuStats", Manager.TypeGfx.Top, true, new Font("Verdana", 8), new SolidBrush(Color.FromArgb(198, 0, 0)));
            StatsImg.Child.Add(FeuStats);
            Manager.manager.GfxFixedList.Add(FeuStats);
            MenuStats.FeuStats = FeuStats;

            // vent
            Txt VentStats = new Txt(CommonCode.TranslateText(79), new Point(70, 127), "__VentStats", Manager.TypeGfx.Top, true, new Font("Verdana", 8), new SolidBrush(Color.FromArgb(0, 197, 125)));
            StatsImg.Child.Add(VentStats);
            Manager.manager.GfxFixedList.Add(VentStats);
            MenuStats.VentStats = VentStats;

            // Foudre
            Txt FoudreStats = new Txt(CommonCode.TranslateText(80), new Point(70, 145), "__FoudreStats", Manager.TypeGfx.Top, true, new Font("Verdana", 8), new SolidBrush(Color.FromArgb(215, 203, 0)));
            StatsImg.Child.Add(FoudreStats);
            Manager.manager.GfxFixedList.Add(FoudreStats);
            MenuStats.FoudreStats = FoudreStats;

            // eau
            Txt EauStats = new Txt(CommonCode.TranslateText(81), new Point(70, 163), "__EauStats", Manager.TypeGfx.Top, true, new Font("Verdana", 8), new SolidBrush(Color.FromArgb(12, 133, 255)));
            StatsImg.Child.Add(EauStats);
            Manager.manager.GfxFixedList.Add(EauStats);
            MenuStats.EauStats = EauStats;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // jauge de lvl Terre
            Rec TerreLvlGauge = new Rec(new SolidBrush(Color.FromArgb(142, 91, 21)), new Point(140, 91), new Size(0, 14), "__TerreLvlGauge", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(TerreLvlGauge);
            Manager.manager.GfxFixedList.Add(TerreLvlGauge);
            MenuStats.TerreLvlGauge = TerreLvlGauge;

            // jauge de lvl Feu
            Rec FeuLvlGauge = new Rec(new SolidBrush(Color.FromArgb(198, 0, 0)), new Point(140, 110), new Size(0, 14), "__FeuLvlGauge", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(FeuLvlGauge);
            Manager.manager.GfxFixedList.Add(FeuLvlGauge);
            MenuStats.FeuLvlGauge = FeuLvlGauge;

            // jauge de lvl Vent
            Rec VentLvlGauge = new Rec(new SolidBrush(Color.FromArgb(0, 197, 125)), new Point(140, 128), new Size(0, 14), "__VentLvlGauge", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(VentLvlGauge);
            Manager.manager.GfxFixedList.Add(VentLvlGauge);
            MenuStats.VentLvlGauge = VentLvlGauge;

            // jauge de lvl Foudre
            Rec FoudreLvlGauge = new Rec(new SolidBrush(Color.FromArgb(215, 203, 0)), new Point(140, 146), new Size(0, 14), "__FoudreLvlGauge", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(FoudreLvlGauge);
            Manager.manager.GfxFixedList.Add(FoudreLvlGauge);
            MenuStats.FoudreLvlGauge = FoudreLvlGauge;

            // jauge de lvl Eau
            Rec EauLvlGauge = new Rec(new SolidBrush(Color.FromArgb(12, 133, 255)), new Point(140, 164), new Size(0, 14), "__EauLvlGauge", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(EauLvlGauge);
            Manager.manager.GfxFixedList.Add(EauLvlGauge);
            MenuStats.EauLvlGauge = EauLvlGauge;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // regle de lvl terre
            Bmp TerreLvlRegle = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(140, 91), "__TerreLvlRegle", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 14));
            StatsImg.Child.Add(TerreLvlRegle);
            Manager.manager.GfxFixedList.Add(TerreLvlRegle);
            MenuStats.TerreLvlRegle = TerreLvlRegle;

            // regle de lvl feu
            Bmp FeuLvlRegle = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(140, 110), "__FeuLvlRegle", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 15));
            StatsImg.Child.Add(FeuLvlRegle);
            Manager.manager.GfxFixedList.Add(FeuLvlRegle);
            MenuStats.FeuLvlRegle = FeuLvlRegle;

            // regle de lvl vent
            Bmp VentLvlRegle = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(140, 128), "__VentLvlRegle", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 16));
            StatsImg.Child.Add(VentLvlRegle);
            Manager.manager.GfxFixedList.Add(VentLvlRegle);
            MenuStats.VentLvlRegle = VentLvlRegle;

            // regle de lvl foudre
            Bmp FoudreLvlRegle = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(140, 146), "__FoudreLvlRegle", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 17));
            StatsImg.Child.Add(FoudreLvlRegle);
            Manager.manager.GfxFixedList.Add(FoudreLvlRegle);
            MenuStats.FoudreLvlRegle = FoudreLvlRegle;

            // regle de lvl eau
            Bmp EauLvlRegle = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(140, 164), "__EauLvlRegle", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 18));
            StatsImg.Child.Add(EauLvlRegle);
            Manager.manager.GfxFixedList.Add(EauLvlRegle);
            MenuStats.EauLvlRegle = EauLvlRegle;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // label lvl1 regle
            Txt Lvl1RegleTxt = new Txt(CommonCode.TranslateText(82), new Point(132, 80), "__Lvl1RegleTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl1RegleTxt);
            Manager.manager.GfxFixedList.Add(Lvl1RegleTxt);
            MenuStats.Lvl1RegleTxt = Lvl1RegleTxt;

            // label lvl2 regle
            Txt Lvl2RegleTxt = new Txt(CommonCode.TranslateText(83), new Point(168, 80), "__Lvl2RegleTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl2RegleTxt);
            Manager.manager.GfxFixedList.Add(Lvl2RegleTxt);
            MenuStats.Lvl2RegleTxt = Lvl2RegleTxt;

            // label lvl3 regle
            Txt Lvl3RegleTxt = new Txt(CommonCode.TranslateText(84), new Point(204, 80), "__Lvl3RegleTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl3RegleTxt);
            Manager.manager.GfxFixedList.Add(Lvl3RegleTxt);
            MenuStats.Lvl3RegleTxt = Lvl3RegleTxt;

            // label lvl4 regle
            Txt Lvl4RegleTxt = new Txt(CommonCode.TranslateText(85), new Point(240, 80), "__Lvl4RegleTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl4RegleTxt);
            Manager.manager.GfxFixedList.Add(Lvl4RegleTxt);
            MenuStats.Lvl4RegleTxt = Lvl4RegleTxt;

            // label lvl5 regle
            Txt Lvl5RegleTxt = new Txt(CommonCode.TranslateText(86), new Point(276, 80), "__EauLvlRegleTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl5RegleTxt);
            Manager.manager.GfxFixedList.Add(Lvl5RegleTxt);
            MenuStats.Lvl5RegleTxt = Lvl5RegleTxt;

            // label lvl6 regle
            Txt Lvl6RegleTxt = new Txt(CommonCode.TranslateText(87), new Point(308, 80), "__Lvl6RegleTxt", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl6RegleTxt);
            Manager.manager.GfxFixedList.Add(Lvl6RegleTxt);
            MenuStats.Lvl6RegleTxt = Lvl6RegleTxt;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // label lvl2 regle points
            Txt Lvl2ReglePts = new Txt("", new Point(168, 68), "__Lvl2ReglePts", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl2ReglePts);
            Manager.manager.GfxFixedList.Add(Lvl2ReglePts);
            MenuStats.Lvl2ReglePts = Lvl2ReglePts;

            // label lvl3 regle points
            Txt Lvl3ReglePts = new Txt("", new Point(204, 68), "__Lvl3ReglePts", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl3ReglePts);
            Manager.manager.GfxFixedList.Add(Lvl3ReglePts);
            MenuStats.Lvl3ReglePts = Lvl3ReglePts;

            // label lvl4 regle points
            Txt Lvl4ReglePts = new Txt("", new Point(240, 68), "__Lvl4ReglePts", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl4ReglePts);
            Manager.manager.GfxFixedList.Add(Lvl4ReglePts);
            MenuStats.Lvl4ReglePts = Lvl4ReglePts;

            // label lvl5 regle points
            Txt Lvl5ReglePts = new Txt("", new Point(276, 68), "__Lvl5ReglePts", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl5ReglePts);
            Manager.manager.GfxFixedList.Add(Lvl5ReglePts);
            MenuStats.Lvl5ReglePts = Lvl5ReglePts;

            // label lvl6 regle points
            Txt Lvl6ReglePts = new Txt("", new Point(308, 68), "__Lvl6ReglePts", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Lvl6ReglePts);
            Manager.manager.GfxFixedList.Add(Lvl6ReglePts);
            MenuStats.Lvl6ReglePts = Lvl6ReglePts;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // affichage du text valeur doton
            Txt TerrePuissance = new Txt("", new Point(150, 92), "__TerrePuissance", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(TerrePuissance);
            Manager.manager.GfxFixedList.Add(TerrePuissance);
            MenuStats.TerrePuissance = TerrePuissance;

            // affichage du text valeur katon
            Txt FeuPuissance = new Txt("", new Point(150, 110), "__FeuPuissance", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(FeuPuissance);
            Manager.manager.GfxFixedList.Add(FeuPuissance);
            MenuStats.FeuPuissance = FeuPuissance;

            // affichage du text valeur futon
            Txt VentPuissance = new Txt("", new Point(150, 128), "__VentPuissance", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(VentPuissance);
            Manager.manager.GfxFixedList.Add(VentPuissance);
            MenuStats.VentPuissance = VentPuissance;

            // affichage du text valeur foudre
            Txt FoudrePuissance = new Txt("", new Point(150, 146), "__FoudrePuissance", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(FoudrePuissance);
            Manager.manager.GfxFixedList.Add(FoudrePuissance);
            MenuStats.FoudrePuissance = FoudrePuissance;

            // affichage du text valeur eau
            Txt EauPuissance = new Txt("", new Point(150, 164), "__EauPuissance", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(EauPuissance);
            Manager.manager.GfxFixedList.Add(EauPuissance);
            MenuStats.EauPuissance = EauPuissance;

            // affichage des points lvl element
            Txt DotonLvl = new Txt("0", new Point(290, 92), "__DotonLvl", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(DotonLvl);
            Manager.manager.GfxFixedList.Add(DotonLvl);
            MenuStats.DotonLvl = DotonLvl;

            Txt KatonLvl = new Txt("0", new Point(290, 110), "__KatonLvl", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(KatonLvl);
            Manager.manager.GfxFixedList.Add(KatonLvl);
            MenuStats.KatonLvl = KatonLvl;

            Txt FutonLvl = new Txt("0", new Point(290, 128), "__FutonLvl", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(FutonLvl);
            Manager.manager.GfxFixedList.Add(FutonLvl);
            MenuStats.FutonLvl = FutonLvl;

            Txt RaitonLvl = new Txt("0", new Point(290, 146), "__RaitonLvl", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(RaitonLvl);
            Manager.manager.GfxFixedList.Add(RaitonLvl);
            MenuStats.RaitonLvl = RaitonLvl;

            Txt SuitonLvl = new Txt("0", new Point(290, 164), "__SuitonLvl", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(SuitonLvl);
            Manager.manager.GfxFixedList.Add(SuitonLvl);
            MenuStats.SuitonLvl = SuitonLvl;

            // label vita
            Txt VieLabel = new Txt(CommonCode.TranslateText(88), new Point(10, 186), "__VieLabel", Manager.TypeGfx.Top, true, new Font("Verdana", 8, FontStyle.Bold), Brushes.Red);
            StatsImg.Child.Add(VieLabel);
            Manager.manager.GfxFixedList.Add(VieLabel);
            MenuStats.VieLabel = VieLabel;

            // Rec1 Vita cadre noir
            Rec VieRec1 = new Rec(Brushes.Black, new Point(80, 186), new Size(240, 14), "__VieRec1", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(VieRec1);
            Manager.manager.GfxFixedList.Add(VieRec1);

            // Rec2 Vita cadre blanc
            Rec VieRec2 = new Rec(Brushes.White, new Point(81, 187), new Size(238, 12), "__VieRec2", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(VieRec2);
            Manager.manager.GfxFixedList.Add(VieRec2);

            // rec vita cadre rouge proporsionellement a sa vita
            Rec VieBar = new Rec(Brushes.Red, new Point(82, 188), new Size(0, 10), "__VieBar", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(VieBar);
            Manager.manager.GfxFixedList.Add(VieBar);
            MenuStats.VieBar = VieBar;

            // label point de vie
            Txt ViePts = new Txt("0", new Point(160, 186), "__ViePts", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(ViePts);
            Manager.manager.GfxFixedList.Add(ViePts);
            MenuStats.ViePts = ViePts;

            // Point de chakra PC
            Txt PC = new Txt("0", new Point(50, 202), "__PC", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Blue);
            StatsImg.Child.Add(PC);
            Manager.manager.GfxFixedList.Add(PC);
            MenuStats.PC = PC;

            // Points de mouvement
            Txt PM = new Txt("0", new Point(50, 218), "__PM", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Green);
            StatsImg.Child.Add(PM);
            Manager.manager.GfxFixedList.Add(PM);
            MenuStats.PM = PM;

            // points etendu
            Txt PE = new Txt("0", new Point(50, 234), "__PE", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), CommonCode.spellAreaNotAllowedColor);
            StatsImg.Child.Add(PE);
            Manager.manager.GfxFixedList.Add(PE);
            MenuStats.PE = PE;

            // CD coup dangereu
            Txt CD = new Txt("0", new Point(130, 202), "__CD", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Red);
            StatsImg.Child.Add(CD);
            Manager.manager.GfxFixedList.Add(CD);
            MenuStats.CD = CD;

            // invoc
            Txt Invoc = new Txt("0", new Point(130, 218), "__Invoc", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.YellowGreen);
            StatsImg.Child.Add(Invoc);
            Manager.manager.GfxFixedList.Add(Invoc);
            MenuStats.Invoc = Invoc;

            // initiative
            Txt Initiative = new Txt("0", new Point(130, 234), "__Initiative", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Brown);
            StatsImg.Child.Add(Initiative);
            Manager.manager.GfxFixedList.Add(Initiative);
            MenuStats.Initiative = Initiative;

            // puissance
            Txt Puissance = new Txt("0", new Point(210, 202), "__Puissance", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(Puissance);
            Manager.manager.GfxFixedList.Add(Puissance);
            MenuStats.Puissance = Puissance;

            // DomFix
            Txt DomFix = new Txt("0", new Point(210, 218), "__DomFix", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), CommonCode.puissanceColor);
            StatsImg.Child.Add(DomFix);
            Manager.manager.GfxFixedList.Add(DomFix);
            MenuStats.DomFix = DomFix;

            // label job 1
            Txt Job1Label = new Txt("", new Point(10, 256), "__Job1Label", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(Job1Label);
            Manager.manager.GfxFixedList.Add(Job1Label);
            MenuStats.Job1Label = Job1Label;

            // label specialité 1
            Txt Specialite1Label = new Txt("", new Point(60, 260), "__Specialite1Label", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(Specialite1Label);
            Manager.manager.GfxFixedList.Add(Specialite1Label);
            MenuStats.Specialite1Label = Specialite1Label;

            // label job 2
            Txt Job2Labe1 = new Txt("", new Point(120, 256), "__Job2Labe1", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(Job2Labe1);
            Manager.manager.GfxFixedList.Add(Job2Labe1);
            MenuStats.Job2Labe1 = Job2Labe1;

            // label specialité 2
            Txt Specialite2Label = new Txt("", new Point(170, 260), "__Specialite2Label", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(Specialite2Label);
            Manager.manager.GfxFixedList.Add(Specialite2Label);
            MenuStats.Specialite2Label = Specialite2Label;

            // job1  + specialite1 + job2 + specialite2///////////////////////
            //
            //  
            //
            //////////////////////////////

            // label poid
            Txt PoidLabel = new Txt("", new Point(10, 315), "__PoidLabel", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Black);
            StatsImg.Child.Add(PoidLabel);
            Manager.manager.GfxFixedList.Add(PoidLabel);
            MenuStats.PoidLabel = PoidLabel;

            // rec1 poid
            Rec PoidRec1 = new Rec(Brushes.Black, new Point(50, 316), new Size(120, 10), "__PoidRec1", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(PoidRec1);
            Manager.manager.GfxFixedList.Add(PoidRec1);

            // rec2 poid
            Rec PoidRec2 = new Rec(Brushes.White, new Point(51, 317), new Size(118, 8), "__PoidRec2", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(PoidRec2);
            Manager.manager.GfxFixedList.Add(PoidRec2);

            // rec poid
            Rec PoidRec = new Rec(Brushes.Orange, new Point(52, 318), new Size(0, 6), "__PoidRec", Manager.TypeGfx.Top, true);
            StatsImg.Child.Add(PoidRec);
            Manager.manager.GfxFixedList.Add(PoidRec);
            MenuStats.PoidRec = PoidRec;

            // label Poid
            Txt Poid = new Txt("", new Point(0, 315), "__PoidLabel", Manager.TypeGfx.Top, true, new Font("Verdana", 6), Brushes.Black);
            StatsImg.Child.Add(Poid);
            Manager.manager.GfxFixedList.Add(Poid);
            MenuStats.Poid = Poid;

            // monais Ryo label
            Txt RyoLabel = new Txt("Ryô (両):", new Point(180, 315), "__RyoL", Manager.TypeGfx.Top, true, new Font("Verdana", 7), Brushes.Black);
            StatsImg.Child.Add(RyoLabel);
            Manager.manager.GfxFixedList.Add(RyoLabel);

            // ryo
            Txt Ryo = new Txt("", new Point(230, 314), "__Ryo", Manager.TypeGfx.Top, true, new Font("Verdana", 7, FontStyle.Bold), Brushes.Maroon);
            StatsImg.Child.Add(Ryo);
            Manager.manager.GfxFixedList.Add(Ryo);
            MenuStats.Ryo = Ryo;

            // bobo doton
            Bmp resiDoton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(10, 330), "__resiDoton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 40));
            StatsImg.Child.Add(resiDoton);
            Manager.manager.GfxFixedList.Add(resiDoton);

            Txt resiDotonTxt = new Txt("0", new Point(20, 330), "__resiDotonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(resiDotonTxt);
            Manager.manager.GfxFixedList.Add(resiDotonTxt);
            MenuStats.resiDotonTxt = resiDotonTxt;

            // bobo katon
            Bmp resiKaton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(70, 330), "__resiKaton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 41));
            StatsImg.Child.Add(resiKaton);
            Manager.manager.GfxFixedList.Add(resiKaton);

            Txt resiKatonTxt = new Txt("0", new Point(80, 330), "__resiKatonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(resiKatonTxt);
            Manager.manager.GfxFixedList.Add(resiKatonTxt);
            MenuStats.resiKatonTxt = resiKatonTxt;

            // bobo futon
            Bmp resiFuton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(130, 330), "__resiFuton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 42));
            StatsImg.Child.Add(resiFuton);
            Manager.manager.GfxFixedList.Add(resiFuton);

            Txt resiFutonTxt = new Txt("0", new Point(140, 330), "__resiFutonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(resiFutonTxt);
            Manager.manager.GfxFixedList.Add(resiFutonTxt);
            MenuStats.resiFutonTxt = resiFutonTxt;

            // bobo Raiton
            Bmp resiRaiton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(190, 330), "__resiRaiton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 43));
            StatsImg.Child.Add(resiRaiton);
            Manager.manager.GfxFixedList.Add(resiRaiton);

            Txt resiRaitonTxt = new Txt("0", new Point(200, 330), "__resiRaitonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(resiRaitonTxt);
            Manager.manager.GfxFixedList.Add(resiRaitonTxt);
            MenuStats.resiRaitonTxt = resiRaitonTxt;

            // bobo suiton
            Bmp resiSuiton = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(250, 330), "__resiSuiton", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 44));
            StatsImg.Child.Add(resiSuiton);
            Manager.manager.GfxFixedList.Add(resiSuiton);

            Txt resiSuitonTxt = new Txt("0", new Point(260, 330), "__resiSuitonTxt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(resiSuitonTxt);
            Manager.manager.GfxFixedList.Add(resiSuitonTxt);
            MenuStats.resiSuitonTxt = resiSuitonTxt;

            // esquive PC
            Bmp __esquivePC = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(6, 346), "__esquivePC", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 45));
            StatsImg.Child.Add(__esquivePC);
            Manager.manager.GfxFixedList.Add(__esquivePC);

            Txt __esquivePC_Txt = new Txt("0", new Point(30, 346), "__esquivePC_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__esquivePC_Txt);
            Manager.manager.GfxFixedList.Add(__esquivePC_Txt);
            MenuStats.__esquivePC_Txt = __esquivePC_Txt;

            Bmp __esquivePM = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(70, 346), "__esquivePM", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 46));
            StatsImg.Child.Add(__esquivePM);
            Manager.manager.GfxFixedList.Add(__esquivePM);

            //Txt __esquivePM_Txt
            Txt __esquivePM_Txt = new Txt("0", new Point(100, 346), "__esquivePM_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__esquivePM_Txt);
            Manager.manager.GfxFixedList.Add(__esquivePM_Txt);
            MenuStats.__esquivePM_Txt = __esquivePM_Txt;

            //esquivePE
            Bmp __esquivePE = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(6, 358), "__esquivePE", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 47));
            StatsImg.Child.Add(__esquivePE);
            Manager.manager.GfxFixedList.Add(__esquivePE);

            //Txt __esquivePE
            Txt __esquivePE_Txt = new Txt("0", new Point(30, 358), "__esquivePE_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__esquivePE_Txt);
            Manager.manager.GfxFixedList.Add(__esquivePE_Txt);
            MenuStats.__esquivePE_Txt = __esquivePE_Txt;

            //esquiveCD
            Bmp __esquiveCD = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(70, 358), "__esquiveCD", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 48));
            StatsImg.Child.Add(__esquiveCD);
            Manager.manager.GfxFixedList.Add(__esquiveCD);

            //Txt __esquiveCD_Txt
            Txt __esquiveCD_Txt = new Txt("0", new Point(100, 358), "__esquiveCD_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__esquiveCD_Txt);
            Manager.manager.GfxFixedList.Add(__esquiveCD_Txt);
            MenuStats.__esquiveCD_Txt = __esquiveCD_Txt;

            // retrait PC
            Bmp __retraitPC = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(156, 346), "__retraitPC", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 49));
            StatsImg.Child.Add(__retraitPC);
            Manager.manager.GfxFixedList.Add(__retraitPC);

            Txt __retraitPC_Txt = new Txt("0", new Point(180, 346), "__retraitPC_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__retraitPC_Txt);
            Manager.manager.GfxFixedList.Add(__retraitPC_Txt);
            MenuStats.__retraitPC_Txt = __retraitPC_Txt;

            Bmp __retraitPM = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(220, 346), "__retraitPM", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 50));
            StatsImg.Child.Add(__retraitPM);
            Manager.manager.GfxFixedList.Add(__retraitPM);

            //Txt __retraitPM_Txt
            Txt __retraitPM_Txt = new Txt("0", new Point(250, 346), "__retraitPM_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__retraitPM_Txt);
            Manager.manager.GfxFixedList.Add(__retraitPM_Txt);
            MenuStats.__retraitPM_Txt = __retraitPM_Txt;

            //__retraitPE
            Bmp __retraitPE = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(156, 358), "__retraitPE", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 51));
            StatsImg.Child.Add(__retraitPE);
            Manager.manager.GfxFixedList.Add(__retraitPE);

            //Txt __retraitPE
            Txt __retraitPE_Txt = new Txt("0", new Point(180, 358), "__retraitPE_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__retraitPE_Txt);
            Manager.manager.GfxFixedList.Add(__retraitPE_Txt);
            MenuStats.__retraitPE_Txt = __retraitPE_Txt;

            //__retraitCD
            Bmp __retraitCD = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(220, 358), "__retraitCD", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 52));
            StatsImg.Child.Add(__retraitCD);
            Manager.manager.GfxFixedList.Add(__retraitCD);

            //Txt __retraitCD_Txt
            Txt __retraitCD_Txt = new Txt("0", new Point(250, 358), "__retraitCD_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__retraitCD_Txt);
            Manager.manager.GfxFixedList.Add(__retraitCD_Txt);
            MenuStats.__retraitCD_Txt = __retraitCD_Txt;

            // evasion
            Bmp __evasion = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(280, 344), "__evasion", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 53));
            StatsImg.Child.Add(__evasion);
            Manager.manager.GfxFixedList.Add(__evasion);

            Txt __evasion_Txt = new Txt("0", new Point(305, 344), "__evasion_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__evasion_Txt);
            Manager.manager.GfxFixedList.Add(__evasion_Txt);

            // blockage
            Bmp __blockage = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(280, 358), "__blockage", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 54));
            StatsImg.Child.Add(__blockage);
            Manager.manager.GfxFixedList.Add(__blockage);

            Txt __blockage_Txt = new Txt("0", new Point(310, 358), "__blockage_Txt", Manager.TypeGfx.Top, true, new Font("verdana", 7), Brushes.Black);
            StatsImg.Child.Add(__blockage_Txt);
            Manager.manager.GfxFixedList.Add(__blockage_Txt);
            #endregion
        }
        static void ChatArea_KeyUp(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.C)
            {
                HudHandle.ChatTextBox.Focus();
            }
            else
            {
                string alpha = "azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN123456789+-*/²&é\"'(-è_çà)=~#{[|`\\^@]}¤£¨µ%§/.?<>";
                string key = e.KeyCode.ToString();
                if (alpha.IndexOf(e.KeyCode.ToString()) != -1)
                {
                    HudHandle.ChatTextBox.Text = e.KeyCode.ToString();
                    HudHandle.ChatTextBox.Focus();
                    HudHandle.ChatTextBox.Select(1, 0);
                }
            }
        }
        public static void DefaultCursorRec(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        public static void DefaultCursorRec(Txt rec, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        public static void HandCursorRec(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        public static void HandCursorTxt(Txt txt, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
    }
}