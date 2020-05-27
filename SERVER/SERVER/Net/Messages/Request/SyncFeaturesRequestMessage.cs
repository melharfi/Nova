using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using SERVER.Enums;
using SERVER.Net.Messages.Response;

namespace SERVER.Net.Messages.Request
{
    internal class SyncFeaturesRequestMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        Actor _actor;

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            Nc = nc;
            CommandStrings = commandStrings;
            _actor = (Actor)Nc.Tag;
        }

        public bool Check()
        {
            if (_actor.Pseudo == "" || _actor.map == "")
            {
                Security.User_banne("Actor without generic stats.SyncFeaturesRequestMessage", Nc);
                return false;
            }
            return true;
        }

        public void Apply()
        {
            // le client demande une mise a jour de ses states
            CommonCode.RefreshStats(Nc);
            string spell = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == _actor.Pseudo).sorts;

            // recuperer le totalXp du niveau en cours
            mysql.xplevel xplevel = ((List<mysql.xplevel>)DataBase.DataTables.xplevel).Find(f => f.level == _actor.level + 1);

            //int totalXp = xplevel?.xp ?? 0;
            int totalXp = xplevel == null ? 0 : xplevel.xp;

            // convertir la list de quete en string, ce code se trouve sur 2 endroit, quand le joueur selectionne un player et quand le joueur été déja dans un combat et que apres un co qui précédé une deco, le player se selectionne tous seul
            string quests = _actor.Quests.Aggregate("", (current, t) => current + (t.QuestName + ":" + t.MaxSteps.ToString() + ":" + t.CurrentStep + ":" + t.Submited) + "/");

            if (quests != "")
                quests = quests.Substring(0, quests.Length - 1);

            // cette cmd dois envoyer les meme données que sur la cmd sur getData Ln 34x
            object[] o = new object[80];
            o[0] = _actor.Pseudo + "#" +_actor.classeName + "#" +_actor.spirit + "#" +_actor.spiritLvl + "#" +_actor.Pvp + "#" +_actor.hiddenVillage + "#" +_actor.maskColorString + "#" +_actor.directionLook + "#" +_actor.level + "#" +_actor.map + "#" +_actor.officialRang + "#" +_actor.currentHealth + "#" +_actor.maxHealth + "#" +_actor.xp + "#" + totalXp + "#" +_actor.doton + "#" +_actor.katon + "#" +_actor.futon + "#" +_actor.raiton + "#" +_actor.suiton + "#" + MainClass.chakralvl1 + "#" + MainClass.chakralvl2 + "#" + MainClass.chakralvl3 + "#" + MainClass.chakralvl4 + "#" + MainClass.chakralvl5 + "#" +_actor.usingDoton + "#" +_actor.usingKaton + "#" +_actor.usingFuton + "#" +_actor.usingRaiton + "#" +_actor.usingSuiton + "#" +_actor.equipedDoton + "#" +_actor.equipedKaton + "#" +_actor.equipedFuton + "#" +_actor.equipedRaiton + "#" +_actor.equipedSuiton + "#" +_actor.originalPc +
                    "#" +_actor.originalPm + "#" +_actor.pe + "#" +_actor.cd + "#" +_actor.summons + "#" +_actor.initiative + "#" +
                   _actor.job1 + "#" +_actor.job2 + "#" +_actor.specialty1 + "#" +_actor.specialty2 + "#" +_actor.maxWeight + "#" +
                   _actor.currentWeight + "#" +_actor.ryo + "#" +_actor.resiDotonPercent + "#" +_actor.resiKatonPercent + "#" +
                   _actor.resiFutonPercent + "#" +_actor.resiRaitonPercent + "#" +_actor.resiSuitonPercent + "#" +_actor.dodgePC + "#" +
                   _actor.dodgePM + "#" +_actor.dodgePE + "#" +_actor.dodgeCD + "#" +_actor.removePC + "#" +_actor.removePM + "#" +
                   _actor.removePE + "#" +_actor.removeCD + "#" +_actor.escape + "#" +_actor.blocage + "#" + spell + "#" +
                   _actor.resiDotonFix + "#" +_actor.resiKatonFix + "#" +_actor.resiFutonFix + "#" +_actor.resiRaitonFix + "#" +
                   _actor.resiSuitonFix + "#" +_actor.resiFix + "#" +_actor.domDotonFix + "#" +_actor.domKatonFix + "#" +_actor.domFutonFix
                    + "#" +_actor.domRaitonFix + "#" +_actor.domSuitonFix + "#" +_actor.domFix + "#" +_actor.power + "#" +_actor.equipedPower
                    + "•" + quests + "•" +_actor.spellPointLeft;
            o[1] = quests;
            o[2] =_actor.spellPointLeft;

            // les states ont été séparé mais j'ai abondonné l'idée pour les reunir dans un seul offset
            /*o[0] = _actor.Pseudo;
            o[1] = _actor.classeName;
            o[2] = _actor.Spirit;
            o[3] = _actor.SpiritLvl;
            o[4] = _actor.Pvp;
            o[5] = _actor.village;
            o[6] = _actor.MaskColors;
            o[7] = _actor.Orientation;
            o[8] = _actor.Level;
            o[9] = _actor.map;
            o[10] = _actor.officialRang;
            o[11] = _actor.currentHealth;
            o[12] = _actor.totalHealth;
            o[13] = _actor.xp;
            o[14] = totalXp;
            o[15] = _actor.doton;
            o[16] = _actor.katon;
            o[17] = _actor.futon;
            o[18] = _actor.raiton;
            o[19] = _actor.suiton;
            o[20] = MainClass.chakralvl1;
            o[21] = MainClass.chakralvl2;
            o[22] = MainClass.chakralvl3;
            o[23] = MainClass.chakralvl4;
            o[24] = MainClass.chakralvl5;
            o[25] = _actor.usingDoton;
            o[26] = _actor.usingKaton;
            o[27] = _actor.usingFuton;
            o[28] = _actor.usingRaiton;
            o[29] = _actor.usingSuiton;
            o[30] = _actor.equipedDoton;
            o[31] = _actor.equipedKaton;
            o[32] = _actor.equipedFuton;
            o[33] = _actor.equipedRaiton;
            o[34] = _actor.equipedSuiton;
            o[35] = _actor.original_Pc;
            o[36] = _actor.original_Pm;
            o[37] = _actor.pe;
            o[38] = _actor.cd;
            o[39] = _actor.summon;
            o[40] = _actor.Initiative;
            o[41] = _actor.job1;
            o[42] = _actor.job2;
            o[43] = _actor.specialty1;
            o[44] = _actor.specialty2;
            o[45] = _actor.maxWeight;
            o[46] = _actor.CurrentWeight;
            o[47] = _actor.Ryo;
            o[48] = _actor.resiDotonPercent;
            o[49] = _actor.resiKatonPercent;
            o[50] = _actor.resiFutonPercent;
            o[51] = _actor.resiRaitonPercent;
            o[52] = _actor.resiSuitonPercent;
            o[53] = _actor.dodgePC;
            o[54] = _actor.dodgePM;
            o[55] = _actor.dodgePE;
            o[56] = _actor.dodgeCD;
            o[57] = _actor.removePC;
            o[58] = _actor.removePM;
            o[59] = _actor.removePE;
            o[60] = _actor.removeCD;
            o[61] = _actor.escape;
            o[62] = _actor.blocage;
            o[63] = spell;
            o[64] = _actor.resiDotonFix;
            o[65] = _actor.resiKatonFix;
            o[66] = _actor.resiFutonFix;
            o[67] = _actor.resiRaitonFix;
            o[68] = _actor.resiSuitonFix;
            o[69] = _actor.resiFix;
            o[70] = _actor.domDotonFix;
            o[71] = _actor.domKatonFix;
            o[72] = _actor.domFutonFix;
            o[73] = _actor.domRaitonFix;
            o[74] = _actor.domSuitonFix;
            o[75] = _actor.domFix;
            o[76] = _actor.power;
            o[77] = _actor.equipedPower;
            o[78] = quests;
            o[79] = _actor.spellPointLeft;*/

            SyncFeaturesResponseMessage syncFeaturesResponseMessage = new SyncFeaturesResponseMessage(o);
            syncFeaturesResponseMessage.Serialize();
            syncFeaturesResponseMessage.Send();
        }
    }
}
