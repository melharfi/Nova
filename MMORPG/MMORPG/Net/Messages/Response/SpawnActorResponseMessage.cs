using System;
using System.Drawing;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class SpawnActorResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            #region
            // connexion d'un joueur
            // pseudo#classe#pvp:spirit:spiritLvl#village#MaskColors#map_position#orientation#level#action#waypoint   - separateur entre plusieurs joueurs
            // pvp=0 donc mode pvp off, si pvp = 1 mode pvp on
            string[] data = commandStrings[1].Split('#');

            string playerName = data[0];
            Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), data[1]);
            bool pvpEnabled = bool.Parse(data[2].Split(':')[0]);
            Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit.Name), data[2].Split(':')[1]);
            int spiritLevel = int.Parse(data[2].Split(':')[2]);
            Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), data[3]);
            string maskColorsString = data[4];
            string[] maskColors = maskColorsString.Split('/');
            Point mapPosition = new Point(Convert.ToInt16(data[5].Split('/')[0]), Convert.ToInt16(data[5].Split('/')[1]));
            int directionLook = int.Parse(data[6]);
            int level = int.Parse(data[7]);
            Enums.AnimatedActions.Name animatedAction = (data[8] != "") ? (Enums.AnimatedActions.Name)Enum.Parse(typeof(Enums.AnimatedActions.Name), data[8]) : Enums.AnimatedActions.Name.idle;
            string waypoint = data[9];

            // verifier si le personnage est deja present pour ne pas créer un doublons, si par erreur le serveur n'envoie pas une cmd de deconnexion du client ou SessionZero
            if (CommonCode.AllActorsInMap.Exists(f => ((Actor)f.tag).pseudo == playerName && ((Actor)f.tag).pseudo != CommonCode.MyPlayerInfo.instance.pseudo))
            {
                Bmp tmpBmp = CommonCode.AllActorsInMap.Find(f => ((Actor)f.tag).pseudo == playerName);
                tmpBmp.visible = false;
                Manager.manager.GfxObjList.RemoveAll(f => f != null && f.GetType() == typeof(Bmp) && f.Tag() != null && f.Tag().GetType() == typeof(Actor) && ((Actor)f.Tag()).pseudo == playerName);
                CommonCode.AllActorsInMap.Remove(CommonCode.AllActorsInMap.Find(f => ((Actor)f.tag).pseudo == playerName));
            }

            // affichage du personnage + position + orientation
            Bmp ibPlayers = new Bmp(@"gfx\general\classes\" + className + ".dat", Point.Empty, "Player_" + playerName, 0, true, 1, SpriteSheet.GetSpriteSheet(className.ToString(), CommonCode.ConvertToClockWizeOrientation(directionLook)));
            ibPlayers.MouseOver += CommonCode.ibPlayers_MouseOver;
            ibPlayers.MouseOut += CommonCode.ibPlayers_MouseOut;
            ibPlayers.MouseMove += CommonCode.CursorHand_MouseMove;
            ibPlayers.MouseClic += CommonCode.ibPlayers_MouseClic;
            Manager.manager.GfxObjList.Add(ibPlayers);

            // attachement des données
            ibPlayers.tag = new Actor(playerName, level, !pvpEnabled ? Enums.Spirit.Name.neutral : spirit, className, pvpEnabled, !pvpEnabled ? 0 : spiritLevel, hiddenVillage, maskColorsString, directionLook, animatedAction, waypoint);
            Actor pi = (Actor)ibPlayers.tag;
            pi.realPosition = mapPosition;

            CommonCode.AdjustPositionAndDirection(ibPlayers, new Point(mapPosition.X * 30, mapPosition.Y * 30));
            CommonCode.VerticalSyncZindex(ibPlayers);

            // affichage des ailles
            if (pi.pvpEnabled)
            {
                if (pi.spirit != Enums.Spirit.Name.neutral)
                {
                    Bmp spiritBmp = new Bmp(@"gfx\general\obj\2\" + pi.spirit + @"\" + pi.spiritLevel + ".dat", Point.Empty, "spirit_" + ibPlayers.name, Manager.TypeGfx.Obj, false, 1);
                    spiritBmp.point = new Point((ibPlayers.rectangle.Width / 2) - (spiritBmp.rectangle.Width / 2), -spiritBmp.rectangle.Height);
                    ibPlayers.Child.Add(spiritBmp);

                    Txt lPseudo = new Txt(pi.pseudo, Point.Empty, "lPseudo_" + ibPlayers.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                    lPseudo.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -spiritBmp.rectangle.Height - 15);
                    ibPlayers.Child.Add(lPseudo);

                    Txt lLvlSpirit = new Txt(pi.spiritLevel.ToString(), Point.Empty, "lLvlSpirit_" + ibPlayers.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Bold), Brushes.Red);
                    lLvlSpirit.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Width / 2) + 2, -spiritBmp.rectangle.Y - (spiritBmp.rectangle.Height / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Height / 2));
                    ibPlayers.Child.Add(lLvlSpirit);

                    Bmp village = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", Point.Empty, "village_" + pi.hiddenVillage, Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet("pays_" + pi.hiddenVillage + "_thumbs", 0));
                    village.point = new Point((ibPlayers.rectangle.Width / 2) - (village.rectangle.Width / 2), lPseudo.point.Y - village.rectangle.Height + 2);
                    ibPlayers.Child.Add(village);
                }
            }
            else
            {
                Txt lPseudo = new Txt(pi.pseudo, Point.Empty, "lPseudo_" + ibPlayers.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                lPseudo.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -15);
                ibPlayers.Child.Add(lPseudo);

                Bmp village = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", Point.Empty, "village_" + pi.hiddenVillage, Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet("pays_" + pi.hiddenVillage + "_thumbs", 0));
                village.point = new Point((ibPlayers.rectangle.Width / 2) - (village.rectangle.Width / 2), lPseudo.point.Y - village.rectangle.Height + 2);
                ibPlayers.Child.Add(village);
            }

            // coloriage selon le MaskColors
            if (maskColors[0] != "null")
            {
                Color tmpColor = Color.FromArgb(Convert.ToInt16(maskColors[0].Split('-')[0]), Convert.ToInt16(maskColors[0].Split('-')[1]), Convert.ToInt16(maskColors[0].Split('-')[2]));
                CommonCode.SetPixelToClass(className, tmpColor, 1, ibPlayers);
            }

            if (maskColors[1] != "null")
            {
                Color tmpColor = Color.FromArgb(Convert.ToInt16(maskColors[1].Split('-')[0]), Convert.ToInt16(maskColors[1].Split('-')[1]), Convert.ToInt16(maskColors[1].Split('-')[2]));
                CommonCode.SetPixelToClass(className, tmpColor, 2, ibPlayers);
            }

            if (maskColors[2] != "null")
            {
                Color tmpColor = Color.FromArgb(Convert.ToInt16(maskColors[2].Split('-')[0]), Convert.ToInt16(maskColors[2].Split('-')[1]), Convert.ToInt16(maskColors[2].Split('-')[2]));
                CommonCode.SetPixelToClass(className, tmpColor, 3, ibPlayers);
            }

            // ajout du joueur dans la liste des joueurs
            CommonCode.ApplyMaskColorToClasse(ibPlayers);
            CommonCode.AllActorsInMap.Add(ibPlayers);
            #endregion
        }
    }
}
