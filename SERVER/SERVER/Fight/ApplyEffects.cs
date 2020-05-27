using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVER.Enums;

namespace SERVER.Fight
{
    static class Effect_Dispatcher
    {
        public static void Apply(Actor spellCaster, Battle _battle, Point spellPos, int spellID)
        {
            NetConnection nim = MainClass.netServer.Connections.Find(f => f.Tag != null && (f.Tag as Actor).Pseudo == spellCaster.Pseudo);
            Actor.SpellsInformations infos_sorts = spellCaster.sorts.Find(f => f.SpellId == spellID);
            mysql.spells spell_Template = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == infos_sorts.Level);

            #region ZoneEffect qui permet la recherche de tous les joueurs prises dans l'effet selon la zone d'effet
            Type typeZoneEffect = Type.GetType("SERVER.Effects.ZoneEffect." + spell_Template.zoneEffect);
            System.Reflection.MethodInfo zoneEffect = typeZoneEffect.GetMethod("affected");

            object[] parameters = new object[4];
            parameters[0] = _battle;
            parameters[1] = spellPos;
            parameters[2] = spell_Template;
            parameters[3] = spellCaster;

            List<Effects.ZoneEffect.ZoneEffectTemplate> affectedPlayers;
            
            if (zoneEffect != null)
                affectedPlayers = zoneEffect.Invoke(null, new object[] { parameters }) as List<Effects.ZoneEffect.ZoneEffectTemplate>;
            else
                affectedPlayers = new List<Effects.ZoneEffect.ZoneEffectTemplate>();
            #endregion

            string buffer = "";

            #region CD
            int cdRequird = spell_Template.cd;
            int cdBonnus = spell_Template.cdDomBonnus;
            int cdLeft = cdRequird - spellCaster.cd;
            if (cdLeft <= 0)
                cdLeft = 1;

            Random rnd = new Random();

            // mise en 1/2 si le nombre cd est inférieur ou égale à 1 
            bool cd = false;
            if (cdLeft <= 1)
                cdLeft = 1;

            // tentative de cd
            int rndCD = rnd.Next(0, cdLeft + 1);
            if (rndCD == 0)
                cd = true;

            #endregion

            #region balayage sur tout les effet (check target type et execution du handler du sort)
            foreach (Actor.effects current_effect in infos_sorts.effect)
            {
                #region check target type
                Actor centralFocus = _battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);
                Enums.spell_effect_target.targets target = CommonCode.TargetParser(spellCaster, centralFocus);
                if (!current_effect.targets.Exists(f => f == target))
                {
                    CommonCode.SendMessage("cmd•spellTargetNotAllowed", nim, true);
                    break;
                }
                #endregion
                
                #region buffer
                int minDice = current_effect.min;
                int maxDice = current_effect.max;

                Type t2 = Type.GetType("SERVER.Effects.Handlers." + current_effect.base_effect);
                if(t2 == null)
                {
                    Console.WriteLine("le handler pour SERVER.Effecrs.Handlers." + current_effect.base_effect + " n'existe pas");
                    return;
                }
                System.Reflection.MethodInfo TypeEffect = t2.GetMethod("Apply");

                object[] parameters2 = new object[6];
                parameters2[0] = spellCaster;
                parameters2[1] = affectedPlayers;
                parameters2[2] = spellID;
                parameters2[3] = current_effect;
                parameters2[4] = cd;
                parameters2[5] = spellPos;

                if (TypeEffect != null)
                    buffer += (string)TypeEffect.Invoke(null, new object[] { parameters2 }) + "#";

                if (buffer != "")
                    buffer = buffer.Substring(0, buffer.Length - 1);
                #endregion
            }
            #endregion

            #region inscription dans les envoutements
            foreach (Actor.effects current_effect in infos_sorts.effect)
            {
                Type buffHandler = Type.GetType("SERVER.Buff." + current_effect.base_effect);
                if (buffHandler != null)
                {
                    System.Reflection.MethodInfo buffHandlerApply = buffHandler.GetMethod("Apply");

                    object[] parameters2 = new object[6];
                    parameters2[0] = spellCaster;
                    parameters2[1] = affectedPlayers;
                    parameters2[2] = spellID;
                    parameters2[3] = current_effect;
                    parameters2[4] = cd;
                    parameters2[5] = spellPos;

                    if (buffHandlerApply != null)
                        buffHandlerApply.Invoke(null, new object[] { parameters2 });
                }
                else
                {

                }
            }
            #endregion

            #region diminution des pc
            spellCaster.currentPc -= spell_Template.pc;
            #endregion

            #region envoie de la command à tous les joueurs
            List<Actor> humans_in_battle = _battle.AllPlayersByOrder.FindAll(f => f.species == Species.Name.Human);

            // envoie d'une cmd a tous les abonnés au combats
            for (int cnt2 = 0; cnt2 < humans_in_battle.Count; cnt2++)
            {
                // envoie d'une requette a tous les personnages
                NetConnection humainNim = MainClass.netServer.Connections.Find(f => f.Tag != null && (f.Tag as Actor).Pseudo == humans_in_battle[cnt2].Pseudo);
                if (nim != null && humainNim != null)
                {
                    CommonCode.SendMessage("cmd•spellTileGranted•" + spellCaster.Pseudo + "•" + spellID + "•" + spellPos.X + "•" + spellPos.Y + "•" + infos_sorts.SpellColor + "•" + infos_sorts.Level + "•" + buffer + "•PcUsed:" + spell_Template.pc, humainNim, true);
                    Console.WriteLine("<--cmd•spellTileGranted•" + spellCaster.Pseudo + "•" + spellID + "•" + spellPos.X + "•" + spellPos.Y + "•" + infos_sorts.SpellColor + "•" + infos_sorts.Level + "•" + buffer + "•PcUsed:" + spell_Template.pc + " to " + (humainNim.Tag as Actor).Pseudo);
                }
            }
            #endregion
            //////////////////////////////////////////////////////////////////////////////////

            ///////////////////// cloture du combat s'il n y a personne
            if (buffer != "")
                for (int cnt2 = 0; cnt2 < buffer.Split('#').Length; cnt2++)
                {
                    string dom2 = buffer.Split('#')[cnt2];
                    if (dom2.Split('|')[0] == "typeRox:rox")
                    {
                        string deadPlayer = dom2.Split('|')[5].Split(':')[1];
                        if (deadPlayer != "")
                        {
                            // supprimer le joueur de la liste des joueurs actifs
                            // check si le combat est terminé	
                            Actor _playerTargeted = _battle.DeadPlayers.Find(f => f.Pseudo == deadPlayer);
                            if (CommonCode.IsClosedBattle(_battle, _playerTargeted))
                                _battle.State = Enums.battleState.state.closed;
                            else
                                Console.WriteLine("nothing !!!");
                        }
                    }
                }
            ///////////////////////////////////////////////////////////////
        }
    }
}
