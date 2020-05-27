using System;
using Lidgren.Network;
using System.Collections.Generic;
using SERVER.Enums;

namespace SERVER.Fight
{
    /*public enum BuffState
    {
        // pour choisir l'étape d'envoutement, lors du début ou la fin
        Debut, Fin
    }*/

	public static class spellsChecker
	{
		/*class spellAreaData
		{
			public int index;
			public PlayerInfo pi;
		}*/

        public static bool spells_Of_Invocs(string _spellinfo, Battle _battle, Actor pi, Point spellPos, Actor playerTargeted)
        {
            // cmd = "cmd•spellTuiles•" + sortID + "•" + (e.X / 30) + "•" + (e.Y / 30)
            // _spellinfo = sortID:emplacementID:level:couleur
            //lors de la création d'un nouveau sort, il faut ajouter ses envoutement dans la classe spellckecker sur Ln 1440
            string[] spellInfo = _spellinfo.Split(':');
            int sortID = Convert.ToInt32(spellInfo[0]);
            int spellLevel = Convert.ToInt32(spellInfo[2]);
            int colorID = Convert.ToInt32(spellInfo[3]);
            
            // position du joueur
            Point playerPos = pi.map_position;

            List<Actor> bl = _battle.AllPlayersByOrder.FindAll(f => f.species == Species.Name.Human);
            bl.InsertRange(0, _battle.DeadPlayers.FindAll(f => f.species == Species.Name.Human));

            if (sortID == 4)
            {
                #region sort Pounch
                mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == 4 && f.level == spellLevel - 1); // contiens les infos du sort 4 avec tous les niveau 5, donc 5 instance dans la liste
                // check des cases autorisés par le sort, 3 cases alignés devant le lanceur
                List<Point> allowedPos2 = new List<Point>();
                allowedPos2.Add(new Point(playerPos.X, playerPos.Y - 1));    // case en haut
                allowedPos2.Add(new Point(playerPos.X, playerPos.Y + 1));    // case en bas
                allowedPos2.Add(new Point(playerPos.X + 1, playerPos.Y));    // case a droite
                allowedPos2.Add(new Point(playerPos.X - 1, playerPos.Y));    // case a gauche

                if (!allowedPos2.Exists(f => f.X == spellPos.X && f.Y == spellPos.Y) || spellPos.X < 0 || spellPos.X >= ScreenManager.TileWidth || spellPos.Y < 0 || spellPos.Y >= ScreenManager.TileHeight)
                {
                    Console.WriteLine("<--cmd•spellTileNotAllowed to " + pi.Pseudo);
                    return false;
                }

                // check si la case selectionné ne pointe pas vers un obstacle non joueur
                if (!playerTargeted.IsFreeCellToSpell(new Point(spellPos.X * 30, spellPos.Y * 30)))
                {
                    Console.WriteLine("<--cmd•spellTileNotAllowed to " + pi.Pseudo);
                    return false;
                }

                string dom = "null";

                // calcule des dom
                //dom = calculateDom(pi, _battle, spellPos, 4);
                bool cdAllowed = Convert.ToBoolean(dom.Split('|')[2].Split(':')[1]);

                //// ajout du sort dans la liste des envoutements systeme
                if (pi.BuffsList.Exists(f => f.SortID == sortID && f.system))
                {
                    // sort trouvé
                    Actor.Buff piEnv = pi.BuffsList.Find(f => f.SortID == sortID && f.system);
                    if (playerTargeted != null)
                        piEnv.playerRoxed.Add(playerTargeted.Pseudo);
                    else
                        piEnv.playerRoxed.Add("null");
                }
                else
                {
                    // ajout du sort dans les envoutements
                    Actor.Buff piEnv1 = new Actor.Buff();
                    piEnv1.SortID = sortID;
                    piEnv1.title = spell.spellName;
                    piEnv1.Debuffable = false;
                    piEnv1.VisibleToPlayers = false;
                    if (playerTargeted != null)
                        piEnv1.playerRoxed.Add(playerTargeted.Pseudo);
                    else
                        piEnv1.playerRoxed.Add("null");
                    piEnv1.relanceInterval = spell.relanceInterval;
                    piEnv1.BuffState = Enums.Buff.State.Fin;
                    piEnv1.relanceParTour = spell.relanceParTour;
                    piEnv1.system = true;
                    piEnv1.Cd = cdAllowed;
                    piEnv1.Player = pi.Pseudo;
                    pi.BuffsList.Add(piEnv1);
                }
                //////////////////

                // diminution des pc
                pi.currentPc -= spell.pc;

                // envoie d'une cmd a tous les abonnés au combats
                for (int cnt = 0; cnt < bl.Count; cnt++)
                {
                    // envoie d'une requette a tous les personnages
                    NetConnection nim = MainClass.netServer.Connections.Find(f => f.Tag != null && (f.Tag as Actor).Pseudo == bl[cnt].Pseudo);
                    if (nim != null)
                    {
                        CommonCode.SendMessage("cmd•spellTileGranted•" + pi.Pseudo + "•" + sortID + "•" + spellPos.X + "•" + spellPos.Y + "•" + colorID + "•" + spellLevel + "•" + dom + "•PcUsed:" + spell.pc, nim, true);
                        Console.WriteLine("<--cmd•spellTileGranted•" + pi.Pseudo + "•" + sortID + "•" + spellPos.X + "•" + spellPos.Y + "•" + colorID + "•" + spellLevel + "•" + dom + "•PcUsed:" + spell.pc);
                    }
                }

                // voir si un joueur est mort
                if (dom != "null")
                    for (int cnt = 0; cnt < dom.Split('#').Length; cnt++)
                    {
                        string dom2 = dom.Split('#')[cnt];
                        string deadPlayer = dom2.Split('|')[5].Split(':')[1];
                        if (deadPlayer != "")
                        {
                            // supprimer le joueur de la liste des joueurs actifs
                            // check si le combat est terminé   
                            Actor _playerTargeted = _battle.DeadPlayers.Find(f => f.Pseudo == deadPlayer);
                            if (CommonCode.IsClosedBattle(_battle, _playerTargeted))
                                _battle.State = Enums.battleState.state.closed;
                            else
                                Console.WriteLine("nothing !!!");
                        }
                    }
                return true;
                #endregion
            }
            else
                return false;
        }
        public static bool spells(int spellID, Point spellPos, NetIncomingMessage im)
        {
            // cmd = "cmd•spellTuiles•" + sortID + "•" + (e.X / 30) + "•" + (e.Y / 30)
            // _spellinfo = sortID:emplacementID:level:couleur|...
            //lors de la création d'un nouveau sort, il faut ajouter ses envoutement dans la classe spellckecker sur Ln 1440
            Actor.SpellsInformations infos_sorts = (im.SenderConnection.Tag as Actor).sorts.Find(f => f.SpellId == spellID);
            Battle _battle = Battle.Battles.Find(f => f.IdBattle == (im.SenderConnection.Tag as Actor).idBattle);
            Actor spellCaster = _battle.AllPlayersByOrder.Find(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo);
            Point playerPos = spellCaster.map_position;
            Actor playerTargeted = _battle.AllPlayersByOrder.Find(f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y);
            mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).FindLast(f => f.spellID == infos_sorts.SpellId && f.level == infos_sorts.Level);
            NetConnection nim = MainClass.netServer.Connections.Find(f => f.Tag != null && (f.Tag as Actor).Pseudo == spellCaster.Pseudo);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ////////////// common checks //////////////
            #region PC check
            // check si le joueur dispose des pc necessaires
            if (spellCaster.currentPc < spell.pc)
            {
                CommonCode.SendMessage("cmd•spellNotEnoughPc", im, true);
                Console.WriteLine("<--cmd•spellNotEnoughPc to " + spellCaster.Pseudo);
                return false;
            }
            #endregion

            #region check target
            List<Enums.spell_effect_target.targets> spell_Targets = CommonCode.SpellTarget(spell);
            Enums.spell_effect_target.targets target = CommonCode.TargetParser(spellCaster, playerTargeted);
            
            if(!spell_Targets.Exists(f => f == target))
            {
                CommonCode.SendMessage("cmd•spellTargetNotAllowed", im, true);
                return false;
            }
            #endregion

            #region buff check
            if (spellCaster.BuffsList.Exists(f => f.SortID == spellID && f.system))
            {
                Actor.Buff piEnv = spellCaster.BuffsList.Find(f => f.SortID == spellID && f.system);

                if (piEnv.relanceInterval < spell.relanceInterval)
                {
                    // check contre relanceInterval si il a déja été lance dans un tours précédent, d'où le relanceInterval ne dois pas égale celui attribué la 1er fois, pour laisser le joueur lancer le sort plusieurs fois le tour actuel
                    CommonCode.SendMessage("cmd•spellIntervalNotReached", im, true);
                    return false;
                }
                else if (spell.relanceParTour > 0 && piEnv.playerRoxed.Count >= spell.relanceParTour)
                {
                    // check contre le max de ralance par tour "relanceParTour", si = 0 donc illimité
                    CommonCode.SendMessage("cmd•spellRelanceParTourReached", im, true);
                    return false;
                }
                else if (spell.relanceParJoueur > 0 && playerTargeted != null && piEnv.playerRoxed.FindAll(f => f == playerTargeted.Pseudo).Count >= spell.relanceParJoueur)
                {
                    // check contre relanceParJoueur s'il a déja été lancé sur le même adversaire, si = 0 donc illimité
                    CommonCode.SendMessage("cmd•spellRelanceParJoueurReached", im, true);
                    return false;
                }
            }
            #endregion

            #region check EffectBase Conditions (loop all effects)
            List<Actor.effects> effects = Cryptography.crypted_data.effects_decoder(spell.spellID, spell.level);
            foreach (Actor.effects e in effects)
            {
                Type effectBaseType = Type.GetType("SERVER.Effects.EffectBase." + e.base_effect);
                if(effectBaseType != null)
                {
                    System.Reflection.MethodInfo effectBaseMethod = effectBaseType.GetMethod("Apply");
                    object[] parametersEB = new object[3];
                    parametersEB[0] = im;
                    parametersEB[1] = spellPos;
                    parametersEB[2] = spellID;

                    if(effectBaseMethod != null)
                    {
                        if (!(bool)(effectBaseMethod.Invoke(null, new object[] { parametersEB })))
                            return false;
                    }
                }
            }

            #endregion

            #region specific check to TypeEffect sert a rien pour le moment
            Type t1 = Type.GetType("SERVER.Effects.TypeEffect." + spell.typeEffect);
            
            if (t1 != null)
            {
                System.Reflection.MethodInfo spellChecker2 = t1.GetMethod("Apply");
                object[] parameters2 = new object[3];
                parameters2[0] = im;
                parameters2[1] = spellPos;
                parameters2[2] = spellID;
                
                if (spellChecker2 != null)
                {
                    if (!(bool)(spellChecker2.Invoke(null, new object[] { parameters2 })))
                        return false;
                }
                else
                    Console.WriteLine("no handler method called 'Apply' in 'SERVER.Effects.TypeEffect." + spell.typeEffect + "' found for current spell [" + spellID + "][" + spell.spellName + "]");
            }
            else
                Console.WriteLine("no handler Class Effect called '" + spell.typeEffect + "' in 'SERVER.Effects.TypeEffect." + spell.typeEffect + "' found for current spell [" + spellID + "][" + spell.spellName + "]");
            #endregion

            #region contrôle de portée
            int spell_scoop = CommonCode.scoop_calculator_standard_algo(spellCaster.map_position, spellPos);
            if (Convert.ToBoolean(spell.peModifiable))
            {
                if (spell_scoop > spellCaster.pe + spell.distanceFromMelee + spell.pe)
                {
                    CommonCode.SendMessage("cmd•spellPointNotEnoughPe", im, true);
                    Console.WriteLine("<--cmd•spellPointNotEnoughPe to " + spellCaster.Pseudo);
                    return false;
                }
            }
            else
            {
                if (spell_scoop > spell.distanceFromMelee + spell.pe)
                {
                    CommonCode.SendMessage("cmd•spellPointNotEnoughPe", im, true);
                    Console.WriteLine("<--cmd•spellPointNotEnoughPe to " + spellCaster.Pseudo);
                    return false;
                }
            }
            #endregion

            #region check accessibilité de la tuile, ca ne devrai pas avoir un obstacle non joueur
            if (!spellCaster.IsFreeCellToSpell(new Point(spellPos.X * 30, spellPos.Y * 30)))
            {
                CommonCode.SendMessage("cmd•spellTileNotAllowed", im, true);
                Console.WriteLine("<--cmd•spellTileNotAllowed to " + spellCaster.Pseudo);
                return false;
            }
            
            if(Convert.ToBoolean(spell.ligneDeVue) && !spellCaster.IsFreeCellToWalk(new Point(spellPos.X * 30, spellPos.Y * 30)))
            {
                CommonCode.SendMessage("cmd•spellTileNotAllowed", im, true);
                Console.WriteLine("<--cmd•spellTileNotAllowed to " + spellCaster.Pseudo);
                return false;
            }
            #endregion

            #region contrôle LDV
            Type peZone = Type.GetType("SERVER.Fight.LDVChecker." + spell.typeEffect);
            Enums.LDVChecker.Availability availability;

            if (peZone != null)
            {
                System.Reflection.MethodInfo peZoneChecker = peZone.GetMethod("Apply");

                object[] parameters = new object[3];
                parameters[0] = spellCaster;
                parameters[1] = spellPos;
                parameters[2] = spell;

                if (peZoneChecker != null)
                    availability = (Enums.LDVChecker.Availability)(peZoneChecker.Invoke(null, new object[] { parameters }));
                else
                {
                    Console.WriteLine("no handler method called 'Apply()' for LDVChecker TypeEffect in 'SERVER.Fight.LDVChecker" + spell.typeEffect + "'");
                    availability = Enums.LDVChecker.Availability.nan;
                }
            }
            else
            {
                Console.WriteLine("no handler class for 'SERVER.Fight.LDVChecker " + spell.typeEffect + "'");
                availability = Enums.LDVChecker.Availability.nan;
            }

            if (availability == Enums.LDVChecker.Availability.allowed)
            {
                // spell autorisé
            }
            else if (availability == Enums.LDVChecker.Availability.notAllowed)
            {
                // spell non autorisé, case obstacle
                CommonCode.SendMessage("cmd•spellTileNotAllowed", nim, true);
                Console.WriteLine("<--cmd•spellTileNotAllowed to " + spellCaster.Pseudo);
                return false;
            }
            else if (availability ==  Enums.LDVChecker.Availability.outSide)
            {
                // spell non autorisé, pas de porté
                CommonCode.SendMessage("cmd•spellPointNotEnoughPe", nim, true);
                return false;
            }
            else if (availability == Enums.LDVChecker.Availability.nan)
            {
                // impossible de determiner la direction, normalement ca deverai le deviner
                return false;
            }
            #endregion

            #region check distanceFromMelee (déjà fait dans le contrôle LDV)
            /*if (spell.distanceFromMelee != 0)
            {
                #region distanceFromMelee check
                // check si le sort ne se trouve pas dans l'une des cases à coté du personnage
                // parce que le sort ne se lance qu'à partir de certaines cases à distance du joueur
                
                if (spellPos.X > playerPos.X && spellPos.X - playerPos.X <= spell.distanceFromMelee)
                {
                    // direction a droite
                    common.SendMessage("cmd•spellPointNotEnoughPe", im, true);
                    Console.WriteLine("<--cmd•spellPointNotEnoughPe to " + spellCaster.Pseudo);
                    return false;
                }
                else if (spellPos.X < playerPos.X && playerPos.X - spellPos.X <= spell.distanceFromMelee)
                {
                    // direction a gauche
                    common.SendMessage("cmd•spellPointNotEnoughPe", im, true);
                    Console.WriteLine("<--cmd•spellPointNotEnoughPe to " + spellCaster.Pseudo);
                    return false;
                }
                else if (spellPos.Y < playerPos.Y && playerPos.Y - spellPos.Y <= spell.distanceFromMelee)
                {
                    // direction en haut
                    common.SendMessage("cmd•spellPointNotEnoughPe", im, true);
                    Console.WriteLine("<--cmd•spellPointNotEnoughPe to " + spellCaster.Pseudo);
                    return false;
                }
                else if (spellPos.Y > playerPos.Y && spellPos.Y - playerPos.Y <= spell.distanceFromMelee)
                {
                    // direction en haut
                    common.SendMessage("cmd•spellPointNotEnoughPe", im, true);
                    Console.WriteLine("<--cmd•spellPointNotEnoughPe to " + spellCaster.Pseudo);
                    return false;
                }
                #endregion
            }*/
            #endregion

            #region specific handler / check to spell
            Type t2 = Type.GetType("SERVER.Spells." + Enum.Parse(typeof(Enums.SpellID.spellID), spellID.ToString()).ToString());
            if(t2 != null)
            {
                System.Reflection.MethodInfo spellChecker = t2.GetMethod("spellChecker");

                object[] parameters = new object[3];
                parameters[0] = spellCaster;
                parameters[1] = spellPos;
                parameters[2] = im;

                if (spellChecker != null)
                {
                    if (!(bool)(spellChecker.Invoke(null, new object[] { parameters })))
                        return false;
                }
                else
                {
                    Console.WriteLine("no handler method called 'spellChecker' in 'SERVER.Spells." + Enum.Parse(typeof(Enums.SpellID.spellID), spellID.ToString()) + "' found for current spell [" + spellID + "][" + spell.spellName + "]");
                }
            }
            #endregion
            
            return true;
        }
		public static Int32 isAllowedSpellArea(int pe, Point playerPos, Actor pi, Battle _battle, Point spellPos, int cacDistance, bool blockView, bool inEmptyTileOnly)
		{
            //////////////////////////////////////////////////////////////////////////
            List<Point> allTuiles = new List<Point>();      // liste qui contiens tous les tuiles affecté par le sort y compris un obstacle ou pas
			List<SortTuileInfo> allTuilesInfo = new List<SortTuileInfo>();

			// 1ere partie consiste a afficher toute la grille de tuiles
			for (int line = 0; line <= pe + cacDistance; line++)
			{
				// insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le bas
				if (line != 0 && playerPos.Y + line < ScreenManager.TileHeight)
				{
					Point p = new Point(playerPos.X, playerPos.Y + line);
					allTuiles.Add(p);

					if (!pi.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || _battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
					{
						SortTuileInfo sti = new SortTuileInfo();
						sti.TuilePoint = p;
						if (_battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
						{
							sti.IsWalkable = true;
							sti.IsBlockingView = true;
						}
						else
						{
							sti.IsWalkable = false;
							sti.IsBlockingView = true;
						}
						allTuilesInfo.Add(sti);
					}
					else
					{
						SortTuileInfo sti = new SortTuileInfo();
						sti.TuilePoint = p;
						sti.IsWalkable = true;
						sti.IsBlockingView = false;
						allTuilesInfo.Add(sti);
					}
				}

				// insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le haut
				if (line != 0 && playerPos.Y - line >= 0)
				{
					Point p = new Point(playerPos.X, playerPos.Y - line);
					allTuiles.Add(p);

					if (!pi.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || _battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
					{
						SortTuileInfo sti = new SortTuileInfo();
						sti.TuilePoint = p;
						if (_battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
						{
							sti.IsWalkable = true;
							sti.IsBlockingView = true;
						}
						else
						{
							sti.IsWalkable = false;
							sti.IsBlockingView = true;
						}
						allTuilesInfo.Add(sti);
					}
					else
					{
						SortTuileInfo sti = new SortTuileInfo();
						sti.TuilePoint = p;
						sti.IsWalkable = true;
						sti.IsBlockingView = false;
						allTuilesInfo.Add(sti);
					}
				}

				if (pe == line)
					break;

				for (int side = 1; side <= pe; side++)
				{
					// ajouter des tuiles coté en bas a droite
                    if (playerPos.X + side < ScreenManager.TileWidth && playerPos.Y + line < ScreenManager.TileHeight)
					{
						Point p = new Point(playerPos.X + side, playerPos.Y + line);
						allTuiles.Add(p);

						if (!pi.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || _battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
						{
							SortTuileInfo sti = new SortTuileInfo();
							sti.TuilePoint = p;
							if (_battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
							{
								sti.IsWalkable = true;
								sti.IsBlockingView = true;
							}
							else
							{
								sti.IsWalkable = false;
								sti.IsBlockingView = true;
							}
							allTuilesInfo.Add(sti);
						}
						else
						{
							SortTuileInfo sti = new SortTuileInfo();
							sti.TuilePoint = p;
							sti.IsWalkable = true;
							sti.IsBlockingView = false;
							allTuilesInfo.Add(sti);
						}
					}

					// ajouter des tuiles coté en bas a gauche
                    if (playerPos.X - side < ScreenManager.TileWidth && playerPos.Y + line < ScreenManager.TileHeight)
					{
						Point p = new Point(playerPos.X - side, playerPos.Y + line);
						allTuiles.Add(p);

						if (!pi.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || _battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
						{
							SortTuileInfo sti = new SortTuileInfo();
							sti.TuilePoint = p;
							if (_battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
							{
								sti.IsWalkable = true;
								sti.IsBlockingView = true;
							}
							else
							{
								sti.IsWalkable = false;
								sti.IsBlockingView = true;
							}
							allTuilesInfo.Add(sti);
						}
						else
						{
							SortTuileInfo sti = new SortTuileInfo();
							sti.TuilePoint = p;
							sti.IsWalkable = true;
							sti.IsBlockingView = false;
							allTuilesInfo.Add(sti);
						}
					}

					// ajouter des tuiles coté en haut a droite
                    if (playerPos.X + side < ScreenManager.TileWidth && playerPos.Y - line < ScreenManager.TileHeight && line > 0)
					{
						Point p = new Point(playerPos.X + side, playerPos.Y - line);
						allTuiles.Add(p);

						if (!pi.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || _battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
						{
							SortTuileInfo sti = new SortTuileInfo();
							sti.TuilePoint = p;
							if (_battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
							{
								sti.IsWalkable = true;
								sti.IsBlockingView = true;
							}
							else
							{
								sti.IsWalkable = false;
								sti.IsBlockingView = true;
							}
							allTuilesInfo.Add(sti);
						}
						else
						{
							SortTuileInfo sti = new SortTuileInfo();
							sti.TuilePoint = p;
							sti.IsWalkable = true;
							sti.IsBlockingView = false;
							allTuilesInfo.Add(sti);
						}
					}

					// ajouter des tuiles coté en haut a gauche
                    if (playerPos.X - side < ScreenManager.TileWidth && playerPos.Y - line < ScreenManager.TileHeight && line > 0)
					{
						Point p = new Point(playerPos.X - side, playerPos.Y - line);
						allTuiles.Add(p);

						if (!pi.IsFreeCellToSpell(new Point(p.X * 30, p.Y * 30)) || _battle.AllPlayersByOrder.Exists(f => f.map_position.X == p.X && f.map_position.Y == p.Y))
						{
							SortTuileInfo sti = new SortTuileInfo();
							sti.TuilePoint = p;
							if (_battle.AllPlayersByOrder.FindAll(f => f.map_position.X == p.X && f.map_position.Y == p.Y).Count > 0)
							{
								sti.IsWalkable = true;
								sti.IsBlockingView = true;
							}
							else
							{
								sti.IsWalkable = false;
								sti.IsBlockingView = true;
							}
							allTuilesInfo.Add(sti);
						}
						else
						{
							SortTuileInfo sti = new SortTuileInfo();
							sti.TuilePoint = p;
							sti.IsWalkable = true;
							sti.IsBlockingView = false;
							allTuilesInfo.Add(sti);
						}
					}
					// determiner si le nombre de tuiles attein à cause de la notion qui vaux 1 tuile diagonale vaux 2 tuile, donc on déduit 1pe de chaque ligne
					if (side + line == pe)
						break;
				}
			}

            
            // mise en mode isWalkable = false des tuiles obstacle mais qui laisse la ligne de vue comme meme comme de l'eau ...
            List<SortTuileInfo> lsti = allTuilesInfo.FindAll(f => f.IsWalkable == true && f.IsBlockingView == false);
			for (int i = 0; i < lsti.Count; i++)
				if (!pi.IsFreeCellToSpell(new Point(lsti[i].TuilePoint.X * 30, lsti[i].TuilePoint.Y * 30)) && _battle.AllPlayersByOrder.Exists(f => f.map_position.X == lsti[i].TuilePoint.X && f.map_position.Y == lsti[i].TuilePoint.Y) && lsti[i].IsWalkable == true && lsti[i].IsBlockingView == false)
					allTuilesInfo.Find(f => f.TuilePoint == lsti[i].TuilePoint && f.IsWalkable == true && lsti[i].IsBlockingView == false).IsWalkable = false;

            // il faut mettre la position des joueur comme accessible mais qui garde toujours le isWalking = true
            // .....
            // .....
            ///////////

            if (blockView)
            {
				//////////////////// algo pour determiner se que les obstacles peuvent cacher comme ligne de vue
				// determination de la liste de tous les obstacles
				List<SortTuileInfo> block_view_tile = allTuilesInfo.FindAll (f => f.IsBlockingView == true);

				// calcules préliminaires
				for (int i = 0; i < block_view_tile.Count; i++) {
					// determination de la distance entre la position du joueur et l'obstacle
					int xDistance = Math.Abs (playerPos.X - block_view_tile [i].TuilePoint.X);
					int yDistance = Math.Abs (playerPos.Y - block_view_tile [i].TuilePoint.Y);

					// determiner le niveau d'envergure de l'angle par tuile
					float AngleA = 0;
					float AngleB = 0;

					// determination de ladirection de l'angle
					bool rightDirection = false;
					bool leftDirection = false;
					bool downDirection = false;
					bool upDirection = false;

					if (playerPos.X > block_view_tile [i].TuilePoint.X)
						leftDirection = true;
					else if (playerPos.X < block_view_tile [i].TuilePoint.X)
						rightDirection = true;

					if (playerPos.Y > block_view_tile [i].TuilePoint.Y)
						upDirection = true;
					else if (playerPos.Y < block_view_tile [i].TuilePoint.Y)
						downDirection = true;

					// point de départ du polygone
					Point pointAOfObstacle = new Point ();
					Point pointBOfObstacle = new Point ();

					// coordonnées de l'obstacle en cours de vérifications selons la position du joueur
					if (upDirection && playerPos.X != block_view_tile [i].TuilePoint.X && playerPos.Y != block_view_tile [i].TuilePoint.Y) {
						// calcule des point d'intersection avec l'obstacle pour tracer la ligne
						pointAOfObstacle.X = (rightDirection) ? block_view_tile [i].TuilePoint.X : block_view_tile [i].TuilePoint.X + 1;
						pointAOfObstacle.Y = block_view_tile [i].TuilePoint.Y;

						pointBOfObstacle.X = (rightDirection) ? block_view_tile [i].TuilePoint.X + 1 : block_view_tile [i].TuilePoint.X;
						pointBOfObstacle.Y = block_view_tile [i].TuilePoint.Y + 1;
					} else if (downDirection && playerPos.X != block_view_tile [i].TuilePoint.X && playerPos.Y != block_view_tile [i].TuilePoint.Y) {
						// calcule des point d'intersection avec l'obstacle pour tracer la ligne
						pointAOfObstacle.X = (rightDirection) ? block_view_tile [i].TuilePoint.X + 1 : block_view_tile [i].TuilePoint.X;
						pointAOfObstacle.Y = block_view_tile [i].TuilePoint.Y;

						pointBOfObstacle.X = (rightDirection) ? block_view_tile [i].TuilePoint.X : block_view_tile [i].TuilePoint.X + 1;
						pointBOfObstacle.Y = block_view_tile [i].TuilePoint.Y + 1;
					} else if ((upDirection || downDirection) && !rightDirection && !leftDirection) {
						// le joueur est aligné horizontalement
						pointAOfObstacle.X = block_view_tile [i].TuilePoint.X;
						pointAOfObstacle.Y = (upDirection) ? block_view_tile [i].TuilePoint.Y + 1 : block_view_tile [i].TuilePoint.Y;

						pointBOfObstacle.X = block_view_tile [i].TuilePoint.X + 1;
						pointBOfObstacle.Y = (upDirection) ? block_view_tile [i].TuilePoint.Y + 1 : block_view_tile [i].TuilePoint.Y;
					} else if ((rightDirection || leftDirection) && !upDirection && !downDirection) {
						// le joueur est aligné horizontalement
						pointAOfObstacle.X = (rightDirection) ? block_view_tile [i].TuilePoint.X : block_view_tile [i].TuilePoint.X + 1;
						pointAOfObstacle.Y = block_view_tile [i].TuilePoint.Y;

						pointBOfObstacle.X = (rightDirection) ? block_view_tile [i].TuilePoint.X : block_view_tile [i].TuilePoint.X + 1;
						pointBOfObstacle.Y = block_view_tile [i].TuilePoint.Y + 1;
					}

					// calcule de l'envergure de l'angle par tuile passé
					if (upDirection && (rightDirection || leftDirection)) {
						AngleA = ((yDistance * 30) + 15) / (xDistance - 0.5F);
						AngleB = (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F);
					} else if (downDirection && (rightDirection || leftDirection)) {
						AngleA = (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F);
						AngleB = ((yDistance * 30) + 15) / (xDistance - 0.5F);
					} else if (!downDirection && !upDirection && (rightDirection || leftDirection)) {
						AngleA = ((yDistance * 30) + 15) / ((xDistance + 1) - 0.5F);   // direction vers la gauche
						AngleB = ((yDistance * 30) + 15) / ((xDistance + 1) - 0.5F);   // direction vers la droite
					} else if (!rightDirection && !leftDirection && (upDirection || downDirection)) {
						AngleA = ((yDistance * 30) + 15) / (yDistance - 0.5F);   // direction vers la gauche
						AngleB = ((yDistance * 30) + 15) / (yDistance - 0.5F);   // direction vers la droite
					}

					AngleA = Math.Abs ((upDirection) ? ((yDistance * 30) + 15) / (xDistance - 0.5F) : (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F));
					AngleB = Math.Abs ((upDirection) ? (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F) : ((yDistance * 30) + 15) / (xDistance - 0.5F));

					List<Point> nextPointAL = new List<Point> ();
					List<Point> nextPointBL = new List<Point> ();

					for (int j = 1; j <= pe; j++) {
						if (upDirection && rightDirection)
							nextPointAL.Add (new Point ((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
						else if (upDirection && leftDirection)
							nextPointAL.Add (new Point ((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
						else if (downDirection && rightDirection)
							nextPointAL.Add (new Point ((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
						else if (downDirection && leftDirection)
							nextPointAL.Add (new Point ((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
						else if (upDirection && !rightDirection && !leftDirection && !downDirection)
							nextPointAL.Add (new Point ((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
						else if (downDirection && !rightDirection && !leftDirection && !upDirection)
							nextPointAL.Add (new Point ((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
						else if (rightDirection && !downDirection && !upDirection && !leftDirection)
							nextPointAL.Add (new Point ((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
						else if (leftDirection && !downDirection && !upDirection && !rightDirection)
							nextPointAL.Add (new Point ((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
						else
							return -1;
					}

					for (int j = 1; j <= pe; j++) {
						if (upDirection && rightDirection)
							nextPointBL.Add (new Point ((pointBOfObstacle.X * 30) + (j * 30) + 30, (pointBOfObstacle.Y * 30) - (int)(AngleB * j)));
						else if (upDirection && leftDirection)
							nextPointBL.Add (new Point ((pointBOfObstacle.X * 30) - (j * 30) - 30, (pointBOfObstacle.Y * 30) - (int)(AngleB * j)));
						else if (downDirection && rightDirection)
							nextPointBL.Add (new Point ((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleB * j)));
						else if (downDirection && leftDirection)
							nextPointBL.Add (new Point ((pointBOfObstacle.X * 30) - (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleB * j)));
						else if (upDirection && !rightDirection && !leftDirection && !downDirection)
							nextPointBL.Add (new Point ((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) - (int)(AngleA * j)));
						else if (downDirection && !rightDirection && !leftDirection && !upDirection)
							nextPointBL.Add (new Point ((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
						else if (rightDirection && !upDirection && !downDirection && !leftDirection)
							nextPointBL.Add (new Point ((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
						else if (leftDirection && !upDirection && !downDirection && !rightDirection)
							nextPointBL.Add (new Point ((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
						else
							return -1;
					}

					// tracage du cadre qui délimite les tuiles entre le joueur et le champ de vision
					for (int a = 0; a < allTuilesInfo.Count; a++) {
						if (allTuilesInfo [a].TuilePoint == block_view_tile [i].TuilePoint)
							continue;
						// check si la tuile en cours se trouve entre les angles A et B
						if (upDirection && rightDirection && !leftDirection) {
							if (allTuilesInfo [a].TuilePoint.X >= block_view_tile [i].TuilePoint.X && allTuilesInfo [a].TuilePoint.Y <= block_view_tile [i].TuilePoint.Y) {
								// la tuile est dans le rectangle superieur à droite du joueur
								// determination du distance entre l'obstacle et la position du tuile en cours
								int x = Math.Abs (allTuilesInfo [a].TuilePoint.X - block_view_tile [i].TuilePoint.X);
								int y = Math.Abs (block_view_tile [i].TuilePoint.Y - allTuilesInfo [a].TuilePoint.Y);
								// cheque si la tuile en cours est en haut de l'angle B, puis 2éme check pour l'angle A
								if ((float)y < ((AngleA * (x + 1)) / 30))
								if ((y + 1) > Math.Floor (((AngleB * x) / 30))) {
									allTuilesInfo [a].IsBlockingView = true;
									allTuilesInfo [a].IsWalkable = false;
								}
							}
						} else if (upDirection && !rightDirection && leftDirection) {
							if (allTuilesInfo [a].TuilePoint.X <= block_view_tile [i].TuilePoint.X && allTuilesInfo [a].TuilePoint.Y <= block_view_tile [i].TuilePoint.Y) {
								// la tuile est dans le rectangle superieur à gauche du joueur
								// determination du distance entre l'obstacle et la position du tuile en cours
								int x = Math.Abs (allTuilesInfo [a].TuilePoint.X - block_view_tile [i].TuilePoint.X);
								int y = Math.Abs (block_view_tile [i].TuilePoint.Y - allTuilesInfo [a].TuilePoint.Y);
								// cheque si la tuile en cours est en haut de l'angle B, puis 2éme check pour l'angle A
								if ((float)y < ((AngleA * (x + 1)) / 30))
								if ((y + 1) > Math.Floor (((AngleB * x) / 30))) {
									allTuilesInfo [a].IsBlockingView = true;
									allTuilesInfo [a].IsWalkable = false;
								}
							}
						} else if (downDirection && !rightDirection && leftDirection) {
							if (allTuilesInfo [a].TuilePoint.X <= block_view_tile [i].TuilePoint.X && allTuilesInfo [a].TuilePoint.Y >= block_view_tile [i].TuilePoint.Y) {
								// la tuile est dans le rectangle inférieure à droite du joueur
								// determination du distance entre l'obstacle et la position du tuile en cours
								int x = Math.Abs (allTuilesInfo [a].TuilePoint.X - block_view_tile [i].TuilePoint.X);
								int y = Math.Abs (block_view_tile [i].TuilePoint.Y - allTuilesInfo [a].TuilePoint.Y);
								// cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
								if ((float)(y) < ((AngleB * (x + 1)) / 30))
								if ((float)(y + 1) > Math.Floor (((AngleA * (x)) / (float)30))) {
									allTuilesInfo [a].IsBlockingView = true;
									allTuilesInfo [a].IsWalkable = false;
								}
							}
						} else if (downDirection && rightDirection && !leftDirection) {
							if (allTuilesInfo [a].TuilePoint.X >= block_view_tile [i].TuilePoint.X && allTuilesInfo [a].TuilePoint.Y >= block_view_tile [i].TuilePoint.Y) {
								// la tuile est dans le rectangle inférieure à droite du joueur
								// determination du distance entre l'obstacle et la position du tuile en cours
								int x = Math.Abs (allTuilesInfo [a].TuilePoint.X - block_view_tile [i].TuilePoint.X);
								int y = Math.Abs (block_view_tile [i].TuilePoint.Y - allTuilesInfo [a].TuilePoint.Y);
								// cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
								if ((float)(y) < ((AngleB * (x + 1)) / 30))
								if ((float)(y + 1) > Math.Floor (((AngleA * (x)) / (float)30))) {
									allTuilesInfo [a].IsBlockingView = true;
									allTuilesInfo [a].IsWalkable = false;
								}
							}
						} else if (rightDirection && !leftDirection && !upDirection && !downDirection) {
							if (allTuilesInfo [a].TuilePoint.X >= block_view_tile [i].TuilePoint.X) {
								int x = Math.Abs (allTuilesInfo [a].TuilePoint.X - block_view_tile [i].TuilePoint.X);
								int y = Math.Abs (block_view_tile [i].TuilePoint.Y - allTuilesInfo [a].TuilePoint.Y);
								// cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
								if ((float)y < ((AngleB * (x + 1)) / 30)) {
									allTuilesInfo [a].IsBlockingView = true;
									allTuilesInfo [a].IsWalkable = false;
								}
							}
						} else if (leftDirection && !rightDirection && !upDirection && !downDirection) {
							if (allTuilesInfo [a].TuilePoint.X <= block_view_tile [i].TuilePoint.X) {
								int x = Math.Abs (allTuilesInfo [a].TuilePoint.X - block_view_tile [i].TuilePoint.X);
								int y = Math.Abs (block_view_tile [i].TuilePoint.Y - allTuilesInfo [a].TuilePoint.Y);
								// cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
								if ((float)y < ((AngleB * (x + 1)) / 30)) {
									allTuilesInfo [a].IsBlockingView = true;
									allTuilesInfo [a].IsWalkable = false;
								}
							}
						} else if (upDirection && !downDirection && !leftDirection && !rightDirection) {
							if (allTuilesInfo [a].TuilePoint.Y <= block_view_tile [i].TuilePoint.Y) {
								int x = Math.Abs (allTuilesInfo [a].TuilePoint.X - block_view_tile [i].TuilePoint.X);
								int y = Math.Abs (block_view_tile [i].TuilePoint.Y - allTuilesInfo [a].TuilePoint.Y);
								// cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
								if ((float)y >= ((AngleB * (x)) / 30)) {
									allTuilesInfo [a].IsBlockingView = true;
									allTuilesInfo [a].IsWalkable = false;
								}
							}
						} else if (downDirection && !upDirection && !leftDirection && !rightDirection) {
							if (allTuilesInfo [a].TuilePoint.Y >= block_view_tile [i].TuilePoint.Y) {
								int x = Math.Abs (allTuilesInfo [a].TuilePoint.X - block_view_tile [i].TuilePoint.X);
								int y = Math.Abs (block_view_tile [i].TuilePoint.Y - allTuilesInfo [a].TuilePoint.Y);
								// cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
								if ((float)y >= ((AngleA * (x)) / 30)) {
									allTuilesInfo [a].IsBlockingView = true;
									allTuilesInfo [a].IsWalkable = false;
								}
							}
						}
					}
				}
			}

			/////////////// supression des tuiles a coté du joueur si le sort n'accepte pas le cac
			if(cacDistance == 2)
				allTuilesInfo.RemoveAll(f => (f.TuilePoint.X == playerPos.X + 1 && f.TuilePoint.Y == playerPos.Y) || (f.TuilePoint.X == playerPos.X - 1 && f.TuilePoint.Y == playerPos.Y) || (f.TuilePoint.X == playerPos.X + 2 && f.TuilePoint.Y == playerPos.Y) || (f.TuilePoint.X == playerPos.X - 2 && f.TuilePoint.Y == playerPos.Y) || (f.TuilePoint.X == playerPos.X + 1 && f.TuilePoint.Y == playerPos.Y + 1) || (f.TuilePoint.X == playerPos.X - 1 && f.TuilePoint.Y == playerPos.Y + 1) || (f.TuilePoint.X == playerPos.X + 1 && f.TuilePoint.Y == playerPos.Y - 1) || (f.TuilePoint.X == playerPos.X - 1 && f.TuilePoint.Y == playerPos.Y - 1) || (f.TuilePoint.X == playerPos.X && f.TuilePoint.Y == playerPos.Y + 1) || (f.TuilePoint.X == playerPos.X && f.TuilePoint.Y == playerPos.Y - 1) || (f.TuilePoint.X == playerPos.X && f.TuilePoint.Y == playerPos.Y + 2) || (f.TuilePoint.X == playerPos.X && f.TuilePoint.Y == playerPos.Y - 2));

			SortTuileInfo focusedTile = allTuilesInfo.Find(f => f.TuilePoint.X == spellPos.X && f.TuilePoint.Y == spellPos.Y);
			if(focusedTile != null)
			{
				// les obstacle joueurs sont prise en compte par le sort
				if (!inEmptyTileOnly && !focusedTile.IsBlockingView && focusedTile.IsWalkable)
				{
					// traitement quand le sort est autorisé sur cette emplacement
					return 0;
				}
				else
				{
					// traitement quand le sorts n'est pas autorisé, ou la case est un obstale qui est un joueur, walkable = true, blockingview = true
					if (!inEmptyTileOnly && focusedTile.IsWalkable && focusedTile.IsBlockingView && _battle.AllPlayersByOrder.Exists (f => f.map_position.X == spellPos.X && f.map_position.Y == spellPos.Y))
						return 0;
					else if (inEmptyTileOnly && !focusedTile.IsBlockingView && focusedTile.IsWalkable)
						return 0;
					else
						return 1;
				}
			}
			else
			{
				// le sort est lancé sur un emplacement en dehors de la zone, le joueur triche
				return 2;
			}
			////////////////////
		}
        public static void BuffCheck(Actor pi, Battle _battle, Enums.Buff.State envoutementEtap)
        {
            Actor currentPlayer = _battle.AllPlayersByOrder[_battle.Turn];
            // on passe sur tous les envoutements si ils sont a l'étape Fin pour decrementer leurs relanceInterval
            for (int cnt = currentPlayer.BuffsList.Count; cnt > 0; cnt--)
            {
                int SortID = currentPlayer.BuffsList[cnt - 1].SortID;
                // si la relance du sort na pas encore arrivé on décremente
                if (envoutementEtap == Enums.Buff.State.Fin && currentPlayer.BuffsList[cnt - 1].BuffState == Enums.Buff.State.Fin && currentPlayer.BuffsList[cnt - 1].relanceInterval > 0)
                {
                    // traitement unique pour les envoutements systeme non visibles au joueurs
                    if(currentPlayer.BuffsList[cnt - 1].system)
                    {
                        currentPlayer.BuffsList[cnt - 1].relanceInterval--;
                        if(currentPlayer.BuffsList[cnt - 1].relanceInterval > 0)
                        {
                            // interval de relance non atteint
                        }
                        else
                        {
                            // interval de relance atteint
                            // envoutement términé
                            // traitement normal, supression de l'envoutement
                            currentPlayer.BuffsList.RemoveAt(cnt - 1);
                        }
                    }
                    else
                    {
                        /////////////// traitement specifique a un envoutement non systeme d'un sort comme decrementer l'index de tours de bonus / malus
                        // envoutement non systeme, bonnus / malus
                        if (currentPlayer.BuffsList[cnt - 1].Duration > 0)
                        {
                            // interval de relance non atteint
                            if (sorts.sort_de_bonnus.Exists(f => f == SortID))
                            {
                                // decrementation des tours restant pour le bonus
                                currentPlayer.BuffsList[cnt - 1].Duration--;
                            }
                        }
                        else
                        {
                            // détail du sort
                            Actor.SpellsInformations piis = currentPlayer.sorts.Find(f => f.SpellId == SortID);
                            mysql.spells spell = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == SortID && f.level == piis.Level - 1);
                            if (SortID == 7)
                            {
                                currentPlayer.originalPc -= spell.pc;
                                currentPlayer.currentPc -= spell.pc;
                                currentPlayer.BuffsList.RemoveAt(cnt - 1);
                            }
                            else if (SortID == 8)
                            {
                                //currentPlayer.original_Pm -= sorts.sort(SortID).isbl[piis.level - 1].piBonus.original_Pm;
                                //currentPlayer.current_Pm -= sorts.sort(SortID).isbl[piis.level - 1].piBonus.original_Pm;
                                //currentPlayer.EnvoutementsList.RemoveAt(cnt - 1);
                            }
                            else if (SortID == 9)
                            {
                                //currentPlayer.puissance -= sorts.sort(SortID).isbl[piis.level - 1].piBonus.puissance;
                                //currentPlayer.EnvoutementsList.RemoveAt(cnt - 1);
                            }
                            else if (SortID == 10)
                            {
                                //currentPlayer.doton -= sorts.sort(SortID).isbl[piis.level - 1].piBonus.doton;
                                //currentPlayer.EnvoutementsList.RemoveAt(cnt - 1);
                            }
                        }
                    }
                }


            }
        }
	}
}