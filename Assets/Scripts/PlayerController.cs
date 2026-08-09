using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework;

public class PlayerController : MonoBehaviour, IDamageable
{
    //STATS
    private float health = 10f;
    [SerializeField] private float healthMax = 10f;
    [SerializeField] private float knockbackMagnitude = 1f;

    //PLAYER MOVEMENT
    [SerializeField] private float baseMoveSpeed = 4f;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float drawSpeedMult = 1.3f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    public bool canMove = true;
    private ParticleSystem particleSystem;
    private Vector2 knockback;

    //DRAWING
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject attackPrefab;
    private List<Vector2> points = new List<Vector2>();
    private List<GameObject> pointPrefabs = new List<GameObject>();
    private LineRenderer lineRenderer;
    private bool isDrawing = false;
    [SerializeField] private float drawPointFrequency = 0.5f;
    Gradient gradient;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
        gradient = lineRenderer.colorGradient;
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();

        playerControls.Movement.Draw.performed += OnDrawPerformed;
        playerControls.Movement.Draw.canceled += OnDrawCanceled;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = healthMax;
        GameManager.Instance.playerHealth = health;
        GameManager.Instance.playerHealthMax = healthMax;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        moveSpeed = isDrawing ? baseMoveSpeed * drawSpeedMult : baseMoveSpeed;
        
        //get input
        movement = playerControls.Movement.Move.ReadValue<Vector2>()/*.normalized*/;
        //unNormalize
        movement.x = movement.x > 0f ? 1f : movement.x;
        movement.x = movement.x < 0f ? -1f : movement.x;
        movement.y = movement.y > 0f ? 1f : movement.y;
        movement.y = movement.y < 0f ? -1f : movement.y;

        sr.flipX = movement.x > 0;
        animator.SetBool("isDrawing", isDrawing);
        animator.SetBool("isMoving", movement.magnitude > 0);        
    }

    private void FixedUpdate()
    {
        if(canMove){Move();}
    }

    private void LateUpdate()
    {
        /*Vector2 snapped;
        snapped.x = Mathf.Round(rb.position.x * 16f) / 16f;
        snapped.y = Mathf.Round(rb.position.y * 16f) / 16f;
        rb.MovePosition(rb.position + snapped);*/
    }

    private void Move()
    {
        movement.x = Mathf.Round(movement.x * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        movement.y = Mathf.Round(movement.y * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        knockback.x = Mathf.Round(knockback.x * 16f) / 16f;
        knockback.y = Mathf.Round(knockback.y * 16f) / 16f;
        rb.MovePosition(rb.position + movement - knockback);
        knockback = Vector2.zero;
    }

    private void OnDrawPerformed(InputAction.CallbackContext context)
    {
        isDrawing = true;
        StartCoroutine(Draw());
    }

    private IEnumerator Draw()
    {
        while(isDrawing)
        {
            pointPrefabs.Add(Instantiate(pointPrefab, transform.position, transform.rotation)); //create points and add to list
            points.Add(rb.position);

            //connect the dots
            lineRenderer.enabled = true;
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count-1, points[points.Count-1]);
            yield return new WaitForSeconds(drawPointFrequency);
        }
    }

    private void OnDrawCanceled(InputAction.CallbackContext context)
    {
        StartCoroutine(Draw());
        isDrawing = false;

        if(points.Count > 2 && Vector2.Distance(points[0], points[points.Count - 1]) < 1f) //if >2 points and ends meet
        {
            points[points.Count - 1] = points[0];
            GameObject attack = Instantiate(attackPrefab);
            attack.GetComponent<PolygonCollider2D>().SetPath(0, points);
            lineRenderer.enabled = false;
        } else
        {
            Debug.Log("invalid attack");
            points.Clear();
            StartCoroutine(FadeLines());
        }

        foreach (GameObject obj in pointPrefabs)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        points.Clear();
        pointPrefabs.Clear();
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        GameManager.Instance.playerHealth = health;
        if(health <= 0f)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        canMove = false;
        particleSystem.Play();
        animator.SetTrigger("wasKilled");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("player hit");
            animator.SetTrigger("wasHurt");
            GameObject enemy = collision.gameObject;
            TakeDamage(enemy.GetComponent<Enemy>().attackPower);
            knockback = enemy.transform.position - this.transform.position;
            knockback = knockback.normalized * knockbackMagnitude;
        }
    }

    private IEnumerator FadeLines()
    {
        float alphaPercent = 1f;
        while(alphaPercent > 0f)
        {
            if (isDrawing){break;}
            yield return new WaitForSeconds(0.01f);
            alphaPercent -= 0.02f; //decay alpha
        
            //fade lines
            gradient.SetAlphaKeys(new GradientAlphaKey[] {
                new GradientAlphaKey(alphaPercent, 0.0f),
                new GradientAlphaKey(alphaPercent, 1.0f)
            });
            lineRenderer.colorGradient = gradient;
        }

        lineRenderer.enabled = false;

        //reset line gradient alpha
        gradient.SetAlphaKeys(new GradientAlphaKey[] {
            new GradientAlphaKey(1.0f, 0.0f),
            new GradientAlphaKey(1.0f, 1.0f)
        });
        lineRenderer.colorGradient = gradient;
    }
}
