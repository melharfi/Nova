using MELHARFI;
using System.Windows.Forms;

namespace MMORPG.Net.Messages.Response
{
    internal class CreateNewActorVillageNotSelectedResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            MessageBox.Show(CommonCode.TranslateText(57), "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Manager.manager.mainForm.Controls.Find("name", false)[0].Focus();
            Manager.manager.GfxObjList.Find(f => f.Name() == "ibValider").Visible(true);
        }
    }
}
