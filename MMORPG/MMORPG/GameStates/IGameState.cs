using System;
using System.Drawing;

namespace MMORPG.GameStates
{
    public interface IGameState
    {
        void Init();

        void Network_stat(string stat); // reception des requette Network et redirection vers Handel_Network_Stat

        void Handle_Network_Stat(string stat);  // technique pour faire du cross thread,si non impossible de modifier les objet depuis un autre thread (celui du network vers le thread principal)

        void Update();

        void CleanUp();
    }
}
