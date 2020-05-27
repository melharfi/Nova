using MELHARFI;
using MELHARFI.Gfx;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MMORPG.Net.Messages.Response
{
    internal class AutoSelectActorInBattleResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            /*SelectActorGrantedResponseMessage•
            string buffer = actor.Pseudo + "#" + actor.ClasseName + "#" + actor.Spirit + "#" +
            actor.SpiritLvl.ToString() + "#" + actor.Pvp.ToString() + "#" + actor.village + "#" + actor.MaskColors + "#" +
            actor.Orientation.ToString() + "#" + actor.Level.ToString() + "#" + actor.map + "#" + actor.rang.ToString() +
            "#" + actor.currentHealth.ToString() + "#" + actor.totalHealth.ToString() + "#" + actor.xp.ToString() +
            "#" + totalXp + "#" + actor.doton.ToString() + "#" + actor.katon.ToString() + "#" +
            actor.futon.ToString() + "#" + actor.raiton.ToString() + "#" + actor.suiton.ToString() + "#" +
            MainClass.chakralvl2 + "#" + MainClass.chakralvl3 + "#" + MainClass.chakralvl4 + "#" +
            MainClass.chakralvl5 + "#" + MainClass.chakralvl6 + "#" + actor.usingDoton.ToString() + "#" +
            actor.usingKaton.ToString() + "#" + actor.usingFuton.ToString() + "#" + actor.usingRaiton.ToString() +
            "#" + actor.usingSuiton.ToString() + "#" + actor.equipedDoton.ToString() + "#" +
            actor.equipedKaton.ToString() + "#" + actor.equipedFuton.ToString() + "#" +
            actor.equipedRaiton.ToString() + "#" + actor.suitonEquiped.ToString() + "#" +
            actor.original_Pc.ToString() + "#" + actor.original_Pm.ToString() + "#" + actor.pe.ToString() + "#" +
            actor.cd.ToString() + "#" + actor.invoc.ToString() + "#" + actor.Initiative.ToString() + "#" +
            actor.job1 + "#" + actor.job2 + "#" + actor.specialite1 + "#" +
            actor.specialite2 + "#" + actor.TotalPoid.ToString() + "#" + actor.CurrentPoid.ToString() +
            "#" + actor.Ryo.ToString() + "#" + actor.resiDotonPercent.ToString() + "#" +
            actor.resiKatonPercent.ToString() + "#" + actor.resiFutonPercent.ToString() + "#" +
            actor.resiRaitonPercent.ToString() + "#" + actor.resiSuitonPercent.ToString() + "#" +
            actor.dodgePC.ToString() + "#" + actor.dodgePM.ToString() + "#" + actor.dodgePE.ToString() + "#" +
            actor.dodgeCD.ToString() + "#" + actor.removePC.ToString() + "#" + actor.removePM.ToString() + "#" +
            actor.removePE.ToString() + "#" + actor.removeCD.ToString() + "#" + actor.escape.ToString() + "#" +
            actor.blocage.ToString() + "#" + _sorts + "#" + actor.resiDotonFix + "#" + actor.resiKatonFix + "#" +
            actor.resiFutonFix + "#" + actor.resiRaitonFix + "#" + actor.resiSuitonFix + "#" + actor.resiFix + "#" +
            actor.domDotonFix + "#" + actor.domKatonFix + "#" + actor.domFutonFix + "#" + actor.domRaitonFix + "#" +
            actor.domSuitonFix + "#" + actor.domFix + "#" + actor.power + "#" + actor.powerEquiped + 
            "•" + quete +
            "•inBattle•" 
            + actor.spellPointLeft;
            */
            #region reception des données du joueur qui a été selectionné dans la liste des joueurs

            // cmd[2] = Pseudo#ClasseName#Spirit#SpiritLvl#Pvp#village#MaskColors#Orientation#Level#map#rang#CurrentPdv#totalPdv#CurrentXP#TotalXp#doton#katon#futon#raiton#suiton#chakralvl2#chakralvl3#chakralvl4#chakralvl5#chakralvl6#usingDoton#usingKaton#usingFuton#usingRaiton#usingSuiton#equipedDoton#equipedKaton#equipedFuton#equipedRaiton#suitonEquiped#pc#pm#pe#cd#invoc#Initiative#job1#job2#specialite1#specialite2#TotalPoid#CurrentPoid#Ryo#resiDoton#resiKaton#resiFuton#resiRaiton#resiSuiton#evasion#blockage#sorts#resiDotonFix#resiKatonFix#resiFutonFix#resiRaitonFix#resiSuitonFix#resiFix#domDotonFix#domKatonFix#domFutonFix#domRaitonFix#domSuitonFix#domFix#puissance#puissanceEquiped
            string[] playerData = commandStrings[1].Split('#');

            string actorName = playerData[0];
            Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), playerData[1]);
            Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit.Name), playerData[2]);
            int spiritLevel = int.Parse(playerData[3]);
            bool pvpEnabled = bool.Parse(playerData[4]);
            Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), playerData[5]);
            string maskColorsString = playerData[6];
            string[] maskColors = playerData[6].Split('/');
            int orientation = int.Parse(playerData[7]);
            int level = int.Parse(playerData[8]);
            string map = playerData[9];
            Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), playerData[10]);
            int currentHealth = int.Parse(playerData[11]);
            int maxHealth = int.Parse(playerData[12]);
            int currentXp = int.Parse(playerData[13]);
            int maxXp = int.Parse(playerData[14]);
            int doton = int.Parse(playerData[15]);
            int katon = int.Parse(playerData[16]);
            int futon = int.Parse(playerData[17]);
            int raiton = int.Parse(playerData[18]);
            int suiton = int.Parse(playerData[19]);
            int chakra1Level = int.Parse(playerData[20]);
            int chakra2Level = int.Parse(playerData[21]);
            int chakra3Level = int.Parse(playerData[22]);
            int chakra4Level = int.Parse(playerData[23]);
            int chakra5Level = int.Parse(playerData[24]);
            int usingDoton = int.Parse(playerData[25]);
            int usingKaton = int.Parse(playerData[26]);
            int usingFuton = int.Parse(playerData[27]);
            int usingRaiton = int.Parse(playerData[28]);
            int usingSuiton = int.Parse(playerData[29]);
            int equipedDoton = int.Parse(playerData[30]);
            int equipedKaton = int.Parse(playerData[31]);
            int equipedFuton = int.Parse(playerData[32]);
            int equipedRaiton = int.Parse(playerData[33]);
            int equipedSuiton = int.Parse(playerData[34]);
            int originalPc = int.Parse(playerData[35]);
            int originalPm = int.Parse(playerData[36]);
            int pe = int.Parse(playerData[37]);
            int cd = int.Parse(playerData[38]);
            int summon = int.Parse(playerData[39]);
            int initiative = int.Parse(playerData[40]);
            string job1 = playerData[41];
            string job2 = playerData[42];
            string specialty1 = playerData[43];
            string specialty2 = playerData[44];
            int maxWeight = int.Parse(playerData[45]);
            int currentWeight = int.Parse(playerData[46]);
            int ryo = int.Parse(playerData[47]);
            int resiDotonPercent = int.Parse(playerData[48]);
            int resiKatonPercent = int.Parse(playerData[49]);
            int resiFutonPercent = int.Parse(playerData[50]);
            int resiRaitonPercent = int.Parse(playerData[51]);
            int resiSuitonPercent = int.Parse(playerData[52]);
            int dodgePc = int.Parse(playerData[53]);
            int dodgePm = int.Parse(playerData[54]);
            int dodgePe = int.Parse(playerData[55]);
            int dodgeCd = int.Parse(playerData[56]);
            int removePc = int.Parse(playerData[57]);
            int removePm = int.Parse(playerData[58]);
            int removePe = int.Parse(playerData[59]);
            int removeCd = int.Parse(playerData[60]);
            int escape = int.Parse(playerData[61]);
            int blocage = int.Parse(playerData[62]);
            string spells = playerData[63];
            int resiDotonFix = int.Parse(playerData[64]);
            int resiKatonFix = int.Parse(playerData[65]);
            int resiFutonFix = int.Parse(playerData[66]);
            int resiRaitonFix = int.Parse(playerData[67]);
            int resiSuitonFix = int.Parse(playerData[68]);
            int resiFix = int.Parse(playerData[69]);
            int domDotonFix = int.Parse(playerData[70]);
            int domKatonFix = int.Parse(playerData[71]);
            int domFutonFix = int.Parse(playerData[72]);
            int domRaitonFix = int.Parse(playerData[73]);
            int domSuitonFix = int.Parse(playerData[74]);
            int domFix = int.Parse(playerData[75]);
            int power = int.Parse(playerData[76]);
            int equipedPower = int.Parse(playerData[77]);

            string quest = commandStrings[2];

            bool inBattle = bool.Parse(commandStrings[3]);      // ce variable est controlé dans chaque map "Start + _0_0_0" pour voir si le joueur est en combat, c'est au niveau des map que le system decoReco fonction, il faut penser a retirer ce systeme des map et de le mettre ici, voir map Start "if (CommonCode.MyPlayerInfo.instance.Event == "inBattle")" et map _0_0_0

            int spellPointLeft = int.Parse(commandStrings[4]);

            new System.Threading.Thread((() =>
            {
                // on affiche une fenetre de chargement
                // compteur pour voir si le temps passé est sufisant pour voir le chargement, si non on oblige le joueur a patienter le reste du temps détérminé pour l'animation qui est de 300 miliseconds
                Benchmark.Start();
                Bmp loadingParent = new Bmp(@"gfx\general\artwork\loading\" + className + ".dat", new Point(0, 0), "__loadingParent", Manager.TypeGfx.Bgr, true, 1);
                loadingParent.zindex = 100;
                Manager.manager.GfxTopList.Add(loadingParent);

                // barre de chargement
                Bmp loadingGif = new Bmp(@"gfx\general\obj\1\loading1.dat", Point.Empty, "__loadingGif", Manager.TypeGfx.Bgr, true);
                loadingGif.point = new Point((ScreenManager.WindowWidth / 2) - (loadingGif.rectangle.Width / 2), ScreenManager.WindowHeight - 50);
                loadingParent.Child.Add(loadingGif);

                Txt loadingLabel = new Txt(CommonCode.TranslateText(187), Point.Empty, "__loadingLabel", Manager.TypeGfx.Top, true, new Font("Verdana", 10, FontStyle.Bold), Brushes.White);
                loadingLabel.point = new Point((ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(loadingLabel.Text, loadingLabel.font).Width / 2), 610);
                loadingParent.Child.Add(loadingLabel);

                Anim LoadingSystemStart = new Anim(15, 1);
                for (int cnt = 0; cnt < 15; cnt++)
                    LoadingSystemStart.AddCell(@"gfx\general\obj\1\LoadingSystem.dat", 0, 300 + (cnt * 12), 550, 25 + (cnt * 2), 25 + (cnt * 2), 0.1F * (Convert.ToSingle(cnt)), 15);
                LoadingSystemStart.AddCell(@"gfx\general\obj\1\LoadingSystem.dat", 0, 300 + (15 * 12), 550, 25 + (15 * 2), 25 + (15 * 2), 0.1F * (Convert.ToSingle(15)), 15);
                LoadingSystemStart.Ini(Manager.TypeGfx.Top, "__LoadingSystemStart", true);
                LoadingSystemStart.AutoResetAnim = false;
                LoadingSystemStart.Start();
                loadingParent.Child.Add(LoadingSystemStart);

                /////////////////////////////////////////////////////////////
                CommonCode.MyPlayerInfo.instance.ibPlayer = new Bmp();
                CommonCode.MyPlayerInfo.instance.ibPlayer.tag = new Actor();
                Actor actor = (Actor)CommonCode.MyPlayerInfo.instance.ibPlayer.tag;
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
                actor.originalPc = originalPc;
                actor.originalPm = originalPm;
                actor.pe = pe;
                actor.cd = cd;
                actor.summons = summon;
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

                string _sorts = spells;
                if (_sorts != "")
                {
                    for (int cnt = 0; cnt < _sorts.Split('|').Length; cnt++)
                    {
                        string tmp_data = _sorts.Split('|')[cnt];
                        Actor.SpellsInformations _info_sorts = new Actor.SpellsInformations();
                        _info_sorts.sortID = Convert.ToInt32(tmp_data.Split(':')[0]);
                        _info_sorts.emplacement = Convert.ToInt32(tmp_data.Split(':')[1]);
                        _info_sorts.level = Convert.ToInt32(tmp_data.Split(':')[2]);
                        _info_sorts.colorSort = Convert.ToInt32(tmp_data.Split(':')[3]);
                        actor.spells.Add(_info_sorts);
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
                    Bmp __spell = new Bmp(@"gfx\general\obj\1\spells.dat", new Point(MMORPG.spells.spellPositions[t.emplacement].X, MMORPG.spells.spellPositions[t.emplacement].Y), "__spell", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet(t.sortID + "_spell", 0));
                    // attachement des infos du sort au tag de l'image
                    __spell.tag = t;
                    __spell.MouseMove += MMORPG.Battle.__spell_MouseMove;
                    __spell.MouseOut += CommonCode.CursorDefault_MouseOut;
                    __spell.MouseClic += MMORPG.Battle.__spell_MouseClic;
                    HudHandle.all_sorts.Child.Add(__spell);
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
                LoadingSystemStart.Visible(false);
                loadingParent.Child.Remove(LoadingSystemStart);

                Anim LoadingSystemEnd = new Anim(15, 1);
                for (int cnt = 0; cnt < 15; cnt++)
                    LoadingSystemEnd.AddCell(@"gfx\general\obj\1\LoadingSystem.dat", 0, LoadingSystemStart.img.point.X + (cnt * 12), 550, 25 + ((15 - cnt) * 2), 25 + ((15 - cnt) * 2), 0.1F * (Convert.ToSingle(15 - cnt)), 15);

                LoadingSystemEnd.Ini(Manager.TypeGfx.Top, "__LoadingSystemEnd", true);
                LoadingSystemEnd.AutoResetAnim = false;
                LoadingSystemEnd.Start();
                loadingParent.Child.Add(LoadingSystemEnd);

                Anim LoadingGfxStart = new Anim(15, 1);
                for (int cnt = 0; cnt < 15; cnt++)
                    LoadingGfxStart.AddCell(@"gfx\general\obj\1\GrayPaletteColor.dat", 0, 300 + (cnt * 12), 550, 25 + (cnt * 2), 25 + (cnt * 2), 0.1F * (Convert.ToSingle(cnt)), 15);
                LoadingGfxStart.AddCell(@"gfx\general\obj\1\paletteColor.dat", 0, 300 + (15 * 12), 550, 25 + (15 * 2), 25 + (15 * 2), 0.1F * (Convert.ToSingle(15)), 15);
                LoadingGfxStart.Ini(Manager.TypeGfx.Top, "__paletteColor", true);
                LoadingGfxStart.AutoResetAnim = false;
                LoadingGfxStart.Start();
                loadingParent.Child.Add(LoadingGfxStart);

                loadingLabel.Text = CommonCode.TranslateText(189);
                loadingLabel.point = new Point((ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(loadingLabel.Text, loadingLabel.font).Width / 2), 610);

                ///////////////// affichage des composants du tableau stats
                // affichage de l'avatar
                MenuStats.ThumbsAvatar = new Bmp(@"gfx\general\classes\" + actor.className + ".dat", new Point(15, 10), "ThumbsAvatar", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("avatar_" + actor.className, 0));
                MenuStats.ThumbsAvatar.tag = CommonCode.MyPlayerInfo.instance.ibPlayer.tag;
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
                MenuStats.LFlag.Text = playerData[5];
                MenuStats.Fusion1.Text = CommonCode.TranslateText(75);
                MenuStats.Fusion2.Text = CommonCode.TranslateText(75);
                MenuStats.NiveauGaugeTxt.Text = CommonCode.TranslateText(50) + " " + actor.level;

                // NiveauGaugeRecPercent, barre de progression du niveau
                // calcule du pourcentage du niveau en progression
                int CurrentProgressLevel = actor.currentXp;
                int TotalProgressLevel = actor.maxXp;
                int PercentProgressLevel;
                
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
                int _maxHealth = actor.maxHealth;
                int _currentHealth = actor.currentHealth;
                int X = 0;
                if (_maxHealth != 0)
                    X = (_currentHealth * 100) / _maxHealth;
                MenuStats.VieBar.size.Width = (236 * X) / 100;

                // point de vie dans Menustats
                MenuStats.VieLabel.Text = CommonCode.TranslateText(88);
                MenuStats.ViePts.Text = _currentHealth.ToString() + " / " + _maxHealth + " (" + X + "%)";
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
                MenuStats.PoidLabel.Text = CommonCode.TranslateText(97);
                int TotalPoid = actor.maxWeight;
                int CurrentPoid = actor.currentWeight;
                int PercentPoid = (CurrentPoid * 100) / TotalPoid;
                MenuStats.PoidRec.size.Width = (116 * PercentPoid) / 100;
                MenuStats.Poid.Text = CurrentPoid + " / " + TotalPoid + " (" + PercentPoid + "%)";
                MenuStats.Poid.point.X = MenuStats.PoidRec.point.X + 58 - (TextRenderer.MeasureText(MenuStats.Poid.Text, MenuStats.Poid.font).Width / 2);
                MenuStats.Ryo.Text = CommonCode.MoneyThousendSeparation(actor.ryo.ToString());
                MenuStats.resiDotonTxt.Text = actor.resiDotonPercent.ToString() + "%";
                MenuStats.resiKatonTxt.Text = actor.resiKatonPercent.ToString() + "%";
                MenuStats.resiFutonTxt.Text = actor.resiFutonPercent.ToString() + "%";
                MenuStats.resiRaitonTxt.Text = actor.resiRaitonPercent.ToString() + "%";
                MenuStats.resiSuitonTxt.Text = actor.resiSuitonPercent.ToString() + "%";
                MenuStats.__esquivePC_Txt.Text = actor.dodgePc.ToString();
                MenuStats.__esquivePM_Txt.Text = actor.dodgePm.ToString();
                MenuStats.__retraitPC_Txt.Text = actor.removePc.ToString();
                MenuStats.__retraitPM_Txt.Text = actor.removePm.ToString();

                // quete
                // convertir la list de quete en string
                if (quest != "")
                    for (int cnt = 0; cnt < quest.Split('/').Length; cnt++)
                    {
                        string quete = quest.Split('/')[cnt];
                        Actor.QuestInformations qi = new Actor.QuestInformations();
                        qi.nom_quete = quete.Split(':')[0];
                        qi.totalSteps = Convert.ToInt16(quete.Split(':')[1]);
                        qi.currentStep = Convert.ToInt16(quete.Split(':')[2]);
                        qi.submited = Convert.ToBoolean(quete.Split(':')[3]);
                        actor.Quests.Add(qi);
                    }

                // verification si le joueur été en combat pour le rediriger vers ce joueur
                if (inBattle)
                {
                    CommonCode.MyPlayerInfo.instance.pseudo = actor.pseudo;
                    CommonCode.MyPlayerInfo.instance.Event = "inBattle";
                }

                actor.spellPointLeft = spellPointLeft;

                ///// effacement du menu loading
                loadingLabel.Text = CommonCode.TranslateText(190);
                loadingLabel.point = new Point((ScreenManager.WindowWidth / 2) - (TextRenderer.MeasureText(loadingLabel.Text, loadingLabel.font).Width / 2), 610);
                System.Threading.Thread.Sleep(2000);

                // changement de map
                Manager.manager.mainForm.BeginInvoke((Action)(() =>
                {
                    CommonCode.ChangeMap(actor.map);  // affichage du hud
                    Manager.manager.GfxTopList.Remove(loadingParent);
                }));

                //////////////////////// affichage du hud
                Manager.manager.mainForm.BeginInvoke((Action)(() =>
                {

                    //////////// menu des sorts
                    MainForm.drawSpellStatesMenuOnce();
                    HudHandle.HudVisibility(true);  // affichage du hud
                }));
            })).Start();
            #endregion
        }
    }
}
