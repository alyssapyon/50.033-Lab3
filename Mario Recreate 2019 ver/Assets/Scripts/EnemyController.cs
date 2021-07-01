using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 2.5f; // original was 5.0f but thats too big for mine
    private float enemyPatroltime = 1.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private bool withinLeftBoundary = true;
    private bool withinRightBoundary = true;

    private Rigidbody2D enemyBody;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void MoveGomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        withinLeftBoundary = -7.9 < enemyBody.position.x;
        withinRightBoundary = enemyBody.position.x < 7.9;
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset || (withinLeftBoundary && withinRightBoundary))
        {// move gomba
            MoveGomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            MoveGomba();
        }
    }
}
