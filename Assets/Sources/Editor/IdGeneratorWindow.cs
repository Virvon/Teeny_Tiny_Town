using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Sources.Editor
{
    public class IdGeneratorWindow : EditorWindow
    {
        private string _id;

        [MenuItem("Window/ID generator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(IdGeneratorWindow));
        }

        public void OnGUI()
        {
            EditorGUILayout.TextField("ID", _id);

            if (GUILayout.Button("Generate ID"))
            {
                _id = Guid.NewGuid().ToString();
            }
        }
    }
}
