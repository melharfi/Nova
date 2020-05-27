using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Misc.Criteria.Handlers
{
    class other : Icriteria
    {
        public object Apply(object[] parameters)
        {
            // on returne l'objet lui même vus que le critère passé en valeur n'est pas une condition
            return parameters[0];            
        }
    }
}
