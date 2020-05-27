using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Enums
{
    public class spell_effect_target
    {
        public enum targets
        {
            self,
            enemy_1,
            ally_1,
            none,
            ally_summon,
            enemy_summon,
            ally_all,
            enemy_all,
            all
        }
    }
}
