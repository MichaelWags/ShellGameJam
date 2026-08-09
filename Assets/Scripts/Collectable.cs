using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform target;
    [SerializeField] private float speed = 1f;
    private float magnitude = 1f;
    private Vector2 movement = Vector2.up;
    private float timer = 0f;
    public float profit = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("Player").transform;
        //randomize x movement
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Debug.Log(timer += Time.deltaTime);
        magnitude = Mathf.Pow(0.5f * timer, 3f) - 1.8f * Mathf.Pow(0.5f * timer, 2f) + 1f;

        if(timer > 2.2f && target != null)
        {
            movement = (target.position - transform.position).normalized;
        }

        float distance = Vector3.Distance(target.position, transform.position);
        if(distance < 0.1f){
            Debug.Log("got collectable");
            GameManager.Instance.AddProfit(profit);
            Destroy(gameObject);
        }

        /*movement.x = Mathf.Round(movement.x * magnitude * speed * Time.fixedDeltaTime * 16f) / 16f;
        movement.y = Mathf.Round(movement.y * magnitude * speed * Time.fixedDeltaTime * 16f) / 16f;*/
        movement.x = movement.x * magnitude * speed * Time.fixedDeltaTime;
        movement.y = movement.y * magnitude * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
