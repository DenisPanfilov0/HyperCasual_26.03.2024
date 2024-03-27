using System.Collections.Generic;
using CodeBase.Config.CardData;
using CodeBase.Infrastructure.AssetManagement;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastructure.Factory
{
    internal class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private int _maxLevel;
        private int _currentLevel;
        private GameObject _gridCells;
        private TextMeshProUGUI _questionField;

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }

        public GameObject CreateCanvas() => 
            Instantiate(AssetPath.CanvasPath);

        public GameObject CreateGrid(Transform at) => 
            Instantiate(AssetPath.GridPath, at);

        public void CreateCells(CardBundleData cardBundleData, Transform at, GameObject payload)
        {
            InitialQuestionField(at);

            Instantiate(AssetPath.CellPath, at, cardBundleData, payload);
            
            //if (_gridCells == null) _gridCells = payload;
            _gridCells ??= payload;
        }

        public GameObject CreateFadeEffects() => 
            Instantiate(AssetPath.FadePath);

        public GameObject CreateRestartButton(Transform at) => 
            Instantiate(AssetPath.RestartButtonPath);

        public bool GetCurrentLevel()
        {
            if (_currentLevel < _maxLevel)
                return true;

            return false;
        }

        public GameObject ResetCurrentLevel()
        {
            _currentLevel = 0;
            return _gridCells;
        }

        public GameObject ClearGrid()
        {
            foreach (Transform child in _gridCells.transform) 
                Object.Destroy(child.gameObject);

            return _gridCells;
        }

        private void InitialQuestionField(Transform at)
        {
            _questionField ??= Instantiate(AssetPath.QuestionFieldPath, at.parent).GetComponent<TextMeshProUGUI>();
            
            _questionField.alpha = 0f;

            _questionField.DOFade(1f, 0.5f).SetEase(Ease.InOutQuad);
        }

        private GameObject Instantiate(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
            return gameObject;
        }

        private GameObject Instantiate(string prefabPath, Transform at)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, at);
            return gameObject;
        }
        
        private void Instantiate(string prefabPath, Transform at, CardBundleData cardBundleData, GameObject payload)
        {
            if (!GetCurrentLevel(cardBundleData)) return;
            
            SetupGridLayout(payload, cardBundleData);
                
            int countCell = cardBundleData.GridSizesLevel[_currentLevel].GridRows * cardBundleData.GridSizesLevel[_currentLevel].GridColumns;

            CardData currentCardData = cardBundleData.CardData[Random.Range(0, cardBundleData.CardData.Count)];

            List<int> indexes = GenerateShuffledIndex(currentCardData.Items.Count);

            bool isSelect = false;

            for (int i = 0; i < countCell; i++)
            {
                int randomIndex = indexes[i];
                    
                GameObject gameObject = _assets.Instantiate(prefabPath, at);

                CellFilling cell = SetupCellFilling(gameObject, currentCardData, randomIndex);

                if (!isSelect && !currentCardData.Items[randomIndex].IsUsed)
                {
                    SetCorrectAnswer(cell, currentCardData, randomIndex);
                    isSelect = true;
                }
                    
                ApplyBounceEffect(at, gameObject);
            }
                
            _currentLevel++;
        }

        private static void ApplyBounceEffect(Transform at, GameObject gameObject)
        {
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            gameObject.transform.SetSiblingIndex(Random.Range(0, at.childCount));
        }

        private void SetCorrectAnswer(CellFilling cell, CardData currentCardData, int randomIndex)
        {
            cell.IsCorrectAnswer = true;
            currentCardData.Items[randomIndex].IsUsed = true;
            _questionField.text = currentCardData.Items[randomIndex].Question;
        }

        private static CellFilling SetupCellFilling(GameObject gameObject, CardData currentCardData, int randomIndex)
        {
            CellFilling cell = gameObject.GetComponent<CellFilling>();

            cell.Question = currentCardData.Items[randomIndex].Question;
            cell.CardIcon.sprite = currentCardData.Items[randomIndex].Sprite;
            cell.CardIcon.transform.rotation = Quaternion.Euler(0f, 0f, -currentCardData.Items[randomIndex].AngleOfRotation);
            return cell;
        }

        private List<int> GenerateShuffledIndex(int count)
        {
            List<int> indexes = new List<int>();
                
            for (int i = 0; i < count; i++)
            {
                indexes.Add(i);
            }
    
            indexes = ShuffleList(indexes);
            return indexes;
        }

        private bool GetCurrentLevel(CardBundleData cardBundleData)
        {
            if (_currentLevel < cardBundleData.GridSizesLevel.Count)
            {
                _maxLevel = cardBundleData.GridSizesLevel.Count;
                return true;
            }
            
            return false;
        }

        private void SetupGridLayout(GameObject payload, CardBundleData cardBundleData)
        {
            var gridLayoutGroup = payload.GetComponent<GridLayoutGroup>();

            gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = cardBundleData.GridSizesLevel[_currentLevel].GridRows;
        }
        
        private List<T> ShuffleList<T>(List<T> list)
        {
            List<T> shuffledList = new List<T>(list);
            for (int i = 0; i < shuffledList.Count; i++)
            {
                int randomIndex = Random.Range(i, shuffledList.Count);
                (shuffledList[randomIndex], shuffledList[i]) = (shuffledList[i], shuffledList[randomIndex]);
            }
            
            return shuffledList;
        }
    }
}