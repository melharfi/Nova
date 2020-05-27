using MELHARFI.Gfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MELHARFI;

namespace MMORPG
{
    public partial class ChatBox : Form
    {
        static NHunspellExtender.NHunspellTextBoxExtender nHunspellTextBoxExtender1;
        // double buffering d'un control en particulier
        public static void SetDoubleBuffered(Control c)
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
        }
        protected override CreateParams CreateParams
        {
            ////// faire disparaitre la form du menu Alt + Tab pour éviter d'afficher la zone de chat tout seul
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;
                return Params;
            }
        }
        public ChatBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.Invalidate();
            this.SuspendLayout();

            nHunspellTextBoxExtender1 = new NHunspellExtender.NHunspellTextBoxExtender();
            ((System.ComponentModel.ISupportInitialize)(nHunspellTextBoxExtender1)).BeginInit();

            // 
            // nHunspellTextBoxExtender1
            // 
            nHunspellTextBoxExtender1.Language = "English";
            nHunspellTextBoxExtender1.MaintainUserChoice = true;
            nHunspellTextBoxExtender1.ShortcutKey = System.Windows.Forms.Shortcut.F7;
            nHunspellTextBoxExtender1.SpellAsYouType = true;
            ((System.ComponentModel.ISupportInitialize)(nHunspellTextBoxExtender1)).EndInit();

            /////////////////////////////////////////////////////////////////////////
            RichTextBoxEx ChatArea = new RichTextBoxEx(MainForm.transparentChatBox);
            ChatArea.Name = "ChatArea";
            ChatArea.Width = 400;
            ChatArea.Height = 128;
            ChatArea.Location = new Point(0, 0);

            if (!MainForm.transparentChatBox)
            {
                byte part1 = byte.Parse(MainForm.ColorBgChatBox.Split(',')[0]);
                byte part2 = byte.Parse(MainForm.ColorBgChatBox.Split(',')[1]);
                byte part3 = byte.Parse(MainForm.ColorBgChatBox.Split(',')[2]);

                ChatArea.BackColor = Color.FromArgb(part1, part2, part3);
            }
            ChatArea.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            ChatArea.BorderStyle = BorderStyle.None;
            ChatArea.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ChatArea.ReadOnly = true;
            ChatArea.Visible = false;
            ChatArea.KeyUp += ChatArea_KeyUp;
            ChatArea.ForeColor = Color.Black;
            ChatArea.TextChanged += ChatArea_TextChanged;
            ChatArea.SendToBack();
            ChatArea.LinkClicked += ChatArea_LinkClicked;
            ChatArea.MouseUp += ChatArea_MouseUp;
            SetDoubleBuffered(ChatArea);
            this.Controls.Add(ChatArea);

            if (CommonCode.langue == 2)
                ChatArea.RightToLeft = RightToLeft.Yes;
            HudHandle.ChatArea = ChatArea;
            this.VisibleChanged += ChatBox_VisibleChanged;
            this.BringToFront();

            if (CommonCode.langue == 2)
                ChatArea.RightToLeft = RightToLeft.Yes;

            if (MainForm.transparentChatBox)
            {
                string path = "";
                if (System.IO.File.Exists(@"gfx\general\UI\chatboxBG.png"))
                    path = @"gfx\general\UI\chatboxBG.png";
                else if (System.IO.File.Exists(@"gfx\general\UI\chatboxBG.bmp"))
                    path = @"gfx\general\UI\chatboxBG.bmp";
                else if (System.IO.File.Exists(@"gfx\general\UI\chatboxBG.jpg"))
                    path = @"gfx\general\UI\chatboxBG.jpg";
                else if (System.IO.File.Exists(@"gfx\general\UI\chatboxBG.png"))
                    path = @"gfx\general\UI\chatboxBG.png";

                if (path == "")
                    MainForm.transparentChatBox = false;
                else
                {
                    // image en arriere plant du chatarea
                    this.BackgroundImage = Image.FromFile(path);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }

        void ChatLink_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            HudHandle.ChatTextBox.AppendText("[l/][\\l]");
            HudHandle.ChatTextBox.SelectionStart = HudHandle.ChatTextBox.Text.IndexOf("[l/]") + 4;
            HudHandle.ChatTextBox.SelectionLength = 0;
            HudHandle.ChatTextBox.Focus();
        }
        void ChatArea_KeyUp(object sender, KeyEventArgs e)
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
        void SendBtn_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            if (HudHandle.SelectedCanalTxt.Text == "P" && HudHandle.ChatTextBox.TextLength > CommonCode.MyPlayerInfo.instance.pseudo.Length && HudHandle.ChatTextBox.Text.Substring(0, CommonCode.MyPlayerInfo.instance.pseudo.Length) == CommonCode.MyPlayerInfo.instance.pseudo)
            {
                ChatArea.AppendText((ChatArea.Text == "") ? "" : "\n");
                ChatArea.SelectionStart = ChatArea.TextLength;
                ChatArea.SelectionLength = 0;
                ChatArea.SelectionColor = Color.Red;
                ChatArea.AppendText(CommonCode.TranslateText(28));
                ChatArea.SelectionColor = ChatArea.ForeColor;
            }
            else if (HudHandle.ChatTextBox.Text != "")
            {
                if (HudHandle.SelectedCanalTxt.Text == "P" && HudHandle.ChatTextBox.Text.Split('#').Count() == 2)
                {
                    // affichage du texte envoyé en mp en chat general
                    ChatArea.AppendText((ChatArea.Text == "") ? "" : "\n");
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Green;

                    // pour recuperer le reste du texte quand l'utilisateur ajoute un # qui s'ajoute a celui du pseudo#...#
                    string tmpMsg = "";
                    if (HudHandle.ChatTextBox.Text.Split('#').Length >= 2)
                    {
                        for (int cnt = 1; cnt < HudHandle.ChatTextBox.Text.Split('#').Length; cnt++)
                            tmpMsg += HudHandle.ChatTextBox.Text.Split('#')[cnt] + "#";

                        tmpMsg = tmpMsg.Substring(0, tmpMsg.Length - 1);
                    }

                    ///////////////////// l'heur
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Green;
                    ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                    //////////////////////////////////////////////////////

                    ChatArea.AppendText(CommonCode.TranslateText(3) + " ");
                    ChatArea.InsertLink(" " + HudHandle.ChatTextBox.Text.Split('#')[0]);
                    ChatArea.AppendText(" : ");

                    // recherche l'existance d'un lien dans le text
                    if (tmpMsg.IndexOf("[l/]") != -1 && tmpMsg.IndexOf("[\\l]") != -1 && tmpMsg.Length > 12)
                    {
                        // le texte contiens un lien
                        // affichage du texte qui precede la balise ouverture de lien
                        ChatArea.AppendText(tmpMsg.Substring(0, tmpMsg.IndexOf("[l/]")) + " ");
                        string tmpMsg2 = tmpMsg.Substring(tmpMsg.IndexOf("[l/]") + 4, tmpMsg.IndexOf("[\\l]") - tmpMsg.IndexOf("[l/]") - 4);
                        ChatArea.InsertLink(tmpMsg2);
                        int pos1 = tmpMsg.IndexOf("[\\l]") + 4;
                        string str1 = tmpMsg.Substring(pos1, tmpMsg.Length - pos1);
                        ChatArea.AppendText(str1);
                    }
                    else
                        ChatArea.AppendText(tmpMsg);

                    ChatArea.SelectionColor = ChatArea.ForeColor;
                }

                Network.SendMessage("cmd•ChatMessage•" + HudHandle.SelectedCanalTxt.Text + "•" + HudHandle.ChatTextBox.Text, true);

                // enregistrement des messages sur le chatlog
                MainForm.ChatLog.Add(HudHandle.ChatTextBox.Text + "•" + HudHandle.SelectedCanalTxt.Text);

                HudHandle.ChatTextBox.Text = "";
                HudHandle.ChannelState("G");
            }
        }
        void ChatArea_TextChanged(object sender, EventArgs e)
        {
            if (HudHandle.ChatCursorDown.Checked)
            {
                HudHandle.ChatArea.SelectionStart = HudHandle.ChatArea.TextLength;
                HudHandle.ChatArea.ScrollToCaret();
            }
        }
        void ChatArea_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (e.LinkText.Split('#').Length > 1 && e.LinkText.Split('#')[1] == "P")
            {
                // passage au mode private
                HudHandle.ChannelState("P");

                // insertion du nom de l'user qui sera contacté par mp
                HudHandle.ChatTextBox.Text = e.LinkText.Split('#')[0] + "#";
                HudHandle.ChatTextBox.Focus();
                HudHandle.ChatTextBox.SelectionStart = HudHandle.ChatTextBox.TextLength;
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(e.LinkText);
                }
                catch
                {
                    HudHandle.ChatArea.AppendText((HudHandle.ChatArea.Text == "") ? "" : "\n");
                    HudHandle.ChatArea.SelectionStart = HudHandle.ChatArea.TextLength;
                    HudHandle.ChatArea.SelectionLength = 0;
                    HudHandle.ChatArea.SelectionColor = Color.Red;
                    HudHandle.ChatArea.AppendText(CommonCode.TranslateText(68));
                    HudHandle.ChatArea.SelectionColor = HudHandle.ChatArea.ForeColor;
                }
            }
        }
        void ChatArea_MouseUp(object sender, MouseEventArgs e)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            // bouton droit sur la zone de chat
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem = new MenuItem(CommonCode.TranslateText(123));

                // menu COPIER
                menuItem.Shortcut = Shortcut.CtrlC;
                menuItem.ShowShortcut = true;
                if (ChatArea.SelectedText != "")
                    menuItem.Click += CopyAction;
                else
                    menuItem.Enabled = false;
                contextMenu.MenuItems.Add(menuItem);

                // menu effacer la zone de chat
                menuItem = new MenuItem(CommonCode.TranslateText(124));
                menuItem.Shortcut = Shortcut.CtrlD;
                menuItem.Click += DelAction;
                contextMenu.MenuItems.Add(menuItem);

                // selectionner tous
                menuItem = new MenuItem(CommonCode.TranslateText(125));
                menuItem.Shortcut = Shortcut.CtrlA;
                menuItem.Click += SelectAllAction;
                contextMenu.MenuItems.Add(menuItem);

                contextMenu.MenuItems.Add("-");

                menuItem = new MenuItem(CommonCode.TranslateText(126));
                menuItem.Click += ChatDirection;
                contextMenu.MenuItems.Add(menuItem);

                ChatArea.ContextMenu = contextMenu;
            }
        }
        void DelAction(object sender, EventArgs e)
        {
            // Supprimer
            HudHandle.ChatArea.Clear();
        }
        void CopyAction(object sender, EventArgs e)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;
            // copier
            Clipboard.SetData(DataFormats.Text, ChatArea.SelectedText);
        }
        void SelectAllAction(object sender, EventArgs e)
        {
            // selectionner tout sur le menu contextuel du chatarea
            HudHandle.ChatArea.SelectAll();
        }
        void ChatDirection(object sender, EventArgs e)
        {
            if (HudHandle.ChatArea.RightToLeft == RightToLeft.No)
                HudHandle.ChatArea.RightToLeft = RightToLeft.Yes;
            else
                HudHandle.ChatArea.RightToLeft = RightToLeft.No;
        }
        private void ChatBox_VisibleChanged(object sender, EventArgs e)
        {
            if (Manager.manager.mainForm.ContainsFocus && Manager.manager.mainForm.TopLevel)
                this.Visible = true;
        }
        private void ChatBox_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.None;
        }
    }
}
