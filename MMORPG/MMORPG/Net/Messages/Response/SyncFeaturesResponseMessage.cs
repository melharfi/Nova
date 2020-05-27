using System;
using System.Drawing;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class SyncFeaturesResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            // cmd pour update des states apres la cloture du combat et les states de mon joueur seulement
            // commandStrings[0] = Pseudo#ClasseName#Spirit#SpiritLvl#Pvp#village#MaskColors#Orientation#Level#map#rang
                //#CurrentPdv#totalPdv#xp#TotalXp#doton#katon#futon#raiton#suiton#chakralvl2
                //#chakralvl3#chakralvl4#chakralvl5#chakralvl6#usingDoton#usingKaton#usingFuton#usingRaiton#usingSuiton#equipedDoton
                //#equipedKaton#equipedFuton#equipedRaiton#suitonEquiped#pc#pm#pe#cd#invoc#Initiative
                //#job1#job2#specialite1#specialite2#TotalPoid#CurrentPoid#Ryo#resiDoton#resiKaton#resiFuton
                //#resiRaiton#resiSuiton#esquivePC#esquivePM#esquivePE#esquiveCD#retraitPC#retraitPM#retraitPE#retraitCD
                //#evasion#blocage#_sorts#resiDotonFix#resiKatonFix#resiFutonFix#resiRaitonFix#resiSuitonFix#resiFix#domDotonFix
                //#domKatonFix#domFutonFix#domRaitonFix#domSuitonFix#domFix#puissance#puissanceEquiped"
            // commandStrings[1]= quete
            // commandStrings[2] = spellPointLeft
            
            string[] states = commandStrings[0].Split('#');
            string actorName = states[0];
            Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
            Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit.Name), states[2]);
            int spiritLevel = int.Parse(states[3]);
            bool pvpEnabled = bool.Parse(states[4]);
            Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[5]);
            string maskColorsString = states[6];
            string[] maskColors = states[6].Split('/');
            int orientation = int.Parse(states[7]);
            int level = int.Parse(states[8]);
            string map = states[9];
            Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), states[10]);
            int currentHealth = int.Parse(states[11]);
            int maxHealth = int.Parse(states[12]);
            int currentXp = int.Parse(states[13]);
            int maxXp = int.Parse(states[14]);
            int doton = int.Parse(states[15]);
            int katon = int.Parse(states[16]);
            int futon = int.Parse(states[17]);
            int raiton = int.Parse(states[18]);
            int suiton = int.Parse(states[19]);
            int chakra1Level = int.Parse(states[20]);
            int chakra2Level = int.Parse(states[21]);
            int chakra3Level = int.Parse(states[22]);
            int chakra4Level = int.Parse(states[23]);
            int chakra5Level = int.Parse(states[24]);
            int usingDoton = int.Parse(states[25]);
            int usingKaton = int.Parse(states[26]);
            int usingFuton = int.Parse(states[27]);
            int usingRaiton = int.Parse(states[28]);
            int usingSuiton = int.Parse(states[29]);
            int equipedDoton = int.Parse(states[30]);
            int equipedKaton = int.Parse(states[31]);
            int equipedFuton = int.Parse(states[32]);
            int equipedRaiton = int.Parse(states[33]);
            int equipedSuiton = int.Parse(states[34]);
            int originalPc = int.Parse(states[35]);
            int originalPm = int.Parse(states[36]);
            int pe = int.Parse(states[37]);
            int cd = int.Parse(states[38]);
            int summons = int.Parse(states[39]);
            int initiative = int.Parse(states[40]);
            string job1 = states[41];
            string job2 = states[42];
            string specialty1 = states[43];
            string specialty2 = states[44];
            int maxWeight = int.Parse(states[45]);
            int currentWeight = int.Parse(states[46]);
            int ryo = int.Parse(states[47]);
            int resiDotonPercent = int.Parse(states[48]);
            int resiKatonPercent = int.Parse(states[49]);
            int resiFutonPercent = int.Parse(states[50]);
            int resiRaitonPercent = int.Parse(states[51]);
            int resiSuitonPercent = int.Parse(states[52]);
            int dodgePc = int.Parse(states[53]);
            int dodgePm = int.Parse(states[54]);
            int dodgePe = int.Parse(states[55]);
            int dodgeCd = int.Parse(states[56]);
            int removePc = int.Parse(states[57]);
            int removePm = int.Parse(states[58]);
            int removePe = int.Parse(states[59]);
            int removeCd = int.Parse(states[60]);
            int escape = int.Parse(states[61]);
            int blocage = int.Parse(states[62]);
            string spells = states[63];
            int resiDotonFix = int.Parse(states[64]);
            int resiKatonFix = int.Parse(states[65]);
            int resiFutonFix = int.Parse(states[66]);
            int resiRaitonFix = int.Parse(states[67]);
            int resiSuitonFix = int.Parse(states[68]);
            int resiFix = int.Parse(states[69]);
            int domDotonFix = int.Parse(states[70]);
            int domKatonFix = int.Parse(states[71]);
            int domFutonFix = int.Parse(states[72]);
            int domRaitonFix = int.Parse(states[73]);
            int domSuitonFix = int.Parse(states[74]);
            int domFix = int.Parse(states[75]);
            int power = int.Parse(states[76]);
            int equipedPower = int.Parse(states[77]);

            string questString = commandStrings[1];
            int spellPointLeft = int.Parse(commandStrings[2]);

            // creation d'une instance Bmp vide just pour contenir les infos dans le tag
            CommonCode.MyPlayerInfo.instance.ibPlayer = new Bmp();
            CommonCode.MyPlayerInfo.instance.ibPlayer.tag = new Actor();
            Actor pi = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
            pi.pseudo = actorName;
            pi.className = className;
            pi.spirit = spirit;
            pi.spiritLevel = spiritLevel;
            pi.pvpEnabled = pvpEnabled;
            pi.hiddenVillage = hiddenVillage;
            pi.maskColorString = maskColorsString;
            pi.directionLook = orientation;
            pi.level = level;
            pi.map = map;
            pi.officialRang = officialRang;
            pi.currentHealth = currentHealth;
            pi.maxHealth = maxHealth;
            pi.currentXp = currentXp;
            pi.maxXp = maxXp;
            pi.doton = doton;
            pi.katon = katon;
            pi.futon = futon;
            pi.raiton = raiton;
            pi.suiton = suiton;

            // association des données des chakralvl2,3,4,5
            CommonCode.chakra1Level = chakra1Level;
            CommonCode.chakra2Level = chakra2Level;
            CommonCode.chakra3Level = chakra3Level;
            CommonCode.chakra4Level = chakra4Level;
            CommonCode.chakra5Level = chakra5Level;

            pi.usingDoton = usingDoton;
            pi.usingKaton = usingKaton;
            pi.usingFuton = usingFuton;
            pi.usingRaiton = usingRaiton;
            pi.usingSuiton = usingSuiton;
            pi.equipedDoton = equipedDoton;
            pi.equipedKaton = equipedKaton;
            pi.equipedFuton = equipedFuton;
            pi.equipedRaiton = equipedRaiton;
            pi.equipedSuiton = equipedSuiton;
            pi.originalPc = originalPc;
            pi.originalPm = originalPm;
            pi.pe = pe;
            pi.cd = cd;
            pi.summons = summons;
            pi.initiative = initiative;
            pi.job1 = job1;
            pi.job2 = job2;
            pi.specialty1 = specialty1;
            pi.specialty2 = specialty2;
            pi.maxWeight = maxWeight;
            pi.currentWeight = currentWeight;
            pi.ryo = ryo;
            pi.resiDotonPercent = resiDotonPercent;
            pi.resiKatonPercent = resiKatonPercent;
            pi.resiFutonPercent = resiFutonPercent;
            pi.resiRaitonPercent = resiRaitonPercent;
            pi.resiSuitonPercent = resiSuitonPercent;
            pi.dodgePc = dodgePc;
            pi.dodgePm = dodgePm;
            pi.dodgePe = dodgePe;
            pi.dodgeCd = dodgeCd;
            pi.removePc = removePc;
            pi.removePm = removePm;
            pi.removePe = removePe;
            pi.removeCd = removeCd;
            pi.escape = escape;
            pi.blocage = blocage;

            if (spells != "")
            {
                for (int cnt = 0; cnt < spells.Split('|').Length; cnt++)
                {
                    string tmp_data = spells.Split('|')[cnt];
                    Actor.SpellsInformations _info_sorts = new Actor.SpellsInformations();
                    _info_sorts.sortID = Convert.ToInt32(tmp_data.Split(':')[0].ToString());
                    _info_sorts.emplacement = Convert.ToInt32(tmp_data.Split(':')[1].ToString());
                    _info_sorts.level = Convert.ToInt32(tmp_data.Split(':')[2]);
                    _info_sorts.colorSort = Convert.ToInt32(tmp_data.Split(':')[3]);
                    pi.spells.Add(_info_sorts);
                }
            }

            pi.resiDotonFix = resiDotonFix;
            pi.resiKatonFix = resiKatonFix;
            pi.resiFutonFix = resiFutonFix;
            pi.resiRaitonFix = resiRaitonFix;
            pi.resiSuitonFix = resiSuitonFix;
            pi.resiFix = resiFix;
            pi.domDotonFix = domDotonFix;
            pi.domKatonFix = domKatonFix;
            pi.domFutonFix = domFutonFix;
            pi.domRaitonFix = domRaitonFix;
            pi.domSuitonFix = domSuitonFix;
            pi.domFix = domFix;
            pi.power = power;
            pi.equipedPower = equipedPower;

            // convertir la list de quete en string
            if (questString != "")
                for (int cnt = 0; cnt < questString.Split('/').Length; cnt++)
                {
                    string _quest = questString.Split('/')[cnt];
                    Actor.QuestInformations qi = new Actor.QuestInformations();
                    qi.nom_quete = _quest.Split(':')[0];
                    qi.totalSteps = Convert.ToInt16(_quest.Split(':')[1]);
                    qi.currentStep = Convert.ToInt16(_quest.Split(':')[2]);
                    qi.submited = Convert.ToBoolean(_quest.Split(':')[3]);
                    pi.Quests.Add(qi);
                }

            CommonCode.CurMap = pi.map;

            // level
            MenuStats.StatsLevel.Text = CommonCode.TranslateText(50) + " " + pi.level;

            // affichage du rang général
            MenuStats.Rang.Text = CommonCode.officialRangToCurrentLangTranslation(pi.officialRang);

            // affichage du level Pvp
            MenuStats.LevelPvp.Text = pi.spiritLevel.ToString();

            // affichage du grade Pvp
            if (spirit != Enums.Spirit.Name.neutral)
            {
                MenuStats.GradePvp = new Bmp(@"gfx\general\obj\2\" + pi.spirit + @"\" + MenuStats.LevelPvp.Text + ".dat", new Point(276 + (15 - Convert.ToInt16(MenuStats.LevelPvp.Text)), 2), new Size(40 + Convert.ToInt16(MenuStats.LevelPvp.Text), 20 + Convert.ToInt16(MenuStats.LevelPvp.Text)), "PlayerStats." + pi.spirit, Manager.TypeGfx.Top, true, 1);
                MenuStats.StatsImg.Child.Add(MenuStats.GradePvp);
            }

            // update des pdv
            HudHandle.UpdateHealth();

            MenuStats.Flag = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", new Point(240, 8), "__Flag", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("pays_" + pi.hiddenVillage + "_thumbs", 0));
            MenuStats.StatsImg.Child.Add(MenuStats.Flag);
            MenuStats.LFlag.Text = hiddenVillage.ToString();
            MenuStats.Fusion1.Text = CommonCode.TranslateText(75);
            MenuStats.Fusion2.Text = CommonCode.TranslateText(75);
            MenuStats.NiveauGaugeTxt.Text = CommonCode.TranslateText(50) + " " + pi.level;

            // NiveauGaugeRecPercent, barre de progression du niveau
            // calcule du pourcentage du niveau en progression
            int CurrentProgressLevel = pi.currentXp;
            int TotalProgressLevel = pi.maxXp;
            int PercentProgressLevel = 0;

            if (TotalProgressLevel != 0)
                PercentProgressLevel = (CurrentProgressLevel * 100) / TotalProgressLevel;
            else
                PercentProgressLevel = 100;

            MenuStats.NiveauGaugeRecPercent.size.Width = (258 * PercentProgressLevel) / 100;

            // affichage du label progression lvl
            MenuStats.NiveauGaugeTxtCurrent.Text = CurrentProgressLevel + "/" + TotalProgressLevel + " (" + PercentProgressLevel + "%)";
            MenuStats.NiveauGaugeTxtCurrent.point = new Point(MenuStats.NiveauGaugeRec2.point.X + (MenuStats.NiveauGaugeRec2.size.Width / 2) - (TextRenderer.MeasureText(MenuStats.NiveauGaugeTxtCurrent.Text, MenuStats.NiveauGaugeTxtCurrent.font).Width / 2), MenuStats.NiveauGaugeRec2.point.Y);

            // raffrechissement du text pour des mesures de changement de langue
            MenuStats.AffiniteElementaireTxt.Text = CommonCode.TranslateText(76);
            MenuStats.terreStats.Text = "(" + CommonCode.TranslateText(77) + ")";
            MenuStats.FeuStats.Text = "(" + CommonCode.TranslateText(78) + ")";
            MenuStats.VentStats.Text = "(" + CommonCode.TranslateText(79) + ")";
            MenuStats.FoudreStats.Text = "(" + CommonCode.TranslateText(80) + ")";
            MenuStats.EauStats.Text = "(" + CommonCode.TranslateText(81) + ")";

            MenuStats.TerrePuissance.Text = "(" + pi.doton + "+" + pi.equipedDoton + ")=" + (pi.doton + pi.equipedDoton);
            MenuStats.FeuPuissance.Text = "(" + pi.katon + "+" + pi.equipedKaton + ")=" + (pi.katon + pi.equipedKaton);
            MenuStats.VentPuissance.Text = "(" + pi.futon + "+" + pi.equipedFuton + ")=" + (pi.futon + pi.equipedFuton);
            MenuStats.FoudrePuissance.Text = "(" + pi.raiton + "+" + pi.equipedRaiton + ")=" + (pi.raiton + pi.equipedRaiton);
            MenuStats.EauPuissance.Text = "(" + pi.suiton + "+" + pi.equipedSuiton + ")=" + (pi.suiton + pi.equipedSuiton);

            MenuStats.Lvl1RegleTxt.Text = CommonCode.TranslateText(82);
            MenuStats.Lvl2RegleTxt.Text = CommonCode.TranslateText(83);
            MenuStats.Lvl3RegleTxt.Text = CommonCode.TranslateText(84);
            MenuStats.Lvl4RegleTxt.Text = CommonCode.TranslateText(85);
            MenuStats.Lvl5RegleTxt.Text = CommonCode.TranslateText(86);
            MenuStats.Lvl6RegleTxt.Text = CommonCode.TranslateText(87);

            // affichage de la gauge lvl chakra selon les points
            MenuStats.Lvl2ReglePts.Text = CommonCode.chakra1Level.ToString();
            MenuStats.Lvl3ReglePts.Text = CommonCode.chakra2Level.ToString();
            MenuStats.Lvl4ReglePts.Text = CommonCode.chakra3Level.ToString();
            MenuStats.Lvl5ReglePts.Text = CommonCode.chakra4Level.ToString();
            MenuStats.Lvl6ReglePts.Text = CommonCode.chakra5Level.ToString();

            // modification du lvl de l'utilisation de l'element
            CommonCode.UpdateUsingElement(Enums.Chakra.Element.doton, pi.usingDoton);
            CommonCode.UpdateUsingElement(Enums.Chakra.Element.katon, pi.usingKaton);
            CommonCode.UpdateUsingElement(Enums.Chakra.Element.futon, pi.usingFuton);
            CommonCode.UpdateUsingElement(Enums.Chakra.Element.raiton, pi.usingRaiton);
            CommonCode.UpdateUsingElement(Enums.Chakra.Element.suiton, pi.equipedSuiton);

            MenuStats.DotonLvl.Text = pi.usingDoton.ToString();
            MenuStats.KatonLvl.Text = pi.usingKaton.ToString();
            MenuStats.FutonLvl.Text = pi.usingFuton.ToString();
            MenuStats.RaitonLvl.Text = pi.usingRaiton.ToString();
            MenuStats.SuitonLvl.Text = pi.usingSuiton.ToString();

            // bar de vie selon les pdv 11 current, 12 total
            int TotalPdv = pi.maxHealth;
            int CurrentPdv = pi.currentHealth;
            int X = 0;
            if (TotalPdv != 0)
                X = (CurrentPdv * 100) / TotalPdv;
            MenuStats.VieBar.size.Width = (236 * X) / 100;

            // point de vie dans Menustats
            MenuStats.VieLabel.Text = CommonCode.TranslateText(88);
            MenuStats.ViePts.Text = CurrentPdv.ToString() + " / " + TotalPdv + " (" + X + "%)";
            //MenuStats.PCLabel.Text = common1.TranslateText(89);
            MenuStats.PC.Text = pi.originalPc.ToString();
            //MenuStats.PMLabel.Text = common1.TranslateText(90);
            MenuStats.PM.Text = pi.originalPm.ToString();
            //MenuStats.PELabel.Text = common1.TranslateText(91);
            MenuStats.PE.Text = pi.pe.ToString();
            //MenuStats.CDLabel.Text = common1.TranslateText(92);
            MenuStats.CD.Text = pi.cd.ToString();
            //MenuStats.InvocLabel.Text = common1.TranslateText(93);
            MenuStats.Invoc.Text = pi.summons.ToString();
            //MenuStats.InitiativeLabel.Text = common1.TranslateText(94);
            MenuStats.Initiative.Text = pi.initiative.ToString();
            MenuStats.Job1Label.Text = CommonCode.TranslateText(95) + " 1";
            MenuStats.Specialite1Label.Text = CommonCode.TranslateText(96) + " 1";
            MenuStats.Job2Labe1.Text = CommonCode.TranslateText(95) + " 2";
            MenuStats.Specialite2Label.Text = CommonCode.TranslateText(96) + " 2";
            //////// playerData[41] = job1
            //////// playerdata[42] = job2
            //////// playerdata[43] = specialite1
            //////// playerdata[44] = specialite2
            MenuStats.PoidLabel.Text = CommonCode.TranslateText(97);
            int TotalPoid = pi.maxWeight;
            int CurrentPoid = pi.currentWeight;
            int PercentPoid = (CurrentPoid * 100) / TotalPoid;
            MenuStats.PoidRec.size.Width = (116 * PercentPoid) / 100;
            MenuStats.Poid.Text = CurrentPoid + " / " + TotalPoid + " (" + PercentPoid + "%)";
            MenuStats.Poid.point.X = MenuStats.PoidRec.point.X + 58 - (TextRenderer.MeasureText(MenuStats.Poid.Text, MenuStats.Poid.font).Width / 2);
            MenuStats.Ryo.Text = CommonCode.MoneyThousendSeparation(pi.ryo.ToString());
            MenuStats.resiDotonTxt.Text = pi.resiDotonPercent.ToString() + "%";
            MenuStats.resiKatonTxt.Text = pi.resiKatonPercent.ToString() + "%";
            MenuStats.resiFutonTxt.Text = pi.resiFutonPercent.ToString() + "%";
            MenuStats.resiRaitonTxt.Text = pi.resiRaitonPercent.ToString() + "%";
            MenuStats.resiSuitonTxt.Text = pi.resiSuitonPercent.ToString() + "%";
            MenuStats.__esquivePC_Txt.Text = pi.dodgePc.ToString();
            MenuStats.__esquivePM_Txt.Text = pi.dodgePm.ToString();
            MenuStats.__retraitPC_Txt.Text = pi.removePc.ToString();
            MenuStats.__retraitPM_Txt.Text = pi.removePm.ToString();

            pi.spellPointLeft = spellPointLeft;
            #endregion
        }
    }
}
