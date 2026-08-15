using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    //Information
    public int levelIndex = 0;
    [SerializeField] private GameObject levelInfoUI;
    private Level level;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = SaveData.Instance.GetLevel(levelIndex);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player here");
            levelInfoUI.SetActive(true);
            levelInfoUI.GetComponentInChildren<TextMeshProUGUI>().text = levelIndex + ". " + level.name + "   Shells: " + level.collectedShells + "/" + level.shells.Count;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            levelInfoUI.SetActive(false);
        }
    }
}
