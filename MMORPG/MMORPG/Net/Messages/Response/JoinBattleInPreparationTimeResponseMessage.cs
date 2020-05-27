using MELHARFI;
using System;
using System.Drawing;
using System.Threading;

namespace MMORPG.Net.Messages.Response
{
    internal class JoinBattleInPreparationTimeResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            //JoinBattleResponseMessage•playersDatasideA•playersDatasideB•battleStartPositions.Map("Start") + "•" + MainClass.InitialisationBattleWaitTime + "•" + _battle.timestamp

            string sideAData = commandStrings[0];
            // currentPlayerInfo.Pseudo + "#" + currentPlayerInfo.classeName + "#" + currentPlayerInfo.Level + "#" + currentPlayerInfo.village + "#" + currentPlayerInfo.MaskColors + "#" + currentPlayerInfo.totalHealth + "#" + currentPlayerInfo.currentHealth + "#" + currentPlayerInfo.officialRang + "|";     | séparateur entre les states des joueurs
            string sideBData = commandStrings[1];
            // currentPlayerInfo.Pseudo + "#" + currentPlayerInfo.classeName + "#" + currentPlayerInfo.Level + "#" + currentPlayerInfo.village + "#" + currentPlayerInfo.MaskColors + "#" + currentPlayerInfo.totalHealth + "#" + currentPlayerInfo.currentHealth + "#" + currentPlayerInfo.officialRang + "|";     | séparateur entre les states des joueurs
            string battleStartPositions = commandStrings[2];
            string initialisationBattleWaitTime = commandStrings[3];
            string timestamp = commandStrings[4];       // sert a rien a supprimer peux etre du sereur, ATTENTION le decoReco ne marche pas quand le temps de préparation a écoulé, il faut prévoir une autre cmd

            if (MMORPG.Battle.state == Enums.battleState.state.idle)
            {
                MMORPG.Battle.state = Enums.battleState.state.initialisation;
                //pseudo#classe#level#village#MaskColors#TotalPdv#CurrentPdv#rang|pseudo#classe#level#village#MaskColors#TotalPdv#CurrentPdv#rang•ValidePos•sideA#p1t1#position1t1X,position1t1Y•sideB#p1t2#position1t2X,position1t2Y
                
                #region données joueurs
                //pseudo#classe#level#village#MaskColors#TotalPdv#CurrentPdv#rang
                // | séparateur d'instance
                string[] sideA = sideAData.Split('|');
                foreach (string t in sideA)
                {
                    string[] states = t.Split('#');

                    string actorName = states[0];
                    Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
                    int level = int.Parse(states[2]);
                    Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[3]);
                    string maskColorsString = states[4];
                    int maxHealth = int.Parse(states[5]);
                    int currentHealth = int.Parse(states[6]);
                    Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), states[7]);

                    Actor piibt1 = new Actor();
                    piibt1.AddPlayer(Enums.Team.Side.A, actorName, className, level, hiddenVillage, maskColorsString, maxHealth, currentHealth, officialRang);
                    piibt1.ibPlayer = CommonCode.AllActorsInMap.Find(f => ((Actor)f.tag).pseudo == actorName);
                    CommonCode.AdjustPositionAndDirection(piibt1.ibPlayer, piibt1.ibPlayer.point);
                    MMORPG.Battle.SideA.Add(piibt1);
                }

                string[] sideB = sideBData.Split('|');
                foreach (string t in sideB)
                {
                    string[] states = t.Split('#');

                    string playerName = states[0];
                    Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
                    int level = int.Parse(states[2]);
                    Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[3]);
                    string maskColorsString = states[4];
                    int maxHealth = int.Parse(states[5]);
                    int currentHealth = int.Parse(states[6]);
                    Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), states[7]);

                    Actor piibt2 = new Actor();
                    piibt2.AddPlayer(Enums.Team.Side.A, playerName, className, level, hiddenVillage, maskColorsString, maxHealth, currentHealth, officialRang);
                    piibt2.ibPlayer = CommonCode.AllActorsInMap.Find(f => ((Actor)f.tag).pseudo == playerName);
                    CommonCode.AdjustPositionAndDirection(piibt2.ibPlayer, piibt2.ibPlayer.point);
                    MMORPG.Battle.SideB.Add(piibt2);
                }
                #endregion

                #region données des positions valide pour les 2 teams
                string[] _battleStartPositions = battleStartPositions.Split('|');

                string[] dataT1 = _battleStartPositions[0].Split('#');
                foreach (string t in dataT1)
                {
                    string[] data2 = t.Split('/');
                    MMORPG.Battle.ValidePosT1.Add(new Point(Convert.ToInt16(data2[0]) * 30, Convert.ToInt16(data2[1]) * 30));
                }

                string[] dataT2 = _battleStartPositions[1].Split('#');
                foreach (string t in dataT2)
                {
                    string[] data2 = t.Split('/');
                    MMORPG.Battle.ValidePosT2.Add(new Point(Convert.ToInt16(data2[0]) * 30, Convert.ToInt16(data2[1]) * 30));
                }
                #endregion

                // dessin des positions valides dans le map
                MMORPG.Battle.DrawBattleValidePos();

                // cette cmd est introuvable, comment est se que le client récupére les données de positionnement des joueurs ??
                Network.SendMessage("cmd•getPlayersPositionInBattle", true);

                // image du chronometre timeout
                Manager.manager.GfxTopList.Add(MMORPG.Battle.Chrono_TimeOut);

                //timer de temps restant
                Manager.manager.GfxTopList.Add(MMORPG.Battle.TimeLeftLabel);

                //lancement du thread du la barre de progression du timeleft
                Thread timeLeftT = new Thread(CommonCode.timeLeftForBattle);
                MMORPG.Battle.TimeLeftLabel.tag = initialisationBattleWaitTime;
                timeLeftT.Start();
            }
            #endregion
        }
    }
}
