using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Effects.Handlers
{
    public static class defaultDamage
    {
        public static string Apply(object[] parameters)
        {
            Actor spellCaster = parameters[0] as Actor;
            //Effects.ZoneEffect.zoneEffectTemplate affected = parameters[1] as Effects.ZoneEffect.zoneEffectTemplate;
            List<Effects.ZoneEffect.ZoneEffectTemplate> affectedPlayers = parameters[1] as List<Effects.ZoneEffect.ZoneEffectTemplate>;
            int spellID = (int)parameters[2];
            Actor.effects effect = parameters[3] as Actor.effects;
            bool cd = Convert.ToBoolean(parameters[4]);
            Point spellPos = parameters[5] as Point;
            
            Actor.SpellsInformations infos_sorts = spellCaster.sorts.Find(f => f.SpellId == spellID);
            mysql.spells spell_Template = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == infos_sorts.Level);
            
            Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);

            // mise en randome du dom sort
            Random rnd = new Random();
            int jet = rnd.Next(effect.min, effect.max + 1 + ((cd == true) ? spell_Template.cdDomBonnus : 0));

            int element = 0, equipedElement = 0, elementChakraLevel = 0, domElementFix = 0, resiElementFix = 0, resiElementPercent = 0;

            string buffer = "";

            // si aucun joueur n'est attribué, on envoie comme meme une cmd vide dans la partie dédié, pour que au moin en entre dans la boucle foreach
            if (affectedPlayers.Count() == 0)
            {
                ZoneEffect.ZoneEffectTemplate zet = new ZoneEffect.ZoneEffectTemplate();
                zet.AffectedActor = null;
                affectedPlayers.Add(zet);
            }

            foreach (ZoneEffect.ZoneEffectTemplate affected in affectedPlayers)
            {
                if (spell_Template.element.ToLower() == Enums.Chakra.Element.doton.ToString())
                {
                    element = spellCaster.doton;
                    equipedElement = spellCaster.equipedDoton;
                    elementChakraLevel = spellCaster.dotonChakraLevel;
                    domElementFix = spellCaster.domDotonFix;
                    if (affected.AffectedActor != null)
                    {
                        resiElementFix = affected.AffectedActor.resiDotonFix;
                        resiElementPercent = affected.AffectedActor.resiDotonPercent;
                    }
                }
                else if (spell_Template.element.ToLower() == Enums.Chakra.Element.futon.ToString())
                {
                    element = spellCaster.futon;
                    equipedElement = spellCaster.equipedFuton;
                    elementChakraLevel = spellCaster.futonChakraLevel;
                    domElementFix = spellCaster.domFutonFix;
                    if (affected.AffectedActor != null)
                    {
                        resiElementFix = affected.AffectedActor.resiFutonFix;
                        resiElementPercent = affected.AffectedActor.resiFutonPercent;
                    }
                }
                else if (spell_Template.element.ToLower() == Enums.Chakra.Element.katon.ToString())
                {
                    element = spellCaster.katon;
                    equipedElement = spellCaster.equipedKaton;
                    elementChakraLevel = spellCaster.katonChakraLevel;
                    domElementFix = spellCaster.domKatonFix;
                    if (affected.AffectedActor != null)
                    {
                        resiElementFix = affected.AffectedActor.resiKatonFix;
                        resiElementPercent = affected.AffectedActor.resiKatonPercent;
                    }
                }
                else if (spell_Template.element.ToLower() == Enums.Chakra.Element.raiton.ToString())
                {
                    element = spellCaster.raiton;
                    equipedElement = spellCaster.equipedRaiton;
                    elementChakraLevel = spellCaster.raitonChakraLevel;
                    domElementFix = spellCaster.domRaitonFix;
                    if (affected.AffectedActor != null)
                    {
                        resiElementFix = affected.AffectedActor.resiRaitonFix;
                        resiElementPercent = affected.AffectedActor.resiRaitonPercent;
                    }
                }
                else if (spell_Template.element.ToLower() == Enums.Chakra.Element.suiton.ToString())
                {
                    element = spellCaster.suiton;
                    equipedElement = spellCaster.equipedSuiton;
                    elementChakraLevel = spellCaster.suitonChakraLevel;
                    domElementFix = spellCaster.domSuitonFix;
                    if (affected.AffectedActor != null)
                    {
                        resiElementFix = affected.AffectedActor.resiSuitonFix;
                        resiElementPercent = affected.AffectedActor.resiSuitonPercent;
                    }
                }

                int dammage = (100 + element + equipedElement + spellCaster.power + spellCaster.equipedPower);
                dammage = (int)(jet * dammage) / 100;
                dammage = (int)(dammage * (1 + (0.1 * elementChakraLevel)));
                dammage = dammage + domElementFix + spellCaster.domFix;

                if (affected.AffectedActor != null)
                {
                    dammage *= affected.Pertinance / 100;                // pour les sort qui tape en zone, tant que c proche du centre tant que le rox est meilleur

                    // déduire les resistances du joueur
                    dammage -= ((dammage * resiElementPercent) / 100);
                    dammage -= affected.AffectedActor.resiFix - resiElementFix;

                    // application des dom sur le personnage
                    affected.AffectedActor.currentHealth -= dammage;
                    affected.AffectedActor.maxHealth -= (dammage * 5) / 100;
                }

                // determiner si le ou les joueurs est/sont mort et si le combat est terminé, séparé par : s'il sagit de plusieurs
                string playerDead = "";
                if (affected.AffectedActor != null && affected.AffectedActor.currentHealth <= 0)
                {
                    //recalibrage des pdv a 0 pour eviter tout future problème
                    // joueur KO
                    affected.AffectedActor.currentHealth = 0;
                    playerDead = affected.AffectedActor.Pseudo;

                    // retirer le joueur de la liste des joueurs en vie
                    _battle.DeadPlayers.Add((Actor)(_battle.AllPlayersByOrder.Find(f => f.Pseudo == affected.AffectedActor.Pseudo)).Clone());
                    _battle.AllPlayersByOrder.RemoveAll(f => f.Pseudo == affected.AffectedActor.Pseudo);
                    _battle.SideA.RemoveAll(f => f.Pseudo == affected.AffectedActor.Pseudo);
                    _battle.SideB.RemoveAll(f => f.Pseudo == affected.AffectedActor.Pseudo);
                }

                ////////////////////////////////////////////////
                /// a verifier si le joueur est immunisé contre les dom ou autre pour un futur sort
                /// ////////////////////////////////////////////
                if (dammage < 0)
                    dammage = 0;

                // typerox peux etre rox(déduction),heal(augementation)
                buffer += "typeRox:rox|jet:" + jet + "|cd:" + cd + "|chakra:" + spell_Template.element + "|dom:" + dammage + "|deadList:" + playerDead + "|" + ((affected.AffectedActor != null) ? affected.AffectedActor.Pseudo : "null") + "#";
            }
            if (buffer != "")
                buffer = buffer.Substring(0, buffer.Length - 1);

            return buffer;
        }
    }
}
