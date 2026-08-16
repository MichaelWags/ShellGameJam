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
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = SaveData.Instance.GetLevel(levelIndex);
        animator = GetComponent<Animator>();
        animator.SetBool("wasBeat", level.wasBeat);



        animator.SetBool("allShells", level.CollectedShells() >= level.shells.Count);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player here");
            levelInfoUI.SetActive(true);
            levelInfoUI.GetComponentInChildren<TextMeshProUGUI>().text = levelIndex + ". " + level.name + "   Shells: " + level.CollectedShells() + "/" + level.shells.Count;
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
