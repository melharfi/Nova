using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER
{
    internal static class GlobalVariable
    {
        public static int MaxStepsInWayPoint;               // maximum des tuiles dans un waypoint pour eviter un overflow, ce variable vaux 100, vérifier ou il se met a égale 100, peux etre depuis la BDD
        public static int WalkingMaxSteps;                  // combien de case un waypoint est considéré comme de la marche, si ce le nombre de tuiles dans un waypoint dépasse ce nombre donc le joueur dois courir
    }
}
