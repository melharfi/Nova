using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Effects.ZoneEffect
{
    //
    static class rhombus
    {
        public static List<Effects.ZoneEffect.ZoneEffectTemplate> affected(object[] parameters)
        {
            /// Glyphe / losange  / rhombus
            ///                 [ ]
            ///              [ ][ ][ ]
            ///           [ ][ ][ ][ ][ ]
            ///        [ ][ ][ ][x][ ][ ][ ]
            ///           [ ][ ][ ][ ][ ]
            ///              [ ][ ][ ]
            ///                 [ ]
            ///                 
            Battle _battle = parameters[0] as Battle;
            Point spellPos = parameters[1] as Point;
            mysql.spells spell_template = parameters[2] as mysql.spells;
            Actor spellCaster = parameters[3] as Actor;

            // liste qui contiens tous les joueurs dans la zone d'effet
            List<ZoneEffectTemplate> l = new List<ZoneEffectTemplate>();

            // joueur dans la case centrale
            Actor pi = _battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);

            if (pi != null)
            {
                ZoneEffectTemplate centralT = new ZoneEffectTemplate();
                centralT.AffectedActor = pi;
                centralT.Pertinance = 100;
                l.Add(centralT);

                // algo pour trouver tout les adversaire dans la glyph
                for (int vertical = 0; vertical < spell_template.sizeEffect; vertical++)
                {
                    for (int horizontal = 0; horizontal < spell_template.sizeEffect; horizontal++)
                    {
                        if (vertical == 0 && horizontal == 0)
                            continue;

                        // tuile à droite du centre
                        Actor rightPlayer = _battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X + horizontal && f.map_position.Y == spellPos.Y + vertical);
                        Effects.ZoneEffect.ZoneEffectTemplate rightT = new ZoneEffectTemplate();
                        rightT.AffectedActor = rightPlayer;
                        rightT.Pertinance = 100 - ((vertical * 10) / 100) - ((horizontal * 10) / 100);
                        l.Add(rightT);

                        // tuile à gauche du centre
                        Actor leftPlayer = _battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X - horizontal && f.map_position.Y == spellPos.Y - vertical);
                        Effects.ZoneEffect.ZoneEffectTemplate leftT = new ZoneEffectTemplate();
                        leftT.AffectedActor = leftPlayer;
                        leftT.Pertinance = 100 - ((vertical * 10) / 100) - ((horizontal * 10) / 100);
                        l.Add(leftT);
                    }
                }
            }
            
            return l;
        }
    }
}
