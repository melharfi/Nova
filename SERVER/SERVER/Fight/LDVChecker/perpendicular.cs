using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Fight.LDVChecker
{
    public static class perpendicular
    {
        /// <summary>
        /// perpendicular.cs for spells checker PE
        /// </summary>
        ///                          [ ]
        ///                          [ ]
        ///                          [ ]
        /// exemple tile    [ ][ ][ ][x][ ][ ][ ]
        /// ///                      [ ]
        ///                          [ ]
        ///                          [ ]
        public static Enums.LDVChecker.Availability Apply(object[] parameters)
        {
            ///////////////// parames
            Actor spellCaster = parameters[0] as Actor;
            Point spellPoint = parameters[1] as Point;
            mysql.spells spell = parameters[2] as mysql.spells;
            ////////////////////////////////
            int pe = (Convert.ToBoolean(spell.peModifiable) ? spellCaster.pe + spell.pe : spell.pe) + spell.distanceFromMelee;
            Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);
            bool inEmptyTileOnly = false;
            List<Enums.spell_effect_target.targets> spell_Targets = CommonCode.SpellTarget(spell);

            if (spell_Targets.Exists(f => f == Enums.spell_effect_target.targets.none) && spell_Targets.Count == 1)
                inEmptyTileOnly = true;
            ////////////////////////////////
            List<Point> allTuiles = new List<Point>();      // list qui contiens tous les tuiles affecté par le sort y compris un obstacle ou pas
            List<SortTuileInfo> allTuilesInfo = CommonCode.PerpondicularShapeWithObstacle(spellCaster, spellPoint, spell);

            List<SortTuileInfo> meleeTileInfo = CommonCode.PerpondicularShapeWithoutObstacle(spellCaster, spell.distanceFromMelee);
            foreach (SortTuileInfo sti in meleeTileInfo)
                allTuilesInfo.RemoveAll(f => f.TuilePoint.X == sti.TuilePoint.X && f.TuilePoint.Y == sti.TuilePoint.Y);

            SortTuileInfo focusedTile = allTuilesInfo.Find(f => f.TuilePoint.X == spellPoint.X && f.TuilePoint.Y == spellPoint.Y);
            if (focusedTile != null)
            {
                // les obstacle joueurs sont prise en compte par le sort
                if (!inEmptyTileOnly && !focusedTile.IsBlockingView && focusedTile.IsWalkable)
                {
                    // traitement quand le sort est autorisé sur cette emplacement
                    return Enums.LDVChecker.Availability.allowed;
                }
                else
                {
                    // traitement quand le sorts n'est pas autorisé, ou la case est un obstale qui est un joueur, walkable = true, blockingview = true
                    if (!inEmptyTileOnly && focusedTile.IsWalkable && focusedTile.IsBlockingView && _battle.AllPlayersByOrder.Exists(f => f.map_position.X == spellPoint.X && f.map_position.Y == spellPoint.Y))
                        return Enums.LDVChecker.Availability.allowed;
                    else if (inEmptyTileOnly && !focusedTile.IsBlockingView && focusedTile.IsWalkable)
                        return Enums.LDVChecker.Availability.allowed;
                    else
                        return Enums.LDVChecker.Availability.notAllowed;
                }
            }
            else
            {
                // le sort est lancé sur un emplacement en dehors de la zone, le joueur triche
                return Enums.LDVChecker.Availability.outSide;
            }
        }
    }
}
