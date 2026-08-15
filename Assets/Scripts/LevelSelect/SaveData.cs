using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance
    {
        get
        {
            return instance;
        }
    }

    private static SaveData instance = null;
    public LevelProgress levelProgress = new LevelProgress();

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        OnLoad();

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        
    }

    public void OnSave()
    {
        string levelData = JsonUtility.ToJson(levelProgress);
        string filePath = Application.persistentDataPath + "/LevelData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, levelData);
        Debug.Log("LevelData Saved");
    }

    public void OnLoad()
    {
        string filePath = Application.persistentDataPath + "/LevelData.json";
        string levelData = System.IO.File.ReadAllText(filePath);

        levelProgress = JsonUtility.FromJson<LevelProgress>(levelData);
        Debug.Log("LevelData Loaded");
    }

    public Level GetLevel(int index)
    {
        return levelProgress.levels[index];
    }
}

[System.Serializable]
public class LevelProgress
{
    public List<Level> levels = new List<Level>();
}

[System.Serializable]
public class Level
{
    public int collectedShells = 0;
    public string name;
    public bool wasBeat = false;
    public bool isSelectable = true;
    public List<Shells> shells = new List<Shells>();
}

[System.Serializable]
public class Shells
{
    public bool wasCollected = false;
}
