using CodeBase.Config.CardData;
using CodeBase.Infrastructure.State;
using UnityEngine;

namespace CodeBase.Game
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private CardBundleData _cardBundleData;
            
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, _cardBundleData);
            
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}