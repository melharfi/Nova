using MELHARFI.Gfx;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MELHARFI;

namespace MMORPG.Net.Messages.Response
{
    internal class GrabingMapInformationResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region MapData
            // commandStrings[1] = pseudo#classe#pvp:spirit:spiritLvl#village#MaskColors#map_position#orientation#level#action#waypoint#TotalPdv#CurrentPdv#rang   | separateur entre plusieurs joueurs
            // pvp=0 donc mode pvp off, si pvp = 1 mode pvp on

            string[] playersInfos = commandStrings[1].Split('|');

            foreach (string t in playersInfos)
            {
                string[] states = t.Split('#');

                string actorName = states[0];
                Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
                bool pvpEnabled = states[2].Split(':')[0] == "1";
                Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit.Name), states[2].Split(':')[1]);
                int spiritLevel = int.Parse(states[2].Split(':')[2]);
                Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[3]);
                string maskColors = states[4];
                Point mapPoint = new Point(int.Parse(states[5].Split('/')[0]), int.Parse(states[5].Split('/')[1]));
                int orientation = int.Parse(states[6]);
                int level = int.Parse(states[7]);
                Enums.AnimatedActions.Name action = (Enums.AnimatedActions.Name)Enum.Parse(typeof(Enums.AnimatedActions.Name), states[8]);
                string waypoint = states[9];
                int maxHealth = int.Parse(states[10]);
                int currentHealth = int.Parse(states[11]);

                // cette valeur n'est pas utilisé dans le jeu puisque le rang n'est pas encore affiché, il faut prévoir un truc pour montrer les rang de chaque joueur, ou pas si le rang dois etre caché, ptet que le rang est caché mais permet d'afficher le joueur avec un ora spécial
                Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), states[12]);

                // supprimer tout les anciennes instances qui correspond a ce joueur pour eviter un doublons ou une erreur de la part du serveur ou client s'il n'envoie pas la cmd de deconnexion du joeur au abonnés
                if (CommonCode.AllActorsInMap.Count(i => ((Actor)i.tag).pseudo == actorName) > 0)
                {
                    Bmp allPlayers = CommonCode.AllActorsInMap.Find(f => ((Actor)f.tag).pseudo == actorName);
                    allPlayers.visible = false;
                    CommonCode.AllActorsInMap.Remove(allPlayers);
                    CommonCode.AllActorsInMap.RemoveAll(i => ((Actor)i.tag).pseudo == actorName);
                }

                // affichage du personnage + position + orientation
                Bmp ibPlayers = new Bmp(@"gfx\general\classes\" + className + ".dat", Point.Empty, "Player_" + actorName, Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet(className.ToString(), CommonCode.ConvertToClockWizeOrientation(orientation)));
                ibPlayers.point = new Point((mapPoint.X * 30) + 15 - (ibPlayers.rectangle.Width / 2), (mapPoint.Y * 30) - (ibPlayers.rectangle.Height) + 15);
                ibPlayers.MouseOver += CommonCode.ibPlayers_MouseOver;
                ibPlayers.MouseOut += CommonCode.ibPlayers_MouseOut;
                ibPlayers.MouseMove += CommonCode.CursorHand_MouseMove;
                ibPlayers.MouseClic += CommonCode.ibPlayers_MouseClic;
                CommonCode.VerticalSyncZindex(ibPlayers);
                ibPlayers.TypeGfx = Manager.TypeGfx.Obj;
                Manager.manager.GfxObjList.Add(ibPlayers);

                ibPlayers.tag = CommonCode.MyPlayerInfo.instance.pseudo != actorName ? new Actor(actorName, level, !pvpEnabled ? Enums.Spirit.Name.neutral : spirit, className, pvpEnabled, !pvpEnabled ? 0 : spiritLevel, hiddenVillage, maskColors, orientation, action, waypoint) : CommonCode.MyPlayerInfo.instance.ibPlayer.tag;
                Actor actor = (Actor)ibPlayers.tag;
                actor.realPosition = mapPoint;
                actor.officialRang = officialRang;

                // affichage des ailles
                if (actor.pvpEnabled)
                {
                    if (actor.spirit != Enums.Spirit.Name.neutral)
                    {
                        Bmp _spirit = new Bmp(@"gfx\general\obj\2\" + actor.spirit + @"\" + actor.spiritLevel + ".dat", Point.Empty, "spirit_" + ibPlayers.name, Manager.TypeGfx.Obj, false, 1);
                        _spirit.point = new Point((ibPlayers.rectangle.Width / 2) - (_spirit.rectangle.Width / 2), -_spirit.rectangle.Height);
                        ibPlayers.Child.Add(_spirit);

                        Txt lPseudo = new Txt(actor.pseudo, Point.Empty, "lPseudo_" + ibPlayers.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                        lPseudo.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -_spirit.rectangle.Height - 15);
                        ibPlayers.Child.Add(lPseudo);

                        Txt lLvlSpirit = new Txt(actor.spiritLevel.ToString(), Point.Empty, "lLvlSpirit_" + ibPlayers.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Bold), Brushes.Red);
                        lLvlSpirit.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Width / 2) + 2, -_spirit.rectangle.Y - (_spirit.rectangle.Height / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Height / 2));
                        ibPlayers.Child.Add(lLvlSpirit);

                        Bmp village = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", Point.Empty, "village_" + actor.hiddenVillage, Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet("pays_" + actor.hiddenVillage + "_thumbs", 0));
                        village.point = new Point((ibPlayers.rectangle.Width / 2) - (village.rectangle.Width / 2), lPseudo.point.Y - village.rectangle.Height + 2);
                        ibPlayers.Child.Add(village);
                    }
                }
                else
                {
                    Txt lPseudo = new Txt(actor.pseudo, Point.Empty, "lPseudo_" + ibPlayers.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                    lPseudo.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -15);
                    ibPlayers.Child.Add(lPseudo);

                    Bmp village = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", Point.Empty, "village_" + actor.hiddenVillage, Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet("pays_" + actor.hiddenVillage + "_thumbs", 0));
                    village.point = new Point((ibPlayers.rectangle.Width / 2) - (village.rectangle.Width / 2), lPseudo.point.Y - village.rectangle.Height + 2);
                    ibPlayers.Child.Add(village);
                }

                // coloriage du personnage et attachement du maskColor au personnage
                CommonCode.ApplyMaskColorToClasse(ibPlayers);

                // pointeur vers l'image ibplayer de notre joueur s'il s'agit de son personnage
                if (CommonCode.MyPlayerInfo.instance.pseudo == actor.pseudo)
                    CommonCode.MyPlayerInfo.instance.ibPlayer = ibPlayers;

                // ajout du personnage dans la liste des joueurs
                CommonCode.AllActorsInMap.Add(ibPlayers);

                // lancement de l'animation de mouvement si le joueur a un waypoint
                if (actor.wayPoint.Count > 0)
                {
                    //mouvement du personnage
                    // thread a part
                    Thread tAnimAction = new Thread(() => CommonCode.AnimAction(ibPlayers, actor.wayPoint, actor.wayPoint.Count > 5 ? 20 : 50));
                    tAnimAction.Start();
                }

                // affectation des pdv
                actor.maxHealth = maxHealth;
                actor.currentHealth = currentHealth;
            }
            #endregion
        }
    }
}
