namespace SERVER.Effects.ZoneEffect
{
    internal class ZoneEffectTemplate
    {
        public Actor AffectedActor = new Actor();      // adveraire affecté par le sort

        public int Pertinance = 0;              // determine de combien l'adveraire est loin du centre de la zone de sort
                                                // tant qu'il est loin du centre tant que les dommage sont moindres
                                                // si = 100, donc l'adversaire aura 100% du dommage
                                                // si = 90, donc l'adversaire aura 90% du dommage
                                                // si = 80, donc l'adversaire aura 80% du dommage
                                                // si = 70, donc l'adversaire aura 70% du dommage
                                                // si = 60, donc l'adversaire aura 60% du dommage
                                                // si <= 50, donc l'adversaire aura 50% du dommage, seuil minimal = 50%
    }
}
