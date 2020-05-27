using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPG.Net.Messages.Response
{
    internal class UpdateHealthResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region Mise à jours des valeurs de la vitalité
            // commandStrings[1] = maxHealth # maxHealth
            (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).maxHealth = Convert.ToInt16(commandStrings[1].Split('#')[0]);
            (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).currentHealth = Convert.ToInt16(commandStrings[1].Split('#')[1]);
            HudHandle.UpdateHealth();
            #endregion
        }
    }
}
