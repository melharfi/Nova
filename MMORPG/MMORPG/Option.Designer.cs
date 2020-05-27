namespace MMORPG
{
    partial class OptionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionForm));
            this.ClosePB = new System.Windows.Forms.PictureBox();
            this.LangTab = new System.Windows.Forms.TabPage();
            this.OnOffPic = new System.Windows.Forms.PictureBox();
            this.SpellCheckerL = new System.Windows.Forms.Label();
            this.DefaultLangL = new System.Windows.Forms.Label();
            this.DefaultLang = new System.Windows.Forms.Label();
            this.InstalledLangL = new System.Windows.Forms.Label();
            this.InstalledLang = new System.Windows.Forms.ComboBox();
            this.AddLanguageL = new System.Windows.Forms.Label();
            this.AddLang = new System.Windows.Forms.PictureBox();
            this.RemoveLang = new System.Windows.Forms.PictureBox();
            this.Interface = new System.Windows.Forms.TabPage();
            this.chatzone = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.backgroundPB = new System.Windows.Forms.PictureBox();
            this.transparentCB = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.colorSelected = new System.Windows.Forms.Panel();
            this.ColorLB = new System.Windows.Forms.Label();
            this.ConnexionTab = new System.Windows.Forms.TabPage();
            this.ServerIPL = new System.Windows.Forms.Label();
            this.ServerPortL = new System.Windows.Forms.Label();
            this.ipserver = new System.Windows.Forms.TextBox();
            this.portserver = new System.Windows.Forms.TextBox();
            this.savePic = new System.Windows.Forms.PictureBox();
            this.ResetPB = new System.Windows.Forms.PictureBox();
            this.OptionFormTab = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePB)).BeginInit();
            this.LangTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OnOffPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddLang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoveLang)).BeginInit();
            this.Interface.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundPB)).BeginInit();
            this.panel2.SuspendLayout();
            this.ConnexionTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.savePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResetPB)).BeginInit();
            this.OptionFormTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClosePB
            // 
            this.ClosePB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClosePB.Image = ((System.Drawing.Image)(resources.GetObject("ClosePB.Image")));
            this.ClosePB.Location = new System.Drawing.Point(313, 6);
            this.ClosePB.Name = "ClosePB";
            this.ClosePB.Size = new System.Drawing.Size(15, 15);
            this.ClosePB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ClosePB.TabIndex = 1;
            this.ClosePB.TabStop = false;
            this.ClosePB.Click += new System.EventHandler(this.ClosePB_Click);
            // 
            // LangTab
            // 
            this.LangTab.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.LangTab.Controls.Add(this.RemoveLang);
            this.LangTab.Controls.Add(this.AddLang);
            this.LangTab.Controls.Add(this.AddLanguageL);
            this.LangTab.Controls.Add(this.InstalledLang);
            this.LangTab.Controls.Add(this.InstalledLangL);
            this.LangTab.Controls.Add(this.DefaultLang);
            this.LangTab.Controls.Add(this.DefaultLangL);
            this.LangTab.Controls.Add(this.SpellCheckerL);
            this.LangTab.Controls.Add(this.OnOffPic);
            this.LangTab.Location = new System.Drawing.Point(4, 22);
            this.LangTab.Name = "LangTab";
            this.LangTab.Padding = new System.Windows.Forms.Padding(3);
            this.LangTab.Size = new System.Drawing.Size(313, 131);
            this.LangTab.TabIndex = 2;
            this.LangTab.Text = "Langue";
            // 
            // OnOffPic
            // 
            this.OnOffPic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OnOffPic.Image = global::MMORPG.Properties.Resources.off;
            this.OnOffPic.Location = new System.Drawing.Point(167, 5);
            this.OnOffPic.Name = "OnOffPic";
            this.OnOffPic.Size = new System.Drawing.Size(50, 18);
            this.OnOffPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.OnOffPic.TabIndex = 13;
            this.OnOffPic.TabStop = false;
            this.OnOffPic.Click += new System.EventHandler(this.OnOffPic_Click);
            // 
            // SpellCheckerL
            // 
            this.SpellCheckerL.AutoSize = true;
            this.SpellCheckerL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpellCheckerL.Location = new System.Drawing.Point(6, 7);
            this.SpellCheckerL.Name = "SpellCheckerL";
            this.SpellCheckerL.Size = new System.Drawing.Size(151, 13);
            this.SpellCheckerL.TabIndex = 11;
            this.SpellCheckerL.Text = "Correction Orthographe :";
            // 
            // DefaultLangL
            // 
            this.DefaultLangL.AutoSize = true;
            this.DefaultLangL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DefaultLangL.Location = new System.Drawing.Point(6, 28);
            this.DefaultLangL.Name = "DefaultLangL";
            this.DefaultLangL.Size = new System.Drawing.Size(120, 13);
            this.DefaultLangL.TabIndex = 14;
            this.DefaultLangL.Text = "Langue par défaut :";
            // 
            // DefaultLang
            // 
            this.DefaultLang.AutoSize = true;
            this.DefaultLang.ForeColor = System.Drawing.Color.Green;
            this.DefaultLang.Location = new System.Drawing.Point(169, 28);
            this.DefaultLang.Name = "DefaultLang";
            this.DefaultLang.Size = new System.Drawing.Size(21, 13);
            this.DefaultLang.TabIndex = 15;
            this.DefaultLang.Text = "Fr";
            // 
            // InstalledLangL
            // 
            this.InstalledLangL.AutoSize = true;
            this.InstalledLangL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstalledLangL.Location = new System.Drawing.Point(6, 57);
            this.InstalledLangL.Name = "InstalledLangL";
            this.InstalledLangL.Size = new System.Drawing.Size(113, 13);
            this.InstalledLangL.TabIndex = 16;
            this.InstalledLangL.Text = "Langues installés :";
            // 
            // InstalledLang
            // 
            this.InstalledLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InstalledLang.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.InstalledLang.FormattingEnabled = true;
            this.InstalledLang.Location = new System.Drawing.Point(151, 53);
            this.InstalledLang.Name = "InstalledLang";
            this.InstalledLang.Size = new System.Drawing.Size(80, 21);
            this.InstalledLang.TabIndex = 17;
            this.InstalledLang.SelectedValueChanged += new System.EventHandler(this.InstalledLang_SelectedValueChanged);
            // 
            // AddLanguageL
            // 
            this.AddLanguageL.AutoSize = true;
            this.AddLanguageL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddLanguageL.Location = new System.Drawing.Point(6, 98);
            this.AddLanguageL.Name = "AddLanguageL";
            this.AddLanguageL.Size = new System.Drawing.Size(125, 13);
            this.AddLanguageL.TabIndex = 18;
            this.AddLanguageL.Text = "Ajouter une langue :";
            // 
            // AddLang
            // 
            this.AddLang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddLang.Image = global::MMORPG.Properties.Resources.upload;
            this.AddLang.Location = new System.Drawing.Point(162, 76);
            this.AddLang.Name = "AddLang";
            this.AddLang.Size = new System.Drawing.Size(36, 40);
            this.AddLang.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.AddLang.TabIndex = 19;
            this.AddLang.TabStop = false;
            this.AddLang.Click += new System.EventHandler(this.AddLang_Click);
            // 
            // RemoveLang
            // 
            this.RemoveLang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RemoveLang.Image = global::MMORPG.Properties.Resources.trash;
            this.RemoveLang.Location = new System.Drawing.Point(240, 49);
            this.RemoveLang.Name = "RemoveLang";
            this.RemoveLang.Size = new System.Drawing.Size(20, 25);
            this.RemoveLang.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.RemoveLang.TabIndex = 20;
            this.RemoveLang.TabStop = false;
            this.RemoveLang.Click += new System.EventHandler(this.RemoveLang_Click);
            // 
            // Interface
            // 
            this.Interface.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Interface.Controls.Add(this.panel2);
            this.Interface.Controls.Add(this.panel1);
            this.Interface.Controls.Add(this.chatzone);
            this.Interface.Location = new System.Drawing.Point(4, 22);
            this.Interface.Name = "Interface";
            this.Interface.Padding = new System.Windows.Forms.Padding(3);
            this.Interface.Size = new System.Drawing.Size(313, 131);
            this.Interface.TabIndex = 1;
            this.Interface.Text = "Chat";
            // 
            // chatzone
            // 
            this.chatzone.AutoSize = true;
            this.chatzone.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chatzone.Location = new System.Drawing.Point(6, 8);
            this.chatzone.Name = "chatzone";
            this.chatzone.Size = new System.Drawing.Size(84, 13);
            this.chatzone.TabIndex = 0;
            this.chatzone.Text = "Arrière plan :";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.Controls.Add(this.transparentCB);
            this.panel1.Controls.Add(this.backgroundPB);
            this.panel1.Location = new System.Drawing.Point(101, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(106, 25);
            this.panel1.TabIndex = 7;
            // 
            // backgroundPB
            // 
            this.backgroundPB.BackColor = System.Drawing.Color.White;
            this.backgroundPB.Location = new System.Drawing.Point(77, 4);
            this.backgroundPB.Name = "backgroundPB";
            this.backgroundPB.Size = new System.Drawing.Size(21, 16);
            this.backgroundPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backgroundPB.TabIndex = 6;
            this.backgroundPB.TabStop = false;
            // 
            // transparentCB
            // 
            this.transparentCB.AutoSize = true;
            this.transparentCB.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transparentCB.Location = new System.Drawing.Point(8, 3);
            this.transparentCB.Name = "transparentCB";
            this.transparentCB.Size = new System.Drawing.Size(63, 17);
            this.transparentCB.TabIndex = 5;
            this.transparentCB.Text = "Image";
            this.transparentCB.UseVisualStyleBackColor = true;
            this.transparentCB.Click += new System.EventHandler(this.transparentCB_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel2.Controls.Add(this.ColorLB);
            this.panel2.Controls.Add(this.colorSelected);
            this.panel2.Location = new System.Drawing.Point(213, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(87, 25);
            this.panel2.TabIndex = 8;
            // 
            // colorSelected
            // 
            this.colorSelected.BackColor = System.Drawing.Color.Black;
            this.colorSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorSelected.Location = new System.Drawing.Point(62, 5);
            this.colorSelected.Name = "colorSelected";
            this.colorSelected.Size = new System.Drawing.Size(18, 16);
            this.colorSelected.TabIndex = 3;
            this.colorSelected.MouseClick += new System.Windows.Forms.MouseEventHandler(this.colorSelected_MouseClick);
            // 
            // ColorLB
            // 
            this.ColorLB.AutoSize = true;
            this.ColorLB.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColorLB.Location = new System.Drawing.Point(5, 5);
            this.ColorLB.Name = "ColorLB";
            this.ColorLB.Size = new System.Drawing.Size(51, 13);
            this.ColorLB.TabIndex = 4;
            this.ColorLB.Text = "Couleur";
            // 
            // ConnexionTab
            // 
            this.ConnexionTab.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ConnexionTab.Controls.Add(this.ResetPB);
            this.ConnexionTab.Controls.Add(this.savePic);
            this.ConnexionTab.Controls.Add(this.portserver);
            this.ConnexionTab.Controls.Add(this.ipserver);
            this.ConnexionTab.Controls.Add(this.ServerPortL);
            this.ConnexionTab.Controls.Add(this.ServerIPL);
            this.ConnexionTab.Location = new System.Drawing.Point(4, 22);
            this.ConnexionTab.Name = "ConnexionTab";
            this.ConnexionTab.Padding = new System.Windows.Forms.Padding(3);
            this.ConnexionTab.Size = new System.Drawing.Size(313, 131);
            this.ConnexionTab.TabIndex = 0;
            this.ConnexionTab.Text = "Connnexion";
            // 
            // ServerIPL
            // 
            this.ServerIPL.AutoSize = true;
            this.ServerIPL.BackColor = System.Drawing.Color.Transparent;
            this.ServerIPL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerIPL.ForeColor = System.Drawing.Color.DimGray;
            this.ServerIPL.Location = new System.Drawing.Point(20, 15);
            this.ServerIPL.Name = "ServerIPL";
            this.ServerIPL.Size = new System.Drawing.Size(95, 13);
            this.ServerIPL.TabIndex = 0;
            this.ServerIPL.Text = "IP du Serveur :";
            // 
            // ServerPortL
            // 
            this.ServerPortL.AutoSize = true;
            this.ServerPortL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerPortL.ForeColor = System.Drawing.Color.DimGray;
            this.ServerPortL.Location = new System.Drawing.Point(20, 45);
            this.ServerPortL.Name = "ServerPortL";
            this.ServerPortL.Size = new System.Drawing.Size(106, 13);
            this.ServerPortL.TabIndex = 2;
            this.ServerPortL.Text = "Port du Serveur :";
            // 
            // ipserver
            // 
            this.ipserver.Location = new System.Drawing.Point(147, 12);
            this.ipserver.Name = "ipserver";
            this.ipserver.Size = new System.Drawing.Size(143, 21);
            this.ipserver.TabIndex = 1;
            // 
            // portserver
            // 
            this.portserver.Location = new System.Drawing.Point(147, 42);
            this.portserver.Name = "portserver";
            this.portserver.Size = new System.Drawing.Size(46, 21);
            this.portserver.TabIndex = 3;
            // 
            // savePic
            // 
            this.savePic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.savePic.Image = global::MMORPG.Properties.Resources.save;
            this.savePic.Location = new System.Drawing.Point(287, 105);
            this.savePic.Name = "savePic";
            this.savePic.Size = new System.Drawing.Size(20, 20);
            this.savePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.savePic.TabIndex = 11;
            this.savePic.TabStop = false;
            this.savePic.Click += new System.EventHandler(this.savePic_Click);
            // 
            // ResetPB
            // 
            this.ResetPB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ResetPB.Image = global::MMORPG.Properties.Resources.reset;
            this.ResetPB.Location = new System.Drawing.Point(261, 105);
            this.ResetPB.Name = "ResetPB";
            this.ResetPB.Size = new System.Drawing.Size(20, 20);
            this.ResetPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ResetPB.TabIndex = 12;
            this.ResetPB.TabStop = false;
            this.ResetPB.Click += new System.EventHandler(this.ResetPB_Click);
            // 
            // OptionFormTab
            // 
            this.OptionFormTab.Controls.Add(this.ConnexionTab);
            this.OptionFormTab.Controls.Add(this.Interface);
            this.OptionFormTab.Controls.Add(this.LangTab);
            this.OptionFormTab.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptionFormTab.Location = new System.Drawing.Point(12, 19);
            this.OptionFormTab.Name = "OptionFormTab";
            this.OptionFormTab.SelectedIndex = 0;
            this.OptionFormTab.Size = new System.Drawing.Size(321, 157);
            this.OptionFormTab.TabIndex = 0;
            this.OptionFormTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OptionFormTab_KeyUp);
            // 
            // OptionForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(342, 188);
            this.Controls.Add(this.ClosePB);
            this.Controls.Add(this.OptionFormTab);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OptionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Option";
            this.Load += new System.EventHandler(this.OptionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ClosePB)).EndInit();
            this.LangTab.ResumeLayout(false);
            this.LangTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OnOffPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddLang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoveLang)).EndInit();
            this.Interface.ResumeLayout(false);
            this.Interface.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundPB)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ConnexionTab.ResumeLayout(false);
            this.ConnexionTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.savePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResetPB)).EndInit();
            this.OptionFormTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ClosePB;
        private System.Windows.Forms.TabPage LangTab;
        private System.Windows.Forms.PictureBox RemoveLang;
        private System.Windows.Forms.PictureBox AddLang;
        private System.Windows.Forms.Label AddLanguageL;
        private System.Windows.Forms.ComboBox InstalledLang;
        private System.Windows.Forms.Label InstalledLangL;
        private System.Windows.Forms.Label DefaultLang;
        private System.Windows.Forms.Label DefaultLangL;
        private System.Windows.Forms.Label SpellCheckerL;
        private System.Windows.Forms.PictureBox OnOffPic;
        private System.Windows.Forms.TabPage Interface;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label ColorLB;
        private System.Windows.Forms.Panel colorSelected;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox transparentCB;
        private System.Windows.Forms.PictureBox backgroundPB;
        private System.Windows.Forms.Label chatzone;
        private System.Windows.Forms.TabPage ConnexionTab;
        private System.Windows.Forms.PictureBox ResetPB;
        private System.Windows.Forms.PictureBox savePic;
        private System.Windows.Forms.TextBox portserver;
        private System.Windows.Forms.TextBox ipserver;
        private System.Windows.Forms.Label ServerPortL;
        private System.Windows.Forms.Label ServerIPL;
        private System.Windows.Forms.TabControl OptionFormTab;
    }
}