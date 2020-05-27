using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PetaPoco;

namespace NovaEffect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        

        private void connexion_Click(object sender, EventArgs e)
        {
            DataBase.DataTables.server = ip.Text;
            DataBase.DataTables.user = username.Text;
            DataBase.DataTables.password = password.Text;
            DataBase.DataTables.database = bdd.Text;
            DataBase.DataTables.IniPetaPoco();
            this.Visible = false;
            Module module = new Module();
            module.Show();
            module.FormClosed += Module_FormClosed;
        }

        private void Module_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void Seh_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void username_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                connexion_Click(null, null);
            }
        }
    }
}
