using UI.ModalViews.Controller;
using UnityEditor;
using UnityEngine;

namespace AppWideSystems.ModalViewSystem
{
    [CustomEditor(typeof(ModalViewController))]
    public class ModalViewControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetToGetChildren = (ModalViewController)target;
            if (GUILayout.Button("Update Child Modules (Editor Only)"))
            {
                targetToGetChildren.UpdateChildModules();
                EditorUtility.SetDirty(targetToGetChildren);
            }
        }
    }
}