using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MMORPG
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // verifier si le joueur a activer un arriere plant pour la chatbox, si oui un code pour rendre le controle transparent rend le textebox du spellchecker en flickering,  si non inclure un wrapper wpf pour le spellcheck
            #region lecture du fichier config.ini
            // verification si le fichier config est corrempu
            CommonCode.repairConfigFile();
            List<string> configFile = new List<string>();
            using (StreamReader sr = new StreamReader(@"Config.ini"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    configFile.Add(line);
                sr.Close();
            }

            foreach (string t in configFile)
            {
                if (t == string.Empty || t.Substring(0, 2) == "//" || t.IndexOf(':') == -1) continue;
                string[] dataLine = t.Split(':');
                dataLine[0] = dataLine[0].Replace(" ", "");
                dataLine[1] = dataLine[1].Replace(" ", "");

                if (dataLine[0] != "TransparentChatBox") continue;
                MainForm.transparentChatBox = Convert.ToBoolean(dataLine[1]);
                break;
            }

            #endregion
            Application.Run(new MainForm());
        }
    }
}
