using MMORPG.GameStates;

namespace MMORPG.Net.Messages.Response
{
    internal class CreatedNewActorSuccessfullyResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            GameStateManager.ChangeState(new SelectPlayer());
            GameStateManager.CheckState();
        }
    }
}
