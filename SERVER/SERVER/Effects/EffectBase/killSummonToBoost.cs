using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Effects.EffectBase
{
    public static class killSummonToBoost
    {
        // contrôles auxiliaires specifique à l'effet en cours
        public static bool Apply(object[] parameters)
        {
            // params = im, spellPos, spellID
            NetIncomingMessage im = parameters[0] as NetIncomingMessage;
            Point spellPos = parameters[1] as Point;
            int spellID = (int)parameters[2];

            Battle _battle = Battle.Battles.Find(f => f.IdBattle == (im.SenderConnection.Tag as Actor).idBattle);
            Actor pi = _battle.AllPlayersByOrder.Find(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo);
            Actor.SpellsInformations spell_player_state = (im.SenderConnection.Tag as Actor).sorts.Find(f => f.SpellId == spellID);
            mysql.spells spell_template = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == spell_player_state.Level);

            Point playerPos = pi.map_position;
            Actor playerTargeted = _battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);

            #region check si la case centrale est bien une invocation avec les critères demandés
            // il faut que le sort sois lancé sur l'une des invocations du joueur en cours
            // on cherche si le joueur a une invocation sur le terrain
            Actor piInvoked = _battle.AllPlayersByOrder.Find(f => f.Pseudo.IndexOf("$") != -1 && f.Pseudo.Substring(0, f.Pseudo.IndexOf("$")) == pi.Pseudo && f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);
            if (piInvoked == null)
                return false;

            // check si l'idForSummon est bien celui du flag1
            // on cherche parmis les effet celui du killSummonToBoost pour soustraire le flag1
            Actor.effects effect = spell_player_state.effect.Find(f => f.base_effect.ToString() == "killSummonToBoost");
            if(effect != null)
            {
                int idForSummon = 0;
                if (!int.TryParse(effect.flag1, out idForSummon))
                    return false;

                if (piInvoked.summonID != idForSummon)
                    return false;
            }
            #endregion

            return true;
        }
    }
}
