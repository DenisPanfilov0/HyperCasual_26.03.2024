using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase
{
    public class CellFilling : MonoBehaviour
    {
        [SerializeField] private Button _gameCellButton;
        
        [SerializeField] private Image _cardIcon;

        [SerializeField] private string _question;
        
        [SerializeField] private bool _isCorrectAnswer = false;

        [SerializeField] private GameObject _particleStar;
        
        private bool _isAnimating = false;


        public Action OnRightButtonClick;
        
        public Image CardIcon => _cardIcon;

        public bool IsCorrectAnswer { set => _isCorrectAnswer = value; }

        public string Question { set => _question = value; }

        
        private void Start() => 
            _gameCellButton.onClick.AddListener(OnClickButton);

        private void OnDestroy() => 
            _gameCellButton.onClick.RemoveListener(OnClickButton);

        private void OnClickButton()
        {
            if (!_isAnimating)
            {
                if (_isCorrectAnswer)
                {
                    ActivateParticle();
                    
                    PlayCorrectAnswerAnimation();
                }
                else
                {
                    PlayIncorrectAnswerAnimation();
                }
            }
        }

        private void PlayIncorrectAnswerAnimation()
        {
            _isAnimating = true;

            _cardIcon.rectTransform.DOShakePosition(duration: 1f, strength: new Vector3(20f, 0f, 0f), vibrato: 10, randomness: 90, fadeOut: true)
                .SetEase(Ease.InBounce)
                .OnComplete(() =>
                {
                    _isAnimating = false;
                });
        }

        private void PlayCorrectAnswerAnimation()
        {
            _isAnimating = true;

            _cardIcon.rectTransform.DOPunchScale(new Vector3(0.3f, 0.3f, 0f), duration: 1f, vibrato: 5, elasticity: 1f)
                .OnComplete(() =>
                {
                    _isAnimating = false;
                    OnRightButtonClick?.Invoke();
                });
        }

        private void ActivateParticle() => 
            _particleStar.SetActive(true);
    }
}