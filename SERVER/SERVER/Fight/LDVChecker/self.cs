using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Fight.LDVChecker
{
    static class self
    {
        public static Enums.LDVChecker.Availability Apply(object[] parameters)
        {
            ///////////////// parames
            /*PlayerInfo spellCaster = parameters[0] as PlayerInfo;
            Point spellPoint = parameters[1] as Point;
            mysql.spells spell = parameters[2] as mysql.spells;
            ////////////////////////////////

            int pe = (Convert.ToBoolean(spell.peModifiable) ? spellCaster.pe + spell.pe : spell.pe) + spell.distanceFromMelee;
            battle _battle = battle.battles.Find(f => f.idBattle == spellCaster.idBattle);
            bool inEmptyTileOnly = false;
            List<Enums.spell_effect_target.targets> spell_Targets = common.spellTarget(spell);

            if (spell_Targets.Exists(f => f == Enums.spell_effect_target.targets.none) && spell_Targets.Count == 1)
                inEmptyTileOnly = true;

            List<Point> allTuiles = new List<Point>();      // liste qui contiens tous les tuiles affecté par le sort y compris un obstacle ou pas
            List<sort_tuile_info> allTuilesInfo = common.rhombusShapeWithObtsacle(spellCaster, spellPoint, spell);*/

            // on retourne toujours accessible puisque le sort se lance sur sois
            return Enums.LDVChecker.Availability.allowed;
        }
        }
}
