using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Toolbars;

namespace Utils.EditorTools
{
    public static class ToolbarButtons
    {
        private const string LeftToolbarPath = "DevTools/Navigate";
        private const string OpenCSharpProjectMenuPath = "Assets/Open C# Project";

        [InitializeOnLoadMethod]
        private static void RegisterPlayModeRefresh()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        [MainToolbarElement(LeftToolbarPath, defaultDockPosition = MainToolbarDockPosition.Left, defaultDockIndex = 101)]
        private static IEnumerable<MainToolbarElement> CreateLeftToolbar()
        {
            var playLabel = EditorApplication.isPlaying ? "Stop" : "Play";

            yield return new MainToolbarButton(
                new MainToolbarContent(playLabel, "Play/Pause From StartUp"),
                PlayFromStartUp);

            yield return new MainToolbarButton(
                new MainToolbarContent("C#", "Open C# Project"),
                OpenCSharpProject);

            yield return new MainToolbarButton(
                new MainToolbarContent("Main", "Open main scene"),
                OpenMainScene);
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange _)
        {
            MainToolbar.Refresh(LeftToolbarPath);
        }

        private static void PlayFromStartUp()
        {
            EditorStartUp.PlayFromStartUp();
        }

        private static void OpenCSharpProject()
        {
            EditorApplication.ExecuteMenuItem(OpenCSharpProjectMenuPath);
        }

        private static void OpenMainScene()
        {
            EditorStartUp.OpenMainScene();
        }
    }
}
