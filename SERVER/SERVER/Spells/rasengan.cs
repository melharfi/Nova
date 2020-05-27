using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace SERVER.Spells
{
    public static class rasengan
    {
        // sort 01 de la classe Naruto
        // contôle specifique au sort
        public static bool spellChecker(object[] parameters)
        {
            int spellID = 0;
            Actor pi = parameters[0] as Actor;
            Point spellPos = parameters[1] as Point;
            NetIncomingMessage im = parameters[2] as NetIncomingMessage;
            
            Actor.SpellsInformations infos_sorts = (im.SenderConnection.Tag as Actor).sorts.Find(f => f.SpellId == spellID);
            Battle _battle = Battle.Battles.Find(f => f.IdBattle == (im.SenderConnection.Tag as Actor).idBattle);
            Point playerPos = pi.map_position;
            Actor playerTargeted = _battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);

            mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).FindLast(f => f.level == infos_sorts.Level);

            int porte = Math.Abs(playerPos.Y - spellPos.Y);
            
            return true;
        }
    }
}
