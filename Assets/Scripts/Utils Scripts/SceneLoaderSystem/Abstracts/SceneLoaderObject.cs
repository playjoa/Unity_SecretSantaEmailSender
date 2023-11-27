using UnityEngine;
using Utils.SceneLoader.Controller;
using Utils.SceneLoader.Data;

namespace Utils.SceneLoader.Abstracts
{
    public abstract class SceneLoaderObject : MonoBehaviour
    {
        private void Start()
        {
            SceneLoaderController.ME.OnStartedToLoadScene += OnStartedToLoadSceneHandler;
            SceneLoaderController.ME.OnSceneLoaded += OnSceneLoadedHandler;
            OnSetup();
        }

        private void OnDestroy()
        {
            SceneLoaderController.ME.OnStartedToLoadScene -= OnStartedToLoadSceneHandler;
            SceneLoaderController.ME.OnSceneLoaded -= OnSceneLoadedHandler;
            OnCleanUp();
        }
        
        protected virtual void OnSetup()
        {
        }
        
        protected virtual void OnCleanUp()
        {
        }

        protected virtual void OnStartedToLoadSceneHandler(LoadingSceneData loadingSceneData)
        {
        }
        
        protected virtual void OnSceneLoadedHandler(LoadingSceneData loadingSceneData)
        {
        }
    }
}