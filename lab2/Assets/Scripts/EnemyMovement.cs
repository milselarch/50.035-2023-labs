using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;

    // public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 startPosition;

    void Start() {
        this.startPosition = transform.position;
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }

    void ComputeVelocity() {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void Movegoomba() {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name);
    }

    public void reset() {
        transform.position = this.startPosition;
    }

    void Update() {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset) {
            // move goomba
            Movegoomba();
        } else {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }
}