using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] int pointsForKill = 10;
    PlayerMovement player;
    CapsuleCollider2D myCapsuleCollider;
    float xSpeed;
    float bulletDirection;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        bulletDirection = -player.transform.localScale.x;
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        transform.localScale = new Vector3(bulletDirection, 1f, 1f); //= -player.transform.localScale;
        myRigidBody.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemies")
        {
            //FindObjectOfType<GameSession>().ScoreIncrement(pointsForKill);//for if it hits from the front
            Destroy(collision.gameObject);
            //Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            FindObjectOfType<GameSession>().ScoreIncrement(pointsForKill);//for if it hits from the back
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
