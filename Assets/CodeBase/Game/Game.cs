using CodeBase.Config.CardData;
using CodeBase.Infrastructure.State;
using CodeBase.Services;

namespace CodeBase.Game
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, CardBundleData cardBundleData)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), cardBundleData, AllServices.Container);
        }
    }
}