using System.Windows.Forms;
using MMORPG.GameStates;

namespace MMORPG.Net.Messages.Response
{
    internal class DeleteActorGrantedResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            MessageBox.Show(CommonCode.TranslateText(61), "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            GameStateManager.ChangeState(new SelectPlayer());
            GameStateManager.CheckState();
        }
    }
}
