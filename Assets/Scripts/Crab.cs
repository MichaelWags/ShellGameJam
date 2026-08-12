using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Crab : Enemy
{
    //VARIABLES
    [SerializeField] private float moveXDir = 0f;
    [SerializeField] private float moveYDir = 0f;
    private LayerMask crabLayerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        crabLayerMask = ~LayerMask.GetMask("Players", "Spawners");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        //movement.x = Mathf.Round(moveXDir * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        //movement.y = Mathf.Round(moveYDir * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        movement.x = moveXDir * moveSpeed * Time.fixedDeltaTime;
        movement.y = moveYDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*Vector2 normal = collision.contacts[0].normal;

        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
        {
            moveXDir = -moveXDir;
        }
        else
        {
            moveYDir = -moveYDir;
        }*/

        moveXDir = -moveXDir;
        moveYDir = -moveYDir;
    }
}
