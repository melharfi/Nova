namespace NovaEffect
{
    partial class Module
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
            this.effectHandler = new System.Windows.Forms.Button();
            this.SpellTarget = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // effectHandler
            // 
            this.effectHandler.Location = new System.Drawing.Point(155, 12);
            this.effectHandler.Name = "effectHandler";
            this.effectHandler.Size = new System.Drawing.Size(124, 23);
            this.effectHandler.TabIndex = 1;
            this.effectHandler.Text = "Effect Handler";
            this.effectHandler.UseVisualStyleBackColor = true;
            this.effectHandler.Click += new System.EventHandler(this.effectHandler_Click);
            // 
            // SpellTarget
            // 
            this.SpellTarget.Location = new System.Drawing.Point(12, 12);
            this.SpellTarget.Name = "SpellTarget";
            this.SpellTarget.Size = new System.Drawing.Size(113, 23);
            this.SpellTarget.TabIndex = 0;
            this.SpellTarget.Text = "Spell Target";
            this.SpellTarget.UseVisualStyleBackColor = true;
            this.SpellTarget.Click += new System.EventHandler(this.SpellTarget_Click);
            // 
            // Module
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 47);
            this.Controls.Add(this.SpellTarget);
            this.Controls.Add(this.effectHandler);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Module";
            this.Text = "Module";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button effectHandler;
        private System.Windows.Forms.Button SpellTarget;
    }
}