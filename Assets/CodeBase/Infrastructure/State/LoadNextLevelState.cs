using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.State
{
    public class LoadNextLevelState : IState
    {
        private readonly GameStateMachine _stateMachine;
        
        private readonly IGameFactory _gameFactory;
        

        public LoadNextLevelState(GameStateMachine stateMachine, IGameFactory gameFactory)
        {
            _stateMachine = stateMachine;
            _gameFactory = gameFactory;
        }

        public void Enter()
        {
            if (_gameFactory.GetCurrentLevel())
            {
                GameObject grid = _gameFactory.ClearGrid();
                
                _stateMachine.Enter<GamePlayState, GameObject>(grid);
            }
            else
                _stateMachine.Enter<RestartLevelState>();
        }

        public void Exit()
        {
            Debug.Log("Exit State");
        }
    }
}