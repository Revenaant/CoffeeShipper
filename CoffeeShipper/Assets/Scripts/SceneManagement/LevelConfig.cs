using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField]
    private SceneField[] levels;
    public SceneField[] Levels => levels;

    public void SaveScenesToBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        foreach (SceneField level in levels)
        {
            EditorBuildSettingsScene scene = new EditorBuildSettingsScene(level.ScenePath, true);
            if (scenes.Any(buildSettingsScene => buildSettingsScene.path == scene.path))
                continue;

            scenes.Add(scene);
        }

        EditorBuildSettings.scenes = scenes.ToArray();
    }
}