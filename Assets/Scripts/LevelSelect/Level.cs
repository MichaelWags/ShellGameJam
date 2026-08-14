using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    //Information
    public int level = 0;
    public string levelName = "temp";
    public bool isSelectable = true;
    private GameObject levelInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelInfo = GameObject.Find("LevelInfo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("player here");
        if (other.CompareTag("Player"))
        {
            levelInfo.SetActive(true);
            levelInfo.GetComponentInChildren<TextMeshProUGUI>().text = level + ". " + levelName;
        }
    }
}
