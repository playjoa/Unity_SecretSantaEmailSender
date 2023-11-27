using AppWide.AppInitialization.Controller;
using AppWideSystems.AppInitialization.Interfaces;
using TMPro;
using UnityEngine;

namespace AppWideSystems.AppInitialization.UI
{
    public class AppInitializationLoadingText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingTMP;
        
        private void Awake()
        {
            AppInitializationController.OnAppSystemStartedLoading += OnStartedLoadingSystemHandler;
        }

        private void OnDestroy()
        {
            AppInitializationController.OnAppSystemStartedLoading -= OnStartedLoadingSystemHandler;
        }
        
        private void OnStartedLoadingSystemHandler(IAppWideSystem loadingSystem)
        {
            loadingTMP.text = $"Loading {loadingSystem.AppSystemName}...";
        }
    }
}