using System.Windows.Forms;
using MELHARFI;

namespace MMORPG.Net.Messages.Response
{
    internal class CreateNewActorMaxCharactersReachedReponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            MessageBox.Show(CommonCode.TranslateText(58), "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Manager.manager.GfxObjList.Find(f => f.Name() == "ibValider").Visible(true);
        }
    }
}
