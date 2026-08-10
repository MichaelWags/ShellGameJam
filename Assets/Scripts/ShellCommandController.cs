using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShellCommandController : MonoBehaviour
{
    private bool isOpen = true;
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
        GameManager.Instance.shellCommandController = this;
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleOpen()
    {
        Debug.Log("buttonClicked");
        isOpen = !isOpen;

        img.sprite = isOpen ? openCommand : closedCommand;
        
        foreach (Transform child in transform) {
            child.gameObject.SetActive(isOpen);
        }
    }
}
