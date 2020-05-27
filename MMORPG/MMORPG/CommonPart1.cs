using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using MELHARFI.Gfx;
using MELHARFI.AStarAlgo;
using System.Windows.Forms;
using MMORPG.GameStates;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Media;
using System.IO;
using System.Diagnostics;
using MELHARFI;
using MMORPG.Enums;
using MMORPG.Net.Messages.Request;

namespace MMORPG
{
    // déplacer vers Enums (pas utilisé jusqu'à maintenant)...
    /*public enum Menu
    {
        // contiens les menus disponibles pour savoir quand est se qu'un menu est affiché pour des contrôles
        None, States, Sorts
    }*/
    public partial class CommonCode
    {
        public static bool debug = false;                    // active le mode débug qui permer de voir les logs et les cmds recus
        public static bool showErrors = true;
        public static bool used = false;
        public static object lockedObj = new object();
        public static int langue = 0;                       // 0 = fr   1 = en      3 = ar
        public static List<Bmp> AllActorsInMap = new List<Bmp>();           // enregistre tous les joueurs present dans la map
        public static int ChatBulleLineMaxLength;
        public static int ChatBulleTimeOut;
        public static int ChatMessageMaxChar;
        public static int chakra1Level, chakra2Level, chakra3Level, chakra4Level, chakra5Level;
        public static IGfx RemoveGfxWhenClicked;
        public static string ChallengeTo = "";          // contiens le nom du joueur a qui la demande de défie a été envoyé, cela sert de controle pour eviter que n'importe qui ne nous envoie une requette qui risque d'effacer notre menu normalement envoyé a un autre joueur
        public static Bmp annulerChallengeMeDlg;
        public static Bmp annulerChallengeHimDlg;       // contiens l'image de l'arriere plant qui contiens la boite de dialgue pour celui qui a lancé le combat
        public static string CurMap;                    // contiens le nom de la map courante
        public delegate bool DelWayPointCallBack(Point p);          // delegate qui pointe vers l'une des 2 méthodes isFreeCellToWalk ou isFreeCellToSpell (qui contiens aussi les joueurs en combats qui sont considéré comme obstacles)
        public static DelWayPointCallBack CurMapFreeCellToSpell;          // contiens la methode isFreeCellToSpell de la classe (map) en cours
        public static DelWayPointCallBack CurMapFreeCellToWalk;           // contiens la methode isFreeCellToWalk de la classe (map) en cours
        private List<MapPoint> _shortestPath = new List<MapPoint>();    // liste des noeud pour pathfinding
        public static SoundPlayer backsound = new System.Media.SoundPlayer();
        public static SoundPlayer foresound = new System.Media.SoundPlayer();
        public static bool blockNetFlow = false;                     // cela permet de bloquer des evenement prematurément comme la cloture d'un combat alors que l'animation du sort na pas encore été términé, du coup la cmd reste mémorisé apres la fin du sort et un thread séparé en boucle check les cmd dans la liste
        public static string DefaultLang = "null";                           // langue par défaut pour le spell checker
        public static bool SpellCheck = false;
        public static List<string> installedLang = new List<string>();                            // contiens la liste des langues installé pour le spellChecker
        public static List<string> cmdBlockedBySpellInPRogress = new List<string>();          // contien la valeur des commande en instance bloqué a cause d'une animation de sort non encore terminé, cela est pratuqye quand un joueur lance un sort et que suite apres la partie finidu coup pour eviter la cloture du combat prématurément on bloque le flux des cmd, que ensuite repris a la fin de l'animation
        public static SolidBrush dotonColor = new SolidBrush(Color.FromArgb(142, 91, 21));              // couleur de l'element TERRE
        public static SolidBrush katonColor = new SolidBrush(Color.FromArgb(198, 0, 0));                // couleur de l'element FEU
        public static SolidBrush futonColor = new SolidBrush(Color.FromArgb(0, 197, 125));              // couleur de l'element VENT
        public static SolidBrush raitonColor = new SolidBrush(Color.FromArgb(215, 203, 0));             // couleur de l'element FOUDRE
        public static SolidBrush suitonColor = new SolidBrush(Color.FromArgb(12, 133, 255));            // couleur de l'element EAU
        public static SolidBrush neutralColor = new SolidBrush(Color.FromArgb(240, 240, 240));           // couleur de l'element NEUTRAL
        public static SolidBrush puissanceColor = new SolidBrush(Color.FromArgb(177, 29, 182));           // couleur de l'element PUISSANCE
        public static SolidBrush spellAreaAllowedColor = new SolidBrush(Color.FromArgb(215, 215, 215));        // couleur de la zone accessible pour le sort
        public static SolidBrush spellAreaNotAllowedColor = new SolidBrush(Color.Gray);                     // couleur de tuile non accessible par un sort
        public static List<string> historyCmd = new List<string>();                                    // contien tous les cmd recus pour les débuguer
        public static bool abortAnimActionThread = false;                                                         // pour arreter le thread proprement au lieu de abort()
        public static bool abortChatThread = false;                                                     // pour arreter le thread qui diffuse le message sur le canal general, quand un 2eme message est diffusé

        ///////////////// pour clignotement de la fenetre et le bouton, lorsque notre tour est arrivé
        [DllImport("user32.dll")]
        private static extern bool FlashWindowEx(ref FLASHWINFO fwi);
        [Flags]
        public enum FlashWindowOptions : uint
        {
            Stop = 0x00,
            Caption = 0x01,
            Taskbar = 0x02,
            All = 0x03,
            Continuous = 0x04,
            UntilForeground = 0x0C
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public FlashWindowOptions dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }
        public static void Flash(uint count)
        {
            var fwi = new FLASHWINFO();
            fwi.cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO));
            //fwi.hwnd = this.Handle;
            fwi.hwnd = Manager.manager.mainForm.Handle;
            // Fait clignoter la barre de titre et le bouton dans la barre des tâches
            fwi.dwFlags = FlashWindowOptions.All;
            fwi.uCount = count;
            FlashWindowEx(ref fwi);
        }
        ////////////////////////////// methoide
        public class MyPlayerInfo : Actor
        {
            // classe qui pointe vers l'instance de notre joueur parmis les joueurs disponible pour faciliter l'accès, Attention, cette classe n'est pas utilisé en mode combat, vus que ses states ne reflettes pas ceux changé apres les boost ... à part utiliser le nom de notre joueur pour l'identifier
            public static MyPlayerInfo instance = new MyPlayerInfo();
            public string User;
        }
        public static void repairConfigFile()
        {
            // on parcours tous les lignes pour voir si tous les cmds sont la
            List<string> configFile = new List<string>();
            using (StreamReader sr = new StreamReader(@"Config.ini"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    configFile.Add(line);
                sr.Close();
            }

            string[] allCmd = new string[11];
            allCmd[0] = "Remote_Ip";
            allCmd[1] = "Remote_Port";
            allCmd[2] = "lang";
            allCmd[3] = "ChatBulleLineMaxLength";
            allCmd[4] = "ChatBulleTimeOut";
            allCmd[5] = "ChatMessageMaxChar";
            allCmd[6] = "TransparentChatBox";
            allCmd[7] = "ColorBgChatBox";
            allCmd[8] = "PictureBgChatBox";
            allCmd[9] = "DefaultLang";
            allCmd[10] = "SpellCheck";

            bool corruptedFile = false;

            for (int cnt = 0; cnt < 8; cnt++)
            {
                bool ParamFound = false;

                for (int cnt1 = 0; cnt1 < configFile.Count; cnt1++)
                {
                    if (configFile[cnt1] != string.Empty && configFile[cnt1].Substring(0, 2) != "//" && configFile[cnt1].IndexOf(':') != -1)
                    {
                        string[] dataLine = configFile[cnt1].Split(':');
                        dataLine[0] = dataLine[0].Replace(" ", "");
                        dataLine[1] = dataLine[1].Replace(" ", "");

                        if (dataLine.Length != 2)
                        {
                            corruptedFile = true;
                            break;
                        }
                        else if (dataLine[0] == allCmd[cnt] && dataLine[1] == "")
                        {
                            corruptedFile = true;
                            break;
                        }
                        else if (dataLine[0] == allCmd[cnt] && dataLine[1] != "")
                        {
                            ParamFound = true;
                            break;
                        }
                    }
                }
                if (!ParamFound)
                {
                    // il ya une erreur avec un parametre, on reconstuit le fichier
                    corruptedFile = true;
                    break;
                }
            }

            if (corruptedFile)
            {
                // reconstruction du fichier
                using (StreamWriter sw = new StreamWriter(@"Config.ini"))
                {
                    sw.WriteLine("Remote_Ip:the-morpher.ddns.net");
                    sw.WriteLine("Remote_Port:7070");
                    sw.WriteLine("lang:0");
                    sw.WriteLine("ChatBulleLineMaxLength:50");
                    sw.WriteLine("ChatBulleTimeOut:5000");
                    sw.WriteLine("ChatMessageMaxChar:200");
                    sw.WriteLine("TransparentChatBox:false");
                    sw.WriteLine("ColorBgChatBox:255,255,255");
                    sw.WriteLine("PictureBgChatBox:null");
                    sw.WriteLine("DefaultLang:Fr");
                    sw.WriteLine("SpellCheck:true");
                    sw.Close();
                }
                MessageBox.Show("File <Config.ini> was corrupted, we just take carre of it :)");
            }
        }
        public static void saveOptions()
        {
            List<string> configFile = new List<string>();
            using (StreamReader sr = new StreamReader(@"Config.ini"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    configFile.Add(line);
                sr.Close();
            }
            string[] data = new string[configFile.Count];

            for (int cnt = 0; cnt < configFile.Count; cnt++)
            {
                if (configFile[cnt] != string.Empty && configFile[cnt].Substring(0, 2) != "//" && configFile[cnt].IndexOf(':') != -1)
                {
                    string[] dataLine = configFile[cnt].Split(':');
                    dataLine[0] = dataLine[0].Replace(" ", "");
                    dataLine[1] = dataLine[1].Replace(" ", "");

                    if (dataLine[0] == "Remote_Ip")
                        data[cnt] = "Remote_Ip:" + Network.host;
                    else if (dataLine[0] == "Remote_Port")
                        data[cnt] = "Remote_Port:" + Network.port;
                    else if (dataLine[0] == "lang")
                        data[cnt] = "lang:" + CommonCode.langue;
                    else if (dataLine[0] == "ChatBulleLineMaxLength")
                        data[cnt] = "ChatBulleLineMaxLength:" + CommonCode.ChatBulleLineMaxLength;
                    else if (dataLine[0] == "ChatBulleTimeOut")
                        data[cnt] = "ChatBulleTimeOut:" + CommonCode.ChatBulleTimeOut;
                    else if (dataLine[0] == "ColorBgChatBox")
                        data[cnt] = "ColorBgChatBox:" + MainForm.ColorBgChatBox;
                    else if (dataLine[0] == "PictureBgChatBox")
                        data[cnt] = "PictureBgChatBox:" + MainForm.PictureBgChatBox;
                    else if (dataLine[0] == "TransparentChatBox")
                        data[cnt] = "TransparentChatBox:" + MainForm.transparentChatBox.ToString();
                    else if (dataLine[0] == "DefaultLang")
                        data[cnt] = "DefaultLang:" + CommonCode.DefaultLang;
                    else if (dataLine[0] == "SpellCheck")
                        data[cnt] = "SpellCheck:" + CommonCode.SpellCheck.ToString();
                    else
                        data[cnt] = dataLine[0] + ":" + dataLine[1];
                }
            }

            using (StreamWriter sw = new StreamWriter(@"Config.ini"))
            {
                foreach (string s in data)
                    sw.WriteLine(s);
                sw.Close();
            }
        }
        public static int ReturnTimeStamp()
        {
            return (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            //return (Int32)(DateTime.Now.Subtract(new DateTime(2013, 1, 1))).TotalSeconds;
        }
        public static void ApplyMaskColorToClasse(Bmp ibPlayer)
        {
            #region
            // nouvelle methode de coloriage qui attache un ColorMap au Bmp pour appliquer les couleurs lors de l'affichage et non lors du traitement
            Actor pi = ibPlayer.tag as Actor;
            //string color = (ibPlayer.tag as PlayerInfo).MaskColor;

            ////////////////////// 1ere coloriage qui s'attache au joueur dans le ImageAttributes en initialisons un tableau de ColorMap[] avec tous les couleur ////////////////////////////////
            ////////////////////// ce masque permet de colorer le joueur lors du rendu graphics, à savoir dans le manager et dans la methode Paint
            if (pi.maskColorString != "null/null/null")
            {
                string[] maskColorsSplitted = pi.maskColorString.Split('/');

                // ce code est just pour les utilisateur qui ont colorés leurs personnages
                if (pi.className == Enums.ActorClass.ClassName.naruto)
                {
                    #region naruto
                    // calcule de la longeur du tableau au cas ou le joueur na pas coloré totalement son personnage
                    int cntTableau = 0;
                    int cnt = -1;
                    Color NewColor;
                    
                    if (maskColorsSplitted[0] != "null")
                        cntTableau += 4;

                    if (maskColorsSplitted[1] != "null")
                        cntTableau += 2;

                    if (maskColorsSplitted[2] != "null")
                        cntTableau += 2;

                    ibPlayer.newColorMap = new ColorMap[cntTableau];
                    if (maskColorsSplitted[0] != "null")
                    {
                        //////////////////////////// partie 1
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(248, 248, 40);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[0].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(248, 144, 8);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb(NewColor.R, (NewColor.G - 104 >= 0) ? NewColor.G - 104 : 0, (NewColor.B - 32 >= 0) ? NewColor.B - 32 : 0);
                        //// 3eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(192, 56, 0);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 56 >= 0) ? NewColor.R - 56 : 0, (NewColor.G - 192 >= 0) ? NewColor.G - 192 : 0, (NewColor.B - 40 >= 0) ? NewColor.B - 40 : 0);
                        //// 4eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(128, 24, 0);
                        NewColor = Color.FromArgb((NewColor.R - 120 >= 0) ? NewColor.R - 120 : 0, (NewColor.G - 224 >= 0) ? NewColor.G - 224 : 0, (NewColor.B - 40 >= 0) ? NewColor.B - 40 : 0);
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                    }

                    if (maskColorsSplitted[1] != "null")
                    {
                        ///////////////////////////// partie 2
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(248, 200, 120);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[1].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(200, 88, 40);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 48 >= 0) ? NewColor.R - 48 : 0, (NewColor.G - 112 >= 0) ? NewColor.G - 112 : 0, (NewColor.B - 80 >= 0) ? NewColor.B - 80 : 0);
                    }

                    if (maskColorsSplitted[2] != "null")
                    {
                        ///////////////////////////// partie 3
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(48, 56, 64);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[2].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(16, 24, 40);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 32 >= 0) ? NewColor.R - 32 : 0, (NewColor.G - 32 >= 0) ? NewColor.G - 32 : 0, (NewColor.B - 24 >= 0) ? NewColor.B - 24 : 0);
                        ///////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    #endregion
                }
                else if (pi.className == Enums.ActorClass.ClassName.choji)
                {
                    #region choji
                    // calcule de la longeur du tableau
                    int cntTableau = 0;
                    int cnt = -1;
                    Color NewColor;

                    if (maskColorsSplitted[0] != "null")
                        cntTableau += 2;

                    if (maskColorsSplitted[1] != "null")
                        cntTableau += 2;

                    if (maskColorsSplitted[2] != "null")
                        cntTableau += 3;

                    ibPlayer.newColorMap = new ColorMap[cntTableau];
                    if (maskColorsSplitted[0] != "null")
                    {
                        //////////////////////////// partie 1
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(176, 80, 24);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[0].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(120, 48, 8);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 56 >= 0) ? NewColor.R - 56 : 0, (NewColor.G - 32 >= 0) ? NewColor.G - 32 : 0, (NewColor.B - 16 >= 0) ? NewColor.B - 16 : 0);
                    }

                    if (maskColorsSplitted[1] != "null")
                    {
                        ///////////////////////////// partie 2
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(168, 16, 16);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[1].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(96, 8, 8);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 72 >= 0) ? NewColor.R - 72 : 0, (NewColor.G - 8 >= 0) ? NewColor.G - 8 : 0, (NewColor.B - 8 >= 0) ? NewColor.B - 8 : 0);
                    }

                    if (maskColorsSplitted[2] != "null")
                    {
                        ///////////////////////////// partie 3
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(120, 120, 120);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[2].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(80, 80, 80);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 40 >= 0) ? NewColor.R - 40 : 0, (NewColor.G - 40 >= 0) ? NewColor.G - 40 : 0, (NewColor.B - 40 >= 0) ? NewColor.B - 40 : 0);
                        //// 3eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(32, 32, 32);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 88 >= 0) ? NewColor.R - 88 : 0, (NewColor.G - 88 >= 0) ? NewColor.G - 88 : 0, (NewColor.B - 88 >= 0) ? NewColor.B - 88 : 0);
                    }
                    #endregion
                }
                else if (pi.className == Enums.ActorClass.ClassName.kabuto)
                {
                    #region kabuto

                    // calcule de la longeur du tableau
                    int cntTableau = 0;
                    int cnt = -1;
                    Color NewColor;

                    if (maskColorsSplitted[0] != "null")
                        cntTableau += 3;

                    if (maskColorsSplitted[1] != "null")
                        cntTableau += 3;

                    if (maskColorsSplitted[2] != "null")
                        cntTableau += 4;

                    ibPlayer.newColorMap = new System.Drawing.Imaging.ColorMap[cntTableau];

                    //////////////////////////// partie 1
                    //// 1ere couleur
                    if (maskColorsSplitted[0] != "null")
                    {
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(200, 192, 184);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[0].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(152, 144, 152);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 48 >= 0) ? NewColor.R - 48 : 0, (NewColor.G - 48 >= 0) ? NewColor.G - 48 : 0, (NewColor.B - 32 >= 0) ? NewColor.B - 32 : 0);
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(88, 88, 88);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 112 >= 0) ? NewColor.R - 112 : 0, (NewColor.G - 104 >= 0) ? NewColor.G - 104 : 0, (NewColor.B - 96 >= 0) ? NewColor.B - 96 : 0);
                    }

                    if (maskColorsSplitted[1] != "null")
                    {
                        ///////////////////////////// partie 2
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(104, 32, 120);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[1].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(64, 24, 96);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 40 >= 0) ? NewColor.R - 40 : 0, (NewColor.G - 8 >= 0) ? NewColor.G - 8 : 0, (NewColor.B - 24 >= 0) ? NewColor.B - 24 : 0);
                        //// 3eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(32, 8, 72);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 32 >= 0) ? NewColor.R - 32 : 0, (NewColor.G - 8 >= 0) ? NewColor.G - 8 : 0, (NewColor.B - 72 >= 0) ? NewColor.B - 72 : 0);
                    }

                    if (maskColorsSplitted[2] != "null")
                    {
                        ///////////////////////////// partie 3
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(248, 200, 120);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[2].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(232, 152, 80);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 16 >= 0) ? NewColor.R - 16 : 0, (NewColor.G - 48 >= 0) ? NewColor.G - 48 : 0, (NewColor.B - 40 >= 0) ? NewColor.B - 40 : 0);
                        //// 3eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(160, 96, 56);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 88 >= 0) ? NewColor.R - 88 : 0, (NewColor.G - 104 >= 0) ? NewColor.G - 104 : 0, (NewColor.B - 64 >= 0) ? NewColor.B - 64 : 0);
                        //// 4eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(80, 48, 32);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 168 >= 0) ? NewColor.R - 168 : 0, (NewColor.G - 52 >= 0) ? NewColor.G - 52 : 0, (NewColor.B - 88 >= 0) ? NewColor.B - 88 : 0);
                    }
                    #endregion
                }
                else if (pi.className == Enums.ActorClass.ClassName.ino)
                {
                    #region ino

                    // calcule de la longeur du tableau
                    int cntTableau = 0;
                    int cnt = -1;
                    Color NewColor;

                    if (maskColorsSplitted[0] != "null")
                        cntTableau += 2;

                    if (maskColorsSplitted[1] != "null")
                        cntTableau += 2;

                    if (maskColorsSplitted[2] != "null")
                        cntTableau += 4;

                    ibPlayer.newColorMap = new System.Drawing.Imaging.ColorMap[cntTableau];
                    //////////////////////////// partie 1
                    //// 1ere couleur
                    if (maskColorsSplitted[0] != "null")
                    {
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(248, 208, 112);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[0].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(208, 136, 32);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 40 >= 0) ? NewColor.R - 40 : 0, (NewColor.G - 72 >= 0) ? NewColor.G - 72 : 0, (NewColor.B - 80 >= 0) ? NewColor.B - 80 : 0);
                    }

                    if (maskColorsSplitted[1] != "null")
                    {
                        ///////////////////////////// partie 2
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(128, 0, 128);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[1].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(56, 0, 64);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 72 >= 0) ? NewColor.R - 72 : 0, 0, (NewColor.B - 64 >= 0) ? NewColor.B - 64 : 0);
                    }

                    if (maskColorsSplitted[2] != "null")
                    {
                        ///////////////////////////// partie 3
                        //// 1ere couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(232, 208, 168);
                        NewColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[2].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[2]));
                        ibPlayer.newColorMap[cnt].NewColor = NewColor;
                        //// 2eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(216, 136, 80);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 16 >= 0) ? NewColor.R - 16 : 0, (NewColor.G - 72 >= 0) ? NewColor.G - 72 : 0, (NewColor.B - 88 >= 0) ? NewColor.B - 88 : 0);
                        //// 3eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(160, 88, 56);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 72 >= 0) ? NewColor.R - 72 : 0, (NewColor.G - 120 >= 0) ? NewColor.G - 120 : 0, (NewColor.B - 112 >= 0) ? NewColor.B - 112 : 0);
                        //// 4eme couleur
                        ibPlayer.newColorMap[++cnt] = new System.Drawing.Imaging.ColorMap();
                        ibPlayer.newColorMap[cnt].OldColor = Color.FromArgb(96, 32, 0);
                        ibPlayer.newColorMap[cnt].NewColor = Color.FromArgb((NewColor.R - 136 >= 0) ? NewColor.R - 136 : 0, (NewColor.G - 176 >= 0) ? NewColor.G - 176 : 0, 0);
                    }
                    #endregion
                }

                /////////////// 2eme coloriage du joueur pour la premiere fois pour que les events detecte la nouvelle couleurs
                if (maskColorsSplitted[0] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[0].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[0].Split('-')[2]));
                    CommonCode.SetPixelToClass(pi.className, tmpColor, 1, ibPlayer);
                }

                if (maskColorsSplitted[1] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[1].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[1].Split('-')[2]));
                    CommonCode.SetPixelToClass(pi.className, tmpColor, 2, ibPlayer);
                }

                if (maskColorsSplitted[2] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt16(maskColorsSplitted[2].Split('-')[0]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[1]), Convert.ToInt16(maskColorsSplitted[2].Split('-')[2]));
                    CommonCode.SetPixelToClass(pi.className, tmpColor, 3, ibPlayer);
                }
            }
            #endregion
        }
        public static Enums.ActorClass.ClassName IdToClassName(int i)
        {
            // non utilisé pour le moment
            // retourne le nom du classe
            if (i == 0)
                return Enums.ActorClass.ClassName.naruto;
            else if (i == 1)
                return Enums.ActorClass.ClassName.choji;
            else if (i == 2)
                return Enums.ActorClass.ClassName.kabuto;
            else if (i == 3)
                return Enums.ActorClass.ClassName.ino;
            else if (i == 4)
                return Enums.ActorClass.ClassName.lee;
            else if (i == 5)
                return Enums.ActorClass.ClassName.kankura;
            else if (i == 6)
                return Enums.ActorClass.ClassName.shikamaru;
            else if (i == 7)
                return Enums.ActorClass.ClassName.sakura;
            else
                return Enums.ActorClass.ClassName.naruto;
        }
        public static Int16 ClassNameToId(Enums.ActorClass.ClassName className)
        {
            #region
            // utilisé dans la classe CreatePlayer, ligne 88, vus qu'un tableau est identifié seulement par un int et non un string
            // retourne le'id du classe
            if (className == Enums.ActorClass.ClassName.naruto)
                return 0;
            else if (className == Enums.ActorClass.ClassName.choji)
                return 1;
            else if (className == Enums.ActorClass.ClassName.kabuto)
                return 2;
            else if (className == Enums.ActorClass.ClassName.ino)
                return 3;
            else if (className == Enums.ActorClass.ClassName.lee)
                return 4;
            else if (className == Enums.ActorClass.ClassName.kankura)
                return 5;
            else if (className == Enums.ActorClass.ClassName.shikamaru)
                return 6;
            else if (className == Enums.ActorClass.ClassName.sakura)
                return 7;
            else if (className == Enums.ActorClass.ClassName.iruka)
                return 8;
            else
                return 0;
            #endregion
        }
        public static void SetPixelToClass2(Enums.ActorClass.ClassName className, Color color, short partie, Bmp bmp)
        {
            #region
            // cette emethode enregistre tous les offset de la 1ere sprite des classe, pour les colorier dans la map CreatePlayer
            // coloriage de la 1ere sprite selons les coordonnées X et Y
            // si cette methode provoque des erreurs de modification de pixels, lui faire le meme systeme que la methode SetPixelToClass() qui utilise d'autre techniques pour securiser la colorisation de l'objet
            if (className == Enums.ActorClass.ClassName.naruto)
            {
                #region
                if (partie == 1)
                {
                    #region partie 1
                    // couleur séléctionné sans degrade
                    bmp.bmp.SetPixel(5, 7, color);
                    bmp.bmp.SetPixel(6, 7, color);
                    bmp.bmp.SetPixel(7, 7, color);
                    bmp.bmp.SetPixel(7, 10, color);
                    bmp.bmp.SetPixel(8, 4, color);
                    bmp.bmp.SetPixel(8, 7, color);
                    bmp.bmp.SetPixel(8, 9, color);
                    bmp.bmp.SetPixel(9, 2, color);
                    bmp.bmp.SetPixel(9, 3, color);
                    bmp.bmp.SetPixel(9, 8, color);
                    bmp.bmp.SetPixel(10, 3, color);
                    bmp.bmp.SetPixel(10, 5, color);
                    bmp.bmp.SetPixel(10, 6, color);
                    bmp.bmp.SetPixel(11, 6, color);
                    bmp.bmp.SetPixel(11, 7, color);
                    bmp.bmp.SetPixel(12, 1, color);
                    bmp.bmp.SetPixel(12, 2, color);
                    bmp.bmp.SetPixel(12, 3, color);
                    bmp.bmp.SetPixel(12, 4, color);
                    bmp.bmp.SetPixel(12, 7, color);
                    bmp.bmp.SetPixel(13, 6, color);
                    bmp.bmp.SetPixel(14, 4, color);
                    bmp.bmp.SetPixel(14, 5, color);
                    bmp.bmp.SetPixel(15, 2, color);
                    bmp.bmp.SetPixel(15, 3, color);
                    bmp.bmp.SetPixel(15, 5, color);
                    bmp.bmp.SetPixel(15, 7, color);
                    bmp.bmp.SetPixel(16, 7, color);
                    bmp.bmp.SetPixel(16, 8, color);
                    bmp.bmp.SetPixel(16, 9, color);
                    bmp.bmp.SetPixel(17, 4, color);
                    bmp.bmp.SetPixel(17, 10, color);

                    // degradation d'un ton
                    Color tmpColor = Color.FromArgb(color.R, (color.G - 104 >= 0) ? color.G - 104 : 0, (color.B - 32 >= 0) ? color.B - 32 : 0);
                    bmp.bmp.SetPixel(7, 4, tmpColor);
                    bmp.bmp.SetPixel(7, 8, tmpColor);
                    bmp.bmp.SetPixel(7, 22, tmpColor);
                    bmp.bmp.SetPixel(7, 23, tmpColor);
                    bmp.bmp.SetPixel(7, 24, tmpColor);
                    bmp.bmp.SetPixel(7, 31, tmpColor);
                    bmp.bmp.SetPixel(7, 32, tmpColor);
                    bmp.bmp.SetPixel(7, 33, tmpColor);
                    bmp.bmp.SetPixel(7, 34, tmpColor);
                    bmp.bmp.SetPixel(7, 35, tmpColor);
                    bmp.bmp.SetPixel(7, 36, tmpColor);
                    bmp.bmp.SetPixel(7, 37, tmpColor);
                    bmp.bmp.SetPixel(8, 5, tmpColor);
                    bmp.bmp.SetPixel(8, 8, tmpColor);
                    bmp.bmp.SetPixel(8, 23, tmpColor);
                    bmp.bmp.SetPixel(8, 24, tmpColor);
                    bmp.bmp.SetPixel(8, 25, tmpColor);
                    bmp.bmp.SetPixel(8, 31, tmpColor);
                    bmp.bmp.SetPixel(8, 32, tmpColor);
                    bmp.bmp.SetPixel(8, 33, tmpColor);
                    bmp.bmp.SetPixel(8, 34, tmpColor);
                    bmp.bmp.SetPixel(8, 35, tmpColor);
                    bmp.bmp.SetPixel(8, 36, tmpColor);
                    bmp.bmp.SetPixel(8, 37, tmpColor);
                    bmp.bmp.SetPixel(9, 4, tmpColor);
                    bmp.bmp.SetPixel(9, 23, tmpColor);
                    bmp.bmp.SetPixel(9, 24, tmpColor);
                    bmp.bmp.SetPixel(9, 25, tmpColor);
                    bmp.bmp.SetPixel(9, 26, tmpColor);
                    bmp.bmp.SetPixel(9, 31, tmpColor);
                    bmp.bmp.SetPixel(9, 32, tmpColor);
                    bmp.bmp.SetPixel(9, 34, tmpColor);
                    bmp.bmp.SetPixel(9, 35, tmpColor);
                    bmp.bmp.SetPixel(10, 7, tmpColor);
                    bmp.bmp.SetPixel(10, 23, tmpColor);
                    bmp.bmp.SetPixel(10, 24, tmpColor);
                    bmp.bmp.SetPixel(10, 25, tmpColor);
                    bmp.bmp.SetPixel(10, 26, tmpColor);
                    bmp.bmp.SetPixel(10, 32, tmpColor);
                    bmp.bmp.SetPixel(11, 4, tmpColor);
                    bmp.bmp.SetPixel(11, 32, tmpColor);
                    bmp.bmp.SetPixel(12, 5, tmpColor);
                    bmp.bmp.SetPixel(12, 6, tmpColor);
                    bmp.bmp.SetPixel(13, 3, tmpColor);
                    bmp.bmp.SetPixel(13, 5, tmpColor);
                    bmp.bmp.SetPixel(13, 7, tmpColor);
                    bmp.bmp.SetPixel(14, 6, tmpColor);
                    bmp.bmp.SetPixel(14, 7, tmpColor);
                    bmp.bmp.SetPixel(14, 23, tmpColor);
                    bmp.bmp.SetPixel(14, 24, tmpColor);
                    bmp.bmp.SetPixel(14, 25, tmpColor);
                    bmp.bmp.SetPixel(14, 26, tmpColor);
                    bmp.bmp.SetPixel(14, 32, tmpColor);
                    bmp.bmp.SetPixel(15, 6, tmpColor);
                    bmp.bmp.SetPixel(15, 8, tmpColor);
                    bmp.bmp.SetPixel(15, 23, tmpColor);
                    bmp.bmp.SetPixel(15, 24, tmpColor);
                    bmp.bmp.SetPixel(15, 25, tmpColor);
                    bmp.bmp.SetPixel(15, 26, tmpColor);
                    bmp.bmp.SetPixel(15, 32, tmpColor);
                    bmp.bmp.SetPixel(15, 34, tmpColor);
                    bmp.bmp.SetPixel(15, 35, tmpColor);
                    bmp.bmp.SetPixel(15, 36, tmpColor);
                    bmp.bmp.SetPixel(16, 4, tmpColor);
                    bmp.bmp.SetPixel(16, 5, tmpColor);
                    bmp.bmp.SetPixel(16, 23, tmpColor);
                    bmp.bmp.SetPixel(16, 24, tmpColor);
                    bmp.bmp.SetPixel(16, 25, tmpColor);
                    bmp.bmp.SetPixel(16, 31, tmpColor);
                    bmp.bmp.SetPixel(16, 32, tmpColor);
                    bmp.bmp.SetPixel(16, 33, tmpColor);
                    bmp.bmp.SetPixel(16, 34, tmpColor);
                    bmp.bmp.SetPixel(16, 35, tmpColor);
                    bmp.bmp.SetPixel(16, 36, tmpColor);
                    bmp.bmp.SetPixel(16, 37, tmpColor);
                    bmp.bmp.SetPixel(17, 5, tmpColor);
                    bmp.bmp.SetPixel(17, 7, tmpColor);
                    bmp.bmp.SetPixel(17, 8, tmpColor);
                    bmp.bmp.SetPixel(17, 22, tmpColor);
                    bmp.bmp.SetPixel(17, 23, tmpColor);
                    bmp.bmp.SetPixel(17, 24, tmpColor);
                    bmp.bmp.SetPixel(17, 31, tmpColor);
                    bmp.bmp.SetPixel(17, 32, tmpColor);
                    bmp.bmp.SetPixel(17, 33, tmpColor);
                    bmp.bmp.SetPixel(17, 34, tmpColor);
                    bmp.bmp.SetPixel(17, 35, tmpColor);
                    bmp.bmp.SetPixel(17, 36, tmpColor);
                    bmp.bmp.SetPixel(17, 37, tmpColor);
                    bmp.bmp.SetPixel(18, 4, tmpColor);
                    bmp.bmp.SetPixel(18, 7, tmpColor);
                    bmp.bmp.SetPixel(19, 7, tmpColor);

                    // degradation de 2 ton
                    tmpColor = Color.FromArgb((color.R - 56 >= 0) ? color.R - 56 : 0, (color.G - 192 >= 0) ? color.G - 192 : 0, (color.B - 40 >= 0) ? color.B - 40 : 0);
                    bmp.bmp.SetPixel(6, 8, tmpColor);
                    bmp.bmp.SetPixel(6, 35, tmpColor);
                    bmp.bmp.SetPixel(6, 36, tmpColor);
                    bmp.bmp.SetPixel(6, 37, tmpColor);
                    bmp.bmp.SetPixel(6, 38, tmpColor);
                    bmp.bmp.SetPixel(6, 39, tmpColor);
                    bmp.bmp.SetPixel(6, 40, tmpColor);
                    bmp.bmp.SetPixel(7, 25, tmpColor);
                    bmp.bmp.SetPixel(7, 30, tmpColor);
                    bmp.bmp.SetPixel(7, 38, tmpColor);
                    bmp.bmp.SetPixel(7, 39, tmpColor);
                    bmp.bmp.SetPixel(7, 40, tmpColor);
                    bmp.bmp.SetPixel(7, 41, tmpColor);
                    bmp.bmp.SetPixel(8, 6, tmpColor);
                    bmp.bmp.SetPixel(8, 38, tmpColor);
                    bmp.bmp.SetPixel(8, 39, tmpColor);
                    bmp.bmp.SetPixel(8, 40, tmpColor);
                    bmp.bmp.SetPixel(9, 5, tmpColor);
                    bmp.bmp.SetPixel(9, 6, tmpColor);
                    bmp.bmp.SetPixel(9, 7, tmpColor);
                    bmp.bmp.SetPixel(9, 33, tmpColor);
                    bmp.bmp.SetPixel(9, 36, tmpColor);
                    bmp.bmp.SetPixel(9, 37, tmpColor);
                    bmp.bmp.SetPixel(9, 38, tmpColor);
                    bmp.bmp.SetPixel(9, 39, tmpColor);
                    bmp.bmp.SetPixel(10, 4, tmpColor);
                    bmp.bmp.SetPixel(10, 31, tmpColor);
                    bmp.bmp.SetPixel(10, 34, tmpColor);
                    bmp.bmp.SetPixel(10, 35, tmpColor);
                    bmp.bmp.SetPixel(11, 5, tmpColor);
                    bmp.bmp.SetPixel(11, 31, tmpColor);
                    bmp.bmp.SetPixel(13, 2, tmpColor);
                    bmp.bmp.SetPixel(13, 4, tmpColor);
                    bmp.bmp.SetPixel(13, 31, tmpColor);
                    bmp.bmp.SetPixel(13, 32, tmpColor);
                    bmp.bmp.SetPixel(14, 3, tmpColor);
                    bmp.bmp.SetPixel(14, 31, tmpColor);
                    bmp.bmp.SetPixel(14, 34, tmpColor);
                    bmp.bmp.SetPixel(14, 35, tmpColor);
                    bmp.bmp.SetPixel(15, 4, tmpColor);
                    bmp.bmp.SetPixel(15, 31, tmpColor);
                    bmp.bmp.SetPixel(15, 33, tmpColor);
                    bmp.bmp.SetPixel(15, 37, tmpColor);
                    bmp.bmp.SetPixel(15, 38, tmpColor);
                    bmp.bmp.SetPixel(15, 39, tmpColor);
                    bmp.bmp.SetPixel(16, 6, tmpColor);
                    bmp.bmp.SetPixel(16, 38, tmpColor);
                    bmp.bmp.SetPixel(16, 39, tmpColor);
                    bmp.bmp.SetPixel(16, 40, tmpColor);
                    bmp.bmp.SetPixel(17, 6, tmpColor);
                    bmp.bmp.SetPixel(17, 25, tmpColor);
                    bmp.bmp.SetPixel(17, 30, tmpColor);
                    bmp.bmp.SetPixel(17, 38, tmpColor);
                    bmp.bmp.SetPixel(17, 39, tmpColor);
                    bmp.bmp.SetPixel(17, 40, tmpColor);
                    bmp.bmp.SetPixel(17, 41, tmpColor);
                    bmp.bmp.SetPixel(18, 8, tmpColor);
                    bmp.bmp.SetPixel(18, 35, tmpColor);
                    bmp.bmp.SetPixel(18, 36, tmpColor);
                    bmp.bmp.SetPixel(18, 37, tmpColor);
                    bmp.bmp.SetPixel(18, 38, tmpColor);
                    bmp.bmp.SetPixel(18, 39, tmpColor);
                    bmp.bmp.SetPixel(18, 40, tmpColor);

                    tmpColor = Color.FromArgb((color.R - 120 >= 0) ? color.R - 120 : 0, (color.G - 224 >= 0) ? color.G - 224 : 0, (color.B - 40 >= 0) ? color.B - 40 : 0);
                    bmp.bmp.SetPixel(6, 41, tmpColor);
                    bmp.bmp.SetPixel(8, 30, tmpColor);
                    bmp.bmp.SetPixel(8, 41, tmpColor);
                    bmp.bmp.SetPixel(9, 40, tmpColor);
                    bmp.bmp.SetPixel(10, 33, tmpColor);
                    bmp.bmp.SetPixel(10, 36, tmpColor);
                    bmp.bmp.SetPixel(11, 33, tmpColor);
                    bmp.bmp.SetPixel(12, 31, tmpColor);
                    bmp.bmp.SetPixel(12, 32, tmpColor);
                    bmp.bmp.SetPixel(13, 33, tmpColor);
                    bmp.bmp.SetPixel(14, 33, tmpColor);
                    bmp.bmp.SetPixel(14, 36, tmpColor);
                    bmp.bmp.SetPixel(15, 40, tmpColor);
                    bmp.bmp.SetPixel(16, 30, tmpColor);
                    bmp.bmp.SetPixel(16, 41, tmpColor);
                    bmp.bmp.SetPixel(18, 41, tmpColor);
                    #endregion
                }
                else if (partie == 2)
                {
                    #region partie 2
                    bmp.bmp.SetPixel(1, 29, color);
                    bmp.bmp.SetPixel(1, 30, color);
                    bmp.bmp.SetPixel(2, 29, color);
                    bmp.bmp.SetPixel(2, 30, color);
                    bmp.bmp.SetPixel(2, 31, color);
                    bmp.bmp.SetPixel(3, 29, color);
                    bmp.bmp.SetPixel(3, 50, color);
                    bmp.bmp.SetPixel(5, 50, color);
                    bmp.bmp.SetPixel(7, 43, color);
                    bmp.bmp.SetPixel(10, 15, color);
                    bmp.bmp.SetPixel(11, 15, color);
                    bmp.bmp.SetPixel(11, 16, color);
                    bmp.bmp.SetPixel(12, 15, color);
                    bmp.bmp.SetPixel(12, 16, color);
                    bmp.bmp.SetPixel(13, 15, color);
                    bmp.bmp.SetPixel(13, 16, color);
                    bmp.bmp.SetPixel(14, 15, color);
                    bmp.bmp.SetPixel(17, 43, color);
                    bmp.bmp.SetPixel(19, 50, color);
                    bmp.bmp.SetPixel(21, 29, color);
                    bmp.bmp.SetPixel(21, 50, color);
                    bmp.bmp.SetPixel(22, 29, color);
                    bmp.bmp.SetPixel(22, 30, color);
                    bmp.bmp.SetPixel(22, 31, color);
                    bmp.bmp.SetPixel(23, 29, color);
                    bmp.bmp.SetPixel(23, 30, color);

                    Color tmpColor = Color.FromArgb((color.R - 48 >= 0) ? color.R - 48 : 0, (color.G - 112 >= 0) ? color.G - 112 : 0, (color.B - 80 >= 0) ? color.B - 80 : 0);
                    bmp.bmp.SetPixel(1, 31, tmpColor);
                    bmp.bmp.SetPixel(2, 32, tmpColor);
                    bmp.bmp.SetPixel(3, 31, tmpColor);
                    bmp.bmp.SetPixel(3, 32, tmpColor);
                    bmp.bmp.SetPixel(4, 29, tmpColor);
                    bmp.bmp.SetPixel(4, 50, tmpColor);
                    bmp.bmp.SetPixel(6, 11, tmpColor);
                    bmp.bmp.SetPixel(6, 12, tmpColor);
                    bmp.bmp.SetPixel(8, 12, tmpColor);
                    bmp.bmp.SetPixel(8, 13, tmpColor);
                    bmp.bmp.SetPixel(8, 14, tmpColor);
                    bmp.bmp.SetPixel(8, 43, tmpColor);
                    bmp.bmp.SetPixel(9, 15, tmpColor);
                    bmp.bmp.SetPixel(10, 16, tmpColor);
                    bmp.bmp.SetPixel(11, 14, tmpColor);
                    bmp.bmp.SetPixel(11, 17, tmpColor);
                    bmp.bmp.SetPixel(12, 14, tmpColor);
                    bmp.bmp.SetPixel(12, 17, tmpColor);
                    bmp.bmp.SetPixel(13, 14, tmpColor);
                    bmp.bmp.SetPixel(13, 17, tmpColor);
                    bmp.bmp.SetPixel(14, 16, tmpColor);
                    bmp.bmp.SetPixel(15, 15, tmpColor);
                    bmp.bmp.SetPixel(16, 12, tmpColor);
                    bmp.bmp.SetPixel(16, 13, tmpColor);
                    bmp.bmp.SetPixel(16, 14, tmpColor);
                    bmp.bmp.SetPixel(16, 43, tmpColor);
                    bmp.bmp.SetPixel(18, 11, tmpColor);
                    bmp.bmp.SetPixel(18, 12, tmpColor);
                    bmp.bmp.SetPixel(20, 29, tmpColor);
                    bmp.bmp.SetPixel(20, 50, tmpColor);
                    bmp.bmp.SetPixel(21, 31, tmpColor);
                    bmp.bmp.SetPixel(21, 32, tmpColor);
                    bmp.bmp.SetPixel(22, 32, tmpColor);
                    bmp.bmp.SetPixel(23, 31, tmpColor);
                    #endregion
                }
                else if (partie == 3)
                {
                    #region partie 3
                    bmp.bmp.SetPixel(1, 25, color);
                    bmp.bmp.SetPixel(1, 26, color);
                    bmp.bmp.SetPixel(2, 22, color);
                    bmp.bmp.SetPixel(2, 23, color);
                    bmp.bmp.SetPixel(2, 24, color);
                    bmp.bmp.SetPixel(2, 25, color);
                    bmp.bmp.SetPixel(2, 26, color);
                    bmp.bmp.SetPixel(3, 19, color);
                    bmp.bmp.SetPixel(3, 20, color);
                    bmp.bmp.SetPixel(3, 21, color);
                    bmp.bmp.SetPixel(3, 22, color);
                    bmp.bmp.SetPixel(3, 23, color);
                    bmp.bmp.SetPixel(3, 24, color);
                    bmp.bmp.SetPixel(4, 17, color);
                    bmp.bmp.SetPixel(4, 18, color);
                    bmp.bmp.SetPixel(4, 19, color);
                    bmp.bmp.SetPixel(4, 20, color);
                    bmp.bmp.SetPixel(4, 21, color);
                    bmp.bmp.SetPixel(4, 22, color);
                    bmp.bmp.SetPixel(4, 49, color);
                    bmp.bmp.SetPixel(5, 17, color);
                    bmp.bmp.SetPixel(5, 18, color);
                    bmp.bmp.SetPixel(5, 19, color);
                    bmp.bmp.SetPixel(5, 20, color);
                    bmp.bmp.SetPixel(5, 48, color);
                    bmp.bmp.SetPixel(5, 49, color);
                    bmp.bmp.SetPixel(6, 17, color);
                    bmp.bmp.SetPixel(6, 44, color);
                    bmp.bmp.SetPixel(6, 48, color);
                    bmp.bmp.SetPixel(6, 49, color);
                    bmp.bmp.SetPixel(7, 9, color);
                    bmp.bmp.SetPixel(7, 16, color);
                    bmp.bmp.SetPixel(7, 17, color);
                    bmp.bmp.SetPixel(7, 18, color);
                    bmp.bmp.SetPixel(7, 19, color);
                    bmp.bmp.SetPixel(7, 20, color);
                    bmp.bmp.SetPixel(7, 21, color);
                    bmp.bmp.SetPixel(7, 27, color);
                    bmp.bmp.SetPixel(7, 44, color);
                    bmp.bmp.SetPixel(7, 48, color);
                    bmp.bmp.SetPixel(8, 19, color);
                    bmp.bmp.SetPixel(8, 20, color);
                    bmp.bmp.SetPixel(8, 21, color);
                    bmp.bmp.SetPixel(8, 22, color);
                    bmp.bmp.SetPixel(8, 27, color);
                    bmp.bmp.SetPixel(8, 28, color);
                    bmp.bmp.SetPixel(8, 44, color);
                    bmp.bmp.SetPixel(9, 10, color);
                    bmp.bmp.SetPixel(9, 11, color);
                    bmp.bmp.SetPixel(9, 20, color);
                    bmp.bmp.SetPixel(9, 21, color);
                    bmp.bmp.SetPixel(9, 22, color);
                    bmp.bmp.SetPixel(9, 28, color);
                    bmp.bmp.SetPixel(10, 9, color);
                    bmp.bmp.SetPixel(10, 21, color);
                    bmp.bmp.SetPixel(10, 22, color);
                    bmp.bmp.SetPixel(10, 28, color);
                    bmp.bmp.SetPixel(11, 9, color);
                    bmp.bmp.SetPixel(11, 10, color);
                    bmp.bmp.SetPixel(11, 21, color);
                    bmp.bmp.SetPixel(11, 22, color);
                    bmp.bmp.SetPixel(11, 23, color);
                    bmp.bmp.SetPixel(11, 24, color);
                    bmp.bmp.SetPixel(11, 25, color);
                    bmp.bmp.SetPixel(11, 26, color);
                    bmp.bmp.SetPixel(11, 28, color);
                    bmp.bmp.SetPixel(12, 8, color);
                    bmp.bmp.SetPixel(12, 9, color);
                    bmp.bmp.SetPixel(12, 10, color);
                    bmp.bmp.SetPixel(12, 19, color);
                    bmp.bmp.SetPixel(12, 27, color);
                    bmp.bmp.SetPixel(13, 8, color);
                    bmp.bmp.SetPixel(13, 9, color);
                    bmp.bmp.SetPixel(13, 10, color);
                    bmp.bmp.SetPixel(13, 21, color);
                    bmp.bmp.SetPixel(13, 22, color);
                    bmp.bmp.SetPixel(13, 23, color);
                    bmp.bmp.SetPixel(13, 24, color);
                    bmp.bmp.SetPixel(13, 25, color);
                    bmp.bmp.SetPixel(13, 26, color);
                    bmp.bmp.SetPixel(13, 28, color);
                    bmp.bmp.SetPixel(14, 9, color);
                    bmp.bmp.SetPixel(14, 21, color);
                    bmp.bmp.SetPixel(14, 22, color);
                    bmp.bmp.SetPixel(14, 28, color);
                    bmp.bmp.SetPixel(15, 10, color);
                    bmp.bmp.SetPixel(15, 11, color);
                    bmp.bmp.SetPixel(15, 20, color);
                    bmp.bmp.SetPixel(15, 21, color);
                    bmp.bmp.SetPixel(15, 22, color);
                    bmp.bmp.SetPixel(15, 28, color);
                    bmp.bmp.SetPixel(16, 19, color);
                    bmp.bmp.SetPixel(16, 20, color);
                    bmp.bmp.SetPixel(16, 21, color);
                    bmp.bmp.SetPixel(16, 22, color);
                    bmp.bmp.SetPixel(16, 27, color);
                    bmp.bmp.SetPixel(16, 28, color);
                    bmp.bmp.SetPixel(16, 44, color);
                    bmp.bmp.SetPixel(17, 9, color);
                    bmp.bmp.SetPixel(17, 16, color);
                    bmp.bmp.SetPixel(17, 17, color);
                    bmp.bmp.SetPixel(17, 18, color);
                    bmp.bmp.SetPixel(17, 19, color);
                    bmp.bmp.SetPixel(17, 20, color);
                    bmp.bmp.SetPixel(17, 21, color);
                    bmp.bmp.SetPixel(17, 27, color);
                    bmp.bmp.SetPixel(17, 44, color);
                    bmp.bmp.SetPixel(17, 48, color);
                    bmp.bmp.SetPixel(18, 17, color);
                    bmp.bmp.SetPixel(18, 44, color);
                    bmp.bmp.SetPixel(18, 48, color);
                    bmp.bmp.SetPixel(18, 49, color);
                    bmp.bmp.SetPixel(19, 17, color);
                    bmp.bmp.SetPixel(19, 18, color);
                    bmp.bmp.SetPixel(19, 19, color);
                    bmp.bmp.SetPixel(19, 20, color);
                    bmp.bmp.SetPixel(19, 48, color);
                    bmp.bmp.SetPixel(19, 49, color);
                    bmp.bmp.SetPixel(20, 17, color);
                    bmp.bmp.SetPixel(20, 18, color);
                    bmp.bmp.SetPixel(20, 19, color);
                    bmp.bmp.SetPixel(20, 20, color);
                    bmp.bmp.SetPixel(20, 21, color);
                    bmp.bmp.SetPixel(20, 22, color);
                    bmp.bmp.SetPixel(20, 49, color);
                    bmp.bmp.SetPixel(21, 19, color);
                    bmp.bmp.SetPixel(21, 20, color);
                    bmp.bmp.SetPixel(21, 21, color);
                    bmp.bmp.SetPixel(21, 22, color);
                    bmp.bmp.SetPixel(21, 23, color);
                    bmp.bmp.SetPixel(21, 24, color);
                    bmp.bmp.SetPixel(22, 22, color);
                    bmp.bmp.SetPixel(22, 23, color);
                    bmp.bmp.SetPixel(22, 24, color);
                    bmp.bmp.SetPixel(22, 25, color);
                    bmp.bmp.SetPixel(22, 26, color);
                    bmp.bmp.SetPixel(23, 25, color);
                    bmp.bmp.SetPixel(23, 26, color);

                    Color tmpColor = Color.FromArgb((color.R - 32 >= 0) ? color.R - 32 : 0, (color.G - 32 >= 0) ? color.G - 32 : 0, (color.B - 24 >= 0) ? color.B - 24 : 0);
                    bmp.bmp.SetPixel(1, 22, tmpColor);
                    bmp.bmp.SetPixel(1, 23, tmpColor);
                    bmp.bmp.SetPixel(1, 24, tmpColor);
                    bmp.bmp.SetPixel(1, 27, tmpColor);
                    bmp.bmp.SetPixel(2, 19, tmpColor);
                    bmp.bmp.SetPixel(2, 20, tmpColor);
                    bmp.bmp.SetPixel(2, 21, tmpColor);
                    bmp.bmp.SetPixel(2, 27, tmpColor);
                    bmp.bmp.SetPixel(3, 17, tmpColor);
                    bmp.bmp.SetPixel(3, 18, tmpColor);
                    bmp.bmp.SetPixel(3, 25, tmpColor);
                    bmp.bmp.SetPixel(3, 26, tmpColor);
                    bmp.bmp.SetPixel(3, 51, tmpColor);
                    bmp.bmp.SetPixel(4, 16, tmpColor);
                    bmp.bmp.SetPixel(4, 23, tmpColor);
                    bmp.bmp.SetPixel(4, 51, tmpColor);
                    bmp.bmp.SetPixel(5, 16, tmpColor);
                    bmp.bmp.SetPixel(5, 21, tmpColor);
                    bmp.bmp.SetPixel(5, 47, tmpColor);
                    bmp.bmp.SetPixel(5, 51, tmpColor);
                    bmp.bmp.SetPixel(6, 10, tmpColor);
                    bmp.bmp.SetPixel(6, 15, tmpColor);
                    bmp.bmp.SetPixel(6, 16, tmpColor);
                    bmp.bmp.SetPixel(6, 18, tmpColor);
                    bmp.bmp.SetPixel(6, 19, tmpColor);
                    bmp.bmp.SetPixel(6, 45, tmpColor);
                    bmp.bmp.SetPixel(6, 47, tmpColor);
                    bmp.bmp.SetPixel(6, 50, tmpColor);
                    bmp.bmp.SetPixel(6, 51, tmpColor);
                    bmp.bmp.SetPixel(7, 11, tmpColor);
                    bmp.bmp.SetPixel(7, 15, tmpColor);
                    bmp.bmp.SetPixel(7, 26, tmpColor);
                    bmp.bmp.SetPixel(7, 28, tmpColor);
                    bmp.bmp.SetPixel(7, 45, tmpColor);
                    bmp.bmp.SetPixel(7, 47, tmpColor);
                    bmp.bmp.SetPixel(7, 49, tmpColor);
                    bmp.bmp.SetPixel(7, 50, tmpColor);
                    bmp.bmp.SetPixel(8, 10, tmpColor);
                    bmp.bmp.SetPixel(8, 11, tmpColor);
                    bmp.bmp.SetPixel(8, 18, tmpColor);
                    bmp.bmp.SetPixel(8, 26, tmpColor);
                    bmp.bmp.SetPixel(8, 45, tmpColor);
                    bmp.bmp.SetPixel(8, 47, tmpColor);
                    bmp.bmp.SetPixel(8, 48, tmpColor);
                    bmp.bmp.SetPixel(8, 49, tmpColor);
                    bmp.bmp.SetPixel(9, 9, tmpColor);
                    bmp.bmp.SetPixel(9, 12, tmpColor);
                    bmp.bmp.SetPixel(9, 17, tmpColor);
                    bmp.bmp.SetPixel(9, 18, tmpColor);
                    bmp.bmp.SetPixel(9, 19, tmpColor);
                    bmp.bmp.SetPixel(9, 27, tmpColor);
                    bmp.bmp.SetPixel(9, 29, tmpColor);
                    bmp.bmp.SetPixel(10, 8, tmpColor);
                    bmp.bmp.SetPixel(10, 13, tmpColor);
                    bmp.bmp.SetPixel(10, 14, tmpColor);
                    bmp.bmp.SetPixel(10, 18, tmpColor);
                    bmp.bmp.SetPixel(10, 19, tmpColor);
                    bmp.bmp.SetPixel(10, 20, tmpColor);
                    bmp.bmp.SetPixel(10, 27, tmpColor);
                    bmp.bmp.SetPixel(10, 29, tmpColor);
                    bmp.bmp.SetPixel(11, 8, tmpColor);
                    bmp.bmp.SetPixel(11, 13, tmpColor);
                    bmp.bmp.SetPixel(11, 19, tmpColor);
                    bmp.bmp.SetPixel(11, 20, tmpColor);
                    bmp.bmp.SetPixel(11, 27, tmpColor);
                    bmp.bmp.SetPixel(11, 29, tmpColor);
                    bmp.bmp.SetPixel(12, 13, tmpColor);
                    bmp.bmp.SetPixel(13, 13, tmpColor);
                    bmp.bmp.SetPixel(13, 19, tmpColor);
                    bmp.bmp.SetPixel(13, 20, tmpColor);
                    bmp.bmp.SetPixel(13, 27, tmpColor);
                    bmp.bmp.SetPixel(13, 29, tmpColor);
                    bmp.bmp.SetPixel(14, 8, tmpColor);
                    bmp.bmp.SetPixel(14, 13, tmpColor);
                    bmp.bmp.SetPixel(14, 14, tmpColor);
                    bmp.bmp.SetPixel(14, 18, tmpColor);
                    bmp.bmp.SetPixel(14, 19, tmpColor);
                    bmp.bmp.SetPixel(14, 20, tmpColor);
                    bmp.bmp.SetPixel(14, 27, tmpColor);
                    bmp.bmp.SetPixel(14, 29, tmpColor);
                    bmp.bmp.SetPixel(15, 9, tmpColor);
                    bmp.bmp.SetPixel(15, 12, tmpColor);
                    bmp.bmp.SetPixel(15, 17, tmpColor);
                    bmp.bmp.SetPixel(15, 18, tmpColor);
                    bmp.bmp.SetPixel(15, 19, tmpColor);
                    bmp.bmp.SetPixel(15, 27, tmpColor);
                    bmp.bmp.SetPixel(15, 29, tmpColor);
                    bmp.bmp.SetPixel(16, 10, tmpColor);
                    bmp.bmp.SetPixel(16, 11, tmpColor);
                    bmp.bmp.SetPixel(16, 18, tmpColor);
                    bmp.bmp.SetPixel(16, 26, tmpColor);
                    bmp.bmp.SetPixel(16, 45, tmpColor);
                    bmp.bmp.SetPixel(16, 47, tmpColor);
                    bmp.bmp.SetPixel(16, 48, tmpColor);
                    bmp.bmp.SetPixel(16, 49, tmpColor);
                    bmp.bmp.SetPixel(17, 11, tmpColor);
                    bmp.bmp.SetPixel(17, 15, tmpColor);
                    bmp.bmp.SetPixel(17, 26, tmpColor);
                    bmp.bmp.SetPixel(17, 28, tmpColor);
                    bmp.bmp.SetPixel(17, 45, tmpColor);
                    bmp.bmp.SetPixel(17, 47, tmpColor);
                    bmp.bmp.SetPixel(17, 49, tmpColor);
                    bmp.bmp.SetPixel(17, 50, tmpColor);
                    bmp.bmp.SetPixel(18, 10, tmpColor);
                    bmp.bmp.SetPixel(18, 15, tmpColor);
                    bmp.bmp.SetPixel(18, 16, tmpColor);
                    bmp.bmp.SetPixel(18, 18, tmpColor);
                    bmp.bmp.SetPixel(18, 19, tmpColor);
                    bmp.bmp.SetPixel(18, 45, tmpColor);
                    bmp.bmp.SetPixel(18, 47, tmpColor);
                    bmp.bmp.SetPixel(18, 50, tmpColor);
                    bmp.bmp.SetPixel(18, 51, tmpColor);
                    bmp.bmp.SetPixel(19, 10, tmpColor);
                    bmp.bmp.SetPixel(19, 16, tmpColor);
                    bmp.bmp.SetPixel(19, 21, tmpColor);
                    bmp.bmp.SetPixel(19, 47, tmpColor);
                    bmp.bmp.SetPixel(19, 51, tmpColor);
                    bmp.bmp.SetPixel(20, 16, tmpColor);
                    bmp.bmp.SetPixel(20, 23, tmpColor);
                    bmp.bmp.SetPixel(20, 51, tmpColor);
                    bmp.bmp.SetPixel(21, 17, tmpColor);
                    bmp.bmp.SetPixel(21, 18, tmpColor);
                    bmp.bmp.SetPixel(21, 25, tmpColor);
                    bmp.bmp.SetPixel(21, 26, tmpColor);
                    bmp.bmp.SetPixel(21, 51, tmpColor);
                    bmp.bmp.SetPixel(22, 19, tmpColor);
                    bmp.bmp.SetPixel(22, 20, tmpColor);
                    bmp.bmp.SetPixel(22, 21, tmpColor);
                    bmp.bmp.SetPixel(22, 27, tmpColor);
                    bmp.bmp.SetPixel(23, 22, tmpColor);
                    bmp.bmp.SetPixel(23, 23, tmpColor);
                    bmp.bmp.SetPixel(23, 24, tmpColor);
                    bmp.bmp.SetPixel(23, 27, tmpColor);
                    #endregion
                }
                #endregion
            }
            else if (className == Enums.ActorClass.ClassName.choji)
            {
                #region
                if (partie == 1)
                {
                    #region
                    bmp.bmp.SetPixel(6, 11, color);
                    bmp.bmp.SetPixel(7, 10, color);
                    bmp.bmp.SetPixel(8, 7, color);
                    bmp.bmp.SetPixel(8, 10, color);
                    bmp.bmp.SetPixel(9, 7, color);
                    bmp.bmp.SetPixel(9, 9, color);
                    bmp.bmp.SetPixel(10, 6, color);
                    bmp.bmp.SetPixel(10, 7, color);
                    bmp.bmp.SetPixel(10, 8, color);
                    bmp.bmp.SetPixel(10, 9, color);
                    bmp.bmp.SetPixel(11, 6, color);
                    bmp.bmp.SetPixel(11, 7, color);
                    bmp.bmp.SetPixel(11, 8, color);
                    bmp.bmp.SetPixel(11, 9, color);
                    bmp.bmp.SetPixel(11, 10, color);
                    bmp.bmp.SetPixel(11, 11, color);
                    bmp.bmp.SetPixel(11, 12, color);
                    bmp.bmp.SetPixel(12, 4, color);
                    bmp.bmp.SetPixel(12, 5, color);
                    bmp.bmp.SetPixel(12, 6, color);
                    bmp.bmp.SetPixel(12, 7, color);
                    bmp.bmp.SetPixel(12, 8, color);
                    bmp.bmp.SetPixel(12, 9, color);
                    bmp.bmp.SetPixel(12, 10, color);
                    bmp.bmp.SetPixel(12, 11, color);
                    bmp.bmp.SetPixel(13, 5, color);
                    bmp.bmp.SetPixel(13, 6, color);
                    bmp.bmp.SetPixel(13, 7, color);
                    bmp.bmp.SetPixel(13, 8, color);
                    bmp.bmp.SetPixel(13, 9, color);
                    bmp.bmp.SetPixel(13, 10, color);
                    bmp.bmp.SetPixel(14, 4, color);
                    bmp.bmp.SetPixel(14, 5, color);
                    bmp.bmp.SetPixel(14, 6, color);
                    bmp.bmp.SetPixel(14, 7, color);
                    bmp.bmp.SetPixel(14, 8, color);
                    bmp.bmp.SetPixel(14, 9, color);
                    bmp.bmp.SetPixel(14, 10, color);
                    bmp.bmp.SetPixel(14, 11, color);
                    bmp.bmp.SetPixel(15, 3, color);
                    bmp.bmp.SetPixel(15, 4, color);
                    bmp.bmp.SetPixel(15, 5, color);
                    bmp.bmp.SetPixel(15, 6, color);
                    bmp.bmp.SetPixel(15, 7, color);
                    bmp.bmp.SetPixel(15, 8, color);
                    bmp.bmp.SetPixel(15, 9, color);
                    bmp.bmp.SetPixel(15, 10, color);
                    bmp.bmp.SetPixel(15, 11, color);
                    bmp.bmp.SetPixel(16, 5, color);
                    bmp.bmp.SetPixel(16, 6, color);
                    bmp.bmp.SetPixel(16, 7, color);
                    bmp.bmp.SetPixel(16, 8, color);
                    bmp.bmp.SetPixel(16, 9, color);
                    bmp.bmp.SetPixel(16, 10, color);
                    bmp.bmp.SetPixel(16, 11, color);
                    bmp.bmp.SetPixel(17, 5, color);
                    bmp.bmp.SetPixel(17, 6, color);
                    bmp.bmp.SetPixel(17, 7, color);
                    bmp.bmp.SetPixel(17, 8, color);
                    bmp.bmp.SetPixel(17, 9, color);
                    bmp.bmp.SetPixel(17, 10, color);
                    bmp.bmp.SetPixel(18, 4, color);
                    bmp.bmp.SetPixel(18, 5, color);
                    bmp.bmp.SetPixel(18, 6, color);
                    bmp.bmp.SetPixel(18, 7, color);
                    bmp.bmp.SetPixel(18, 8, color);
                    bmp.bmp.SetPixel(18, 9, color);
                    bmp.bmp.SetPixel(18, 10, color);
                    bmp.bmp.SetPixel(18, 11, color);
                    bmp.bmp.SetPixel(19, 6, color);
                    bmp.bmp.SetPixel(19, 7, color);
                    bmp.bmp.SetPixel(19, 8, color);
                    bmp.bmp.SetPixel(19, 9, color);
                    bmp.bmp.SetPixel(19, 10, color);
                    bmp.bmp.SetPixel(19, 11, color);
                    bmp.bmp.SetPixel(19, 12, color);
                    bmp.bmp.SetPixel(20, 6, color);
                    bmp.bmp.SetPixel(20, 7, color);
                    bmp.bmp.SetPixel(20, 9, color);
                    bmp.bmp.SetPixel(21, 7, color);
                    bmp.bmp.SetPixel(22, 10, color);
                    bmp.bmp.SetPixel(23, 11, color);

                    Color tmpColor = Color.FromArgb((color.R - 56 >= 0) ? color.R - 56 : 0, (color.G - 32 >= 0) ? color.G - 32 : 0, (color.B - 16 >= 0) ? color.B - 16 : 0);
                    bmp.bmp.SetPixel(7, 11, tmpColor);
                    bmp.bmp.SetPixel(7, 12, tmpColor);
                    bmp.bmp.SetPixel(7, 15, tmpColor);
                    bmp.bmp.SetPixel(8, 9, tmpColor);
                    bmp.bmp.SetPixel(8, 14, tmpColor);
                    bmp.bmp.SetPixel(9, 6, tmpColor);
                    bmp.bmp.SetPixel(9, 8, tmpColor);
                    bmp.bmp.SetPixel(9, 10, tmpColor);
                    bmp.bmp.SetPixel(9, 11, tmpColor);
                    bmp.bmp.SetPixel(10, 10, tmpColor);
                    bmp.bmp.SetPixel(13, 11, tmpColor);
                    bmp.bmp.SetPixel(17, 11, tmpColor);
                    bmp.bmp.SetPixel(20, 8, tmpColor);
                    bmp.bmp.SetPixel(20, 10, tmpColor);
                    bmp.bmp.SetPixel(21, 6, tmpColor);
                    bmp.bmp.SetPixel(21, 8, tmpColor);
                    bmp.bmp.SetPixel(21, 9, tmpColor);
                    bmp.bmp.SetPixel(21, 10, tmpColor);
                    bmp.bmp.SetPixel(21, 11, tmpColor);
                    bmp.bmp.SetPixel(22, 7, tmpColor);
                    bmp.bmp.SetPixel(22, 11, tmpColor);
                    bmp.bmp.SetPixel(22, 14, tmpColor);
                    bmp.bmp.SetPixel(23, 15, tmpColor);
                    #endregion
                }
                else if (partie == 2)
                {
                    #region
                    bmp.bmp.SetPixel(5, 25, color);
                    bmp.bmp.SetPixel(5, 26, color);
                    bmp.bmp.SetPixel(6, 25, color);
                    bmp.bmp.SetPixel(6, 26, color);
                    bmp.bmp.SetPixel(7, 24, color);
                    bmp.bmp.SetPixel(7, 25, color);
                    bmp.bmp.SetPixel(8, 22, color);
                    bmp.bmp.SetPixel(8, 23, color);
                    bmp.bmp.SetPixel(9, 19, color);
                    bmp.bmp.SetPixel(9, 20, color);
                    bmp.bmp.SetPixel(9, 21, color);
                    bmp.bmp.SetPixel(10, 20, color);
                    bmp.bmp.SetPixel(10, 21, color);
                    bmp.bmp.SetPixel(10, 22, color);
                    bmp.bmp.SetPixel(10, 23, color);
                    bmp.bmp.SetPixel(10, 42, color);
                    bmp.bmp.SetPixel(11, 36, color);
                    bmp.bmp.SetPixel(11, 37, color);
                    bmp.bmp.SetPixel(11, 38, color);
                    bmp.bmp.SetPixel(11, 39, color);
                    bmp.bmp.SetPixel(11, 40, color);
                    bmp.bmp.SetPixel(11, 41, color);
                    bmp.bmp.SetPixel(11, 42, color);
                    bmp.bmp.SetPixel(11, 43, color);
                    bmp.bmp.SetPixel(12, 18, color);
                    bmp.bmp.SetPixel(12, 36, color);
                    bmp.bmp.SetPixel(12, 37, color);
                    bmp.bmp.SetPixel(12, 38, color);
                    bmp.bmp.SetPixel(12, 39, color);
                    bmp.bmp.SetPixel(12, 40, color);
                    bmp.bmp.SetPixel(12, 41, color);
                    bmp.bmp.SetPixel(12, 42, color);
                    bmp.bmp.SetPixel(12, 43, color);
                    bmp.bmp.SetPixel(13, 25, color);
                    bmp.bmp.SetPixel(13, 26, color);
                    bmp.bmp.SetPixel(13, 36, color);
                    bmp.bmp.SetPixel(13, 37, color);
                    bmp.bmp.SetPixel(13, 38, color);
                    bmp.bmp.SetPixel(13, 39, color);
                    bmp.bmp.SetPixel(13, 40, color);
                    bmp.bmp.SetPixel(13, 41, color);
                    bmp.bmp.SetPixel(13, 42, color);
                    bmp.bmp.SetPixel(13, 43, color);
                    bmp.bmp.SetPixel(14, 24, color);
                    bmp.bmp.SetPixel(14, 26, color);
                    bmp.bmp.SetPixel(14, 27, color);
                    bmp.bmp.SetPixel(14, 36, color);
                    bmp.bmp.SetPixel(15, 24, color);
                    bmp.bmp.SetPixel(15, 25, color);
                    bmp.bmp.SetPixel(15, 27, color);
                    bmp.bmp.SetPixel(16, 24, color);
                    bmp.bmp.SetPixel(16, 26, color);
                    bmp.bmp.SetPixel(16, 27, color);
                    bmp.bmp.SetPixel(16, 36, color);
                    bmp.bmp.SetPixel(17, 25, color);
                    bmp.bmp.SetPixel(17, 26, color);
                    bmp.bmp.SetPixel(17, 36, color);
                    bmp.bmp.SetPixel(17, 37, color);
                    bmp.bmp.SetPixel(17, 38, color);
                    bmp.bmp.SetPixel(17, 39, color);
                    bmp.bmp.SetPixel(17, 40, color);
                    bmp.bmp.SetPixel(17, 41, color);
                    bmp.bmp.SetPixel(17, 42, color);
                    bmp.bmp.SetPixel(17, 43, color);
                    bmp.bmp.SetPixel(18, 18, color);
                    bmp.bmp.SetPixel(18, 36, color);
                    bmp.bmp.SetPixel(18, 37, color);
                    bmp.bmp.SetPixel(18, 38, color);
                    bmp.bmp.SetPixel(18, 39, color);
                    bmp.bmp.SetPixel(18, 40, color);
                    bmp.bmp.SetPixel(18, 41, color);
                    bmp.bmp.SetPixel(18, 42, color);
                    bmp.bmp.SetPixel(18, 43, color);
                    bmp.bmp.SetPixel(19, 36, color);
                    bmp.bmp.SetPixel(19, 37, color);
                    bmp.bmp.SetPixel(19, 38, color);
                    bmp.bmp.SetPixel(19, 39, color);
                    bmp.bmp.SetPixel(19, 40, color);
                    bmp.bmp.SetPixel(19, 41, color);
                    bmp.bmp.SetPixel(19, 42, color);
                    bmp.bmp.SetPixel(19, 43, color);
                    bmp.bmp.SetPixel(20, 20, color);
                    bmp.bmp.SetPixel(20, 21, color);
                    bmp.bmp.SetPixel(20, 22, color);
                    bmp.bmp.SetPixel(20, 23, color);
                    bmp.bmp.SetPixel(20, 42, color);
                    bmp.bmp.SetPixel(21, 19, color);
                    bmp.bmp.SetPixel(21, 20, color);
                    bmp.bmp.SetPixel(21, 21, color);
                    bmp.bmp.SetPixel(22, 22, color);
                    bmp.bmp.SetPixel(22, 23, color);
                    bmp.bmp.SetPixel(23, 24, color);
                    bmp.bmp.SetPixel(23, 25, color);
                    bmp.bmp.SetPixel(24, 25, color);
                    bmp.bmp.SetPixel(24, 26, color);
                    bmp.bmp.SetPixel(25, 25, color);
                    bmp.bmp.SetPixel(25, 26, color);

                    Color tmpColor = Color.FromArgb((color.R - 72 >= 0) ? color.R - 72 : 0, (color.G - 8 >= 0) ? color.G - 8 : 0, (color.B - 8 >= 0) ? color.B - 8 : 0);
                    bmp.bmp.SetPixel(4, 26, tmpColor);
                    bmp.bmp.SetPixel(5, 27, tmpColor);
                    bmp.bmp.SetPixel(6, 27, tmpColor);
                    bmp.bmp.SetPixel(7, 23, tmpColor);
                    bmp.bmp.SetPixel(7, 26, tmpColor);
                    bmp.bmp.SetPixel(7, 27, tmpColor);
                    bmp.bmp.SetPixel(8, 21, tmpColor);
                    bmp.bmp.SetPixel(8, 24, tmpColor);
                    bmp.bmp.SetPixel(8, 25, tmpColor);
                    bmp.bmp.SetPixel(8, 42, tmpColor);
                    bmp.bmp.SetPixel(9, 18, tmpColor);
                    bmp.bmp.SetPixel(9, 22, tmpColor);
                    bmp.bmp.SetPixel(9, 29, tmpColor);
                    bmp.bmp.SetPixel(9, 31, tmpColor);
                    bmp.bmp.SetPixel(9, 34, tmpColor);
                    bmp.bmp.SetPixel(9, 42, tmpColor);
                    bmp.bmp.SetPixel(10, 18, tmpColor);
                    bmp.bmp.SetPixel(10, 19, tmpColor);
                    bmp.bmp.SetPixel(10, 29, tmpColor);
                    bmp.bmp.SetPixel(10, 31, tmpColor);
                    bmp.bmp.SetPixel(10, 34, tmpColor);
                    bmp.bmp.SetPixel(10, 35, tmpColor);
                    bmp.bmp.SetPixel(10, 37, tmpColor);
                    bmp.bmp.SetPixel(10, 39, tmpColor);
                    bmp.bmp.SetPixel(10, 41, tmpColor);
                    bmp.bmp.SetPixel(10, 43, tmpColor);
                    bmp.bmp.SetPixel(11, 14, tmpColor);
                    bmp.bmp.SetPixel(11, 30, tmpColor);
                    bmp.bmp.SetPixel(11, 32, tmpColor);
                    bmp.bmp.SetPixel(11, 35, tmpColor);
                    bmp.bmp.SetPixel(12, 13, tmpColor);
                    bmp.bmp.SetPixel(12, 14, tmpColor);
                    bmp.bmp.SetPixel(12, 30, tmpColor);
                    bmp.bmp.SetPixel(12, 32, tmpColor);
                    bmp.bmp.SetPixel(12, 35, tmpColor);
                    bmp.bmp.SetPixel(13, 12, tmpColor);
                    bmp.bmp.SetPixel(13, 15, tmpColor);
                    bmp.bmp.SetPixel(13, 30, tmpColor);
                    bmp.bmp.SetPixel(13, 32, tmpColor);
                    bmp.bmp.SetPixel(13, 35, tmpColor);
                    bmp.bmp.SetPixel(14, 12, tmpColor);
                    bmp.bmp.SetPixel(14, 15, tmpColor);
                    bmp.bmp.SetPixel(14, 21, tmpColor);
                    bmp.bmp.SetPixel(14, 30, tmpColor);
                    bmp.bmp.SetPixel(14, 32, tmpColor);
                    bmp.bmp.SetPixel(14, 35, tmpColor);
                    bmp.bmp.SetPixel(15, 12, tmpColor);
                    bmp.bmp.SetPixel(15, 15, tmpColor);
                    bmp.bmp.SetPixel(15, 21, tmpColor);
                    bmp.bmp.SetPixel(15, 30, tmpColor);
                    bmp.bmp.SetPixel(15, 32, tmpColor);
                    bmp.bmp.SetPixel(16, 12, tmpColor);
                    bmp.bmp.SetPixel(16, 15, tmpColor);
                    bmp.bmp.SetPixel(16, 21, tmpColor);
                    bmp.bmp.SetPixel(16, 30, tmpColor);
                    bmp.bmp.SetPixel(16, 32, tmpColor);
                    bmp.bmp.SetPixel(16, 35, tmpColor);
                    bmp.bmp.SetPixel(17, 12, tmpColor);
                    bmp.bmp.SetPixel(17, 15, tmpColor);
                    bmp.bmp.SetPixel(17, 30, tmpColor);
                    bmp.bmp.SetPixel(17, 32, tmpColor);
                    bmp.bmp.SetPixel(17, 35, tmpColor);
                    bmp.bmp.SetPixel(18, 13, tmpColor);
                    bmp.bmp.SetPixel(18, 14, tmpColor);
                    bmp.bmp.SetPixel(18, 30, tmpColor);
                    bmp.bmp.SetPixel(18, 32, tmpColor);
                    bmp.bmp.SetPixel(18, 35, tmpColor);
                    bmp.bmp.SetPixel(19, 14, tmpColor);
                    bmp.bmp.SetPixel(19, 30, tmpColor);
                    bmp.bmp.SetPixel(19, 32, tmpColor);
                    bmp.bmp.SetPixel(19, 35, tmpColor);
                    bmp.bmp.SetPixel(20, 18, tmpColor);
                    bmp.bmp.SetPixel(20, 19, tmpColor);
                    bmp.bmp.SetPixel(20, 29, tmpColor);
                    bmp.bmp.SetPixel(20, 31, tmpColor);
                    bmp.bmp.SetPixel(20, 34, tmpColor);
                    bmp.bmp.SetPixel(20, 35, tmpColor);
                    bmp.bmp.SetPixel(20, 37, tmpColor);
                    bmp.bmp.SetPixel(20, 39, tmpColor);
                    bmp.bmp.SetPixel(20, 41, tmpColor);
                    bmp.bmp.SetPixel(20, 43, tmpColor);
                    bmp.bmp.SetPixel(21, 18, tmpColor);
                    bmp.bmp.SetPixel(21, 22, tmpColor);
                    bmp.bmp.SetPixel(21, 29, tmpColor);
                    bmp.bmp.SetPixel(21, 31, tmpColor);
                    bmp.bmp.SetPixel(21, 34, tmpColor);
                    bmp.bmp.SetPixel(21, 42, tmpColor);
                    bmp.bmp.SetPixel(22, 21, tmpColor);
                    bmp.bmp.SetPixel(22, 24, tmpColor);
                    bmp.bmp.SetPixel(22, 25, tmpColor);
                    bmp.bmp.SetPixel(22, 42, tmpColor);
                    bmp.bmp.SetPixel(23, 23, tmpColor);
                    bmp.bmp.SetPixel(23, 26, tmpColor);
                    bmp.bmp.SetPixel(23, 27, tmpColor);
                    bmp.bmp.SetPixel(24, 27, tmpColor);
                    bmp.bmp.SetPixel(25, 27, tmpColor);
                    bmp.bmp.SetPixel(26, 26, tmpColor);
                    #endregion
                }
                else if (partie == 3)
                {
                    #region
                    bmp.bmp.SetPixel(4, 29, color);
                    bmp.bmp.SetPixel(4, 30, color);
                    bmp.bmp.SetPixel(4, 31, color);
                    bmp.bmp.SetPixel(4, 33, color);
                    bmp.bmp.SetPixel(4, 34, color);
                    bmp.bmp.SetPixel(4, 35, color);
                    bmp.bmp.SetPixel(5, 23, color);
                    bmp.bmp.SetPixel(6, 21, color);
                    bmp.bmp.SetPixel(7, 19, color);
                    bmp.bmp.SetPixel(8, 36, color);
                    bmp.bmp.SetPixel(8, 38, color);
                    bmp.bmp.SetPixel(8, 40, color);
                    bmp.bmp.SetPixel(10, 27, color);
                    bmp.bmp.SetPixel(10, 28, color);
                    bmp.bmp.SetPixel(10, 30, color);
                    bmp.bmp.SetPixel(10, 32, color);
                    bmp.bmp.SetPixel(10, 47, color);
                    bmp.bmp.SetPixel(11, 25, color);
                    bmp.bmp.SetPixel(11, 26, color);
                    bmp.bmp.SetPixel(11, 27, color);
                    bmp.bmp.SetPixel(11, 28, color);
                    bmp.bmp.SetPixel(11, 48, color);
                    bmp.bmp.SetPixel(12, 21, color);
                    bmp.bmp.SetPixel(12, 24, color);
                    bmp.bmp.SetPixel(12, 25, color);
                    bmp.bmp.SetPixel(12, 26, color);
                    bmp.bmp.SetPixel(12, 27, color);
                    bmp.bmp.SetPixel(12, 28, color);
                    bmp.bmp.SetPixel(12, 29, color);
                    bmp.bmp.SetPixel(12, 31, color);
                    bmp.bmp.SetPixel(12, 33, color);
                    bmp.bmp.SetPixel(13, 13, color);
                    bmp.bmp.SetPixel(13, 14, color);
                    bmp.bmp.SetPixel(13, 23, color);
                    bmp.bmp.SetPixel(13, 24, color);
                    bmp.bmp.SetPixel(13, 27, color);
                    bmp.bmp.SetPixel(13, 28, color);
                    bmp.bmp.SetPixel(13, 29, color);
                    bmp.bmp.SetPixel(13, 31, color);
                    bmp.bmp.SetPixel(13, 33, color);
                    bmp.bmp.SetPixel(14, 23, color);
                    bmp.bmp.SetPixel(14, 25, color);
                    bmp.bmp.SetPixel(14, 28, color);
                    bmp.bmp.SetPixel(14, 29, color);
                    bmp.bmp.SetPixel(14, 31, color);
                    bmp.bmp.SetPixel(14, 33, color);
                    bmp.bmp.SetPixel(15, 23, color);
                    bmp.bmp.SetPixel(15, 26, color);
                    bmp.bmp.SetPixel(15, 28, color);
                    bmp.bmp.SetPixel(15, 29, color);
                    bmp.bmp.SetPixel(15, 31, color);
                    bmp.bmp.SetPixel(15, 33, color);
                    bmp.bmp.SetPixel(16, 23, color);
                    bmp.bmp.SetPixel(16, 25, color);
                    bmp.bmp.SetPixel(16, 28, color);
                    bmp.bmp.SetPixel(16, 29, color);
                    bmp.bmp.SetPixel(16, 31, color);
                    bmp.bmp.SetPixel(16, 33, color);
                    bmp.bmp.SetPixel(17, 13, color);
                    bmp.bmp.SetPixel(17, 14, color);
                    bmp.bmp.SetPixel(17, 23, color);
                    bmp.bmp.SetPixel(17, 24, color);
                    bmp.bmp.SetPixel(17, 27, color);
                    bmp.bmp.SetPixel(17, 28, color);
                    bmp.bmp.SetPixel(17, 29, color);
                    bmp.bmp.SetPixel(17, 31, color);
                    bmp.bmp.SetPixel(17, 33, color);
                    bmp.bmp.SetPixel(18, 21, color);
                    bmp.bmp.SetPixel(18, 24, color);
                    bmp.bmp.SetPixel(18, 25, color);
                    bmp.bmp.SetPixel(18, 26, color);
                    bmp.bmp.SetPixel(18, 27, color);
                    bmp.bmp.SetPixel(18, 28, color);
                    bmp.bmp.SetPixel(18, 29, color);
                    bmp.bmp.SetPixel(18, 31, color);
                    bmp.bmp.SetPixel(18, 33, color);
                    bmp.bmp.SetPixel(19, 25, color);
                    bmp.bmp.SetPixel(19, 26, color);
                    bmp.bmp.SetPixel(19, 27, color);
                    bmp.bmp.SetPixel(19, 28, color);
                    bmp.bmp.SetPixel(19, 48, color);
                    bmp.bmp.SetPixel(20, 27, color);
                    bmp.bmp.SetPixel(20, 28, color);
                    bmp.bmp.SetPixel(20, 30, color);
                    bmp.bmp.SetPixel(20, 32, color);
                    bmp.bmp.SetPixel(20, 47, color);
                    bmp.bmp.SetPixel(22, 36, color);
                    bmp.bmp.SetPixel(22, 38, color);
                    bmp.bmp.SetPixel(22, 40, color);
                    bmp.bmp.SetPixel(23, 19, color);
                    bmp.bmp.SetPixel(24, 21, color);
                    bmp.bmp.SetPixel(25, 23, color);
                    bmp.bmp.SetPixel(26, 29, color);
                    bmp.bmp.SetPixel(26, 30, color);
                    bmp.bmp.SetPixel(26, 31, color);
                    bmp.bmp.SetPixel(26, 33, color);
                    bmp.bmp.SetPixel(26, 34, color);
                    bmp.bmp.SetPixel(26, 35, color);

                    Color tmpColor = Color.FromArgb((color.R - 40 >= 0) ? color.R - 40 : 0, (color.G - 40 >= 0) ? color.G - 40 : 0, (color.B - 40 >= 0) ? color.B - 40 : 0);
                    bmp.bmp.SetPixel(4, 28, tmpColor);
                    bmp.bmp.SetPixel(6, 22, tmpColor);
                    bmp.bmp.SetPixel(6, 29, tmpColor);
                    bmp.bmp.SetPixel(6, 30, tmpColor);
                    bmp.bmp.SetPixel(6, 31, tmpColor);
                    bmp.bmp.SetPixel(7, 20, tmpColor);
                    bmp.bmp.SetPixel(9, 27, tmpColor);
                    bmp.bmp.SetPixel(9, 28, tmpColor);
                    bmp.bmp.SetPixel(9, 30, tmpColor);
                    bmp.bmp.SetPixel(9, 32, tmpColor);
                    bmp.bmp.SetPixel(9, 36, tmpColor);
                    bmp.bmp.SetPixel(9, 38, tmpColor);
                    bmp.bmp.SetPixel(9, 40, tmpColor);
                    bmp.bmp.SetPixel(10, 25, tmpColor);
                    bmp.bmp.SetPixel(10, 26, tmpColor);
                    bmp.bmp.SetPixel(11, 24, tmpColor);
                    bmp.bmp.SetPixel(11, 29, tmpColor);
                    bmp.bmp.SetPixel(11, 31, tmpColor);
                    bmp.bmp.SetPixel(11, 33, tmpColor);
                    bmp.bmp.SetPixel(11, 47, tmpColor);
                    bmp.bmp.SetPixel(12, 20, tmpColor);
                    bmp.bmp.SetPixel(12, 23, tmpColor);
                    bmp.bmp.SetPixel(18, 20, tmpColor);
                    bmp.bmp.SetPixel(18, 23, tmpColor);
                    bmp.bmp.SetPixel(19, 24, tmpColor);
                    bmp.bmp.SetPixel(19, 29, tmpColor);
                    bmp.bmp.SetPixel(19, 31, tmpColor);
                    bmp.bmp.SetPixel(19, 33, tmpColor);
                    bmp.bmp.SetPixel(19, 47, tmpColor);
                    bmp.bmp.SetPixel(20, 25, tmpColor);
                    bmp.bmp.SetPixel(20, 26, tmpColor);
                    bmp.bmp.SetPixel(21, 27, tmpColor);
                    bmp.bmp.SetPixel(21, 28, tmpColor);
                    bmp.bmp.SetPixel(21, 30, tmpColor);
                    bmp.bmp.SetPixel(21, 32, tmpColor);
                    bmp.bmp.SetPixel(21, 36, tmpColor);
                    bmp.bmp.SetPixel(21, 38, tmpColor);
                    bmp.bmp.SetPixel(21, 40, tmpColor);
                    bmp.bmp.SetPixel(23, 20, tmpColor);
                    bmp.bmp.SetPixel(24, 22, tmpColor);
                    bmp.bmp.SetPixel(24, 29, tmpColor);
                    bmp.bmp.SetPixel(24, 30, tmpColor);
                    bmp.bmp.SetPixel(24, 31, tmpColor);
                    bmp.bmp.SetPixel(26, 28, tmpColor);

                    tmpColor = Color.FromArgb((color.R - 88 >= 0) ? color.R - 88 : 0, (color.G - 88 >= 0) ? color.G - 88 : 0, (color.B - 88 >= 0) ? color.B - 88 : 0);
                    bmp.bmp.SetPixel(7, 56, tmpColor);
                    bmp.bmp.SetPixel(8, 52, tmpColor);
                    bmp.bmp.SetPixel(8, 56, tmpColor);
                    bmp.bmp.SetPixel(9, 45, tmpColor);
                    bmp.bmp.SetPixel(9, 50, tmpColor);
                    bmp.bmp.SetPixel(9, 52, tmpColor);
                    bmp.bmp.SetPixel(9, 56, tmpColor);
                    bmp.bmp.SetPixel(10, 49, tmpColor);
                    bmp.bmp.SetPixel(10, 52, tmpColor);
                    bmp.bmp.SetPixel(10, 54, tmpColor);
                    bmp.bmp.SetPixel(10, 56, tmpColor);
                    bmp.bmp.SetPixel(11, 49, tmpColor);
                    bmp.bmp.SetPixel(11, 52, tmpColor);
                    bmp.bmp.SetPixel(11, 53, tmpColor);
                    bmp.bmp.SetPixel(11, 54, tmpColor);
                    bmp.bmp.SetPixel(12, 45, tmpColor);
                    bmp.bmp.SetPixel(12, 50, tmpColor);
                    bmp.bmp.SetPixel(12, 54, tmpColor);
                    bmp.bmp.SetPixel(12, 55, tmpColor);
                    bmp.bmp.SetPixel(15, 37, tmpColor);
                    bmp.bmp.SetPixel(15, 38, tmpColor);
                    bmp.bmp.SetPixel(15, 39, tmpColor);
                    bmp.bmp.SetPixel(18, 45, tmpColor);
                    bmp.bmp.SetPixel(18, 50, tmpColor);
                    bmp.bmp.SetPixel(18, 54, tmpColor);
                    bmp.bmp.SetPixel(18, 55, tmpColor);
                    bmp.bmp.SetPixel(19, 49, tmpColor);
                    bmp.bmp.SetPixel(19, 52, tmpColor);
                    bmp.bmp.SetPixel(19, 53, tmpColor);
                    bmp.bmp.SetPixel(19, 54, tmpColor);
                    bmp.bmp.SetPixel(20, 49, tmpColor);
                    bmp.bmp.SetPixel(20, 52, tmpColor);
                    bmp.bmp.SetPixel(20, 54, tmpColor);
                    bmp.bmp.SetPixel(20, 56, tmpColor);
                    bmp.bmp.SetPixel(21, 45, tmpColor);
                    bmp.bmp.SetPixel(21, 50, tmpColor);
                    bmp.bmp.SetPixel(21, 52, tmpColor);
                    bmp.bmp.SetPixel(21, 56, tmpColor);
                    bmp.bmp.SetPixel(22, 52, tmpColor);
                    bmp.bmp.SetPixel(22, 56, tmpColor);
                    bmp.bmp.SetPixel(23, 56, tmpColor);
                    #endregion
                }
                #endregion
            }
            else if (className == Enums.ActorClass.ClassName.kabuto)
            {
                #region
                if (partie == 1)
                {
                    #region
                    bmp.bmp.SetPixel(2, 18, color);
                    bmp.bmp.SetPixel(2, 19, color);
                    bmp.bmp.SetPixel(3, 17, color);
                    bmp.bmp.SetPixel(3, 18, color);
                    bmp.bmp.SetPixel(3, 19, color);
                    bmp.bmp.SetPixel(3, 20, color);
                    bmp.bmp.SetPixel(6, 6, color);
                    bmp.bmp.SetPixel(6, 7, color);
                    bmp.bmp.SetPixel(6, 8, color);
                    bmp.bmp.SetPixel(6, 9, color);
                    bmp.bmp.SetPixel(6, 33, color);
                    bmp.bmp.SetPixel(6, 35, color);
                    bmp.bmp.SetPixel(7, 4, color);
                    bmp.bmp.SetPixel(7, 5, color);
                    bmp.bmp.SetPixel(7, 6, color);
                    bmp.bmp.SetPixel(8, 3, color);
                    bmp.bmp.SetPixel(8, 4, color);
                    bmp.bmp.SetPixel(8, 5, color);
                    bmp.bmp.SetPixel(9, 1, color);
                    bmp.bmp.SetPixel(9, 4, color);
                    bmp.bmp.SetPixel(9, 10, color);
                    bmp.bmp.SetPixel(9, 11, color);
                    bmp.bmp.SetPixel(9, 33, color);
                    bmp.bmp.SetPixel(9, 35, color);
                    bmp.bmp.SetPixel(10, 1, color);
                    bmp.bmp.SetPixel(10, 2, color);
                    bmp.bmp.SetPixel(10, 4, color);
                    bmp.bmp.SetPixel(11, 1, color);
                    bmp.bmp.SetPixel(11, 2, color);
                    bmp.bmp.SetPixel(11, 3, color);
                    bmp.bmp.SetPixel(11, 4, color);
                    bmp.bmp.SetPixel(11, 5, color);
                    bmp.bmp.SetPixel(12, 1, color);
                    bmp.bmp.SetPixel(12, 2, color);
                    bmp.bmp.SetPixel(12, 4, color);
                    bmp.bmp.SetPixel(13, 1, color);
                    bmp.bmp.SetPixel(13, 4, color);
                    bmp.bmp.SetPixel(13, 10, color);
                    bmp.bmp.SetPixel(13, 11, color);
                    bmp.bmp.SetPixel(14, 3, color);
                    bmp.bmp.SetPixel(14, 4, color);
                    bmp.bmp.SetPixel(14, 5, color);
                    bmp.bmp.SetPixel(15, 4, color);
                    bmp.bmp.SetPixel(15, 5, color);
                    bmp.bmp.SetPixel(15, 6, color);
                    bmp.bmp.SetPixel(16, 6, color);
                    bmp.bmp.SetPixel(16, 7, color);
                    bmp.bmp.SetPixel(16, 8, color);
                    bmp.bmp.SetPixel(16, 9, color);
                    bmp.bmp.SetPixel(19, 17, color);
                    bmp.bmp.SetPixel(19, 18, color);
                    bmp.bmp.SetPixel(19, 19, color);
                    bmp.bmp.SetPixel(19, 20, color);
                    bmp.bmp.SetPixel(20, 18, color);
                    bmp.bmp.SetPixel(20, 19, color);

                    Color tmpColor = Color.FromArgb((color.R - 48 >= 0) ? color.R - 48 : 0, (color.G - 48 >= 0) ? color.G - 48 : 0, (color.B - 32 >= 0) ? color.B - 32 : 0);
                    bmp.bmp.SetPixel(3, 16, tmpColor);
                    bmp.bmp.SetPixel(4, 19, tmpColor);
                    bmp.bmp.SetPixel(4, 20, tmpColor);
                    bmp.bmp.SetPixel(4, 21, tmpColor);
                    bmp.bmp.SetPixel(6, 5, tmpColor);
                    bmp.bmp.SetPixel(6, 10, tmpColor);
                    bmp.bmp.SetPixel(7, 3, tmpColor);
                    bmp.bmp.SetPixel(7, 7, tmpColor);
                    bmp.bmp.SetPixel(7, 8, tmpColor);
                    bmp.bmp.SetPixel(7, 26, tmpColor);
                    bmp.bmp.SetPixel(7, 28, tmpColor);
                    bmp.bmp.SetPixel(8, 6, tmpColor);
                    bmp.bmp.SetPixel(8, 26, tmpColor);
                    bmp.bmp.SetPixel(9, 3, tmpColor);
                    bmp.bmp.SetPixel(9, 5, tmpColor);
                    bmp.bmp.SetPixel(9, 26, tmpColor);
                    bmp.bmp.SetPixel(10, 5, tmpColor);
                    bmp.bmp.SetPixel(10, 28, tmpColor);
                    bmp.bmp.SetPixel(11, 6, tmpColor);
                    bmp.bmp.SetPixel(11, 24, tmpColor);
                    bmp.bmp.SetPixel(12, 5, tmpColor);
                    bmp.bmp.SetPixel(12, 26, tmpColor);
                    bmp.bmp.SetPixel(13, 3, tmpColor);
                    bmp.bmp.SetPixel(13, 5, tmpColor);
                    bmp.bmp.SetPixel(13, 25, tmpColor);
                    bmp.bmp.SetPixel(13, 27, tmpColor);
                    bmp.bmp.SetPixel(14, 6, tmpColor);
                    bmp.bmp.SetPixel(14, 23, tmpColor);
                    bmp.bmp.SetPixel(14, 25, tmpColor);
                    bmp.bmp.SetPixel(15, 3, tmpColor);
                    bmp.bmp.SetPixel(15, 7, tmpColor);
                    bmp.bmp.SetPixel(15, 8, tmpColor);
                    bmp.bmp.SetPixel(15, 26, tmpColor);
                    bmp.bmp.SetPixel(16, 5, tmpColor);
                    bmp.bmp.SetPixel(16, 10, tmpColor);
                    bmp.bmp.SetPixel(18, 19, tmpColor);
                    bmp.bmp.SetPixel(18, 20, tmpColor);
                    bmp.bmp.SetPixel(18, 21, tmpColor);
                    bmp.bmp.SetPixel(19, 16, tmpColor);

                    tmpColor = Color.FromArgb((color.R - 112 >= 0) ? color.R - 112 : 0, (color.G - 104 >= 0) ? color.G - 104 : 0, (color.B - 96 >= 0) ? color.B - 96 : 0);
                    bmp.bmp.SetPixel(4, 18, tmpColor);
                    bmp.bmp.SetPixel(5, 5, tmpColor);
                    bmp.bmp.SetPixel(5, 6, tmpColor);
                    bmp.bmp.SetPixel(5, 20, tmpColor);
                    bmp.bmp.SetPixel(5, 21, tmpColor);
                    bmp.bmp.SetPixel(6, 3, tmpColor);
                    bmp.bmp.SetPixel(6, 4, tmpColor);
                    bmp.bmp.SetPixel(7, 2, tmpColor);
                    bmp.bmp.SetPixel(7, 9, tmpColor);
                    bmp.bmp.SetPixel(7, 11, tmpColor);
                    bmp.bmp.SetPixel(7, 27, tmpColor);
                    bmp.bmp.SetPixel(8, 1, tmpColor);
                    bmp.bmp.SetPixel(8, 2, tmpColor);
                    bmp.bmp.SetPixel(8, 7, tmpColor);
                    bmp.bmp.SetPixel(8, 9, tmpColor);
                    bmp.bmp.SetPixel(8, 27, tmpColor);
                    bmp.bmp.SetPixel(9, 0, tmpColor);
                    bmp.bmp.SetPixel(9, 2, tmpColor);
                    bmp.bmp.SetPixel(9, 6, tmpColor);
                    bmp.bmp.SetPixel(9, 9, tmpColor);
                    bmp.bmp.SetPixel(9, 27, tmpColor);
                    bmp.bmp.SetPixel(10, 0, tmpColor);
                    bmp.bmp.SetPixel(10, 3, tmpColor);
                    bmp.bmp.SetPixel(10, 6, tmpColor);
                    bmp.bmp.SetPixel(10, 10, tmpColor);
                    bmp.bmp.SetPixel(10, 26, tmpColor);
                    bmp.bmp.SetPixel(11, 0, tmpColor);
                    bmp.bmp.SetPixel(11, 7, tmpColor);
                    bmp.bmp.SetPixel(11, 11, tmpColor);
                    bmp.bmp.SetPixel(11, 26, tmpColor);
                    bmp.bmp.SetPixel(12, 0, tmpColor);
                    bmp.bmp.SetPixel(12, 3, tmpColor);
                    bmp.bmp.SetPixel(12, 6, tmpColor);
                    bmp.bmp.SetPixel(12, 10, tmpColor);
                    bmp.bmp.SetPixel(13, 0, tmpColor);
                    bmp.bmp.SetPixel(13, 2, tmpColor);
                    bmp.bmp.SetPixel(13, 6, tmpColor);
                    bmp.bmp.SetPixel(13, 9, tmpColor);
                    bmp.bmp.SetPixel(14, 1, tmpColor);
                    bmp.bmp.SetPixel(14, 2, tmpColor);
                    bmp.bmp.SetPixel(14, 7, tmpColor);
                    bmp.bmp.SetPixel(14, 9, tmpColor);
                    bmp.bmp.SetPixel(15, 2, tmpColor);
                    bmp.bmp.SetPixel(15, 9, tmpColor);
                    bmp.bmp.SetPixel(15, 11, tmpColor);
                    bmp.bmp.SetPixel(16, 3, tmpColor);
                    bmp.bmp.SetPixel(16, 4, tmpColor);
                    bmp.bmp.SetPixel(17, 5, tmpColor);
                    bmp.bmp.SetPixel(17, 6, tmpColor);
                    bmp.bmp.SetPixel(17, 20, tmpColor);
                    bmp.bmp.SetPixel(17, 21, tmpColor);
                    bmp.bmp.SetPixel(18, 18, tmpColor);
                    #endregion
                }
                else if (partie == 2)
                {
                    #region
                    bmp.bmp.SetPixel(5, 15, color);
                    bmp.bmp.SetPixel(6, 16, color);
                    bmp.bmp.SetPixel(6, 17, color);
                    bmp.bmp.SetPixel(6, 18, color);
                    bmp.bmp.SetPixel(6, 30, color);
                    bmp.bmp.SetPixel(6, 31, color);
                    bmp.bmp.SetPixel(7, 14, color);
                    bmp.bmp.SetPixel(7, 17, color);
                    bmp.bmp.SetPixel(7, 18, color);
                    bmp.bmp.SetPixel(7, 19, color);
                    bmp.bmp.SetPixel(7, 20, color);
                    bmp.bmp.SetPixel(7, 21, color);
                    bmp.bmp.SetPixel(7, 22, color);
                    bmp.bmp.SetPixel(7, 30, color);
                    bmp.bmp.SetPixel(7, 31, color);
                    bmp.bmp.SetPixel(8, 15, color);
                    bmp.bmp.SetPixel(8, 18, color);
                    bmp.bmp.SetPixel(8, 19, color);
                    bmp.bmp.SetPixel(8, 20, color);
                    bmp.bmp.SetPixel(8, 21, color);
                    bmp.bmp.SetPixel(8, 22, color);
                    bmp.bmp.SetPixel(8, 23, color);
                    bmp.bmp.SetPixel(8, 30, color);
                    bmp.bmp.SetPixel(8, 31, color);
                    bmp.bmp.SetPixel(9, 16, color);
                    bmp.bmp.SetPixel(9, 18, color);
                    bmp.bmp.SetPixel(9, 19, color);
                    bmp.bmp.SetPixel(9, 20, color);
                    bmp.bmp.SetPixel(9, 21, color);
                    bmp.bmp.SetPixel(9, 22, color);
                    bmp.bmp.SetPixel(9, 23, color);
                    bmp.bmp.SetPixel(9, 30, color);
                    bmp.bmp.SetPixel(10, 16, color);
                    bmp.bmp.SetPixel(10, 17, color);
                    bmp.bmp.SetPixel(10, 18, color);
                    bmp.bmp.SetPixel(10, 19, color);
                    bmp.bmp.SetPixel(10, 20, color);
                    bmp.bmp.SetPixel(10, 21, color);
                    bmp.bmp.SetPixel(10, 22, color);
                    bmp.bmp.SetPixel(10, 30, color);
                    bmp.bmp.SetPixel(11, 16, color);
                    bmp.bmp.SetPixel(11, 17, color);
                    bmp.bmp.SetPixel(11, 18, color);
                    bmp.bmp.SetPixel(11, 19, color);
                    bmp.bmp.SetPixel(11, 20, color);
                    bmp.bmp.SetPixel(11, 21, color);
                    bmp.bmp.SetPixel(11, 22, color);
                    bmp.bmp.SetPixel(11, 30, color);
                    bmp.bmp.SetPixel(12, 16, color);
                    bmp.bmp.SetPixel(12, 17, color);
                    bmp.bmp.SetPixel(12, 18, color);
                    bmp.bmp.SetPixel(12, 19, color);
                    bmp.bmp.SetPixel(12, 20, color);
                    bmp.bmp.SetPixel(12, 21, color);
                    bmp.bmp.SetPixel(12, 22, color);
                    bmp.bmp.SetPixel(12, 30, color);
                    bmp.bmp.SetPixel(13, 16, color);
                    bmp.bmp.SetPixel(13, 18, color);
                    bmp.bmp.SetPixel(13, 19, color);
                    bmp.bmp.SetPixel(13, 20, color);
                    bmp.bmp.SetPixel(13, 21, color);
                    bmp.bmp.SetPixel(13, 29, color);
                    bmp.bmp.SetPixel(13, 30, color);
                    bmp.bmp.SetPixel(14, 15, color);
                    bmp.bmp.SetPixel(14, 18, color);
                    bmp.bmp.SetPixel(14, 19, color);
                    bmp.bmp.SetPixel(14, 20, color);
                    bmp.bmp.SetPixel(14, 21, color);
                    bmp.bmp.SetPixel(14, 29, color);
                    bmp.bmp.SetPixel(14, 30, color);
                    bmp.bmp.SetPixel(14, 31, color);
                    bmp.bmp.SetPixel(14, 32, color);
                    bmp.bmp.SetPixel(14, 33, color);
                    bmp.bmp.SetPixel(14, 34, color);
                    bmp.bmp.SetPixel(14, 35, color);
                    bmp.bmp.SetPixel(14, 36, color);
                    bmp.bmp.SetPixel(15, 14, color);
                    bmp.bmp.SetPixel(15, 17, color);
                    bmp.bmp.SetPixel(15, 18, color);
                    bmp.bmp.SetPixel(15, 19, color);
                    bmp.bmp.SetPixel(15, 20, color);
                    bmp.bmp.SetPixel(15, 28, color);
                    bmp.bmp.SetPixel(15, 29, color);
                    bmp.bmp.SetPixel(15, 30, color);
                    bmp.bmp.SetPixel(15, 31, color);
                    bmp.bmp.SetPixel(15, 32, color);
                    bmp.bmp.SetPixel(15, 33, color);
                    bmp.bmp.SetPixel(15, 34, color);
                    bmp.bmp.SetPixel(15, 35, color);
                    bmp.bmp.SetPixel(15, 36, color);
                    bmp.bmp.SetPixel(16, 16, color);
                    bmp.bmp.SetPixel(16, 17, color);
                    bmp.bmp.SetPixel(16, 18, color);
                    bmp.bmp.SetPixel(16, 28, color);
                    bmp.bmp.SetPixel(16, 29, color);
                    bmp.bmp.SetPixel(16, 30, color);
                    bmp.bmp.SetPixel(16, 31, color);
                    bmp.bmp.SetPixel(16, 32, color);
                    bmp.bmp.SetPixel(16, 33, color);
                    bmp.bmp.SetPixel(16, 34, color);
                    bmp.bmp.SetPixel(16, 35, color);
                    bmp.bmp.SetPixel(17, 15, color);
                    bmp.bmp.SetPixel(17, 16, color);

                    Color tmpColor = Color.FromArgb((color.R - 40 >= 0) ? color.R - 40 : 0, (color.G - 8 >= 0) ? color.G - 8 : 0, (color.B - 24 >= 0) ? color.B - 24 : 0);
                    bmp.bmp.SetPixel(4, 14, tmpColor);
                    bmp.bmp.SetPixel(4, 15, tmpColor);
                    bmp.bmp.SetPixel(5, 7, tmpColor);
                    bmp.bmp.SetPixel(5, 8, tmpColor);
                    bmp.bmp.SetPixel(5, 9, tmpColor);
                    bmp.bmp.SetPixel(5, 10, tmpColor);
                    bmp.bmp.SetPixel(5, 14, tmpColor);
                    bmp.bmp.SetPixel(5, 16, tmpColor);
                    bmp.bmp.SetPixel(5, 17, tmpColor);
                    bmp.bmp.SetPixel(6, 11, tmpColor);
                    bmp.bmp.SetPixel(6, 19, tmpColor);
                    bmp.bmp.SetPixel(6, 29, tmpColor);
                    bmp.bmp.SetPixel(6, 32, tmpColor);
                    bmp.bmp.SetPixel(6, 36, tmpColor);
                    bmp.bmp.SetPixel(6, 37, tmpColor);
                    bmp.bmp.SetPixel(6, 38, tmpColor);
                    bmp.bmp.SetPixel(6, 39, tmpColor);
                    bmp.bmp.SetPixel(7, 10, tmpColor);
                    bmp.bmp.SetPixel(7, 13, tmpColor);
                    bmp.bmp.SetPixel(7, 15, tmpColor);
                    bmp.bmp.SetPixel(7, 23, tmpColor);
                    bmp.bmp.SetPixel(7, 32, tmpColor);
                    bmp.bmp.SetPixel(7, 36, tmpColor);
                    bmp.bmp.SetPixel(7, 37, tmpColor);
                    bmp.bmp.SetPixel(7, 38, tmpColor);
                    bmp.bmp.SetPixel(7, 39, tmpColor);
                    bmp.bmp.SetPixel(7, 40, tmpColor);
                    bmp.bmp.SetPixel(8, 8, tmpColor);
                    bmp.bmp.SetPixel(8, 12, tmpColor);
                    bmp.bmp.SetPixel(8, 14, tmpColor);
                    bmp.bmp.SetPixel(8, 16, tmpColor);
                    bmp.bmp.SetPixel(8, 17, tmpColor);
                    bmp.bmp.SetPixel(8, 32, tmpColor);
                    bmp.bmp.SetPixel(8, 36, tmpColor);
                    bmp.bmp.SetPixel(8, 37, tmpColor);
                    bmp.bmp.SetPixel(8, 38, tmpColor);
                    bmp.bmp.SetPixel(8, 39, tmpColor);
                    bmp.bmp.SetPixel(8, 40, tmpColor);
                    bmp.bmp.SetPixel(9, 12, tmpColor);
                    bmp.bmp.SetPixel(9, 15, tmpColor);
                    bmp.bmp.SetPixel(9, 17, tmpColor);
                    bmp.bmp.SetPixel(9, 31, tmpColor);
                    bmp.bmp.SetPixel(9, 32, tmpColor);
                    bmp.bmp.SetPixel(9, 36, tmpColor);
                    bmp.bmp.SetPixel(9, 37, tmpColor);
                    bmp.bmp.SetPixel(9, 38, tmpColor);
                    bmp.bmp.SetPixel(9, 39, tmpColor);
                    bmp.bmp.SetPixel(10, 11, tmpColor);
                    bmp.bmp.SetPixel(10, 23, tmpColor);
                    bmp.bmp.SetPixel(11, 29, tmpColor);
                    bmp.bmp.SetPixel(12, 11, tmpColor);
                    bmp.bmp.SetPixel(12, 29, tmpColor);
                    bmp.bmp.SetPixel(13, 12, tmpColor);
                    bmp.bmp.SetPixel(13, 15, tmpColor);
                    bmp.bmp.SetPixel(13, 17, tmpColor);
                    bmp.bmp.SetPixel(13, 22, tmpColor);
                    bmp.bmp.SetPixel(13, 31, tmpColor);
                    bmp.bmp.SetPixel(13, 32, tmpColor);
                    bmp.bmp.SetPixel(13, 33, tmpColor);
                    bmp.bmp.SetPixel(13, 34, tmpColor);
                    bmp.bmp.SetPixel(13, 35, tmpColor);
                    bmp.bmp.SetPixel(13, 36, tmpColor);
                    bmp.bmp.SetPixel(13, 37, tmpColor);
                    bmp.bmp.SetPixel(13, 38, tmpColor);
                    bmp.bmp.SetPixel(13, 39, tmpColor);
                    bmp.bmp.SetPixel(14, 8, tmpColor);
                    bmp.bmp.SetPixel(14, 12, tmpColor);
                    bmp.bmp.SetPixel(14, 14, tmpColor);
                    bmp.bmp.SetPixel(14, 16, tmpColor);
                    bmp.bmp.SetPixel(14, 17, tmpColor);
                    bmp.bmp.SetPixel(14, 28, tmpColor);
                    bmp.bmp.SetPixel(14, 37, tmpColor);
                    bmp.bmp.SetPixel(14, 38, tmpColor);
                    bmp.bmp.SetPixel(14, 39, tmpColor);
                    bmp.bmp.SetPixel(14, 40, tmpColor);
                    bmp.bmp.SetPixel(15, 10, tmpColor);
                    bmp.bmp.SetPixel(15, 13, tmpColor);
                    bmp.bmp.SetPixel(15, 15, tmpColor);
                    bmp.bmp.SetPixel(15, 21, tmpColor);
                    bmp.bmp.SetPixel(15, 37, tmpColor);
                    bmp.bmp.SetPixel(15, 38, tmpColor);
                    bmp.bmp.SetPixel(15, 39, tmpColor);
                    bmp.bmp.SetPixel(15, 40, tmpColor);
                    bmp.bmp.SetPixel(16, 11, tmpColor);
                    bmp.bmp.SetPixel(16, 19, tmpColor);
                    bmp.bmp.SetPixel(16, 27, tmpColor);
                    bmp.bmp.SetPixel(16, 36, tmpColor);
                    bmp.bmp.SetPixel(16, 37, tmpColor);
                    bmp.bmp.SetPixel(16, 38, tmpColor);
                    bmp.bmp.SetPixel(16, 39, tmpColor);
                    bmp.bmp.SetPixel(17, 7, tmpColor);
                    bmp.bmp.SetPixel(17, 8, tmpColor);
                    bmp.bmp.SetPixel(17, 9, tmpColor);
                    bmp.bmp.SetPixel(17, 10, tmpColor);
                    bmp.bmp.SetPixel(17, 14, tmpColor);
                    bmp.bmp.SetPixel(17, 17, tmpColor);
                    bmp.bmp.SetPixel(18, 14, tmpColor);
                    bmp.bmp.SetPixel(18, 15, tmpColor);

                    tmpColor = Color.FromArgb((color.R - 32 >= 0) ? color.R - 32 : 0, (color.G - 8 >= 0) ? color.G - 8 : 0, (color.B - 72 >= 0) ? color.B - 72 : 0);
                    bmp.bmp.SetPixel(6, 15, tmpColor);
                    bmp.bmp.SetPixel(6, 40, tmpColor);
                    bmp.bmp.SetPixel(7, 16, tmpColor);
                    bmp.bmp.SetPixel(9, 40, tmpColor);
                    bmp.bmp.SetPixel(10, 31, tmpColor);
                    bmp.bmp.SetPixel(12, 31, tmpColor);
                    bmp.bmp.SetPixel(13, 40, tmpColor);
                    bmp.bmp.SetPixel(15, 16, tmpColor);
                    bmp.bmp.SetPixel(16, 15, tmpColor);
                    bmp.bmp.SetPixel(16, 40, tmpColor);
                    #endregion
                }
                else if (partie == 3)
                {
                    #region
                    bmp.bmp.SetPixel(2, 25, color);
                    bmp.bmp.SetPixel(2, 26, color);
                    bmp.bmp.SetPixel(2, 27, color);
                    bmp.bmp.SetPixel(2, 28, color);
                    bmp.bmp.SetPixel(2, 29, color);
                    bmp.bmp.SetPixel(3, 24, color);
                    bmp.bmp.SetPixel(3, 25, color);
                    bmp.bmp.SetPixel(3, 28, color);
                    bmp.bmp.SetPixel(7, 43, color);
                    bmp.bmp.SetPixel(8, 43, color);
                    bmp.bmp.SetPixel(9, 13, color);
                    bmp.bmp.SetPixel(10, 8, color);
                    bmp.bmp.SetPixel(10, 13, color);
                    bmp.bmp.SetPixel(11, 8, color);
                    bmp.bmp.SetPixel(11, 9, color);
                    bmp.bmp.SetPixel(11, 13, color);
                    bmp.bmp.SetPixel(11, 14, color);
                    bmp.bmp.SetPixel(12, 8, color);
                    bmp.bmp.SetPixel(12, 13, color);
                    bmp.bmp.SetPixel(13, 13, color);
                    bmp.bmp.SetPixel(14, 43, color);
                    bmp.bmp.SetPixel(15, 43, color);
                    bmp.bmp.SetPixel(19, 24, color);
                    bmp.bmp.SetPixel(19, 25, color);
                    bmp.bmp.SetPixel(19, 28, color);
                    bmp.bmp.SetPixel(20, 25, color);
                    bmp.bmp.SetPixel(20, 26, color);
                    bmp.bmp.SetPixel(20, 27, color);
                    bmp.bmp.SetPixel(20, 28, color);
                    bmp.bmp.SetPixel(20, 29, color);

                    Color tmpColor = Color.FromArgb((color.R - 16 >= 0) ? color.R - 16 : 0, (color.G - 48 >= 0) ? color.G - 48 : 0, (color.B - 40 >= 0) ? color.B - 40 : 0);
                    bmp.bmp.SetPixel(1, 28, tmpColor);
                    bmp.bmp.SetPixel(1, 29, tmpColor);
                    bmp.bmp.SetPixel(1, 30, tmpColor);
                    bmp.bmp.SetPixel(2, 30, tmpColor);
                    bmp.bmp.SetPixel(2, 31, tmpColor);
                    bmp.bmp.SetPixel(3, 23, tmpColor);
                    bmp.bmp.SetPixel(3, 26, tmpColor);
                    bmp.bmp.SetPixel(3, 27, tmpColor);
                    bmp.bmp.SetPixel(3, 31, tmpColor);
                    bmp.bmp.SetPixel(4, 23, tmpColor);
                    bmp.bmp.SetPixel(4, 24, tmpColor);
                    bmp.bmp.SetPixel(4, 28, tmpColor);
                    bmp.bmp.SetPixel(4, 29, tmpColor);
                    bmp.bmp.SetPixel(5, 51, tmpColor);
                    bmp.bmp.SetPixel(6, 51, tmpColor);
                    bmp.bmp.SetPixel(7, 42, tmpColor);
                    bmp.bmp.SetPixel(8, 42, tmpColor);
                    bmp.bmp.SetPixel(8, 51, tmpColor);
                    bmp.bmp.SetPixel(9, 8, tmpColor);
                    bmp.bmp.SetPixel(10, 7, tmpColor);
                    bmp.bmp.SetPixel(10, 9, tmpColor);
                    bmp.bmp.SetPixel(10, 14, tmpColor);
                    bmp.bmp.SetPixel(11, 10, tmpColor);
                    bmp.bmp.SetPixel(11, 12, tmpColor);
                    bmp.bmp.SetPixel(12, 7, tmpColor);
                    bmp.bmp.SetPixel(12, 9, tmpColor);
                    bmp.bmp.SetPixel(12, 14, tmpColor);
                    bmp.bmp.SetPixel(13, 8, tmpColor);
                    bmp.bmp.SetPixel(14, 42, tmpColor);
                    bmp.bmp.SetPixel(14, 51, tmpColor);
                    bmp.bmp.SetPixel(15, 42, tmpColor);
                    bmp.bmp.SetPixel(16, 51, tmpColor);
                    bmp.bmp.SetPixel(17, 51, tmpColor);
                    bmp.bmp.SetPixel(18, 23, tmpColor);
                    bmp.bmp.SetPixel(18, 24, tmpColor);
                    bmp.bmp.SetPixel(18, 28, tmpColor);
                    bmp.bmp.SetPixel(18, 29, tmpColor);
                    bmp.bmp.SetPixel(19, 23, tmpColor);
                    bmp.bmp.SetPixel(19, 26, tmpColor);
                    bmp.bmp.SetPixel(19, 27, tmpColor);
                    bmp.bmp.SetPixel(19, 31, tmpColor);
                    bmp.bmp.SetPixel(20, 30, tmpColor);
                    bmp.bmp.SetPixel(20, 31, tmpColor);
                    bmp.bmp.SetPixel(21, 28, tmpColor);
                    bmp.bmp.SetPixel(21, 29, tmpColor);
                    bmp.bmp.SetPixel(21, 30, tmpColor);

                    tmpColor = Color.FromArgb((color.R - 88 >= 0) ? color.R - 88 : 0, (color.G - 104 >= 0) ? color.G - 104 : 0, (color.B - 64 >= 0) ? color.B - 64 : 0);
                    bmp.bmp.SetPixel(3, 22, tmpColor);
                    bmp.bmp.SetPixel(9, 7, tmpColor);
                    bmp.bmp.SetPixel(10, 12, tmpColor);
                    bmp.bmp.SetPixel(12, 12, tmpColor);
                    bmp.bmp.SetPixel(13, 7, tmpColor);
                    bmp.bmp.SetPixel(19, 22, tmpColor);

                    tmpColor = Color.FromArgb((color.R - 168 >= 0) ? color.R - 168 : 0, (color.G - 52 >= 0) ? color.G - 52 : 0, (color.B - 88 >= 0) ? color.B - 88 : 0);
                    bmp.bmp.SetPixel(0, 28, tmpColor);
                    bmp.bmp.SetPixel(0, 29, tmpColor);
                    bmp.bmp.SetPixel(0, 30, tmpColor);
                    bmp.bmp.SetPixel(1, 25, tmpColor);
                    bmp.bmp.SetPixel(1, 26, tmpColor);
                    bmp.bmp.SetPixel(1, 27, tmpColor);
                    bmp.bmp.SetPixel(1, 31, tmpColor);
                    bmp.bmp.SetPixel(2, 21, tmpColor);
                    bmp.bmp.SetPixel(2, 22, tmpColor);
                    bmp.bmp.SetPixel(2, 23, tmpColor);
                    bmp.bmp.SetPixel(2, 24, tmpColor);
                    bmp.bmp.SetPixel(2, 32, tmpColor);
                    bmp.bmp.SetPixel(3, 29, tmpColor);
                    bmp.bmp.SetPixel(3, 30, tmpColor);
                    bmp.bmp.SetPixel(3, 32, tmpColor);
                    bmp.bmp.SetPixel(4, 25, tmpColor);
                    bmp.bmp.SetPixel(4, 26, tmpColor);
                    bmp.bmp.SetPixel(4, 27, tmpColor);
                    bmp.bmp.SetPixel(4, 30, tmpColor);
                    bmp.bmp.SetPixel(4, 31, tmpColor);
                    bmp.bmp.SetPixel(5, 23, tmpColor);
                    bmp.bmp.SetPixel(5, 24, tmpColor);
                    bmp.bmp.SetPixel(6, 42, tmpColor);
                    bmp.bmp.SetPixel(6, 43, tmpColor);
                    bmp.bmp.SetPixel(7, 51, tmpColor);
                    bmp.bmp.SetPixel(9, 42, tmpColor);
                    bmp.bmp.SetPixel(9, 43, tmpColor);
                    bmp.bmp.SetPixel(13, 42, tmpColor);
                    bmp.bmp.SetPixel(13, 43, tmpColor);
                    bmp.bmp.SetPixel(15, 51, tmpColor);
                    bmp.bmp.SetPixel(16, 42, tmpColor);
                    bmp.bmp.SetPixel(16, 43, tmpColor);
                    bmp.bmp.SetPixel(17, 23, tmpColor);
                    bmp.bmp.SetPixel(17, 24, tmpColor);
                    bmp.bmp.SetPixel(18, 25, tmpColor);
                    bmp.bmp.SetPixel(18, 26, tmpColor);
                    bmp.bmp.SetPixel(18, 27, tmpColor);
                    bmp.bmp.SetPixel(18, 30, tmpColor);
                    bmp.bmp.SetPixel(18, 31, tmpColor);
                    bmp.bmp.SetPixel(19, 29, tmpColor);
                    bmp.bmp.SetPixel(19, 30, tmpColor);
                    bmp.bmp.SetPixel(19, 32, tmpColor);
                    bmp.bmp.SetPixel(20, 21, tmpColor);
                    bmp.bmp.SetPixel(20, 22, tmpColor);
                    bmp.bmp.SetPixel(20, 23, tmpColor);
                    bmp.bmp.SetPixel(20, 24, tmpColor);
                    bmp.bmp.SetPixel(20, 32, tmpColor);
                    bmp.bmp.SetPixel(21, 25, tmpColor);
                    bmp.bmp.SetPixel(21, 26, tmpColor);
                    bmp.bmp.SetPixel(21, 27, tmpColor);
                    bmp.bmp.SetPixel(21, 31, tmpColor);
                    bmp.bmp.SetPixel(22, 28, tmpColor);
                    bmp.bmp.SetPixel(22, 29, tmpColor);
                    bmp.bmp.SetPixel(22, 30, tmpColor);
                    #endregion
                }
                #endregion
            }
            else if (className == Enums.ActorClass.ClassName.ino)
            {
                #region
                if (partie == 1)
                {
                    #region
                    bmp.bmp.SetPixel(8, 8, color);
                    bmp.bmp.SetPixel(8, 9, color);
                    bmp.bmp.SetPixel(8, 10, color);
                    bmp.bmp.SetPixel(8, 11, color);
                    bmp.bmp.SetPixel(9, 5, color);
                    bmp.bmp.SetPixel(9, 6, color);
                    bmp.bmp.SetPixel(9, 7, color);
                    bmp.bmp.SetPixel(9, 8, color);
                    bmp.bmp.SetPixel(9, 9, color);
                    bmp.bmp.SetPixel(9, 10, color);
                    bmp.bmp.SetPixel(9, 11, color);
                    bmp.bmp.SetPixel(9, 12, color);
                    bmp.bmp.SetPixel(9, 13, color);
                    bmp.bmp.SetPixel(10, 4, color);
                    bmp.bmp.SetPixel(10, 5, color);
                    bmp.bmp.SetPixel(10, 6, color);
                    bmp.bmp.SetPixel(10, 7, color);
                    bmp.bmp.SetPixel(11, 1, color);
                    bmp.bmp.SetPixel(11, 3, color);
                    bmp.bmp.SetPixel(11, 4, color);
                    bmp.bmp.SetPixel(11, 5, color);
                    bmp.bmp.SetPixel(11, 6, color);
                    bmp.bmp.SetPixel(11, 7, color);
                    bmp.bmp.SetPixel(11, 8, color);
                    bmp.bmp.SetPixel(12, 1, color);
                    bmp.bmp.SetPixel(12, 3, color);
                    bmp.bmp.SetPixel(12, 4, color);
                    bmp.bmp.SetPixel(12, 5, color);
                    bmp.bmp.SetPixel(12, 6, color);
                    bmp.bmp.SetPixel(13, 1, color);
                    bmp.bmp.SetPixel(13, 3, color);
                    bmp.bmp.SetPixel(13, 4, color);
                    bmp.bmp.SetPixel(13, 5, color);
                    bmp.bmp.SetPixel(13, 6, color);
                    bmp.bmp.SetPixel(14, 4, color);
                    bmp.bmp.SetPixel(14, 5, color);
                    bmp.bmp.SetPixel(14, 6, color);
                    bmp.bmp.SetPixel(15, 5, color);
                    bmp.bmp.SetPixel(15, 6, color);
                    bmp.bmp.SetPixel(16, 7, color);

                    Color tmpColor = Color.FromArgb((color.R - 40 >= 0) ? color.R - 40 : 0, (color.G - 72 >= 0) ? color.G - 72 : 0, (color.B - 80 >= 0) ? color.B - 80 : 0);
                    bmp.bmp.SetPixel(8, 6, tmpColor);
                    bmp.bmp.SetPixel(8, 7, tmpColor);
                    bmp.bmp.SetPixel(8, 12, tmpColor);
                    bmp.bmp.SetPixel(8, 13, tmpColor);
                    bmp.bmp.SetPixel(9, 14, tmpColor);
                    bmp.bmp.SetPixel(9, 15, tmpColor);
                    bmp.bmp.SetPixel(10, 2, tmpColor);
                    bmp.bmp.SetPixel(10, 8, tmpColor);
                    bmp.bmp.SetPixel(10, 9, tmpColor);
                    bmp.bmp.SetPixel(10, 10, tmpColor);
                    bmp.bmp.SetPixel(10, 11, tmpColor);
                    bmp.bmp.SetPixel(10, 12, tmpColor);
                    bmp.bmp.SetPixel(11, 9, tmpColor);
                    bmp.bmp.SetPixel(12, 7, tmpColor);
                    bmp.bmp.SetPixel(14, 2, tmpColor);
                    bmp.bmp.SetPixel(15, 7, tmpColor);
                    bmp.bmp.SetPixel(16, 6, tmpColor);
                    bmp.bmp.SetPixel(16, 8, tmpColor);
                    #endregion
                }
                else if (partie == 2)
                {
                    #region
                    bmp.bmp.SetPixel(7, 27, color);
                    bmp.bmp.SetPixel(7, 28, color);
                    bmp.bmp.SetPixel(7, 29, color);
                    bmp.bmp.SetPixel(7, 30, color);
                    bmp.bmp.SetPixel(7, 31, color);
                    bmp.bmp.SetPixel(8, 18, color);
                    bmp.bmp.SetPixel(8, 19, color);
                    bmp.bmp.SetPixel(8, 25, color);
                    bmp.bmp.SetPixel(8, 26, color);
                    bmp.bmp.SetPixel(8, 28, color);
                    bmp.bmp.SetPixel(8, 29, color);
                    bmp.bmp.SetPixel(8, 30, color);
                    bmp.bmp.SetPixel(8, 31, color);
                    bmp.bmp.SetPixel(8, 32, color);
                    bmp.bmp.SetPixel(9, 17, color);
                    bmp.bmp.SetPixel(9, 18, color);
                    bmp.bmp.SetPixel(9, 19, color);
                    bmp.bmp.SetPixel(9, 20, color);
                    bmp.bmp.SetPixel(9, 26, color);
                    bmp.bmp.SetPixel(9, 27, color);
                    bmp.bmp.SetPixel(9, 29, color);
                    bmp.bmp.SetPixel(9, 30, color);
                    bmp.bmp.SetPixel(9, 31, color);
                    bmp.bmp.SetPixel(9, 32, color);
                    bmp.bmp.SetPixel(9, 33, color);
                    bmp.bmp.SetPixel(10, 18, color);
                    bmp.bmp.SetPixel(10, 19, color);
                    bmp.bmp.SetPixel(10, 20, color);
                    bmp.bmp.SetPixel(10, 27, color);
                    bmp.bmp.SetPixel(10, 29, color);
                    bmp.bmp.SetPixel(10, 30, color);
                    bmp.bmp.SetPixel(10, 31, color);
                    bmp.bmp.SetPixel(10, 32, color);
                    bmp.bmp.SetPixel(10, 33, color);
                    bmp.bmp.SetPixel(11, 17, color);
                    bmp.bmp.SetPixel(11, 18, color);
                    bmp.bmp.SetPixel(11, 19, color);
                    bmp.bmp.SetPixel(11, 20, color);
                    bmp.bmp.SetPixel(11, 27, color);
                    bmp.bmp.SetPixel(13, 17, color);
                    bmp.bmp.SetPixel(13, 18, color);
                    bmp.bmp.SetPixel(13, 19, color);
                    bmp.bmp.SetPixel(13, 20, color);
                    bmp.bmp.SetPixel(13, 27, color);
                    bmp.bmp.SetPixel(14, 17, color);
                    bmp.bmp.SetPixel(14, 18, color);
                    bmp.bmp.SetPixel(14, 19, color);
                    bmp.bmp.SetPixel(14, 20, color);
                    bmp.bmp.SetPixel(14, 27, color);
                    bmp.bmp.SetPixel(14, 29, color);
                    bmp.bmp.SetPixel(14, 30, color);
                    bmp.bmp.SetPixel(14, 31, color);
                    bmp.bmp.SetPixel(14, 32, color);
                    bmp.bmp.SetPixel(14, 33, color);
                    bmp.bmp.SetPixel(15, 16, color);
                    bmp.bmp.SetPixel(15, 17, color);
                    bmp.bmp.SetPixel(15, 18, color);
                    bmp.bmp.SetPixel(15, 19, color);
                    bmp.bmp.SetPixel(15, 20, color);
                    bmp.bmp.SetPixel(15, 26, color);
                    bmp.bmp.SetPixel(15, 27, color);
                    bmp.bmp.SetPixel(15, 29, color);
                    bmp.bmp.SetPixel(15, 30, color);
                    bmp.bmp.SetPixel(15, 31, color);
                    bmp.bmp.SetPixel(15, 32, color);
                    bmp.bmp.SetPixel(15, 33, color);
                    bmp.bmp.SetPixel(16, 18, color);
                    bmp.bmp.SetPixel(16, 19, color);
                    bmp.bmp.SetPixel(16, 25, color);
                    bmp.bmp.SetPixel(16, 26, color);
                    bmp.bmp.SetPixel(16, 28, color);
                    bmp.bmp.SetPixel(16, 29, color);
                    bmp.bmp.SetPixel(16, 30, color);
                    bmp.bmp.SetPixel(16, 31, color);
                    bmp.bmp.SetPixel(16, 32, color);
                    bmp.bmp.SetPixel(17, 27, color);
                    bmp.bmp.SetPixel(17, 28, color);
                    bmp.bmp.SetPixel(17, 29, color);
                    bmp.bmp.SetPixel(17, 30, color);
                    bmp.bmp.SetPixel(17, 31, color);

                    Color tmpColor = Color.FromArgb((color.R - 72 >= 0) ? color.R - 72 : 0, 0, (color.B - 64 >= 0) ? color.B - 64 : 0);
                    bmp.bmp.SetPixel(7, 32, tmpColor);
                    bmp.bmp.SetPixel(8, 20, tmpColor);
                    bmp.bmp.SetPixel(8, 21, tmpColor);
                    bmp.bmp.SetPixel(8, 27, tmpColor);
                    bmp.bmp.SetPixel(8, 33, tmpColor);
                    bmp.bmp.SetPixel(9, 16, tmpColor);
                    bmp.bmp.SetPixel(9, 21, tmpColor);
                    bmp.bmp.SetPixel(9, 28, tmpColor);
                    bmp.bmp.SetPixel(10, 21, tmpColor);
                    bmp.bmp.SetPixel(10, 22, tmpColor);
                    bmp.bmp.SetPixel(10, 28, tmpColor);
                    bmp.bmp.SetPixel(11, 16, tmpColor);
                    bmp.bmp.SetPixel(11, 21, tmpColor);
                    bmp.bmp.SetPixel(11, 22, tmpColor);
                    bmp.bmp.SetPixel(11, 28, tmpColor);
                    bmp.bmp.SetPixel(12, 17, tmpColor);
                    bmp.bmp.SetPixel(12, 18, tmpColor);
                    bmp.bmp.SetPixel(12, 19, tmpColor);
                    bmp.bmp.SetPixel(12, 20, tmpColor);
                    bmp.bmp.SetPixel(12, 27, tmpColor);
                    bmp.bmp.SetPixel(13, 16, tmpColor);
                    bmp.bmp.SetPixel(13, 21, tmpColor);
                    bmp.bmp.SetPixel(13, 22, tmpColor);
                    bmp.bmp.SetPixel(13, 28, tmpColor);
                    bmp.bmp.SetPixel(14, 16, tmpColor);
                    bmp.bmp.SetPixel(14, 21, tmpColor);
                    bmp.bmp.SetPixel(14, 22, tmpColor);
                    bmp.bmp.SetPixel(14, 28, tmpColor);
                    bmp.bmp.SetPixel(15, 21, tmpColor);
                    bmp.bmp.SetPixel(15, 28, tmpColor);
                    bmp.bmp.SetPixel(16, 20, tmpColor);
                    bmp.bmp.SetPixel(16, 21, tmpColor);
                    bmp.bmp.SetPixel(16, 27, tmpColor);
                    bmp.bmp.SetPixel(16, 33, tmpColor);
                    bmp.bmp.SetPixel(17, 32, tmpColor);
                    #endregion
                }
                else if (partie == 3)
                {
                    #region
                    bmp.bmp.SetPixel(3, 27, color);
                    bmp.bmp.SetPixel(3, 28, color);
                    bmp.bmp.SetPixel(4, 25, color);
                    bmp.bmp.SetPixel(4, 26, color);
                    bmp.bmp.SetPixel(4, 27, color);
                    bmp.bmp.SetPixel(5, 27, color);
                    bmp.bmp.SetPixel(5, 28, color);
                    bmp.bmp.SetPixel(6, 17, color);
                    bmp.bmp.SetPixel(6, 18, color);
                    bmp.bmp.SetPixel(6, 50, color);
                    bmp.bmp.SetPixel(7, 16, color);
                    bmp.bmp.SetPixel(8, 36, color);
                    bmp.bmp.SetPixel(8, 50, color);
                    bmp.bmp.SetPixel(9, 36, color);
                    bmp.bmp.SetPixel(9, 42, color);
                    bmp.bmp.SetPixel(9, 43, color);
                    bmp.bmp.SetPixel(10, 24, color);
                    bmp.bmp.SetPixel(10, 25, color);
                    bmp.bmp.SetPixel(10, 36, color);
                    bmp.bmp.SetPixel(11, 24, color);
                    bmp.bmp.SetPixel(11, 25, color);
                    bmp.bmp.SetPixel(12, 11, color);
                    bmp.bmp.SetPixel(12, 12, color);
                    bmp.bmp.SetPixel(12, 13, color);
                    bmp.bmp.SetPixel(12, 14, color);
                    bmp.bmp.SetPixel(12, 24, color);
                    bmp.bmp.SetPixel(13, 8, color);
                    bmp.bmp.SetPixel(13, 9, color);
                    bmp.bmp.SetPixel(13, 10, color);
                    bmp.bmp.SetPixel(13, 11, color);
                    bmp.bmp.SetPixel(13, 12, color);
                    bmp.bmp.SetPixel(13, 13, color);
                    bmp.bmp.SetPixel(13, 14, color);
                    bmp.bmp.SetPixel(13, 24, color);
                    bmp.bmp.SetPixel(13, 25, color);
                    bmp.bmp.SetPixel(14, 9, color);
                    bmp.bmp.SetPixel(14, 13, color);
                    bmp.bmp.SetPixel(14, 24, color);
                    bmp.bmp.SetPixel(14, 25, color);
                    bmp.bmp.SetPixel(14, 36, color);
                    bmp.bmp.SetPixel(15, 36, color);
                    bmp.bmp.SetPixel(15, 42, color);
                    bmp.bmp.SetPixel(15, 43, color);
                    bmp.bmp.SetPixel(16, 36, color);
                    bmp.bmp.SetPixel(16, 50, color);
                    bmp.bmp.SetPixel(17, 10, color);
                    bmp.bmp.SetPixel(17, 16, color);
                    bmp.bmp.SetPixel(18, 17, color);
                    bmp.bmp.SetPixel(18, 18, color);
                    bmp.bmp.SetPixel(18, 50, color);
                    bmp.bmp.SetPixel(19, 27, color);
                    bmp.bmp.SetPixel(19, 28, color);
                    bmp.bmp.SetPixel(20, 25, color);
                    bmp.bmp.SetPixel(20, 26, color);
                    bmp.bmp.SetPixel(20, 27, color);
                    bmp.bmp.SetPixel(21, 27, color);
                    bmp.bmp.SetPixel(21, 28, color);

                    Color tmpColor = Color.FromArgb((color.R - 16 >= 0) ? color.R - 16 : 0, (color.G - 72 >= 0) ? color.G - 72 : 0, (color.B - 88 >= 0) ? color.B - 88 : 0);
                    bmp.bmp.SetPixel(3, 29, tmpColor);
                    bmp.bmp.SetPixel(4, 24, tmpColor);
                    bmp.bmp.SetPixel(4, 30, tmpColor);
                    bmp.bmp.SetPixel(5, 24, tmpColor);
                    bmp.bmp.SetPixel(5, 25, tmpColor);
                    bmp.bmp.SetPixel(5, 29, tmpColor);
                    bmp.bmp.SetPixel(6, 19, tmpColor);
                    bmp.bmp.SetPixel(7, 15, tmpColor);
                    bmp.bmp.SetPixel(7, 17, tmpColor);
                    bmp.bmp.SetPixel(7, 50, tmpColor);
                    bmp.bmp.SetPixel(8, 16, tmpColor);
                    bmp.bmp.SetPixel(8, 35, tmpColor);
                    bmp.bmp.SetPixel(9, 24, tmpColor);
                    bmp.bmp.SetPixel(9, 35, tmpColor);
                    bmp.bmp.SetPixel(10, 35, tmpColor);
                    bmp.bmp.SetPixel(10, 42, tmpColor);
                    bmp.bmp.SetPixel(10, 43, tmpColor);
                    bmp.bmp.SetPixel(11, 13, tmpColor);
                    bmp.bmp.SetPixel(11, 14, tmpColor);
                    bmp.bmp.SetPixel(12, 10, tmpColor);
                    bmp.bmp.SetPixel(12, 25, tmpColor);
                    bmp.bmp.SetPixel(14, 8, tmpColor);
                    bmp.bmp.SetPixel(14, 10, tmpColor);
                    bmp.bmp.SetPixel(14, 14, tmpColor);
                    bmp.bmp.SetPixel(14, 35, tmpColor);
                    bmp.bmp.SetPixel(14, 42, tmpColor);
                    bmp.bmp.SetPixel(14, 43, tmpColor);
                    bmp.bmp.SetPixel(15, 13, tmpColor);
                    bmp.bmp.SetPixel(15, 24, tmpColor);
                    bmp.bmp.SetPixel(15, 35, tmpColor);
                    bmp.bmp.SetPixel(16, 15, tmpColor);
                    bmp.bmp.SetPixel(16, 35, tmpColor);
                    bmp.bmp.SetPixel(17, 11, tmpColor);
                    bmp.bmp.SetPixel(17, 15, tmpColor);
                    bmp.bmp.SetPixel(17, 17, tmpColor);
                    bmp.bmp.SetPixel(17, 50, tmpColor);
                    bmp.bmp.SetPixel(18, 19, tmpColor);
                    bmp.bmp.SetPixel(19, 24, tmpColor);
                    bmp.bmp.SetPixel(19, 25, tmpColor);
                    bmp.bmp.SetPixel(19, 29, tmpColor);
                    bmp.bmp.SetPixel(20, 24, tmpColor);
                    bmp.bmp.SetPixel(20, 30, tmpColor);
                    bmp.bmp.SetPixel(21, 29, tmpColor);

                    tmpColor = Color.FromArgb((color.R - 72 >= 0) ? color.R - 72 : 0, (color.G - 120 >= 0) ? color.G - 120 : 0, (color.B - 112 >= 0) ? color.B - 112 : 0);
                    bmp.bmp.SetPixel(5, 26, tmpColor);
                    bmp.bmp.SetPixel(7, 34, tmpColor);
                    bmp.bmp.SetPixel(7, 35, tmpColor);
                    bmp.bmp.SetPixel(9, 4, tmpColor);
                    bmp.bmp.SetPixel(15, 4, tmpColor);
                    bmp.bmp.SetPixel(15, 9, tmpColor);
                    bmp.bmp.SetPixel(16, 14, tmpColor);
                    bmp.bmp.SetPixel(17, 34, tmpColor);
                    bmp.bmp.SetPixel(17, 35, tmpColor);
                    bmp.bmp.SetPixel(19, 26, tmpColor);

                    tmpColor = Color.FromArgb((color.R - 136 >= 0) ? color.R - 136 : 0, (color.G - 176 >= 0) ? color.G - 176 : 0, 0);
                    bmp.bmp.SetPixel(2, 27, tmpColor);
                    bmp.bmp.SetPixel(2, 28, tmpColor);
                    bmp.bmp.SetPixel(2, 29, tmpColor);
                    bmp.bmp.SetPixel(3, 24, tmpColor);
                    bmp.bmp.SetPixel(3, 25, tmpColor);
                    bmp.bmp.SetPixel(3, 26, tmpColor);
                    bmp.bmp.SetPixel(3, 30, tmpColor);
                    bmp.bmp.SetPixel(4, 23, tmpColor);
                    bmp.bmp.SetPixel(4, 28, tmpColor);
                    bmp.bmp.SetPixel(4, 29, tmpColor);
                    bmp.bmp.SetPixel(4, 31, tmpColor);
                    bmp.bmp.SetPixel(5, 17, tmpColor);
                    bmp.bmp.SetPixel(5, 18, tmpColor);
                    bmp.bmp.SetPixel(5, 19, tmpColor);
                    bmp.bmp.SetPixel(5, 30, tmpColor);
                    bmp.bmp.SetPixel(5, 31, tmpColor);
                    bmp.bmp.SetPixel(6, 15, tmpColor);
                    bmp.bmp.SetPixel(6, 16, tmpColor);
                    bmp.bmp.SetPixel(6, 24, tmpColor);
                    bmp.bmp.SetPixel(6, 25, tmpColor);
                    bmp.bmp.SetPixel(6, 26, tmpColor);
                    bmp.bmp.SetPixel(6, 35, tmpColor);
                    bmp.bmp.SetPixel(7, 6, tmpColor);
                    bmp.bmp.SetPixel(7, 7, tmpColor);
                    bmp.bmp.SetPixel(7, 8, tmpColor);
                    bmp.bmp.SetPixel(7, 9, tmpColor);
                    bmp.bmp.SetPixel(7, 10, tmpColor);
                    bmp.bmp.SetPixel(7, 11, tmpColor);
                    bmp.bmp.SetPixel(7, 12, tmpColor);
                    bmp.bmp.SetPixel(7, 13, tmpColor);
                    bmp.bmp.SetPixel(7, 14, tmpColor);
                    bmp.bmp.SetPixel(7, 36, tmpColor);
                    bmp.bmp.SetPixel(8, 4, tmpColor);
                    bmp.bmp.SetPixel(8, 5, tmpColor);
                    bmp.bmp.SetPixel(8, 14, tmpColor);
                    bmp.bmp.SetPixel(8, 15, tmpColor);
                    bmp.bmp.SetPixel(8, 42, tmpColor);
                    bmp.bmp.SetPixel(8, 43, tmpColor);
                    bmp.bmp.SetPixel(9, 2, tmpColor);
                    bmp.bmp.SetPixel(9, 3, tmpColor);
                    bmp.bmp.SetPixel(9, 23, tmpColor);
                    bmp.bmp.SetPixel(10, 1, tmpColor);
                    bmp.bmp.SetPixel(10, 3, tmpColor);
                    bmp.bmp.SetPixel(10, 13, tmpColor);
                    bmp.bmp.SetPixel(10, 14, tmpColor);
                    bmp.bmp.SetPixel(10, 15, tmpColor);
                    bmp.bmp.SetPixel(10, 16, tmpColor);
                    bmp.bmp.SetPixel(10, 17, tmpColor);
                    bmp.bmp.SetPixel(11, 0, tmpColor);
                    bmp.bmp.SetPixel(11, 2, tmpColor);
                    bmp.bmp.SetPixel(11, 10, tmpColor);
                    bmp.bmp.SetPixel(11, 11, tmpColor);
                    bmp.bmp.SetPixel(11, 12, tmpColor);
                    bmp.bmp.SetPixel(11, 15, tmpColor);
                    bmp.bmp.SetPixel(11, 35, tmpColor);
                    bmp.bmp.SetPixel(11, 36, tmpColor);
                    bmp.bmp.SetPixel(11, 42, tmpColor);
                    bmp.bmp.SetPixel(11, 43, tmpColor);
                    bmp.bmp.SetPixel(12, 0, tmpColor);
                    bmp.bmp.SetPixel(12, 2, tmpColor);
                    bmp.bmp.SetPixel(12, 8, tmpColor);
                    bmp.bmp.SetPixel(12, 9, tmpColor);
                    bmp.bmp.SetPixel(12, 15, tmpColor);
                    bmp.bmp.SetPixel(13, 0, tmpColor);
                    bmp.bmp.SetPixel(13, 2, tmpColor);
                    bmp.bmp.SetPixel(13, 7, tmpColor);
                    bmp.bmp.SetPixel(13, 15, tmpColor);
                    bmp.bmp.SetPixel(13, 35, tmpColor);
                    bmp.bmp.SetPixel(13, 36, tmpColor);
                    bmp.bmp.SetPixel(13, 42, tmpColor);
                    bmp.bmp.SetPixel(13, 43, tmpColor);
                    bmp.bmp.SetPixel(14, 1, tmpColor);
                    bmp.bmp.SetPixel(14, 3, tmpColor);
                    bmp.bmp.SetPixel(14, 7, tmpColor);
                    bmp.bmp.SetPixel(14, 15, tmpColor);
                    bmp.bmp.SetPixel(15, 2, tmpColor);
                    bmp.bmp.SetPixel(15, 3, tmpColor);
                    bmp.bmp.SetPixel(15, 8, tmpColor);
                    bmp.bmp.SetPixel(15, 10, tmpColor);
                    bmp.bmp.SetPixel(15, 14, tmpColor);
                    bmp.bmp.SetPixel(15, 23, tmpColor);
                    bmp.bmp.SetPixel(16, 4, tmpColor);
                    bmp.bmp.SetPixel(16, 5, tmpColor);
                    bmp.bmp.SetPixel(16, 9, tmpColor);
                    bmp.bmp.SetPixel(16, 13, tmpColor);
                    bmp.bmp.SetPixel(16, 42, tmpColor);
                    bmp.bmp.SetPixel(16, 43, tmpColor);
                    bmp.bmp.SetPixel(17, 6, tmpColor);
                    bmp.bmp.SetPixel(17, 7, tmpColor);
                    bmp.bmp.SetPixel(17, 8, tmpColor);
                    bmp.bmp.SetPixel(17, 9, tmpColor);
                    bmp.bmp.SetPixel(17, 12, tmpColor);
                    bmp.bmp.SetPixel(17, 14, tmpColor);
                    bmp.bmp.SetPixel(17, 36, tmpColor);
                    bmp.bmp.SetPixel(18, 10, tmpColor);
                    bmp.bmp.SetPixel(18, 11, tmpColor);
                    bmp.bmp.SetPixel(18, 15, tmpColor);
                    bmp.bmp.SetPixel(18, 16, tmpColor);
                    bmp.bmp.SetPixel(18, 24, tmpColor);
                    bmp.bmp.SetPixel(18, 25, tmpColor);
                    bmp.bmp.SetPixel(18, 26, tmpColor);
                    bmp.bmp.SetPixel(18, 33, tmpColor);
                    bmp.bmp.SetPixel(18, 34, tmpColor);
                    bmp.bmp.SetPixel(18, 35, tmpColor);
                    bmp.bmp.SetPixel(19, 17, tmpColor);
                    bmp.bmp.SetPixel(19, 18, tmpColor);
                    bmp.bmp.SetPixel(19, 30, tmpColor);
                    bmp.bmp.SetPixel(19, 31, tmpColor);
                    bmp.bmp.SetPixel(20, 23, tmpColor);
                    bmp.bmp.SetPixel(20, 28, tmpColor);
                    bmp.bmp.SetPixel(20, 29, tmpColor);
                    bmp.bmp.SetPixel(20, 31, tmpColor);
                    bmp.bmp.SetPixel(21, 24, tmpColor);
                    bmp.bmp.SetPixel(21, 25, tmpColor);
                    bmp.bmp.SetPixel(21, 26, tmpColor);
                    bmp.bmp.SetPixel(21, 30, tmpColor);
                    bmp.bmp.SetPixel(22, 27, tmpColor);
                    bmp.bmp.SetPixel(22, 28, tmpColor);
                    bmp.bmp.SetPixel(22, 29, tmpColor);
                    #endregion
                }
                #endregion
            }
            #endregion
        }
        public static void SetPixelToClass(Enums.ActorClass.ClassName className, Color color, short partie, Bmp bmp)
        {
            #region
            // applique les couleurs du MaskColors sur les parties de la classe
            try
            {
                if (className == Enums.ActorClass.ClassName.naruto)
                {
                    #region
                    Bitmap bmp_Clone = null;// = (Bitmap)bmp.bmp.Clone();
                    // des fois impossible de clonner une image parcequ'elle est en cours d'utilisation
                    // on refait autant de fois jusqu'a se que l'image se clone, mais le thread risque de ralentir l'application s'il s'agit d'un thread principale
                    int cnt = 1;
                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        try
                        {
                            bmp_Clone = (Bitmap)bmp.bmp.Clone();
                            break;
                        }
                        catch
                        {

                        }
                        CommonCode.ChatMsgFormat("S", "null", cnt + " tentative de clonnage de l'image échoué");
                        cnt++;
                        Thread.Sleep(20);
                    }
                    LockBitmap b = new LockBitmap(bmp_Clone);
                    b.LockBits();
                    //Benchmark.Start();

                    if (partie == 1)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }

                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(248, 248, 40))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(248, 144, 8))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R=R		V=V-104		B=B-32
                                    Color tmpColor = Color.FromArgb(color.R, (color.G - 104 >= 0) ? color.G - 104 : 0, (color.B - 32 >= 0) ? color.B - 32 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(192, 56, 0))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 3 = R=R-56	v=v-192		B=B-40 ou B=0
                                    Color tmpColor = Color.FromArgb((color.R - 56 >= 0) ? color.R - 56 : 0, (color.G - 192 >= 0) ? color.G - 192 : 0, (color.B - 40 >= 0) ? color.B - 40 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(128, 24, 0))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 3 = R-120	v=v-224		b=b-40 ou b=0
                                    Color tmpColor = Color.FromArgb((color.R - 120 >= 0) ? color.R - 120 : 0, (color.G - 224 >= 0) ? color.G - 224 : 0, (color.B - 40 >= 0) ? color.B - 40 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    else if (partie == 2)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }

                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(248, 200, 120))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(200, 88, 40))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    Color tmpColor = Color.FromArgb((color.R - 48 >= 0) ? color.R - 48 : 0, (color.G - 112 >= 0) ? color.G - 112 : 0, (color.B - 80 >= 0) ? color.B - 80 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    else if (partie == 3)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }

                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(48, 56, 64))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(16, 24, 40))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-32	v=v-32		b=b-24
                                    Color tmpColor = Color.FromArgb((color.R - 32 >= 0) ? color.R - 32 : 0, (color.G - 32 >= 0) ? color.G - 32 : 0, (color.B - 24 >= 0) ? color.B - 24 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    
                    b.UnlockBits();
                    bmp.bmp = bmp_Clone;
                    #endregion
                }
                else if (className == Enums.ActorClass.ClassName.choji)
                {
                    #region
                    Bitmap bmp_Clone = null;// = (Bitmap)bmp.bmp.Clone();
                    // des fois impossible de clonner une image parcequ'elle est en cours d'utilisation
                    // on refait autant de fois jusqu'a se que l'image se clone, mais le thread risque de ralentir l'application s'il s'agit d'un thread principale, se qui n'est pas le cas NORMALEMENT vus que cette methode est utilisé dans un autre thread mais par oublie on sais jamais
                    int cnt = 1;
                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        try
                        {
                            bmp_Clone = (Bitmap)bmp.bmp.Clone();
                            break;
                        }
                        catch
                        {

                        }
                        CommonCode.ChatMsgFormat("S", "null", cnt + " tentative de clonnage de l'image échoué");
                        cnt++;
                        Thread.Sleep(20);
                    }

                    //Bitmap bmp_Clone = (Bitmap)bmp.bmp.Clone();
                    LockBitmap b = new LockBitmap(bmp_Clone);
                    b.LockBits();
                    
                    if (partie == 1)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }

                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(176, 80, 24))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(120, 48, 8))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-56	    v=v-32		b=b-16
                                    Color tmpColor = Color.FromArgb((color.R - 56 >= 0) ? color.R - 56 : 0, (color.G - 32 >= 0) ? color.G - 32 : 0, (color.B - 16 >= 0) ? color.B - 16 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    else if (partie == 2)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }

                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(168, 16, 16))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(96, 8, 8))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-72	    v=v-8		b=b-8
                                    Color tmpColor = Color.FromArgb((color.R - 72 >= 0) ? color.R - 72 : 0, (color.G - 8 >= 0) ? color.G - 8 : 0, (color.B - 8 >= 0) ? color.B - 8 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    else if (partie == 3)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }

                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(120, 120, 120))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(80, 80, 80))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-40	    v=v-40		b=b-40
                                    Color tmpColor = Color.FromArgb((color.R - 40 >= 0) ? color.R - 40 : 0, (color.G - 40 >= 0) ? color.G - 40 : 0, (color.B - 40 >= 0) ? color.B - 40 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(32, 32, 32))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-88	    v=v-88		b=b-88
                                    Color tmpColor = Color.FromArgb((color.R - 88 >= 0) ? color.R - 88 : 0, (color.G - 88 >= 0) ? color.G - 88 : 0, (color.B - 88 >= 0) ? color.B - 88 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }

                    b.UnlockBits();
                    bmp.bmp = bmp_Clone;
                    #endregion
                }
                else if (className == Enums.ActorClass.ClassName.kabuto)
                {
                    #region
                    Bitmap bmp_Clone = null;// = (Bitmap)bmp.bmp.Clone();
                    // des fois impossible de clonner une image parcequ'elle est en cours d'utilisation
                    // on refait autant de fois jusqu'a se que l'image se clone, mais le thread risque de ralentir l'application s'il s'agit d'un thread principale
                    int cnt = 1;
                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        try
                        {
                            bmp_Clone = (Bitmap)bmp.bmp.Clone();
                            break;
                        }
                        catch
                        {

                        }
                        CommonCode.ChatMsgFormat("S", "null", cnt + " tentative de clonnage de l'image échoué");
                        cnt++;
                        Thread.Sleep(20);
                    }
                    //Bitmap bmp_Clone = (Bitmap)bmp.bmp.Clone();
                    LockBitmap b = new LockBitmap(bmp_Clone);
                    b.LockBits();
                    if (partie == 1)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(200, 192, 184))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(152, 144, 152))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-48	    v=v-48		b=b-32
                                    Color tmpColor = Color.FromArgb((color.R - 48 >= 0) ? color.R - 48 : 0, (color.G - 48 >= 0) ? color.G - 48 : 0, (color.B - 32 >= 0) ? color.B - 32 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(88, 88, 88))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-112	    v=v-104		b=b-96
                                    Color tmpColor = Color.FromArgb((color.R - 112 >= 0) ? color.R - 112 : 0, (color.G - 104 >= 0) ? color.G - 104 : 0, (color.B - 96 >= 0) ? color.B - 96 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    else if (partie == 2)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(104, 32, 120))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(64, 24, 96))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-40	    v=v-8		b=b-24
                                    Color tmpColor = Color.FromArgb((color.R - 40 >= 0) ? color.R - 40 : 0, (color.G - 8 >= 0) ? color.G - 8 : 0, (color.B - 24 >= 0) ? color.B - 24 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(32, 8, 72))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-72	    v=v-24		b=b-48
                                    Color tmpColor = Color.FromArgb((color.R - 32 >= 0) ? color.R - 32 : 0, (color.G - 8 >= 0) ? color.G - 8 : 0, (color.B - 72 >= 0) ? color.B - 72 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    else if (partie == 3)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(248, 200, 120))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(232, 152, 80))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-16	    v=v-48		b=b-40
                                    Color tmpColor = Color.FromArgb((color.R - 16 >= 0) ? color.R - 16 : 0, (color.G - 48 >= 0) ? color.G - 48 : 0, (color.B - 40 >= 0) ? color.B - 40 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(160, 96, 56))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-88	    v=v-104		b=b-64
                                    Color tmpColor = Color.FromArgb((color.R - 88 >= 0) ? color.R - 88 : 0, (color.G - 104 >= 0) ? color.G - 104 : 0, (color.B - 64 >= 0) ? color.B - 64 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(80, 48, 32))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-168	    v=v-52		b=b-88
                                    Color tmpColor = Color.FromArgb((color.R - 168 >= 0) ? color.R - 168 : 0, (color.G - 52 >= 0) ? color.G - 52 : 0, (color.B - 88 >= 0) ? color.B - 88 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    
                    b.UnlockBits();
                    bmp.bmp = bmp_Clone;
                    #endregion
                }
                else if (className == Enums.ActorClass.ClassName.ino)
                {
                    #region
                    Bitmap bmp_Clone = null;// = (Bitmap)bmp.bmp.Clone();
                    // des fois impossible de clonner une image parcequ'elle est en cours d'utilisation
                    // on refait autant de fois jusqu'a se que l'image se clone, mais le thread risque de ralentir l'application s'il s'agit d'un thread principale
                    int cnt = 1;
                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        try
                        {
                            bmp_Clone = (Bitmap)bmp.bmp.Clone();
                            break;
                        }
                        catch
                        {

                        }
                        CommonCode.ChatMsgFormat("S", "null", cnt + " tentative de clonnage de l'image échoué");
                        cnt++;
                        Thread.Sleep(20);
                    }
                    //Bitmap bmp_Clone = (Bitmap)bmp.bmp.Clone();
                    LockBitmap b = new LockBitmap(bmp_Clone);
                    b.LockBits();
                    if (partie == 1)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(248, 208, 112))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(208, 136, 32))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-40	    v=v-72		b=b-80
                                    Color tmpColor = Color.FromArgb((color.R - 40 >= 0) ? color.R - 40 : 0, (color.G - 72 >= 0) ? color.G - 72 : 0, (color.B - 80 >= 0) ? color.B - 80 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    else if (partie == 2)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(128, 0, 128))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(56, 0, 64))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-72	    v=0		b=b-64
                                    Color tmpColor = Color.FromArgb((color.R - 72 >= 0) ? color.R - 72 : 0, 0, (color.B - 64 >= 0) ? color.B - 64 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    else if (partie == 3)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            bool pixelOutSide = false;      // des fois l'image change et du coup la routine garde les anciens dimentions, du coup un erreur se regénére
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                // controle contre une erreur qui arrive que la pixel choisie ne se trouve pas dans le cadre de notre image
                                if (b.Width < cnt1 + bmp.rectangle.X)
                                {
                                    ChatMsgFormat("S", "null", "pixel horizontal introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                else if (b.Height < cnt2 + bmp.rectangle.Y)
                                {
                                    ChatMsgFormat("S", "null", "pixel verticale introuvable");
                                    pixelOutSide = true;
                                    break;
                                }
                                Color p = b.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(232, 208, 168))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(216, 136, 80))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-16	    v=v-72		b=b-88
                                    Color tmpColor = Color.FromArgb((color.R - 16 >= 0) ? color.R - 16 : 0, (color.G - 72 >= 0) ? color.G - 72 : 0, (color.B - 88 >= 0) ? color.B - 88 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(160, 88, 56))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-72	    v=v-120		b=b-112
                                    Color tmpColor = Color.FromArgb((color.R - 72 >= 0) ? color.R - 72 : 0, (color.G - 120 >= 0) ? color.G - 120 : 0, (color.B - 112 >= 0) ? color.B - 112 : 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(96, 32, 0))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-136	    v=v-176		b=0
                                    Color tmpColor = Color.FromArgb((color.R - 136 >= 0) ? color.R - 136 : 0, (color.G - 176 >= 0) ? color.G - 176 : 0, 0);
                                    b.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                            if (pixelOutSide)
                                break;
                        }
                    }
                    
                    b.UnlockBits();
                    bmp.bmp = bmp_Clone;
                    #endregion
                }
                else if (className == Enums.ActorClass.ClassName.lee)
                {
                    #region
                    // ajouter la creation du copy du bitmap
                    /*if (partie == 1)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                Color p = bmp.bmp.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(1, 1, 1))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                            }
                        }
                    }
                    else if (partie == 2)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                Color p = bmp.bmp.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(248, 216, 160))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(232, 152, 80))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-16	    v=v-64		b=b-80
                                    Color tmpColor = Color.FromArgb((color.R - 16 >= 0) ? color.R - 16 : 0, (color.R - 64 >= 0) ? color.R - 64 : 0, (color.B - 80 >= 0) ? color.B - 80 : 0);
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(144, 88, 24))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-104	    v=v-128		b=b-136
                                    Color tmpColor = Color.FromArgb((color.R - 104 >= 0) ? color.R - 104 : 0, (color.R - 128 >= 0) ? color.R - 128 : 0, (color.B - 136 >= 0) ? color.B - 136 : 0);
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(80, 32, 0))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-168	    v=v-184		b=0
                                    Color tmpColor = Color.FromArgb((color.R - 168 >= 0) ? color.R - 168 : 0, (color.R - 184 >= 0) ? color.R - 184 : 0, 0);
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(224, 168, 48))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-24	    v=v-48		b=112
                                    Color tmpColor = Color.FromArgb((color.R - 24 >= 0) ? color.R - 24 : 0, (color.R - 48 >= 0) ? color.R - 48 : 0, (color.R - 112 >= 0) ? color.R - 112 : 0);
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                        }
                    }
                    else if (partie == 3)
                    {
                        for (int cnt1 = 0; cnt1 < bmp.rectangle.Width; cnt1++)
                        {
                            for (int cnt2 = 0; cnt2 < bmp.rectangle.Height; cnt2++)
                            {
                                Color p = bmp.bmp.GetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y);
                                if (p == Color.FromArgb(168, 208, 144))
                                {
                                    // la 1ere couleurs ne subis pas de dégrade
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, color);
                                }
                                else if (p == Color.FromArgb(88, 128, 88))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-80	    v=v-80		b=b-56
                                    Color tmpColor = Color.FromArgb((color.R - 80 >= 0) ? color.R - 80 : 0, (color.G - 80 >= 0) ? color.G - 80 : 0, (color.B - 56 >= 0) ? color.B - 56 : 0);
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(48, 120, 56))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-120	    v=v-88		b=b-88
                                    Color tmpColor = Color.FromArgb((color.R - 120 >= 0) ? color.R - 120 : 0, (color.G - 88 >= 0) ? color.G - 88 : 0, (color.B - 88 >= 0) ? color.B - 88 : 0);
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(40, 72, 48))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-128	    v=v-136		b=96
                                    Color tmpColor = Color.FromArgb((color.R - 128 >= 0) ? color.R - 128 : 0, (color.G - 136 >= 0) ? color.G - 136 : 0, (color.G - 96 >= 0) ? color.G - 96 : 0);
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                                else if (p == Color.FromArgb(32, 48, 32))
                                {
                                    // tamisation de la couleur selon le filtre suivant
                                    // couleur 2 = R-136	    v=v-160		b=112
                                    Color tmpColor = Color.FromArgb((color.R - 136 >= 0) ? color.R - 136 : 0, (color.G - 160 >= 0) ? color.G - 160 : 0, (color.G - 112 >= 0) ? color.G - 112 : 0);
                                    bmp.bmp.SetPixel(cnt1 + bmp.rectangle.X, cnt2 + bmp.rectangle.Y, tmpColor);
                                }
                            }
                        }
                    }*/
                    #endregion
                }
                else if (className == Enums.ActorClass.ClassName.kankura)
                {

                }
                else if (className == Enums.ActorClass.ClassName.shikamaru)
                {

                }
                else if (className == Enums.ActorClass.ClassName.sakura)
                {

                }
            }
            catch (Exception ex)
            {
                if (CommonCode.debug)
                    MessageBox.Show(ex.ToString());
            }
            #endregion
        }
        public static void ChangeMap(string map)
        {
            #region
            // change la map
            if (map == "Start")
            {
                GameStateManager.ChangeState(new Start());
                GameStateManager.CheckState();
            }
            /*else if (map == "CreatePlayer")
            {
                GameStateManager.ChangeState(new CreatePlayer());
                GameStateManager.CheckState();
            }*/
            else if (map == "_0_0_0")
            {
                GameStateManager.ChangeState(new _0_0_0());
                GameStateManager.CheckState();
            }
            #endregion
        }
        public static Int16 ConvertToClockWizeOrientation(int i)
        {
            #region
            // convertie de l'orientation au spriteSheet corespondante, 1 = vus de derriere,2 vus vers l'adroite,3 vus vers le bas (en face),4 vers la gauche
            if (i == 0)
                return 12;
            else if (i == 1)
                return 8;
            else if (i == 2)
                return 0;
            else if (i == 3)
                return 4;
            else
                return 0;
            #endregion
        }
        public static bool ValidateMove(Point start, Point end)
        {
            #region
            // verifie la validité du waypoint coté client
            if ((start.X - end.X == 1 || start.X - end.X == -1) && (start.Y - end.Y == 1 || start.Y - end.Y == -1))
                return true;
            else
                return false;
            #endregion
        }
        public static string TranslateText(int i)
        {
            #region
            List<string> langFile = new List<string>();
            using (StreamReader sr = new StreamReader(@"lang\" + langue + ".ini"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    langFile.Add(line);
                sr.Close();
            }

            // traduit un texte selon la langue séléctionné
            return langFile.ElementAt(i).Replace("☼", Environment.NewLine);
            #endregion
        }
        public static void AdjustPositionAndDirection(Bmp ibplayer, Point point)
        {
            #region
            // redresse le joueur sur sa case s'il a été décalé de quelque pixel selon les vrais coordonées de sa position relative
            ibplayer.ChangeBmp(ibplayer.path, SpriteSheet.GetSpriteSheet(((Actor)ibplayer.tag).className.ToString(), CommonCode.ConvertToClockWizeOrientation((ibplayer.tag as Actor).directionLook)));
            // repositionnement horizental
            ibplayer.point = new Point((point.X / 30 * 30) + 15 - (ibplayer.rectangle.Width / 2), (point.Y / 30 * 30) - (ibplayer.rectangle.Height) + 15);
            #endregion
        }
        public static string RemoveBalises(string msg)
        {
            #region
            // formate le message affiché comme info bulle des balise
            msg = msg.Replace("[l/]", "");
            msg = msg.Replace("[\\l]", "");

            return msg;
            #endregion
        }
        public static void VerticalSyncZindex(Bmp bmp)
        {
            #region
            // réatribue une valeur Zindex selon la position vertical
            if (bmp.TypeGfx == Manager.TypeGfx.Bgr)
                bmp.zindex = (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) + Manager.manager.GfxBgrList.FindAll(o => (o != null && o.GetType() == typeof(Bmp)) && (o as Bmp).zindex >= (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) && (o as Bmp).zindex <= (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) + 99).Count();
            else if (bmp.TypeGfx == Manager.TypeGfx.Obj)
                bmp.zindex = (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) + Manager.manager.GfxObjList.FindAll(o => (o != null && o.GetType() == typeof(Bmp)) && (o as Bmp).zindex >= (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) && (o as Bmp).zindex <= (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) + 99).Count();
            else if (bmp.TypeGfx == Manager.TypeGfx.Top)
                bmp.zindex = (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) + Manager.manager.GfxTopList.FindAll(o => (o != null && o.GetType() == typeof(Bmp)) && (o as Bmp).zindex >= (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) && (o as Bmp).zindex <= (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) + 99).Count();
            else
                bmp.zindex = (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) + Manager.manager.GfxBgrList.FindAll(o => (o != null && o.GetType() == typeof(Bmp)) && (o as Bmp).zindex >= (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) && (o as Bmp).zindex <= (((bmp.point.Y + bmp.rectangle.Height) / 30) * 100) + 99).Count();
            #endregion
        }
        public static int VerticalSyncZindex(int relatifVerticalPosition, Manager.TypeGfx TypeGfx)
        {
            #region // pos de la tuile sur la map et non du pixel
            // réatribue une valeur Zindex selon la position vertical
            switch (TypeGfx)
            {
                case Manager.TypeGfx.Bgr:
                    return (relatifVerticalPosition * 100) + Manager.manager.GfxBgrList.FindAll(o => o != null && o.Zindex() >= relatifVerticalPosition * 100 && o.Zindex() <= (relatifVerticalPosition * 100) + 99).Count();
                case Manager.TypeGfx.Obj:
                    return (relatifVerticalPosition * 100) + Manager.manager.GfxObjList.FindAll(o => o != null && o.Zindex() >= relatifVerticalPosition * 100 && o.Zindex() <= (relatifVerticalPosition * 100) + 99).Count();
                case Manager.TypeGfx.Top:
                    return (relatifVerticalPosition * 100) + Manager.manager.GfxTopList.FindAll(o => o != null && o.Zindex() >= relatifVerticalPosition * 100 && o.Zindex() <= (relatifVerticalPosition * 100) + 99).Count();
                default:
                    return (relatifVerticalPosition * 100) + Manager.manager.GfxBgrList.FindAll(o => o != null && o.Zindex() >= relatifVerticalPosition * 100 && o.Zindex() <= (relatifVerticalPosition * 100) + 99).Count();
            }

            #endregion
        }
        public static string officialRangToCurrentLangTranslation(Enums.Rang.official i)
        {
            #region
            List<string> langFile = new List<string>();
            using (StreamReader sr = new StreamReader(@"lang\" + langue + ".ini"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    langFile.Add(line);
                sr.Close();
            }
            if (i == 0)
                return langFile.ElementAt(70);
            else if ((int)i == 1)
                return langFile.ElementAt(71);
            else if ((int)i == 2)
                return langFile.ElementAt(72);
            else if ((int)i == 3)
                return langFile.ElementAt(73);
            else if ((int)i == 4)
                return langFile.ElementAt(74);
            else
                return langFile.ElementAt(70);
            #endregion
        }
        public static void UpdateUsingElement(Enums.Chakra.Element element, int value)
        {
            #region // redimentionnement de la bar qui represente l'utilisation fréquante d'un element selon sa valeur en niveau
            if (value >= CommonCode.chakra1Level && value < CommonCode.chakra2Level)
            {
                if (element == Enums.Chakra.Element.doton)
                    MenuStats.TerreLvlGauge.size.Width = 37;
                else if (element == Enums.Chakra.Element.katon)
                    MenuStats.FeuLvlGauge.size.Width = 37;
                else if (element == Enums.Chakra.Element.futon)
                    MenuStats.VentLvlGauge.size.Width = 37;
                else if (element == Enums.Chakra.Element.raiton)
                    MenuStats.FoudreLvlGauge.size.Width = 37;
                else if (element == Enums.Chakra.Element.suiton)
                    MenuStats.EauLvlGauge.size.Width = 37;
            }
            else if (value >= CommonCode.chakra2Level && value < CommonCode.chakra3Level)
            {
                if (element == Enums.Chakra.Element.doton)
                    MenuStats.TerreLvlGauge.size.Width = 74;
                else if (element == Enums.Chakra.Element.katon)
                    MenuStats.FeuLvlGauge.size.Width = 74;
                else if (element == Enums.Chakra.Element.futon)
                    MenuStats.VentLvlGauge.size.Width = 74;
                else if (element == Enums.Chakra.Element.raiton)
                    MenuStats.FoudreLvlGauge.size.Width = 74;
                else if (element == Enums.Chakra.Element.suiton)
                    MenuStats.EauLvlGauge.size.Width = 74;
            }
            else if (value >= CommonCode.chakra3Level && value < CommonCode.chakra4Level)
            {
                if (element == Enums.Chakra.Element.doton)
                    MenuStats.TerreLvlGauge.size.Width = 108;
                else if (element == Enums.Chakra.Element.katon)
                    MenuStats.FeuLvlGauge.size.Width = 108;
                else if (element == Enums.Chakra.Element.futon)
                    MenuStats.VentLvlGauge.size.Width = 108;
                else if (element == Enums.Chakra.Element.raiton)
                    MenuStats.FoudreLvlGauge.size.Width = 108;
                else if (element == Enums.Chakra.Element.suiton)
                    MenuStats.EauLvlGauge.size.Width = 108;
            }
            else if (value >= CommonCode.chakra4Level && value < CommonCode.chakra5Level)
            {
                if (element == Enums.Chakra.Element.doton)
                    MenuStats.TerreLvlGauge.size.Width = 144;
                else if (element == Enums.Chakra.Element.katon)
                    MenuStats.FeuLvlGauge.size.Width = 144;
                else if (element == Enums.Chakra.Element.futon)
                    MenuStats.VentLvlGauge.size.Width = 144;
                else if (element == Enums.Chakra.Element.raiton)
                    MenuStats.FoudreLvlGauge.size.Width = 144;
                else if (element == Enums.Chakra.Element.suiton)
                    MenuStats.EauLvlGauge.size.Width = 144;
            }
            else if (value >= CommonCode.chakra5Level)
            {
                if (element == Enums.Chakra.Element.doton)
                    MenuStats.TerreLvlGauge.size.Width = 180;
                else if (element == Enums.Chakra.Element.katon)
                    MenuStats.FeuLvlGauge.size.Width = 180;
                else if (element == Enums.Chakra.Element.futon)
                    MenuStats.VentLvlGauge.size.Width = 180;
                else if (element == Enums.Chakra.Element.raiton)
                    MenuStats.FoudreLvlGauge.size.Width = 180;
                else if (element == Enums.Chakra.Element.suiton)
                    MenuStats.EauLvlGauge.size.Width = 180;
            }
            #endregion
        }
        public static string MoneyThousendSeparation(string number)
        {
            // ajoute des séparateur de millier apres chaque 3 caractères
            int cnt = number.Count();
            string data = "";
            int separatorCnt = 0;
            for (int cnt2 = 0; cnt2 < cnt; cnt2++)
            {
                if (separatorCnt < 3)
                    data = number[number.Length - 1 - cnt2] + data;
                else
                {
                    data = number[number.Length - 1 - cnt2] + " " + data;
                    separatorCnt = 0;
                }
                separatorCnt++;
            }
            return data;
        }
        public static void ChatMsgFormat(string chan, string user, string msg)
        {
            #region recevoir un message
            // ecrir un message sur l'ecran si c générale si non sur la barre de chat
            ///////////////// affichage sur la zone de chat
            // reinvocation du thread principale au cas ou le control est appelé depuis un autre thread
            if (Manager.manager.mainForm.IsDisposed)
                return;
            Manager.manager.mainForm.BeginInvoke((Action)(() =>
            {
                RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

                // user
                //////// afficher l'info bulle quand c'est un message général
                if (chan == "G")
                {
                    #region chan general
                    
                    /////////////////////// l'heur
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Black;
                    ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                    /////////////////////////////////////////

                    // affichage du texte
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Bold);
                    ChatArea.InsertLink(user[0].ToString().ToUpper() + user.Substring(1, user.Length - 1), "P");

                    //said
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Black;
                    ChatArea.AppendText(" " + CommonCode.TranslateText(37) + " : ");
                    //message
                    ChatArea.SelectionColor = ChatArea.ForeColor;
                    ChatArea.SelectionFont = new Font("verdana", 7, FontStyle.Italic);
                    // recherche l'existance d'un lien dans le text
                    if (msg.IndexOf("[l/]") != -1 && msg.IndexOf("[\\l]") != -1 && msg.Length > 12)
                    {
                        // le texte contiens un lien
                        // affichage du texte qui precede la balise ouverture de lien
                        ChatArea.AppendText(msg.Substring(0, msg.IndexOf("[l/]")) + " ");
                        string tmpMsg = msg.Substring(msg.IndexOf("[l/]") + 4, msg.IndexOf("[\\l]") - msg.IndexOf("[l/]") - 4);
                        ChatArea.InsertLink(tmpMsg);
                        int pos1 = msg.IndexOf("[\\l]") + 4;
                        string str1 = msg.Substring(pos1, msg.Length - pos1);
                        ChatArea.AppendText(str1);
                    }
                    else
                        ChatArea.AppendText(msg);

                    ChatArea.SelectionColor = ChatArea.ForeColor;
                    ////////////////////////////////////////////////////////////

                    if (CommonCode.AllActorsInMap.Exists(f => (f.tag as Actor).pseudo == user))
                    {
                        if (Battle.state == Enums.battleState.state.idle)
                        {
                            // le client diffuse deja un message
                            // annulation de l'ancienne image du chatbulle
                            Bmp tmp = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == user);
                            Manager.manager.GfxObjList.Remove((tmp.tag as Actor).chatBulleRec);

                            // arret du thread du chat
                            try
                            {
                                abortChatThread = true;
                                // supression d'un message s'il est toujours affiché malgré que le thread sois térmné
                                if (Manager.manager.GfxObjList.Exists(f => f.Name() == "__InfoBulleRec_" + user))
                                {
                                    Manager.manager.GfxObjList.Find(f => f.Name() == "__InfoBulleRec_" + user).Visible(false);
                                    Manager.manager.GfxObjList.RemoveAll(f => f.Name() == "__InfoBulleRec_" + user);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                            // lancement du thread
                            Thread tChatBulle = new Thread(() => InfoBulle(msg, tmp));
                            tChatBulle.Start();
                        }
                    }
                    #endregion
                }
                else if (chan == "P")
                {
                    #region canal privé
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;

                    /////////////////////// symbole de conversation
                    Bmp ___CBchatFlag = new Bmp(@"gfx\general\obj\1\CBchatFlag.dat", Point.Empty, "__CBchatFlag", Manager.TypeGfx.Obj, true, 1);
                    Bitmap _myBitmap = new Bitmap(___CBchatFlag.bmp);
                    ChatArea.InsertImage(_myBitmap);
                    ////////////////////////////////////

                    ///////////////////// l'heur
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Green;
                    ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                    //////////////////////////////////////////////////////

                    ////////////////////////////////////////
                    // message privé
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.InsertLink(user, "P");

                    //said
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Green;
                    ChatArea.AppendText(" " + CommonCode.TranslateText(37) + " : ");
                    //message
                    ChatArea.SelectionColor = ChatArea.ForeColor;

                    // recherche l'existance d'un lien dans le text
                    if (msg.IndexOf("[l/]") != -1 && msg.IndexOf("[\\l]") != -1 && msg.Length > 12)
                    {
                        // le texte contiens un lien
                        // affichage du texte qui precede la balise ouverture de lien
                        ChatArea.AppendText(msg.Substring(0, msg.IndexOf("[l/]")) + " ");
                        string tmpMsg = msg.Substring(msg.IndexOf("[l/]") + 4, msg.IndexOf("[\\l]") - msg.IndexOf("[l/]") - 4);
                        ChatArea.InsertLink(tmpMsg);
                        int pos1 = msg.IndexOf("[\\l]") + 4;
                        string str1 = msg.Substring(pos1, msg.Length - pos1);
                        ChatArea.AppendText(str1);
                    }
                    else
                        ChatArea.AppendText(msg);

                    ChatArea.SelectionColor = ChatArea.ForeColor;
                    ////////////////////////////////////////////////////////////
                    #endregion
                }
                else if (chan == "S")
                {
                    #region canal system
                    ////////////////////// mise a la fin de ligne
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;

                    ///////////////////// l'heur
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Red;
                    ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                    //////////////////////////////////////////////////////

                    // chan system
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Red;
                    ChatArea.AppendText(" " + msg);
                    #endregion
                }
                else if (chan == "dead")
                {
                    #region lorsqu'un joueur est mort

                    ////////////////////// mise a la fin de ligne
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;

                    /////////////////////// symbole de conversation
                    /*Bmp __CBsystemFlag = new Bmp(@"gfx\general\obj\1\CBbattleFlag.dat", Point.Empty, "__CBbattleFlag", Manager.TypeGfx.Obj, true, 1);
                    Bitmap myBitmap = new Bitmap(__CBsystemFlag.bmp);
                    ChatArea.InsertImage(myBitmap);*/
                    ////////////////////////////////////

                    ///////////////////// l'heur
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Brown;
                    ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                    //////////////////////////////////////////////////////

                    // chan system
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Brown;
                    string _deadName = (user.IndexOf('$') == -1) ? user : user.Substring(0, user.IndexOf('$')) + "(" + TranslateText(93) + ")";
                    _deadName += "(" + CommonCode.TranslateText(121) + ")";
                    ChatArea.AppendText(_deadName);
                    #endregion
                }
                else if(chan == "B")
                {
                    #region canal battle en general

                    ////////////////////// mise a la fin de ligne
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;

                    /////////////////////// symbole de conversation
                    /*Bmp __CBbattleFlag = new Bmp(@"gfx\general\obj\1\CBbattleFlag.dat", Point.Empty, "__CBbattleFlag", Manager.TypeGfx.Obj, true, 1);
                    Bitmap myBitmap = new Bitmap(__CBbattleFlag.bmp);
                    ChatArea.InsertImage(myBitmap);*/
                    ////////////////////////////////////

                    ///////////////////// l'heur
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Brown;
                    ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                    //////////////////////////////////////////////////////

                    // chan system
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Brown;
                    ChatArea.AppendText(" " + msg);
                    #endregion
                }
                if (chan == "I")
                {
                    #region chan general
                    /////////////////////// l'heur
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Green;
                    ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                    /////////////////////////////////////////

                    // affichage du texte
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Bold);

                    //message
                    ChatArea.SelectionColor = Color.Green;
                    // recherche l'existance d'un lien dans le text
                    if (msg.IndexOf("[l/]") != -1 && msg.IndexOf("[\\l]") != -1 && msg.Length > 12)
                    {
                        // le texte contiens un lien
                        // affichage du texte qui precede la balise ouverture de lien
                        ChatArea.AppendText(msg.Substring(0, msg.IndexOf("[l/]")) + " ");
                        string tmpMsg = msg.Substring(msg.IndexOf("[l/]") + 4, msg.IndexOf("[\\l]") - msg.IndexOf("[l/]") - 4);
                        ChatArea.InsertLink(tmpMsg);
                        int pos1 = msg.IndexOf("[\\l]") + 4;
                        string str1 = msg.Substring(pos1, msg.Length - pos1);
                        ChatArea.AppendText(str1);
                    }
                    else
                        ChatArea.AppendText(msg);

                    ChatArea.SelectionColor = Color.Green;
                    ////////////////////////////////////////////////////////////

                    if (CommonCode.AllActorsInMap.Exists(f => (f.tag as Actor).pseudo == user))
                    {
                        if (Battle.state == Enums.battleState.state.idle)
                        {
                            // le client diffuse deja un message
                            // annulation de l'ancienne image du chatbulle
                            Bmp tmp = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == user);
                            Manager.manager.GfxObjList.Remove((tmp.tag as Actor).chatBulleRec);

                            // arret du thread du chat
                            try
                            {
                                abortChatThread = true;
                                // supression d'un message s'il est toujours affiché malgré que le thread sois térmné
                                if (Manager.manager.GfxObjList.Exists(f => f.Name() == "__InfoBulleRec_" + user))
                                {
                                    Manager.manager.GfxObjList.Find(f => f.Name() == "__InfoBulleRec_" + user).Visible(false);
                                    Manager.manager.GfxObjList.RemoveAll(f => f.Name() == "__InfoBulleRec_" + user);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                            // lancement du thread
                            Thread tChatBulle = new Thread(() => InfoBulle(msg, tmp));
                            tChatBulle.Start();
                        }
                    }
                    #endregion
                }

                ChatArea.AppendText(Environment.NewLine);
                ChatAreaCursorInTheEnd();
            }));
            #endregion
        }
        public static void ChatMsgBattleFormat(string chan, string user, string domString, Point TargetPlayer, int sortID)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            // ecrir un message sur l'ecran si c générale si non sur la barre de chat
            ///////////////// affichage sur la zone de chat
            //reinvocation du thread principale au cas ou le control est appelé depuis un autre thread
            Manager.manager.mainForm.BeginInvoke((Action)(() =>
            {   
                if (chan == "spell")
                {
                    #region lancement d'un sort
                    // chan de combat "battle"
                    // calculateDom(typeRox:rox ou heal|jet:x|cd:true ou false|chakra:futon...|dom:x|deadList:joueurMort1:joueurMort2...|roxed

                    // s'il sagit d'une invoc, on soustrait l'id pour ne pas l'afficher
                    string player = (user.IndexOf('$') != -1) ? user.Split('$')[0] + "(" + CommonCode.TranslateText(93) + ")" : user;

                    for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
                    {
                        ChatArea.AppendText((ChatArea.Text == "") ? "" : "\n");
                        string data = domString.Split('#')[cnt];
                        string[] DomString = data.Split('|');
                        string typeRox = DomString[0].Split(':')[1];
                        if (typeRox == "rox")
                        {
                            // dom(typeRox:addInvoc|name:x|cd:x|totalPdv:x)
                            int jet = Convert.ToInt16(DomString[1].Split(':')[1]);
                            bool cd = Convert.ToBoolean(DomString[2].Split(':')[1]);
                            string chakra = DomString[3].Split(':')[1];
                            Enums.Chakra.Element element = (Enums.Chakra.Element)Enum.Parse(typeof(Enums.Chakra.Element), chakra);
                            
                            int dom = Convert.ToInt32(DomString[4].Split(':')[1]);

                            // si celui roxé est une invoc
                            string targetPlayer = (DomString[6].IndexOf('$') != -1) ? DomString[6].Split('$')[0] + "(" + CommonCode.TranslateText(93) + ")" : DomString[6];

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Bold);
                            ChatArea.AppendText(player[0].ToString().ToUpper() + player.Substring(1, player.Length - 1));

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Regular);
                            ChatArea.AppendText(" " + CommonCode.TranslateText(149));

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Bold);
                            ChatArea.AppendText(" " + spells.sort(sortID).title);

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Regular);
                            ChatArea.AppendText(" " + CommonCode.TranslateText(113) + " " + jet + " ");

                            if (cd)
                            {
                                /////////////////////// image de CD
                                ChatArea.SelectionStart = ChatArea.TextLength;
                                ChatArea.SelectionLength = 0;

                                /////////////////////// symbole de CD
                                Bmp __CDchatFlag = new Bmp(@"gfx\general\obj\1\CDchatFlag.dat", Point.Empty, "__CDchatFlag", Manager.TypeGfx.Obj, true, 1);
                                Bitmap myBitmap = new Bitmap(__CDchatFlag.bmp);

                                ChatArea.InsertImage(myBitmap);
                                //////////////////////////////////
                            }

                            if (targetPlayer != "null")
                            {
                                ChatArea.SelectionStart = ChatArea.TextLength;
                                ChatArea.SelectionLength = 0;
                                ChatArea.AppendText(" > ");

                                ChatArea.SelectionStart = ChatArea.TextLength;
                                ChatArea.SelectionLength = 0;
                                ChatArea.SelectionColor = Color.Brown;

                                if (typeRox == "rox")
                                    ChatArea.AppendText(targetPlayer + " - ");
                                else if (typeRox == "heal")
                                    ChatArea.AppendText(targetPlayer + " + ");

                                ChatArea.SelectionStart = ChatArea.TextLength;
                                ChatArea.SelectionLength = 0;

                                if (element == Enums.Chakra.Element.doton)
                                    ChatArea.SelectionColor = Color.FromArgb(142, 91, 21);
                                else if (element == Enums.Chakra.Element.katon)
                                    ChatArea.SelectionColor = Color.FromArgb(198, 0, 0);
                                else if (element == Enums.Chakra.Element.futon)
                                    ChatArea.SelectionColor = Color.FromArgb(0, 197, 125);
                                else if (element == Enums.Chakra.Element.raiton)
                                    ChatArea.SelectionColor = Color.FromArgb(215, 203, 0);
                                else if (element == Enums.Chakra.Element.suiton)
                                    ChatArea.SelectionColor = Color.FromArgb(12, 133, 255);

                                ChatArea.AppendText(dom.ToString());

                                ChatArea.SelectionStart = ChatArea.TextLength;
                                ChatArea.SelectionLength = 0;
                                ChatArea.SelectionColor = Color.Brown;
                                ChatArea.AppendText(" " + CommonCode.TranslateText(115));
                            }
                        }
                        else if(typeRox == "addInvoc")
                        {
                            //dom(typeRox:addInvoc|name:x|cd:x|totalPdv:x)
                            string nomInvoc = DomString[1].Split(':')[0];

                            string targetPlayer = nomInvoc.Split('$')[0] + "(" + CommonCode.TranslateText(93) + ")";

                            bool cd = bool.Parse(DomString[2].Split(':')[1]);

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Bold);
                            ChatArea.AppendText(player[0].ToString().ToUpper() + player.Substring(1, player.Length - 1));

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Regular);
                            ChatArea.AppendText(" " + CommonCode.TranslateText(149));

                            ChatArea.SelectionStart = ChatArea.TextLength;
                            ChatArea.SelectionLength = 0;
                            ChatArea.SelectionColor = Color.Brown;
                            ChatArea.SelectionFont = new Font("Verdana", ChatArea.Font.Size, FontStyle.Bold);
                            ChatArea.AppendText(" " + spells.sort(sortID).title + " ");

                            if (cd)
                            {
                                /////////////////////// image de CD
                                ChatArea.SelectionStart = ChatArea.TextLength;
                                ChatArea.SelectionLength = 0;

                                /////////////////////// symbole de CD
                                Bmp __CDchatFlag = new Bmp(@"gfx\general\obj\1\CDchatFlag.dat", Point.Empty, "__CDchatFlag", Manager.TypeGfx.Obj, true, 1);
                                Bitmap myBitmap = new Bitmap(__CDchatFlag.bmp);

                                ChatArea.InsertImage(myBitmap);
                                //////////////////////////////////
                            }
                        }
                    }
                    #endregion
                }
                ChatArea.AppendText(Environment.NewLine);
                ChatAreaCursorInTheEnd();
            }));
        }
        public static void InfoBulle(string msg, Bmp Player)
        {
            // affichage de l'info bulle qui contiens le texte du chat canal general
            // supression des balises comme pour le liens [/l][\l]
            msg = RemoveBalises(msg);
            abortChatThread = false;                // desactivation du variable de controle qui vérifie si le thread dois sortir de la boucle ou pas
            // si le text est plus grand que le taux autorisé, ce qui necessite plusieurs lignes
            // separation du message selon le caractere espace " "
            string[] splitedMsg = msg.Split(' ');
            string lineStr = string.Empty;                  // contien une ligne qui ne depasse pas ChatBulleLineMaxLength charactères, cette ligne s'ajoute au var data et se réinitialise
            string data = string.Empty;                     // message dans son entier
            int topLongLine = 0;                            // la longeur de la ligne la plus longue

            for (int cnt = 0; cnt < splitedMsg.Length; cnt++)
            {
                if (lineStr == "")
                {
                    lineStr = splitedMsg[cnt];
                    if (cnt + 1 == splitedMsg.Length)
                    {
                        data += lineStr;
                        if (topLongLine <= TextRenderer.MeasureText(lineStr, new Font("Verdana", 7)).Width)
                            topLongLine = TextRenderer.MeasureText(lineStr, new Font("Verdana", 7)).Width;
                    }
                }
                else if (lineStr.Length + splitedMsg[cnt].Length + 1 < ChatBulleLineMaxLength)
                {
                    lineStr += " " + splitedMsg[cnt];
                    if (cnt + 1 == splitedMsg.Length)
                    {
                        data += lineStr;
                        if (topLongLine <= TextRenderer.MeasureText(lineStr, new Font("Verdana", 7)).Width)
                            topLongLine = TextRenderer.MeasureText(lineStr, new Font("Verdana", 7)).Width;
                    }
                }
                else
                {
                    if (splitedMsg[cnt].Length > ChatBulleLineMaxLength)
                        lineStr += "...";
                    else
                    {
                        data += lineStr + "\n";
                        if (cnt >= splitedMsg.Length - 1)
                            data += splitedMsg[cnt];
                        if (topLongLine <= TextRenderer.MeasureText(lineStr, new Font("Verdana", 7)).Width)
                            topLongLine = TextRenderer.MeasureText(lineStr, new Font("Verdana", 7)).Width;
                        lineStr = splitedMsg[cnt];
                    }
                }

                // si on a eu une demande d'annulation  de thread se qui arrive quand un 2eme message est envoyé
                if (abortChatThread)
                    break;
            }

            // dessin du chatbulle
            Size s = new Size(topLongLine, (data.Split('\n').Length));

            Point p = new Point(Player.point.X - (s.Width / 2) + (Player.rectangle.Width / 2) + (s.Width / 10), Player.point.Y - (s.Height * 12) - 12);

            // check si le joueur se trouve sur les bords
            // verification horizonta
            if (p.X < 0)
                p.X = 0;

            if (p.X + s.Width > (ScreenManager.TilesWidth * 30))
                p.X = (ScreenManager.TilesWidth * 30) - s.Width;

            // verification vericale
            if (p.Y < 0)
            {
                // le joueur se trouve au somet du map, il faut afficher le message au dessous de lui
                p.Y = Player.point.Y + Player.rectangle.Height + 10;
            }

            Rec ChatBulleRec = new Rec(Brushes.White, p, new Size(s.Width - (s.Width / 7) + 4, (s.Height * 12) + 4), "__InfoBulleRec_" + (Player.tag as Actor).pseudo, Manager.TypeGfx.Obj, true);
            ChatBulleRec.zindex = 1500;
            Manager.manager.GfxObjList.Add(ChatBulleRec);
            
            // affichage du texte avec une marche sur les bord de 2 pixels
            Txt ChatBulleTxt = new Txt(data, new Point(2, 2), "__InfoBulleTxt", Manager.TypeGfx.Obj, true, new Font("Verdana", 7), Brushes.Black);
            ChatBulleRec.Child.Add(ChatBulleTxt);

            // affichage du triangle qui montre la provenance de chatbulle
            Bmp ChatBulleObj1 = new Bmp(@"gfx\general\obj\1\all1.dat", new Point((ChatBulleRec.size.Width / 2) - 6, ChatBulleRec.size.Height), "__ChatBulleObj1", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 11));
            ChatBulleRec.Child.Add(ChatBulleObj1);

            // pointeur vers l'image ChatBulleObj1
            (Player.tag as Actor).chatBulleRec = ChatBulleRec;

            // delais de 5 seconds avant la suppression de l'image
            Thread.Sleep(ChatBulleTimeOut);
            Manager.manager.GfxObjList.Remove(ChatBulleRec);
        }
        public static void CursorHand_MouseMove(Bmp bmp, MouseEventArgs e)
        {
            if (Battle.currentCursor == string.Empty)
                Manager.manager.mainForm.Cursor = Cursors.Hand;
        }
        public static void CursorDefault_MouseOut(Bmp bmp, MouseEventArgs e)
        {
            #region
            if (Battle.currentCursor == string.Empty)
                Manager.manager.mainForm.Cursor = Cursors.Default;
            else if(Battle.currentCursor == "spell")
            {
                // on supprime l'ancienne image du curseur qui peux etre changé quand on pas assez de pc et on survole un sort qui demande plus, du coup le curseur change en croix,
                // il faut donc remetre l'icone de spell au lieu de celle du croix
                if(Manager.manager.GfxTopList.Exists(f => f.Name() == "__spellCursor"))
                {
                    Bmp ___spellCursor = Manager.manager.GfxTopList.Find(f => f.Name() == "__spellCursor") as Bmp;
                    ___spellCursor.visible = false;
                    Manager.manager.GfxTopList.Remove(___spellCursor);
                }
                Bmp __spellCursor = new Bmp(@"gfx\general\obj\1\spellArrow.dat", new Point(-20, -20), "__spellCursor", Manager.TypeGfx.Top, true, 1);
                Manager.manager.GfxTopList.Add(__spellCursor);
                Manager.manager.mainForm.Cursor = new Cursor(__spellCursor.bmp.GetHicon());
            }
            #endregion
        }
        public static void DisposeIGfxAndChild(IGfx igfx)
        {
            #region
            // supression d'un IGfx et de ses enfants
            if (igfx != null)
            {
                if (igfx.GetType() == typeof(Bmp))
                {
                    Bmp b = igfx as Bmp;
                    b.visible = false;
                    b.Child.Clear();
                }
                else if (igfx.GetType() == typeof(Rec))
                {
                    Rec r = igfx as Rec;
                    r.visible = false;
                    r.Child.Clear();
                }
                else if (igfx.GetType() == typeof(Anim))
                {
                    Anim a = igfx as Anim;
                    a.visible(false);
                    a.Close();
                    a.Child.Clear();
                }

                // supression du parent
                if (igfx.GetType() == typeof(Bmp))
                    (igfx as Bmp).visible = false;
                else if (igfx.GetType() == typeof(Anim))
                {
                    (igfx as Anim).img.visible = false;
                    (igfx as Anim).Close();
                }
                else if (igfx.GetType() == typeof(Rec))
                    (igfx as Rec).visible = false;
                else if (igfx.GetType() == typeof(Txt))
                    (igfx as Txt).visible = false;

                Manager.manager.GfxBgrList.RemoveAll(f => f != null && f.Name() == igfx.Name());
                Manager.manager.GfxObjList.RemoveAll(f => f != null && f.Name() == igfx.Name());
                Manager.manager.GfxTopList.RemoveAll(f => f != null && f.Name() == igfx.Name());
            }
            #endregion
        }
        public static bool isBusy()
        {
            // check si le joueur est occupé par un menu comme le resultat d'un combat
            // menu resultat combat
            if (Manager.manager.GfxTopList.Exists(f => f.Name() == "__finalResultRecParent"))
                return true;
            else
                return false;
        }
        public static int CalculateElementLvl(int value)
        {
            #region
            // return le lvl par rapport au nombre de l'utilisation de l'element
            if (value < chakra1Level)
                return 1;
            else if (value < chakra2Level)
                return 2;
            else if (value < chakra3Level)
                return 3;
            else if (value < chakra4Level)
                return 4;
            else if (value < chakra5Level)
                return 5;
            else
                return 6;
            #endregion
        }
        public static void FocusPlayerByArrow()
        {
            #region
            // petite flech qui met le joueur en surbillance
            int cnt = 15;
            Bmp focusPlayerByArrowBmp = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__focusPlayerByArrowBmp", Manager.TypeGfx.Top, true, 1, SpriteSheet.GetSpriteSheet("_Main_option", 39));
            focusPlayerByArrowBmp.zindex = 1;
            Manager.manager.GfxTopList.Add(focusPlayerByArrowBmp);

            while (Battle.state == Enums.battleState.state.started)
            {
                Actor piib = Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn);
                Point p = new Point();
                p.X = piib.ibPlayer.point.X + ((piib.ibPlayer.rectangle.Width - 16) / 2);

                p.Y = piib.ibPlayer.point.Y;

                if (cnt == 0)
                    cnt = 15;
                focusPlayerByArrowBmp.point = new Point(p.X, p.Y - cnt - 15);
                cnt--;
                Thread.Sleep(80);
            }
            #endregion
        }
        public static void timeLeftForBattle()
        {
            #region
            // compte a rebour lors du temps de préparation du combat avant son début
            Battle.timeleft = 0;
            bool affected = false;
            while (!Manager.manager.mainForm.IsDisposed && Battle.state == Enums.battleState.state.initialisation)
            {
                if (Manager.manager.GfxTopList.Exists(f => f != null && f.GetType() == typeof(Bmp) && (f as Bmp).name == "Chrono_TimeOut"))
                {
                    Txt t = Battle.TimeLeftLabel;
                    //Battle.timeLeftLabel.zindex = ZOrder.Obj();
                    int MaxTimeLeft = Convert.ToInt32(t.tag as string);
                    if (!affected)
                    {
                        Battle.timeleft = MaxTimeLeft;
                        affected = true;
                    }
                    if (Battle.timeleft > 0 && Battle.state != Enums.battleState.state.closed)
                    {
                        t.Text = Battle.timeleft.ToString();
                        t.visible = true;
                        t.point.X = Battle.Chrono_TimeOut.point.X + ((Battle.Chrono_TimeOut.rectangle.Width - TextRenderer.MeasureText(t.Text, t.font).Width) / 2) + 2;
                        t.point.Y = Battle.Chrono_TimeOut.point.Y + 28;
                        Battle.timeleft--;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        // éffacement du compteur et sortir du thread
                        t.visible = false;
                        Manager.manager.GfxTopList.Remove(t);

                        Battle.Chrono_TimeOut.visible = false;
                        Manager.manager.GfxTopList.RemoveAll(r => r.GetType() == typeof(Bmp) && r.Name() == "Chrono_TimeOut");
                        
                        // effacement des positions valides dans la map
                        //validePosT1Rec et validePosT2Rec
                        List<IGfx> ValidePos = Manager.manager.GfxBgrList.FindAll(f => f.GetType() == typeof(Rec) && (f.Name() == "__validePosT1Rec" || f.Name() == "__validePosT2Rec"));
                        for (int cnt = 0; cnt < ValidePos.Count(); cnt++ )
                        {
                            (ValidePos[cnt] as Rec).visible = false;
                            Manager.manager.GfxBgrList.Remove(ValidePos[cnt]);
                        }
                        break;
                    }
                }
                else
                    break;
            }

            if (Manager.manager.GfxTopList.Exists(f => f.GetType() == typeof(Bmp) && f.Name() == "Chrono_TimeOut"))
            {
                IGfx Chrono_TimeOut = Manager.manager.GfxTopList.Find(f => f.GetType() == typeof(Bmp) && f.Name() == "Chrono_TimeOut");
                if(Chrono_TimeOut != null)
                    (Chrono_TimeOut as Bmp).visible = false;
            }
            #endregion
        }
        public static void map_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            #region
            // quand le joueur clic sur un endroit pour ce deplacer
            DelWayPointCallBack isFreeCell = bmp.tag as DelWayPointCallBack;
            // verification si le joueur est en mode battle ou non
            if (Battle.state == Enums.battleState.state.idle)
            {
                #region
                // verification si le joueur est deja en mode WayPoint Walk/Run
                Actor pi = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
                if (pi.animatedAction == Enums.AnimatedActions.Name.idle)
                {
                    // check si le joueur avais demandé un waypoint il ya 5 seconds pour eviter un spam
                    if (pi.timeBeforeNextWaypoint != 0 && CommonCode.ReturnTimeStamp() - pi.timeBeforeNextWaypoint < 2)
                        return;
                    else
                        pi.timeBeforeNextWaypoint = CommonCode.ReturnTimeStamp();

                    List<Point> wayPointList = new List<Point>();

                    MapPoint startPoint = new MapPoint(CommonCode.MyPlayerInfo.instance.ibPlayer.point.X / 30, (CommonCode.MyPlayerInfo.instance.ibPlayer.point.Y + CommonCode.MyPlayerInfo.instance.ibPlayer.rectangle.Height) / 30);
                    MapPoint endPoint = new MapPoint(e.Location.X / 30, e.Location.Y / 30);
                    byte[,] byteMap = new byte[ScreenManager.TilesWidth, ScreenManager.TilesHeight];
                    for (int i = 0; i < ScreenManager.TilesWidth; i++)
                        for (int j = 0; j < ScreenManager.TilesHeight; j++)
                        {
                            if (!isFreeCell(new Point(i * 30, j * 30)))
                                byteMap[i, j] = 3;
                            else
                                byteMap[i, j] = 0;
                        }

                    Map _map = new Map(ScreenManager.TilesWidth, ScreenManager.TilesHeight, startPoint, endPoint, byteMap);

                    if (_map != null && _map.StartPoint != MapPoint.InvalidPoint && _map.EndPoint != MapPoint.InvalidPoint)
                    {
                        AStar astart = new AStar(_map);
                        List<MapPoint> sol = astart.CalculateBestPath();
                        if (sol != null)
                            sol.Reverse();
                        else
                        {
                            // impossible de determiner le chemain, peux etre que la case ciblé est un obstacle
                            CommonCode.ChatMsgFormat("S", null, CommonCode.TranslateText(118));
                            return;
                        }
                        // conversion de la liste MapPoint a une liste Point
                        for (int i = 0; i < sol.Count; i++)
                            wayPointList.Add(new Point(sol[i].X * 30, sol[i].Y * 30));
                    }
                    ////////////////////////////////////////////////////////////////////
                    if (wayPointList.Count > 0)
                    {
                        string wayPointString = "";
                        for (int cnt = 0; cnt < wayPointList.Count; cnt++)
                            wayPointString += wayPointList[cnt].X + "," + wayPointList[cnt].Y + ':';

                        wayPointString = wayPointString.Substring(0, wayPointString.Length - 1);
                        pi.wayPoint = wayPointList;
                        WayPointConfirmationRequestMessage wayPointConfirmationRequestMessage = new WayPointConfirmationRequestMessage(wayPointString);
                        wayPointConfirmationRequestMessage.Serialize();
                        wayPointConfirmationRequestMessage.Send();
                    }
                }
                else
                {
                    // le joueur est deja en mouvement
                    // il faut informer le serveur de l'arret du joueur
                    WayPointInteruptedByActorRequestMessage WayPointInteruptedByUserRequestMessage = new WayPointInteruptedByActorRequestMessage();
                    WayPointInteruptedByUserRequestMessage.Serialize();
                    WayPointInteruptedByUserRequestMessage.Send();
                }
                #endregion
            }
            else if (Battle.state == Enums.battleState.state.started)
            {
                #region
                // check si le joueur avais cliqué sur un sort si oui on remet le curseur, si non on lance le déplacement du joueur
                if (Battle.currentCursor == "")
                {
                    // le joueur est en combat
                    // verification si le joueur a la main pour jouer
                    if (MyPlayerInfo.instance.pseudo == Battle.PlayerTurn)
                    {
                        // check si le joueur est libre ou est entrain de bouger
                        if (Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).animatedAction == Enums.AnimatedActions.Name.idle)
                        {
                            // determination du waypoint
                            ////////////////////////////////////////////////////////
                            List<Point> wayPointList = new List<Point>();
                            MapPoint startPoint = new MapPoint(MyPlayerInfo.instance.ibPlayer.point.X / 30, (MyPlayerInfo.instance.ibPlayer.point.Y + CommonCode.MyPlayerInfo.instance.ibPlayer.rectangle.Height) / 30);
                            MapPoint endPoint = new MapPoint(e.Location.X / 30, e.Location.Y / 30);
                            byte[,] byteMap = new byte[ScreenManager.TilesWidth, ScreenManager.TilesHeight];
                            for (int i = 0; i < ScreenManager.TilesWidth; i++)
                                for (int j = 0; j < ScreenManager.TilesHeight; j++)
                                {
                                    if (!isFreeCell(new Point(i * 30, j * 30)))
                                        byteMap[i, j] = 3;
                                    else
                                        byteMap[i, j] = 0;
                                }

                            Map _map = new Map(ScreenManager.TilesWidth, ScreenManager.TilesHeight, startPoint, endPoint, byteMap);

                            if (_map != null && _map.StartPoint != MapPoint.InvalidPoint && _map.EndPoint != MapPoint.InvalidPoint)
                            {
                                AStar astart = new AStar(_map);
                                List<MapPoint> sol = astart.CalculateBestPath();
                                if (sol != null)
                                    sol.Reverse();
                                else
                                {
                                    // impossible de determiner le chemain, peux etre que la case ciblé est un obstacle
                                    CommonCode.ChatMsgFormat("S", null, CommonCode.TranslateText(118));
                                    return;
                                }
                                // conversion de la liste MapPoint a une liste Point
                                for (int i = 0; i < sol.Count; i++)
                                    wayPointList.Add(new Point(sol[i].X * 30, sol[i].Y * 30));
                            }

                            ////////////////////////////////////////////////////////
                            if (wayPointList.Count > 0)
                            {
                                string wayPointString = "";
                                for (int cnt = 0; cnt < wayPointList.Count; cnt++)
                                    wayPointString += wayPointList[cnt].X + "," + wayPointList[cnt].Y + ':';
                                wayPointString = wayPointString.Substring(0, wayPointString.Length - 1);

                                Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).wayPoint = wayPointList;
                                WayPointConfirmationRequestMessage wayPointConfirmationRequestMessage = new WayPointConfirmationRequestMessage(wayPointString);
                                wayPointConfirmationRequestMessage.Serialize();
                                wayPointConfirmationRequestMessage.Send();
                            }
                        }
                    }
                }
                else
                {
                    Battle.currentCursor = "";
                    CommonCode.CursorDefault_MouseOut(null, null);
                    Bmp __spellCursor = Manager.manager.GfxTopList.Find(f => f.Name() == "__spellCursor") as Bmp;
                    if (__spellCursor != null)
                    {
                        __spellCursor.visible = false;
                        Manager.manager.GfxTopList.Remove(__spellCursor);
                        Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles");

                        IGfx __clone_jutsu = Manager.manager.GfxObjList.Find(f => f.Name() == "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto");
                        if (__clone_jutsu != null)
                        {
                            (__clone_jutsu as Bmp).visible = false;
                            Manager.manager.GfxObjList.Remove(__clone_jutsu);
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
        public static void map_MouseMove(Bmp bmp, MouseEventArgs e)
        {
            #region
            DelWayPointCallBack isFreeCell = bmp.tag as DelWayPointCallBack;
            // methode commune a tous les map qui dois génerer le déplacement des joueurs en combat et autre choses
            if (Battle.state == Enums.battleState.state.idle && Battle.currentCursor == string.Empty)
            {
                if (isFreeCell(e.Location))
                    CursorHand_MouseMove(bmp, e);
                else
                    CursorDefault_MouseOut(null, null);
            }
            else if (Battle.state == Enums.battleState.state.started && Battle.currentCursor == string.Empty)
            {
                if (isFreeCell(e.Location))
                    CursorHand_MouseMove(bmp, e);
                else
                    CursorDefault_MouseOut(null, null);
            }

            // on dessine le chemain, si on ai on combat, si on ai en statut Started, si on a la main, si on a pas lancé un sort,si on ai pas en action préalablement (mouvement ou autre anim)
            if (Battle.state == Enums.battleState.state.started && Battle.PlayerTurn == MyPlayerInfo.instance.pseudo && Battle.currentCursor == "" && Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).animatedAction == Enums.AnimatedActions.Name.idle && !blockNetFlow)
            {
                // effacement de tous les chemain tracés avant
                if (Manager.manager.GfxBgrList.FindAll(f => f.GetType() == typeof(Rec) && f.Name() == "__wayPointRec").Count > 0)
                {
                    (Manager.manager.GfxBgrList.Find(f => f.GetType() == typeof(Rec) && f.Name() == "__wayPointRec") as Rec).visible = false;
                    (Manager.manager.GfxBgrList.Find(f => f.GetType() == typeof(Rec) && f.Name() == "__wayPointRec") as Rec).Child.Clear();
                    Manager.manager.GfxBgrList.RemoveAll(f => f.GetType() == typeof(Rec) && f.Name() == "__wayPointRec");
                }
                
                // generer un waypoint
                //////////////////////////////////////
                List<Point> wayPointList = new List<Point>();
                MapPoint startPoint = new MapPoint(Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).realPosition.X, Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).realPosition.Y);
                MapPoint endPoint = new MapPoint(e.Location.X / 30, e.Location.Y / 30);
                byte[,] byteMap = new byte[ScreenManager.TilesWidth, ScreenManager.TilesHeight];
                for (int i = 0; i < ScreenManager.TilesWidth; i++)
                    for (int j = 0; j < ScreenManager.TilesHeight; j++)
                    {
                        if (!isFreeCell(new Point(i * 30, j * 30)))
                            byteMap[i, j] = 3;
                        else
                            byteMap[i, j] = 0;
                    }

                Map _map = new Map(ScreenManager.TilesWidth, ScreenManager.TilesHeight, startPoint, endPoint, byteMap);

                if (_map != null && _map.StartPoint != MapPoint.InvalidPoint && _map.EndPoint != MapPoint.InvalidPoint)
                {
                    AStar astart = new AStar(_map);
                    List<MapPoint> sol = astart.CalculateBestPath();
                    if (sol != null)
                        sol.Reverse();
                    else
                    {
                        // impossible de determiner le chemain, peux etre que la case ciblé est un obstacle
                        return;
                    }
                    // conversion de la liste MapPoint a une liste Point
                    for (int i = 0; i < sol.Count; i++)
                        wayPointList.Add(new Point(sol[i].X * 30, sol[i].Y * 30));
                }

                //////////////////////////////////////
                int pm2 = Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).currentPm;
                if (wayPointList.Count > 0 && wayPointList.Count <= Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).currentPm)
                {
                    // check si notre waypoint se croise avec l'une des position des joueur dans le combat
                    bool blocked = false;           // variable qui nous permet de savoir si un joueur tacle une case pour que tous les autres cases soit en rouge

                    ////////////////////////////////////////////////////////////

                    // variable qui nous permet de savoir si un joueur tacle une case pour que tous les autres cases soit en rouge
                    Brush brushes = Brushes.Green;

                    // premiere comparésons avec la position actuel
                    ///////////////////////////////////////////////////////////
                    // check contre le blocage pour la case actuel
                    // check si le rectangle en cours est entouré par un joueur ou invoc
                    // tourné sur tous les objets et joueur dans la liste
                    int cumuleBlocage1 = 0;
                    Point[] validePos1 = new Point[4];
                    Point curPlayerPos1 = Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).realPosition;
                    validePos1[0] = new Point(curPlayerPos1.X , curPlayerPos1.Y - 1);    // case en haut
                    validePos1[1] = new Point(curPlayerPos1.X + 1, curPlayerPos1.Y);    // case a droite
                    validePos1[2] = new Point(curPlayerPos1.X, curPlayerPos1.Y + 1);    // case en bas
                    validePos1[3] = new Point(curPlayerPos1.X - 1, curPlayerPos1.Y);    // case a gauche

                    // check si un joueur se trouve sur l'une des cases cités en haut
                    for (int cnt1 = 0; cnt1 < Battle.AllPlayersByOrder.Count; cnt1++)
                    {
                        //Enums.Team.Side tmp = Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).team;
                        if (Battle.AllPlayersByOrder[cnt1].teamSide != Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).teamSide && Battle.AllPlayersByOrder[cnt1].visible)
                        {
                            Actor pi = Battle.AllPlayersByOrder[cnt1];
                            Point p = new Point(Battle.AllPlayersByOrder[cnt1].realPosition.X, Battle.AllPlayersByOrder[cnt1].realPosition.Y);
                            if ((p.X == validePos1[0].X && p.Y == validePos1[0].Y) || (p.X == validePos1[1].X && p.Y == validePos1[1].Y) || (p.X == validePos1[2].X && p.Y == validePos1[2].Y) || (p.X == validePos1[3].X && p.Y == validePos1[3].Y))
                                cumuleBlocage1 += pi.blocage;
                        }
                    }
                    bool alreadyBlocked = false;
                    if (cumuleBlocage1 > Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).escape + 10)
                    {
                        brushes = Brushes.Red;
                        alreadyBlocked = true;
                    }
                    ////////////////////////////////////////////////////////////

                    bool FirstTuileBlock = false;     // variable de contrôle pour determiner si le joueur sera taclé depuis la 1ere tuile dans le waypoint
                    // malgré que le controle en haut le verifie mais ce controle n'est pas pris en consideration tout seul puisque quand le joueur
                    // sera taclé depuis la 2eme tuiles, elle va colorer la 1ere tuile en rouge pour dir que ce chemain n'est pas valide
                    // alors il faut un variable de controle pour empecher que la 1ere tuile se met en rouge

                    // tracage du chemain
                    for (int cntWayPoint = 0; cntWayPoint < wayPointList.Count(); cntWayPoint++)
                    {
                        if (!isFreeCell(new Point(wayPointList[cntWayPoint].X, wayPointList[cntWayPoint].Y)))
                            break;
                        else
                        {
                            // check si le rectangle en cours est entouré par un joueur ou invoc
                            // tourné sur tous les objets et joueur dans la liste
                            if (!alreadyBlocked)
                            {
                                int cumuleBlocage = cumuleBlocage1;

                                Point[] validePos = new Point[4];
                                validePos[0] = new Point(wayPointList[cntWayPoint].X, wayPointList[cntWayPoint].Y - 30);    // case en haut
                                validePos[1] = new Point(wayPointList[cntWayPoint].X + 30, wayPointList[cntWayPoint].Y);    // case a droite
                                validePos[2] = new Point(wayPointList[cntWayPoint].X, wayPointList[cntWayPoint].Y + 30);    // case en bas
                                validePos[3] = new Point(wayPointList[cntWayPoint].X - 30, wayPointList[cntWayPoint].Y);    // case a gauche

                                for (int cnt = 0; cnt < Battle.AllPlayersByOrder.Count; cnt++)
                                {
                                    if (Battle.AllPlayersByOrder[cnt].teamSide != Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).teamSide && Battle.AllPlayersByOrder[cnt].visible)
                                    {
                                        Actor piib = Battle.AllPlayersByOrder[cnt];
                                        try
                                        {
                                            Point p = new Point((piib.ibPlayer.point.X - 15 + piib.ibPlayer.rectangle.Width), piib.ibPlayer.point.Y + piib.ibPlayer.rectangle.Height);
                                            p.X = (p.X / 30) * 30;
                                            p.Y = (p.Y / 30) * 30;
                                            if (p == validePos[0] || p == validePos[1] || p == validePos[2] || p == validePos[3])
                                                cumuleBlocage += piib.blocage;
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }

                                brushes = Brushes.Red;
                                // determination si le cumuleBlocage est plus grand que celui de notre joueur, on ajoute 10 points d'evasion a notre joueur comme tolerance
                                if (cumuleBlocage > Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).escape + 10)
                                {
                                    if (FirstTuileBlock)
                                    {
                                        brushes = Brushes.Red;
                                        blocked = true;
                                    }
                                    else
                                    {
                                        FirstTuileBlock = true;
                                        brushes = Brushes.Green;
                                        blocked = true;
                                    }
                                }
                                else if (!blocked)
                                    brushes = Brushes.Green;
                            }

                            // check qui surviens 1 seul fois, pour eviter que la 1ere tuile qui suit un chemain taclé ne se met pas en rouge
                            
                            Rec wayPointRec = new Rec(brushes, new Point(wayPointList[cntWayPoint].X + 1, wayPointList[cntWayPoint].Y + 1), new Size(28, 28), "__wayPointRec", Manager.TypeGfx.Bgr, true);
                            wayPointRec.tag = isFreeCell;
                            wayPointRec.MouseClic += wayPointRec_MouseClic;
                            wayPointRec.MouseMove += wayPointRec_MouseMove;
                            wayPointRec.EscapeGfxWhileMouseClic = true;
                            Manager.manager.GfxBgrList.Add(wayPointRec);

                            Txt wayPointTxt = new Txt((cntWayPoint + 1).ToString(), new Point(0, 10), "__waypointTxt", Manager.TypeGfx.Bgr, true, new Font("verdana", 6), Brushes.Black);
                            wayPointTxt.point.X = 15 - (TextRenderer.MeasureText(wayPointTxt.Text, wayPointTxt.font).Width / 2);
                            wayPointRec.Child.Add(wayPointTxt);
                        }
                    }
                }
            }
            #endregion
        }
        public static void envoutementCheck(Actor pi, Enums.Buff.State buffState)
        {
            // traitement pour les envoutement en cours
            // ATTENTION, si une modification est faite sur ce code, il faut le faire aussi sur la methode RedrawSpellsWithCtrl()
            Actor currentPlayer = Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn);
            if (pi.pseudo != currentPlayer.pseudo)
                MessageBox.Show("what a fuck");
            // remettre les sorts tous accessible pour griser seulemement ceux en envoutements
            if (pi.pseudo == MyPlayerInfo.instance.pseudo && buffState == Enums.Buff.State.Fin)
            {
                // réactivation de tous les sorts avant des les traiter
                List<Actor.SpellsInformations> iSort = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).spells;
                for (int cnt = 0; cnt < iSort.Count; cnt++)
                {
                    // pointeur vers l'image du sort
                    Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f != null && f.GetType() == typeof(Bmp) && (f as Bmp).tag != null && f.Tag().GetType() == typeof(Actor.SpellsInformations) && (f.Tag() as Actor.SpellsInformations).sortID == iSort[cnt].sortID));
                    Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;
                    // changement de l'image de sort en image en normale "accessible"
                    spellIcon.ChangeBmp(@"gfx\general\obj\1\spells.dat", SpriteSheet.GetSpriteSheet(spellIS.sortID + "_spell", 0));
                }
            }

            // passage sur tous les envoutements pour les verifier
            for (int cnt = pi.BuffsList.Count; cnt > 0; cnt--)
            {
                int SortID = pi.BuffsList[cnt - 1].SortID;

                if (buffState == Buff.State.Fin && pi.BuffsList[cnt - 1].BuffState == Buff.State.Fin && pi.BuffsList[cnt - 1].relanceInterval > 0)
                {
                    // traitement unique pour les envoutements systemes, décrementations des compteurs et changement des images activer ou desactiver
                    if (pi.BuffsList[cnt - 1].systeme)
                    {
                        pi.BuffsList[cnt - 1].relanceInterval--;

                        if (pi.BuffsList[cnt - 1].relanceInterval > 0)
                        {
                            // interval de relance non atteint
                            // si c'est notre personnage on reverifie la disponibilité des sort
                            if (pi.pseudo == MyPlayerInfo.instance.pseudo)
                            {
                                // decrementation de l'index affiché sur le sort
                                // pointeur vers l'image du sort lancé sur le tableau des sort
                                Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == pi.BuffsList[cnt - 1].SortID));

                                Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;
                                // sort pas encore accessible 
                                Txt relanceInterval_sortID = (Txt)(HudHandle.all_sorts.Child.Find(f => f.Name() == "relanceInterval_" + pi.BuffsList[cnt - 1].SortID));
                                relanceInterval_sortID.Text = pi.BuffsList[cnt - 1].relanceInterval.ToString();

                                // centrage
                                relanceInterval_sortID.point = new Point(spellIcon.point.X + (spellIcon.rectangle.Width / 2) - (TextRenderer.MeasureText(relanceInterval_sortID.Text, relanceInterval_sortID.font).Width / 2), spellIcon.point.Y + (spellIcon.rectangle.Height / 2) - (TextRenderer.MeasureText(relanceInterval_sortID.Text, relanceInterval_sortID.font).Height / 2));
                                // changement de l'image du sort en image grisé "non accessible"
                                spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(spellIS.sortID + "_spell", 0));
                            }
                            /////////////////// fin traitement specifique a notre personnage
                        }
                        else
                        {
                            // interval de relance atteint
                            // traitement pour notre personnage a fin de réactiver les sorts inacessibles auparavant
                            if (pi.pseudo == CommonCode.MyPlayerInfo.instance.pseudo)
                            {
                                // decrementation de l'index affiché sur le sort
                                // pointeur vers l'image du sort lancé sur le tableau des sort
                                Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == pi.BuffsList[cnt - 1].SortID));

                                Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;
                                // sort accessible
                                // changement de l'image de sort en image en normale "accessible"
                                spellIcon.ChangeBmp(@"gfx\general\obj\1\spells.dat", SpriteSheet.GetSpriteSheet(pi.BuffsList[cnt - 1].SortID + "_spell", 0));

                                // effassement du compteur de relanceInterval
                                HudHandle.all_sorts.Child.RemoveAll(f => f.Name() == "relanceInterval_" + pi.BuffsList[cnt - 1].SortID);
                            }
                            ///////////// fin de traitement sur notre personnage

                            // traitement special sur les sort comme annuler un boost ou un malus ...
                            pi.BuffsList.RemoveAt(cnt - 1);
                        }
                    }
                    else
                    {
                        /////////////// traitement specifique a un envoutement non systeme d'un sort comme decrementer l'index de tours de bonus / malus
                        if (pi.BuffsList[cnt - 1].BonusRoundLeft > 0)
                        {
                            // interval de relance non atteint
                            // lister les conditions de tous les envoutement qui n'ont pas été achevés
                            if (spells.sort_de_bonnus.Exists(f => f == SortID))
                            {
                                // decrementation des tours restant pour le bonus
                                pi.BuffsList[cnt - 1].BonusRoundLeft--;
                            }
                            // on cherche si l'envoutement est affiché sur la timeline
                            // il se trouve dans un Txt child d'une image Bmp sous forme d'un triangle
                            if (Battle.ShowEnvoutement)
                            {
                                string str = "BonusRoundLeftBmp_" + SortID;
                                IGfx BonusRoundLeftBmp = Manager.manager.GfxTopList.Find(f => f.Name() == str);
                                if (BonusRoundLeftBmp != null)
                                {
                                    IGfx child = (BonusRoundLeftBmp as Bmp).Child.Find(f => f.Name().Substring(0, 15) == "BonusRoundLeft_");
                                    (child as Txt).Text = pi.BuffsList[cnt - 1].BonusRoundLeft.ToString();
                                }
                            }
                        }
                        else
                        {
                            // interval de relance atteint
                            // lister les conditions de tous les envoutements qui viens de se terminé
                            if (SortID == 7)
                            {
                                // sort qui ajouté 2 PC, donc on les soustraits
                                //pour verifier le lvl du sort sorts.sort(SortID).isbl[currentPlayer.sorts.Find(f => f.sortID == SortID).lvl - 1]
                                pi.originalPc -= 2;
                                pi.currentPc -= 2;
                                pi.BuffsList.RemoveAt(cnt - 1);
                            }
                            else if (SortID == 8)
                            {
                                // sort qui ajouté 2 PM, donc on les soustraits
                                //pour verifier le lvl du sort sorts.sort(SortID).isbl[currentPlayer.sorts.Find(f => f.sortID == SortID).lvl - 1]
                                pi.originalPm -= 2;
                                pi.currentPm -= 2;
                                pi.BuffsList.RemoveAt(cnt - 1);
                            }
                            else if (SortID == 9)
                            {
                                // sort qui ajouté la puissance "tous les elements", donc on les soustraits
                                pi.power -= spells.sort(SortID).isbl[pi.spells.Find(f => f.sortID == SortID).level - 1].piBonus.power;
                                pi.BuffsList.RemoveAt(cnt - 1);
                            }
                            else if (SortID == 10)
                            {
                                // sort qui ajouté la puissance "tous les elements", donc on les soustraits
                                pi.doton -= spells.sort(SortID).isbl[pi.spells.Find(f => f.sortID == SortID).level - 1].piBonus.doton;
                                pi.BuffsList.RemoveAt(cnt - 1);
                            }
                            // supression des images de l'envoutement dans la timeline si Battle.ShowEnvoutement = true
                            if (Battle.ShowEnvoutement)
                            {
                                // supression de l'image de sort de l'envoutement affiché
                                Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "SpellIcon1_" + SortID);

                                // supression de l'image triangle du bonus
                                Manager.manager.GfxTopList.RemoveAll(f => f.Name() == "BonusRoundLeftBmp_" + SortID);
                            }
                        }
                    }
                }
                else if (buffState == Buff.State.Debut && pi.BuffsList[cnt - 1].BuffState == Buff.State.Debut && pi.BuffsList[cnt - 1].relanceInterval > 0)
                {
                    // traitement specifique au début du tours et interval de relance non atteint
                    // comme check si les sorts d'invocation ne sont pas accessible suite a un manque de point d'invocation, ou les pa (pas encore introduit)
                }
                else if (buffState == Buff.State.Debut && pi.BuffsList[cnt - 1].BuffState == Buff.State.Debut && pi.BuffsList[cnt - 1].relanceInterval == 0)
                {
                    // traitement specifique au début du tours et interval de relance atteint
                }
            }

            // traitement spécifique à des sorts qui n'ont pas besoin d'un envoutement pour être declanché, comme des sort qui nécessite un état précis, comme Etat Sennin qui débloque le sort Rasen Shuriken
            // on passe sur tous les sorts pour les verifier
            if (pi.pseudo == MyPlayerInfo.instance.pseudo)
            {
                for (int cnt = 0; cnt < pi.spells.Count; cnt++)
                {
                    Actor.SpellsInformations piis = pi.spells[cnt];
                    if (spells.sort_need_etat_sennin.Exists(f => f == piis.sortID))
                    {
                        // sort qui necessite le mode Sennin, on verifie si le joueur à l'envoutement du mode sennin
                        if (!pi.BuffsList.Exists(f => f.StateList.Exists(e => e == Buff.Name.Senin)))
                        {
                            // le client n'est pas en mode Sennin, on désactive tous les sorts dépandant
                            // check si le joueur à le sort futon rasen shuriken
                            Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == piis.sortID));
                            spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(piis.sortID + "_spell", 0));
                        }
                    }
                    else if(spells.sort_d_invocation.Exists(f => f == piis.sortID))
                    {
                        // sort qui necessite un point d'invocation, on verifie le nombre de point d'invocation que dispose le joueur
                        int sumOfInvoc = Battle.AllPlayersByOrder.FindAll(f => f.pseudo.IndexOf(MyPlayerInfo.instance.pseudo + "$") != -1).Count;
                        if (Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).summons <= sumOfInvoc)
                        {
                            Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == piis.sortID));
                            spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(piis.sortID + "_spell", 0));
                        }
                    }
                }
            }
        }
        public static void turnPast(string player, bool increment)
        {
            #region passer la main au joueur suivant si increment = true, si non la main reste pour le joueur player
            // le serveur nous demande de passer la main au joueur suivant
            // cmd[2] contien le nom du joueur qui a la main
            bool redrawSpell = false;               // variable de contrôl pour redessiner les sort si un changement a eu lieu comme un sort si il vien de se réactivé, tour de relance ...

            // des fois sa bug quand un client quite le combat prématurément, et qu'un autre client recois la demande de passer la main
            // et dans le meme timing il recois la cmd d'annulation du combat, du coup la liste Battle.AllPlayersByOrde devients vide et bug en bas
            if (Battle.AllPlayersByOrder.Count == 0)
                return;

            // reinitialisation des states pc2 et pm2 du joueur actuel 
            Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn).currentPc = Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn).originalPc;
            Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn).currentPm = Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn).originalPm;

            // check des envoutements du fin de tour, appliquer ceux de ce tour et decrementer les autre en cours
            envoutementCheck(Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn), Buff.State.Fin);
            if (Battle.PlayerTurn == MyPlayerInfo.instance.pseudo)
                redrawSpell = true;
                //RedrawSpellsWithCtrl();         // 1er appel a cette methode

            //passer la main
            if (increment)
            {
                // incrementation de la main
                int tmpHand = Battle.AllPlayersByOrder.FindIndex(f => f.pseudo == Battle.PlayerTurn);
                tmpHand++;

                // on verifie si l'index incrementé ne dépasse pas le nombre des joueurs
                if (tmpHand == Battle.AllPlayersByOrder.Count)
                    tmpHand = 0;

                Battle.PlayerTurn = Battle.AllPlayersByOrder[tmpHand].pseudo;
            }
            // check des envoutements du début de tour
            envoutementCheck(Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn), Buff.State.Debut);
            if (Battle.PlayerTurn == MyPlayerInfo.instance.pseudo)
                redrawSpell = true;         // 2eme appel a cette methode

            // controle pour voir si le joueur qui a la main sur le serveur est le meme chez le client, normalement ils doivent etre synchronisé
            if (Battle.PlayerTurn != player)
                ChatMsgFormat("S", "null", "erreur, le joueur qui dois avoir la main n'est pas celui sur le serveur [" + player + "]");

            // remise a zero du timer pour le tour actuel
            Battle.TimeLeftToPlay = Battle.TimeToPlayInBattle;
            if (Battle.TimeLeftToPlayT.Enabled == true)
                Battle.TimeLeftToPlayT.Stop();

            Battle.TimeLeftToPlayT.Start();
            // check si le prochain joueur est vraiment celui dis par le serveur, si non, il faut demander au serveur de nous envoyer la nouvelle liste maj
            if (Battle.PlayerTurn == player)
            {
                // check si on a la main
                if (Battle.PlayerTurn == MyPlayerInfo.instance.pseudo)
                {
                    // on a la main
                    // clignotement de notre fenetre + bouton dans la bar de tache si notre fenetre n'est pas active
                    Flash(3);
                }
                else
                {
                    // la main est a un autre joueur
                    if (Battle.currentCursor != "")
                    {
                        Battle.currentCursor = "";
                        CursorDefault_MouseOut(null, null);
                        Bmp __spellCursor = Manager.manager.GfxTopList.Find(f => f.Name() == "__spellCursor") as Bmp;

                        __spellCursor.visible = false;
                        Manager.manager.GfxTopList.Remove(__spellCursor);
                        Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles");

                        // supression des tuiles supplémentaires pour les sorts de zones
                        Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles_child");

                        IGfx __clone_jutsu = Manager.manager.GfxObjList.Find(f => f.Name() == "__clone_jutsu_" + MyPlayerInfo.instance.pseudo + "_naruto");
                        if (__clone_jutsu != null)
                        {
                            Bmp bmp = __clone_jutsu as Bmp;
                            if (bmp != null) bmp.visible = false;
                            Manager.manager.GfxObjList.Remove(__clone_jutsu);
                        }
                    }
                }
            }
            else
            {
                // le nom de celui qui dois avoir la main selon le serveur ne correspond pas a celui dans notre liste, il faut demander la nouvelle liste de joueurs
                MessageBox.Show("le joueur qui dois avoir la main ne correspond pas a celui sur le serveur");
            }

            // changer l'emplacement du cadre qui encercle la cellule de l'avatar du joueur
            Battle.UpdateSelectedPlayerInBattle();

            // effacement de tous les instance du lancement du sort
            // reinitialisation du stat du sort en instance
            Battle.infos_sorts = null;
            Battle.currentCursor = "";

            // changement du curseur a son état initiale
            CursorHand_MouseMove(null, null);

            // effacement de tous les images du sort
            Manager.manager.GfxBgrList.RemoveAll(f => f.Name() == "__spellTuiles");

            // check si on a la main, si oui on cheque les sorts accessibles qui ont assez de pc pour etre lancé, ou pour les sort d'invocations si on a les points necessaires
            // on passe sur tous les sorts pour checker leur pc / leurs points d'invoc s'il sagit d'un sort d'invocatio, il faut lister tous les sorts d'invocation sur la condition en bas par contre
            if (Battle.PlayerTurn == MyPlayerInfo.instance.pseudo)
                redrawSpell = true;         // 3eme appel a cette methode
            
            // mise a jour des states du clients ET NON DU JOUEUR EN COURS, les pc et pm sont modifié dans les methodes qui recoivents les cmd de mouvement ou lors de l'utilisation des pc
            HudHandle.UpdatePc();
            HudHandle.UpdatePm();

            if(redrawSpell)
                RedrawSpellsWithCtrl();
            #endregion
        }
        public static void ChatAreaCursorInTheEnd()
        {
            // mettre le curseur sur la fin du chatbox
            if (HudHandle.ChatCursorDown.Checked)
            {
                RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;
                ChatArea.SelectionLength = 0;
                ChatArea.SelectionStart = ChatArea.TextLength;
                ChatArea.ScrollToCaret();
                ChatArea.SelectionLength = 0;
                ChatArea.SelectionStart = ChatArea.TextLength;
                ChatArea.ScrollToCaret();
            }
        }
        public static void RedrawSpellsWithCtrl()
        {
            // ATTENTION cette methode dois contenir les même contrôles de la methode envoutementCheck
            // reafficher les sorts avec leurs etat grisé ou non avec le nombre de relance
            // réactivation de tous les sorts avant des les traiter

            List<Actor.SpellsInformations> iSort = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).spells;
            for (int cnt = 0; cnt < iSort.Count; cnt++)
            {
                // pointeur vers l'image du sort
                
                Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f != null && f.GetType() == typeof(Bmp) && (f as Bmp).tag != null && f.Tag().GetType() == typeof(Actor.SpellsInformations) && (f.Tag() as Actor.SpellsInformations).sortID == iSort[cnt].sortID));
                Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;
                // changement de l'image de sort en image en normale "accessible"
                spellIcon.ChangeBmp(@"gfx\general\obj\1\spells.dat", SpriteSheet.GetSpriteSheet(spellIS.sortID + "_spell", 0));
            }

            // supression des indexeurs d'envoutement
            HudHandle.all_sorts.Child.RemoveAll(f => f.Name().Length > 16 && f.Name().Substring(0, 16) == "relanceInterval_");

            // check si on a la main, si oui on cheque les sorts accessibles qui ont assez de pc pour etre lancé, ou pour les sort d'invocations si on a les points necessaires
            // on passe sur tous les sorts pour checker leur pc / leurs points d'invoc s'il sagit d'un sort d'invocatio, il faut lister tous les sorts d'invocation sur la condition en bas par contre
            Actor pi = Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo);
            for (int cnt = 0; cnt < pi.spells.Count(); cnt++)
            {
                int SortID = pi.spells[cnt].sortID;
                int lvl = pi.spells[cnt].level;

                // pointeur vers l'image du sort lancé sur le tableau des sort
                Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == SortID));

                if (Battle.AllPlayersByOrder.Find(f => f.pseudo == Battle.PlayerTurn).currentPc < spells.sort(SortID).isbl[lvl - 1].pi.originalPc)
                {
                    // pas assez de pc, on grise le sort
                    // changement de l'image du sort en image grisé "non accessible" pour manque de PC
                    spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(SortID + "_spell", 0));
                }
                else if (spells.sort_d_invocation.Exists(f => f == SortID))
                {
                    // check pour les sorts d'invocation si on a assez de point d'invocation apres avoir compté les invocs du joueur
                    // il faut ajouter le id de tous les sorts d'invocation sur cette condition
                    // cheque si le joueur a assez de points d'invocation pour invoquer
                    // comptage des invocs sur la map qui nous appartiens
                    int sumOfInvoc = Battle.AllPlayersByOrder.FindAll(f => f.pseudo.IndexOf(MyPlayerInfo.instance.pseudo + "$") != -1).Count;
                    if (Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).summons <= sumOfInvoc)
                    {
                        spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(SortID + "_spell", 0));
                    }
                }

                if (pi.BuffsList.Exists(f => f.relanceInterval > 0 && f.SortID == SortID && f.systeme))
                {
                    // interval de relance non atteint
                    Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;
                    Actor.Buff piev = pi.BuffsList.Find(f => f.relanceInterval > 0 && f.SortID == SortID && f.systeme);
                    // sort pas encore accessible 
                    Txt relanceInterval_sortID = new Txt(piev.relanceInterval.ToString(), Point.Empty, "relanceInterval_" + SortID, Manager.TypeGfx.Top, true, new Font("Verdana", 9, FontStyle.Bold), Brushes.White);

                    // centrage
                    relanceInterval_sortID.point = new Point(spellIcon.point.X + (spellIcon.rectangle.Width / 2) - (TextRenderer.MeasureText(relanceInterval_sortID.Text, relanceInterval_sortID.font).Width / 2), spellIcon.point.Y + (spellIcon.rectangle.Height / 2) - (TextRenderer.MeasureText(relanceInterval_sortID.Text, relanceInterval_sortID.font).Height / 2));
                    HudHandle.all_sorts.Child.Add(relanceInterval_sortID);

                    spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(spellIS.sortID + "_spell", 0));
                    /////////////////// fin traitement specifique a notre personnage
                }
                else if(pi.BuffsList.Exists(f => f.SortID == SortID && f.relanceParTour <= f.playerRoxed.Count && f.systeme))
                {
                    // relance par tour maximum atteint
                    Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;
                    spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(spellIS.sortID + "_spell", 0));
                }
            }

            // traitement spécifique des sorts qui ont pas besoin d'un envoutement pour être declanché, comme des sort qui nécessite un état précis, comme Etat Sennin qui débloque le sort Rasen Shuriken
            // on passe sur tous les sorts pour les verifier
            for (int cnt = 0; cnt < pi.spells.Count; cnt++)
            {
                Actor.SpellsInformations piis = pi.spells[cnt];
                if (spells.sort_need_etat_sennin.Exists(f => f == piis.sortID))
                {
                    // sort qui necessite le mode Sennin, on verifie si le joueur à l'nvoutement du mode sennin
                    if (!pi.BuffsList.Exists(f => f.StateList.Exists(e => e == Buff.Name.Senin)))
                    {
                        // le client n'est pas en mode Sennin, on désactive tous les sorts dépandant
                        // check si le joueur à le sort futon rasen shuriken
                        Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == piis.sortID));
                        spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(piis.sortID + "_spell", 0));
                    }
                }
                else if (spells.sort_d_invocation.Exists(f => f == piis.sortID))
                {
                    // sort qui necessite un point d'invocation, on verifie le nombre de point d'invocation que dispose le joueur
                    int sumOfInvoc = Battle.AllPlayersByOrder.FindAll(f => f.pseudo.IndexOf(MyPlayerInfo.instance.pseudo + "$") != -1).Count;
                    if (Battle.AllPlayersByOrder.Find(f => f.pseudo == MyPlayerInfo.instance.pseudo).summons <= sumOfInvoc)
                    {
                        Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == piis.sortID));
                        spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(piis.sortID + "_spell", 0));
                    }
                }
            }
        }
        public static void RedrawPlayerAfterRespawnInBattle()
        {
            Actor myPlayerInfo = CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor;
            // supprimer tout les anciennes instances qui correspond a ce joueur pour eviter un doublons ou une erreur de la part du serveur ou client s'il n'envoie pas la cmd de deconnexion du joeur au abonnés
            if (CommonCode.AllActorsInMap.FindAll(i => (i.tag as Actor).pseudo == myPlayerInfo.pseudo).Count > 0)
            {
                Bmp allPlayers = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == myPlayerInfo.pseudo);
                allPlayers.visible = false;
                CommonCode.AllActorsInMap.Remove(allPlayers);
                CommonCode.AllActorsInMap.RemoveAll(i => (i.tag as Actor).pseudo == myPlayerInfo.pseudo);
            }

            // affichage du personnage + position + orientation
            Bmp ibPlayers = new Bmp(@"gfx\general\classes\" + myPlayerInfo.className + ".dat", Point.Empty,
                "Player_" + myPlayerInfo.pseudo, Manager.TypeGfx.Obj, true, 1,
                SpriteSheet.GetSpriteSheet(myPlayerInfo.className.ToString(),
                    ConvertToClockWizeOrientation(Convert.ToInt16(myPlayerInfo.directionLook))))
            {
                point = new Point(-100, -100)
            };
            ibPlayers.MouseOver += ibPlayers_MouseOver;
            ibPlayers.MouseOut += ibPlayers_MouseOut;
            ibPlayers.MouseMove += CursorHand_MouseMove;
            ibPlayers.MouseClic += ibPlayers_MouseClic;
            VerticalSyncZindex(ibPlayers);
            ibPlayers.TypeGfx = Manager.TypeGfx.Obj;
            Manager.manager.GfxObjList.Add(ibPlayers);

            // associer a notre joueur tous les données anciennement ajouté a notre joueur vus que les données recus sont minimes
            ibPlayers.tag = MyPlayerInfo.instance.ibPlayer.tag;
            Actor pi = ibPlayers.tag as Actor;

            // affichage des ailles
            if (pi.pvpEnabled)
            {
                if (pi.spirit != Enums.Spirit.Name.neutral)
                {
                    Bmp spirit = new Bmp(@"gfx\general\obj\2\" + pi.spirit + @"\" + pi.spiritLevel + ".dat", Point.Empty, "spirit_" + ibPlayers.name, Manager.TypeGfx.Obj, false, 1);
                    spirit.point = new Point((ibPlayers.rectangle.Width / 2) - (spirit.rectangle.Width / 2), -spirit.rectangle.Height);
                    ibPlayers.Child.Add(spirit);

                    Txt lPseudo = new Txt(pi.pseudo, Point.Empty, "lPseudo_" + ibPlayers.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                    lPseudo.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -spirit.rectangle.Height - 15);
                    ibPlayers.Child.Add(lPseudo);

                    Txt lLvlSpirit = new Txt(pi.spiritLevel.ToString(), Point.Empty, "lLvlSpirit_" + ibPlayers.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Bold), Brushes.Red);
                    lLvlSpirit.point = new Point((ibPlayers.rectangle.Width / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Width / 2) + 2, -spirit.rectangle.Y - (spirit.rectangle.Height / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Height / 2));
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

            // coloriage du personnage et attachement du maskColor au personnage
            CommonCode.ApplyMaskColorToClasse(ibPlayers);

            // pointeur vers l'image ibplayer de notre joueur s'il s'agit de son personnage
            if (CommonCode.MyPlayerInfo.instance.pseudo == pi.pseudo)
                CommonCode.MyPlayerInfo.instance.ibPlayer = ibPlayers;

            // ajout du personnage dans la liste des joueurs
            CommonCode.AllActorsInMap.Add(ibPlayers);
        }
        
    }
    public static class HudHandle
    {
        // gere les controles du hud
        // pointeurs vers des contrôles dans la classe MainForm
        public static RichTextBoxEx ChatArea;
        public static TextBox ChatTextBox;
        public static Rec SelectedCanalRec, GCanalChatRec, PCanalChatRec, HealthBarRec1, HealthBarRec2, TimeLeftToPlayRec1;
        public static Txt SelectedCanalTxt, GCanalChatTxt, PCanalChatTxt, HealthTxt, TimeLeftToPlayTxt1, Pc_Indicator_Txt, Pm_Indicator_Txt;
        public static Bmp SendBtn, ChatLink, HealthBar, StatsIcon, _passer_La_Main_btn, _quiter_le_combat, all_sorts, ChatAreaBG, Pc_Indicator, Pm_Indicator, SpellIcon;
        public static CheckBox ChatCursorDown;
        public static bool ChatCursorDownValidator = false;         // je pense que si true, le curseur reste en bas pour afficher les dernier message, si false il est reste sur sa position pour pouvoir lire l'historique
        public static int HealthTxtMethode = 2;         // if 0 le joueur vois seulement ses pdv, 1 vois ses pdv actuel + "/" + total pdv, 2 vois ses pdv en %
        public static bool hudVisible = false;             // pour determiner si la zone de sort + chat (hud) a été déssiné
        public static void UpdateHealth()
        {
            // cette methode est appelé lorsque l'utilisateur click sur la gauge de santé
            // ceci pour eviter d'attendre 2 seconds pour que les donnés se mette a jour

            int TotalPdv = 0;
            int CurrentPdv = 0;

            if (Battle.state == Enums.battleState.state.idle)
            {
                TotalPdv = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).maxHealth;
                CurrentPdv = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).currentHealth;
            }
            else
            {
                if (Battle.AllPlayersByOrder.Count() == 0)
                    return;
                if (Battle.AllPlayersByOrder.Exists(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo))
                {
                    TotalPdv = Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).maxHealth;
                    CurrentPdv = Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).currentHealth;
                }
                else if (Battle.DeadPlayers.Exists(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo))
                {
                    TotalPdv = Battle.DeadPlayers.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).maxHealth;
                    CurrentPdv = Battle.DeadPlayers.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).currentHealth;
                }
            }

            int X = 0;
            if (TotalPdv != 0)
                X = (CurrentPdv * 100) / TotalPdv;

            if (HealthTxtMethode == 0)
            {
                // affichage des pdv
                HealthTxt.Text = CurrentPdv.ToString();
                HealthTxt.point = new Point(HealthBarRec2.point.X + (HealthBarRec2.size.Width / 2) - (TextRenderer.MeasureText(HealthTxt.Text, HealthTxt.font).Width / 2), HealthBarRec2.point.Y + (HealthBarRec2.size.Height / 2) - (TextRenderer.MeasureText(HealthTxt.Text, HealthTxt.font).Height / 2));
            }
            else if (HealthTxtMethode == 1)
            {
                // affichage des pdv actuel + total pdv
                HealthTxt.Text = CurrentPdv + "\n" + CommonCode.TranslateText(69) + "\n" + TotalPdv;
                HealthTxt.point = new Point(HealthBarRec2.point.X + (HealthBarRec2.size.Width / 2) - (TextRenderer.MeasureText(HealthTxt.Text, HealthTxt.font).Width / 2), HealthBarRec2.point.Y + (HealthBarRec2.size.Height / 2) - (TextRenderer.MeasureText(HealthTxt.Text, HealthTxt.font).Height / 2));
            }
            else if (HealthTxtMethode == 2)
            {
                // affichage des pdv en pourcentage
                HealthTxt.Text = X + "%";
                HealthTxt.point = new Point(HealthBarRec2.point.X + (HealthBarRec2.size.Width / 2) - (TextRenderer.MeasureText(HealthTxt.Text, HealthTxt.font).Width / 2), HealthBarRec2.point.Y + (HealthBarRec2.size.Height / 2) - (TextRenderer.MeasureText(HealthTxt.Text, HealthTxt.font).Height / 2));
            }

            // bar de santé dans hudhandle
            HudHandle.HealthBar.rectangle = new Rectangle(new Point(HudHandle.HealthBar.rectangle.Location.X, HudHandle.HealthBar.rectangle.Location.Y), new Size(HudHandle.HealthBar.rectangle.Size.Width, (72 * X) / 100));
            HudHandle.HealthBar.point.Y = HudHandle.HealthBarRec2.point.Y + HudHandle.HealthBarRec2.size.Height - HudHandle.HealthBar.rectangle.Height - 1;

            // bar de santé dans menustats
            MenuStats.VieBar.size.Width = (236 * X) / 100;
            MenuStats.ViePts.Text = CurrentPdv.ToString() + " / " + TotalPdv + " (" + X + "%)";
        }
        static System.Windows.Forms.Timer chatBoxVisibilityIssueHandle = new System.Windows.Forms.Timer();
        public static List<string> ChatLog = new List<string>();                               // contien l'historique de conversation
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        public static void UpdatePc()
        {
            if (Battle.state == Enums.battleState.state.idle)
                Pc_Indicator_Txt.Text = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).originalPc.ToString();
            else
                Pc_Indicator_Txt.Text = Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).currentPc.ToString();
            Pc_Indicator_Txt.point = new Point(Pc_Indicator.point.X + (Pc_Indicator.rectangle.Size.Width / 2) - (TextRenderer.MeasureText(Pc_Indicator_Txt.Text, Pc_Indicator_Txt.font).Width / 2) + 5, Pc_Indicator.point.Y + (Pc_Indicator.rectangle.Size.Height / 2) - (TextRenderer.MeasureText(Pc_Indicator_Txt.Text, Pc_Indicator_Txt.font).Height / 2));
        }
        public static void UpdatePm()
        {
            if (Battle.state == Enums.battleState.state.idle)
                Pm_Indicator_Txt.Text = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).originalPm.ToString();
            else
                Pm_Indicator_Txt.Text = Battle.AllPlayersByOrder.Find(f => f.pseudo == CommonCode.MyPlayerInfo.instance.pseudo).currentPm.ToString();
            Pm_Indicator_Txt.point = new Point(Pm_Indicator.point.X + (Pm_Indicator.rectangle.Size.Width / 2) - (TextRenderer.MeasureText(Pm_Indicator_Txt.Text, Pm_Indicator_Txt.font).Width / 2) + 6, Pm_Indicator.point.Y + (Pm_Indicator.rectangle.Size.Height / 2) - (TextRenderer.MeasureText(Pm_Indicator_Txt.Text, Pm_Indicator_Txt.font).Height / 2) - 2);
        }
        public static void UpdatePm(string player)
        {
            if (Battle.state == Enums.battleState.state.started)
            {
                Pm_Indicator_Txt.Text = Battle.AllPlayersByOrder.Find(f => f.pseudo == player).currentPm.ToString();
                Pm_Indicator_Txt.point = new Point(Pm_Indicator.point.X + (Pm_Indicator.rectangle.Size.Width / 2) - (TextRenderer.MeasureText(Pm_Indicator_Txt.Text, Pm_Indicator_Txt.font).Width / 2) + 6, Pm_Indicator.point.Y + (Pm_Indicator.rectangle.Size.Height / 2) - (TextRenderer.MeasureText(Pm_Indicator_Txt.Text, Pm_Indicator_Txt.font).Height / 2) - 2);
            }
        }
        public static void ChannelState(string s)
        {
            // pour changer l'etat du canal, ainsi colorer et modifier le texte du SelectedCanalRec/Txt

            if (s == "G")
            {
                // G = General, chat qui s'affiche seulement dans le map
                SelectedCanalTxt.Text = "G";
                SelectedCanalRec.brush = CommonCode.spellAreaNotAllowedColor;
            }
            else if (s == "P")
            {
                // P = Privé
                SelectedCanalTxt.Text = "P";
                SelectedCanalRec.brush = Brushes.CadetBlue;
            }
        }
        public static void HudVisibility(bool visible)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            if (visible)
                MainForm.hudVisible = true;
            else
                MainForm.hudVisible = false;

            if (visible) MainForm.chatBox.Show();
            else MainForm.chatBox.Hide();

            ChatArea.Visible = visible;
            ChatTextBox.Visible = visible;
            SelectedCanalRec.visible = visible;
            SelectedCanalTxt.visible = visible;
            SendBtn.visible = visible;
            GCanalChatRec.visible = visible;
            GCanalChatTxt.visible = visible;
            PCanalChatRec.visible = visible;
            PCanalChatTxt.visible = visible;
            ChatCursorDown.Visible = visible;
            ChatLink.visible = visible;
            HealthBar.visible = visible;
            HealthBarRec1.visible = visible;
            HealthBarRec2.visible = visible;
            HealthTxt.visible = visible;
            StatsIcon.visible = visible;
            all_sorts.visible = visible;
            Pc_Indicator.visible = visible;
            Pm_Indicator.visible = visible;
            Pc_Indicator_Txt.visible = visible;
            Pm_Indicator_Txt.visible = visible;
            SpellIcon.visible = visible;

            if (visible)
                ChatTextBox.Focus();
        }
        public static void hudCleaner()
        {
            Control[] ChatCursorDown = Manager.manager.mainForm.Controls.Find("ChatCursorDown", false);
            Control[] ChatTextBox = Manager.manager.mainForm.Controls.Find("ChatTextBox", false);
            if (ChatCursorDown != null && ChatCursorDown.Count() > 0)
            {
                Manager.manager.mainForm.Controls.RemoveByKey("ChatCursorDown");
                ChatCursorDown[0].Dispose();
            }

            if (ChatTextBox != null && ChatTextBox.Count() > 0)
            {
                Manager.manager.mainForm.Controls.RemoveByKey("ChatTextBox");
                ChatTextBox[0].Dispose();
            }
        }
        public static void chatboxCleaner()
        {
            // pour détruire l'objet form ChatBox qui est créée lors de la connexion du joueur en jeux, l'objet se créer lors d'une nouvelle connexion dans la classe SelectPlayer.cs Ln 283
            chatBoxVisibilityIssueHandle.Stop();
            if (MainForm.chatBox != null)
                MainForm.chatBox.Dispose();
            MainForm.chatBox = null;
        }
        public static void recalibrateChatBoxPosition()
        {
            Point chatBoxPoint;
            //if (Process.GetProcessesByName("MMORPG").Count() + Process.GetProcessesByName("MMORPG.vshost").Count() == 1)
                //chatBoxPoint = new Point(8, -22);
            //else
                chatBoxPoint = new Point(3, -17);

            //reposition la 2eme forme qui contien le controle RichTextBoxEx nomé chatBox pour éviter le problème de flikering gdi
            MainForm.chatBox.Location = new Point(Manager.manager.mainForm.Location.X + chatBoxPoint.X, Manager.manager.mainForm.Location.Y + Manager.manager.mainForm.Height - MainForm.chatBox.Height + chatBoxPoint.Y);
        }
        public static void chatBoxRefreshHandler()
        {
            // appel d'une fonction par un timer qui vérifie indéfiniment si la form est en avant plant pour afficher la chatbox
            chatBoxVisibilityIssueHandle.Interval = 30;
            chatBoxVisibilityIssueHandle.Tick += T_chatBoxVisibleHandle_Tick;
            chatBoxVisibilityIssueHandle.Start();
        }
        public static void DrawHud()
        {
            #region affichage du hud, sorts, bar de santé, zone de chat ...
            if (MainForm.chatBox == null)
                MainForm.chatBox = new ChatBox();

            Point hudPoint = new Point(400, 540);

            // zone de texte chat
            TextBox ChatTextBox = new System.Windows.Forms.TextBox();
            ChatTextBox.Name = "ChatTextBox";
            ChatTextBox.BackColor = System.Drawing.Color.Tan;
            ChatTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ChatTextBox.Location = new System.Drawing.Point(0, hudPoint.Y + MainForm.chatBox.Size.Height);
            ChatTextBox.Width = 390;
            ChatTextBox.Height = 13;
            ChatTextBox.KeyUp += ChatTextBox_KeyUp;
            ChatTextBox.Visible = false;
            if (CommonCode.langue == 2)
                ChatTextBox.RightToLeft = RightToLeft.Yes;
            Manager.manager.mainForm.Controls.Add(ChatTextBox);
            HudHandle.ChatTextBox = ChatTextBox;

            CommonCode.installedLang = MainForm.nHunspellTextBoxExtender1.GetAvailableLanguages().ToList();
            if (CommonCode.installedLang.Count() > 0)
            {
                // des langues sonts installés
                // on vérifie si la langue choisie par défaut se trouve parmis ceux installés
                try
                {
                    foreach (string s in CommonCode.installedLang)
                        if (s == CommonCode.DefaultLang)
                        {
                            // la langue par défaut est installé
                            MainForm.nHunspellTextBoxExtender1.SetLanguage(CommonCode.DefaultLang);
                            if (!MainForm.transparentChatBox && CommonCode.SpellCheck)
                                MainForm.nHunspellTextBoxExtender1.EnableTextBoxBase(ChatTextBox);
                            break;
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            // on vérifie si les données sont correcte
            if (!CommonCode.installedLang.Exists(f => f == CommonCode.DefaultLang))
            {
                CommonCode.DefaultLang = CommonCode.installedLang[0];
                MainForm.nHunspellTextBoxExtender1.SetLanguage(CommonCode.DefaultLang);
                if (!MainForm.transparentChatBox && CommonCode.SpellCheck)
                    MainForm.nHunspellTextBoxExtender1.EnableTextBoxBase(ChatTextBox);
                CommonCode.saveOptions();
            }

            Bmp SendBtn = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ChatTextBox.Location.X + ChatTextBox.Size.Width, ChatTextBox.Location.Y + ChatTextBox.Size.Height - 13), "__SendBtn", Manager.TypeGfx.Top, false, 1, SpriteSheet.GetSpriteSheet("_Main_option", 8));
            SendBtn.MouseOver += CommonCode.CursorHand_MouseMove;
            SendBtn.MouseOut += CommonCode.CursorDefault_MouseOut;
            SendBtn.MouseClic += SendBtn_MouseClic;
            HudHandle.SendBtn = SendBtn;
            Manager.manager.GfxTopList.Add(SendBtn);
            Manager.manager.GfxFixedList.Add(SendBtn);

            ///////////////////// Selected Canal G
            Rec SelectedCanalRec = new Rec(CommonCode.spellAreaNotAllowedColor, new Point(SendBtn.point.X + SendBtn.rectangle.Size.Width, SendBtn.point.Y), new Size(10, 13), "__SelectedCanalRec", Manager.TypeGfx.Top, true);
            SelectedCanalRec.MouseMove += HandCursorRec;
            SelectedCanalRec.MouseOut += DefaultCursorRec;
            SelectedCanalRec.visible = false;
            Manager.manager.GfxTopList.Add(SelectedCanalRec);
            Manager.manager.GfxFixedList.Add(SelectedCanalRec);
            HudHandle.SelectedCanalRec = SelectedCanalRec;

            Txt SelectedCanalTxt = new Txt("G", Point.Empty, "__SelectedCanalTxt", Manager.TypeGfx.Top, false, new Font("Verdana", 6, FontStyle.Bold), Brushes.Black);
            SelectedCanalTxt.point = new Point(SelectedCanalRec.point.X + (SelectedCanalRec.size.Width / 2) - (TextRenderer.MeasureText(SelectedCanalTxt.Text, SelectedCanalTxt.font).Width / 2), SelectedCanalRec.point.Y + (SelectedCanalRec.size.Height / 2) - (TextRenderer.MeasureText(SelectedCanalTxt.Text, SelectedCanalTxt.font).Height / 2));
            Manager.manager.GfxTopList.Add(SelectedCanalTxt);
            Manager.manager.GfxFixedList.Add(SelectedCanalTxt);
            HudHandle.SelectedCanalTxt = SelectedCanalTxt;

            // canal Général
            Rec GCanalChatRec = new Rec(CommonCode.spellAreaNotAllowedColor, new Point(SelectedCanalRec.point.X, SelectedCanalRec.point.Y - SelectedCanalRec.size.Height), new Size(10, 13), "__GCanalChatRec", Manager.TypeGfx.Top, false);
            GCanalChatRec.MouseMove += HandCursorRec;
            GCanalChatRec.MouseOut += DefaultCursorRec;
            GCanalChatRec.MouseClic += GCanalChatRec_MouseClic;
            HudHandle.GCanalChatRec = GCanalChatRec;
            Manager.manager.GfxTopList.Add(GCanalChatRec);
            Manager.manager.GfxFixedList.Add(GCanalChatRec);

            Txt GCanalChatTxt = new Txt("G", new Point(321, 575), "__GCanalChatTxt", Manager.TypeGfx.Top, false, new Font("Verdana", 6, FontStyle.Bold), Brushes.Black);
            GCanalChatTxt.point = new Point(GCanalChatRec.point.X + (GCanalChatRec.size.Width / 2) - (TextRenderer.MeasureText(GCanalChatTxt.Text, GCanalChatTxt.font).Width / 2), GCanalChatRec.point.Y + (GCanalChatRec.size.Height / 2) - (TextRenderer.MeasureText(GCanalChatTxt.Text, GCanalChatTxt.font).Height / 2));
            GCanalChatTxt.MouseClic += GCanalChatTxt_MouseClic;
            Manager.manager.GfxTopList.Add(GCanalChatTxt);
            Manager.manager.GfxFixedList.Add(GCanalChatTxt);
            HudHandle.GCanalChatTxt = GCanalChatTxt;

            // canal Privé
            Rec PCanalChatRec = new Rec(Brushes.CadetBlue, new Point(GCanalChatRec.point.X, GCanalChatRec.point.Y - GCanalChatRec.size.Height), new Size(10, 13), "__PCanalChatRec", Manager.TypeGfx.Top, false);
            PCanalChatRec.MouseMove += HandCursorRec;
            PCanalChatRec.MouseOut += DefaultCursorRec;
            PCanalChatRec.MouseClic += PCanalChatRec_MouseClic;
            HudHandle.PCanalChatRec = PCanalChatRec;
            Manager.manager.GfxTopList.Add(PCanalChatRec);
            Manager.manager.GfxFixedList.Add(PCanalChatRec);

            Txt PCanalChatTxt = new Txt("P", new Point(321, 562), "__PCanalChatTxt", Manager.TypeGfx.Top, false, new Font("Verdana", 6, FontStyle.Bold), Brushes.Black);
            PCanalChatTxt.point = new Point(PCanalChatRec.point.X + (PCanalChatRec.size.Width / 2) - (TextRenderer.MeasureText(PCanalChatTxt.Text, PCanalChatTxt.font).Width / 2), PCanalChatRec.point.Y + (PCanalChatRec.size.Height / 2) - (TextRenderer.MeasureText(PCanalChatTxt.Text, PCanalChatTxt.font).Height / 2));

            Manager.manager.GfxTopList.Add(PCanalChatTxt);
            Manager.manager.GfxFixedList.Add(PCanalChatTxt);
            HudHandle.PCanalChatTxt = PCanalChatTxt;

            CheckBox ChatCursorDown = new CheckBox();
            ChatCursorDown.Name = "ChatCursorDown";
            ChatCursorDown.CheckState = CheckState.Checked;
            ChatCursorDown.FlatStyle = FlatStyle.Flat;
            ChatCursorDown.ForeColor = Color.Green;
            ChatCursorDown.Location = new Point(MainForm.chatBox.Size.Width, hudPoint.Y);
            ChatCursorDown.BackColor = Color.Transparent;
            ChatCursorDown.Visible = false;
            ChatCursorDown.KeyDown += ChatCursorDown_KeyDown;
            ChatCursorDown.Height = 12;
            HudHandle.ChatCursorDown = ChatCursorDown;
            Manager.manager.mainForm.Controls.Add(ChatCursorDown);
            //Manager.manager.GfxCtrlList.Add(ChatCursorDown);

            // bouton qui permet d'inserer un lien
            Bmp ChatLink = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ChatCursorDown.Location.X, ChatCursorDown.Location.Y + ChatCursorDown.Size.Height), "__ChatLink", Manager.TypeGfx.Top, false, 1, SpriteSheet.GetSpriteSheet("_Main_option", 7));
            ChatLink.MouseClic += ChatLink_MouseClic;
            ChatLink.MouseMove += CommonCode.CursorHand_MouseMove;
            ChatLink.MouseOut += CommonCode.CursorDefault_MouseOut;
            Manager.manager.GfxTopList.Add(ChatLink);
            Manager.manager.GfxFixedList.Add(ChatLink);
            HudHandle.ChatLink = ChatLink;

            // cadre qui encercle la gauge du santé
            Rec HealthBarRec1 = new Rec(Brushes.Black, Point.Empty, new Size(38, 76), "__HealthBarRec1", Manager.TypeGfx.Top, false);
            HealthBarRec1.point = new Point(618, ScreenManager.WindowHeight - HealthBarRec1.size.Height - 3);
            Manager.manager.GfxTopList.Add(HealthBarRec1);
            Manager.manager.GfxFixedList.Add(HealthBarRec1);
            HudHandle.HealthBarRec1 = HealthBarRec1;

            // cadre qui encercle la gauge du santé
            Rec HealthBarRec2 = new Rec(Brushes.WhiteSmoke, new Point(HealthBarRec1.point.X + 1, HealthBarRec1.point.Y + 1), new Size(36, 74), "__HealthBarRec2", Manager.TypeGfx.Top, false);
            Manager.manager.GfxTopList.Add(HealthBarRec2);
            Manager.manager.GfxFixedList.Add(HealthBarRec2);
            HudHandle.HealthBarRec2 = HealthBarRec2;

            // draw du gauge de santé
            Bmp HealthBar = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(HealthBarRec2.point.X + 1, HealthBarRec2.point.Y + 1), "__HealthBar", Manager.TypeGfx.Top, false, 1, SpriteSheet.GetSpriteSheet("_Main_option", 12));
            HealthBar.rectangle = new Rectangle(HealthBar.rectangle.Location, new Size(HealthBar.rectangle.Size.Width, 0));
            HealthBar.MouseClic += HealthBar_MouseClic;
            Manager.manager.GfxTopList.Add(HealthBar);
            Manager.manager.GfxFixedList.Add(HealthBar);
            HudHandle.HealthBar = HealthBar;

            // label du vita
            Txt HealthTxt = new Txt("0", Point.Empty, "__HealthTxt", Manager.TypeGfx.Top, false, new Font("Verdana", 6, FontStyle.Bold), Brushes.Black);
            HealthTxt.point = new Point(HealthBarRec2.point.X + (HealthBarRec2.size.Width / 2) - (TextRenderer.MeasureText(HealthTxt.Text, HealthTxt.font).Width / 2), HealthBarRec2.point.Y + (HealthBarRec2.size.Height / 2) - (TextRenderer.MeasureText(HealthTxt.Text, HealthTxt.font).Height / 2));
            Manager.manager.GfxTopList.Add(HealthTxt);
            Manager.manager.GfxFixedList.Add(HealthTxt);
            HudHandle.HealthTxt = HealthTxt;

            // icon stats
            Bmp StatsIcon = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ScreenManager.WindowWidth - 17, 38), "__StatsIcon", Manager.TypeGfx.Top, false, 1, SpriteSheet.GetSpriteSheet("_Main_option", 13));
            StatsIcon.MouseOver += CommonCode.CursorHand_MouseMove;
            StatsIcon.MouseOut += CommonCode.CursorDefault_MouseOut;
            StatsIcon.MouseClic += StatsImg_MouseClic;
            Manager.manager.GfxTopList.Add(StatsIcon);
            Manager.manager.GfxFixedList.Add(StatsIcon);
            HudHandle.StatsIcon = StatsIcon;

            // initialisation du timer du timeleft du combat
            Battle.TimeLeftToPlayT.Interval = 1000;
            Battle.TimeLeftToPlayT.Enabled = false;
            Battle.TimeLeftToPlayT.Tick += TimeLeftToPlayT_Tick;

            // rectangle affichant les sorts
            Bmp all_sorts = new Bmp(@"gfx\general\obj\1\all_sorts.dat", Point.Empty, "__all_sorts", Manager.TypeGfx.Top, false, 1);
            all_sorts.point = new Point(ScreenManager.WindowWidth - all_sorts.rectangle.Size.Width, hudPoint.Y);
            Manager.manager.GfxTopList.Add(all_sorts);
            Manager.manager.GfxFixedList.Add(all_sorts);
            HudHandle.all_sorts = all_sorts;

            // Indicateur de PC
            Bmp Pc_Indicator = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__Pc_Indicator", Manager.TypeGfx.Top, false, 1, SpriteSheet.GetSpriteSheet("_Main_option", 64));
            Pc_Indicator.point = new Point(HealthBarRec1.point.X + 5, HealthBarRec1.point.Y - Pc_Indicator.rectangle.Height - 40);
            Manager.manager.GfxTopList.Add(Pc_Indicator);
            Manager.manager.GfxFixedList.Add(Pc_Indicator);
            HudHandle.Pc_Indicator = Pc_Indicator;

            // Txt Pc_Indicator_Txt
            Txt Pc_Indicator_Txt = new Txt("0", Point.Empty, "__Pc_Indicator_Txt", Manager.TypeGfx.Top, false, new Font("Verdana", 10, FontStyle.Bold), Brushes.White);
            Pc_Indicator_Txt.point = new Point(Pc_Indicator.point.X + (Pc_Indicator.rectangle.Size.Width / 2) - (TextRenderer.MeasureText(Pc_Indicator_Txt.Text, Pc_Indicator_Txt.font).Width / 2) + 5, Pc_Indicator.point.Y + (Pc_Indicator.rectangle.Size.Height / 2) - (TextRenderer.MeasureText(Pc_Indicator_Txt.Text, Pc_Indicator_Txt.font).Height / 2));
            Manager.manager.GfxTopList.Add(Pc_Indicator_Txt);
            Manager.manager.GfxFixedList.Add(Pc_Indicator_Txt);
            HudHandle.Pc_Indicator_Txt = Pc_Indicator_Txt;

            // Indicateur de Pm
            Bmp Pm_Indicator = new Bmp(@"gfx\general\obj\1\all1.dat", Point.Empty, "__Pm_Indicator", Manager.TypeGfx.Top, false, 1, SpriteSheet.GetSpriteSheet("_Main_option", 65));
            Pm_Indicator.point = new Point(Pc_Indicator.point.X - Pm_Indicator.rectangle.Width - 1, Pc_Indicator.point.Y + 3);
            Manager.manager.GfxTopList.Add(Pm_Indicator);
            Manager.manager.GfxFixedList.Add(Pm_Indicator);
            HudHandle.Pm_Indicator = Pm_Indicator;

            // Txt Pc_Indicator_Txt
            Txt Pm_Indicator_Txt = new Txt("0", Point.Empty, "__Pm_Indicator_Txt", Manager.TypeGfx.Top, false, new Font("Verdana", 10, FontStyle.Bold), Brushes.White);
            Pm_Indicator_Txt.point = new Point(Pm_Indicator.point.X + (Pm_Indicator.rectangle.Size.Width / 2) - (TextRenderer.MeasureText(Pm_Indicator_Txt.Text, Pm_Indicator_Txt.font).Width / 2) + 6, Pm_Indicator.point.Y + (Pm_Indicator.rectangle.Size.Height / 2) - (TextRenderer.MeasureText(Pm_Indicator_Txt.Text, Pm_Indicator_Txt.font).Height / 2) - 2);
            Manager.manager.GfxTopList.Add(Pm_Indicator_Txt);
            Manager.manager.GfxFixedList.Add(Pm_Indicator_Txt);
            HudHandle.Pm_Indicator_Txt = Pm_Indicator_Txt;

            // icon sorts
            Bmp SpellIcon = new Bmp(@"gfx\general\obj\1\all1.dat", new Point(ScreenManager.WindowWidth - 17, 54), "__SpellIcon", Manager.TypeGfx.Top, false, 1, SpriteSheet.GetSpriteSheet("_Main_option", 73));
            SpellIcon.MouseOver += CommonCode.CursorHand_MouseMove;
            SpellIcon.MouseOut += CommonCode.CursorDefault_MouseOut;
            SpellIcon.MouseClic += SpellIcon_MouseClic;
            Manager.manager.GfxTopList.Add(SpellIcon);
            Manager.manager.GfxFixedList.Add(SpellIcon);
            HudHandle.SpellIcon = SpellIcon;
            hudVisible = true;
            #endregion
        }

        private static void T_chatBoxVisibleHandle_Tick(object sender, EventArgs e)
        {
            if (ApplicationIsActivated() && hudVisible)
                MainForm.chatBox.Show();
            else
                MainForm.chatBox.Hide();
        }
        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }
        public static void HandCursorRec(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorHand_MouseMove(null, null);
        }
        static void ChatTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;
            if (e.KeyCode == Keys.Enter)
            {
                if (!Security.check_valid_msg(HudHandle.ChatTextBox.Text))
                    MessageBox.Show(CommonCode.TranslateText(30));
                else if (HudHandle.ChatTextBox.Text.Length > CommonCode.ChatMessageMaxChar)
                    MessageBox.Show(CommonCode.TranslateText(27));
                else
                    SendBtn_MouseClic(null, null);
            }
            else if (e.KeyCode == Keys.Up)
            {
                try
                {
                    // affichage de l'historique en arriere
                    // recherche si la chaine dans ChatTextBox se trouve dans la liste ChatLog

                    int pos = -1;
                    if (HudHandle.ChatTextBox.Text != "")
                        pos = ChatLog.FindIndex(f => f.Split('•')[0] == HudHandle.ChatTextBox.Text);

                    // la chaine a été trouvé dans le fichier ChatLog, il faut que sa sois plus grand que 0, parsqu'on peux pas retourner avant la 1ere occurance
                    // affichage de l'occurance precedente
                    if (pos <= 0) return;
                    HudHandle.ChatTextBox.Text = ChatLog.ElementAt(pos - 1).Split('•')[0];
                    HudHandle.ChannelState(ChatLog.ElementAt(pos - 1).Split('•')[1]);
                }
                catch (Exception ex)
                {
                    if (CommonCode.debug && CommonCode.showErrors)
                        ChatArea.AppendText(ex.ToString());
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                try
                {
                    // affichage de l'historique en avant
                    // recherche si la chaine dans ChatTextBox se trouve dans la liste ChatLog

                    int pos = -1;
                    if (HudHandle.ChatTextBox.Text != "")
                        pos = ChatLog.FindIndex(f => f.Split('•')[0] == HudHandle.ChatTextBox.Text);

                    // la chaine a été trouvé dans l'historique ChatLog, il faut que sa sois plus grand que 0, parsqu'on peux pas retourner avant la 1ere occurance
                    // affichage de l'occurance precedente
                    if (pos < ChatLog.Count - 1)
                    {
                        HudHandle.ChatTextBox.Text = ChatLog.ElementAt(pos + 1).Split('•')[0];
                        HudHandle.ChannelState(ChatLog.ElementAt(pos + 1).Split('•')[1]);
                    }
                }
                catch (Exception ex)
                {
                    if (CommonCode.debug && CommonCode.showErrors)
                        ChatArea.AppendText(ex.ToString());
                }
            }
        }
        static void SendBtn_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            RichTextBoxEx ChatArea = MainForm.chatBox.Controls.Find("ChatArea", false)[0] as RichTextBoxEx;

            if (HudHandle.SelectedCanalTxt.Text == "P" && HudHandle.ChatTextBox.TextLength > CommonCode.MyPlayerInfo.instance.pseudo.Length && HudHandle.ChatTextBox.Text.Substring(0, CommonCode.MyPlayerInfo.instance.pseudo.Length).ToLower() == CommonCode.MyPlayerInfo.instance.pseudo)
            {
                ChatArea.AppendText((ChatArea.Text == "") ? "" : "\n");
                ChatArea.SelectionStart = ChatArea.TextLength;
                ChatArea.SelectionLength = 0;
                ChatArea.SelectionColor = Color.Red;
                ChatArea.AppendText(CommonCode.TranslateText(28));
                ChatArea.SelectionColor = ChatArea.ForeColor;
            }
            else if (HudHandle.ChatTextBox.Text != "")
            {
                if (HudHandle.SelectedCanalTxt.Text == "P" && HudHandle.ChatTextBox.Text.Split('#').Count() == 2)
                {
                    // impossible d'envoyer un message a sois meme
                    if (HudHandle.ChatTextBox.Text.Split('#')[0].ToLower() == CommonCode.MyPlayerInfo.instance.pseudo)
                    {
                        ChatArea.AppendText((ChatArea.Text == "") ? "" : "\n");
                        ChatArea.SelectionStart = ChatArea.TextLength;
                        ChatArea.SelectionLength = 0;
                        ChatArea.SelectionColor = Color.Red;
                        ChatArea.AppendText(CommonCode.TranslateText(28));
                        ChatArea.SelectionColor = ChatArea.ForeColor;
                        return;
                    }

                    // affichage du texte envoyé en mp en chat general
                    ChatArea.AppendText((ChatArea.Text == "") ? "" : "\n");
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Green;

                    // pour recuperer le reste du texte quand l'utilisateur ajoute un # qui s'ajoute a celui du pseudo#...#
                    string tmpMsg = "";
                    if (HudHandle.ChatTextBox.Text.Split('#').Length >= 2)
                    {
                        for (int cnt = 1; cnt < HudHandle.ChatTextBox.Text.Split('#').Length; cnt++)
                            tmpMsg += HudHandle.ChatTextBox.Text.Split('#')[cnt] + "#";

                        tmpMsg = tmpMsg.Substring(0, tmpMsg.Length - 1);
                    }

                    ///////////////////// l'heur
                    ChatArea.SelectionStart = ChatArea.TextLength;
                    ChatArea.SelectionLength = 0;
                    ChatArea.SelectionColor = Color.Green;
                    ChatArea.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                    //////////////////////////////////////////////////////

                    ChatArea.AppendText(CommonCode.TranslateText(3) + " ");
                    ChatArea.InsertLink(" " + HudHandle.ChatTextBox.Text.Split('#')[0]);
                    ChatArea.AppendText(" : ");

                    // recherche l'existance d'un lien dans le text
                    if (tmpMsg.IndexOf("[l/]") != -1 && tmpMsg.IndexOf("[\\l]") != -1 && tmpMsg.Length > 12)
                    {
                        // le texte contiens un lien
                        // affichage du texte qui precede la balise ouverture de lien
                        ChatArea.AppendText(tmpMsg.Substring(0, tmpMsg.IndexOf("[l/]")) + " ");
                        string tmpMsg2 = tmpMsg.Substring(tmpMsg.IndexOf("[l/]") + 4, tmpMsg.IndexOf("[\\l]") - tmpMsg.IndexOf("[l/]") - 4);
                        ChatArea.InsertLink(tmpMsg2);
                        int pos1 = tmpMsg.IndexOf("[\\l]") + 4;
                        string str1 = tmpMsg.Substring(pos1, tmpMsg.Length - pos1);
                        ChatArea.AppendText(str1);
                    }
                    else
                        ChatArea.AppendText(tmpMsg);

                    ChatArea.SelectionColor = ChatArea.ForeColor;
                }

                Network.SendMessage("cmd•ChatMessage•" + HudHandle.SelectedCanalTxt.Text + "•" + HudHandle.ChatTextBox.Text, true);

                // enregistrement des messages sur le chatlog
                ChatLog.Add(HudHandle.ChatTextBox.Text + "•" + HudHandle.SelectedCanalTxt.Text);

                HudHandle.ChatTextBox.Text = "";
                HudHandle.ChannelState("G");
            }
        }
        public static void DefaultCursorRec(Rec rec, MouseEventArgs e)
        {
            CommonCode.CursorDefault_MouseOut(null, null);
        }
        static void GCanalChatRec_MouseClic(Rec rec, MouseEventArgs e)
        {
            HudHandle.ChannelState("G");
        }
        static void GCanalChatTxt_MouseClic(Txt txt, MouseEventArgs e)
        {
            HudHandle.ChannelState("G");
        }
        static void PCanalChatRec_MouseClic(Rec rec, MouseEventArgs e)
        {
            HudHandle.ChannelState("P");
        }
        static void ChatCursorDown_KeyDown(object sender, KeyEventArgs e)
        {
            ChatArea_KeyUp(sender, e);
        }
        static void ChatLink_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            HudHandle.ChatTextBox.AppendText("[l/][\\l]");
            HudHandle.ChatTextBox.SelectionStart = HudHandle.ChatTextBox.Text.IndexOf("[l/]") + 4;
            HudHandle.ChatTextBox.SelectionLength = 0;
            HudHandle.ChatTextBox.Focus();
        }
        static void HealthBar_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            HudHandle.HealthTxtMethode++;
            if (HudHandle.HealthTxtMethode > 2)
                HudHandle.HealthTxtMethode = 0;
            HudHandle.UpdateHealth();
        }
        static void StatsImg_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            if (MenuStats.StatsImg.visible)
                MenuStats.StatsImg.visible = false;
            else
                MenuStats.StatsImg.visible = true;
        }
        public static void TimeLeftToPlayT_Tick(object sender, EventArgs e)
        {
            if (Battle.TimeLeftToPlay >= 0)
            {
                // calcule du pourcentage du temps qui reste pour le joueur avant de passer la main
                int timeLeftInPercent = (Battle.TimeLeftToPlay * 100) / Battle.TimeToPlayInBattle;
                int newSize = (34 * timeLeftInPercent) / 100;
                Rec timeLeftRec = (HudHandle.TimeLeftToPlayRec1.Child.Find(f => f.Name() == "__TimeLeftToPlayBar") as Rec);
                timeLeftRec.size.Height = newSize;
                timeLeftRec.point.Y = 36 - newSize;

                HudHandle.TimeLeftToPlayTxt1.Text = Battle.TimeLeftToPlay.ToString();
                HudHandle.TimeLeftToPlayTxt1.point.X = HudHandle.TimeLeftToPlayRec1.point.X + 2 + (38 - TextRenderer.MeasureText(HudHandle.TimeLeftToPlayTxt1.Text, HudHandle.TimeLeftToPlayTxt1.font).Width) / 2;
                HudHandle.TimeLeftToPlayTxt1.visible = true;

                Battle.TimeLeftToPlay--;

                if (Battle.PlayerTurn == CommonCode.MyPlayerInfo.instance.pseudo)
                    timeLeftRec.brush = new SolidBrush(Color.FromArgb(31, 142, 255));
                else
                    timeLeftRec.brush = CommonCode.spellAreaNotAllowedColor;
            }
            else
                Battle.TimeLeftToPlayT.Stop();
        }
        static void SpellIcon_MouseClic(Bmp bmp, MouseEventArgs e)
        {
            // clic sur l'icone du menu sort en haut à droite
            ShowDrawSpellStatesMenu();
        }
        static void ChatArea_KeyUp(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.C)
            {
                HudHandle.ChatTextBox.Focus();
            }
            else
            {
                string alpha = "azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN123456789+-*/²&é\"'(-è_çà)=~#{[|`\\^@]}¤£¨µ%§/.?<>";
                string key = e.KeyCode.ToString();
                if (alpha.IndexOf(e.KeyCode.ToString()) != -1)
                {
                    HudHandle.ChatTextBox.Text = e.KeyCode.ToString();
                    HudHandle.ChatTextBox.Focus();
                    HudHandle.ChatTextBox.Select(1, 0);
                }
            }
        }
        public static void ShowDrawSpellStatesMenu()
        {
            // affiche ou cache le menu des sorts
            Rec showSpellsParent = Manager.manager.GfxTopList.Find(f => f.Name() == "__showSpellsParent") as Rec;
            bool visible = !showSpellsParent.visible;
            showSpellsParent.visible = visible;
            Manager.manager.mainForm.Controls.Find("__spellsVscroll", false)[0].Visible = visible;
            Manager.manager.mainForm.Controls.Find("__typeOfSpellCB", false)[0].Visible = visible;
            if (!visible)
                Manager.manager.GfxTopList.RemoveAll(f => f.Name() != null && f.Name().Length > 5 && f.Name().Substring(0, 5) == "DSI__");
        }

    }
    public static class MenuStats
    {
        public static Bmp StatsImg, ThumbsAvatar, GradePvp, Flag, TerreLvlRegle, FeuLvlRegle, VentLvlRegle, FoudreLvlRegle, EauLvlRegle;
        public static Txt StatsPlayerName, StatsLevel, Rang, LevelPvp, LFlag, Fusion1, Fusion2, NiveauGaugeTxt, NiveauGaugeTxtCurrent, AffiniteElementaireTxt, terreStats, FeuStats, VentStats, FoudreStats, EauStats, TerrePuissance, FeuPuissance, VentPuissance, FoudrePuissance, EauPuissance, Lvl1RegleTxt, Lvl2RegleTxt, Lvl3RegleTxt, Lvl4RegleTxt, Lvl5RegleTxt, Lvl6RegleTxt, Lvl2ReglePts, Lvl3ReglePts, Lvl4ReglePts, Lvl5ReglePts, Lvl6ReglePts, DotonLvl, KatonLvl, FutonLvl, RaitonLvl, SuitonLvl, VieLabel, ViePts, PC, PM, PE, CD, Invoc, Initiative, Job1Label, Specialite1Label, Job2Labe1, Specialite2Label, PoidLabel, Poid, Ryo, resiDotonTxt, resiKatonTxt, resiFutonTxt, resiRaitonTxt, resiSuitonTxt, __esquivePC_Txt, __esquivePM_Txt, __esquivePE_Txt, __esquiveCD_Txt, __retraitPC_Txt, __retraitPM_Txt, __retraitPE_Txt, __retraitCD_Txt, Puissance, DomFix;
        public static Rec NiveauGaugeRecPercent, NiveauGaugeRec2, TerreLvlGauge, FeuLvlGauge, VentLvlGauge, FoudreLvlGauge, EauLvlGauge, VieBar, PoidRec, spellrec;
    }
    public class PiibSortByInitiative : IComparer<Actor>
    {
        // pour classer les team selon leurs initiative
        public int Compare(Actor x, Actor y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    if (x.initiative > y.initiative)
                        return 1;
                    else if (x.initiative < y.initiative)
                        return -1;
                    else
                        return 0;
                }
            }
        }
    }
}