using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG
{
    public partial class OptionForm : Form
    {
        public OptionForm()
        {
            InitializeComponent();
        }
        private void OptionForm_Load(object sender, EventArgs e)
        {
            LangTab.Text = CommonCode.TranslateText(135);
            ipserver.Text = Network.host;
            portserver.Text = Network.port.ToString();
            SpellCheckerL.Text = CommonCode.TranslateText(134) + " : ";
            InstalledLangL.Text = CommonCode.TranslateText(138) + " : ";
            AddLanguageL.Text = CommonCode.TranslateText(137) + " : ";
            ServerIPL.Text = CommonCode.TranslateText(143) + " : ";
            ServerPortL.Text = CommonCode.TranslateText(144) + " : ";
            ConnexionTab.Text = CommonCode.TranslateText(145) + " : ";
            Interface.Text = CommonCode.TranslateText(146) + " : ";

            if (CommonCode.SpellCheck)
                OnOffPic.Image = Properties.Resources.on;
            else
                OnOffPic.Image = Properties.Resources.off;
            DefaultLangL.Text = CommonCode.TranslateText(136) + " : ";

            // on vérifie si la langue séléctionné par défaut se trouve réelement installé
            if (CommonCode.installedLang.FindAll(f => f == CommonCode.DefaultLang).Count() == 0)
            {
                CommonCode.DefaultLang = CommonCode.installedLang[0];
                DefaultLang.Text = CommonCode.DefaultLang;
                CommonCode.saveOptions();
            }
            else
                DefaultLang.Text = CommonCode.DefaultLang; 

            if (CommonCode.installedLang.Count() > 0)
            {
                // des langues sonts installés
                // on vérifie si la langue choisie par défaut se trouve parmis ceux installé
                foreach (string s in CommonCode.installedLang)
                        InstalledLang.Items.Add(s);
                InstalledLang.SelectedItem = CommonCode.DefaultLang;
            }
            ReloadString();
        }
        public void ReloadString()
        {
            chatzone.Text = CommonCode.TranslateText(127) + " : ";
            transparentCB.Text = CommonCode.TranslateText(128);
            ColorLB.Text = CommonCode.TranslateText(129);

            if (MainForm.transparentChatBox && MainForm.PictureBgChatBox != "null")
                transparentCB.Checked = true;
            else
                transparentCB.Checked = false;

            // affichage de l'image d'arrier plant de chat box
            if (MainForm.PictureBgChatBox != "null")
            {
                if (File.Exists(MainForm.PictureBgChatBox))
                    backgroundPB.Image = Image.FromFile(MainForm.PictureBgChatBox);
                else
                {
                    MainForm.PictureBgChatBox = "null";
                    CommonCode.saveOptions();
                }
            }

            // verifier les couleurs presentes dans le fichier config.ini
            if(MainForm.ColorBgChatBox.Split(',').Count() < 3)
            {
                // il ya une erreur sur la variableColorBgChatBox dans le fichier config.ini
                MessageBox.Show("File <Config.ini> corrupted, Param <ColorBgChatBox> is not a known color.\nClic OK to carry out, we'll handle this for you :)\nNext time the default Chatbox back color will be White, then you can change it.");
                saveOptionsWhenColorBgChatBoxIsCorrupt();
            }
            else
            {
                bool error = false;
                int part1;
                int part2;
                int part3;

                bool result = int.TryParse(MainForm.ColorBgChatBox.Split(',')[0], out part1);
                if (!result)
                {
                    error = true;
                    MessageBox.Show("File <Config.ini> corrupted, Param <ColorBgChatBox> is not a known color.\nClic OK to carry out, we'll handle this for you :)\nNext time the default Chatbox back color will be White, then you can change it.\nError 0x1");
                    saveOptionsWhenColorBgChatBoxIsCorrupt();
                }

                result = int.TryParse(MainForm.ColorBgChatBox.Split(',')[1], out part2);
                if (!result)
                {
                    error = true;
                    MessageBox.Show("File <Config.ini> corrupted, Param <ColorBgChatBox> is not a known color.\nClic OK to carry out, we'll handle this for you :)\nNext time the default Chatbox back color will be White, then you can change it.\nError 0x2");
                    saveOptionsWhenColorBgChatBoxIsCorrupt();
                }

                result = int.TryParse(MainForm.ColorBgChatBox.Split(',')[2], out part3);
                if (!result)
                {
                    error = true;
                    MessageBox.Show("File <Config.ini> corrupted, Param <ColorBgChatBox> is not a known color.\nClic OK to carry out, we'll handle this for you :)\nNext time the default Chatbox back color will be White, then you can change it.\nError 0x3");
                    saveOptionsWhenColorBgChatBoxIsCorrupt();
                }

                if(!error)
                {
                    if (part1 < 0 || part1 > 255)
                    {
                        error = true;
                        MessageBox.Show("File <Config.ini> corrupted, Param <ColorBgChatBox> is not a known color.\nClic OK to carry out, we'll handle this for you :)\nNext time the default Chatbox back color will be White, then you can change it.\nError 0x4");
                        saveOptionsWhenColorBgChatBoxIsCorrupt();
                    }
                    else if (part2 < 0 || part2 > 255)
                    {
                        error = true;
                        MessageBox.Show("File <Config.ini> corrupted, Param <ColorBgChatBox> is not a known color.\nClic OK to carry out, we'll handle this for you :)\nNext time the default Chatbox back color will be White, then you can change it.\nError 0x5");
                        saveOptionsWhenColorBgChatBoxIsCorrupt();
                    }
                    else if (part3 < 0 || part3 > 255)
                    {
                        error = true;
                        MessageBox.Show("File <Config.ini> corrupted, Param <ColorBgChatBox> is not a known color.\nClic OK to carry out, we'll handle this for you :)\nNext time the default Chatbox back color will be White, then you can change it.\nError 0x6");
                        saveOptionsWhenColorBgChatBoxIsCorrupt();
                    }

                    if (!error)
                        colorSelected.BackColor = System.Drawing.Color.FromArgb(part1, part2, part3);
                    else
                    {
                        // maintenant on color le calc
                        part1 = int.Parse(MainForm.ColorBgChatBox.Split(':')[1].Split(',')[0]);
                        part2 = int.Parse(MainForm.ColorBgChatBox.Split(':')[1].Split(',')[1]);
                        part3 = int.Parse(MainForm.ColorBgChatBox.Split(':')[1].Split(',')[2]);

                        colorSelected.BackColor = System.Drawing.Color.FromArgb(part1, part2, part3);
                    }
                }
            }
        }
        private void saveOptionsWhenColorBgChatBoxIsCorrupt()
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
            MainForm.ColorBgChatBox = "255,255,255";

            for (int cnt = 0; cnt < configFile.Count; cnt++)
            {
                if (configFile[cnt] != string.Empty && configFile[cnt].Substring(0, 2) != "//" && configFile[cnt].IndexOf(':') != -1)
                {
                    string[] dataLine = configFile[cnt].Split(':');
                    dataLine[0] = dataLine[0].Replace(" ", "");
                    dataLine[1] = dataLine[1].Replace(" ", "");

                    if (dataLine[0] == "ColorBgChatBox")
                        data[cnt] = "ColorBgChatBox:" + MainForm.ColorBgChatBox;
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
        private void colorSelected_MouseClick(object sender, MouseEventArgs e)
        {
            // evenement pour afficher la palette de couleur afin de choisir une couleur
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            colorSelected.BackColor = cd.Color;
            MainForm.ColorBgChatBox = cd.Color.R + "," + cd.Color.G + "," + cd.Color.B;
            CommonCode.saveOptions();
            ChatArea.BackColor = cd.Color;
        }
        private void transparentCB_Click(object sender, EventArgs e)
        {
            // changement du propriété transparent
            if (transparentCB.Checked)
            {
                // traitement pour parcourir un fichier
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.bmp, *.jpg, *.gif, *.png) | *.bmp; *.jpg; *.gif; *.png";
                openFileDialog.FilterIndex = 1;
                openFileDialog.InitialDirectory = @"gfx\general\UI\Stored\";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.CheckFileExists)
                    {
                        // traitement quand on a choisie un fichier
                        Bitmap bmp = new Bitmap(openFileDialog.FileName);
                        if (bmp.Width > 800 || bmp.Height > 600)
                        {
                            MessageBox.Show(CommonCode.TranslateText(132));
                            transparentCB.Checked = false;
                        }
                        else
                        {
                            // image autorisé
                            // on copie l'image dans le dossier gfx\general\UI
                            string Ext = openFileDialog.FileName.Substring(openFileDialog.FileName.IndexOf('.') + 1, openFileDialog.FileName.Length - openFileDialog.FileName.IndexOf('.') - 1);

                            // on vérifie si l'image n'est pas préalablement dans le dossier aproprié
                            if (!System.IO.File.Exists(@"gfx\general\UI\chatboxBG." + Ext))
                            {
                                System.IO.File.Delete(@"gfx\general\UI\chatboxBG." + Ext);
                                System.IO.File.Copy(openFileDialog.FileName, @"gfx\general\UI\chatboxBG." + Ext);
                            }
                            MainForm.PictureBgChatBox = @"gfx\general\UI\chatboxBG." + Ext;
                            backgroundPB.Image = Image.FromFile(MainForm.PictureBgChatBox);
                            MainForm.transparentChatBox = true;
                            CommonCode.saveOptions();
                            MessageBox.Show(CommonCode.TranslateText(133), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show(CommonCode.TranslateText(130));
                        transparentCB.Checked = false;
                    }
                }
                else
                {
                    MessageBox.Show(CommonCode.TranslateText(131));
                    transparentCB.Checked = false;
                }
            }
            else
            {
                MainForm.transparentChatBox = false;
                CommonCode.saveOptions();
            }
        }
        private void AddLang_Click(object sender, EventArgs e)
        {
            
            bool added = MainForm.nHunspellTextBoxExtender1.AddNewLanguage();
            if (added)
            {
                // langue ajouté
                CommonCode.installedLang = MainForm.nHunspellTextBoxExtender1.GetAvailableLanguages().ToList();
                InstalledLang.Items.Clear();
                foreach (string s in CommonCode.installedLang)
                    InstalledLang.Items.Add(s);
                InstalledLang.SelectedItem = CommonCode.DefaultLang;
            }
        }
        private void RemoveLang_Click(object sender, EventArgs e)
        {
            if (InstalledLang.SelectedItem.ToString() != "")
            {
                if (CommonCode.installedLang.Count == 0)
                    MessageBox.Show(CommonCode.TranslateText(142), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                {
                    DialogResult confirm = MessageBox.Show(CommonCode.TranslateText(139) + " [" + InstalledLang.SelectedItem.ToString() + "]", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == System.Windows.Forms.DialogResult.Yes)
                    {
                        MainForm.nHunspellTextBoxExtender1.RemoveLanguage(InstalledLang.SelectedItem.ToString());
                        MessageBox.Show(CommonCode.TranslateText(140), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (InstalledLang.SelectedItem.ToString() != "English")
                        {
                            string deletedLang = InstalledLang.SelectedItem.ToString();
                            CommonCode.installedLang.Remove(InstalledLang.SelectedItem.ToString());
                            InstalledLang.Items.Remove(InstalledLang.SelectedItem);
                            if (CommonCode.DefaultLang == deletedLang)
                            {
                                CommonCode.DefaultLang = InstalledLang.Items[0].ToString();
                                InstalledLang.Text = CommonCode.DefaultLang;
                                InstalledLang.SelectedItem = CommonCode.DefaultLang;
                                CommonCode.saveOptions();
                            }
                        }
                        else
                        {
                            // la langue englais ne se supprime pas
                        }
                    }
                    else
                    {
                        MessageBox.Show(CommonCode.TranslateText(141), "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
        private void OnOffPic_Click(object sender, EventArgs e)
        {
            if (CommonCode.SpellCheck)
            {
                OnOffPic.Image = Properties.Resources.off;
                CommonCode.SpellCheck = false;
                MainForm.nHunspellTextBoxExtender1.SetSpellCheckEnabled(HudHandle.ChatTextBox, false);
                CommonCode.saveOptions();
            }
            else
            {
                OnOffPic.Image = Properties.Resources.on;
                CommonCode.SpellCheck = true;
                MainForm.nHunspellTextBoxExtender1.SetSpellCheckEnabled(HudHandle.ChatTextBox, true);
                CommonCode.saveOptions();
            }

            // il faut desactiver le chatbox, des fois bizarement il s'affiche alors qu'on ai dans la form d'ientification
            if(CommonCode.CurMap == "LoginMap")
                HudHandle.ChatTextBox.Visible = false;
        }
        private void InstalledLang_SelectedValueChanged(object sender, EventArgs e)
        {
            CommonCode.DefaultLang = InstalledLang.SelectedItem.ToString();
            DefaultLang.Text = CommonCode.DefaultLang;
            MainForm.nHunspellTextBoxExtender1.SetLanguage(CommonCode.DefaultLang);
            CommonCode.saveOptions();
        }
        private void savePic_Click(object sender, EventArgs e)
        {
            Network.host = ipserver.Text;
            Network.port = Convert.ToInt32(portserver.Text);
            CommonCode.saveOptions();
            MessageBox.Show(CommonCode.TranslateText(36), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ResetPB_Click(object sender, EventArgs e)
        {
            ipserver.Text = "the-morpher.ddns.net";
            portserver.Text = "7070";
        }

        private void ClosePB_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manager.manager.mainForm.Enabled = true;
            Manager.manager.mainForm.BringToFront();
        }

        private void OptionFormTab_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Hide();
                Manager.manager.mainForm.Enabled = true;
                Manager.manager.mainForm.BringToFront();
            }
        }
    }
}