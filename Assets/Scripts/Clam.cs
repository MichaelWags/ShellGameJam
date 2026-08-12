using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clam : Enemy
{
    //VARIABLES
    private Transform playerTransform;
    [SerializeField] private float moveRadius = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        sr.flipX = movement.x > 0f;
    }

    protected override void Move()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) < moveRadius){
            base.animator.SetBool("isMoving", true);

            movement = (playerTransform.position - transform.position).normalized;
            //movement.x = Mathf.Round(movement.x * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
            //movement.y = Mathf.Round(movement.y * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
            movement.x = movement.x * moveSpeed * Time.fixedDeltaTime;
            movement.y = movement.y * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
        else
        {
            base.animator.SetBool("isMoving", false);
        }
    }
}
