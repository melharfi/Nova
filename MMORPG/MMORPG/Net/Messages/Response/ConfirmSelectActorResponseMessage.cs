using System;
using System.Drawing;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class ConfirmSelectActorResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            // il faut voir le rawdata reçu si ca correspond vraiment a la longeur demandé et associé au variables ?
            #region reception des données du joueur qui a été selectionné dans la liste des joueurs
            /*_actor.Pseudo + "#"_actor.ClasseName + "#" + _actor.Spirit + "#" + _actor.SpiritLvl +
                             "#" + _actor.Pvp + "#" + _actor.village + "#" + _actor.MaskColors + "#" +
                             _actor.Orientation + "#" + _actor.Level + "#" + _actor.map + "#" + _actor.rang + "#" +
                             _actor.currentHealth + "#" + _actor.totalHealth + "#" + _actor.xp + "#" + totalXp + "#" +
                             _actor.doton + "#" + _actor.katon + "#" + _actor.futon + "#" + _actor.raiton + "#" +
                             _actor.suiton + "#" + MainClass.chakralvl2 + "#" + MainClass.chakralvl3 + "#" +
                             MainClass.chakralvl4 + "#" + MainClass.chakralvl5 + "#" + MainClass.chakralvl6 + "#" +
                             _actor.usingDoton + "#" + _actor.usingKaton + "#" + _actor.usingFuton + "#" +
                             _actor.usingRaiton + "#" + _actor.usingSuiton + "#" + _actor.equipedDoton + "#" +
                             _actor.equipedKaton + "#" + _actor.equipedFuton + "#" + _actor.equipedRaiton + "#" +
                             _actor.suitonEquiped + "#" + _actor.original_Pc + "#" + _actor.original_Pm + "#" +
                             _actor.pe + "#" + _actor.cd + "#" + _actor.invoc + "#" + _actor.Initiative + "#" +
                             _actor.job1 + "#" + _actor.job2 + "#" + _actor.specialite1 + "#" + _actor.specialite2 + "#" +
                             _actor.TotalPoid + "#" + _actor.CurrentPoid + "#" + _actor.Ryo + "#" +
                             _actor.resiDotonPercent + "#" + _actor.resiKatonPercent + "#" + _actor.resiFutonPercent +
                             "#" + _actor.resiRaitonPercent + "#" + _actor.resiSuitonPercent + "#" + _actor.dodgePC +
                             "#" + _actor.dodgePM + "#" + _actor.dodgePE + "#" + _actor.dodgeCD + "#" + _actor.removePC +
                             "#" + _actor.removePM + "#" + _actor.removePE + "#" + _actor.removeCD + "#" + _actor.escape +
                             "#" + _actor.blocage + "#" + _sorts + "#" + _actor.resiDotonFix + "#" + _actor.resiKatonFix +
                             "#" + _actor.resiFutonFix + "#" + _actor.resiRaitonFix + "#" + _actor.resiSuitonFix + "#" +
                             _actor.resiFix + "#" + _actor.domDotonFix + "#" + _actor.domKatonFix + "#" +
                             _actor.domFutonFix + "#" + _actor.domRaitonFix + "#" + _actor.domSuitonFix + "#" +
                             _actor.domFix + "#" + _actor.power + "#" + _actor.powerEquiped + "•" + quete + "•" +
                             _actor.spellPointLeft;*/

            string[] states = commandStrings[1].Split('#');

            string actorName = states[0];
            Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
            Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit.Name), states[2]);
            int spiritLevel = int.Parse(states[3]);
            bool pvpEnabled = bool.Parse(states[4]);
            Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[5]);
            string maskColorsString = states[6];
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
            int chakra4Level =  int.Parse(states[23]);
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
            int pc = int.Parse(states[35]);
            int pm = int.Parse(states[36]);
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
            string[] spellsString = states[63].Split('/');
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
            string questString = commandStrings[2];
            int spellPointsLeft = int.Parse(commandStrings[3]);

            HudHandle.DrawHud();

            new System.Threading.Thread(() =>
            {
                #region on affiche une fenetre de chargement
                // compteur pour voir si le temps passé est sufisant pour voir le chargement, si non on oblige le joueur a patienter le reste du temps détérminé pour l'animation qui est de 300 miliseconds
                Benchmark.Start();

                Bmp loadingParent = new Bmp(@"gfx\general\artwork\loading\naruto.dat", new Point(0, 0),
                    "__loadingParent", Manager.TypeGfx.Bgr, true, 1)
                { zindex = 100 };
                Manager.manager.GfxTopList.Add(loadingParent);

                // barre de chargement
                Bmp loadingGif = new Bmp(@"gfx\general\obj\1\loading1.dat", Point.Empty, "__loadingGif", Manager.TypeGfx.Bgr, true);
                loadingGif.point = new Point((ScreenManager.WindowWidth / 2) - (loadingGif.rectangle.Width / 2), ScreenManager.WindowHeight - 50);
                loadingParent.Child.Add(loadingGif);

                Txt loadingLabel = new Txt(CommonCode.TranslateText(187), Point.Empty, "__loadingLabel", Manager.TypeGfx.Top, true, new Font("Verdana", 10, FontStyle.Bold), Brushes.White);
                loadingLabel.point = new Point((ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(loadingLabel.Text, loadingLabel.font).Width / 2), 610);
                loadingParent.Child.Add(loadingLabel);

                Anim loadingSystemStart = new Anim(15, 1);
                for (int cnt = 0; cnt < 15; cnt++)
                    loadingSystemStart.AddCell(@"gfx\general\obj\1\LoadingSystem.dat", 0, 300 + (cnt * 12), 550, 25 + (cnt * 2), 25 + (cnt * 2), 0.1F * (Convert.ToSingle(cnt)), 15);
                loadingSystemStart.AddCell(@"gfx\general\obj\1\LoadingSystem.dat", 0, 300 + (15 * 12), 550, 25 + (15 * 2), 25 + (15 * 2), 0.1F * (Convert.ToSingle(15)), 15);
                loadingSystemStart.Ini(Manager.TypeGfx.Top, "__LoadingSystemStart", true);
                loadingSystemStart.AutoResetAnim = false;
                loadingSystemStart.Start();
                loadingParent.Child.Add(loadingSystemStart);

                /////////////////////////////////////////////////////////////
                CommonCode.MyPlayerInfo.instance.ibPlayer = new Bmp { tag = new Actor() };
                Actor actor = (Actor) CommonCode.MyPlayerInfo.instance.ibPlayer.tag;
                actor.pseudo = actorName;
                actor.className = className;
                actor.spirit = spirit;
                actor.spiritLevel = spiritLevel;
                actor.pvpEnabled = pvpEnabled;
                actor.hiddenVillage = hiddenVillage;
                actor.maskColorString = maskColorsString;
                actor.directionLook = orientation;
                actor.level = level;
                actor.map = map;
                actor.officialRang = officialRang;
                actor.currentHealth = currentHealth;
                actor.maxHealth = maxHealth;
                actor.currentXp = currentXp;
                actor.maxXp = maxXp;
                actor.doton = doton;
                actor.katon = katon;
                actor.futon = futon;
                actor.raiton = raiton;
                actor.suiton = suiton;

                // association des données des chakralvl2,3,4,5
                CommonCode.chakra1Level = chakra1Level;
                CommonCode.chakra2Level = chakra2Level;
                CommonCode.chakra3Level = chakra3Level;
                CommonCode.chakra4Level = chakra4Level;
                CommonCode.chakra5Level = chakra5Level;

                actor.usingDoton = usingDoton;
                actor.usingKaton = usingKaton;
                actor.usingFuton = usingFuton;
                actor.usingRaiton = usingRaiton;
                actor.usingSuiton = usingSuiton;
                actor.equipedDoton = equipedDoton;
                actor.equipedKaton = equipedKaton;
                actor.equipedFuton = equipedFuton;
                actor.equipedRaiton = equipedRaiton;
                actor.equipedSuiton = equipedSuiton;
                actor.originalPc = pc;
                actor.originalPm = pm;
                actor.pe = pe;
                actor.cd = cd;
                actor.summons = summons;
                actor.initiative = initiative;
                actor.job1 = job1;
                actor.job2 = job2;
                actor.specialty1 = specialty1;
                actor.specialty2 = specialty2;
                actor.maxWeight = maxWeight;
                actor.currentWeight = currentWeight;
                actor.ryo = ryo;
                actor.resiDotonPercent = resiDotonPercent;
                actor.resiKatonPercent = resiKatonPercent;
                actor.resiFutonPercent = resiFutonPercent;
                actor.resiRaitonPercent = resiRaitonPercent;
                actor.resiSuitonPercent = resiSuitonPercent;
                actor.dodgePc = dodgePc;
                actor.dodgePm = dodgePm;
                actor.dodgePe = dodgePe;
                actor.dodgeCd = dodgeCd;
                actor.removePc = removePc;
                actor.removePm = removePm;
                actor.removePe = removePe;
                actor.removeCd = removeCd;
                actor.escape = escape;
                actor.blocage = blocage;

                if (spellsString.Length > 0)
                {
                    foreach (string t in spellsString)
                    {
                        Actor.SpellsInformations infoSorts = new Actor.SpellsInformations
                        {
                            sortID = Convert.ToInt32(t.Split(':')[0]),
                            emplacement = Convert.ToInt32(t.Split(':')[1]),
                            level = Convert.ToInt32(t.Split(':')[2]),
                            colorSort = Convert.ToInt32(t.Split(':')[3])
                        };
                        actor.spells.Add(infoSorts);
                    }
                }
                actor.resiDotonFix = resiDotonFix;
                actor.resiKatonFix = resiKatonFix;
                actor.resiFutonFix = resiFutonFix;
                actor.resiRaitonFix = resiRaitonFix;
                actor.resiSuitonFix = resiSuitonFix;
                actor.resiFix = resiFix;

                // supression des sorts s'il sont déja été affiché avant
                if (HudHandle.all_sorts.Child.Count > 0)
                    HudHandle.all_sorts.Child.Clear();

                // affichage des sorts
                foreach (Actor.SpellsInformations t in actor.spells)
                {
                    Bmp spell = new Bmp(@"gfx\general\obj\1\spells.dat",
                        new Point(spells.spellPositions[t.emplacement].X, spells.spellPositions[t.emplacement].Y),
                        "__spell", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet(t.sortID + "_spell", 0))
                    {
                        tag = t
                    };
                    // attachement des infos du sort au tag de l'image
                    spell.MouseMove += MMORPG.Battle.__spell_MouseMove;
                    spell.MouseOut += CommonCode.CursorDefault_MouseOut;
                    spell.MouseClic += MMORPG.Battle.__spell_MouseClic;
                    HudHandle.all_sorts.Child.Add(spell);
                }

                actor.domDotonFix = domDotonFix;
                actor.domKatonFix = domKatonFix;
                actor.domFutonFix = domFutonFix;
                actor.domRaitonFix = domRaitonFix;
                actor.domSuitonFix = domSuitonFix;
                actor.domFix = domFix;
                actor.power = power;
                actor.equipedPower = equipedPower;
                CommonCode.CurMap = actor.map;

                Benchmark.End();
                System.Threading.Thread.Sleep(225);
                loadingLabel.Text = CommonCode.TranslateText(188);
                loadingLabel.point = new Point((ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(loadingLabel.Text, loadingLabel.font).Width / 2), 610);

                ////////////////// mode designe
                loadingSystemStart.Visible(false);
                loadingParent.Child.Remove(loadingSystemStart);

                Anim loadingSystemEnd = new Anim(15, 1);
                for (int cnt = 0; cnt < 15; cnt++)
                    loadingSystemEnd.AddCell(@"gfx\general\obj\1\LoadingSystem.dat", 0, loadingSystemStart.img.point.X + (cnt * 12), 550, 25 + ((15 - cnt) * 2), 25 + ((15 - cnt) * 2), 0.1F * (Convert.ToSingle(15 - cnt)), 15);

                loadingSystemEnd.Ini(Manager.TypeGfx.Top, "__LoadingSystemEnd", true);
                loadingSystemEnd.AutoResetAnim = false;
                loadingSystemEnd.Start();
                loadingParent.Child.Add(loadingSystemEnd);

                Anim loadingGfxStart = new Anim(15, 1);
                for (int cnt = 0; cnt < 15; cnt++)
                    loadingGfxStart.AddCell(@"gfx\general\obj\1\GrayPaletteColor.dat", 0, 300 + (cnt * 12), 550, 25 + (cnt * 2), 25 + (cnt * 2), 0.1F * (Convert.ToSingle(cnt)), 15);
                loadingGfxStart.AddCell(@"gfx\general\obj\1\paletteColor.dat", 0, 300 + (15 * 12), 550, 25 + (15 * 2), 25 + (15 * 2), 0.1F * (Convert.ToSingle(15)), 15);
                loadingGfxStart.Ini(Manager.TypeGfx.Top, "__paletteColor", true);
                loadingGfxStart.AutoResetAnim = false;
                loadingGfxStart.Start();
                loadingParent.Child.Add(loadingGfxStart);

                loadingLabel.Text = CommonCode.TranslateText(189);
                loadingLabel.point = new Point((ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(loadingLabel.Text, loadingLabel.font).Width / 2), 610);

                ///////////////// affichage des composants du tableau stats
                // affichage de l'avatar
                MenuStats.ThumbsAvatar = new Bmp(@"gfx\general\classes\" + actor.className + ".dat", new Point(15, 10),
                    "ThumbsAvatar", Manager.TypeGfx.Top, true, 1,
                    SpriteSheet.GetSpriteSheet("avatar_" + actor.className, 0))
                {
                    tag = CommonCode.MyPlayerInfo.instance.ibPlayer.tag
                };
                CommonCode.ApplyMaskColorToClasse(MenuStats.ThumbsAvatar);
                MenuStats.StatsImg.Child.Add(MenuStats.ThumbsAvatar);

                // affichage du nom du personnage
                MenuStats.StatsPlayerName.Text = actor.pseudo[0].ToString().ToUpper() + actor.pseudo.Substring(1, actor.pseudo.Length - 1);

                // level
                MenuStats.StatsLevel.Text = CommonCode.TranslateText(50) + " " + actor.level;

                // affichage du rang général
                MenuStats.Rang.Text = CommonCode.officialRangToCurrentLangTranslation(actor.officialRang);

                // affichage du level Pvp
                MenuStats.LevelPvp.Text = actor.spiritLevel.ToString();

                // affichage du grade Pvp
                if (spirit != Enums.Spirit.Name.neutral)
                {
                    MenuStats.GradePvp = new Bmp(@"gfx\general\obj\2\" + actor.spirit + @"\" + MenuStats.LevelPvp.Text + ".dat", new Point(276 + (15 - Convert.ToInt16(MenuStats.LevelPvp.Text)), 2), new Size(40 + Convert.ToInt16(MenuStats.LevelPvp.Text), 20 + Convert.ToInt16(MenuStats.LevelPvp.Text)), "PlayerStats." + actor.spirit, Manager.TypeGfx.Top, true, 1);
                    MenuStats.StatsImg.Child.Add(MenuStats.GradePvp);
                }

                // update des pdv,Pc
                HudHandle.UpdateHealth();
                HudHandle.UpdatePc();
                HudHandle.UpdatePm();

                MenuStats.Flag = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", new Point(240, 8), "__Flag", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("pays_" + actor.hiddenVillage + "_thumbs", 0));
                MenuStats.StatsImg.Child.Add(MenuStats.Flag);
                MenuStats.LFlag.Text = hiddenVillage.ToString();
                MenuStats.Fusion1.Text = CommonCode.TranslateText(75);
                MenuStats.Fusion2.Text = CommonCode.TranslateText(75);
                MenuStats.NiveauGaugeTxt.Text = CommonCode.TranslateText(50) + " " + actor.level;

                // NiveauGaugeRecPercent, barre de progression du niveau
                // calcule du pourcentage du niveau en progression
                int currentProgressLevel = actor.currentXp;
                int totalProgressLevel = actor.maxXp;
                int percentProgressLevel;

                if (totalProgressLevel != 0)
                    percentProgressLevel = (currentProgressLevel * 100) / totalProgressLevel;
                else
                    percentProgressLevel = 100;

                MenuStats.NiveauGaugeRecPercent.size.Width = (258 * percentProgressLevel) / 100;

                // affichage du label progression lvl
                MenuStats.NiveauGaugeTxtCurrent.Text = currentProgressLevel + "/" + totalProgressLevel + " (" + percentProgressLevel + "%)";
                MenuStats.NiveauGaugeTxtCurrent.point = new Point(MenuStats.NiveauGaugeRec2.point.X + (MenuStats.NiveauGaugeRec2.size.Width / 2) - (TextRenderer.MeasureText(MenuStats.NiveauGaugeTxtCurrent.Text, MenuStats.NiveauGaugeTxtCurrent.font).Width / 2), MenuStats.NiveauGaugeRec2.point.Y);

                // raffrechissement du text pour des mesures de changement de langue
                MenuStats.AffiniteElementaireTxt.Text = CommonCode.TranslateText(76);
                MenuStats.terreStats.Text = "(" + CommonCode.TranslateText(77) + ")";
                MenuStats.FeuStats.Text = "(" + CommonCode.TranslateText(78) + ")";
                MenuStats.VentStats.Text = "(" + CommonCode.TranslateText(79) + ")";
                MenuStats.FoudreStats.Text = "(" + CommonCode.TranslateText(80) + ")";
                MenuStats.EauStats.Text = "(" + CommonCode.TranslateText(81) + ")";

                MenuStats.TerrePuissance.Text = "(" + actor.doton + "+" + actor.equipedDoton + ")=" + (actor.doton + actor.equipedDoton);
                MenuStats.FeuPuissance.Text = "(" + actor.katon + "+" + actor.equipedKaton + ")=" + (actor.katon + actor.equipedKaton);
                MenuStats.VentPuissance.Text = "(" + actor.futon + "+" + actor.equipedFuton + ")=" + (actor.futon + actor.equipedFuton);
                MenuStats.FoudrePuissance.Text = "(" + actor.raiton + "+" + actor.equipedRaiton + ")=" + (actor.raiton + actor.equipedRaiton);
                MenuStats.EauPuissance.Text = "(" + actor.suiton + "+" + actor.equipedSuiton + ")=" + (actor.suiton + actor.equipedSuiton);

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
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.doton, actor.usingDoton);
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.katon, actor.usingKaton);
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.futon, actor.usingFuton);
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.raiton, actor.usingRaiton);
                CommonCode.UpdateUsingElement(Enums.Chakra.Element.suiton, actor.equipedSuiton);

                MenuStats.DotonLvl.Text = actor.usingDoton.ToString();
                MenuStats.KatonLvl.Text = actor.usingKaton.ToString();
                MenuStats.FutonLvl.Text = actor.usingFuton.ToString();
                MenuStats.RaitonLvl.Text = actor.usingRaiton.ToString();
                MenuStats.SuitonLvl.Text = actor.usingSuiton.ToString();

                // bar de vie selon les pdv 11 current, 12 total
                int totalPdv = actor.maxHealth;
                int currentPdv = actor.currentHealth;
                int x = 0;
                if (totalPdv != 0)
                    x = (currentPdv * 100) / totalPdv;
                MenuStats.VieBar.size.Width = (236 * x) / 100;

                // point de vie dans Menustats
                MenuStats.VieLabel.Text = CommonCode.TranslateText(88);
                MenuStats.ViePts.Text = currentPdv + " / " + totalPdv + " (" + x + "%)";
                MenuStats.PC.Text = actor.originalPc.ToString();
                MenuStats.PM.Text = actor.originalPm.ToString();
                MenuStats.PE.Text = actor.pe.ToString();
                MenuStats.CD.Text = actor.cd.ToString();
                MenuStats.Invoc.Text = actor.summons.ToString();
                MenuStats.Initiative.Text = actor.initiative.ToString();
                MenuStats.Puissance.Text = (actor.power + actor.equipedPower).ToString();
                MenuStats.DomFix.Text = actor.domFix.ToString();
                MenuStats.Job1Label.Text = CommonCode.TranslateText(95) + " 1";
                MenuStats.Specialite1Label.Text = CommonCode.TranslateText(96) + " 1";
                MenuStats.Job2Labe1.Text = CommonCode.TranslateText(95) + " 2";
                MenuStats.Specialite2Label.Text = CommonCode.TranslateText(96) + " 2";
                //////// playerData[41] = job1
                //////// playerdata[42] = job2
                //////// playerdata[43] = specialite1
                //////// playerdata[44] = specialite2
                MenuStats.PoidLabel.Text = CommonCode.TranslateText(97);
                int totalPoid = actor.maxWeight;
                int currentPoid = actor.currentWeight;
                int percentPoid = (currentPoid * 100) / totalPoid;
                MenuStats.PoidRec.size.Width = (116 * percentPoid) / 100;
                MenuStats.Poid.Text = currentPoid + " / " + totalPoid + " (" + percentPoid + "%)";
                MenuStats.Poid.point.X = MenuStats.PoidRec.point.X + 58 - (TextRenderer.MeasureText(MenuStats.Poid.Text, MenuStats.Poid.font).Width / 2);
                MenuStats.Ryo.Text = CommonCode.MoneyThousendSeparation(actor.ryo.ToString());
                MenuStats.resiDotonTxt.Text = actor.resiDotonPercent + "%";
                MenuStats.resiKatonTxt.Text = actor.resiKatonPercent + "%";
                MenuStats.resiFutonTxt.Text = actor.resiFutonPercent + "%";
                MenuStats.resiRaitonTxt.Text = actor.resiRaitonPercent + "%";
                MenuStats.resiSuitonTxt.Text = actor.resiSuitonPercent + "%";
                MenuStats.__esquivePC_Txt.Text = actor.dodgePc.ToString();
                MenuStats.__esquivePM_Txt.Text = actor.dodgePm.ToString();
                MenuStats.__retraitPC_Txt.Text = actor.removePc.ToString();
                MenuStats.__retraitPM_Txt.Text = actor.removePm.ToString();

                // quete
                // convertir la list de quete en string
                if (questString != "")
                    for (int cnt = 0; cnt < questString.Split('/').Length; cnt++)
                    {
                        string quest = questString.Split('/')[cnt];
                        Actor.QuestInformations qi = new Actor.QuestInformations
                        {
                            nom_quete = quest.Split(':')[0],
                            totalSteps = Convert.ToInt16(quest.Split(':')[1]),
                            currentStep = Convert.ToInt16(quest.Split(':')[2]),
                            submited = Convert.ToBoolean(quest.Split(':')[3])
                        };
                        actor.Quests.Add(qi);
                    }

                // verification si le joueur été en combat pour le rediriger vers ce joueur
                /*if (commandStrings[3] == "inBattle")
                {
                    common1.MyPlayerInfo.instance.pseudo = pi.pseudo;
                    common1.MyPlayerInfo.instance.Event = "inBattle";
                }*/

                actor.spellPointLeft = spellPointsLeft;

                ///// effacement du menu loading
                loadingLabel.Text = CommonCode.TranslateText(190);
                loadingLabel.point = new Point((ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(loadingLabel.Text, loadingLabel.font).Width / 2), 610);
                System.Threading.Thread.Sleep(2000);

                // changement de map
                Manager.manager.mainForm.BeginInvoke((Action)(() =>
                {
                    CommonCode.ChangeMap(actor.map);  // affichage du hud
                    Manager.manager.GfxTopList.Remove(loadingParent);       // enlever l'image d'avant plant qui empeche de voir la map
                    MainForm.chatBox.Show();
                    HudHandle.HudVisibility(true);
                    MainForm.drawSpellStatesMenuOnce();
                    HudHandle.chatBoxRefreshHandler();
                    HudHandle.recalibrateChatBoxPosition();
                }));
                #endregion
            }).Start();
            #endregion
        }
    }
}
