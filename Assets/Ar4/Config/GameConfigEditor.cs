using UnityEditor;
using UnityEngine;

namespace Ar4.Config
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GameConfig))]
    public class GameConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gameConfig = (GameConfig)target;
            if(GUILayout.Button("Generate ids"))
                gameConfig.GenerateIds();
        }
    }
#endif
}