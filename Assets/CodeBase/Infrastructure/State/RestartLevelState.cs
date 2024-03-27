using System;
using CodeBase.Config.CardData;
using CodeBase.Infrastructure.Factory;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.State
{
    public class RestartLevelState : IState
    {
        private readonly GameStateMachine _stateMachine;
        
        private readonly IGameFactory _gameFactory;
        
        private readonly CardBundleData _cardBundleData;
        
        private GameObject _grid;
        
        private GameObject _fade;
        
        private GameObject _restartButton;

        public RestartLevelState(GameStateMachine stateMachine, IGameFactory gameFactory, CardBundleData cardBundleData)
        {
            _stateMachine = stateMachine;
            _gameFactory = gameFactory;
            _cardBundleData = cardBundleData;
        }

        public void Enter()
        {
            _grid = _gameFactory.ResetCurrentLevel();

            ResetConfigCollection();

            CreateFade();

            ChangeFading(0.5f, 1f, _fade);
            
            CreateRestartButton(_grid.transform);
        }

        public void Exit()
        {
        }

        private void CreateFade()
        {
            _fade = _gameFactory.CreateFadeEffects();
            
            _fade.transform.SetParent(_grid.transform.parent, false);
        }

        private void RestartLevel(GameObject grid)
        {
            ChangeFading(1f, 1f, _fade, ChangeStateMachine);
            
            _restartButton.transform.DOMoveY(_restartButton.transform.position.y - 2000f, 1f).SetEase(Ease.OutQuad);
        }

        private void ChangeStateMachine()
        {
            ChangeFading(0f, 1f, _fade, ClearUIObject);
            
            Object.Destroy(_restartButton);

            _stateMachine.Enter<LoadNextLevelState>();
        }

        private void ClearUIObject()
        {
            Object.Destroy(_fade);
            Object.Destroy(_restartButton);
        }

        private void ChangeFading(float count, float duration, GameObject fade, Action onChangeFade = null)
        {
            Image fadeImage = fade.GetComponent<Image>();

            fadeImage.DOFade(count, duration).OnComplete(() => {
                onChangeFade?.Invoke();
            });
        }

        private void CreateRestartButton(Transform grid)
        {
            _restartButton = _gameFactory.CreateRestartButton(grid.transform);
            
            _restartButton.transform.SetParent(grid.transform.parent, false);
            
            _restartButton.GetComponent<Button>().onClick.AddListener(() => RestartLevel(grid.gameObject));
            
            ChangeFading(0.6f, 1f, _fade);
        }

        private void ResetConfigCollection()
        {
            foreach (var cardData in _cardBundleData.CardData)
            {
                foreach (var item in cardData.Items)
                {
                    if (item.IsUsed) item.IsUsed = false;
                }
            }
        }
    }
}