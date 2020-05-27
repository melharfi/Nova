using System;
using System.Drawing;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;
using MMORPG.Enums;

namespace MMORPG.Net.Messages.Response
{
    internal class GrabingMapObjectsInformationResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            // GrabingMapObjectsInformationResponseMessage•FreeChallenge#10/9#1#A  °  hamid#0#konoha#50#angel#9\yassin#0#konoha#50#angel#9    |    FreeChallenge#15/4#1#B  °  #morad#0#konoha#50#angel#9/soufian#0#konoha#50#angel#9
            // GrabingMapObjectsInformationResponseMessage•FreeChallenge#map_position#IdBattle#side°Pseudo#ClassID#Village#Level#Spirite#SpiriteLvl \ <-- séparateur entre les joueur de la team en cour ... | <-- séparateur entre les objets en cours sur la map
            
            string objs = commandStrings[1];
            foreach (string t in objs.Split('|'))
            {
                // t = FreeChallenge#map_position#IdBattle#side  °  Pseudo#ClassID#Village#Level#Spirite#SpiriteLvl \ Pseudo#ClassID#Village#Level#Spirite#SpiriteLvl
                // |    séparateur entre les tous les objets du map
                // t = FreeChallenge#map_position#IdBattle#side  °  Pseudo#ClassID#Village#Level#Spirite#SpiriteLvl \ Pseudo#ClassID#Village#Level#Spirite#SpiriteLvl
                // les données de tous les joueurs present dans le combats sont séparé par le caractère \      comme ca Pseudo#ClassID#Village#Level#Spirite#SpiriteLvl \ Pseudo#ClassID#Village#Level#Spirite#SpiriteLvl \ Pseudo#ClassID#Village#Level#Spirite#SpiriteLvl ...

                // ...
                if (t.Split('#').Length > 0 && t.Split('#')[0] == BattleType.Type.FreeChallenge.ToString())
                {
                    // objParamsStrings[0] = FreeChallenge#10/9#1#A
                    // objParamsStrings[1] = hamid#0#konoha#50#angel#9\yassin#0#konoha#50#angel#9
                    string[] objParamsStrings = t.Split('°');

                    string[] metaData = objParamsStrings[0].Split('#');
                    const BattleType.Type battleType = BattleType.Type.FreeChallenge;
                    Point mapPoint = new Point(int.Parse(metaData[1].Split('/')[0]), int.Parse(metaData[1].Split('/')[1]));
                    int idBattle = int.Parse(metaData[2]);
                    Team.Side side = (Team.Side)Enum.Parse(typeof(Team.Side), metaData[3]);

                    TagedBattleForSpectators tagedBattleForSpectators = new TagedBattleForSpectators
                    {
                        BattleType = battleType,
                        MapPoint = mapPoint,
                        IdBattle = idBattle,
                        TeamSide = side
                    };

                    // on parcour tous les joueurs presents
                    foreach (string actorDataString in objParamsStrings[1].Split('\\'))
                    {
                        string[] actorParams = actorDataString.Split('#');
                        Actor currentActor = new Actor
                        {
                            pseudo = actorParams[0],
                            teamSide = side,
                            className = CommonCode.IdToClassName(int.Parse(actorParams[1])),
                            hiddenVillage =
                                (HiddenVillage.Names)
                                Enum.Parse(typeof(HiddenVillage.Names), actorParams[2]),
                            level = int.Parse(actorParams[3]),
                            spirit = (Spirit.Name) Enum.Parse(typeof(Spirit.Name), actorParams[4]),
                            spiritLevel = int.Parse(actorParams[5])
                        };

                        tagedBattleForSpectators.AllPlayersByOrder.Add(currentActor);
                    }

                    //tag = battleID#sideA ou sideB#pseudo#classID#Village#Class Level#Spirit#Level Alignement Séparé par |
                    Bmp mapDataObjFreeChallenge =
                        new Bmp(@"gfx\general\obj\3\challenge\" + BattleType.Type.FreeChallenge + ".dat", mapPoint,
                            "_MapDataObj_" + BattleType.Type.FreeChallenge, Manager.TypeGfx.Obj, true, 1,
                            SpriteSheet.GetSpriteSheet(BattleType.Type.FreeChallenge.ToString(), 0))
                        {
                            tag = tagedBattleForSpectators
                        };
                    int x = tagedBattleForSpectators.MapPoint.X * 30;
                    int y = tagedBattleForSpectators.MapPoint.Y * 30;
                    x += 15 - mapDataObjFreeChallenge.rectangle.Size.Width / 2;
                    y += 30 - mapDataObjFreeChallenge.rectangle.Size.Height;
                    mapDataObjFreeChallenge.point = new Point(x, y);

                    // voir si ces methodes sont appelé par d'autre procedures si non les supprimer psuiqu'il existe en double dans commonCode
                    mapDataObjFreeChallenge.MouseOver += _MapDataObj_FreeChallenge_MouseOver;
                    mapDataObjFreeChallenge.MouseOut += _MapDataObj_FreeChallenge_MouseOut;
                    mapDataObjFreeChallenge.MouseClic += _MapDataObj_FreeChallenge_MouseClic;
                    mapDataObjFreeChallenge.MouseMove += _MapDataObj_FreeChallenge_MouseMove;
                    Manager.manager.GfxObjList.Add(mapDataObjFreeChallenge);
                }
            }
            #endregion
        }

        static void _MapDataObj_FreeChallenge_MouseOver(Bmp bmp, MouseEventArgs e)
        {
            #region
            //lors d'un survole sur l'objet "bouclié" qui represente le combat sur le map
            bmp.point.X -= 2;
            TagedBattleForSpectators tagedBattleForSpectators = (TagedBattleForSpectators)bmp.tag;

            bmp.ChangeBmp(@"gfx\general\obj\3\challenge\" + BattleType.Type.FreeChallenge + ".dat", SpriteSheet.GetSpriteSheet(BattleType.Type.FreeChallenge.ToString(), tagedBattleForSpectators.TeamSide == Team.Side.A ? 1 : 2));
            Bmp mapDataObjFreeChallengeP2 = (Bmp)Manager.manager.GfxObjList.Find(f => f.GetType() == typeof(Bmp) && ((Bmp)f).name == "_MapDataObj_" + BattleType.Type.FreeChallenge && ((TagedBattleForSpectators)f.Tag()).IdBattle == tagedBattleForSpectators.IdBattle && ((TagedBattleForSpectators)f.Tag()).TeamSide == (tagedBattleForSpectators.TeamSide == Team.Side.A ? Team.Side.B : Team.Side.A));
            mapDataObjFreeChallengeP2.ChangeBmp(@"gfx\general\obj\3\challenge\" + BattleType.Type.FreeChallenge + ".dat", SpriteSheet.GetSpriteSheet(BattleType.Type.FreeChallenge.ToString(), tagedBattleForSpectators.TeamSide == Team.Side.A ? 2 : 1));
            #endregion
        }

        private static void _MapDataObj_FreeChallenge_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            #region
            //mouseout sur l'objet "bouclé" qui represente le combat sur le map
            CommonCode.CursorDefault_MouseOut(null, null);

            bmp.point.X += 2;

            TagedBattleForSpectators tagedBattleForSpectators = (TagedBattleForSpectators) bmp.tag;
            bmp.ChangeBmp(@"gfx\general\obj\3\challenge\" + BattleType.Type.FreeChallenge + ".dat", SpriteSheet.GetSpriteSheet(BattleType.Type.FreeChallenge.ToString(), 0));

            IGfx mapDataObjFreeChallengeP2 = Manager.manager.GfxObjList.Find(f => f.GetType() == typeof(Bmp) && ((Bmp)f).name == "_MapDataObj_" + BattleType.Type.FreeChallenge && ((TagedBattleForSpectators)f.Tag()).IdBattle == tagedBattleForSpectators.IdBattle && ((TagedBattleForSpectators)f.Tag()).TeamSide == (tagedBattleForSpectators.TeamSide == Team.Side.A ? Team.Side.B : Team.Side.A));
            ((Bmp)mapDataObjFreeChallengeP2).ChangeBmp(@"gfx\general\obj\3\challenge\" + BattleType.Type.FreeChallenge + ".dat", SpriteSheet.GetSpriteSheet(BattleType.Type.FreeChallenge.ToString(), 0));
            #endregion
        }

        private static void _MapDataObj_FreeChallenge_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // tag = battleID#sideA ou sideB#pseudo#classID#Village#Class Level#Alignement#Level Alignement
            // menu qui affiche les joueurs en attente du combat
            CommonCode.DrawBattleContextMenu(bmp, e);
        }

        private static void _MapDataObj_FreeChallenge_MouseMove(Bmp flag, MouseEventArgs e)
        {
            // survole le drapeau bouclier du combat
            CommonCode.CursorHand_MouseMove(null, null);
        }
    }
}
