using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Snail : Enemy
{
    //VARIABLES
    [SerializeField] private float moveXDir = 0f;
    [SerializeField] private float moveYDir = 0f;
    LayerMask snailLayerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        snailLayerMask = ~LayerMask.NameToLayer("Players");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Move()
    {
        //movement.x = Mathf.Round(moveXDir * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        //movement.y = Mathf.Round(moveYDir * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        movement.x = moveXDir * moveSpeed * Time.fixedDeltaTime;
        movement.y = moveYDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collide");
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, new Vector2(moveYDir, -moveXDir), 1f, snailLayerMask); //check to the right (relative) of snail
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, new Vector2(-moveYDir, moveXDir), 1f, snailLayerMask); //check to the left (relative) of snail

        Debug.Log("right " + rightHit + " left " + leftHit);

        if (!rightHit)//move right relative
        {
            float newMoveXDir = moveYDir;
            float newMoveYDir = -moveXDir;
            moveXDir = newMoveXDir;
            moveYDir = newMoveYDir;
        }
        else if (!leftHit)//move left relative
        {
            float newMoveXDir = -moveYDir;
            float newMoveYDir = moveXDir;
            moveXDir = newMoveXDir;
            moveYDir = newMoveYDir;
        }
        else //reverse
        {
            moveXDir = -moveXDir;
            moveYDir = -moveYDir;
        }
    }
}
