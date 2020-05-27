using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Effects.TypeEffect
{
    static class threeFrontalPerpendicular
    {
        // Not extentable
        ///                    [ ]                       [ ][ ][ ]                  [ ]
        ///                 [x][ ]          or              [x]             or      [ ][x]          or          [x]
        ///                    [ ]                                                  [ ]                      [ ][ ][ ]
        ///

        public static bool Apply(object[] parameters)
        {
            // params = im, spellPos, spellID
            /*NetIncomingMessage im = parameters[0] as NetIncomingMessage;
            Point spellPos = parameters[1] as Point;
            int spellID = (int)parameters[2];

            battle _battle = battle.battles.Find(f => f.idBattle == (im.SenderConnection.Tag as PlayerInfo).idBattle);
            PlayerInfo pi = _battle.AllPlayersByOrder.Find(f => f.Pseudo == (im.SenderConnection.Tag as PlayerInfo).Pseudo);
            PlayerInfo.infos_sorts spell_player_state = (im.SenderConnection.Tag as PlayerInfo).sorts.Find(f => f.sortID == spellID);
            mysql.spells spell_template = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == spell_player_state.level);

            Point playerPos = pi.map_position;
            PlayerInfo playerTargeted = _battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);
            */
            return true;
        }
    }
}
