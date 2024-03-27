using CodeBase.Config.CardData;
using CodeBase.Game;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.State
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        
        private readonly SceneLoader _sceneLoader;
        
        private readonly AllServices _services;
        
        private readonly CardBundleData _cardBundleData;
        
        private const string GameplaySceneName = "Game";
        
        private const string InitialSceneName = "Initial";

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services, CardBundleData cardBundleData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _cardBundleData = cardBundleData;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, EnterLoadLeve);
            
            ResetConfigCollection();
        }

        private void EnterLoadLeve() => 
            _stateMachine.Enter<LoadLevelState, string>(GameplaySceneName);

        public void Exit()
        {
            Debug.Log("Exit State");
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IAssets>(new AssetProvider());
            
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>()));
        }
        
        private void ResetConfigCollection()
        {
            foreach (var cardData in _cardBundleData.CardData)
            {
                foreach (var item in cardData.Items)
                {
                    if (item.IsUsed) 
                        item.IsUsed = false;
                }
            }
        }
    }
}