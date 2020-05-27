using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Features.Handlers
{
    static class pc
    {
        public static string Apply(object[] parameters)
        {
            Actor spellCaster = parameters[0] as Actor;
            Effects.ZoneEffect.ZoneEffectTemplate affected = parameters[1] as Effects.ZoneEffect.ZoneEffectTemplate;
            Actor sacrifiedSummon = parameters[2] as Actor;
            string criteria = (string)parameters[3];
            mysql.spells spell = parameters[4] as mysql.spells;
            Actor.effects effect = parameters[5] as Actor.effects;
            bool cd = (bool)parameters[6];
            ///////////////////////////////////////////////////////////

            Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);

            Misc.Criteria.Handlers.Icriteria ci = Misc.Criteria.Handlers.translation.translate(criteria);
            object[] parameters1 = new object[1];
            parameters1[0] = criteria;
            int value = Convert.ToInt32(ci.Apply(parameters1));

            // un evenement dois être créer pour gérer tous seul si le max pdv a été franchie grace au variable d'accessibilité get, set
            affected.AffectedActor.originalPc += value;
            affected.AffectedActor.currentPc += value;
            string buffer =  affected.AffectedActor.Pseudo + ":" + value + ":" + affected.AffectedActor.originalPc + ":pc";

            //// ajout du sort dans la liste des envoutements visibles pour les joueurs concernés
            if (spellCaster.BuffsList.Exists(f => f.SortID == spell.spellID && !f.system))
            {
                // sort trouvé
                Actor.Buff piEnv = spellCaster.BuffsList.Find(f => f.SortID == spell.spellID && !f.system);
                piEnv.playerRoxed.Add("null");
            }
            else
            {
                // ajout du sort dans les envoutements
                Actor.Buff piEnv1 = new Actor.Buff();
                piEnv1.SortID = spell.spellID;
                piEnv1.title = spell.spellName;
                piEnv1.Debuffable = true;
                piEnv1.VisibleToPlayers = true;
                piEnv1.playerRoxed.Add("null");
                piEnv1.relanceInterval = spell.relanceInterval;
                piEnv1.BuffState = Enums.Buff.State.Fin;
                piEnv1.relanceParTour = spell.relanceParTour;
                piEnv1.system = false;
                piEnv1.Duration = effect.duration;
                piEnv1.Bonus.originalPc = value;
                piEnv1.Cd = cd;
                piEnv1.Player = spellCaster.Pseudo;
                spellCaster.BuffsList.Add(piEnv1);
            }

            return buffer;
        }
    }
}
