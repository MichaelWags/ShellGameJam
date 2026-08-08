using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework;

public class PlayerController : MonoBehaviour, IDamageable
{
    //STATS
    [SerializeField] private float health = 10f;
    [SerializeField] private float knockbackMagnitude = 1f;
    private Vector2 knockback;

    //PLAYER MOVEMENT
    public float moveSpeed = 5f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Sprite sprite_walk;
    public Sprite sprite_draw;

    //DRAWING
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject attackPrefab;
    private List<Vector2> points = new List<Vector2>();
    private List<GameObject> pointPrefabs = new List<GameObject>();
    private LineRenderer lineRenderer;
    private bool isDrawing;
    [SerializeField] private float drawPointFrequency = 0.5f;
    Gradient gradient;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite_walk;
        lineRenderer = GetComponent<LineRenderer>();
        gradient = lineRenderer.colorGradient;

        playerControls.Movement.Draw.performed += OnDrawPerformed;
        playerControls.Movement.Draw.canceled += OnDrawCanceled;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        //get input
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;

        sr.flipX = movement.x > 0;
    }

    private void FixedUpdate()
    {
        Move();
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
        sr.sprite = sprite_draw;
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
        sr.sprite = sprite_walk;

        if(points.Count > 2 && Vector2.Distance(points[0], points[points.Count - 1]) < 1f) //if >2 points and ends meet
        {
            points[points.Count - 1] = points[0];
            GameObject attack = Instantiate(attackPrefab);
            attack.GetComponent<PolygonCollider2D>().SetPath(0, points);
        } else
        {
            Debug.Log("invalid attack");
        }

        StartCoroutine(FadeLines());

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
        if(health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("player hit");
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
