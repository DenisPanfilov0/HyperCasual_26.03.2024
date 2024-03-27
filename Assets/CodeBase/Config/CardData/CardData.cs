using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Config.CardData
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Config/Create Card Data", order = 1)]
    public class CardData : ScriptableObject
    {
        [SerializeField] private List<CategoryItem> _items;

        public List<CategoryItem> Items => _items;
    }
    
    [System.Serializable]
    public class CategoryItem
    {
        [SerializeField] private string _question;
        
        [SerializeField] private Sprite _sprite;
        
        [SerializeField] private float _angleOfRotation;
        
        [SerializeField] private bool _isUsed = false;

        
        public string Question => _question;
        
        public Sprite Sprite => _sprite;
        
        public float AngleOfRotation => _angleOfRotation;
        
        public bool IsUsed
        {
            get => _isUsed;
            set => _isUsed = value;
        }
    }


}