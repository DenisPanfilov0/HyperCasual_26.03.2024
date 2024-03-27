using CodeBase.Game;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.State
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        
        private readonly SceneLoader _sceneLoader;
        
        private readonly IGameFactory _gameFactory;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
        }

        public void Enter(string payload) => 
            _sceneLoader.Load(payload, OnLoaded);

        public void Exit()
        {
            Debug.Log("Exit State");
        }

        private void OnLoaded()
        {
            GameObject grid = InitLevel();
            
            _stateMachine.Enter<GamePlayState, GameObject>(grid);
        }

        private GameObject InitLevel()
        {
            GameObject canvas = _gameFactory.CreateCanvas();
            
            Canvas canvasComponent = canvas.GetComponent<Canvas>();
            
            canvasComponent.renderMode = RenderMode.ScreenSpaceCamera;
    
            canvasComponent.worldCamera = Camera.main;
            
            GameObject grid = _gameFactory.CreateGrid(at: canvas.transform);
            
            return grid;
        }
    }
}