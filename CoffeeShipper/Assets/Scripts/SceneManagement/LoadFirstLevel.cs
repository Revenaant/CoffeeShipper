using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFirstLevel : MonoBehaviour
{
    [SerializeField]
    private SceneField firstLevel;
    
    void Start()
    {
        SceneManager.LoadScene(firstLevel.ScenePath, LoadSceneMode.Single);
    }
}
