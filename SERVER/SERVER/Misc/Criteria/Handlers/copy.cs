using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Misc.Criteria.Handlers
{
    class copy : Icriteria
    {
        public object Apply(object[] parameters)
        {
            int value = (int)parameters[0];
            string criteria = (string)parameters[1];            //$copy
            ///////////////////////////////////////
            int data = 0;

            if (criteria.IndexOf(RawData.criteria.copy) != -1)
                data = value;

            return data;
        }
    }
}
