



















// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `mysql`
//     Provider:               `MySql.Data.MySqlClient`
//     Connection String:      `server=localhost;user id=root;database=MMORPG;pooling=False`
//     Schema:                 ``
//     Include Views:          `False`



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace mysql
{

	public partial class mysqlDB : Database
	{
		public mysqlDB() 
			: base("mysql")
		{
			CommonConstruct();
		}

		public mysqlDB(string connectionStringName) 
			: base(connectionStringName)
		{
			CommonConstruct();
		}
		
		partial void CommonConstruct();
		
		public interface IFactory
		{
			mysqlDB GetInstance();
		}
		
		public static IFactory Factory { get; set; }
        public static mysqlDB GetInstance()
        {
			if (_instance!=null)
				return _instance;
				
			if (Factory!=null)
				return Factory.GetInstance();
			else
				return new mysqlDB();
        }

		[ThreadStatic] static mysqlDB _instance;
		
		public override void OnBeginTransaction()
		{
			if (_instance==null)
				_instance=this;
		}
		
		public override void OnEndTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
        

		public class Record<T> where T:new()
		{
			public static mysqlDB repo { get { return mysqlDB.GetInstance(); } }
			public bool IsNew() { return repo.IsNew(this); }
			public object Insert() { return repo.Insert(this); }

			public void Save() { repo.Save(this); }
			public int Update() { return repo.Update(this); }

			public int Update(IEnumerable<string> columns) { return repo.Update(this, columns); }
			public static int Update(string sql, params object[] args) { return repo.Update<T>(sql, args); }
			public static int Update(Sql sql) { return repo.Update<T>(sql); }
			public int Delete() { return repo.Delete(this); }
			public static int Delete(string sql, params object[] args) { return repo.Delete<T>(sql, args); }
			public static int Delete(Sql sql) { return repo.Delete<T>(sql); }
			public static int Delete(object primaryKey) { return repo.Delete<T>(primaryKey); }
			public static bool Exists(object primaryKey) { return repo.Exists<T>(primaryKey); }
			public static bool Exists(string sql, params object[] args) { return repo.Exists<T>(sql, args); }
			public static T SingleOrDefault(object primaryKey) { return repo.SingleOrDefault<T>(primaryKey); }
			public static T SingleOrDefault(string sql, params object[] args) { return repo.SingleOrDefault<T>(sql, args); }
			public static T SingleOrDefault(Sql sql) { return repo.SingleOrDefault<T>(sql); }
			public static T FirstOrDefault(string sql, params object[] args) { return repo.FirstOrDefault<T>(sql, args); }
			public static T FirstOrDefault(Sql sql) { return repo.FirstOrDefault<T>(sql); }
			public static T Single(object primaryKey) { return repo.Single<T>(primaryKey); }
			public static T Single(string sql, params object[] args) { return repo.Single<T>(sql, args); }
			public static T Single(Sql sql) { return repo.Single<T>(sql); }
			public static T First(string sql, params object[] args) { return repo.First<T>(sql, args); }
			public static T First(Sql sql) { return repo.First<T>(sql); }
			public static List<T> Fetch(string sql, params object[] args) { return repo.Fetch<T>(sql, args); }
			public static List<T> Fetch(Sql sql) { return repo.Fetch<T>(sql); }
			public static List<T> Fetch(long page, long itemsPerPage, string sql, params object[] args) { return repo.Fetch<T>(page, itemsPerPage, sql, args); }
			public static List<T> Fetch(long page, long itemsPerPage, Sql sql) { return repo.Fetch<T>(page, itemsPerPage, sql); }
			public static List<T> SkipTake(long skip, long take, string sql, params object[] args) { return repo.SkipTake<T>(skip, take, sql, args); }
			public static List<T> SkipTake(long skip, long take, Sql sql) { return repo.SkipTake<T>(skip, take, sql); }
			public static Page<T> Page(long page, long itemsPerPage, string sql, params object[] args) { return repo.Page<T>(page, itemsPerPage, sql, args); }
			public static Page<T> Page(long page, long itemsPerPage, Sql sql) { return repo.Page<T>(page, itemsPerPage, sql); }
			public static IEnumerable<T> Query(string sql, params object[] args) { return repo.Query<T>(sql, args); }
			public static IEnumerable<T> Query(Sql sql) { return repo.Query<T>(sql); }

		}

	}
	



    

	[TableName("mmorpg.bannedip")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class bannedip : mysqlDB.Record<bannedip>  
    {



		[Column] public int id { get; set; }





		[Column] public string ip { get; set; }





		[Column] public string reson { get; set; }





		[Column] public int censure { get; set; }



	}

    

	[TableName("mmorpg.banneduser")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class banneduser : mysqlDB.Record<banneduser>  
    {



		[Column] public int id { get; set; }





		[Column] public string user { get; set; }





		[Column] public string reson { get; set; }





		[Column] public int temps { get; set; }



	}

    

	[TableName("mmorpg.classes")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class classes : mysqlDB.Record<classes>  
    {



		[Column] public int id { get; set; }





		[Column] public int classeId { get; set; }





		[Column] public string classeName { get; set; }



	}

    

	[TableName("mmorpg.connected")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class connected : mysqlDB.Record<connected>  
    {



		[Column] public int id { get; set; }





		[Column] public string ip { get; set; }





		[Column] public string user { get; set; }





		[Column] public string pseudo { get; set; }





		[Column] public string uid { get; set; }





		[Column] public string date { get; set; }





		[Column] public int timestamp { get; set; }





		[Column] public string map { get; set; }





		[Column] public string map_position { get; set; }



	}

    

	[TableName("mmorpg.effects")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class effects : mysqlDB.Record<effects>  
    {



		[Column] public int id { get; set; }





		[Column] public int effectID { get; set; }





		[Column] public string type { get; set; }



	}

    

	[TableName("mmorpg.logcounter")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class logcounter : mysqlDB.Record<logcounter>  
    {



		[Column] public int id { get; set; }





		[Column] public string user { get; set; }





		[Column] public int timestamp { get; set; }



	}

    

	[TableName("mmorpg.logerror")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class logerror : mysqlDB.Record<logerror>  
    {



		[Column] public int id { get; set; }





		[Column] public string error { get; set; }





		[Column] public string date { get; set; }





		[Column] public string user { get; set; }





		[Column] public string ip { get; set; }





		[Column] public string detail { get; set; }



	}

    

	[TableName("mmorpg.mapobj")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class mapobj : mysqlDB.Record<mapobj>  
    {



		[Column] public int id { get; set; }





		[Column] public string map { get; set; }





		[Column] public string map_position { get; set; }





		[Column] public string state { get; set; }





		[Column] public string obj { get; set; }





		[Column] public string assoc { get; set; }





		[Column] public int idBattle { get; set; }



	}

    

	[TableName("mmorpg.npc")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class npc : mysqlDB.Record<npc>  
    {



		[Column] public int id { get; set; }





		[Column] public int npcId { get; set; }





		[Column] public string npcName { get; set; }



	}

    

	[TableName("mmorpg.npcaction")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class npcaction : mysqlDB.Record<npcaction>  
    {



		[Column] public int id { get; set; }





		[Column] public int? npcID { get; set; }





		[Column] public int action { get; set; }



	}

    

	[TableName("mmorpg.npcspawn")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class npcspawn : mysqlDB.Record<npcspawn>  
    {



		[Column] public int id { get; set; }





		[Column] public int npcID { get; set; }





		[Column] public string npcName { get; set; }





		[Column] public string map { get; set; }





		[Column] public int posX { get; set; }





		[Column] public int posY { get; set; }





		[Column] public string typeDialog { get; set; }





		[Column] public string comment { get; set; }



	}

    

	[TableName("mmorpg.paramsbkp")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class paramsbkp : mysqlDB.Record<paramsbkp>  
    {



		[Column] public int id { get; set; }





		[Column] public int Port { get; set; }





		[Column] public int MaxConnexions { get; set; }





		[Column] public int MaxTimeAfk { get; set; }



	}

    

	[TableName("mmorpg.players")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class players : mysqlDB.Record<players>  
    {



		[Column] public int id { get; set; }





		[Column] public string pseudo { get; set; }





		[Column] public string user { get; set; }





		[Column] public string spirit { get; set; }





		[Column] public int spiritLvl { get; set; }





		[Column] public sbyte pvp { get; set; }





		[Column] public string classe { get; set; }





		[Column] public string map { get; set; }





		[Column] public string map_position { get; set; }





		[Column] public sbyte orientation { get; set; }





		[Column] public string size { get; set; }





		[Column] public int level { get; set; }





		[Column] public string sexe { get; set; }





		[Column] public string village { get; set; }





		[Column] public string MaskColors { get; set; }





		[Column] public int TotalPdv { get; set; }





		[Column] public int CurrentPdv { get; set; }





		[Column] public int inBattle { get; set; }





		[Column] public string inBattleType { get; set; }





		[Column] public int inBattleID { get; set; }





		[Column] public int rang { get; set; }





		[Column] public int xp { get; set; }





		[Column] public int doton { get; set; }





		[Column] public int katon { get; set; }





		[Column] public int futon { get; set; }





		[Column] public int raiton { get; set; }





		[Column] public int suiton { get; set; }





		[Column] public int puissance { get; set; }





		[Column] public int usingDoton { get; set; }





		[Column] public int usingKaton { get; set; }





		[Column] public int usingFuton { get; set; }





		[Column] public int usingRaiton { get; set; }





		[Column] public int usingSuiton { get; set; }





		[Column] public int dotonEquiped { get; set; }





		[Column] public int katonEquiped { get; set; }





		[Column] public int futonEquiped { get; set; }





		[Column] public int raitonEquiped { get; set; }





		[Column] public int suitonEquiped { get; set; }





		[Column] public int puissanceEquiped { get; set; }





		[Column] public int pc { get; set; }





		[Column] public int pm { get; set; }





		[Column] public int pe { get; set; }





		[Column] public int cd { get; set; }





		[Column] public int invoc { get; set; }





		[Column] public int initiative { get; set; }





		[Column] public string job1 { get; set; }





		[Column] public string job2 { get; set; }





		[Column] public string specialite1 { get; set; }





		[Column] public string specialite2 { get; set; }





		[Column] public int TotalPoid { get; set; }





		[Column] public int CurrentPoid { get; set; }





		[Column] public int Ryo { get; set; }





		[Column] public int resiDoton { get; set; }





		[Column] public int resiKaton { get; set; }





		[Column] public int resiFuton { get; set; }





		[Column] public int resiRaiton { get; set; }





		[Column] public int resiSuiton { get; set; }





		[Column] public int resiDotonFix { get; set; }





		[Column] public int resiKatonFix { get; set; }





		[Column] public int resiFutonFix { get; set; }





		[Column] public int resiRaitonFix { get; set; }





		[Column] public int resiSuitonFix { get; set; }





		[Column] public int resiFix { get; set; }





		[Column] public int esquivePC { get; set; }





		[Column] public int esquivePM { get; set; }





		[Column] public int esquivePE { get; set; }





		[Column] public int esquiveCD { get; set; }





		[Column] public int retraitPC { get; set; }





		[Column] public int retraitPM { get; set; }





		[Column] public int retraitPE { get; set; }





		[Column] public int retraitCD { get; set; }





		[Column] public int evasion { get; set; }





		[Column] public int blocage { get; set; }





		[Column] public string sorts { get; set; }





		[Column] public int domDotonFix { get; set; }





		[Column] public int domKatonFix { get; set; }





		[Column] public int domFutonFix { get; set; }





		[Column] public int domRaitonFix { get; set; }





		[Column] public int domSuitonFix { get; set; }





		[Column] public int domFix { get; set; }





		[Column] public int spellPointLeft { get; set; }



	}

    

	[TableName("mmorpg.quete")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class quete : mysqlDB.Record<quete>  
    {



		[Column] public int id { get; set; }





		[Column] public string nom_quete { get; set; }





		[Column] public int totalSteps { get; set; }





		[Column] public int currentStep { get; set; }





		[Column] public sbyte submited { get; set; }





		[Column] public string pseudo { get; set; }



	}

    

	[TableName("mmorpg.spells")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class spells : mysqlDB.Record<spells>  
    {



		[Column] public int id { get; set; }





		[Column] public int spellID { get; set; }





		//[Column] public string effects { get; set; }





		[Column] public string spellName { get; set; }





		[Column] public int level { get; set; }





		[Column] public int pc { get; set; }





		[Column] public int pe { get; set; }





		[Column] public string technique { get; set; }





		[Column] public string rang { get; set; }





		[Column] public sbyte peModifiable { get; set; }





		[Column] public sbyte ligneDeVue { get; set; }





		[Column] public int minValue { get; set; }





		[Column] public int maxValue { get; set; }





		[Column] public int cd { get; set; }





		[Column] public int relanceInterval { get; set; }





		[Column] public int relanceParJoueur { get; set; }





		[Column] public int relanceParTour { get; set; }





		[Column] public int cdDomBonnus { get; set; }





		[Column] public string element { get; set; }





		[Column] public int distanceFromMelee { get; set; }





		[Column] public string owner { get; set; }





		[Column] public string target { get; set; }





		[Column] public string typeEffect { get; set; }





		[Column] public string zoneEffect { get; set; }





		[Column] public int? sizeEffect { get; set; }





		[Column] public string extraDataEffect { get; set; }



	}

    

	[TableName("mmorpg.state")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class state : mysqlDB.Record<state>  
    {



		[Column] public int id { get; set; }





		[Column("state")] public string _state { get; set; }





		[Column] public string detail { get; set; }



	}

    

	[TableName("mmorpg.users")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class users : mysqlDB.Record<users>  
    {



		[Column] public int id { get; set; }





		[Column] public string username { get; set; }





		[Column] public string password { get; set; }





		[Column] public string email { get; set; }





		[Column] public string confirmation_lien_email { get; set; }





		[Column] public sbyte confirmation_email { get; set; }





		[Column] public string ville { get; set; }





		[Column] public string pays { get; set; }





		[Column] public string date_de_nessance { get; set; }





		[Column] public string centre_d_interet { get; set; }





		[Column] public string etat_compte { get; set; }





		[Column] public string etat_compte_reson { get; set; }





		[Column] public string statut_compte { get; set; }





		[Column] public sbyte vip { get; set; }





		[Column] public sbyte premium { get; set; }





		[Column] public sbyte abonnement { get; set; }





		[Column] public string fin_abonnement { get; set; }





		[Column] public string question_secrette { get; set; }





		[Column] public string reponse_secrette { get; set; }



	}

    

	[TableName("mmorpg.version")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class version : mysqlDB.Record<version>  
    {



		[Column] public int id { get; set; }





		[Column] public string app { get; set; }





		[Column("version")] public string _version { get; set; }



	}

    

	[TableName("mmorpg.xplevel")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class xplevel : mysqlDB.Record<xplevel>  
    {



		[Column] public int id { get; set; }





		[Column] public int level { get; set; }





		[Column] public int xp { get; set; }



	}


}
