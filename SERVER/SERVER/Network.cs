using System;
using Lidgren.Network;
using System.Collections.Generic;
using SERVER.Enums;
using SERVER.Net.Messages;
using SERVER.Net.Messages.Request;
using SERVER.Net.Messages.Response;
using SERVER.Net.Messages.Response.Map;

namespace SERVER
{
    public static class Network
	{
        private static string[] cmd;                        // pour les cmd recus des clients
		public static string downloadMajorLink; //= "www.mmorpg.com/package/lastversion.html";
		public static string downloadRevLink; //= "www.mmorpg.com/package/update.html";
		
		public static int ChatMessageMaxChar;
		public static int counter = 0;
        public static int maxCharLengthCmd = 500;   // les cmd recus par le clients ne doivents pas dépasser 100 caractères, pas présent sur le menu ini

	    static Network ()
		{

		}

		public static void GetData (NetIncomingMessage im)
		{
            #region parametres
            CommonCode.TimeStampUpdate(im.SenderConnection);
			string tmp = im.ReadString();
            if (tmp.Length > maxCharLengthCmd)
                return;
            Console.WriteLine("(GET)" + tmp);
			cmd = tmp.Split (CommandDelimitterChar.Delimitter);

		    var incomingMessage = Type.GetType("SERVER.Net.Messages.Request." + cmd[0]);
		    if (incomingMessage != null)
		    {
                
		        var iMessageRequest = (IRequestMessage) Activator.CreateInstance(incomingMessage);
                iMessageRequest.Initialize(cmd, im.SenderConnection);
		        if (iMessageRequest.Check())
                    iMessageRequest.Apply();
		        else
		            return;
		    }
		    else
		    {
				Console.WriteLine("cette commande est introuvable, il faut informer le client");
                // cette commande est introuvable, il faut informer le client
                //return;
            }
		    //return;
            ////////////////////////////////////////////////
            /// 
            ///////// verification si l'ip est banné ///////

            if ((DataBase.DataTables.bannedip as List<mysql.bannedip>).Exists(f => f.ip == im.SenderEndPoint.Address.ToString()))
			{
				// client bannie, on cherche si le ban est términé ou pas encore
                mysql.bannedip censureTimeStamp = (DataBase.DataTables.bannedip as List<mysql.bannedip>).Find(f => f.ip == im.SenderEndPoint.Address.ToString());

                int timestamp = 0;
				if(censureTimeStamp != null)
					timestamp = censureTimeStamp.censure;

				if(timestamp > CommonCode.ReturnTimeStamp())
				{
					im.SenderConnection.Disconnect ("YOUR ARE BANNED FOR " + ((timestamp - CommonCode.ReturnTimeStamp()) / 60) + " MIN");
					return;
				}
				else
				{
					// enlévement du ban
                    (DataBase.DataTables.bannedip as List<mysql.bannedip>).RemoveAll(f => f.ip == im.SenderEndPoint.Address.ToString());
				}
			}
            
			/////////////// verification de l'état du serveur ////////////////////////////
			string verState = (DataBase.DataTables.state as List<mysql.state>)[0]._state;

			if (verState == "maintenance")
			{
				im.SenderConnection.Disconnect ("maintenance");
				Console.WriteLine ("<--maintenance to " + (im.SenderConnection.Tag as Actor).Pseudo);
				return;
			}
			else if (verState == "restarting")
			{
				im.SenderConnection.Disconnect ("restarting");
				Console.WriteLine ("<--restarting•restarting to " + (im.SenderConnection.Tag as Actor).Pseudo);
				return;
			}
			else if (verState == "shutdown")
			{
				im.SenderConnection.Disconnect ("shutdown");
				Console.WriteLine ("<--cmd•state•shutdown to " + (im.SenderConnection.Tag as Actor).Pseudo);
				return;
			}
			else
			{
				// server is in idle mode, nothing to do for now
			}

            // verification contre le SpamCMD
            if (cmd.Length > 0 && cmd [0] != typeof(AuthentificationRequestMessage).Name  && (im.SenderConnection.Tag as Actor).Username == "")
			{
				// verifier si le client envoie une cmd alors qu'il n'est méme pas identifié, la seul cmd qui y est autorisé est "identification"
				Console.WriteLine ("Client " + (im.SenderConnection.Tag as Actor).Username + " bannie pour 1 semaine");

				// ajout du client parmis les joueurs banni
				int timestamp = CommonCode.ReturnTimeStamp ();
				int censure = 60 * 60 * 24 * 7;		// 1 semaine

			    mysql.banneduser bannedUser = new mysql.banneduser
			    {
			        user = ((Actor) im.SenderConnection.Tag as Actor).Username,
			        reson = "SpamCMD:LoadingMap",
			        temps = timestamp + censure
			    };
			    ((List<mysql.banneduser>)DataBase.DataTables.banneduser).Add(bannedUser);

                return;
            }

            #endregion

            if (cmd.Length >= 1 && cmd [0] == "cmd")
			{
                if (cmd.Length >= 2 && cmd [1] == "waypoint")
				{
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "")
					{
						Security.User_banne ("waypoint", im.SenderConnection);
						return;
					}
					Battle b = Battle.Battles.Find (f => f.IdBattle == (im.SenderConnection.Tag as Actor).idBattle);
					Actor piib = null;
					if ((im.SenderConnection.Tag as Actor).inBattle == 1)
						piib = b.AllPlayersByOrder.Find (f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo);

					// demande de mouvement
					if (cmd.Length == 4 && cmd [2] == "past")
					{
                        #region
                        // lorsque le joueurs "client" avance, il envoie cette cmd pour indiquer qu'il a passer une tuile
                        // a fin de décrémonter le wayPointList, et de comparer le timeStamp entre chaque mouvement

                        // incrementation du timestamp contre le spam waypoint qui se libere apres 2seconds
                        (im.SenderConnection.Tag as Actor).timeBeforeNextWaypoint = CommonCode.ReturnTimeStamp ();
						// verifier si le joueur a un wayPointList
						if (((im.SenderConnection.Tag as Actor).inBattle == 0 && (im.SenderConnection.Tag as Actor).wayPoint.Count > 0) || ((im.SenderConnection.Tag as Actor).inBattle == 1 && piib.wayPoint.Count > 0))
						{
							try
							{
								int.Parse(cmd [3].Split (',') [0]);
								int.Parse(cmd [3].Split (',') [1]);
							}
							catch
							{
								Security.User_banne ("client inject a non int 'waypoint past'", im.SenderConnection);
								return;
							}

							Point p = new Point (Convert.ToInt32 (cmd [3].Split (',') [0]) / 30 * 30, Convert.ToInt32 (cmd [3].Split (',') [1]) / 30 * 30);

							// verification si c'est la derniere offset pour mettre a jour la position du joueur
							//if (p.X == (im.SenderConnection.Tag as PlayerInfo).wayPoint [0].X && p.Y == (im.SenderConnection.Tag as PlayerInfo).wayPoint [0].Y)
							if(((im.SenderConnection.Tag as Actor).inBattle == 0 && p.X == (im.SenderConnection.Tag as Actor).wayPoint [0].X && p.Y == (im.SenderConnection.Tag as Actor).wayPoint [0].Y) || ((im.SenderConnection.Tag as Actor).inBattle == 1 && p.X == piib.wayPoint [0].X && p.Y == piib.wayPoint [0].Y))
							{
								// modification du way point sur la bd seulement si le joueur n'est pas dans un combat
								if ((im.SenderConnection.Tag as Actor).inBattle == 0)
								{
                                    (DataBase.DataTables.connected as List<mysql.connected>).Find(f => f.pseudo == (im.SenderConnection.Tag as Actor).Pseudo).map_position = ((im.SenderConnection.Tag as Actor).wayPoint[0].X / 30) + "/" + ((im.SenderConnection.Tag as Actor).wayPoint[0].Y / 30);
                                    (DataBase.DataTables.players as List<mysql.players>).Find(f => f.pseudo == (im.SenderConnection.Tag as Actor).Pseudo).map_position = ((im.SenderConnection.Tag as Actor).wayPoint[0].X / 30) + "/" + ((im.SenderConnection.Tag as Actor).wayPoint[0].Y / 30);
								}
								else
								{
									// sustraction de 1 du nombre des pm2
									piib.currentPm--;
								}

								if((im.SenderConnection.Tag as Actor).inBattle == 0)
								{
									(im.SenderConnection.Tag as Actor).map_position = new Point (((im.SenderConnection.Tag as Actor).wayPoint [0].X / 30), ((im.SenderConnection.Tag as Actor).wayPoint [0].Y / 30));
									(im.SenderConnection.Tag as Actor).wayPoint.RemoveAt (0);
								}
								else
								{
									piib.map_position = new Point ((piib.wayPoint [0].X / 30), (piib.wayPoint [0].Y / 30));
									piib.wayPoint.RemoveAt (0);
								}

								// supression de la 1ere position depuis le way point
								// verification du temps passé apres le passage du waypoint
								if((im.SenderConnection.Tag as Actor).inBattle == 0)
								{
									if ((im.SenderConnection.Tag as Actor).wayPoint.Count == 0)
									{
										if ((im.SenderConnection.Tag as Actor).wayPointCnt > 1)
										{
											int elapsedTime = CommonCode.ReturnTimeStamp () - (im.SenderConnection.Tag as Actor).wayPointTimeStamp;

											if ((im.SenderConnection.Tag as Actor).animatedAction == Enums.AnimatedActions.Name.walk)
											{
												//double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
												int trueTime = (int)(Math.Round ((im.SenderConnection.Tag as Actor).wayPointCnt * 0.2));

												if (elapsedTime - trueTime < -1)
												{		
													// seuille de 5 seconds tolléré (1 = 0.2 * 5 seconds)
													// le joueur a triché en diminuons le temps de relance pour chaque tuille passé
													im.SenderConnection.Disconnect ("0x17");
													return;
												}
											}
											else if ((im.SenderConnection.Tag as Actor).animatedAction == Enums.AnimatedActions.Name.run)
											{
												//double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
												int trueTime = (int)(Math.Round ((im.SenderConnection.Tag as Actor).wayPointCnt * 0.2));

												if (elapsedTime - trueTime < (((im.SenderConnection.Tag as Actor).wayPointCnt < 10) ? -1 : -3)) {
													// le joueur a triché en diminuons le temps de relance pour chaque tuille passé
													Security.User_banne ("time too short between 2 tile", im.SenderConnection);
													return;
												}
											}
										}

										(im.SenderConnection.Tag as Actor).wayPointTimeStamp = 0;
										(im.SenderConnection.Tag as Actor).animatedAction = Enums.AnimatedActions.Name.idle;
										(im.SenderConnection.Tag as Actor).wayPoint.Clear ();
										(im.SenderConnection.Tag as Actor).wayPointCnt = 0;
									}
								}
								else
								{
									if (piib.wayPoint.Count == 0)
									{
										if (piib.wayPointCnt > 1)
										{
											int elapsedTime = CommonCode.ReturnTimeStamp () - piib.wayPointTimeStamp;

											if (piib.animatedAction == Enums.AnimatedActions.Name.walk)
											{
												//double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
												int trueTime = (int)(Math.Round (piib.wayPointCnt * 0.2));

												if (elapsedTime - trueTime < -1)
												{		
													// seuille de 5 seconds toléré
													// le joueur a triché en diminuons le temps de relance pour chaque tuille passé
													Security.User_banne ("too short time between 2 tiles, err2", im.SenderConnection);
													return;
												}
											}
											else if (piib.animatedAction == Enums.AnimatedActions.Name.run)
											{
												//double trueTime = (im.SenderConnection.Tag as PlayerInfo).wayPointCnt * 0.8;
												int trueTime = (int)(Math.Round (piib.wayPointCnt * 0.2));

												if (elapsedTime - trueTime < ((piib.wayPointCnt < 10) ? -1 : -3))
												{
													// le joueur a triché en diminuons le temps de relance pour chaque tuille passé
													Security.User_banne ("too short time between 2 tiles, err3", im.SenderConnection);
													return;
												}
											}
										}

										piib.wayPointTimeStamp = 0;
										piib.animatedAction = Enums.AnimatedActions.Name.idle;
										piib.wayPoint.Clear ();
										piib.wayPointCnt = 0;
									}
								}
							}
						}
						else
						{
							// cette condition n'est pas possible vus que le client devera avoir un waypoint > 0
							// mais pour des raisons de sécurité on le bloque
							return;
						}
                        #endregion
                    }
                    #endregion
                }
                else if (cmd.Length == 2 && cmd [1] == "SessionZero")
				{
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "" || (im.SenderConnection.Tag as Actor).inBattle == 1)
					{
						//Security.User_banne ("SessionZero", im);
                        Console.WriteLine("Ban SessionZero");
						return;
					}

					// informer tout les abonnées que le client viens de se désabonner du map
					List<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll (p => (p.Tag as Actor).map == (im.SenderConnection.Tag as Actor).map && (p.Tag as Actor).Pseudo != (im.SenderConnection.Tag as Actor).Pseudo && (p.Tag as Actor).inBattle == 0);
					foreach (NetConnection t in abonnedPlayers)
					{
					    ActorDisconnectedResponseMessage playerDisconnectedResponseMessage = new ActorDisconnectedResponseMessage();
					    playerDisconnectedResponseMessage.Initialize(new[] { ((Actor)im.SenderConnection.Tag).Pseudo }, t);
					    playerDisconnectedResponseMessage.Serialize();
					    playerDisconnectedResponseMessage.Send();
					}
					abonnedPlayers.Clear ();
					abonnedPlayers = null;

                    // modification de la bdd
                    mysql.connected connected = (DataBase.DataTables.connected as List<mysql.connected>).Find(f => f.pseudo == (im.SenderConnection.Tag as Actor).Pseudo);
                    connected.pseudo = "";
                    connected.timestamp = 0;
                    connected.map = "";
                    connected.map_position = "";

                    // remise a zero tous les données du client
                    (im.SenderConnection.Tag as Actor).SessionZero ();
                    #endregion
                }
				else if (cmd.Length == 4 && cmd [1] == "ChatMessage")
				{
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "")
					{
						Security.User_banne ("ChatMessage", im.SenderConnection);
						return;
					}
					// cmd•ChatMessage•G•salut
					if (cmd [1] != "" && Security.check_valid_msg (cmd [3])) {
						// verifier si le canal de diffusion n'est pas differente de celle referencé, g = general
						if (cmd [2] == "G") {
							// le message ne dois pas depasser le ChatMessageMaxChar
							if (cmd [3].Length > ChatMessageMaxChar) {
								Security.User_banne ("message too long mor than allowed", im.SenderConnection);
								return;
							}

							// informer tous les aboonées du map
							if((im.SenderConnection.Tag as Actor).inBattle == 0)
							{
								IList<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll (f => (f.Tag as Actor).map == (im.SenderConnection.Tag as Actor).map && (f.Tag as Actor).inBattle == 0);
								for (int cnt = 0; cnt < abonnedPlayers.Count; cnt++) {
									CommonCode.SendMessage ("cmd•ChatMessage•" + cmd [2] + "•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [3], abonnedPlayers [cnt], true);
									Console.WriteLine ("<--cmd•ChatMessage•" + cmd [2] + "•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [3] + " to " + (abonnedPlayers [cnt].Tag as Actor).Pseudo);
								}
								abonnedPlayers.Clear ();
								abonnedPlayers = null;
							}
							else
							{
								IList<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll (f => (f.Tag as Actor).map == (im.SenderConnection.Tag as Actor).map && (f.Tag as Actor).idBattle == (im.SenderConnection.Tag as Actor).idBattle);
								for (int cnt = 0; cnt < abonnedPlayers.Count; cnt++) {
									CommonCode.SendMessage ("cmd•ChatMessage•" + cmd [2] + "•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [3], abonnedPlayers [cnt], true);
									Console.WriteLine ("<--cmd•ChatMessage•" + cmd [2] + "•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [3] + " to " + (abonnedPlayers [cnt].Tag as Actor).Pseudo);
								}
								abonnedPlayers.Clear ();
								abonnedPlayers = null;
							}
						}
                        else if (cmd [2] == "P")
                        {
							//canal privé
							// mnemonique message "destinataire#message"
							// si le messge contiens une autre #, apres le 2eme # sera tranqué
							// verification si le destinataire est connecté
							if (MainClass.netServer.Connections.Exists (nc => (nc.Tag as Actor).Pseudo == cmd [3].Split ('#') [0]) && (im.SenderConnection.Tag as Actor).Pseudo != cmd [3].Split ('#') [0]) {
								string tmpMsg = "";
								if (cmd [3].Split ('#').Length >= 2) {
									for (int cnt = 1; cnt< cmd[3].Split('#').Length; cnt ++)
										tmpMsg += cmd [3].Split ('#') [cnt] + "#";

									tmpMsg = tmpMsg.Substring (0, tmpMsg.Length - 1);
								}
								CommonCode.SendMessage ("cmd•ChatMessage•" + cmd [2] + "•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + tmpMsg, MainClass.netServer.Connections.Find (nc => (nc.Tag as Actor).Pseudo == cmd [3].Split ('#') [0]), true);
								Console.WriteLine ("<--cmd•ChatMessage•" + cmd [2] + "•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [3]);
							}
						} else {
							Security.User_banne ("canal not found", im.SenderConnection);
							return;
						}
					}
                    else
                    {
						// l'utilisateur a envoyé un message avec un caractere non autorisé •
						// le cleint dispose deja d'une fonction de filtrage, alors le client a modifié l'exe
						Security.User_banne ("notAllowedChar[•]", im.SenderConnection);
						return;
					}
                    #endregion
                }
				else if (cmd.Length == 3 && cmd [1] == "challenge")
				{
                    #region
                    try
                    {
                        if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "" || (im.SenderConnection.Tag as Actor).inBattle == 1)
                        {
                            Security.User_banne("challenge", im.SenderConnection);
                            return;
                        }
                        // cmd•challenge•kabox
                        // verification si le joueur existe
                        NetConnection nt = MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == cmd[2]);
                        if (nt != null && (nt.Tag as Actor).IgnoredPlayersChallenge.IndexOf((im.SenderConnection.Tag as Actor).Pseudo) == -1)
                        {
                            // verification si les 2 joueurs sont dans la meme map
                            if ((im.SenderConnection.Tag as Actor).map != (nt.Tag as Actor).map)
                            {
                                Console.WriteLine("error# 0x025 : client " + (im.SenderConnection.Tag as Actor).Pseudo + " challenge player " + cmd[2] + " in different map");
                                return;
                            }

                            if ((im.SenderConnection.Tag as Actor).inBattle == 0 && (MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == cmd[2]).Tag as Actor).inBattle == 0)
                            {
                                // verification si les 2 joueunts ne sont pas en attente ou demande un défie d'une autre personne
                                if ((im.SenderConnection.Tag as Actor).YouChallengePlayer == "" && (im.SenderConnection.Tag as Actor).PlayerChallengeYou == "" && (nt.Tag as Actor).PlayerChallengeYou == "" && (nt.Tag as Actor).YouChallengePlayer == "")
                                {
                                    // les 2 utilisateurs sont libre
                                    // demander au jouer défié si il accepte le défie ou pas
                                    CommonCode.SendMessage("cmd•askingToChallenge•" + (im.SenderConnection.Tag as Actor).Pseudo, nt, true);
                                    Console.WriteLine("<--cmd•askingToChallenge to " + (nt.Tag as Actor).Pseudo);
                                    (nt.Tag as Actor).PlayerChallengeYou = (im.SenderConnection.Tag as Actor).Pseudo;

                                    CommonCode.SendMessage("cmd•waitingToChallenge•" + cmd[2], im, true);
                                    Console.WriteLine("<--cmd•waitingToChallenge•" + cmd[2]);
                                    (im.SenderConnection.Tag as Actor).YouChallengePlayer = cmd[2];
                                }
                                else
                                {
                                    // sois celui demandé en défie est occupé sois le demandeur triche en envoyons une demande de défie alors qu'il est déja entrain de defier ou demande un autre défie
                                    CommonCode.SendMessage("cmd•playerBusyToChallengeYou", im, true);
                                    Console.WriteLine("<--cmd•playerBusyToChallengeYou");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    #endregion
                }
                else if (cmd.Length == 3 && cmd [1] == "CancelChallengeRespond")
				{
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "" || (im.SenderConnection.Tag as Actor).inBattle == 1)
                    {
                        //Security.User_banne ("CancelChallengeRespond", im);
                        Console.WriteLine("CancelChallengeRespond failed");
                        return;
                    }
                    // cmd•CancelChallengeRespond•narutox
                    // informer l'autre partie que l'utilisateur ne veux pas de challenge
                    NetConnection nt = MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == cmd[2]);
                    if (nt == null)
                        return;
                    CommonCode.SendMessage("cmd•CancelChallengeRespond•" + (im.SenderConnection.Tag as Actor).Pseudo, nt, true);
                    Console.WriteLine("<--cmd•CancelChallengeRespond");
                    (im.SenderConnection.Tag as Actor).PlayerChallengeYou = "";
                    (im.SenderConnection.Tag as Actor).YouChallengePlayer = "";
                    (nt.Tag as Actor).YouChallengePlayer = "";
                    (nt.Tag as Actor).PlayerChallengeYou = "";
                    #endregion
                }
                else if (cmd.Length == 3 && cmd [1] == "CancelChallengeAsking")
                { 
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "" || (im.SenderConnection.Tag as Actor).inBattle == 1)
					{
						Security.User_banne ("CancelChallengeAsking", im.SenderConnection);
						return;
					}
					NetConnection nt = MainClass.netServer.Connections.Find (f => (f.Tag as Actor).Pseudo == cmd [2]);
					if (nt == null)
						return;

					// informer l'autre partie que l'utilisateur annule son challenge
					CommonCode.SendMessage ("cmd•CancelChallengeAsking•" + (im.SenderConnection.Tag as Actor).Pseudo, nt, true);
					Console.WriteLine ("<--cmd•CancelChallengeAsking");
					(im.SenderConnection.Tag as Actor).PlayerChallengeYou = "";
					(im.SenderConnection.Tag as Actor).YouChallengePlayer = "";
					(nt.Tag as Actor).YouChallengePlayer = "";
					(nt.Tag as Actor).PlayerChallengeYou = "";
                    #endregion
                } 
                else if (cmd.Length == 3 && cmd [1] == "IgnorePlayerChallenge")
                {
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "" || (im.SenderConnection.Tag as Actor).inBattle == 1)
					{
						Security.User_banne ("IgnorePlayerChallenge", im.SenderConnection);
						return;
					}
					if ((im.SenderConnection.Tag as Actor).IgnoredPlayersChallenge.IndexOf (cmd [2]) == -1) {
						(im.SenderConnection.Tag as Actor).IgnoredPlayersChallenge.Add (cmd [2]);
						NetConnection nt = MainClass.netServer.Connections.Find (f => (f.Tag as Actor).Pseudo == cmd [2]);
						if (nt != null) {
							(im.SenderConnection.Tag as Actor).PlayerChallengeYou = "";
							(im.SenderConnection.Tag as Actor).YouChallengePlayer = "";
							(nt.Tag as Actor).YouChallengePlayer = "";
							(nt.Tag as Actor).PlayerChallengeYou = "";
						}
					}
                    #endregion
                } 
                else if (cmd.Length == 3 && cmd [1] == "AcceptChallenge")
                {
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "" || (im.SenderConnection.Tag as Actor).inBattle == 1)
                    {
                        Security.User_banne("AcceptChallenge", im.SenderConnection);
                        return;
                    }
                    Actor pi = im.SenderConnection.Tag as Actor;
                    // ATTENTION si un changement est fait sur ce code, on doit apporter les meme changement sur le fichier Network2.cs Cmd acceptFirstFight, pour un combat Pnj
                    if (pi.Pseudo == "" || pi.map == "" || pi.inBattle == 1)
					{
                        if (pi.inBattle == 1)
                            Console.WriteLine("client qui devais recevoir déja la cmd d'acceptation du combat," + Environment.NewLine + "mais il nous demande une 2eme fois c'est q'uil na pas recu la requête");
						//Security.User_banne ("AcceptChallenge", im);
						return;
					}
					// challenge accepté
					//verifier si celui qui demande le challenge a vraiment lancé un défie
					if (MainClass.netServer.Connections.Exists (f => (f.Tag as Actor).Pseudo == cmd [2]))
                    {
						if ((MainClass.netServer.Connections.Find (f => (f.Tag as Actor).Pseudo == cmd[2]).Tag as Actor).YouChallengePlayer == pi.Pseudo)
						{
							// le joueur a demandé a nous défié
							// verification si nous avons vraiment reçu une demande de défie de sa part
							if (pi.PlayerChallengeYou == cmd [2])
							{
								Console.WriteLine ("combat commancé");
								// création d'une session de combat
                                
								Battle _battle = new Battle ();
								// 2eme joueur
								Actor pi1t2 = (Actor)pi.Clone ();
								pi1t2.directionLook = 3;
								pi1t2.teamSide = Enums.Team.Side.B;
								pi1t2.idBattle = _battle.IdBattle;
								_battle.SideB.Add (pi1t2);

								// 1er joueur
								NetConnection nc = MainClass.netServer.Connections.Find (f => (f.Tag as Actor).Pseudo == pi.PlayerChallengeYou);

								Actor pi1t1 = (Actor)(nc.Tag as Actor).Clone ();
								pi1t1.directionLook = 1;
								pi1t1.teamSide = Enums.Team.Side.A;
								pi1t1.idBattle = _battle.IdBattle;

								_battle.SideA.Add (pi1t1);
								_battle.Owner = (nc.Tag as Actor).Pseudo;
								_battle.Map = pi.map;
								_battle.BattleType = BattleType.Type.FreeChallenge;
								_battle.Timestamp = CommonCode.ReturnTimeStamp ();
								_battle.IsFreeCellToSpell = CommonCode.ReturnFreeCellToSpellFunc (_battle.Map);
								_battle.IsFreeCellToWalk = CommonCode.ReturnFreeCellToWalkFunc (_battle.Map);
								Battle.Battles.Add (_battle);

                                // sauveguard de l'id dans la tables des joueurs
                                foreach (mysql.players player in (DataBase.DataTables.players as List<mysql.players>).FindAll(f => f.pseudo == pi.Pseudo || f.pseudo == (im.SenderConnection.Tag as Actor).PlayerChallengeYou))
                                {
                                    player.inBattle = 1;
                                    player.inBattleType = _battle.BattleType.ToString();
                                    player.inBattleID = _battle.IdBattle;
                                }

                                pi.inBattle = 1;
								pi.idBattle = _battle.IdBattle;
								pi.teamSide = Enums.Team.Side.B;
								(nc.Tag as Actor).idBattle = _battle.IdBattle;
								(nc.Tag as Actor).teamSide = Enums.Team.Side.A;
								(nc.Tag as Actor).inBattle = 1;

								// ajout de l'objet image de combat freeChallenge a la table MapObj pour les 2 joueurs
								NetConnection p1 = MainClass.netServer.Connections.Find (f => (f.Tag as Actor).Pseudo == pi.PlayerChallengeYou);

                                mysql.mapobj MapObj = new mysql.mapobj() { map = pi.map, state = "dynamic", obj = "BattleShields", map_position = (p1.Tag as Actor).map_position.X + "/" + (p1.Tag as Actor).map_position.Y, assoc = Enums.Team.Side.A.ToString(), idBattle = _battle.IdBattle };
                                (DataBase.DataTables.mapobj as List<mysql.mapobj>).Add(MapObj);

                                mysql.mapobj MapObj2 = new mysql.mapobj() { map = pi.map, state = "dynamic", obj = "BattleShields", map_position = pi.map_position.X + "/" + pi.map_position.Y, assoc = Enums.Team.Side.B.ToString(), idBattle = _battle.IdBattle };
                                (DataBase.DataTables.mapobj as List<mysql.mapobj>).Add(MapObj2);

                                // informer tous les abonnées au map du nouveau objets sur le map pour les 2 joueurs, ainsi la disparition des 2 joueurs
                                List<NetConnection> PlayersInMap = MainClass.netServer.Connections.FindAll (f => (f.Tag as Actor).map == pi.map && (f.Tag as Actor).inBattle == 0 && (f.Tag as Actor).Pseudo != pi.Pseudo && (f.Tag as Actor).Pseudo != pi.PlayerChallengeYou);
								for (int cnt = 0; cnt< PlayersInMap.Count; cnt++)
                                {
									// envoie pour l'affichage des 2 objets de combat
                                    //string str1 = "cmd•MapAllDataObj•" + Enums.BattleType.Type.FreeChallenge + "•" + (p1.Tag as Actor).map_position.X + "/" + (p1.Tag as Actor).map_position.Y + "•" + _battle.IdBattle + "#" + Enums.Team.Side.A + "•" + pi1t1.Pseudo + "#" + pi1t1.ClasseId + "#" + pi1t1.village + "#" + pi1t1.Level + "#" + pi1t1.Spirit + "#" + pi1t1.SpiritLvl + "|" + "cmd•MapAllDataObj•" + Enums.BattleType.Type.FreeChallenge + "•" + pi.map_position.X + "/" + pi.map_position.Y + "•" + _battle.IdBattle + "#team2" + "•" + pi1t2.Pseudo + "#" + pi1t2.ClasseId + "#" + pi1t2.village + "#" + pi1t2.Level + "#" + pi1t2.Spirit + "#" + pi1t2.SpiritLvl;

                                    GrabingMapObjectsInformationResponseMessage grabingMapObjectsInformationResponseMessage = new GrabingMapObjectsInformationResponseMessage();
                                    string[] bufffer =
                                    (BattleType.Type.FreeChallenge + "•" + ((Actor) p1.Tag).map_position.X + "/" +
                                        ((Actor) p1.Tag).map_position.Y + "•" + _battle.IdBattle + "#" + Team.Side.A +
                                        "•" + pi1t1.Pseudo + "#" + pi1t1.classeId + "#" + pi1t1.hiddenVillage + "#" +
                                        pi1t1.level + "#" + pi1t1.spirit + "#" + pi1t1.spiritLvl + "|" +
                                        BattleType.Type.FreeChallenge + "•" + pi.map_position.X + "/" +
                                        pi.map_position.Y + "•" + _battle.IdBattle + "#team2" + "•" + pi1t2.Pseudo +
                                        "#" + pi1t2.classeId + "#" + pi1t2.hiddenVillage + "#" + pi1t2.level + "#" +
                                        pi1t2.spirit + "#" + pi1t2.spiritLvl).Split('•');

                                    grabingMapObjectsInformationResponseMessage.Initialize(bufffer, PlayersInMap[cnt]);
                                    grabingMapObjectsInformationResponseMessage.Serialize();
                                    grabingMapObjectsInformationResponseMessage.Send();

                                    //CommonCode.SendMessage (str1, PlayersInMap [cnt], true);
                                    //Console.WriteLine ("<--" + str1 + " to " + PlayersInMap [cnt]);
									// informer tous les abonnées au map du combat de la disparition des joueurs qui sont en combats
									List<NetConnection> abonnedPlayers = MainClass.netServer.Connections.FindAll (p => (p.Tag as Actor).map == pi.map && (p.Tag as Actor).Pseudo != pi.Pseudo && (p.Tag as Actor).inBattle == 0);
									for (int cnt1 = 0; cnt1 < abonnedPlayers.Count; cnt1++)
                                    {
                                        ActorDisconnectedResponseMessage playerDisconnectedResponseMessage1 = new ActorDisconnectedResponseMessage();
                                        playerDisconnectedResponseMessage1.Initialize(new[] { pi.Pseudo }, abonnedPlayers[cnt1]);
                                        playerDisconnectedResponseMessage1.Serialize();
                                        playerDisconnectedResponseMessage1.Send();

                                        ActorDisconnectedResponseMessage playerDisconnectedResponseMessage2 = new ActorDisconnectedResponseMessage();
                                        playerDisconnectedResponseMessage2.Initialize(new[] { pi.PlayerChallengeYou }, abonnedPlayers[cnt1]);
                                        playerDisconnectedResponseMessage2.Serialize();
                                        playerDisconnectedResponseMessage2.Send();
                                    }
									abonnedPlayers.Clear ();
									abonnedPlayers = null;
								}

								PlayersInMap.Clear ();
								PlayersInMap = null;

								// collecte des données des 2 joueurs
								string playersData = "";

								playersData += pi1t1.Pseudo + "#" + pi1t1.classeName + "#" + pi1t1.level + "#" + pi1t1.hiddenVillage + "#" + pi1t1.maskColorString + "#" + pi1t1.maxHealth + "#" + pi1t1.currentHealth + "#" + pi1t1.officialRang + "#" + pi1t1.initiative + "#" + pi1t1.doton + "#" + pi1t1.katon + "#" + pi1t1.futon + "#" + pi1t1.raiton + "#" + pi1t1.suiton + "#" + pi1t1.usingDoton + "#" + pi1t1.usingKaton + "#" + pi1t1.usingFuton + "#" + pi1t1.usingRaiton + "#" + pi1t1.usingSuiton + "#" + pi1t1.equipedDoton + "#" + pi1t1.equipedKaton + "#" + pi1t1.equipedFuton + "#" + pi1t1.equipedRaiton + "#" + pi1t1.equipedSuiton + "#" + pi1t1.originalPc + "#" + pi1t1.originalPm + "#" + pi1t1.pe + "#" + pi1t1.cd + "#" + pi1t1.summons + "#" + pi1t1.resiDotonPercent + "#" + pi1t1.resiKatonPercent + "#" + pi1t1.resiFutonPercent + "#" + pi1t1.resiRaitonPercent + "#" + pi1t1.resiSuitonPercent + "#" + pi1t1.dodgePC + "#" + pi1t1.dodgePM + "#" + pi1t1.dodgePE + "#" + pi1t1.dodgeCD + "#" + pi1t1.removePC + "#" + pi1t1.removePM + "#" + pi1t1.removePE + "#" + pi1t1.removeCD + "#" + pi1t1.escape + "#" + pi1t1.blocage + "#" + pi1t1.species.ToString() + "#" + pi1t1.directionLook + "|";
								playersData += pi1t2.Pseudo + "#" + pi1t2.classeName + "#" + pi1t2.level + "#" + pi1t2.hiddenVillage + "#" + pi1t2.maskColorString + "#" + pi1t2.maxHealth + "#" + pi1t2.currentHealth + "#" + pi1t2.officialRang + "#" + pi1t2.initiative + "#" + pi1t2.doton + "#" + pi1t2.katon + "#" + pi1t2.futon + "#" + pi1t2.raiton + "#" + pi1t2.suiton + "#" + pi1t2.usingDoton + "#" + pi1t2.usingKaton + "#" + pi1t2.usingFuton + "#" + pi1t2.usingRaiton + "#" + pi1t2.usingSuiton + "#" + pi1t2.equipedDoton + "#" + pi1t2.equipedKaton + "#" + pi1t2.equipedFuton + "#" + pi1t2.equipedRaiton + "#" + pi1t2.equipedSuiton + "#" + pi1t2.originalPc + "#" + pi1t2.originalPm + "#" + pi1t2.pe + "#" + pi1t2.cd + "#" + pi1t2.summons + "#" + pi1t2.resiDotonPercent + "#" + pi1t2.resiKatonPercent + "#" + pi1t2.resiFutonPercent + "#" + pi1t2.resiRaitonPercent + "#" + pi1t2.resiSuitonPercent + "#" + pi1t2.dodgePC + "#" + pi1t2.dodgePM + "#" + pi1t2.dodgePE + "#" + pi1t2.dodgeCD + "#" + pi1t2.removePC + "#" + pi1t2.removePM + "#" + pi1t2.removePE + "#" + pi1t2.removeCD + "#" + pi1t2.escape + "#" + pi1t2.blocage + "#" + pi1t2.species.ToString() + "#" + pi1t2.directionLook;

								// modification des position des joueurs selon les position valide du map aléatoirement
								string[] teamAValidePos = battleStartPositions.Map(_battle.Map, _battle).Split ('|') [0].Split ('#');
								string[] teamBValidePos = battleStartPositions.Map(_battle.Map, _battle).Split ('|') [1].Split ('#');

                                _battle.SideAValidePos = battleStartPositions.Map(_battle.Map, _battle).Split('|')[0];
                                _battle.SideBValidePos = battleStartPositions.Map(_battle.Map, _battle).Split('|')[1];

                                Random random = new Random ();
								int rand = random.Next (teamBValidePos.Length);
                                pi1t2.map_position = new Point (Convert.ToInt32 (teamBValidePos [rand].Split ('/') [0]), Convert.ToInt32 (teamBValidePos [rand].Split ('/') [1]));

								rand = random.Next (teamAValidePos.Length);
								pi1t1.map_position = new Point (Convert.ToInt32 (teamAValidePos [rand].Split ('/') [0]), Convert.ToInt32 (teamAValidePos [rand].Split ('/') [1]));

								////////////////////////////////////
								string team1 = "";
								string team2 = "";

                                // séparateur entre les membre d'une meme team '|', séparateur entre les team '/'
								foreach (Actor player in _battle.SideA)
									team1 += player.Pseudo + "|" + player.map_position.X + "/" + player.map_position.Y + "#";
								team1 = team1.Substring (0, team1.Length - 1);

								foreach (Actor player in _battle.SideB)
									team2 += player.Pseudo + "|" + player.map_position.X + "/" + player.map_position.Y + "#";
								team2 = team2.Substring (0, team2.Length - 1);

								////////////////////////////////////
								//pseudo#classe#level#village#MaskColors#TotalPdv#CurrentPdv#rang#initiative#doton#katon#futon#raiton#siuton#usingDoton#usingKaton#usingFuton#usingRaiton#usingSuiton#dotonEquiped#katonEquiped#futonEquiped#raitonEquiped#suitonEquiped#pc#pm#pe#cd#invoc#resiDoton#resiKaton#resiFuton#resiRaiton#resiSuiton#esquivePC#esquivePM#esquivePE#esquiveCD#retraitPC#retraitPM#retraitPE#retraitCD#evasion#blocage
								// envoie au client la confirmation du challenge
                                object[] o = new object[9];
                                o[0] = playersData;
                                o[1] = battleStartPositions.Map(_battle.Map, _battle);
                                o[2] = MainClass.InitialisationBattleWaitTime;
                                o[3] = _battle.BattleType;
                                o[4] = team1;
                                o[5] = team2;
                                // cette partie est non géré par le client étrange
                                o[6] = _battle.State;
                                o[7] = battleStartPositions.Map(_battle.Map, _battle).Split('|')[0];
                                o[8] = battleStartPositions.Map(_battle.Map, _battle).Split('|')[1];
                                StartDuelBattleResponseMessage startDuelBattleResponseMessag = new StartDuelBattleResponseMessage();
                                startDuelBattleResponseMessag.Initialize(o, MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == pi.PlayerChallengeYou));
                                startDuelBattleResponseMessag.Serialize();
                                startDuelBattleResponseMessag.Send();
                            }
						}
					}
                    #endregion
                }
				else if (cmd.Length == 3 && cmd [1] == "battleIniNewPos")
				{
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "" || (im.SenderConnection.Tag as Actor).inBattle == 0)
					{
						Security.User_banne ("battleIniNewPos", im.SenderConnection);
						return;
					}
					// changement de la position lors du temps d'attente d'un combat
					// verification si le joueur est en mode battle + state= initialisation
					if ((im.SenderConnection.Tag as Actor).inBattle == 0)
						return;

					Battle _battle = Battle.Battles.Find (f => f.IdBattle == (im.SenderConnection.Tag as Actor).idBattle);
					if (_battle == null)
					{
						// le client est supposé sur un combat mais aucun combat n'éxiste selon son idBattle
						Console.WriteLine ("no battle existe for player " + (im.SenderConnection.Tag as Actor).Pseudo);
						return;
					}

                    // check si le joueur n'est pas en mode initialisation ou si il a vérouillé sa position
                    if (_battle.State != Enums.battleState.state.initialisation || _battle.LockedPosInIniTime.Exists(f => f == (im.SenderConnection.Tag as Actor).Pseudo))
                    {
                        CommonCode.SendMessage ("cmd•lockedPosition", im, true);
                        Console.WriteLine("cmd•lockedPosition to " + (im.SenderConnection.Tag as Actor).Pseudo);
                        return;
                    }

					try
					{
						int.Parse (cmd [2].Split ('/') [0]);
						int.Parse (cmd [2].Split ('/') [1]);
					}
					catch (Exception ex)
					{
						// le client triche en envoyons des lettre au lieu que des chiffres
						Console.WriteLine ((im.SenderConnection.Tag as Actor).Pseudo + " triche, letter in place of numbers, 0001 " + ex.ToString ());
						return;
					}
                    
					// recuperation des positions pour la map demandé + conversion en list
					List<string> sideAValidePos = new List<string> (_battle.SideAValidePos.Split ('#'));
					List<string> sideBValidePos = new List<string> (_battle.SideBValidePos.Split ('#'));

					if (((im.SenderConnection.Tag as Actor).teamSide == Enums.Team.Side.A && sideAValidePos.Exists (f => f == cmd [2])) || ((im.SenderConnection.Tag as Actor).teamSide == Enums.Team.Side.B && sideBValidePos.Exists (f => f == cmd [2])))
                    {
						// informer tous abonnées du nouveau placement du joueur, sideA
						List<Actor> sideA = _battle.SideA;
						for (int cnt = 0; cnt < sideA.Count; cnt++) {
							if (MainClass.netServer.Connections.Exists (f => (f.Tag as Actor).Pseudo == sideA [cnt].Pseudo)) {
								NetConnection nc = MainClass.netServer.Connections.Find (f => (f.Tag as Actor).Pseudo == sideA [cnt].Pseudo);
								CommonCode.SendMessage ("cmd•battleIniNewPos•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [2], nc, true);
								Console.WriteLine ("<--cmd•battleIniNewPos•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [2] + " to " + (nc.Tag as Actor).Pseudo);
							}
						}

						// informer tous abonnées du nouveau placement du joueur, team2
						List<Actor> team2 = _battle.SideB;
						for (int cnt = 0; cnt < team2.Count; cnt++)
						{
							if (MainClass.netServer.Connections.Exists (f => (f.Tag as Actor).Pseudo == team2 [cnt].Pseudo))
							{
								NetConnection nc = MainClass.netServer.Connections.Find (f => (f.Tag as Actor).Pseudo == team2 [cnt].Pseudo);
								CommonCode.SendMessage ("cmd•battleIniNewPos•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [2], nc, true);
								Console.WriteLine ("<--cmd•battleIniNewPos•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + cmd [2] + " to " + (nc.Tag as Actor).Pseudo);
							}
						}

						// mise a jours des variables
						if(_battle.SideA.Exists(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo))
							_battle.SideA.Find(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo).map_position = new Point (Convert.ToInt32 (cmd [2].Split ('/') [0]), Convert.ToInt32 (cmd [2].Split ('/') [1]));
						else if(_battle.SideB.Exists(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo))
						        _battle.SideB.Find(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo).map_position = new Point (Convert.ToInt32 (cmd [2].Split ('/') [0]), Convert.ToInt32 (cmd [2].Split ('/') [1]));
					}
                    #endregion
                }
				else if (cmd.Length == 2 && cmd [1] == "finishTurn")
				{
                    #region code pour passer la main lors d'un combat en mode Started, ou pour verouiller sa position en combat en mode initialisation
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "")
					{
						Security.User_banne ("finishTurn", im.SenderConnection);
						return;
					}
					// le client veux passer la main sans attendre
					// check si le joueur et en combat et qu'il a la main
                    if ((im.SenderConnection.Tag as Actor).inBattle == 1)
					{
						Battle _battle = Battle.Battles.Find (f => f.IdBattle == (im.SenderConnection.Tag as Actor).idBattle);
                        if (_battle != null)
						{
                            //check si le joueur inject cette cmd en étant en mode initialisation
                            // enlever cette cmd si le joueur peux lancer cette cmd en mode initialisation pour bloquer sa position
                            if(_battle.State == Enums.battleState.state.initialisation)
                            {
                                // client valide ou annule le verouillage de sa position
                                List<Actor> _lPI = new List<Actor>();
                                _lPI.AddRange(_battle.SideA.FindAll(f => f.species != Species.Name.Summon));
                                _lPI.AddRange(_battle.SideB.FindAll(f => f.species != Species.Name.Summon));

                                if(_battle.LockedPosInIniTime.Exists(f => f == (im.SenderConnection.Tag as Actor).Pseudo))
                                {
                                    // unlock position
                                    _battle.LockedPosInIniTime.RemoveAll(f => f == (im.SenderConnection.Tag as Actor).Pseudo);
                                    for(int cnt2 = 0; cnt2 < _lPI.Count; cnt2++)
                                    {
                                        NetConnection nc = MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == _lPI[cnt2].Pseudo);
                                        CommonCode.SendMessage ("cmd•BattlePosUnlocked•" + (im.SenderConnection.Tag as Actor).Pseudo, nc, true);
                                        Console.WriteLine ("<--cmd•BattlePosUnlocked•" + (im.SenderConnection.Tag as Actor).Pseudo);
                                    }
                                }
                                else
                                {
                                    // lock position
                                    bool launchBattle = false;
                                    _battle.LockedPosInIniTime.Add((im.SenderConnection.Tag as Actor).Pseudo);

                                    // check si tous les joueurs ont verrouillés leurs positions
                                    if (_battle.LockedPosInIniTime.Count == _lPI.Count)
                                    {
                                        _battle.Timestamp = CommonCode.ReturnTimeStamp() - MainClass.InitialisationBattleWaitTime - 2;
                                        launchBattle = true;
                                    }

                                    for(int cnt2 = 0; cnt2 < _lPI.Count; cnt2++)
                                    {
                                        if (_lPI[cnt2].species == Species.Name.Human)
                                        {
                                            NetConnection nc = MainClass.netServer.Connections.Find(f => (f.Tag as Actor).Pseudo == _lPI[cnt2].Pseudo);
                                            CommonCode.SendMessage("cmd•BattlePosLocked•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + launchBattle.ToString(), nc, true);
                                            Console.WriteLine("<--cmd•BattlePosLocked•" + (im.SenderConnection.Tag as Actor).Pseudo + "•" + launchBattle.ToString() + " to " + (nc.Tag as Actor).Pseudo);
                                        }
                                    }
                                }
                            }
                            else if(_battle.State == Enums.battleState.state.started)
                            {
                                // check si le joueur a la main
                                if (_battle.AllPlayersByOrder [_battle.Turn].Pseudo == (im.SenderConnection.Tag as Actor).Pseudo)
                                    CommonCode.FinishTurn (_battle, true);
                                else
                                    Console.WriteLine ("vous n'avez pas la main pour passer votre tour,c'est " + _battle.AllPlayersByOrder [_battle.Turn].Pseudo + " qui a la main");
                            }
                            else if(_battle.State != Enums.battleState.state.started)
                            {
                                return;
                            }
						}
					}
                    #endregion
                }
				else if (cmd.Length == 2 && cmd [1] == "leaveBattle")
				{
                    #region
                    if ((im.SenderConnection.Tag as Actor).Pseudo == "" || (im.SenderConnection.Tag as Actor).map == "")
					{
						Security.User_banne ("leaveBattle", im.SenderConnection);
						return;
					}
					// le client veux quiter la partie
					// check si le joueur est en combat
					if ((im.SenderConnection.Tag as Actor).inBattle == 1)
					{
						// recherche si un combat existe s'il sagit dun combat FreePlay
						Battle _battle = Battle.Battles.Find (f => f.IdBattle == (im.SenderConnection.Tag as Actor).idBattle);

                        if (_battle.State == Enums.battleState.state.started)
                        {
                            // on vérifie si le joueur a des invocations pour les supprimer
                            List<Actor> invocOfPlayer = _battle.AllPlayersByOrder.FindAll(f => f.Pseudo.Length > (im.SenderConnection.Tag as Actor).Pseudo.Length && f.Pseudo.Substring(0, (im.SenderConnection.Tag as Actor).Pseudo.Length + 1) == (im.SenderConnection.Tag as Actor).Pseudo + "$");

                            // retirer le joueur et ses invocs de la liste des joueurs en vie
                            for (int cnt = 0; cnt < invocOfPlayer.Count; cnt++)
                                _battle.AllPlayersByOrder.Remove(invocOfPlayer[cnt]);
                            _battle.DeadPlayers.Add((Actor)(_battle.AllPlayersByOrder.Find(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo)).Clone());
                            _battle.AllPlayersByOrder.RemoveAll(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo);
                            _battle.SideA.RemoveAll(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo);
                            _battle.SideB.RemoveAll(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo);

                            if (CommonCode.IsClosedBattle(_battle, true))
                                _battle.State = Enums.battleState.state.closed;
                            else
                            {
                                // il faut informer les joueurs que le joueur en cours veux quiter le combat, donc effacer le joueur (mort) et ses invocs
                            }
                        }
                        else if (_battle.State == Enums.battleState.state.initialisation)
                        {
                            List<Actor> _lPI = new List<Actor>();
                            _lPI.AddRange(_battle.SideA.FindAll(f => f.species == Species.Name.Human));
                            _lPI.AddRange(_battle.SideB.FindAll(f => f.species == Species.Name.Human));

                            _battle.DeadPlayers.Add((Actor)(_lPI.Find(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo)).Clone());
                            _battle.SideA.RemoveAll(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo);
                            _battle.SideB.RemoveAll(f => f.Pseudo == (im.SenderConnection.Tag as Actor).Pseudo);

                            if (CommonCode.IsClosedBattle(_battle, true))
                                _battle.State = Enums.battleState.state.closed;
                            else
                            {
                                // il faut informer les joueurs que le joueur en cours veux quiter le combat, donc effacer le joueur (mort) et ses invocs
                            }
                            // informer les joueurs abonnées au map de combat et qui ne sont pas en combat de la supression des 2 objets
                            List<NetConnection> nc = MainClass.netServer.Connections.FindAll (f => (f.Tag as Actor).inBattle == 0 && (f.Tag as Actor).map == _battle.Map);
                            for (int cnt2 = 0; cnt2 < nc.Count; cnt2 ++)
                            {
                                MapObjetRemovedResponseMessage mapObjetRemovedResponseMessage = new MapObjetRemovedResponseMessage();
                                object[] o = new object[2];
                                o[0] = Enums.BattleType.Type.FreeChallenge;
                                o[1] = _battle.IdBattle;

                                mapObjetRemovedResponseMessage.Initialize(o, nc[cnt2]);
                                mapObjetRemovedResponseMessage.Serialize();
                                mapObjetRemovedResponseMessage.Send();
                            }
                        }
					}
                    #endregion
                }
				else
				{
					Network2.GetData (cmd, im);
				}
			}
		}
	}
}