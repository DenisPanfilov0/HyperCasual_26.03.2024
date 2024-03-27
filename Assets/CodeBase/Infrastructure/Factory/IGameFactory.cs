using CodeBase.Config.CardData;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateCanvas();
        
        GameObject CreateGrid(Transform at);
        
        void CreateCells(CardBundleData cardBundleData, Transform at, GameObject payload);
        
        GameObject CreateFadeEffects();
        
        GameObject CreateRestartButton(Transform at);
        
        bool GetCurrentLevel();
        
        GameObject ResetCurrentLevel();
        
        GameObject ClearGrid();
    }
}