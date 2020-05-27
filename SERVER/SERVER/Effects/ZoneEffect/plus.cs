using System.Collections.Generic;

namespace SERVER.Effects.ZoneEffect
{
    class plus
    {
        /// +
        ///                 [x]
        ///              [x][x][x]
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
            Actor piCentral = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);
            if (piCentral != null)
            {
                ZoneEffectTemplate t = new ZoneEffectTemplate
                {
                    AffectedActor = piCentral,
                    Pertinance = 100
                };

                l.Add(t);
            }

            Actor piLeft = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X - 1 && f.map_position.Y == spellPos.Y);
            if (piLeft != null)
            {
                ZoneEffectTemplate t = new ZoneEffectTemplate
                {
                    AffectedActor = piLeft,
                    Pertinance = 90
                };

                l.Add(t);
            }

            Actor piRight = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X + 1 && f.map_position.Y == spellPos.Y);

            if (piRight != null)
            {
                ZoneEffectTemplate t = new ZoneEffectTemplate
                {
                    AffectedActor = piRight,
                    Pertinance = 90
                };

                l.Add(t);
            }

            Actor piUp = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y - 1);

            if (piUp != null)
            {
                ZoneEffectTemplate t = new ZoneEffectTemplate
                {
                    AffectedActor = piUp,
                    Pertinance = 90
                };

                l.Add(t);
            }

            Actor piDown = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y + 1);

            if (piDown == null) return l;
            {
                ZoneEffectTemplate t = new ZoneEffectTemplate
                {
                    AffectedActor = piDown,
                    Pertinance = 90
                };
                // pertinance toujours 100% puisque le sort se lance sur une seul case qui est la case centrale

                l.Add(t);
            }
            return l;
        }
    }
}
