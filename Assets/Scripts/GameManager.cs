using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Clear duplicates
            return;
        }

        Instance = this;
    }

    public float playerHealth = 10f;
    public float playerHealthMax = 10f;
    public int kills = 0;
    public float gameTime = 0f;
    public float profit = 0f;

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += AddKill;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= AddKill;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        UpdateShellCommandUI();
    }

    private void AddKill()
    {
        kills ++;
    }

    private void AddProfit(float profitToAdd)
    {
        
    }

    public ShellCommandController shellCommandController;

    private void UpdateShellCommandUI()
    {
        //Health
        shellCommandController.healthLabel.text = "Health: " + playerHealth + " / " + playerHealthMax;

        //kills
        shellCommandController.killsLabel.text = "Kills: " + kills;

        //timer
        int seconds = (int)gameTime % 60;
        int minutes = (int)gameTime / 60;
        shellCommandController.timeLabel.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);

        //profit
        shellCommandController.profitLabel.text = "Profit: $" + profit;
    }
}
