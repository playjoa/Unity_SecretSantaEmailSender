using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace Utils.EditorTools
{
    public static class ToolbarButtons
    {
        private const float ButtonWidth = 40f;
        private const float ButtonHeight = 22f;
        
        private static GUIStyle _buttonStyle;
        private static GUIStyle _gameplayButtonStyle;
        
        private static GUIContent _openProjectIcon;
        private static GUIContent _mainSceneIcon;
        
        private static bool EditorPlaying => EditorApplication.isPlaying;
        private static string PlayPauseButtonInfo => EditorPlaying ? "Stop" : "Play";
        
        private static GUIStyle ButtonStyle => _buttonStyle ??= new GUIStyle(EditorStyles.toolbarButton)
        {
            fixedWidth = ButtonWidth,
            fixedHeight = ButtonHeight,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        
        private static GUIContent OpenProjectIcon => _openProjectIcon ??= new GUIContent("C#", "Open C# Project");
        private static GUIContent PlayFromStartUpIcon => new(PlayPauseButtonInfo, "Play/Pause From StartUp");
        private static GUIContent MainSceneIcon => _mainSceneIcon ??= new GUIContent("Main", "Open main scene");

        [InitializeOnLoadMethod]
        private static void Setup() => ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);

        private static void OnLeftToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            
            BuildButton(PlayFromStartUpIcon, ButtonStyle, EditorStartUp.PlayFromStartUpInfo);
            BuildButton(OpenProjectIcon, ButtonStyle, "Assets/Open C# Project");
            
            GUILayout.Space(8f);
            
            BuildButton(MainSceneIcon, ButtonStyle, EditorStartUp.OpenMainSceneInfo);
            
            GUILayout.FlexibleSpace();
        }

        private static void BuildButton(GUIContent content, GUIStyle style, string info)
        {
            if (GUILayout.Button(content, style))
                EditorApplication.ExecuteMenuItem(info);
        }
    }
}