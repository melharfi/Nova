using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace SERVER.DataBase
{
    class DataTables
    {
        public static Database dataContext;                            // instance PetaPoco

        public static IEnumerable<mysql.bannedip> bannedip;
        public static IEnumerable<mysql.banneduser> banneduser;
        public static IEnumerable<mysql.classes> classes;              // pas besoin dans le system
        public static IEnumerable<mysql.connected> connected;
        public static IEnumerable<mysql.logcounter> logcounter;
        public static IEnumerable<mysql.logerror> logerror;
        public static IEnumerable<mysql.mapobj> mapobj;
        public static IEnumerable<mysql.npc> npc;
        public static IEnumerable<mysql.npcaction> npcaction;
        public static IEnumerable<mysql.npcspawn> npcspawn;
        public static IEnumerable<mysql.paramsbkp> paramsbkp;
        public static IEnumerable<mysql.players> players;
        public static IEnumerable<mysql.quete> quete;
        public static IEnumerable<mysql.state> state;
        public static IEnumerable<mysql.users> users;
        public static IEnumerable<mysql.version> version;
        public static IEnumerable<mysql.xplevel> xplevel;
        public static IEnumerable<mysql.spells> spells;
        public static IEnumerable<mysql.effects> effects;
        public static IEnumerable<mysql.summon> summon;

        delegate List<mysql.bannedip> opo();

        public static void IniPetaPoco()
        {
            dataContext = new PetaPoco.Database("server=localhost;user id=root;database=MMORPG;pooling=False", "MySql");

            bannedip = dataContext.Fetch<mysql.bannedip>("SELECT * FROM bannedip");
            banneduser = dataContext.Fetch<mysql.banneduser>("SELECT * FROM banneduser");
            classes = dataContext.Fetch<mysql.classes>("SELECT * FROM classes");
            connected = dataContext.Fetch<mysql.connected>("SELECT * FROM connected");
            logcounter = dataContext.Fetch<mysql.logcounter>("SELECT * FROM logcounter");
            logerror = dataContext.Fetch<mysql.logerror>("SELECT * FROM logerror");
            mapobj = dataContext.Fetch<mysql.mapobj>("SELECT * FROM mapobj");
            npc = dataContext.Fetch<mysql.npc>("SELECT * FROM npc");
            npcaction = dataContext.Fetch<mysql.npcaction>("SELECT * FROM npcaction");
            npcspawn = dataContext.Fetch<mysql.npcspawn>("SELECT * FROM npcspawn");
            paramsbkp = dataContext.Fetch<mysql.paramsbkp>("SELECT * FROM paramsbkp");
            players = dataContext.Fetch<mysql.players>("SELECT * FROM players");
            quete = dataContext.Fetch<mysql.quete>("SELECT * FROM quete");
            state = dataContext.Fetch<mysql.state>("SELECT * FROM state");
            users = dataContext.Fetch<mysql.users>("SELECT * FROM users");
            version = dataContext.Fetch<mysql.version>("SELECT * FROM version");
            xplevel = dataContext.Fetch<mysql.xplevel>("SELECT * FROM xplevel");
            spells = dataContext.Fetch<mysql.spells>("SELECT * FROM spells");
            effects = dataContext.Fetch<mysql.effects>("SELECT * FROM effects");
            summon = dataContext.Fetch<mysql.summon>("SELECT * FROM summon");
        }
        public static void Update()
        {
            foreach (var a in bannedip)
                dataContext.Update("bannedip", "id", a);
            Console.WriteLine("Update BDD 6.6 %");
            foreach (var a in banneduser)
                dataContext.Update("banneduser", "id", a);
            Console.WriteLine("Update BDD 13.3 %");
            foreach (var a in connected)
                dataContext.Update("connected", "id", a);
            Console.WriteLine("Update BDD 20 %");
            foreach (var a in logcounter)
                dataContext.Update("logcounter", "id", a);
            Console.WriteLine("Update BDD 26.6 %");
            foreach (var a in logerror)
                dataContext.Update("logerror", "id", a);
            Console.WriteLine("Update BDD 33.3 %");
            foreach (var a in mapobj)
                dataContext.Update("mapobj", "id", a);
            Console.WriteLine("Update BDD 46,2 %");
            foreach (var a in npc)
                dataContext.Update("npc", "id", a);
            Console.WriteLine("Update BDD 40 %");
            foreach (var a in npcaction)
                dataContext.Update("npcaction", "id", a);
            Console.WriteLine("Update BDD 46.7 %");
            foreach (var a in npcspawn)
                dataContext.Update("npcspawn", "id", a);
            Console.WriteLine("Update BDD 53.3 %");
            foreach (var a in players)
                dataContext.Update("players", "id", a);
            Console.WriteLine("Update BDD 60 %");
            foreach (var a in quete)
                dataContext.Update("quete", "id", a);
            Console.WriteLine("Update BDD 66.7 %");
            foreach (var a in state)
                dataContext.Update("state", "id", a);
            Console.WriteLine("Update BDD 73.37 %");
            foreach (var a in users)
                dataContext.Update("users", "id", a);
            Console.WriteLine("Update BDD 80 %");

            foreach (var a in xplevel)
                dataContext.Update("xplevel", "id", a);
            Console.WriteLine("Update BDD 86.7 %");

            foreach (var a in spells)
                dataContext.Update("spells", "id", a);
            Console.WriteLine("Update BDD 100 %");
        }
    }
}
