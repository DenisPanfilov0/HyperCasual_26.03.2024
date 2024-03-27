using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Config.CardData
{
    [CreateAssetMenu(fileName = "CardBundleData", menuName = "Config/Create Card Bundle Data", order = 2)]
    public class CardBundleData : ScriptableObject
    {
        [SerializeField] private List<GridSizeLevel> _gridSizesLevel;
        
        [SerializeField] private List<CardData> _cardData;

        public List<GridSizeLevel> GridSizesLevel => _gridSizesLevel;

        public List<CardData> CardData => _cardData;

        
        [System.Serializable]
        public class GridSizeLevel
        {
            [SerializeField] private int _gridRows;
            
            [SerializeField] private int _gridColumns;

            public int GridRows => _gridRows;

            public int GridColumns => _gridColumns;
        }
    }
}