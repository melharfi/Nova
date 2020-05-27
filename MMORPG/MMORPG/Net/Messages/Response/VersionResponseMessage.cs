using System;
using System.Linq;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class VersionMessageResponse : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            Enums.Version.version version = (Enums.Version.version)Enum.Parse(typeof(Enums.Version.version), commandStrings[1], true);
            Txt connexion_stat = (Txt)Manager.manager.GfxObjList.Find(f => f.Name() == "__connexion_stat");
            if (!Manager.manager.mainForm.Controls.Find("username", false).Any())
                return;
            TextBox username = (TextBox)Manager.manager.mainForm.Controls.Find("username", false)[0];
            if (!Manager.manager.mainForm.Controls.Find("password", false).Any())
                return;
            TextBox password = (TextBox)Manager.manager.mainForm.Controls.Find("password", false)[0];
            if (connexion_stat == null)
                return;
            
            // pas de differance entre MAJOR_LESS et REVISION_LESS puisque le client dois avoir la meme version avec le serveur
            switch(version)
            {
                case Enums.Version.version.MAJOR_LESS :
                    // VersionResponseMessage•MAJOR_LESS•Network.downloadMajorLink
                    break;
                case Enums.Version.version.REVISION_LESS :
                    // VersionResponseMessage•REVISION_LESS•Network.downloadMajorLink
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Enum " + version + " does'nt have it's handle in switch scoop");
            }

            DialogResult result1 = MessageBox.Show(CommonCode.TranslateText(11), "Version Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
                System.Diagnostics.Process.Start(commandStrings[2]);

            Bmp ConnexionBtn = (Bmp) Manager.manager.GfxObjList.Find(f => f.Name() == "__ConnexionBtn");

            username.Focus();
            username.Enabled = true;
            password.Enabled = true;
            ConnexionBtn.visible = true;
            IGfx __connexionBtnLabel = Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel");
            if (__connexionBtnLabel != null)
                Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel").Visible(true);
            Network.netClient.Shutdown("0x5");
        }
    }
}
