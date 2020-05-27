using System.Collections.Generic;

namespace SERVER.Effects.ZoneEffect
{
    static class threeFrontalPerpendicular
    {
        // Not extentable
        ///                    [ ]                       [ ][ ][ ]                  [ ]
        ///                 [x][ ]          or              [x]             or      [ ][x]          or          [x]
        ///                    [ ]                                                  [ ]                      [ ][ ][ ]
        ///

        public static List<ZoneEffectTemplate> affected(object[] parameters)
        {
            Battle battle = (Battle)parameters[0];
            Point spellPos = (Point)parameters[1];
            //mysql.spells spell_template = parameters[2] as mysql.spells;
            Actor actor = (Actor)parameters[3];

            // liste des joueurs affecté par l'effet
            List<ZoneEffectTemplate> l = new List<ZoneEffectTemplate>();

            Actor middleTile = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);

            if (middleTile != null)
            {
                ZoneEffectTemplate t = new ZoneEffectTemplate
                {
                    AffectedActor = middleTile,
                    Pertinance = 100
                };

                l.Add(t);
            }

            // determination de l'orientation pour extraire les positions
            if (actor.map_position.X == spellPos.X)
            {
                // vers le bas
                Actor leftTile = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X - 1 && f.map_position.Y == spellPos.Y);

                if (leftTile != null)
                {
                    ZoneEffectTemplate t = new ZoneEffectTemplate
                    {
                        AffectedActor = leftTile,
                        Pertinance = 90
                    };
                    // pertinance toujours 100% puisque le sort se lance sur une seul case qui est la case centrale

                    l.Add(t);
                }

                // case à droite
                Actor rightTile = battle.AllPlayersByOrder.Find(f => f.map_position.X + 1 == spellPos.X && f.map_position.Y == spellPos.Y);

                if (rightTile == null) return l;
                {
                    ZoneEffectTemplate t = new ZoneEffectTemplate
                    {
                        AffectedActor = rightTile,
                        Pertinance = 90
                    };
                    // pertinance toujours 100% puisque le sort se lance sur une seul case qui est la case centrale

                    l.Add(t);
                }
            }
            else
            {
                // vers l'adroite ou la gauche
                Actor upTile = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y - 1);

                if (upTile != null)
                {
                    ZoneEffectTemplate t = new ZoneEffectTemplate
                    {
                        AffectedActor = upTile,
                        Pertinance = 90
                    };
                    // pertinance toujours 100% puisque le sort se lance sur une seul case qui est la case centrale

                    l.Add(t);
                }

                // case à droite
                Actor downTile = battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y + 1);

                if (downTile == null) return l;
                {
                    ZoneEffectTemplate t = new ZoneEffectTemplate
                    {
                        AffectedActor = downTile,
                        Pertinance = 90
                    };
                    // pertinance toujours 100% puisque le sort se lance sur une seul case qui est la case centrale

                    l.Add(t);
                }
            }

            return l;
        }
    }
}
