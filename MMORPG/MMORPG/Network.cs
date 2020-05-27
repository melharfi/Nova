using System;
using System.Diagnostics;
using MELHARFI.Lidgren.Network;
using System.Threading;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;
using MMORPG.Enums;
using MMORPG.Net.Messages.Request;
using MMORPG.Net.Messages.Response;

namespace MMORPG
{
    public class Network
    {
        ///////////// Connnection socket////////////////
        public static string host;
        public static string ip;
        public static int port;
        /// ////////////////////////////////////////////

        public static NetClient netClient;
        public static INetEncryption algo;
        private static string[] CommandStrings;                        // pour les cmd recus des clients

        static Network()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("the-morpher");
            config.AutoFlushSendQueue = false;
            config.PingInterval = 1F;
            config.ConnectionTimeout = 10;
            //config.PingInterval = 100;
            //config.ConnectionTimeout = 600;
            netClient = new NetClient(config);
            netClient.RegisterReceivedCallback(new SendOrPostCallback(GotMessage));
            algo = new NetXtea("the-morpher");
        }
        
        public static void Connect(string host, int port)
        {
            netClient.Start();
            NetOutgoingMessage hail = netClient.CreateMessage("Try to connect");
            netClient.Connect(host, port, hail);
        }
        public static void GotMessage(object peer)
        {
            NetIncomingMessage im;
            while ((im = netClient.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                    if (status == NetConnectionStatus.Connected)
                        client_Connected();
                    else if (status == NetConnectionStatus.Disconnected)
                    {
                        string reason = im.ReadString();
                        client_Disconnected(reason);
                    }
                    break;
                    case NetIncomingMessageType.Data:
                        im.Decrypt(algo);
                        // routage des cmd selon les classes apropriés
                        string data = im.ReadString();

                        CommonCode.historyCmd.Add("(Rec) " + data);

                        CommandStrings = data.Split(Net.Messages.CommandDelimitterChar.Delimitter);
                        Type incomingMessage = Type.GetType("MMORPG.Net.Messages.Response." + CommandStrings[0]);
                        if (incomingMessage != null)
                        {
                            IResponseMessage iMessageResponse = (IResponseMessage)Activator.CreateInstance(incomingMessage);
                            iMessageResponse.Fetch(CommandStrings);
                            return;

                        }
                        else
                        {
                            // cette commande est introuvable, il faut informer le client
                        }

                        /////////////////////////////////////////////////////////////////////////////////////////////////
                        if (data.Split('•')[1] == "ChatMessage")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "askingToChallenge")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "waitingToChallenge")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "CancelChallengeRespond")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "CancelChallengeAsking")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "playerBusyToChallengeYou")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "getPlayersPositionInBattle")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "battleIniNewPos")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "JoinChallenge")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "CloseBattle")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "change map")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "BattleStarted")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "BattleTurnPast")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "obstacleFound")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellPlayed")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellTileGranted")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellTileNotAllowed")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellPointNotEnoughPe")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "recheckPosition")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellNotEnoughPc")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellIntervalNotReached")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellRelanceParTourReached")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellRelanceParJoueurReached")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "BattlePosLocked")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "BattlePosUnlocked")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "lockedPosition")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellNotEnoughInvoc")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellNeedEtatSennin")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "submitedQuest")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "beganQuete")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "upgradedSpell")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellHandlerNotFound")
                            network_Command_Dispatcher.cmd(data);
                        else if (data.Split('•')[1] == "spellTargetNotAllowed")
                            network_Command_Dispatcher.cmd(data);
                        else
                        {
                            // si ils sont spécifiques au map, les éxécutés ici
                            GameStates.GameStateManager.Network_stat(data);
                        }
                        break;
                    default:
                        MessageBox.Show("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
                        break;
                }
            }
        }

        private static void client_Connected()
        {
            //GameStates.GameStateManager.Network_stat("internal•network•connection•succeded");
            var username = (TextBox) Manager.manager.mainForm.Controls.Find("username", false)[0];
            var password = (TextBox) Manager.manager.mainForm.Controls.Find("password", false)[0];
            var authentification = new Net.Messages.Request.AuthentificationRequestMessage(username: username.Text, password: password.Text, overridePreviousConnexion:AuthentificationRequestMessage.OverridePreviousConnexion.Normal);
            authentification.Serialize();
            authentification.Send();
        }
        static void client_Disconnected(string reason)
        {
            Enums.DisconnectReason.disconnectReason disconnectReason;

            if (!Enum.TryParse(reason, true, out disconnectReason))
                disconnectReason = DisconnectReason.disconnectReason.OTHER;
            else
                disconnectReason =
                    (Enums.DisconnectReason.disconnectReason)
                        Enum.Parse(typeof(Enums.DisconnectReason.disconnectReason), reason, true);

            switch (disconnectReason)
            {
                case Enums.DisconnectReason.disconnectReason.HOST_UNREACHABLE:
                    MessageBox.Show(CommonCode.TranslateText(2) + Environment.NewLine + reason, "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MainForm.DrawDisconnectImg(true);
                    GameStates.GameStateManager.ChangeState(new GameStates.LoginMap());
                    GameStates.GameStateManager.CheckState();
                    Disconnect();
                    break;
                case Enums.DisconnectReason.disconnectReason.USER_BANNED:
                    DialogResult userBannedResult = MessageBox.Show(CommonCode.TranslateText(10), "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (userBannedResult == DialogResult.Yes)
                        System.Diagnostics.Process.Start(MainForm.url);
                    GameStates.GameStateManager.ChangeState(new GameStates.LoginMap());
                    GameStates.GameStateManager.CheckState();
                    MainForm.DrawDisconnectImg(true);
                    
                    Disconnect();
                    break;
                case Enums.DisconnectReason.disconnectReason.MAINTENANCE:
                    MessageBox.Show(CommonCode.TranslateText(14), "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    MainForm.DrawDisconnectImg(true);
                    Disconnect();
                    break;
                case Enums.DisconnectReason.disconnectReason.RESTARTING:
                    MessageBox.Show(CommonCode.TranslateText(15), "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    MainForm.DrawDisconnectImg(true);
                    Disconnect();
                    break;
                case Enums.DisconnectReason.disconnectReason.SHUTDOWN:
                    MessageBox.Show(CommonCode.TranslateText(16), "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    MainForm.DrawDisconnectImg(true);
                    Disconnect();
                    break;
                case Enums.DisconnectReason.disconnectReason.WRONG_CREDENTIALS:
                    MessageBox.Show(CommonCode.TranslateText(8), "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    MainForm.DrawDisconnectImg(true);
                    Disconnect();
                    break;
                case Enums.DisconnectReason.disconnectReason.IP_BANNED:
                    DialogResult ipBannedResult = MessageBox.Show(CommonCode.TranslateText(17), "IP Banned", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ipBannedResult == DialogResult.Yes)
                        System.Diagnostics.Process.Start(MainForm.url);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    MainForm.DrawDisconnectImg(true);
                    Disconnect();
                    break;
                case Enums.DisconnectReason.disconnectReason.INVALID_TYPES:
                    MessageBox.Show(CommonCode.TranslateText(31), "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    MainForm.DrawDisconnectImg(true);
                    Disconnect();
                    break;
                case DisconnectReason.disconnectReason.ANOTHER_USER_OVERRIDE_CONNEXION:
                    // il faut tester ce mode
                    MessageBox.Show(CommonCode.TranslateText(13) + Environment.NewLine + reason, "Connexion Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    Shutdown();
                    break;
                case DisconnectReason.disconnectReason.TIME_OUT:
                    MessageBox.Show(CommonCode.TranslateText(193) + Environment.NewLine + reason, "Connexion Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    break;
                case Enums.DisconnectReason.disconnectReason.OTHER:
                    MessageBox.Show(CommonCode.TranslateText(4) + Environment.NewLine + reason, "Connexion Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MMORPG.GameStates.GameStateManager.ChangeState(new MMORPG.GameStates.LoginMap());
                    MMORPG.GameStates.GameStateManager.CheckState();
                    Disconnect();

                    if(Manager.manager.mainForm.Controls.Find("username", false).Length == 0)
                        return;
                    TextBox username = Manager.manager.mainForm.Controls.Find("username", false)[0] as TextBox;
                    username.Enabled = true;
                    TextBox password = Manager.manager.mainForm.Controls.Find("password", false)[0] as TextBox;
                    password.Enabled = true;

                    Bmp ConnexionBtn = Manager.manager.GfxObjList.FindLast(f => f.Name() == "__ConnexionBtn") as Bmp;
                    ConnexionBtn.visible = true;
                    IGfx __connexionBtnLabel = Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel");
                    if (__connexionBtnLabel != null)
                        Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel").Visible(true);
                    break;
            }
            Battle.Clear();
            Shutdown();
        }

        private static void Disconnect()
        {
            var username = (TextBox)Manager.manager.mainForm.Controls.Find("username", false)[0];
            var password = (TextBox)Manager.manager.mainForm.Controls.Find("password", false)[0];
            Bmp ConnexionBtn = Manager.manager.GfxObjList.FindLast(f => f.Name() == "__ConnexionBtn") as Bmp;

            // deconnexion avec le serveur
            username.Enabled = true;
            password.Enabled = true;
            ConnexionBtn.visible = true;
            IGfx __connexionBtnLabel = Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel");
            if (__connexionBtnLabel != null)
                Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel").Visible(true);
        }
        public static void Shutdown()
        {
            netClient.Shutdown(Enums.DisconnectReason.disconnectReason.SELF_DISCONNECT.ToString());
        }
        public static void SendMessage(string msg, bool crypted)
        {
            NetOutgoingMessage om = netClient.CreateMessage(msg);
            if (crypted)
                om.Encrypt(Network.algo);
            netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            netClient.FlushSendQueue();
            CommonCode.historyCmd.Add("--> " + msg);
        }
    }
}