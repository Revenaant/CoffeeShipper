using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if  UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [Serializable]
    public struct Level
    {
        // Index is unnecessary since we access levels through array index already, but might as well make sure.
        [SerializeField]
        private int levelIndex;
        public int LevelIndex => levelIndex;
        
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

    public int GetNextLevelIndex(int currentIndex)
    {
        if (currentIndex >= levels.Length - 1)
            return -1;

        return levels[currentIndex + 1].LevelIndex;
    }

#if  UNITY_EDITOR
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
#endif
}
