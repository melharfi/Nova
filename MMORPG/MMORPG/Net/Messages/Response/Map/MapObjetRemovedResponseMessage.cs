using MELHARFI;
using MELHARFI.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPG.Net.Messages.Response
{
    class MapObjetRemovedResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            // suprime un objet
            //commandStrings[1] = obj
            //commandStrings[2] = assoc
            if (commandStrings[1] == Enums.BattleType.Type.FreeChallenge.ToString())
            {
                // supression des boucliers qui represente le combat FreeChallenge
                Manager.manager.GfxObjList.RemoveAll(f => f.GetType() == typeof(Bmp) && f.Name() == "_MapDataObj_" + Enums.BattleType.Type.FreeChallenge && f.Tag().ToString().Split('#')[0] == commandStrings[2]);
            }
            #endregion
        }
    }
}
