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

    public float playerHealth = 0f;
    public float playerHealthMax = 0f;
    public int kills = 0;
    public int shells = 0;
    public float gameTime = 0f;
    public float profit = 0f;
    public bool isPaused = false;
    [SerializeField] private AudioClip collectSFX;

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

    public void StartGame()
    {
        playerHealth = 0f;
        playerHealthMax = 0f;
        kills = 0;
        shells = 0;
        gameTime = 0f;
        profit = 0f;
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        if(shellCommandController != null){UpdateShellCommandUI();}
    }

    private void AddKill(float enemyProfit)
    {
        kills ++;
        AddProfit(enemyProfit);
    }
    public void AddShell()
    {
        shells ++;
    }

    public void AddProfit(float profitToAdd)
    {
        GetComponent<AudioSource>().PlayOneShot(collectSFX);
        profit += profitToAdd;
    }

    public ShellCommandController shellCommandController;

    private void UpdateShellCommandUI()
    {
        //Health
        shellCommandController.healthLabel.text = "Health: " + playerHealth + " / " + playerHealthMax;

        //kills
        shellCommandController.killsLabel.text = "Kills: " + kills;

        //shells
        shellCommandController.shellsLabel.text = "Shells: " + shells;

        //timer
        int seconds = (int)gameTime % 60;
        int minutes = (int)gameTime / 60;
        shellCommandController.timeLabel.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);

        //profit
        shellCommandController.profitLabel.text = "Profit: $" + profit;
    }
}
