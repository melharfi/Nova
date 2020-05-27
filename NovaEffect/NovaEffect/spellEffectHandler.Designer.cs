namespace NovaEffect
{
    partial class spellEffectHandler
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
            this.label1 = new System.Windows.Forms.Label();
            this.spellID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.filter = new System.Windows.Forms.Button();
            this.effectTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.PictureBox();
            this.delete = new System.Windows.Forms.PictureBox();
            this.level = new System.Windows.Forms.ComboBox();
            this.effectTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.add)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delete)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "spell ID";
            // 
            // spellID
            // 
            this.spellID.Location = new System.Drawing.Point(60, 17);
            this.spellID.Name = "spellID";
            this.spellID.Size = new System.Drawing.Size(44, 20);
            this.spellID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "level";
            // 
            // filter
            // 
            this.filter.Location = new System.Drawing.Point(249, 15);
            this.filter.Name = "filter";
            this.filter.Size = new System.Drawing.Size(96, 23);
            this.filter.TabIndex = 3;
            this.filter.Text = "Filter";
            this.filter.UseVisualStyleBackColor = true;
            this.filter.Click += new System.EventHandler(this.filter_Click);
            // 
            // effectTab
            // 
            this.effectTab.Controls.Add(this.tabPage1);
            this.effectTab.Enabled = false;
            this.effectTab.Location = new System.Drawing.Point(15, 46);
            this.effectTab.Name = "effectTab";
            this.effectTab.SelectedIndex = 0;
            this.effectTab.Size = new System.Drawing.Size(524, 290);
            this.effectTab.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(516, 264);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Effect 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(132, 342);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(286, 36);
            this.button1.TabIndex = 6;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // add
            // 
            this.add.Image = global::NovaEffect.Properties.Resources.add;
            this.add.Location = new System.Drawing.Point(365, 17);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(20, 20);
            this.add.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.add.TabIndex = 8;
            this.add.TabStop = false;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // delete
            // 
            this.delete.Image = global::NovaEffect.Properties.Resources.delete;
            this.delete.Location = new System.Drawing.Point(398, 17);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(20, 20);
            this.delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.delete.TabIndex = 7;
            this.delete.TabStop = false;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // level
            // 
            this.level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.level.FormattingEnabled = true;
            this.level.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.level.Location = new System.Drawing.Point(191, 15);
            this.level.Name = "level";
            this.level.Size = new System.Drawing.Size(52, 21);
            this.level.TabIndex = 2;
            // 
            // spellEffectHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 386);
            this.Controls.Add(this.level);
            this.Controls.Add(this.add);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.effectTab);
            this.Controls.Add(this.filter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.spellID);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "spellEffectHandler";
            this.Text = "spellEffectHandler";
            this.Load += new System.EventHandler(this.spellEffectHandler_Load);
            this.effectTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.add)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delete)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox spellID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button filter;
        private System.Windows.Forms.TabControl effectTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox delete;
        private System.Windows.Forms.PictureBox add;
        private System.Windows.Forms.ComboBox level;
    }
}