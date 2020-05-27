using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Enums
{
    public static class LDVChecker
    {
        // pour determiner si une le sort est autorisé a être lancé sur une tuile ou pas
        public enum Availability
        {
            allowed,            // spell autorisé
            notAllowed,         // spell non autorisé, case obstacle
            outSide,            // spell non autorisé, pas de porté, joueur triche
            nan                // impossible de determiner la direction, normalement ca deverai le deviner
        }
    }
}
