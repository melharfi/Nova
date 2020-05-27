using System.Windows.Forms;
using MELHARFI;
using MELHARFI.Gfx;

namespace MMORPG.Net.Messages.Response
{
    internal class DeleteActorIncorrectSecretAnswerResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            MessageBox.Show(CommonCode.TranslateText(62), "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Rec rec1 = (Rec)Manager.manager.GfxTopList.Find(f => f.Name() == "rec1");
            rec1.visible = true;
            TextBox secretQuestionTB = (TextBox)Manager.manager.mainForm.Controls.Find("secretQuestionTB", false)[0];
            secretQuestionTB.Visible = true;
            Button envoyerBtn = (Button)Manager.manager.mainForm.Controls.Find("envoyerBtn", false)[0];
            envoyerBtn.Visible = true;
            Button annulerBtn = (Button)Manager.manager.mainForm.Controls.Find("annulerBtn", false)[0];
            annulerBtn.Visible = true;
            secretQuestionTB.Focus();
        }
    }
}
