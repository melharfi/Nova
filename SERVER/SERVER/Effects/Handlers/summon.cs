using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Effects.Handlers
{
    public static class summon
    {
        public static string Apply(object[] parameters)
        {
            // flag1 = id des states qui se trouve dans la table summon
            // flag2 = idForSummon

            Actor spellCaster = parameters[0] as Actor;
            List<Effects.ZoneEffect.ZoneEffectTemplate> affectedPlayers = parameters[1] as List<Effects.ZoneEffect.ZoneEffectTemplate>;
            int spellID = (int)parameters[2];
            Actor.effects effect = parameters[3] as Actor.effects;
            bool cd = Convert.ToBoolean(parameters[4]);
            Point spellPos = parameters[5] as Point;

            Actor.SpellsInformations infos_sorts = spellCaster.sorts.Find(f => f.SpellId == spellID);
            mysql.spells spell_Template = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == infos_sorts.Level);
            Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);
            Lidgren.Network.NetConnection nim = MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == spellCaster.Pseudo);

            // check si le joueur a assez de point d'invocation, pour cela, on dois calculer combiens d'invoc il a déja invoqué
            int sumOfInvoc = _battle.AllPlayersByOrder.FindAll(f => f.Pseudo.IndexOf(spellCaster.Pseudo + "$") != -1).Count;
            
            if (spellCaster.summons <= sumOfInvoc)
            {
                // pas assez de point d'invocation
                CommonCode.SendMessage("cmd•spellNotEnoughInvoc", nim, true);
                Console.WriteLine("<--cmd•spellNotEnoughInvoc to " + spellCaster.Pseudo);
                return "";
            }

            int isAllowedSpellArea = Fight.spellsChecker.isAllowedSpellArea(spell_Template.pe, spellCaster.map_position, spellCaster, _battle, spellPos, 0, false, true);

            if (isAllowedSpellArea == 0)
            {
                // spell autorisé
            }
            else if (isAllowedSpellArea == 1)
            {
                // spell non autorisé, case obstacle
                CommonCode.SendMessage("cmd•spellTileNotAllowed", nim, true);
                Console.WriteLine("<--cmd•spellTileNotAllowed to " + spellCaster.Pseudo);
                return "";
            }
            else if (isAllowedSpellArea == 2)
            {
                // spell non autorisé, pas de porté
                CommonCode.SendMessage("cmd•spellPointNotEnoughPe", nim, true);
                return "";
            }
            else if (isAllowedSpellArea == -1)
            {
                // impossible de determiner la direction, normalement ca deverai le deviner
                return "";
            }

            // creation d'un clone
            mysql.summon _summon = (DataBase.DataTables.summon as List<mysql.summon>).Find(f => f.template_id == Convert.ToInt32(effect.flag1));
            Actor __clone_jutsu_naruto = Summon.StatsDispatcher.Apply(spellCaster, _summon, spell_Template, effect);

            // creation d'un id aleatoire pour l'invocation
            string rndStr = "";
            // check si le nom de l'invoc est déja dans la liste des joueurs pour eviter un doublons
            while (true)
            {
                rndStr = CommonCode.ReturnRandomId();
                if (!_battle.AllPlayersByOrder.Exists(f => f.Pseudo == __clone_jutsu_naruto.Pseudo + "$" + rndStr))
                    break;
            }

            __clone_jutsu_naruto.Pseudo = __clone_jutsu_naruto.Pseudo + "$" + rndStr;   // separateur entre le nom et le id de l'invocation
            __clone_jutsu_naruto.map_position = spellPos;
            

            // insert dans la liste
            int index = _battle.AllPlayersByOrder.FindIndex(f => f.Pseudo == spellCaster.Pseudo);
            _battle.AllPlayersByOrder.Insert(index + 1, __clone_jutsu_naruto);

            string buffer = "";

            // conversion des rawdata sort en base64
            string encryptedSpellsRaw = Cryptography.Algo.Encoding.Base64Encode(_summon.sorts);

            //rawData = "typeRox:addInvoc|name:x|cd:x|totalPdv:x";
            string piRaw = __clone_jutsu_naruto.Pseudo + ":" + __clone_jutsu_naruto.classeName + ":" + __clone_jutsu_naruto.spirit + ":" + __clone_jutsu_naruto.spiritLvl.ToString() + ":" + __clone_jutsu_naruto.Pvp.ToString() + ":" + __clone_jutsu_naruto.hiddenVillage + ":" + __clone_jutsu_naruto.maskColorString + ":" + __clone_jutsu_naruto.directionLook.ToString() + ":" + __clone_jutsu_naruto.level.ToString() + ":" + __clone_jutsu_naruto.map + ":" + __clone_jutsu_naruto.officialRang.ToString() + ":" + __clone_jutsu_naruto.currentHealth.ToString() + ":" + __clone_jutsu_naruto.maxHealth.ToString() + ":" + __clone_jutsu_naruto.doton.ToString() + ":" + __clone_jutsu_naruto.katon.ToString() + ":" + __clone_jutsu_naruto.futon.ToString() + ":" + __clone_jutsu_naruto.raiton.ToString() + ":" + __clone_jutsu_naruto.suiton.ToString() + ":" + /*MainClass.chakralvl2*/ 0 + ":" + /*MainClass.chakralvl3*/ 0 + ":" + /*MainClass.chakralvl4*/ 0 + ":" + /*MainClass.chakralvl5*/ 0 + ":" + /*MainClass.chakralvl6*/ 0 + ":" + /*pi.usingDoton.ToString()*/ 0 + ":" + /*pi.usingKaton.ToString()*/ 0 + ":" + /*pi.usingFuton.ToString()*/ 0 + ":" + /*pi.usingRaiton.ToString()*/ 0 + ":" + /*pi.usingSuiton.ToString()*/ 0 + ":" + /*pi.dotonEquiped.ToString()*/ 0 + ":" + /*pi.katonEquiped.ToString()*/ 0 + ":" + /*pi.futonEquiped.ToString()*/ 0 + ":" + /*pi.raitonEquiped.ToString()*/ 0 + ":" + /*pi.suitonEquiped.ToString()*/ 0 + ":" + __clone_jutsu_naruto.originalPc.ToString() + ":" + __clone_jutsu_naruto.originalPm.ToString() + ":" + __clone_jutsu_naruto.pe.ToString() + ":" + __clone_jutsu_naruto.cd.ToString() + ":" + __clone_jutsu_naruto.summons.ToString() + ":" + __clone_jutsu_naruto.initiative.ToString() + ":" + __clone_jutsu_naruto.resiDotonPercent.ToString() + ":" + __clone_jutsu_naruto.resiKatonPercent.ToString() + ":" + __clone_jutsu_naruto.resiFutonPercent.ToString() + ":" + __clone_jutsu_naruto.resiRaitonPercent.ToString() + ":" + __clone_jutsu_naruto.resiSuitonPercent.ToString() + ":" + __clone_jutsu_naruto.dodgePC.ToString() + ":" + __clone_jutsu_naruto.dodgePM.ToString() + ":" + __clone_jutsu_naruto.dodgePE.ToString() + ":" + __clone_jutsu_naruto.dodgeCD.ToString() + ":" + __clone_jutsu_naruto.removePC.ToString() + ":" + __clone_jutsu_naruto.removePM.ToString() + ":" + __clone_jutsu_naruto.removePE.ToString() + ":" + __clone_jutsu_naruto.removeCD.ToString() + ":" + __clone_jutsu_naruto.escape.ToString() + ":" + __clone_jutsu_naruto.blocage.ToString() + ":" + encryptedSpellsRaw + ":" + __clone_jutsu_naruto.resiDotonFix + ":" + __clone_jutsu_naruto.resiKatonFix + ":" + __clone_jutsu_naruto.resiFutonFix + ":" + __clone_jutsu_naruto.resiRaitonFix + ":" + __clone_jutsu_naruto.resiSuitonFix + ":" + __clone_jutsu_naruto.resiFix + ":" + __clone_jutsu_naruto.domDotonFix + ":" + __clone_jutsu_naruto.domKatonFix + ":" + __clone_jutsu_naruto.domFutonFix + ":" + __clone_jutsu_naruto.domRaitonFix + ":" + __clone_jutsu_naruto.domSuitonFix + ":" + __clone_jutsu_naruto.domFix + ":" + __clone_jutsu_naruto.power + ":" + __clone_jutsu_naruto.equipedPower;
            buffer = "typeRox:addInvoc|" + piRaw + "|cd:" + cd;

            return buffer;
        }
    }
}
