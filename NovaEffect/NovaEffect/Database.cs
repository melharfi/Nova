using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using System.Windows.Forms;

namespace NovaEffect.DataBase
{
    class DataTables
    {
        public static Database dataContext;                            // instance PetaPoco
        public static IEnumerable<mysql.spells> spells;
        public static string server, user, password, database;

        public static void IniPetaPoco()
        {
            dataContext = new PetaPoco.Database("server=" + server + ";user id=" + user + ";database=" + database + ";pooling=False", "MySql");
            spells = dataContext.Fetch<mysql.spells>("SELECT * FROM spells");
        }
        public static void Update()
        {
            foreach (var a in spells)
                dataContext.Update("spells", "id", a);
        }
    }
}
