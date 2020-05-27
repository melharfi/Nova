using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Misc.Criteria.Handlers
{
    class percent : Icriteria
    {
        public object Apply(object[] parameters)
        {
            int value = (int)parameters[0];
            string criteria = (string)parameters[1];            //$%:20
            ///////////////////////////////////////
            int data = 0;

            if (criteria.IndexOf(RawData.criteria.percent) != -1)
            {
                int extractedPercent;
                if (int.TryParse(criteria.Split(':')[1], out extractedPercent))
                    data = (value * extractedPercent) / 100;
            }

            return data;
        }
    }
}
