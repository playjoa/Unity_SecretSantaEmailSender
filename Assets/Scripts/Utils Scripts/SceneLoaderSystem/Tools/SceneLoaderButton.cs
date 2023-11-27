using UI.ModalViews.Controller;
using UI.ModalViews.Data;
using UI.ModalViews.Data.DataBuilders;
using UnityEngine;
using UnityEngine.UI;
using Utils.SceneLoader.Controller;
using Utils.SceneLoader.Data;

namespace Utils.Loader.Tools
{
    [RequireComponent(typeof(Button))]
    public class SceneLoaderButton : MonoBehaviour
    {
        [Header("Scene Target")] 
        [SerializeField] private GameScene targetGameScene;

        [Header("Button Config.")] 
        [SerializeField] private Button button;

        [Header("Configuration")] 
        [SerializeField] private bool showConfirmSceneChangePopUp = false;

        private void OnValidate()
        {
            if (button != null)
                button = GetComponent<Button>();
        }

        private void Awake()
        {
            button.onClick.AddListener(OnLoadSceneButtonClickHandler);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnLoadSceneButtonClickHandler);
        }
        
        private void LoadScene()
        {
            SceneLoaderController.ME.LoadScene(targetGameScene);
        }

        private void OnLoadSceneButtonClickHandler()
        {
            if (!showConfirmSceneChangePopUp)
            {
                LoadScene();
                return;
            }
            
            var modalDataBuilder = new ConfirmCancelModalPackageDataBuilder()
                .AddConfirmAction(LoadScene)
                .AddTitle("Scene Change")
                .AddDescription($"Are you sure you want to move to scene {targetGameScene}?");
            
            ModalViewController.ME.RequestModalView(modalDataBuilder.Build());
        }
    }
}
