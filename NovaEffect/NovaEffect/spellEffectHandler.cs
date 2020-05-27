using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace NovaEffect
{
    public partial class spellEffectHandler : Form
    {
        public spellEffectHandler()
        {
            InitializeComponent();
        }

        List<flagDetailsStructure> flagDetails = new List<flagDetailsStructure>();
        List<flagDetailsStructure> fds = new List<flagDetailsStructure>();
        public spellEffectHandler(MySqlConn mySqlConn)
        {
            InitializeComponent();
        }

        private void filter_Click(object sender, EventArgs e)
        {
            int _spellID, _level;
            if(!int.TryParse(spellID.Text, out _spellID))
            {
                MessageBox.Show("le spellID n'est pas un nombre entier numérique");
                return;
            }

            if (!int.TryParse(level.Text, out _level))
            {
                MessageBox.Show("le level n'est pas un nombre entier numérique");
                return;
            }

            if (_spellID < 0)
            {
                MessageBox.Show("le spellID dois être supérieur ou égale à 0");
                return;
            }

            if (_level < 1)
            {
                MessageBox.Show("le level dois être supérieur ou égale à 1");
                return;
            }
            effectTab.TabPages.Clear();
            TabPage tempTP = new TabPage();
            effectTab.TabPages.Add(tempTP);
            effectTab.TabPages[0].Text = "Effect 1"; 
            
            mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID.ToString() == spellID.Text && f.level.ToString() == level.Text);
            if (spell == null)
                MessageBox.Show("Sort introuvable");
            else if (spell.extraDataEffect == "" || spell.extraDataEffect == null)
                MessageBox.Show("Aucun effets est affecté a ce sort");
            else
            {
                string[] effects = spell.extraDataEffect.Split('|');

                for (int cnt = 0; cnt < effects.Length; cnt++)
                {
                    string[] data = effects[cnt].Split('#');

                    string effectBase = data[0];
                    string effectID = data[1];
                    string[] targets = data[2].Split('/');
                    string element = data[3];
                    string duration = data[4];
                    string delay = data[5];
                    string zoneSize = data[6];
                    string zoneExtensible = data[7];
                    string handToHand = data[8];
                    string min = data[9];
                    string max = data[10];
                    string flag1 = data[11];
                    string flag2 = data[12];
                    string flag3 = data[13];

                    if (cnt > 0)
                    {
                        TabPage tp = new TabPage();
                        effectTab.TabPages.Add(tp);

                    }
                    ///////////////////////////////////////////////////////////
                    ComboBox _targetSelection = new ComboBox();
                    Label label5 = new Label();
                    TextBox _effectID = new TextBox();
                    Label label4 = new Label();
                    ComboBox _effectBase = new ComboBox();
                    Label label3 = new Label();
                    ComboBox _targets = new ComboBox();
                    Button Ajouter = new Button();
                    Label label6 = new Label();
                    ComboBox _element = new ComboBox();
                    Label label7 = new Label();
                    TextBox _duration = new TextBox();
                    TextBox _delay = new TextBox();
                    Label label8 = new Label();
                    TextBox _zoneSize = new TextBox();
                    Label label9 = new Label();
                    Label label10 = new Label();
                    ComboBox _zoneExtensible = new ComboBox();
                    TextBox _handToHand = new TextBox();
                    Label label11 = new Label();
                    TextBox _max = new TextBox();
                    Label label12 = new Label();
                    TextBox _min = new TextBox();
                    Label label13 = new Label();
                    TextBox _flag1 = new TextBox();
                    Label label15 = new Label();
                    TextBox _flag2 = new TextBox();
                    Label label14 = new Label();
                    TextBox _flag3 = new TextBox();
                    Label label16 = new Label();
                    Button button1 = new Button();
                    Button button2 = new Button();
                    PictureBox flag1Indicator = new PictureBox();
                    PictureBox flag2Indicator = new PictureBox();
                    PictureBox flag3Indicator = new PictureBox();

                    effectTab.TabPages[cnt].Controls.Add(button2);
                    effectTab.TabPages[cnt].Controls.Add(_flag3);
                    effectTab.TabPages[cnt].Controls.Add(label16);
                    effectTab.TabPages[cnt].Controls.Add(_flag2);
                    effectTab.TabPages[cnt].Controls.Add(label14);
                    effectTab.TabPages[cnt].Controls.Add(_flag1);
                    effectTab.TabPages[cnt].Controls.Add(label15);
                    effectTab.TabPages[cnt].Controls.Add(_max);
                    effectTab.TabPages[cnt].Controls.Add(label12);
                    effectTab.TabPages[cnt].Controls.Add(_min);
                    effectTab.TabPages[cnt].Controls.Add(label13);
                    effectTab.TabPages[cnt].Controls.Add(_handToHand);
                    effectTab.TabPages[cnt].Controls.Add(label11);
                    effectTab.TabPages[cnt].Controls.Add(_zoneExtensible);
                    effectTab.TabPages[cnt].Controls.Add(label10);
                    effectTab.TabPages[cnt].Controls.Add(_zoneSize);
                    effectTab.TabPages[cnt].Controls.Add(label9);
                    effectTab.TabPages[cnt].Controls.Add(_delay);
                    effectTab.TabPages[cnt].Controls.Add(label8);
                    effectTab.TabPages[cnt].Controls.Add(_duration);
                    effectTab.TabPages[cnt].Controls.Add(label7);
                    effectTab.TabPages[cnt].Controls.Add(_element);
                    effectTab.TabPages[cnt].Controls.Add(label6);
                    effectTab.TabPages[cnt].Controls.Add(Ajouter);
                    effectTab.TabPages[cnt].Controls.Add(_targets);
                    effectTab.TabPages[cnt].Controls.Add(_targetSelection);
                    effectTab.TabPages[cnt].Controls.Add(label5);
                    effectTab.TabPages[cnt].Controls.Add(_effectID);
                    effectTab.TabPages[cnt].Controls.Add(label4);
                    effectTab.TabPages[cnt].Controls.Add(_effectBase);
                    effectTab.TabPages[cnt].Controls.Add(label3);
                    effectTab.TabPages[cnt].Controls.Add(flag1Indicator);
                    effectTab.TabPages[cnt].Controls.Add(flag2Indicator);
                    effectTab.TabPages[cnt].Controls.Add(flag3Indicator);
                    // 
                    // targetSelection
                    // 
                    _targetSelection.DropDownStyle = ComboBoxStyle.DropDownList;
                    _targetSelection.FormattingEnabled = true;
                    _targetSelection.Items.AddRange(new object[] {
                    "self",
                    "enemy_1",
                    "ally_1",
                    "none",
                    "ally_summon",
                    "enemy_summon"});
                    _targetSelection.Location = new Point(233, 67);
                    _targetSelection.Name = "targetSelection";
                    _targetSelection.Size = new Size(121, 21);
                    _targetSelection.TabIndex = 5;
                    // 
                    // label5
                    // 
                    label5.AutoSize = true;
                    label5.Location = new Point(17, 70);
                    label5.Name = "label5";
                    label5.Size = new Size(38, 13);
                    label5.TabIndex = 4;
                    label5.Text = "Target";
                    // 
                    // effectID
                    // 
                    _effectID.Location = new Point(106, 42);
                    _effectID.Name = "effectID";
                    _effectID.Size = new Size(52, 20);
                    _effectID.TabIndex = 3;
                    // 
                    // label4
                    // 
                    label4.AutoSize = true;
                    label4.Location = new Point(17, 45);
                    label4.Name = "label4";
                    label4.Size = new Size(46, 13);
                    label4.TabIndex = 2;
                    label4.Text = "EffectID";
                    // 
                    // effectBase
                    // 
                    _effectBase.DropDownStyle = ComboBoxStyle.DropDownList;
                    _effectBase.FormattingEnabled = true;
                    _effectBase.Items.AddRange(new object[] {
                    "dammage",
                    "defaultDamage",
                    "heal",
                    "summon",
                    "drain_life",
                    "drain_pc",
                    "drain_pm",
                    "drain_power",
                    "mode",
                    "boost",
                    "killSummonToBoost",
                    "state"
                    });
                    _effectBase.Location = new Point(106, 16);
                    _effectBase.Name = "effectBase";
                    _effectBase.Size = new Size(160, 21);
                    _effectBase.TabIndex = 1;
                    // 
                    // label3
                    // 
                    label3.AutoSize = true;
                    label3.Location = new Point(17, 20);
                    label3.Name = "label3";
                    label3.Size = new Size(59, 13);
                    label3.TabIndex = 0;
                    label3.Text = "EffectBase";
                    // 
                    // targets
                    // 
                    _targets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                    _targets.FormattingEnabled = true;
                    _targets.Location = new System.Drawing.Point(106, 67);
                    _targets.Name = "targets";
                    _targets.Size = new System.Drawing.Size(121, 21);
                    _targets.TabIndex = 6;
                    // 
                    // Ajouter
                    // 
                    Ajouter.Location = new System.Drawing.Point(360, 66);
                    Ajouter.Name = "Ajouter";
                    Ajouter.Size = new System.Drawing.Size(75, 23);
                    Ajouter.TabIndex = 7;
                    Ajouter.Text = "add";
                    Ajouter.UseVisualStyleBackColor = true;
                    Ajouter.Click += new System.EventHandler(Ajouter_Click);
                    // 
                    // label6
                    // 
                    label6.AutoSize = true;
                    label6.Location = new System.Drawing.Point(17, 96);
                    label6.Name = "label6";
                    label6.Size = new System.Drawing.Size(45, 13);
                    label6.TabIndex = 8;
                    label6.Text = "Element";
                    // 
                    // element
                    // 
                    _element.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                    _element.FormattingEnabled = true;
                    _element.Items.AddRange(new object[] {
                    "doton",
                    "katon",
                    "futon",
                    "suiton",
                    "raiton",
                    "neutre"});
                    _element.Location = new System.Drawing.Point(106, 93);
                    _element.Name = "element";
                    _element.Size = new System.Drawing.Size(121, 21);
                    _element.TabIndex = 9;
                    // 
                    // label7
                    // 
                    label7.AutoSize = true;
                    label7.Location = new System.Drawing.Point(17, 123);
                    label7.Name = "label7";
                    label7.Size = new System.Drawing.Size(47, 13);
                    label7.TabIndex = 10;
                    label7.Text = "Duration";
                    // 
                    // duration
                    // 
                    _duration.Location = new System.Drawing.Point(106, 119);
                    _duration.Name = "duration";
                    _duration.Size = new System.Drawing.Size(52, 20);
                    _duration.TabIndex = 11;
                    // 
                    // delay
                    // 
                    _delay.Location = new System.Drawing.Point(106, 144);
                    _delay.Name = "delay";
                    _delay.Size = new System.Drawing.Size(52, 20);
                    _delay.TabIndex = 13;
                    // 
                    // label8
                    // 
                    label8.AutoSize = true;
                    label8.Location = new System.Drawing.Point(17, 148);
                    label8.Name = "label8";
                    label8.Size = new System.Drawing.Size(34, 13);
                    label8.TabIndex = 12;
                    label8.Text = "Delay";
                    // 
                    // zoneSize
                    // 
                    _zoneSize.Location = new System.Drawing.Point(106, 169);
                    _zoneSize.Name = "zoneSize";
                    _zoneSize.Size = new System.Drawing.Size(52, 20);
                    _zoneSize.TabIndex = 15;
                    // 
                    // label9
                    // 
                    label9.AutoSize = true;
                    label9.Location = new System.Drawing.Point(17, 173);
                    label9.Name = "label9";
                    label9.Size = new System.Drawing.Size(55, 13);
                    label9.TabIndex = 14;
                    label9.Text = "Zone Size";
                    // 
                    // label10
                    // 
                    label10.AutoSize = true;
                    label10.Location = new System.Drawing.Point(17, 200);
                    label10.Name = "label10";
                    label10.Size = new System.Drawing.Size(83, 13);
                    label10.TabIndex = 16;
                    label10.Text = "Zone Extensible";
                    // 
                    // zoneExtensible
                    // 
                    _zoneExtensible.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                    _zoneExtensible.FormattingEnabled = true;
                    _zoneExtensible.Items.AddRange(new object[] {
                    "true",
                    "false"});
                    _zoneExtensible.Location = new System.Drawing.Point(106, 195);
                    _zoneExtensible.Name = "zoneExtensible";
                    _zoneExtensible.Size = new System.Drawing.Size(66, 21);
                    _zoneExtensible.TabIndex = 17;
                    // 
                    // handToHand
                    // 
                    _handToHand.Location = new System.Drawing.Point(105, 223);
                    _handToHand.Name = "handToHand";
                    _handToHand.Size = new System.Drawing.Size(52, 20);
                    _handToHand.TabIndex = 19;
                    // 
                    // label11
                    // 
                    label11.AutoSize = true;
                    label11.Location = new System.Drawing.Point(17, 227);
                    label11.Name = "label11";
                    label11.Size = new System.Drawing.Size(72, 13);
                    label11.TabIndex = 18;
                    label11.Text = "HandToHand Distance";
                    // 
                    // Max
                    // 
                    _max.Location = new System.Drawing.Point(432, 147);
                    _max.Name = "Max";
                    _max.Size = new System.Drawing.Size(52, 20);
                    _max.TabIndex = 23;
                    // 
                    // label12
                    // 
                    label12.AutoSize = true;
                    label12.Location = new System.Drawing.Point(343, 151);
                    label12.Name = "label12";
                    label12.Size = new System.Drawing.Size(27, 13);
                    label12.TabIndex = 22;
                    label12.Text = "Max";
                    // 
                    // Min
                    // 
                    _min.Location = new System.Drawing.Point(432, 122);
                    _min.Name = "Min";
                    _min.Size = new System.Drawing.Size(52, 20);
                    _min.TabIndex = 21;
                    // 
                    // label13
                    // 
                    label13.AutoSize = true;
                    label13.Location = new System.Drawing.Point(343, 126);
                    label13.Name = "label13";
                    label13.Size = new System.Drawing.Size(24, 13);
                    label13.TabIndex = 20;
                    label13.Text = "Min";
                    // 
                    // flag1
                    // 
                    _flag1.Location = new System.Drawing.Point(432, 172);
                    _flag1.Name = "flag1";
                    _flag1.Size = new System.Drawing.Size(50, 20);
                    _flag1.TabIndex = 25;
                    //
                    // image indicatif
                    //
                    flag1Indicator.Image = Image.FromFile("information.png");
                    flag1Indicator.SizeMode = PictureBoxSizeMode.AutoSize;
                    flag1Indicator.Location = new Point(_flag1.Location.X + _flag1.Width + 10, _flag1.Location.Y);
                    flag1Indicator.Cursor = Cursors.Hand;
                    flag1Indicator.Tag = 1;
                    flag1Indicator.Name = "flag1Indicator";
                    flag1Indicator.Click += FlagIndicator_Click;
                    // 
                    // label15
                    // 
                    label15.AutoSize = true;
                    label15.Location = new System.Drawing.Point(344, 176);
                    label15.Name = "label15";
                    label15.Size = new System.Drawing.Size(36, 13);
                    label15.TabIndex = 24;
                    label15.Text = "Flag 1";
                    // 
                    // flag2
                    // 
                    _flag2.Location = new System.Drawing.Point(432, 198);
                    _flag2.Name = "flag2";
                    _flag2.Size = new System.Drawing.Size(50, 20);
                    _flag2.TabIndex = 27;
                    // 
                    // flag2Indicator
                    // 
                    flag2Indicator.Image = Image.FromFile("information.png");
                    flag2Indicator.SizeMode = PictureBoxSizeMode.AutoSize;
                    flag2Indicator.Location = new Point(_flag2.Location.X + _flag2.Width + 10, _flag2.Location.Y);
                    flag2Indicator.Cursor = Cursors.Hand;
                    flag2Indicator.Tag = 2;
                    flag2Indicator.Name = "flag2Indicator";
                    flag2Indicator.Click += FlagIndicator_Click;
                    //
                    // label14
                    //
                    label14.AutoSize = true;
                    label14.Location = new System.Drawing.Point(344, 202);
                    label14.Name = "label14";
                    label14.Size = new System.Drawing.Size(36, 13);
                    label14.TabIndex = 26;
                    label14.Text = "Flag 2";
                    // 
                    // flag3
                    // 
                    _flag3.Location = new System.Drawing.Point(432, 223);
                    _flag3.Name = "flag3";
                    _flag3.Size = new System.Drawing.Size(50, 20);
                    _flag3.TabIndex = 29;
                    // 
                    // flag3Indicator
                    // 
                    flag3Indicator.Image = Image.FromFile("information.png");
                    flag3Indicator.SizeMode = PictureBoxSizeMode.AutoSize;
                    flag3Indicator.Location = new Point(_flag3.Location.X + _flag3.Width + 10, _flag3.Location.Y);
                    flag3Indicator.Cursor = Cursors.Hand;
                    flag3Indicator.Tag = 3;
                    flag3Indicator.Name = "flag3Indicator";
                    flag3Indicator.Click += FlagIndicator_Click;
                    // 
                    // label16
                    // 
                    label16.AutoSize = true;
                    label16.Location = new System.Drawing.Point(344, 227);
                    label16.Name = "label16";
                    label16.Size = new System.Drawing.Size(36, 13);
                    label16.TabIndex = 28;
                    label16.Text = "Flag 3";
                    // 
                    // button1
                    // 
                    button1.Location = new System.Drawing.Point(132, 342);
                    button1.Name = "button1";
                    button1.Size = new System.Drawing.Size(286, 36);
                    button1.TabIndex = 30;
                    button1.Text = "Save";
                    button1.UseVisualStyleBackColor = true;
                    // 
                    // button2
                    // 
                    button2.Location = new System.Drawing.Point(438, 66);
                    button2.Name = "button2";
                    button2.Size = new System.Drawing.Size(75, 23);
                    button2.TabIndex = 30;
                    button2.Text = "remove";
                    button2.UseVisualStyleBackColor = true;
                    button2.Click += new System.EventHandler(button2_Click);

                    ////////////////////////////////////////////////////////////
                    // on utilise la tabPage1 déjà existante
                    (effectTab.TabPages[cnt].Controls.Find("effectBase", false)[0] as ComboBox).SelectedItem = effectBase;
                    fds.Add(flagDetails.Find(f => f.baseEffect == effectBase));
                    (effectTab.TabPages[cnt].Controls.Find("effectID", false)[0] as TextBox).Text = effectID;
                    foreach (string t in targets)
                        (effectTab.TabPages[cnt].Controls.Find("targets", false)[0] as ComboBox).Items.Add(t);

                    foreach (string t in (effectTab.TabPages[cnt].Controls.Find("targets", false)[0] as ComboBox).Items)
                        (effectTab.TabPages[cnt].Controls.Find("targetSelection", false)[0] as ComboBox).Items.Remove(t);

                    (effectTab.TabPages[cnt].Controls.Find("element", false)[0] as ComboBox).SelectedItem = element;
                    (effectTab.TabPages[cnt].Controls.Find("duration", false)[0] as TextBox).Text = duration;
                    (effectTab.TabPages[cnt].Controls.Find("delay", false)[0] as TextBox).Text = delay;
                    (effectTab.TabPages[cnt].Controls.Find("zoneSize", false)[0] as TextBox).Text = zoneSize;
                    (effectTab.TabPages[cnt].Controls.Find("zoneExtensible", false)[0] as ComboBox).SelectedItem = zoneExtensible;
                    (effectTab.TabPages[cnt].Controls.Find("handToHand", false)[0] as TextBox).Text = handToHand;
                    (effectTab.TabPages[cnt].Controls.Find("min", false)[0] as TextBox).Text = min;
                    (effectTab.TabPages[cnt].Controls.Find("max", false)[0] as TextBox).Text = max;
                    (effectTab.TabPages[cnt].Controls.Find("flag1", false)[0] as TextBox).Text = flag1;
                    (effectTab.TabPages[cnt].Controls.Find("flag2", false)[0] as TextBox).Text = flag2;
                    (effectTab.TabPages[cnt].Controls.Find("flag3", false)[0] as TextBox).Text = flag3;
                    effectTab.TabPages[cnt].Text = "Effect " + (cnt + 1);
                    
                    if (fds[cnt] == null || (fds[cnt] != null && fds[cnt].flag1 == ""))
                        flag1Indicator.Visible = false;

                    if (fds[cnt] == null || (fds[cnt] != null && fds[cnt].flag2 == ""))
                        flag2Indicator.Visible = false;

                    if (fds[cnt] == null || (fds[cnt] != null && fds[cnt].flag3 == ""))
                        flag3Indicator.Visible = false;
                }
                effectTab.Enabled = true;
            }
        }

        private void FlagIndicator_Click(object sender, EventArgs e)
        {
            TabPage tp = effectTab.SelectedTab;
            if (fds[tp.TabIndex] != null)
            {
                int tag = Convert.ToInt32((sender as PictureBox).Tag);
                if (tag == 1)
                    MessageBox.Show(fds[tp.TabIndex].flag1.Replace(".", Environment.NewLine));
                else if (tag == 2)
                    MessageBox.Show(fds[tp.TabIndex].flag2.Replace(".", Environment.NewLine));
                else if (tag == 3)
                    MessageBox.Show(fds[tp.TabIndex].flag3.Replace(".", Environment.NewLine));
            }
        }

        private void Ajouter_Click(object sender, EventArgs e)
        {
            TabPage tp = effectTab.SelectedTab;
            if ((tp.Controls.Find("targetSelection", false)[0] as ComboBox).SelectedItem != null)
            {
                (tp.Controls.Find("targets", false)[0] as ComboBox).Items.Add((tp.Controls.Find("targetSelection", false)[0] as ComboBox).SelectedItem);
                (tp.Controls.Find("targetSelection", false)[0] as ComboBox).Items.Remove((tp.Controls.Find("targetSelection", false)[0] as ComboBox).SelectedItem);
            }
            else
                MessageBox.Show("Veuillez selectionner un target");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TabPage tp = effectTab.SelectedTab;
            if ((tp.Controls.Find("targets", false)[0] as ComboBox).SelectedItem != null)
            {
                (tp.Controls.Find("targetSelection", false)[0] as ComboBox).Items.Add((tp.Controls.Find("targets", false)[0] as ComboBox).SelectedItem);
                (tp.Controls.Find("targets", false)[0] as ComboBox).Items.Remove((tp.Controls.Find("targets", false)[0] as ComboBox).SelectedItem);
            }
            else
                MessageBox.Show("Veuillez selectionner un target");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = "";

            foreach(TabPage tp in effectTab.TabPages)
            {
                string effectBase = (tp.Controls.Find("effectBase", false)[0] as ComboBox).SelectedItem.ToString();
                string effectID = (tp.Controls.Find("effectID", false)[0] as TextBox).Text;
                string targets = "";
                foreach (string s in (tp.Controls.Find("targets", false)[0] as ComboBox).Items)
                    targets += s + "/";

                if (targets != "")
                    targets = targets.Substring(0, targets.Length - 1);

                string element = (tp.Controls.Find("element", false)[0] as ComboBox).SelectedItem.ToString();
                string duration = (tp.Controls.Find("duration", false)[0] as TextBox).Text;
                string delay = (tp.Controls.Find("delay", false)[0] as TextBox).Text;
                string zoneSize = (tp.Controls.Find("zonesize", false)[0] as TextBox).Text;
                string zoneExtensible = (tp.Controls.Find("zoneExtensible", false)[0] as ComboBox).SelectedItem.ToString();
                string handToHand = (tp.Controls.Find("handToHand", false)[0] as TextBox).Text;
                string min = (tp.Controls.Find("min", false)[0] as TextBox).Text;
                string max = (tp.Controls.Find("max", false)[0] as TextBox).Text;
                string flag1 = (tp.Controls.Find("flag1", false)[0] as TextBox).Text;
                string flag2 = (tp.Controls.Find("flag2", false)[0] as TextBox).Text;
                string flag3 = (tp.Controls.Find("flag3", false)[0] as TextBox).Text;

                data += effectBase + "#" + effectID + "#" + targets + "#" + element + "#" + duration + "#" + delay + "#" + zoneSize + "#" + zoneExtensible + "#" + handToHand + "#" + min + "#" + max + "#" + flag1 + "#" + flag2 + "#" + flag3 + "|";
            }

            if (data != "")
                data = data.Substring(0, data.Length - 1);

            (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID.ToString() == spellID.Text && f.level.ToString() == level.Text).extraDataEffect = data;

            this.Enabled = false;

            DataBase.DataTables.dataContext.Update("spells", "id", (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID.ToString() == spellID.Text && f.level.ToString() == level.Text));

            MessageBox.Show("Sauveguardé");
            this.Enabled = true;
        }

        private void add_Click(object sender, EventArgs e)
        {
            ///////////////////////////////////////////////////////////
            ComboBox _targetSelection = new ComboBox();
            Label label5 = new Label();
            TextBox _effectID = new TextBox();
            Label label4 = new Label();
            ComboBox _effectBase = new ComboBox();
            Label label3 = new Label();
            ComboBox _targets = new ComboBox();
            Button Ajouter = new Button();
            Label label6 = new Label();
            ComboBox _element = new ComboBox();
            Label label7 = new Label();
            TextBox _duration = new TextBox();
            TextBox _delay = new TextBox();
            Label label8 = new Label();
            TextBox _zoneSize = new TextBox();
            Label label9 = new Label();
            Label label10 = new Label();
            ComboBox _zoneExtensible = new ComboBox();
            TextBox _handToHand = new TextBox();
            Label label11 = new Label();
            TextBox _max = new TextBox();
            Label label12 = new Label();
            TextBox _min = new TextBox();
            Label label13 = new Label();
            TextBox _flag1 = new TextBox();
            Label label15 = new Label();
            TextBox _flag2 = new TextBox();
            Label label14 = new Label();
            TextBox _flag3 = new TextBox();
            Label label16 = new Label();
            Button button1 = new Button();
            Button button2 = new Button();

            mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID.ToString() == spellID.Text && f.level.ToString() == level.Text);

            TabPage tp;
            if (effectTab.TabPages[0].Controls.Count == 0)
                tp = effectTab.SelectedTab;
            else
            {
                tp = new TabPage();
                effectTab.TabPages.Add(tp);
                effectTab.SelectTab(effectTab.TabCount - 1);
            }

            tp.Controls.Add(button2);
            tp.Controls.Add(_flag3);
            tp.Controls.Add(label16);
            tp.Controls.Add(_flag2);
            tp.Controls.Add(label14);
            tp.Controls.Add(_flag1);
            tp.Controls.Add(label15);
            tp.Controls.Add(_max);
            tp.Controls.Add(label12);
            tp.Controls.Add(_min);
            tp.Controls.Add(label13);
            tp.Controls.Add(_handToHand);
            tp.Controls.Add(label11);
            tp.Controls.Add(_zoneExtensible);
            tp.Controls.Add(label10);
            tp.Controls.Add(_zoneSize);
            tp.Controls.Add(label9);
            tp.Controls.Add(_delay);
            tp.Controls.Add(label8);
            tp.Controls.Add(_duration);
            tp.Controls.Add(label7);
            tp.Controls.Add(_element);
            tp.Controls.Add(label6);
            tp.Controls.Add(Ajouter);
            tp.Controls.Add(_targets);
            tp.Controls.Add(_targetSelection);
            tp.Controls.Add(label5);
            tp.Controls.Add(_effectID);
            tp.Controls.Add(label4);
            tp.Controls.Add(_effectBase);
            tp.Controls.Add(label3);


            // 
            // targetSelection
            // 
            _targetSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            _targetSelection.FormattingEnabled = true;
            _targetSelection.Items.AddRange(new object[] {
                    "self",
                    "enemy_1",
                    "ally_1",
                    "none",
                    "ally_summon",
                    "enemy_summon"});
            _targetSelection.Location = new Point(233, 67);
            _targetSelection.Name = "targetSelection";
            _targetSelection.Size = new Size(121, 21);
            _targetSelection.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(17, 70);
            label5.Name = "label5";
            label5.Size = new Size(38, 13);
            label5.TabIndex = 4;
            label5.Text = "Target";
            // 
            // effectID
            // 
            _effectID.Location = new Point(106, 42);
            _effectID.Name = "effectID";
            _effectID.Size = new Size(52, 20);
            _effectID.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 45);
            label4.Name = "label4";
            label4.Size = new Size(46, 13);
            label4.TabIndex = 2;
            label4.Text = "EffectID";
            // 
            // effectBase
            // 
            _effectBase.DropDownStyle = ComboBoxStyle.DropDownList;
            _effectBase.FormattingEnabled = true;
            _effectBase.Items.AddRange(new object[] {
                    "dammage",
                    "defaultDamage",
                    "heal",
                    "summon",
                    "drain_life",
                    "drain_pc",
                    "drain_pm",
                    "drain_power",
                    "mode",
                    "boost",
                    "killSummonToBoost",
                    "state"
            });
            _effectBase.Location = new Point(106, 16);
            _effectBase.Name = "effectBase";
            _effectBase.Size = new Size(121, 21);
            _effectBase.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 20);
            label3.Name = "label3";
            label3.Size = new Size(59, 13);
            label3.TabIndex = 0;
            label3.Text = "EffectBase";
            // 
            // targets
            // 
            _targets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _targets.FormattingEnabled = true;
            _targets.Location = new System.Drawing.Point(106, 67);
            _targets.Name = "targets";
            _targets.Size = new System.Drawing.Size(121, 21);
            _targets.TabIndex = 6;
            // 
            // Ajouter
            // 
            Ajouter.Location = new System.Drawing.Point(360, 66);
            Ajouter.Name = "Ajouter";
            Ajouter.Size = new System.Drawing.Size(75, 23);
            Ajouter.TabIndex = 7;
            Ajouter.Text = "add";
            Ajouter.UseVisualStyleBackColor = true;
            Ajouter.Click += new System.EventHandler(Ajouter_Click);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(17, 96);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(45, 13);
            label6.TabIndex = 8;
            label6.Text = "Element";
            // 
            // element
            // 
            _element.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _element.FormattingEnabled = true;
            _element.Items.AddRange(new object[] {
                    "doton",
                    "katon",
                    "futon",
                    "suiton",
                    "raiton",
                    "neutre"});
            _element.Location = new System.Drawing.Point(106, 93);
            _element.Name = "element";
            _element.Size = new System.Drawing.Size(121, 21);
            _element.TabIndex = 9;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(17, 123);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(47, 13);
            label7.TabIndex = 10;
            label7.Text = "Duration";
            // 
            // duration
            // 
            _duration.Location = new System.Drawing.Point(106, 119);
            _duration.Name = "duration";
            _duration.Size = new System.Drawing.Size(52, 20);
            _duration.TabIndex = 11;
            // 
            // delay
            // 
            _delay.Location = new System.Drawing.Point(106, 144);
            _delay.Name = "delay";
            _delay.Size = new System.Drawing.Size(52, 20);
            _delay.TabIndex = 13;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(17, 148);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(34, 13);
            label8.TabIndex = 12;
            label8.Text = "Delay";
            // 
            // zoneSize
            // 
            _zoneSize.Location = new System.Drawing.Point(106, 169);
            _zoneSize.Name = "zoneSize";
            _zoneSize.Size = new System.Drawing.Size(52, 20);
            _zoneSize.TabIndex = 15;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(17, 173);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(55, 13);
            label9.TabIndex = 14;
            label9.Text = "Zone Size";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(17, 200);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(83, 13);
            label10.TabIndex = 16;
            label10.Text = "Zone Extensible";
            // 
            // zoneExtensible
            // 
            _zoneExtensible.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _zoneExtensible.FormattingEnabled = true;
            _zoneExtensible.Items.AddRange(new object[] {
                    "true",
                    "false"});
            _zoneExtensible.Location = new System.Drawing.Point(106, 195);
            _zoneExtensible.Name = "zoneExtensible";
            _zoneExtensible.Size = new System.Drawing.Size(66, 21);
            _zoneExtensible.TabIndex = 17;
            // 
            // handToHand
            // 
            _handToHand.Location = new System.Drawing.Point(105, 223);
            _handToHand.Name = "handToHand";
            _handToHand.Size = new System.Drawing.Size(52, 20);
            _handToHand.TabIndex = 19;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(17, 227);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(72, 13);
            label11.TabIndex = 18;
            label11.Text = "HandToHand";
            // 
            // Max
            // 
            _max.Location = new System.Drawing.Point(432, 147);
            _max.Name = "Max";
            _max.Size = new System.Drawing.Size(52, 20);
            _max.TabIndex = 23;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(343, 151);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(27, 13);
            label12.TabIndex = 22;
            label12.Text = "Max";
            // 
            // Min
            // 
            _min.Location = new System.Drawing.Point(432, 122);
            _min.Name = "Min";
            _min.Size = new System.Drawing.Size(52, 20);
            _min.TabIndex = 21;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(343, 126);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(24, 13);
            label13.TabIndex = 20;
            label13.Text = "Min";
            // 
            // flag1
            // 
            _flag1.Location = new System.Drawing.Point(432, 172);
            _flag1.Name = "flag1";
            _flag1.Size = new System.Drawing.Size(52, 20);
            _flag1.TabIndex = 25;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(344, 176);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(36, 13);
            label15.TabIndex = 24;
            label15.Text = "Flag 1";
            // 
            // flag2
            // 
            _flag2.Location = new System.Drawing.Point(432, 198);
            _flag2.Name = "flag2";
            _flag2.Size = new System.Drawing.Size(52, 20);
            _flag2.TabIndex = 27;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(344, 202);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(36, 13);
            label14.TabIndex = 26;
            label14.Text = "Flag 2";
            // 
            // flag3
            // 
            _flag3.Location = new System.Drawing.Point(432, 223);
            _flag3.Name = "flag3";
            _flag3.Size = new System.Drawing.Size(52, 20);
            _flag3.TabIndex = 29;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(344, 227);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(36, 13);
            label16.TabIndex = 28;
            label16.Text = "Flag 3";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(132, 342);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(286, 36);
            button1.TabIndex = 30;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(438, 66);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 30;
            button2.Text = "remove";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(button2_Click);

            (tp.Controls.Find("effectBase", false)[0] as ComboBox).SelectedIndex = 0;
            (tp.Controls.Find("element", false)[0] as ComboBox).SelectedIndex = 0;
            (tp.Controls.Find("zoneExtensible", false)[0] as ComboBox).SelectedIndex = 0;
            tp.Text = "Effect " + (effectTab.TabCount);
            effectTab.Enabled = true;
        }

        private void delete_Click(object sender, EventArgs e)
        {
            effectTab.TabPages.Remove(effectTab.SelectedTab);
        }

        private void spellEffectHandler_Load(object sender, EventArgs e)
        {
            string[] allLines = File.ReadAllLines("flagDetails.txt");
            foreach(string s in allLines)
            {
                flagDetailsStructure fdi = new flagDetailsStructure();
                fdi.baseEffect = s.Split('#')[0];
                fdi.flag1 = s.Split('#')[1];
                fdi.flag2 = s.Split('#')[2];
                fdi.flag3 = s.Split('#')[3];
                flagDetails.Add(fdi);
            }
        }
    }
}
