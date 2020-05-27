using Lidgren.Network;
using SERVER.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Effects.Handlers
{
    public static class killSummonToBoost
    {
        public static string Apply(object[] parameters)
        {
            // flag 1 = id de la classe qui se trouve dans la table Classes afin de soustraire les caractèristique de l'invocation
            // flag 2 = caracteristique a booster
            // flag 3 = valeur
            Actor spellCaster = parameters[0] as Actor;
            List<Effects.ZoneEffect.ZoneEffectTemplate> affectedPlayers = parameters[1] as List<Effects.ZoneEffect.ZoneEffectTemplate>;
            int spellID = (int)parameters[2];
            Actor.effects effect = parameters[3] as Actor.effects;
            bool cd = Convert.ToBoolean(parameters[4]);
            Point spellPos = parameters[5] as Point;
            mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID);
            //NetConnection nim = MainClass.netServer.Connections.Find(f => f.Tag != null && (f.Tag as PlayerInfo).Pseudo == spellCaster.Pseudo);

            //Actor.SpellsInformations infos_sorts = spellCaster.sorts.Find(f => f.sortID == spellID);
            //mysql.spells spell_Template = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == infos_sorts.level);

            Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);
            ////////////////////////////////////////////////////////////////////////

            int summonID = Convert.ToInt32(effect.flag1);
            
            // si sacrifiedSummon est null alors il y à un problème puisque l'invocation dois exister, et que les ancien controle faite n'ont pas vérifier s'il y avais une invoc ou pas, a voir SERVER.Effects.EffectBase.killSummonToBoost pourquoi elle na pas vérifier
            Actor sacrifiedSummon = affectedPlayers.Find(f => f.AffectedActor.summonID == summonID && f.AffectedActor.owner == spellCaster.Pseudo && f.AffectedActor.species == Species.Name.Summon).AffectedActor;

            // elimination de l'invocation qui se trouve dans la liste affected
            affectedPlayers.RemoveAll(f => f.AffectedActor.summonID == summonID && f.AffectedActor.owner == spellCaster.Pseudo && f.AffectedActor.species == Species.Name.Summon);

            string Segment = "";
            foreach (ZoneEffect.ZoneEffectTemplate zet in affectedPlayers)
            {
                Type feature = Type.GetType("SERVER.Features.Handlers." + effect.flag2);
                if(feature != null)
                {
                    System.Reflection.MethodInfo featureHandler = feature.GetMethod("Apply");
                    if (featureHandler != null)
                    {
                        object[] featureParameters = new object[7];
                        featureParameters[0] = spellCaster;
                        featureParameters[1] = zet;
                        featureParameters[2] = sacrifiedSummon;
                        featureParameters[3] = effect.flag3;
                        featureParameters[4] = spell;
                        featureParameters[5] = effect;
                        featureParameters[6] = cd;

                        Segment += featureHandler.Invoke(null, new object[] { featureParameters }) + "/";
                    }
                    else
                        Console.WriteLine("There's no methode called 'Apply' for character feature called '" + effect.flag2 + "'");
                }
                else
                    Console.WriteLine("There's no handler for '" + effect.flag2 + "' character feature in SERVER.Features.Handlers");
            }

            if (Segment != "")
                Segment = Segment.Substring(0, Segment.Length - 1);
            else
                Segment = "null";

            // supression de l'invocation
            // retirer le joueur de la liste des joueurs en vie
            string playerDead = sacrifiedSummon.Pseudo;
            _battle.DeadPlayers.Add((Actor)(_battle.AllPlayersByOrder.Find(f => f.Pseudo == sacrifiedSummon.Pseudo)).Clone());
            _battle.AllPlayersByOrder.RemoveAll(f => f.Pseudo == sacrifiedSummon.Pseudo);
            _battle.SideA.RemoveAll(f => f.Pseudo == sacrifiedSummon.Pseudo);
            _battle.SideB.RemoveAll(f => f.Pseudo == sacrifiedSummon.Pseudo);

            string buffer = "typeRox:desinvocation|cd:" + cd + "|chakra:neutre|deadList:" + playerDead + "|" + Segment;

            return buffer;
        }
    }
}
