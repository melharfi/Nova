using System;
using System.Windows.Forms;
using MELHARFI;
using MMORPG.GameStates;

namespace MMORPG.Net.Messages.Response
{
    internal class AuthentificationGrantedResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            var username = Manager.manager.mainForm.Controls.Find("username", false)[0] as TextBox;
            if (username != null) CommonCode.MyPlayerInfo.instance.User = username.Text;
            else
                throw new NotImplementedException("can't find username TextBox control, are you still in login form ?");
            GameStateManager.ChangeState(new SelectPlayer());
            GameStateManager.CheckState();
        }
    }
}
