using System.Diagnostics;
using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;
using MMORPG.Net.Messages.Request;

namespace MMORPG.Net.Messages.Response
{
    internal class AuthentificationUserBusyMessageResponse : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            // client déjà connecté, on laisse le choix au client sois de le deconnecter sois de se deconnecter sois même
            var result = MessageBox.Show(CommonCode.TranslateText(5), @"Connexion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            var username = Manager.manager.mainForm.Controls.Find(@"username", false)[0] as TextBox;
            var password = Manager.manager.mainForm.Controls.Find(@"username", false)[0] as TextBox;
            var connexionBtn = Manager.manager.GfxObjList.Find(f => f.Name() == @"__ConnexionBtn") as Bmp;

            if (result == DialogResult.Yes)
            {
                // connexion en mode Replace qui prend la relève et deconnecte l'autre utilisateur
                var authentification = new AuthentificationRequestMessage(username.Text, password.Text, AuthentificationRequestMessage.OverridePreviousConnexion.Replace);
                authentification.Serialize();
                authentification.Send();

                // tester le remplacement d'un client deja connecté pour voir est se que ca va marcher
            }
            else
            {
                Debug.Assert(username != null, "username != null");
                username.Enabled = true;
                Debug.Assert(password != null, "password != null");
                password.Enabled = true;
                Debug.Assert(connexionBtn != null, "connexionBtn != null");
                connexionBtn.visible = true;
                IGfx connexionBtnLabel = Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel");
                if (connexionBtnLabel != null)
                    Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel").Visible(true);
                // fermeture de connexion
                Network.Shutdown();
            }
        }
    }
}
