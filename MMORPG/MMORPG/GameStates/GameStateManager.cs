using System;
using System.Drawing;

namespace MMORPG.GameStates
{
    public sealed class GameStateManager
    {
        #region Objet Statique
        private static GameStateManager instance = new GameStateManager();

        public static bool Running
        {
            get { return instance._Running; }
        }

        public static void ChangeState(IGameState state)
        {
            instance.changeState(state);
        }

        public static void CheckState()
        {
            instance.checkState();
        }

        public static void Update()
        {
            instance.update();
        }

        public static void Quit()
        {
            instance.quit();
        }

        public static void Network_stat(string stat)
        {
            instance.network_stat(stat);
        }

        #endregion
        ////////////////////////////////////////////////



        /////////////////////////////////////////////
        #region Objet Dynamique
        IGameState _currentState;
        IGameState _nextState;
        bool _Running;

        private GameStateManager()
        {
            _currentState = null;
            _nextState = null;
            _Running = true;
        }

        private void changeState(IGameState newState)
        {
            if (_currentState != null)
                _currentState.CleanUp();
            _currentState = null;
            _nextState = newState;
        }

        private void checkState()
        {
            if ((_currentState != null) || (_nextState == null))
                return;
            // Un nouvel état est utilisé
            _currentState = _nextState;
            _nextState = null;
            _currentState.Init();
        }

        private void update()
        {
            if (_currentState != null)
                _currentState.Update();
        }

        private void quit()
        {
            _Running = false;
            _currentState.CleanUp();
            _currentState = null;
        }

        private void network_stat(string stat)
        {
            if (_currentState != null)
                _currentState.Network_stat(stat);
        }

        #endregion
    }
}
