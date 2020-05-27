using System.Collections.Generic;

namespace SERVER
{
	public class Battle
	{
		// contiens les données du combat
		public static List<Battle> Battles = new List<Battle>();			// contiens tous les sessions de combat
		public static int BattleCounter;								// contiens le nombre de combats
		public int IdBattle;
		public List<Actor> SideA = new List<Actor> ();
		public List<Actor> SideB = new List<Actor> ();
		public List<Actor> AllPlayersByOrder = new List<Actor> ();
		public List<Actor> DeadPlayers = new List<Actor> ();			// contiens la liste des joueurs mort dans un combat pour les reinvoquer peux etre ou pour le bonus fin de combat
		public int Timestamp;
        public Enums.battleState.state State;
		public string Owner;
		public string Map;
		public isFreeCellToSpellDel IsFreeCellToSpell;
		public isFreeCellToWalkDel IsFreeCellToWalk;
		public Enums.BattleType.Type BattleType;
	    public string[] BattleFlags = new string[3];        // contiens des inforrmations specifique a certains type de combat comme VsPnj dons on doit enregistrer le nom du pnj dans un flag et le nom de la quete
		public int Turn = 0;
		public int TimeLeftToPlay = 0;						// contien le temps quand le joueur a eu la main pour verifier quand passer a l'autre joueur
        public List<string> LockedPosInIniTime = new List<string>();        // contient la liste des joueurs qui ont vérouillé leurs position en combat en état initialisation
        public string SideAValidePos = "";                  // contien les position de la phase de preparation, normalement il y a une classe qui retourn ses pos sur battleStartPositions.Map(string map) mais des fois on aimerai changer les positions offert par le map pour par ex laisse q'une seul cas comme lort d'un combat avec un PNJ "Iruka"
        public string SideBValidePos = "";                  // contien les position de la phase de preparation, normalement il y a une classe qui retourn ses pos sur battleStartPositions.Map(string map) mais des fois on aimerai changer les positions offert par le map pour par ex laisse q'une seul cas comme lort d'un combat avec un PNJ "Iruka"
        public Battle ()
		{
			IdBattle = ++BattleCounter;
			State = Enums.battleState.state.initialisation;
			Timestamp = CommonCode.ReturnTimeStamp ();
		}
	}
}
