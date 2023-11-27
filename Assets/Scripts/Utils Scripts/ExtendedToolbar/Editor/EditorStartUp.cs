using UnityEditor;
using UnityEditor.SceneManagement;

namespace Utils.EditorTools
{
    public static class EditorStartUp
    {
        private const string StartUpScenePath = "Assets/Scenes/_StartUp.unity";
        private const string MainScenePath = "Assets/Scenes/Main.unity";
        
        public const string PlayFromStartUpInfo = "DevTools/Navigate/Play-Stop From StartUp %e";
        public const string OpenStartUpInfo = "DevTools/Navigate/Open StartUp %l";
        public const string OpenMainSceneInfo = "DevTools/Navigate/Open Main Scene %m";

        [MenuItem(PlayFromStartUpInfo)]
        public static void PlayFromStartUp()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(StartUpScenePath);
            EditorApplication.isPlaying = true;
        }
        
        [MenuItem(OpenStartUpInfo)]
        public static void OpenStartUp()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(StartUpScenePath);
        }
        
        [MenuItem(OpenMainSceneInfo)]
        public static void OpenMainScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(MainScenePath);
        }
    }
}

