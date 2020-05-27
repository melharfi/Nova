namespace SERVER.Features.Handlers
{
    static class health
    {
        public static string Apply(object[] parameters)
        {
            //Actor spellCaster = parameters[0] as Actor;
            Effects.ZoneEffect.ZoneEffectTemplate affected = (Effects.ZoneEffect.ZoneEffectTemplate)parameters[1];
            Actor sacrifiedSummon = (Actor)parameters[2];
            string criteria = (string)parameters[3];
            //mysql.spells spell = parameters[4] as mysql.spells;
            //Actor.effects effect = parameters[5] as Actor.effects;
            //bool cd = (bool)parameters[6];
            ///////////////////////////////////////////////////////////

            //Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);

            Misc.Criteria.Handlers.Icriteria ci = Misc.Criteria.Handlers.translation.translate(criteria);
            object[] parameters1 = new object[2];
            parameters1[0] = sacrifiedSummon.currentHealth;
            parameters1[1] = criteria;
            int value = (int)ci.Apply(parameters1);

            // un evenement dois être créer pour gérer tous seul si le max pdv a été franchie grace au variable d'accessibilité get, set
            int reliquat;
            if (affected.AffectedActor.currentHealth + value > affected.AffectedActor.maxHealth)
                reliquat = affected.AffectedActor.maxHealth - affected.AffectedActor.currentHealth;
            else
                reliquat = value;

            affected.AffectedActor.currentHealth += reliquat;
            string buffer = affected.AffectedActor.Pseudo + ":" + reliquat + ":" + affected.AffectedActor.currentHealth + ":" + affected.AffectedActor.maxHealth + ":health";

            return buffer;
        }
    }
}
