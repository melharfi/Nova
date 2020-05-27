using System.Windows.Forms;
using MELHARFI;

namespace MMORPG.Net.Messages.Response
{
    internal class CreateNewActorNameWrongSizeResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            MessageBox.Show(CommonCode.TranslateText(120), "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Manager.manager.mainForm.Controls.Find("name", false)[0].Focus();
            Manager.manager.GfxObjList.Find(f => f.Name() == "ibRetour");
        }
    }
}
