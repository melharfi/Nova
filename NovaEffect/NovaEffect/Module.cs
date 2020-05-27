using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovaEffect
{
    public partial class Module : Form
    {
        public Module()
        {
            InitializeComponent();
        }

        private void effectHandler_Click(object sender, EventArgs e)
        {
            spellEffectHandler seh = new spellEffectHandler();
            seh.FormClosed += Seh_FormClosed;
            this.Hide();
            seh.Show();
        }

        private void Seh_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void SpellTarget_Click(object sender, EventArgs e)
        {
            SpellTarget st = new NovaEffect.SpellTarget();
            st.FormClosed += St_FormClosed;
            this.Hide();
            st.Show();
        }

        private void St_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
    }
}
