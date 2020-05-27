using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Summon
{
    static class MnemonicStatsDispatcher
    {
        public static string Str_Apply(string criteria, object OriginValue)
        {
            #region
            string value = "";
            string _OriginValue = (string)OriginValue;

            if (criteria.IndexOf('$') == -1)
                value = criteria;
            else
            {
                if (criteria.Substring(1, criteria.Length - 1).Split(':')[0] == "copy")
                    value = _OriginValue;
                else
                {
                    if (criteria.Substring(1, criteria.Length - 1).Split(':')[0] == "%")
                        value = ((Convert.ToInt32(_OriginValue) * Convert.ToInt32(criteria.Split(':')[1])) / 100).ToString();
                    else
                        value = "";
                }
            }

            return value;
            #endregion
        }

        public static int Int_Apply(string criteria, int OriginValue)
        {
            #region
            int value = 0;
            int _OriginValue = OriginValue;
            if (criteria == "")
                criteria = "0";

            if (criteria.IndexOf('$') == -1)
                value = Convert.ToInt32(criteria);
            else
            {
                if (criteria.Substring(1, criteria.Length - 1).Split(':')[0] == "copy")
                    value = Convert.ToInt32(_OriginValue);
                else
                {
                    if (criteria.Substring(1, criteria.Length - 1).Split(':')[0] == "%")
                        value = (Convert.ToInt32(_OriginValue) * Convert.ToInt32(criteria.Split(':')[1])) / 100;
                    else
                        value = 0;
                }
            }

            return value;
            #endregion
        }
    }
}
