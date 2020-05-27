/*
 * Crée par SharpDevelop.
 * Utilisateur: IBM
 * Date: 07/08/2016
 * Heure: 19:43
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Collections.Generic;
using Lidgren.Network;

namespace SERVER.Effects.TypeEffect
{
    /// <summary>
    /// perpendicular.cs for spells throwed away and not close to the player
    /// </summary>
    ///                          [ ]
    ///                          [ ]
    ///                          [ ]
    /// exemple tile    [ ][ ][ ][x][ ][ ][ ]
    /// ///                      [ ]
    ///                          [ ]
    ///                          [ ]
    public static class perpendicular
	{
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
