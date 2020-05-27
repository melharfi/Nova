using System;
using System.Drawing;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class GrabingActorsInformationResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            Txt[] lPseudo = MMORPG.GameStates.SelectPlayer.lPseudo;
            Txt[] lLvlPlayer = MMORPG.GameStates.SelectPlayer.lLvlPlayer;
            Txt[] lLvlSpirit = MMORPG.GameStates.SelectPlayer.lLvlSpirit;
            Bmp[] ibPlayers = MMORPG.GameStates.SelectPlayer.ibPlayers;
            Bmp[] delete = MMORPG.GameStates.SelectPlayer.delete;
            Bmp[] village = MMORPG.GameStates.SelectPlayer.village;

            #region liste les joueurs que le joueur a crée
            string[] players = commandStrings[1].Split('|');

            for (int cnt = 0; cnt < players.Length; cnt++)
            {
                // pseudo,level,spirit,classe,pvp,spiritLvl,village,MaskColors,null,null
                
                string[] states = players[cnt].Split('#');
                string actorName = states[0];
                int level = int.Parse(states[1]);
                Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit.Name), states[2]);
                Enums.ActorClass.ClassName actorClass = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[3]);
                bool pvpEnabled = Convert.ToBoolean(Convert.ToInt16(states[4]));
                int spiritLevel = int.Parse(states[5]);
                Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[6]);
                string maskColorsString = states[7];
                // states 8 + 9 not used

                lPseudo[cnt].Text = actorName;
                ibPlayers[cnt].Child.Add(lPseudo[cnt]);

                lLvlPlayer[cnt].Text = CommonCode.TranslateText(50) + " " + level;


                ibPlayers[cnt].Child.Add(lLvlPlayer[cnt]);

                ibPlayers[cnt].ChangeBmp(@"gfx\general\classes\" + actorClass + ".dat", SpriteSheet.GetSpriteSheet(actorClass.ToString(), 0));
                Manager.manager.GfxObjList.Add(ibPlayers[cnt]);

                ibPlayers[cnt].tag = new Actor(actorName, level, spirit, actorClass, pvpEnabled, spiritLevel, hiddenVillage, maskColorsString, 0, Enums.AnimatedActions.Name.idle, "null");

                //////// alignement neutral ou ange ou demon
                if (spirit != Enums.Spirit.Name.neutral)
                {
                    Bmp spiritBmp = new Bmp(@"gfx\general\obj\2\" + spirit + @"\" + spiritLevel + ".dat", new Point(0, 0), "spirit." + actorName, Manager.TypeGfx.Obj, true, 1);
                    spiritBmp.point = new Point((ibPlayers[cnt].rectangle.Width / 2) - (spiritBmp.rectangle.Width / 2), -spiritBmp.rectangle.Height);
                    ibPlayers[cnt].Child.Add(spiritBmp);

                    lLvlPlayer[cnt].point = new Point((ibPlayers[cnt].rectangle.Width / 2) - ((TextRenderer.MeasureText(lLvlPlayer[cnt].Text, lLvlPlayer[cnt].font).Width) / 2), -spiritBmp.rectangle.Height - 15);
                    lPseudo[cnt].point = new Point((ibPlayers[cnt].rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo[cnt].Text, lPseudo[cnt].font).Width / 2) + 5, lLvlPlayer[cnt].point.Y - 15);
                    lLvlSpirit[cnt].Text = spiritLevel.ToString();
                    lLvlSpirit[cnt].point = new Point((ibPlayers[cnt].rectangle.Width / 2) - (TextRenderer.MeasureText(lLvlSpirit[cnt].Text, lLvlSpirit[cnt].font).Width / 2) + 2, -spiritBmp.rectangle.Y - (spiritBmp.rectangle.Height / 2) - (TextRenderer.MeasureText(lLvlSpirit[cnt].Text, lLvlSpirit[cnt].font).Height / 2));
                    ibPlayers[cnt].Child.Add(lLvlSpirit[cnt]);

                    if (pvpEnabled)
                    {
                        Bmp PvpOn = new Bmp(@"gfx\map\SelectPlayer\obj\1.dat", new Point(ibPlayers[cnt].point.X + (ibPlayers[cnt].rectangle.Width / 2) - 24, ibPlayers[cnt].point.Y + ibPlayers[cnt].rectangle.Height + 30), "PvpOn", Manager.TypeGfx.Obj, true, 1);
                        PvpOn.point = new Point((ibPlayers[cnt].rectangle.Width / 2) - (PvpOn.rectangle.Width / 2), ibPlayers[cnt].rectangle.Height + 30);
                        ibPlayers[cnt].Child.Add(PvpOn);
                    }
                }
                else
                {
                    lLvlPlayer[cnt].point = new Point((ibPlayers[cnt].rectangle.Width / 2) - ((TextRenderer.MeasureText(lLvlPlayer[cnt].Text, lLvlPlayer[cnt].font).Width) / 2), -20);
                    lPseudo[cnt].point = new Point((ibPlayers[cnt].rectangle.Width / 2) - ((TextRenderer.MeasureText(states[0], lPseudo[cnt].font).Width) / 2) + 2, -35);
                }

                village[cnt] = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", new Point(0, 0), "village " + hiddenVillage, Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("pays_" + hiddenVillage + "_thumbs", 0));
                village[cnt].point = new Point((ibPlayers[cnt].rectangle.Width / 2) - (village[cnt].rectangle.Width / 2), lPseudo[cnt].point.Y - village[cnt].rectangle.Height);
                ibPlayers[cnt].Child.Add(village[cnt]);

                // coloriage du personnage
                CommonCode.ApplyMaskColorToClasse(ibPlayers[cnt]);

                delete[cnt].point = new Point((ibPlayers[cnt].rectangle.Width / 2) - (delete[cnt].rectangle.Width / 2), village[cnt].point.Y - 20);
                delete[cnt].visible = true;

                ibPlayers[cnt].name = actorName;
            }
            #endregion
        }
    }
}
