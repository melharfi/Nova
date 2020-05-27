using MMORPG.GameStates;

namespace MMORPG.Net.Messages.Response
{
    internal class CreateNewActorResponseMessage : IResponseMessage
    {
        public void Fetch(string[] commandStrings)
        {
            GameStateManager.ChangeState(new CreatePlayer());
            GameStateManager.CheckState();
        }
    }
}
