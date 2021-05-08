using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [Serializable]
    public struct Level
    {
        [SerializeField]
        private SceneField scene;
        public SceneField Scene => scene;

        [SerializeField]
        private int coffeesRequired;
        public int CoffeesRequired => coffeesRequired;
    }
    
    [SerializeField]
    private Level[] levels;
    public Level[] Levels => levels;

    public void SaveScenesToBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        foreach (Level level in levels)
        {
            EditorBuildSettingsScene scene = new EditorBuildSettingsScene(level.Scene.ScenePath, true);
            if (scenes.Any(buildSettingsScene => buildSettingsScene.path == scene.path))
                continue;

            scenes.Add(scene);
        }

        EditorBuildSettings.scenes = scenes.ToArray();
    }
}