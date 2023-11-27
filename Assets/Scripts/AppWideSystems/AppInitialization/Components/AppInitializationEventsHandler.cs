using AppWide.AppInitialization.Controller;
using AppWideSystems.AppInitialization.Interfaces;
using UI.ModalViews.Controller;
using UI.ModalViews.Data.DataBuilders;
using UnityEngine;

namespace AppWideSystems.AppInitialization.Components
{
    public class AppInitializationEventsHandler : MonoBehaviour
    {
        private void Awake()
        {
            AppInitializationController.OnAppInitializationFailed += OnAppInitializationError;
        }

        private void OnDestroy()
        {
            AppInitializationController.OnAppInitializationFailed -= OnAppInitializationError;
        }
        
        private void OnAppInitializationError(IAppWideSystem failedSystem, string errorMessage)
        {
            var modalBuilder = new InformationModalDataBuilder()
                .AddConfirmAction(QuitApp)
                .AddButtonText("Exit")
                .AddTitle("Initialization Error")
                .AddDescription($"There was a problem loading {failedSystem.AppSystemName}! \n {errorMessage}");
            
            ModalViewController.ME.RequestModalView(modalBuilder.Build());
        }

        private void QuitApp()
        {
            AppInitializationController.QuitGameApp();
        }
    }
}