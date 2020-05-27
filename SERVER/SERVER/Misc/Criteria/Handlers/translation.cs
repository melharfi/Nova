using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Misc.Criteria.Handlers
{
    static class translation
    {
        // traduit des veleurs conditionnels comme $%:20, $copy ...
        public static Icriteria translate(string s)
        {
            if(s.IndexOf(RawData.criteria.percent) != -1)  //$%:20 pour 20%
            {
                // percent pour retourner que 20%
                Icriteria _percent = new percent();
                return _percent;
            }
            else if(s == RawData.criteria.copy)
            {
                // copy pour copier la valuer entiere
                Icriteria _copy = new copy();
                return _copy;
            }
            else
            {
                // return same value
                Icriteria _other = new other();
                return _other;
            }
        }
    }
}
