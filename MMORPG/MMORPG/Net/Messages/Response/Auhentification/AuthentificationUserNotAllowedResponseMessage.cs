using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class AuthentificationUserNotAllowedMessageResponse : IResponseMessage
    {
        // client déjà connecté, on laisse le choix au client sois de le deconnecter sois de se deconnecter sois même
        private readonly TextBox _username = Manager.manager.mainForm.Controls.Find(@"username", false)[0] as TextBox;
        //TextBox password = Manager.manager.mainForm.Controls.Find(@"username", false)[0] as TextBox;
        Txt connexion_stat;

        public void Fetch(string[] commandStrings)
        {
            connexion_stat = (Txt)Manager.manager.GfxObjList.Find(f => f.Name() == "__connexion_stat");
            if (Manager.manager.mainForm.Controls.Find("username", false).Length == 0)
                return;

            connexion_stat.Text = string.Empty;
            _username.Focus();
            _username.Focus();
            Network.netClient.Shutdown(CommonCode.TranslateText(23));
        }
    }
}
