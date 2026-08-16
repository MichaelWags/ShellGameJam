using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public static SceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static SceneManager instance = null;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
