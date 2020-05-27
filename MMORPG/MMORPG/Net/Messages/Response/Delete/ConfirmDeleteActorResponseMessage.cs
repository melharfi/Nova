using MELHARFI;
using MELHARFI.Gfx;
using System.Windows.Forms;

namespace MMORPG.Net.Messages.Response
{
    internal class ConfirmDeleteActorResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            Rec rec1 = (Rec)Manager.manager.GfxTopList.Find(f => f.Name() == "rec1");
            rec1.visible = true;
            Txt secretQuestion = (Txt)rec1.Child.Find(f => f.Name() == "qs");
            secretQuestion.Text = commandStrings[1];
            secretQuestion.point.X = (rec1.size.Width / 2) - (TextRenderer.MeasureText(secretQuestion.Text, secretQuestion.font).Width / 2);

            TextBox secretQuestionTb = (TextBox)Manager.manager.mainForm.Controls.Find("secretQuestionTB", false)[0];
            secretQuestionTb.Visible = true;
            secretQuestionTb.Focus();

            Button envoyerBtn = (Button)Manager.manager.mainForm.Controls.Find("envoyerBtn", false)[0];
            envoyerBtn.Visible = true;

            Button annulerBtn = (Button)Manager.manager.mainForm.Controls.Find("annulerBtn", false)[0];
            annulerBtn.Visible = true;
        }
    }
}
