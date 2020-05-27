using System;

namespace SERVER.Features.Handlers
{
    static class pm
    {
        public static string Apply(object[] parameters)
        {
            Actor actor = (Actor)parameters[0];
            Effects.ZoneEffect.ZoneEffectTemplate affected = (Effects.ZoneEffect.ZoneEffectTemplate)parameters[1];
            //Actor sacrifiedSummon = parameters[2] as Actor;
            string criteria = (string)parameters[3];
            mysql.spells spell = (mysql.spells)parameters[4];
            Actor.effects effect = (Actor.effects)parameters[5];
            bool cd = (bool)parameters[6];
            ///////////////////////////////////////////////////////////

            //Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);

            Misc.Criteria.Handlers.Icriteria ci = Misc.Criteria.Handlers.translation.translate(criteria);
            object[] parameters1 = new object[1];
            parameters1[0] = criteria;
            int value = Convert.ToInt32(ci.Apply(parameters1));

            // un evenement dois être créer pour gérer tous seul si le max pdv a été franchie grace au variable d'accessibilité get, set
            affected.AffectedActor.originalPm += value;
            affected.AffectedActor.currentPm += value;
            string buffer = affected.AffectedActor.Pseudo + ":" + value + ":" + affected.AffectedActor.originalPm + ":pm";

            //// ajout du sort dans la liste des envoutements visibles pour les joueurs concernés
            if (actor.BuffsList.Exists(f => f.SortID == spell.spellID && !f.system))
            {
                // sort trouvé
                Actor.Buff piEnv = actor.BuffsList.Find(f => f.SortID == spell.spellID && !f.system);
                piEnv.playerRoxed.Add("null");
            }
            else
            {
                // ajout du sort dans les envoutements
                Actor.Buff piEnv1 = new Actor.Buff
                {
                    SortID = spell.spellID,
                    title = spell.spellName,
                    Debuffable = true,
                    VisibleToPlayers = true,
                    relanceInterval = spell.relanceInterval,
                    BuffState = Enums.Buff.State.Fin,
                    relanceParTour = spell.relanceParTour,
                    system = false,
                    Duration = effect.duration,
                    Cd = cd,
                    Player = actor.Pseudo
                };
                piEnv1.playerRoxed.Add("null");
                piEnv1.Bonus.originalPm = value;
                actor.BuffsList.Add(piEnv1);
            }

            return buffer;
        }
    }
}
