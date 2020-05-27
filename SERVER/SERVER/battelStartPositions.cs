using System;
using System.Collections.Generic;
using SERVER.Enums;

namespace SERVER
{
	public static class battleStartPositions
	{
		// contiens les cases disponibles pour les joueurs lors du début d'un combat
		// généralement 10 cases pour chaques team sois 20 cases dans le joueur choisira

		public static string Map(string map, Battle battle)
		{
			if (map == "Start")
				return Start (battle);

			return "NaN";
		}

		public static string Start(Battle battle)
		{
            if(battle.BattleType == BattleType.Type.VsPnj)
            {
                if (battle.BattleFlags[0] == "iruka")
                {
                    if (battle.BattleFlags[1] == "FirstFight")
                    {
                        string sideAPositions = "14/7#16/5#18/7";
                        string sideBPositions = "14/10#16/12#18/10";
                        return sideAPositions + "|" + sideBPositions;
                    }
                    return "NaN";
                }
                return "NaN";
            }

		    string sideACommonPositions = "5/4#5/7#5/10#5/13#7/3#7/6#7/9#7/12#9/7#9/10";
		    string sideBCommonPositions = "21/4#21/7#21/10#21/13#19/3#19/6#19/9#19/12#17/6#17/9";
		    return sideACommonPositions + "|" + sideBCommonPositions;
		}
	}
}