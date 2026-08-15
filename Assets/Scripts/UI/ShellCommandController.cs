using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShellCommandController : MonoBehaviour
{
    public bool isOpen = true;
    private Image img;
    [SerializeField] private Sprite openCommand;
    [SerializeField] private Sprite closedCommand;
    public TextMeshProUGUI healthLabel;
    public TextMeshProUGUI killsLabel;
    public TextMeshProUGUI shellsLabel;
    public TextMeshProUGUI timeLabel;
    public TextMeshProUGUI profitLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(gameObject.name == "ShellCommand"){LevelManager.Instance.shellCommandController = this;}
        if(gameObject.name == "OptionsMenu"){LevelManager.Instance.optionsMenu = this;}
        img = GetComponent<Image>();
        if(!isOpen){isOpen = true; ToggleOpen();}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleOpen()
    {
        isOpen = !isOpen;

        img.sprite = isOpen ? openCommand : closedCommand;
        
        foreach (Transform child in transform) {
            child.gameObject.SetActive(isOpen);
        }
    }
}
