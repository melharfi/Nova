using System;
using System.Collections.Generic;
using Lidgren.Network;
using SERVER.Enums;

namespace SERVER
{
	public delegate bool isFreeCellToWalkDel(Point p);			// delegate qui pointe vers la methode isFreeCell du map encours
	public delegate bool isFreeCellToSpellDel(Point p);			// delegate qui pointe vers la methode isFreeCellToSpell du map encours
	public class Actor : ICloneable
	{
		public object Clone()
		{
			return this.MemberwiseClone();
        }
        // contiens les info concernant les joueurs
        // si un champ a été ajouté, il faut l'ajouter sur la classe SERVER.Summon.StatsDispatcher qui recois les stats d'une invocation
	    public NetConnection Nc;
        public bool SignedIn = false;
        public Enums.battleState.state BattleState = Enums.battleState.state.idle;			// idle ou busy(en combat ou autre))
        public List<Enums.Buff.Name> BuffState = new List<Enums.Buff.Name>();         // état du joueur comme mode senin de naruto
		public string Username = "";//
        public string Pseudo = "";//

        public long SubstituteUid = 0;                // si c'est different de 0 c'est que un autre utilisateur essai de prendre le controle du joueur, et pour éviter q'un 3eme joueur essai lui aussi de prendre le controle on dois bloquer tous les autres instances de connexion tant que la 1ere personne qui a fait la requête na pas términé sa tache, cette valeur égale au token du joueur qui veux prendre le controle de la session

        #region states
        public int level = 0;//
		public int directionLook = 0;
		public Enums.Spirit.Name spirit = Enums.Spirit.Name.neutral;//
        public int spiritLvl = 0;//
        public int classeId = 0;//
        public Enums.ActorClass.ClassName classeName = Enums.ActorClass.ClassName.__notAffected;
        public Enums.ActorSexe.Sexe sexe = Enums.ActorSexe.Sexe.__notAffected;
        public bool Pvp = false;//
		public int Timestamp = 0;//
		public Point map_position = Point.Zero();//
		public string map = "";//
		public Enums.AnimatedActions.Name animatedAction = Enums.AnimatedActions.Name.idle;
		public List<Point> wayPoint = new List<Point>();//
		public int wayPointTimeStamp = 0;//
		public int wayPointCnt = 0;//		// contiens le combre de tuiles dans la wayoint, ceci est necessaire parsque le wayPoint se decremante apres chaque point
		public string YouChallengePlayer = string.Empty;//		// contiens le nom du joueur que vous défier
		public string PlayerChallengeYou = string.Empty;//		// contiens le nom du joueur qui vous défie
		public List<string> IgnoredPlayersChallenge = new List<string>();//
		public int inBattle = 0;//
		public int idBattle = -1;//			//contient l'id de l'enregistrement dans la table Battle
		public int xp = 0;//
		public string maskColorString = "";//
		public string hiddenVillage = "";//
		public Enums.Team.Side teamSide;//					// contiens la team dons il fait partie, sois team1 ou team2, pour facilité la recherche
		public int initiative = 0;
		public int doton = 0;//
		public int katon = 0;//
		public int futon = 0;//
		public int raiton = 0;//
		public int suiton = 0;//
        public int power = 0;
		public int usingDoton = 0;//
		public int usingKaton = 0;//
		public int usingFuton = 0;//
		public int usingRaiton = 0;//
		public int usingSuiton = 0;//
		public int equipedDoton = 0;//
		public int equipedKaton = 0;//
		public int equipedFuton = 0;//
		public int equipedRaiton = 0;//
		public int equipedSuiton = 0;//
        public int equipedPower = 0;
		public int originalPc = 0;//
		public int currentPc = 0;//
		public int originalPm = 0;//
		public int currentPm = 0;//
		public int pe = 0;//
		public int cd = 0;//
		public int summons = 0;//
		public int maxHealth = 0;//
		public int currentHealth = 0;//
		public Enums.Rang.official officialRang = Enums.Rang.official.neutral;//
		public string job1, job2, specialty1, specialty2;
		public int maxWeight = 0, currentWeight = 0, ryo = 0;
		public Species.Name species = Species.Name.Human;
		public int resiDotonPercent = 0;
		public int resiKatonPercent = 0;
		public int resiFutonPercent = 0;
		public int resiRaitonPercent = 0;
		public int resiSuitonPercent = 0;
		public int resiDotonFix = 0;
		public int resiKatonFix = 0;
		public int resiFutonFix = 0;
		public int resiRaitonFix = 0;
		public int resiSuitonFix = 0;
		public int resiFix = 0;
		public int dodgePC = 0;
		public int dodgePM = 0;
		public int dodgePE = 0;
		public int dodgeCD = 0;
		public int removePC = 0;
		public int removePM = 0;
		public int removePE = 0;
		public int removeCD = 0;
		public int escape = 0;
		public int blocage = 0;
		public bool visible = true;
		public List<SpellsInformations> sorts = new List<SpellsInformations>();
		public int domDotonFix = 0;
		public int domKatonFix = 0;
		public int domFutonFix = 0;
		public int domRaitonFix = 0;
		public int domSuitonFix = 0;
		public int domFix = 0;
        public int spellPointLeft = 0;
		public bool isAlive= true;
		public string owner = "";								// si c'est une invocation, ce variable contien le nom de l'invocateur
		public int timeBeforeNextWaypoint = 0;					// pour eviter un spam de waypoint
        public int dotonChakraLevel = 0;                        // comment ces variable s'affecte ils
        public int katonChakraLevel = 0;
        public int futonChakraLevel = 0;
        public int raitonChakraLevel = 0;
        public int suitonChakraLevel = 0;
        public int summonID = 0;                                // pour memoriser le id du type de l'invocation qui est soustrait du champ flag1
        #endregion

        public isFreeCellToWalkDel IsFreeCellToWalk;
		public isFreeCellToSpellDel IsFreeCellToSpell;
		public List<Buff> BuffsList = new List<Buff> ();         // List des envoutements
        public class Buff
		{
			// contient l'envoutement d'un sort sur le joueur
			// cette classe est instancié dans et intégré dans la liste EnvoutementsList
			public string title;
			public int SortID;
			public int relanceInterval;                     // grace a ce variable qu'on va determiner si le sort poura être lancer le tour prochain, chaque tour on decremente ce variable, si = 0 on supprime l'instance
			//public int relanceParJoueur;
            public List<string> playerRoxed = new List<string>();
			public int relanceParTour;                      // maximum d'utilisation du sort pondant le tour actuel
		    public bool Debuffable;
			public bool VisibleToPlayers;                   // si l'envoutement dois être visible chez le client ou pas
            public Enums.Buff.State BuffState;        // pour determiner si l'envoutement dois être évaluer au début du tour ou à la fin
            public bool system;                            // si true, l'envoutement est un envoutement qui n'est pas visible au joueur et utilisé pour l'affichage de l'accessibilité du sort et / ou l'interdiction de relance ..., si false c'est un sort visible
            public Actor Bonus = new Actor();        // quand un bonus s'ajout au stats de joueur quand c'est positif ou se retire quand c negatif
            public int Duration;                      // s'il s'agit d'un bonus ou malus, ce variable compte combien de tour ceci est appliqué
            public bool Cd = false;                         // pour voir si le sort a été lancé en Cd ou pas, lors de l'annulement de l'envoutement, les bonnus d'un sort peuvent changé en étant en CD ou pas
            public List<Enums.Buff.Name> StateList = new List<Enums.Buff.Name>();          // contiens les états du joueurs, comme Ermite
            public string Player;                           // contien le nom du joueur qui a l'envoutement
        }
		public void SessionZero()
		{
			Pseudo = string.Empty;
			level = 0;
			directionLook = 0;
			spirit = Enums.Spirit.Name.neutral;
			spiritLvl = 0;
			classeId = 0;
			classeName = Enums.ActorClass.ClassName.__notAffected;
			sexe = Enums.ActorSexe.Sexe.__notAffected;
			Pvp = false;
			map_position = Point.Zero();
			map = string.Empty;
			animatedAction = Enums.AnimatedActions.Name.idle;
			wayPoint.Clear();
			wayPointTimeStamp = 0;
			wayPointCnt = 0;
		}
        public List<QuestInformation> Quests = new List<QuestInformation>();                         // list des quetes réalisées
        public void Disconnect(string message)
        {
            Nc.Disconnect(message);
        }
        public void Disconnect(DisconnectReason.disconnectReason reason)
        {
            Nc.Disconnect(reason.ToString());
        }
        public class QuestInformation
        {
            public string QuestName;
            public int MaxSteps;
            public int CurrentStep;
            public bool Submited;
        }
        public class SpellsInformations
		{
			// contien les infos tirés de la bdd pour chaque sort
			public int SpellPlace;
			public int SpellId;
			public int SpellColor;
			public int Level;
            public List<effects> effect = new List<effects>();
		}
        /*public enum Species
        {
            // contien les different type de joueur, pour faire la differance entre une invoc est un joueur et un pnj ...
            // utilisé dans la classe PlayerInfo.genre
            // humain = un utilisateur réel
            // pnj = pnj qui agi comme un humain concernant les interactions, comme dialogue, en combat il vérou sa position, il est pris en charge dans les controles comme celui qui vérifie si tous les joueurs ont vérouillés leurs position pour lancer le combat, seul les invocs ne sont pas compté puisqu'ils ne doivents pas être exister
            // invoc = une invocation d'un joueur, qui n'est pas pris en charge par les contrôles de validations
            humain, pnj, summon
        }*/
        public class effects
        {
            public Effect.effects_base base_effect;
            public int effect_id;
            public List<spell_effect_target.targets> targets = new List<spell_effect_target.targets>();  // cible
            public Chakra.Element element;
            public int duration;       // combien de tour l'effet reste
            public int delay;          // interval de lancer, si = 0 l'effet se lance sur le champ, si = 1, le tour procain
            //public string zoneShape;    // zone d'effet
            public int zoneSize;       // etendue de l'effet
            public bool zoneExtensible;     // determine si la zone est extensible par la po
            public int handToHandDistance;
            public int min;             // valeur minimal
            public int max;             // valeur maximal
            public string flag1, flag2, flag3;      // pour des variable suplementaires
        }
	}
}

