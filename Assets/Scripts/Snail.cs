using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Snail : Enemy
{
    //VARIABLES
    [SerializeField] private float moveXDir = 0f;
    [SerializeField] private float moveYDir = 0f;
    private LayerMask snailLayerMask;
    [SerializeField] private GameObject snailShellPrefab;
    [SerializeField] private Sprite snailShellSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        snailLayerMask = ~LayerMask.GetMask("Players", "Spawners");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        sr.flipX = movement.x < 0f;

        animator.SetBool("isSideways", moveXDir != 0f);
        animator.SetBool("isDown", moveYDir < 0f);
        animator.SetBool("isUp", moveYDir > 0f);
    }

    protected override void Move()
    {
        //movement.x = Mathf.Round(moveXDir * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        //movement.y = Mathf.Round(moveYDir * moveSpeed * Time.fixedDeltaTime * 16f) / 16f;
        movement.x = moveXDir * moveSpeed * Time.fixedDeltaTime;
        movement.y = moveYDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);

        GameObject drop = Instantiate(snailShellPrefab, transform.position, Quaternion.identity);
        drop.GetComponent<SpriteRenderer>().sprite = snailShellSprite;
    }

    void FixedUpdate()
    {
        RaycastHit2D forwardHit = Physics2D.Raycast(transform.position, new Vector2(moveXDir, moveYDir), 0.5f, snailLayerMask); //check to the front (relative) of snail
        if (forwardHit)
        {
            RaycastHit2D rightHit = Physics2D.Raycast(transform.position, new Vector2(moveYDir, -moveXDir), 0.7f, snailLayerMask); //check to the right (relative) of snail
            RaycastHit2D leftHit = Physics2D.Raycast(transform.position, new Vector2(-moveYDir, moveXDir), 0.7f, snailLayerMask); //check to the left (relative) of snail

            //Debug.Log("right " + rightHit + " left " + leftHit);

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
}
