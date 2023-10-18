using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController: MonoBehaviour {
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    public Animator enemyAnimator;

    private Rigidbody2D enemyBody;
    public AudioSource enemyAudio;

    // public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 startPosition;
    private bool dead = false;

    // public UnityEvent onKillPlayer;

    void Start() {
        this.startPosition = transform.position;
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }

    void Awake() {
        // other instructions
    }


    public void GameRestart()
    {
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<SpriteRenderer>().enabled = true;
        enemyAnimator.SetBool("die", false);

        transform.position = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
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

    void OnCollisionEnter2D(Collision2D col) {
        // Debug.Log("COLLIDE_TAG: " + col.gameObject.tag);
        var contact = col.contacts[0];
        // check that collision is from the bottom of the player
        bool fromTop = Vector2.Dot(contact.normal, Vector2.up) < -0.5;

        if (col.gameObject.CompareTag("Player")) {
            if (fromTop) {
                enemyAudio.PlayOneShot(enemyAudio.clip);
                GetComponent<Rigidbody2D>().simulated = false;
                enemyAnimator.SetBool("die", true);
                StartCoroutine(waitDeathAnimation());
            }
        }
    }

    IEnumerator waitDeathAnimation() {
        // gameOverUI.SetActive(true);
        // normalUI.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        GetComponent<SpriteRenderer>().enabled = false;
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