using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    int direction = 1;
    public float speed = 4f;
    bool shouldMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (shouldMove)
        {
            rigidBody.velocity = new Vector2(speed * direction, rigidBody.velocity.y);
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Ground") || !col.gameObject.CompareTag("Obstacles"))
        {
            direction = direction * (-1);
        }

        if (col.gameObject.CompareTag("Player"))
        {
            shouldMove = false;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
