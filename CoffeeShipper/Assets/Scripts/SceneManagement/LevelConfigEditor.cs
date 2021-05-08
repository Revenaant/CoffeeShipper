using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (LevelConfig)target;
 
        if(GUILayout.Button("Save", GUILayout.Height(40)))
        {
            script.SaveScenesToBuildSettings();
        }
    }
}
#endif
