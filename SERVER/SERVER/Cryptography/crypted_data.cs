using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Cryptography
{
    static class crypted_data
    {
        // decodage de la chaine des effets
        // effect base # effect id # target(separed by / if many) # element # duraion # delay # zoneSize # zoneExtensible # handToHandDistance # min # max # flag1 # flag2 # flag3 | (separator between effects)
        public static List<Actor.effects> effects_decoder(int spellID, int level)
        {
            mysql.spells info_sort = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == level);
            List<Actor.effects> Alleffects = new List<Actor.effects>();
            
            if (info_sort.extraDataEffect == null || info_sort.extraDataEffect == "")
                return Alleffects;

            int cnt = info_sort.extraDataEffect.Split('|').Length;
            for (int c = 0; c < cnt; c++)
            {
                Actor.effects effect = new Actor.effects();
                string[] effect_data = info_sort.extraDataEffect.Split('|')[c].Split('#');
                
                Enums.Effect.effects_base effect_base;
                Enum.TryParse(effect_data[0], out effect_base);

                effect.base_effect = effect_base;

                effect.effect_id = Convert.ToInt32(effect_data[1]);
                string[] targets_data = effect_data[2].Split('/');
                for (int c2 = 0; c2 < targets_data.Length; c2++)
                {
                    Enums.spell_effect_target.targets target;
                    Enum.TryParse(targets_data[c2], out target);
                    effect.targets.Add(target);
                }
                Enums.Chakra.Element element;
                Enum.TryParse(effect_data[3], out element);

                effect.duration = Convert.ToInt32(effect_data[4]);
                effect.delay = Convert.ToInt32(effect_data[5]);
                effect.zoneSize = Convert.ToInt32(effect_data[6]);
                effect.zoneExtensible = Convert.ToBoolean(effect_data[7]);
                effect.handToHandDistance = Convert.ToInt32(effect_data[8]);
                effect.min = Convert.ToInt32(effect_data[9]);
                effect.max = Convert.ToInt32(effect_data[10]);
                effect.flag1 = effect_data[11];
                effect.flag2 = effect_data[12];
                effect.flag3 = effect_data[13];

                Alleffects.Add(effect);
            }

            return Alleffects;
        }
    }
}
