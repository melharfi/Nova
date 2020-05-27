using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Effects.Handlers
{
    static class state
    {
        public static string Apply(object[] parameters)
        {
            // flag 1 = id de la classe qui se trouve dans la table Classes afin de soustraire les caractèristique de l'invocation
            // flag 2 = caracteristique a booster
            // flag 3 = valeur
            Actor spellCaster = parameters[0] as Actor;
            List<Effects.ZoneEffect.ZoneEffectTemplate> affectedPlayers = parameters[1] as List<Effects.ZoneEffect.ZoneEffectTemplate>;
            int spellID = (int)parameters[2];
            Actor.effects effect = parameters[3] as Actor.effects;
            bool cd = Convert.ToBoolean(parameters[4]);
            Point spellPos = parameters[5] as Point;
            mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID);
            //NetConnection nim = MainClass.netServer.Connections.Find(f => f.Tag != null && (f.Tag as PlayerInfo).Pseudo == spellCaster.Pseudo);

            Actor.SpellsInformations infos_sorts = spellCaster.sorts.Find(f => f.SpellId == spellID);
            mysql.spells spell_Template = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == infos_sorts.Level);

            Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);
            ////////////////////////////////////////////////////////////////////////
            Enums.Buff.Name pState = (Enums.Buff.Name)Enum.Parse(typeof(Enums.Buff.Name), effect.flag1);

            //string DotonString = ""
            foreach (ZoneEffect.ZoneEffectTemplate affected in affectedPlayers)
            {
                affected.AffectedActor.BuffState.Add(pState);

                // on augemente la puissance des personnages dans la zone
                //spellCaster.doton += sorts.sort(sortID).isbl[spellLvl - 1].piBonus.doton;
                //DotonString += spellCaster.Pseudo + ":" + sorts.sort(sortID).isbl[spellLvl - 1].piBonus.doton + ":" + pi.doton;

                //string dom = "typeRox:etat:sinnin|cd:" + cdAllowed + "|chakra:neutre|deadList:" + playerDead + "|" + DotonString;
            }


            string buffer = "typeRox:etat:sinnin|cd:" + cd + "|chakra:neutre|deadList:|";
            return buffer;
        }
    }
}
