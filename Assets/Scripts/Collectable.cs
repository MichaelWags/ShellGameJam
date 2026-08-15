using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform target;
    [SerializeField] private float speed = 5f;
    private float magnitude = 1f;
    private Vector2 movement = Vector2.up;
    private float timer = 0f;
    public float profit = 3f;
    public bool isShell = false;
    private float randXMag = 0f;
    public int shellIndex;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("Player").transform;
        //randomize x movement
        randXMag = isShell ? 0f : Random.Range(-0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        magnitude = 1.8f * Mathf.Pow(2f*timer - 1f, 2f);

        if(timer > 0.5f && target != null || timer > 3f)
        {
            movement = (target.position - transform.position).normalized;

            float distance = Vector3.Distance(target.position, transform.position);

            if(distance < 0.5f || timer > 3f){ //if close enough to player, collect
                LevelManager.Instance.AddProfit(profit);
                if(isShell){LevelManager.Instance.AddShell(); SaveData.Instance.levelProgress.levels[LevelManager.Instance.levelIndex].shells[shellIndex].wasCollected = true;}
                Destroy(gameObject);
            }
        }
        else
        {
            movement.x = randXMag;
            movement.y = 1f;
            movement = movement.normalized;
        }

        //movement.x = Mathf.Round(movement.x * magnitude * speed * Time.fixedDeltaTime * 16f) / 16f;
        //movement.y = Mathf.Round(movement.y * magnitude * speed * Time.fixedDeltaTime * 16f) / 16f;
        movement.x = movement.x * magnitude * speed * Time.fixedDeltaTime;
        movement.y = movement.y * magnitude * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
