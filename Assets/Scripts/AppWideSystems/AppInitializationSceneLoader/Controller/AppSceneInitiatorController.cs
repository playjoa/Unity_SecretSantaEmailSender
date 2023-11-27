using System.Collections;
using AppWide.AppInitialization.Controller;
using AppWideSystems.AppInitialization.Interfaces;
using Utils_Scripts.Patterns;
using Utils.SceneLoader.Controller;
using Utils.SceneLoader.Data;

namespace AppWideSystems.AppInitializationSceneLoader.Controller
{
    public class AppSceneInitiatorController : MonoBehaviourSingleton<AppSceneInitiatorController>, IAppWideLogInSystem
    {
        public string AppSystemName => "Scene Initiator";
        
        public IEnumerator Initiate(AppInitializationController appInitializationController)
        {
            yield return true;
            SceneLoaderController.ME.LoadScene(GameScene.Main);
        }
    }
}