using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class AuthentificationEmailNotValidatedMessageResponse : IResponseMessage
    {
        readonly TextBox _username = Manager.manager.mainForm.Controls.Find(@"username", false)[0] as TextBox;
        readonly TextBox _password = Manager.manager.mainForm.Controls.Find(@"username", false)[0] as TextBox;
        readonly Bmp _connexionBtn = Manager.manager.GfxObjList.Find(f => f.Name() == @"__ConnexionBtn") as Bmp;

        public void Fetch(string[] commandStrings)
        {
            MessageBox.Show(CommonCode.TranslateText(106), "Identification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _username.Enabled = true;
            _password.Enabled = true;
            _connexionBtn.visible = true;
            IGfx __connexionBtnLabel = Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel");
            if (__connexionBtnLabel != null)
                Manager.manager.GfxObjList.Find(f => f.Name() == "__connexionBtnLabel").Visible(true);
            // fermeture de connexion
            Network.Shutdown();
        }
    }
}
