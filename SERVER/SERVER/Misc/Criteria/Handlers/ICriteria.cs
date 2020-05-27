using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Misc.Criteria.Handlers
{
    interface Icriteria
    {
        // tous les classes dérivés du dossier SERVER.Misc.Criteria deverons avoir le même contrat pour que la traduction du champ flag3 de l'éffet killSummonToBoost qui égale des fois la valeur $%:20 puisse être renommé en quelque chose comme "percent" qui redirige vers la classe SERVER.Misc.Criteria, et cette classe retourne justement cette cette "percent" classe, puisqu'ils sont connu grace à leur interface
        object Apply(object[] parameters);
    }
}
