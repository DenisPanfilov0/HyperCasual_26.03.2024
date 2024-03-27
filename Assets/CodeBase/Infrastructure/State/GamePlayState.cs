using CodeBase.Config.CardData;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.State
{
    public class GamePlayState : IPayloadedState<GameObject>
    {
        private readonly GameStateMachine _stateMachine;
        
        private readonly CardBundleData _cardBundleData;
        
        private readonly IGameFactory _gameFactory;
        
        public GamePlayState(GameStateMachine stateMachine, CardBundleData cardBundleData, IGameFactory gameFactory)
        {
            _stateMachine = stateMachine;
            _cardBundleData = cardBundleData;
            _gameFactory = gameFactory;
        }

        public void Enter(GameObject payload) => 
            InitialCells(payload);

        public void Exit()
        {
            Debug.Log("Exit State");
        }

        private void InitialCells(GameObject payload)
        {
            _gameFactory.CreateCells(_cardBundleData, payload.transform, payload);
            
            SubscribeToRightButtonClick(payload);
        }

        private void SubscribeToRightButtonClick(GameObject payload)
        {
            var cellFillingComponents = payload.GetComponentsInChildren<CellFilling>();
            foreach (var cellFilling in cellFillingComponents)
            {
                cellFilling.OnRightButtonClick += HandleRightButtonClick;
            }
        }

        private void HandleRightButtonClick() => 
            _stateMachine.Enter<LoadNextLevelState>();
    }
}