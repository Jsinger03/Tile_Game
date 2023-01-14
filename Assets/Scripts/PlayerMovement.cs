using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 3.5f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    [Header("General variables")]
    Vector2 moveInput;
    //RigidBody is what lets us move around
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myBoxCollider;
    SpriteRenderer mySpriteRenderer;
    float gravityStart;
    bool isAlive = true;
    Vector2 yeeted = new Vector2(0f, 25f);


    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityStart = myRigidBody.gravityScale;
        myBoxCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = new Color(255f, 255f, 255f, 255f);
    }

    void Update()
    {
        if (!isAlive) { return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        else if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);//whatever current velocity on y is keep it the same. Only change x velocity
        myRigidBody.velocity = playerVelocity;


        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
        myAnimator.speed = 2f;
    }

    void ClimbLadder()
    {
        //to not climb if I get off the ladder
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("IsClimbing", false);
            myRigidBody.gravityScale = gravityStart;
            return;
        }

        //set climbing speed and animation
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);//whatever current velocity on y is keep it the same. Only change x velocity
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("IsClimbing", true);

        //to pause the animation of climbing if stationary
        if (!playerHasVerticalSpeed)
        {
            myAnimator.speed = 0;
        }
        else
        {
            myAnimator.speed = 2f;
        }


        //Debug.Log("Yeet Bucket");
        //if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        //{
        //    Debug.Log("climby");
        //}
    }
    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetBool("Dying", true);
            myAnimator.SetBool("IsRunning", false);
            mySpriteRenderer.color = new Color(255f, 255f, 255f, 1f);
            myRigidBody.velocity = yeeted;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }

    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }

}
