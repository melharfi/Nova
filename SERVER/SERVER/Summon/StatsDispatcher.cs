using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVER.Enums;

namespace SERVER.Summon
{
    static class StatsDispatcher
    {
        public static Actor Apply(Actor spellCaster, mysql.summon summon, mysql.spells spell_Template, Actor.effects effect)
        {
            // si la classe PlayerInfo est modifié comme l'ajout d'un nouveau champ, il faut ajouter son attribution ici
            Actor pi = new Actor();
            pi.blocage = MnemonicStatsDispatcher.Int_Apply(summon.blocage, spellCaster.blocage);
            pi.cd = MnemonicStatsDispatcher.Int_Apply(summon.cd, spellCaster.cd);
            pi.BuffsList = new List<Actor.Buff>();
            pi.maxHealth = MnemonicStatsDispatcher.Int_Apply(summon.TotalPdv, spellCaster.maxHealth);
            pi.currentHealth = MnemonicStatsDispatcher.Int_Apply(summon.TotalPdv, spellCaster.maxHealth);
            pi.currentPc = MnemonicStatsDispatcher.Int_Apply(summon.pc, spellCaster.originalPc);
            pi.currentPm = MnemonicStatsDispatcher.Int_Apply(summon.pm, spellCaster.originalPm);
            pi.classeName = spellCaster.classeName;
            pi.domDotonFix = MnemonicStatsDispatcher.Int_Apply(summon.domDotonFix, spellCaster.domDotonFix);
            pi.domFix = MnemonicStatsDispatcher.Int_Apply(summon.domFix, spellCaster.domFix);
            pi.domFutonFix = MnemonicStatsDispatcher.Int_Apply(summon.domFutonFix, spellCaster.domFutonFix);
            pi.domKatonFix = MnemonicStatsDispatcher.Int_Apply(summon.domKatonFix, spellCaster.domKatonFix);
            pi.domRaitonFix = MnemonicStatsDispatcher.Int_Apply(summon.domRaitonFix, spellCaster.domRaitonFix);
            pi.domSuitonFix = MnemonicStatsDispatcher.Int_Apply(summon.domSuitonFix, spellCaster.domSuitonFix);
            pi.doton = MnemonicStatsDispatcher.Int_Apply(summon.doton, spellCaster.doton);
            pi.dodgeCD = MnemonicStatsDispatcher.Int_Apply(summon.dodgeCD, spellCaster.dodgeCD);
            pi.dodgePC = MnemonicStatsDispatcher.Int_Apply(summon.dodgePC, spellCaster.dodgePC);
            pi.dodgePE = MnemonicStatsDispatcher.Int_Apply(summon.dodgePE, spellCaster.dodgePE);
            pi.dodgePM = MnemonicStatsDispatcher.Int_Apply(summon.dodgePM, spellCaster.dodgePM);
            pi.escape = MnemonicStatsDispatcher.Int_Apply(summon.escape, spellCaster.escape);
            pi.futon = MnemonicStatsDispatcher.Int_Apply(summon.futon, spellCaster.futon);
            pi.idBattle = spellCaster.idBattle;
            pi.summons = MnemonicStatsDispatcher.Int_Apply(summon.summons, spellCaster.summons);
            pi.katon = MnemonicStatsDispatcher.Int_Apply(summon.katon, spellCaster.katon);
            pi.level = spell_Template.level;
            pi.maskColorString = MnemonicStatsDispatcher.Str_Apply(summon.MaskColors, spellCaster.maskColorString);
            pi.map = spellCaster.map;
            pi.directionLook = 0;
            pi.originalPc = MnemonicStatsDispatcher.Int_Apply(summon.pc, spellCaster.originalPc);
            pi.originalPm = MnemonicStatsDispatcher.Int_Apply(summon.pm, spellCaster.originalPm);
            pi.owner = spellCaster.Pseudo;
            pi.pe = MnemonicStatsDispatcher.Int_Apply(summon.pe, spellCaster.pe);
            pi.Pseudo = MnemonicStatsDispatcher.Str_Apply(summon.name, spellCaster.Pseudo); ;
            pi.raiton = MnemonicStatsDispatcher.Int_Apply(summon.raiton, spellCaster.raiton);
            pi.resiDotonFix = MnemonicStatsDispatcher.Int_Apply(summon.resiDotonFix, spellCaster.resiDotonFix);
            pi.resiDotonPercent = MnemonicStatsDispatcher.Int_Apply(summon.resiDotonPercent, spellCaster.resiDotonPercent);
            pi.resiFix = MnemonicStatsDispatcher.Int_Apply(summon.resiFix, spellCaster.resiFix);
            pi.resiFutonFix = MnemonicStatsDispatcher.Int_Apply(summon.resiFutonFix, spellCaster.resiFutonFix);
            pi.resiFutonPercent = MnemonicStatsDispatcher.Int_Apply(summon.resiFix, spellCaster.resiFix);
            pi.resiKatonFix = MnemonicStatsDispatcher.Int_Apply(summon.resiKatonFix, spellCaster.resiKatonFix);
            pi.resiKatonPercent = MnemonicStatsDispatcher.Int_Apply(summon.resiKatonPercent, spellCaster.resiKatonPercent);
            pi.resiRaitonFix = MnemonicStatsDispatcher.Int_Apply(summon.resiRaitonFix, spellCaster.resiRaitonFix);
            pi.resiRaitonPercent = MnemonicStatsDispatcher.Int_Apply(summon.resiRaitonPercent, spellCaster.resiRaitonPercent);
            pi.resiSuitonFix = MnemonicStatsDispatcher.Int_Apply(summon.resiSuitonFix, spellCaster.resiSuitonFix);
            pi.resiSuitonPercent = MnemonicStatsDispatcher.Int_Apply(summon.resiFix, spellCaster.resiFix);
            pi.removeCD = MnemonicStatsDispatcher.Int_Apply(summon.retraitCD, spellCaster.removeCD);
            pi.removePC = MnemonicStatsDispatcher.Int_Apply(summon.retraitPC, spellCaster.removePC);
            pi.removePE = MnemonicStatsDispatcher.Int_Apply(summon.retraitPE, spellCaster.removePE);
            pi.removePM = MnemonicStatsDispatcher.Int_Apply(summon.retraitPM, spellCaster.removePM);
            pi.sexe = spellCaster.sexe;

            for (int cnt = 0; cnt < summon.sorts.Split('/').Length; cnt++)
            {
                string tmp_data = summon.sorts.Split('/')[cnt];
                Actor.SpellsInformations _info_sorts = new Actor.SpellsInformations();
                _info_sorts.SpellId = Convert.ToInt32(tmp_data.Split(':')[0]);
                _info_sorts.SpellPlace = Convert.ToInt32(tmp_data.Split(':')[1]);
                _info_sorts.Level = Convert.ToInt32(tmp_data.Split(':')[2]);
                _info_sorts.SpellColor = Convert.ToInt32(tmp_data.Split(':')[3]);
                _info_sorts.effect = Cryptography.crypted_data.effects_decoder(_info_sorts.SpellId, _info_sorts.Level);
                pi.sorts.Add(_info_sorts);
            }

            pi.species = Species.Name.Summon;
            pi.suiton = MnemonicStatsDispatcher.Int_Apply(summon.suiton, spellCaster.suiton);
            pi.maxHealth = MnemonicStatsDispatcher.Int_Apply(summon.TotalPdv, spellCaster.maxHealth);
            pi.teamSide = spellCaster.teamSide;
            int.TryParse(effect.flag2, out pi.summonID);
            pi.visible = true;

            return pi;
        }
    }
}
