using System;
using Lidgren.Network;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace SERVER
{
	public static class Network2
	{
		public static void getData(string[] cmd, NetIncomingMessage im)
		{
			if (cmd [1] == "spellTuiles")
			{
				// check si le joueur est en combat, si le combat est en mode Started
				/*if ((im.SenderConnection.Tag as PlayerInfo).inBattle == 0 && battle.battles[(im.SenderConnection.Tag as PlayerInfo).idBattle].State != "Started")
					return;*/

				if ((im.SenderConnection.Tag as PlayerInfo).inBattle == 0 && battle.battles.Find (f => f.idBattle == (im.SenderConnection.Tag as PlayerInfo).idBattle).State != "Started")
					return;

				// sort de la classe naruto "rasengan"
				// check si le client est autorisé a avoir ce sort
				MySqlConn mySqlConn = new MySqlConn ();
				mySqlConn.cmd.CommandText = "select sorts from players where pseudo='" + (im.SenderConnection.Tag as PlayerInfo).Pseudo + "'";
				mySqlConn.reader = mySqlConn.cmd.ExecuteReader ();
				if (mySqlConn.reader.Read ())
				{
					string[] spells = mySqlConn.reader ["sorts"].ToString ().Split ('|');
					bool found = false;
					string spellInfo = string.Empty;
					for (int cnt = 0; cnt< spells.Length; cnt++)
					{
						string[] spellsData = spells [cnt].Split (':');
						if (spellsData [0] == cmd[2])
						{
							found = true;
							spellInfo = spells [cnt];
							break;
						}
					}

					if (found)
					{
						// check si la position du sort lancé est autorisé par l'etendu du sort
						// creer une methode general qui return si la position du sort est autorisé par sa porté
						spellsChecker.spells (cmd, spellInfo, im);
					}
					else
					{
						// le client tente de lancer un sort qu'il na pas
						// bannissement peux etre
					}
				}

				mySqlConn.reader.Close ();
				mySqlConn.conn.Close ();
				mySqlConn.Dispose ();
				mySqlConn = null;
			}
		}
	}
}