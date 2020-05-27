namespace NovaEffect
{
    partial class SpellTarget
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
            this.spellTargetCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SpellID = new System.Windows.Forms.TextBox();
            this.filter = new System.Windows.Forms.Button();
            this.targetList = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.Level = new System.Windows.Forms.TextBox();
            this.remove = new System.Windows.Forms.PictureBox();
            this.add = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Save = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.remove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.add)).BeginInit();
            this.SuspendLayout();
            // 
            // spellTargetCB
            // 
            this.spellTargetCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.spellTargetCB.FormattingEnabled = true;
            this.spellTargetCB.Location = new System.Drawing.Point(7, 22);
            this.spellTargetCB.Name = "spellTargetCB";
            this.spellTargetCB.Size = new System.Drawing.Size(89, 21);
            this.spellTargetCB.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Spell Target";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "sortID";
            // 
            // SpellID
            // 
            this.SpellID.Location = new System.Drawing.Point(29, 23);
            this.SpellID.Name = "SpellID";
            this.SpellID.Size = new System.Drawing.Size(42, 20);
            this.SpellID.TabIndex = 0;
            // 
            // filter
            // 
            this.filter.Location = new System.Drawing.Point(165, 20);
            this.filter.Name = "filter";
            this.filter.Size = new System.Drawing.Size(75, 23);
            this.filter.TabIndex = 3;
            this.filter.Text = "Filtre";
            this.filter.UseVisualStyleBackColor = true;
            this.filter.Click += new System.EventHandler(this.filter_Click);
            // 
            // targetList
            // 
            this.targetList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetList.FormattingEnabled = true;
            this.targetList.Location = new System.Drawing.Point(152, 22);
            this.targetList.Name = "targetList";
            this.targetList.Size = new System.Drawing.Size(91, 21);
            this.targetList.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.remove);
            this.panel1.Controls.Add(this.add);
            this.panel1.Controls.Add(this.targetList);
            this.panel1.Controls.Add(this.spellTargetCB);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(252, 56);
            this.panel1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Level";
            // 
            // Level
            // 
            this.Level.Location = new System.Drawing.Point(100, 24);
            this.Level.Name = "Level";
            this.Level.Size = new System.Drawing.Size(42, 20);
            this.Level.TabIndex = 1;
            // 
            // remove
            // 
            this.remove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.remove.Image = global::NovaEffect.Properties.Resources.Rightarrow;
            this.remove.Location = new System.Drawing.Point(128, 22);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(20, 21);
            this.remove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.remove.TabIndex = 8;
            this.remove.TabStop = false;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // add
            // 
            this.add.Cursor = System.Windows.Forms.Cursors.Hand;
            this.add.Image = global::NovaEffect.Properties.Resources.Leftarrow;
            this.add.Location = new System.Drawing.Point(100, 22);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(20, 21);
            this.add.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.add.TabIndex = 7;
            this.add.TabStop = false;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "List Targets";
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(12, 112);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(250, 23);
            this.Save.TabIndex = 10;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // SpellTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 145);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Level);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.filter);
            this.Controls.Add(this.SpellID);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SpellTarget";
            this.Text = "SpellTarget";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.remove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.add)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox spellTargetCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SpellID;
        private System.Windows.Forms.Button filter;
        private System.Windows.Forms.ComboBox targetList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Level;
        private System.Windows.Forms.PictureBox add;
        private System.Windows.Forms.PictureBox remove;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Save;
    }
}