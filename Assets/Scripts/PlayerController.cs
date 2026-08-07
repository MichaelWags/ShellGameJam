using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //PLAYER MOVEMENT
    public float moveSpeed = 5f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //ATTACKING
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private List<Vector2> points;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
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
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        //normalize input
        if (movement.sqrMagnitude > 1)
        {
            movement = movement.normalized;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        movement.x = Mathf.Round(movement.x * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        movement.y = Mathf.Round(movement.y * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        rb.MovePosition(rb.position + movement);
    }

    private void OnDraw() // will change to automatic intervals rather than button press
    {
        Instantiate(pointPrefab, transform.position, transform.rotation); //show points
        points.Add(rb.position);

        //connect the dots
        lineRenderer.enabled = true;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count-1, points[points.Count-1]);
    }

    private void OnAttack()
    {
        if(Vector2.Distance(points[0], points[points.Count - 1]) < 1f) //make sure ends meet
        {
            lineRenderer.enabled = false;
            points[points.Count - 1] = points[0];
            GameObject attack = Instantiate(attackPrefab);
            attack.GetComponent<PolygonCollider2D>().SetPath(0, points);
            points.Clear();
        } else
        {
            Debug.Log("invalid attack");
        }
    }
}
