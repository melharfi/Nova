using MELHARFI.Gfx;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MMORPG
{
    public class Actor : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        // contiens les infos concernant les joueurs
        public Enums.Team.Side teamSide;//
        public string pseudo = "";
        public Enums.Spirit.Name spirit;
        public int spiritLevel;
        public int level;
        public Enums.ActorClass.ClassName className = Enums.ActorClass.ClassName.__notAffected;
        public bool pvpEnabled;
        public Enums.HiddenVillage.Names hiddenVillage;
        public List<Point> wayPoint = new List<Point>();
        public Enums.AnimatedActions.Name animatedAction = Enums.AnimatedActions.Name.idle;
        public int directionLook;
        public Rec chatBulleRec;                           // pointeur vers l'image de la derniere ChatBulle
        public Enums.Rang.official officialRang;
        public Enums.Rang.special specialRang;              // il faut envoyer cette information lorsque le joueur se connecte
        public int maxXp;
        public int currentXp;
        public int currentHealth = 0;
        public int maxHealth = 0;
        public int currentWeight = 0;
        public int maxWeight = 0;
        public Enums.ActorSexe.Sexe sexe = Enums.ActorSexe.Sexe.__notAffected;
        public int ryo = 0;
        public string map, job1, job2, specialty1, specialty2;
        public int doton, katon, futon, raiton, suiton, usingDoton, usingKaton, usingFuton, usingRaiton, usingSuiton, equipedDoton,equipedKaton, equipedFuton, equipedRaiton, equipedSuiton, equipedPower, originalPc, originalPm, pe, cd, summons, initiative, currentPc, currentPm;
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
        public int dodgePc = 0;
        public int dodgePm = 0;
        public int dodgePe = 0;
        public int dodgeCd = 0;
        public int removePc = 0;
        public int removePm = 0;
        public int removePe = 0;
        public int removeCd = 0;
        public int escape = 0;
        public int blocage = 0;
        public int domDotonFix = 0;
        public int domKatonFix = 0;
        public int domFutonFix = 0;
        public int domRaitonFix = 0;
        public int domSuitonFix = 0;
        public int domFix = 0;
        public int power = 0;
        public int spellPointLeft = 0;
        public Enums.Species.Name species = Enums.Species.Name.Human;      // contien le type du joueur, sois humain, pnj, invoc
        public int timeBeforeNextWaypoint = 0;					// pour eviter un spam de waypoint
        public Point realPosition;
        public List<SpellsInformations> spells = new List<SpellsInformations>();           // contien une nouvelle instance de la class infos_sorts
        public string maskColorString;
        public int dotonChakraLevel = 0;
        public int katonChakraLevel = 0;
        public int futonChakraLevel = 0;
        public int raitonChakraLevel = 0;
        public int suitonChakraLevel = 0;
        public Bmp iconeAvatar;                 // elle contiens l'icone qui affiche le joueur parmis la liste des combatant adroite, pour un accée facile
        public Bmp ibPlayer;                    // pointeur vers l'image du joueur
        public bool visible = true;             // si le joueur est invisible ou pas
        public bool isAlive = true;
        public string Event = "";           // contiens des infos comme inBattle si le joueur viens de co et qu'il été déja en combat avant
        public List<Buff> BuffsList = new List<Buff>();          // List des envoutements
        public class Buff
        {
            // contient l'envoutement d'un sort sur le joueur
            // cette classe est instancié dans et intégré dans la liste EnvoutementsList
            public string title;
            public int SortID;
            public int relanceInterval;                     // grace a ce variable qu'on va determiner si le sort poura être lancer le tour prochain, chaque tour on decremente ce variable, si = 0 on supprime l'instance
            public List<string> playerRoxed = new List<string>();
            public int relanceParTour;                      // maximum d'utilisation du sort pondant le tour actuel
            public bool Debuffable;
            public bool visibleToPlayers;                   // si l'envoutement dois être visible chez le client ou pas
            public Enums.Buff.State BuffState;        // pour determiner si l'envoutement dois être évaluer au début du tour ou à la fin
            public bool systeme;                             // si true, l'envoutement est un envoutement qui n'est pas visible aujoueur et utilisé pour l'affichage de l'accessibilité du sort et / ou l'interdiction de relance ..., si false c'est un sort visible
            public Actor Bonus = new Actor();        // // quand un bonus s'ajout au stats de joueur quand c'est positif ou se retire quand c negatif
            public int BonusRoundLeft;                      // s'il s'agit d'un bonus ou malus, ce variable compte combien de tour ceci est appliqué
            public bool Cd = false;                         // pour voir si le sort a été lancé en Cd ou pas, lors de l'annulement de l'envoutement, les bonnus d'un sort peuvent changé en étant en CD ou pas
            public List<Enums.Buff.Name> StateList = new List<Enums.Buff.Name>();          // contiens les états du joueurs, comme Ermite
            public string player;                           // contien le nom du joueur qui a l'envoutement
        }
        public void AddPlayer(Enums.Team.Side side, string _pseudo, Enums.ActorClass.ClassName _className, int _level, Enums.HiddenVillage.Names _hiddenVillage, string _maskColors, int _maxHealth, int _currentHealth, Enums.Rang.official _rang)
        {
            this.teamSide = side;
            this.pseudo = _pseudo;
            className = _className;
            this.level = _level;
            hiddenVillage = _hiddenVillage;
            this.maskColorString = _maskColors;
            maxHealth = _maxHealth;
            this.currentHealth = _currentHealth;
            this.officialRang = _rang;
        }
        public List<QuestInformations> Quests = new List<QuestInformations>();                         // list des quetes réalisées
        public class QuestInformations
        {
            public string nom_quete;
            public int totalSteps;
            public int currentStep;
            public bool submited;
        }
        public Actor()
        {
        }
        public Actor(string _pseudo, int _level, Enums.Spirit.Name _spirit, Enums.ActorClass.ClassName _className, bool _pvp, int _spiritLvl, Enums.HiddenVillage.Names _hiddenVillage, string _maskColor, int _orientation, Enums.AnimatedActions.Name _action, string _waypoint)
        {
            this.pseudo = _pseudo;
            this.spirit = _spirit;
            this.spiritLevel = _spiritLvl;
            this.level = _level;
            className = _className;
            pvpEnabled = _pvp;
            hiddenVillage = _hiddenVillage;
            maskColorString = _maskColor;
            this.directionLook = _orientation;
            this.animatedAction = _action;

            // seulement quand le joueur est en mouvement alors qu'on viens de se connecter
            if (_waypoint != "null" && _waypoint != "")
            {
                string[] tmpWayPoint = _waypoint.Split(':');
                foreach (string s in tmpWayPoint)
                    wayPoint.Add(new Point(Convert.ToInt32(s.Split(',')[0]), Convert.ToInt32(s.Split(',')[1])));
            }
        }
        public class SpellsInformations
        {
            // conties les infos tiré de la bdd pour chaque sort
            public int emplacement;
            public int sortID;
            public int colorSort;
            public int level;
        }
    }
}
