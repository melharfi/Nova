using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class GrabingMapObjectsInformationRequestMessage : IRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        private Actor _actor;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            CommandStrings = commandStrings;
            Nc = nc;
            _actor = (Actor)nc.Tag;
        }

        public bool Check()
        {
            return _actor.Pseudo != "" && _actor.inBattle != 1;
        }

        public void Apply()
        {
            // envoie au client les objets sur le map
            StringBuilder buffer = new StringBuilder();

            // pour simula la présence des objets créée lors d'un combat,2 bouclier
            /*(DataBase.DataTables.mapobj as List<mysql.mapobj>).Clear();
            mysql.mapobj moA = new mapobj
            {
                id = 1,
                idBattle = 1,
                map = "Start",
                map_position = "10/15",
                state = "dynamic",
                obj = "FreeChallenge",
                assoc = "A"
            };

            mysql.mapobj moB = new mapobj
            {
                id = 1,
                idBattle = 1,
                map = "Start",
                map_position = "12/15",
                state = "dynamic",
                obj = "FreeChallenge",
                assoc = "B"
            };

            (DataBase.DataTables.mapobj as List<mysql.mapobj>).Add(moA);
            (DataBase.DataTables.mapobj as List<mysql.mapobj>).Add(moB);*/

            foreach (mysql.mapobj mapobj in ((List<mysql.mapobj>)DataBase.DataTables.mapobj).FindAll(f => f.map == _actor.map))
            {
                if (mapobj.obj == Enums.BattleType.Type.FreeChallenge.ToString())
                {
                    // crafted data specific to FreeChallenge syntax
                    buffer.Append(mapobj.obj + "#" + mapobj.map_position + "#" + mapobj.idBattle + "#" + mapobj.assoc);

                    Battle battle = Battle.Battles.Find(f => f.IdBattle == mapobj.idBattle);

                    /////
                    /*battle = new Battle();
                    battle.SideA = new List<Actor>();

                    Actor sideA1 = new Actor
                    {
                        Pseudo = "hamid",
                        ClasseId = 0,
                        village = Enums.HiddenVillage.Names.konoha.ToString(),
                        Level = 50,
                        Spirit = Spirit.Name.angel,
                        SpiritLvl = 9
                    };

                    Actor sideA2 = new Actor
                    {
                        Pseudo = "yassin",
                        ClasseId = 0,
                        village = Enums.HiddenVillage.Names.konoha.ToString(),
                        Level = 50,
                        Spirit = Spirit.Name.angel,
                        SpiritLvl = 9
                    };

                    Actor sideB1 = new Actor
                    {
                        Pseudo = "morad",
                        ClasseId = 0,
                        village = Enums.HiddenVillage.Names.konoha.ToString(),
                        Level = 50,
                        Spirit = Spirit.Name.angel,
                        SpiritLvl = 9
                    };

                    Actor sideB2 = new Actor
                    {
                        Pseudo = "soufian",
                        ClasseId = 0,
                        village = Enums.HiddenVillage.Names.konoha.ToString(),
                        Level = 50,
                        Spirit = Spirit.Name.angel,
                        SpiritLvl = 9
                    };

                    battle.SideA.Add(sideA1);
                    battle.SideA.Add(sideA2);
                    battle.SideB.Add(sideB1);
                    battle.SideB.Add(sideB2);*/
                    ////////

                    //if (battle == null) continue;
                    string sidesInforomations = "";
                    if (mapobj.assoc == Enums.Team.Side.A.ToString())
                        sidesInforomations = battle.SideA.Aggregate(sidesInforomations, (current, t) => current + (t.Pseudo + "#" + t.classeId + "#" + t.hiddenVillage + "#" + t.level + "#" + t.spirit + "#" + t.spiritLvl + @"\"));
                    else if (mapobj.assoc == Enums.Team.Side.B.ToString())
                        sidesInforomations = battle.SideB.Aggregate(sidesInforomations, (current, t) => current + (t.Pseudo + "#" + t.classeId + "#" + t.hiddenVillage + "#" + t.level + "#" + t.spirit + "#" + t.spiritLvl + @"\"));
                    sidesInforomations = sidesInforomations.Substring(0, sidesInforomations.Length - 1);
                    buffer.Append("°" + sidesInforomations);
                }
                buffer.Append("|");
            }

            // pourquoi on retir 2 fois le dernier caractere si > 0 et si != ""
            if (buffer.Length > 0)
                buffer = new StringBuilder().Append(buffer.ToString().Substring(0, buffer.Length - 1));
                
            GrabingMapObjectsInformationResponseMessage grabingMapObjectsInformationResponseMessage = new GrabingMapObjectsInformationResponseMessage();
            grabingMapObjectsInformationResponseMessage.Initialize(new []{buffer.ToString()}, Nc);
            grabingMapObjectsInformationResponseMessage.Serialize();
            grabingMapObjectsInformationResponseMessage.Send();
        }
    }
}
