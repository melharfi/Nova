using System.Collections.Generic;

namespace SERVER.Effects.ZoneEffect
{
    // effet sur une seul tuile
    internal static class OneTile
    {
        /// Une seul case
        ///                 [x]
        ///                 
        public static List<ZoneEffectTemplate> affected(object[] parameters)
        {
            Battle battle = (Battle)parameters[0];
            Point spellPos = (Point)parameters[1];
            //mysql.spells spell_template = parameters[2] as mysql.spells;
            #region spellCaster not used
            //PlayerInfo spellCaster = parameters[3] as PlayerInfo;
            #endregion

            List<ZoneEffectTemplate> l = new List<ZoneEffectTemplate>();
            Actor actor = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);

            if (actor == null) return l;
            ZoneEffectTemplate t = new ZoneEffectTemplate
            {
                AffectedActor = actor,
                Pertinance = 100
            };
            // pertinance toujours 100% puisque le sort se lance sur une seul case qui est la case centrale

            l.Add(t);
            return l;
        }
    }
}
