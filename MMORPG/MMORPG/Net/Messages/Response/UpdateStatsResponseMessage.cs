using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MMORPG.Net.Messages.Response
{
    internal class UpdateStatsResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            // mis-à-jour un stat d'un joueur
            // commandStrings[1]= nomStat#valeur | séparé par pip
            foreach (string s in commandStrings[1].Split('|'))
            {
                FieldInfo statField = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).GetType().GetField(s.Split('#')[0], BindingFlags.Public | BindingFlags.Instance);
                statField.SetValue(CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor, Convert.ChangeType(s.Split('#')[1], statField.FieldType));

                HudHandle.UpdateHealth();
                // il faut mettre a jour les valeurs sur le hud, il faut utiliser les propirété, et lancer un update automatiquement lors d'une nouvelle assignation
                // il faut pas appeler HudHandle.UpdateHealth()si il s'agit d'autre parametre que maxHealth ou ou currentHealth
            }
        }
    }
}
