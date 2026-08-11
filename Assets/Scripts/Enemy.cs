using System.Collections;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Enemy : MonoBehaviour, IDamageable
{
    //VARIABLES
    [SerializeField] protected float health = 3f;
    [SerializeField] public float attackPower = 1;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float enemyProfit = 10f;
    protected Vector2 movement = new Vector2(0f, 0f);
    protected bool canMove = true;

    //COMPONENTS
    protected Animator animator;
    protected ParticleSystem particleSystem;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    void Awake()
    {
        animator = GetComponent<Animator>();
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(canMove){Move();}
    }

    public virtual void Move()
    {
        //
    }

    public virtual void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            animator.SetTrigger("wasHurt");   
        }
    }

    public static event Action<float> OnEnemyKilled;

    private IEnumerator Die()
    {
        OnEnemyKilled?.Invoke(enemyProfit);
        attackPower = 0f;
        canMove = false;
        particleSystem.Play();
        GetComponent<AudioSource>().Play();
        animator.SetTrigger("wasKilled");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
