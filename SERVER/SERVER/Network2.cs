using System;
using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;
using SERVER.Enums;
using SERVER.Net.Messages.Response;

namespace SERVER
{
    public static class Network2
    {
        public static void GetData(string[] cmd, NetIncomingMessage im)
        {
            Actor actor = (Actor) im.SenderConnection.Tag;
            if (cmd.Length == 5 && cmd[1] == "spellTuiles")
            {
                #region
                if (actor.Pseudo == "" || actor.map == "" || actor.inBattle == 0)
                {
                    Security.User_banne("spellTuiles", im.SenderConnection);
                    return;
                }

                int pointX, pointY;
                if (!int.TryParse(cmd[3], out pointX))
                    return;

                if (!int.TryParse(cmd[4], out pointY))
                    return;

                int spellId;
                if (!int.TryParse(cmd[2], out spellId))
                    return;

                Point spellPoint = new Point(pointX, pointY);

                // cmd•spellTuiles•sortID•MousePosX•MousePosY
                // check si le joueur est en combat, si le combat est en mode Started
                if (actor.inBattle == 0 && Battle.Battles.Find(f => f.IdBattle == actor.idBattle).State != battleState.state.started)
                    return;

                // check si le client est autorisé a avoir ce sort
                mysql.players player = ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == actor.Pseudo);

                if (player == null)
                    return;

                if (!actor.sorts.Exists(f => f.SpellId == spellId))
                {
                    // le client tente de lancer un sort qu'il na pas
                    // bannissement peux etre
                    Console.WriteLine("client qui cheat en lançons un sort qui n'est pas le sien");
                    return;
                }

                // common check
                if (!Fight.spellsChecker.spells(spellId, spellPoint, im))
                    return;

                Battle battle = Battle.Battles.Find(f => f.IdBattle == actor.idBattle);
                Actor spellCaster = battle.AllPlayersByOrder.Find(f => f.Pseudo == actor.Pseudo);
                Fight.Effect_Dispatcher.Apply(spellCaster, battle, spellPoint, spellId);
                #endregion
            }
            else if (cmd[1] == "InteractWithPNJ")
            {
                #region
                // interaction avec pnj cmd.InteractWithPNJ.nom pnj.parametre
                if (actor.Pseudo == "")
                {
                    Security.User_banne("InteractWithPNJ", im.SenderConnection);
                    return;
                }

                string pnj = cmd[2];

                if (pnj == "iruka")
                {
                    // 1er PNJ Créée
                    if (cmd.Length > 2 && cmd[3] == "acceptFirstFight")
                    {
                        // "cmd•InteractWithPNJ•iruka•acceptFirstFight"
                        // on vérifie si le joueur a préalablement fait la quête acceptFirstFight
                        int quete = CheckQuete.isSubmitedQuest("FirstFight", actor);
                        if (quete == 1)
                        {
                            //commun.SendMessage("cmd•checkQuete•FirstFight•" + quete, im, true);
                            Console.WriteLine("client a déja la quete, et il essai de la refaire alors que ce n'est pas possible sans avoir modifier le code ou les paquets !!!");
                            return;
                        }

                        // on vérifie si la quete est déja presente et si elle est en mode submited
                        List<mysql.quete> quetes = ((List<mysql.quete>)DataBase.DataTables.quete).FindAll(f => f.pseudo == actor.Pseudo);
                        if (quetes.Count == 0)
                        {
                            // création de la quete chez le client
                            mysql.quete quete2 = new mysql.quete() { pseudo = actor.Pseudo, nom_quete = "FirstFight", currentStep = 1 };
                            ((List<mysql.quete>)DataBase.DataTables.quete).Add(quete2);

                        }

                        // envoyer au client que la quete FirstFight a été débuté
                        CommonCode.SendMessage("cmd•beganQuete•FirstFight", im, true);

                        // ce code dois ressembler au code de la cmd AcceptChallenge sur la class Network.cs a part quelque modification comme les position qui ne sont pas enregistrés sur un fichier
                        // on vérifie si le joueur n'est pas en combat se qui est impossible vus que dans cette map il est seul normalement
                        if (actor.inBattle == 1)
                        {
                            Console.WriteLine("Player " + actor.Pseudo + " is in battle, can't interact with pnj iruka");
                            return;
                        }

                        // preparation du combat
                        Console.WriteLine("combat commancé avec PNJ Iruka");
                        // création d'une session de combat
                        // ATTENTION, ce code a été copié depuis la methode Network.cs GetData() cmd AcceptChallenge, Alors si un changement est fait sur cette methode, il faut l'appliquer sur celle la aussi
                        Battle battle = new Battle();

                        // 2eme joueur
                        Actor actor1SideB = (Actor)actor.Clone();
                        actor1SideB.directionLook = 0;
                        actor1SideB.teamSide = Team.Side.B;
                        actor1SideB.idBattle = battle.IdBattle;
                        battle.SideB.Add(actor1SideB);

                        // 1er player
                        Actor actor1SideA = new Actor
                        {
                            currentHealth = 1000,
                            maxHealth = 1000,
                            classeName = ActorClass.ClassName.iruka,
                            classeId = 8,
                            initiative = 1000,
                            summons = 5,
                            species = Species.Name.Pnj,
                            Pseudo = ActorClass.ClassName.iruka.ToString(),
                            maskColorString = "null/null/null",
                            directionLook = 2,
                            teamSide = Team.Side.A,
                            idBattle = battle.IdBattle,
                            originalPc = 10,
                            originalPm = 10,
                            doton = 500
                        };
                        // pour jouer au premier

                        // ajout des sorts
                        Actor.SpellsInformations infosSorts = new Actor.SpellsInformations { SpellId = 4, SpellColor = 0, Level = 1 };
                        actor1SideA.sorts.Add(infosSorts);

                        battle.SideA.Add(actor1SideA);
                        battle.Owner = actor1SideB.Pseudo;
                        battle.Map = actor.map;
                        battle.BattleType = BattleType.Type.VsPnj;
                        battle.BattleFlags[0] = "iruka";
                        battle.BattleFlags[1] = "FirstFight";
                        battle.Timestamp = CommonCode.ReturnTimeStamp();
                        battle.IsFreeCellToSpell = CommonCode.ReturnFreeCellToSpellFunc(battle.Map);
                        battle.IsFreeCellToWalk = CommonCode.ReturnFreeCellToWalkFunc(battle.Map);
                        Battle.Battles.Add(battle);

                        // sauveguard de l'id dans la tables des joueurs

                        foreach (mysql.players player in ((List<mysql.players>)DataBase.DataTables.players).FindAll(f => f.pseudo == actor.Pseudo || f.pseudo == actor.PlayerChallengeYou))
                        {
                            player.inBattle = 1;
                            player.inBattleType = battle.BattleType.ToString();
                            player.inBattleID = battle.IdBattle;
                        }

                        actor.inBattle = 1;
                        actor.idBattle = battle.IdBattle;
                        actor.teamSide = Team.Side.B;

                        // collecte des données des 2 joueurs
                        string playersData = "";

                        playersData += actor1SideA.Pseudo + "#" + actor1SideA.classeName + "#" + actor1SideA.level + "#" + actor1SideA.hiddenVillage + "#" + actor1SideA.maskColorString + "#" + actor1SideA.maxHealth + "#" + actor1SideA.currentHealth + "#" + actor1SideA.officialRang + "#" + actor1SideA.initiative + "#" + actor1SideA.doton + "#" + actor1SideA.katon + "#" + actor1SideA.futon + "#" + actor1SideA.raiton + "#" + actor1SideA.suiton + "#" + actor1SideA.usingDoton + "#" + actor1SideA.usingKaton + "#" + actor1SideA.usingFuton + "#" + actor1SideA.usingRaiton + "#" + actor1SideA.usingSuiton + "#" + actor1SideA.equipedDoton + "#" + actor1SideA.equipedKaton + "#" + actor1SideA.equipedFuton + "#" + actor1SideA.equipedRaiton + "#" + actor1SideA.equipedSuiton + "#" + actor1SideA.originalPc + "#" + actor1SideA.originalPm + "#" + actor1SideA.pe + "#" + actor1SideA.cd + "#" + actor1SideA.summons + "#" + actor1SideA.resiDotonPercent + "#" + actor1SideA.resiKatonPercent + "#" + actor1SideA.resiFutonPercent + "#" + actor1SideA.resiRaitonPercent + "#" + actor1SideA.resiSuitonPercent + "#" + actor1SideA.dodgePC + "#" + actor1SideA.dodgePM + "#" + actor1SideA.dodgePE + "#" + actor1SideA.dodgeCD + "#" + actor1SideA.removePC + "#" + actor1SideA.removePM + "#" + actor1SideA.removePE + "#" + actor1SideA.removeCD + "#" + actor1SideA.escape + "#" + actor1SideA.blocage + "#" + actor1SideA.species.ToString() + "#" + actor1SideA.directionLook + "|";
                        playersData += actor1SideB.Pseudo + "#" + actor1SideB.classeName + "#" + actor1SideB.level + "#" + actor1SideB.hiddenVillage + "#" + actor1SideB.maskColorString + "#" + actor1SideB.maxHealth + "#" + actor1SideB.currentHealth + "#" + actor1SideB.officialRang + "#" + actor1SideB.initiative + "#" + actor1SideB.doton + "#" + actor1SideB.katon + "#" + actor1SideB.futon + "#" + actor1SideB.raiton + "#" + actor1SideB.suiton + "#" + actor1SideB.usingDoton + "#" + actor1SideB.usingKaton + "#" + actor1SideB.usingFuton + "#" + actor1SideB.usingRaiton + "#" + actor1SideB.usingSuiton + "#" + actor1SideB.equipedDoton + "#" + actor1SideB.equipedKaton + "#" + actor1SideB.equipedFuton + "#" + actor1SideB.equipedRaiton + "#" + actor1SideB.equipedSuiton + "#" + actor1SideB.originalPc + "#" + actor1SideB.originalPm + "#" + actor1SideB.pe + "#" + actor1SideB.cd + "#" + actor1SideB.summons + "#" + actor1SideB.resiDotonPercent + "#" + actor1SideB.resiKatonPercent + "#" + actor1SideB.resiFutonPercent + "#" + actor1SideB.resiRaitonPercent + "#" + actor1SideB.resiSuitonPercent + "#" + actor1SideB.dodgePC + "#" + actor1SideB.dodgePM + "#" + actor1SideB.dodgePE + "#" + actor1SideB.dodgeCD + "#" + actor1SideB.removePC + "#" + actor1SideB.removePM + "#" + actor1SideB.removePE + "#" + actor1SideB.removeCD + "#" + actor1SideB.escape + "#" + actor1SideB.blocage + "#" + actor1SideB.species.ToString() + "#" + actor1SideB.directionLook;

                        // modification des position des joueurs selon les position valide du map aléatoirement
                        string[] sideAValidePos = battleStartPositions.Map(battle.Map, battle).Split('|')[0].Split('#');
                        string[] team2ValidePos = battleStartPositions.Map(battle.Map, battle).Split('|')[1].Split('#');

                        battle.SideAValidePos = battleStartPositions.Map(battle.Map, battle).Split('|')[0];
                        battle.SideBValidePos = battleStartPositions.Map(battle.Map, battle).Split('|')[1];

                        Random random = new Random();
                        int rand = random.Next(sideAValidePos.Length);
                        actor1SideB.map_position = new Point(Convert.ToInt32(team2ValidePos[rand].Split('/')[0]), Convert.ToInt32(team2ValidePos[rand].Split('/')[1]));

                        rand = random.Next(sideAValidePos.Length);
                        actor1SideA.map_position = new Point(Convert.ToInt32(sideAValidePos[rand].Split('/')[0]), Convert.ToInt32(sideAValidePos[rand].Split('/')[1]));

                        ////////////////////////////////////

                        string team1 = battle.SideA.Aggregate("", (current, piib) => current + (piib.Pseudo + "|" + piib.map_position.X + "/" + piib.map_position.Y + "#"));
                        team1 = team1.Substring(0, team1.Length - 1);

                        string team2 = battle.SideB.Aggregate("", (current, piib) => current + (piib.Pseudo + "|" + piib.map_position.X + "/" + piib.map_position.Y + "#"));
                        team2 = team2.Substring(0, team2.Length - 1);

                        ////////////////////////////////////
                        //pseudo#classe#level#village#MaskColors#TotalPdv#CurrentPdv#rang#initiative#doton#katon#futon#raiton#siuton#usingDoton#usingKaton#usingFuton#usingRaiton#usingSuiton#dotonEquiped#katonEquiped#futonEquiped#raitonEquiped#suitonEquiped#pc#pm#pe#cd#invoc#resiDoton#resiKaton#resiFuton#resiRaiton#resiSuiton#esquivePC#esquivePM#esquivePE#esquiveCD#retraitPC#retraitPM#retraitPE#retraitCD#evasion#blocage
                        // envoie au client la confirmation du challenge
                        CommonCode.SendMessage("cmd•challengeBegan•" + playersData + "•" + battle.SideAValidePos + "|" + battle.SideBValidePos + "•" + MainClass.InitialisationBattleWaitTime + "•" + battle.BattleType + "•" + team1 + "•" + team2 + "•" + battle.State + "•" + "14/5#16/5#18/5" + "•" + "14/10#16/10#18/10", im, true);
                        //Console.WriteLine("<--cmd•challengeBegan•" + playersData + "•" + battle.SideAValidePos + "|" + battle.SideBValidePos + "•" + MainClass.InitialisationBattleWaitTime + "•" + battle.BattleType + "•" + team1 + "•" + team2 + "•" + battle.State + "•" + "14/5#16/5#18/5" + "•" + "14/10#16/10#18/10" + " to " + actor.Pseudo);

                        // lock position
                        bool launchBattle = false;
                        battle.LockedPosInIniTime.Add(actor1SideA.Pseudo);

                        foreach (Actor t in battle.SideB)
                        {
                            NetConnection nc = MainClass.netServer.Connections.Find(f => ((Actor)f.Tag).Pseudo == t.Pseudo);
                            if (nc != null)
                            {
                                CommonCode.SendMessage("cmd•BattlePosLocked•" + actor1SideA.Pseudo + "•" + launchBattle, nc, true);
                                //Console.WriteLine("<--cmd•BattlePosLocked•" + actor1SideA.Pseudo + "•" + launchBattle + " to " + ((Actor)nc.Tag).Pseudo);
                            }
                        }
                    }
                    else if (cmd.Length > 4 && cmd[3] == "tp")
                    {
                        // cmd[4] = le tp apres la quette FirtFight
                        switch (cmd[4])
                        {
                            case "1":
                                // tp au village du joueur
                                // pour le moment je vais tp vers une seul map vus qu'au début il n'aura pas bc de monde
                                if (actor.map.ToLower() == "start" && actor.Quests.Exists(f => f.QuestName == "FirstFight" && f.Submited))
                                {
                                    // l'utilisateur a déja fait la quete FirstFight, on le tp a son statue

                                    actor.map = "_0_0_0";

                                    // changement du map du jour sur la table players
                                    ((List<mysql.players>)DataBase.DataTables.players).Find(f => f.pseudo == actor.Pseudo).map = "_0_0_0";

                                    // changement du map du jour sur la table connected
                                    ((List<mysql.connected>)DataBase.DataTables.connected).Find(f => f.pseudo == actor.Pseudo).map = "_0_0_0";

                                    CommonCode.SendMessage("cmd•change map•_0_0_0", im, true);
                                    //Console.WriteLine("<--cmd•change map•_0_0_0 to " + actor.Pseudo);
                                }
                                else
                                {
                                    // quete pas encore faite
                                    Console.WriteLine("Error, quete FirstFight na pas été faite, et le joueur " + actor.Pseudo + " essai de se TP, se qui n'est pas possible");
                                    Console.WriteLine("Le client et le serveurs doivent etre synchronisé, si on arrive la c que le joueur a modifier le client");
                                }
                                break;
                            /*if(pi.village == "konoha")
                            {
                                // tp vers map konoha
                            }
                            else if(pi.village == "iwa")
                            {

                            }
                            else if (pi.village == "kiri")
                            {

                            }
                            else if (pi.village == "kumo")
                            {

                            }
                            else if (pi.village == "suna")
                            {

                            }*/
                        }
                    }
                }
                #endregion
            }
            else if (cmd.Length > 1 && cmd[1] == "checkQuete")
            {
                #region
                if (actor.Pseudo == "")
                {
                    Security.User_banne("checkQuete", im.SenderConnection);
                    return;
                }
                Actor pi = (Actor)im.SenderConnection.Tag;
                CommonCode.SendMessage("cmd•checkQuete•" + cmd[2] + "•" + CheckQuete.isSubmitedQuest(cmd[2], pi), im, true);
                #endregion
            }
            else if (cmd.Length > 1 && cmd[1] == "requireBattleData")
            {
                #region
                if (actor.Pseudo == "")
                {
                    Security.User_banne("requireBattleData", im.SenderConnection);
                    return;
                }
                CommonCode.RequireBattleData(im);
                #endregion
            }
            else if (cmd.Length > 1 && cmd[1] == "getMyPlayerPos")
            {
                #region
                if (actor.Pseudo == "" || actor.map == "" || actor.inBattle == 1)
                {
                    Security.User_banne("getMyPlayerPos", im.SenderConnection);
                    return;
                }
                if (actor.Pseudo == "")
                {
                    Security.User_banne("getMyPlayerPos", im.SenderConnection);
                    return;
                }
                CommonCode.SendMessage("cmd•getMyPlayerPos•" + actor.map_position.X + "/" + actor.map_position.Y, im, true);
                #endregion
            }
            else if (cmd.Length > 1 && cmd[1] == "upgradeSpell")
            {
                #region
                if (actor.Pseudo == "" || actor.map == "" || actor.inBattle == 1)
                {
                    Security.User_banne("upgradeSpell", im.SenderConnection);
                    return;
                }
                // joueur qui veux augementer le lvl d'un sort
                int spellId;
                if (int.TryParse(cmd[2], out spellId) && actor.inBattle == 0)
                    if (actor.sorts.Exists(f => f.SpellId == spellId))
                    {
                        Actor.SpellsInformations infoSort = actor.sorts.Find(f => f.SpellId == spellId);
                        // on check si le sort na pas atteint sa limite qui est de 5
                        if (infoSort.Level < 5 && actor.spellPointLeft >= infoSort.Level)
                        {
                            actor.spellPointLeft -= infoSort.Level;
                            infoSort.Level++;

                            // augmentation du level su sort
                            string[] spells =
                                ((List<mysql.players>) DataBase.DataTables.players).Find(
                                    f => f.pseudo == actor.Pseudo).sorts.Split('/');

                            for (int cnt = 0; cnt < spells.Length; cnt++)
                            {
                                bool found = false;
                                string[] spellsData = spells[cnt].Split(':');
                                if (spellsData[0] == spellId.ToString())
                                    found = true;

                                if (found)
                                {
                                    string newSpellData = "";
                                    for (int cnt2 = 0; cnt2 < spellsData.Length; cnt2++)
                                    {
                                        if (cnt2 == 2)
                                            newSpellData += (Convert.ToInt16(spellsData[cnt2]) + 1) + ":";
                                        else
                                            newSpellData += spellsData[cnt2] + ":";
                                    }
                                    newSpellData = newSpellData.Substring(0, newSpellData.Length - 1);
                                    spells[cnt] = newSpellData;
                                }
                            }

                            // reconstruction des states des sorts apres augementation du lvl
                            string newData = spells.Aggregate("", (current, t) => current + t + "/");
                            newData = newData.Substring(0, newData.Length - 1);

                            // mise à jours des données
                            mysql.players player =
                                ((List<mysql.players>) DataBase.DataTables.players).Find(
                                    f => f.pseudo == actor.Pseudo);
                            player.sorts = newData;
                            player.spellPointLeft = actor.spellPointLeft;
                            CommonCode.SendMessage(
                                "cmd•upgradedSpell•" + spellId + "•" + infoSort.Level + "•" + actor.spellPointLeft,
                                im, true);
                        }
                    }
                #endregion
            }
        }
    }
}