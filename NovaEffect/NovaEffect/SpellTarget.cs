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
    public partial class SpellTarget : Form
    {
        int _spellID, _level;

        public SpellTarget()
        {
            InitializeComponent();
        }

        private void add_Click(object sender, EventArgs e)
        {
            if(targetList.SelectedItem.ToString() != "")
            {
                spellTargetCB.Items.Add(targetList.SelectedItem);
                targetList.Items.Remove(targetList.SelectedItem);
            }

            if (spellTargetCB.Items.Count > 0)
                spellTargetCB.SelectedIndex = 0;

            if (targetList.Items.Count > 0)
                targetList.SelectedIndex = 0;
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if(spellTargetCB.SelectedItem.ToString() != "")
            {
                targetList.Items.Add(spellTargetCB.SelectedItem);
                spellTargetCB.Items.Remove(spellTargetCB.SelectedItem);
            }

            if (spellTargetCB.Items.Count > 0)
                spellTargetCB.SelectedIndex = 0;

            if (targetList.Items.Count > 0)
                targetList.SelectedIndex = 0;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string targets = "";
            foreach (string s in spellTargetCB.Items)
                targets += s + "#";

            if (targets != "")
                targets = targets.Substring(0, targets.Length - 1);

            (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == _spellID && f.level == _level).target = targets;
            DataBase.DataTables.dataContext.Update("spells", "id", (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == _spellID && f.level == _level));
            MessageBox.Show("Données sauveguardé");
        }

        private void filter_Click(object sender, EventArgs e)
        {
            if(!int.TryParse(SpellID.Text, out _spellID))
            {
                MessageBox.Show("Veuillez entrez une valeur numérique supérieur ou égale à 0 pour le champ SpellID");
                SpellID.Focus();
                return;
            }

            if(!int.TryParse(Level.Text, out _level))
            {
                MessageBox.Show("Veuillez entrez une valeur numérique supérieur ou égale à 0 pour le champ Level");
                Level.Focus();
                return;
            }

            mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == _spellID && f.level == _level);

            if (spell.target != null)
            {
                string[] filter = spell.target.Split('#');
                spellTargetCB.Items.AddRange(filter);

                targetList.Items.AddRange("self#enemy_1#ally_1#none#ally_summon#enemy_summon#ally_all#enemy_all".Split('#'));
                foreach (string s in spellTargetCB.Items)
                    targetList.Items.Remove(s);

                if (spellTargetCB.Items.Count > 0)
                    spellTargetCB.SelectedIndex = 0;

                if (targetList.Items.Count > 0)
                    targetList.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No target found for this spell");
                targetList.Items.AddRange("self#enemy_1#ally_1#none#ally_summon#enemy_summon#ally_all#enemy_all".Split('#'));
            }
        }
    }
}
