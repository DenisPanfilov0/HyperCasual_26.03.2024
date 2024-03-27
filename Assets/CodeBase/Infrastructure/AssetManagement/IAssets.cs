using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        GameObject Instantiate(string path);
        
        GameObject Instantiate(string path, Transform at);
    }
}