using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCoinPowerup : BasePowerup
{
    // setup this object's type
    // instantiate variables
    public GameObject childObject;
    public float targetSpeed = 5.0f;
    public float catchUpRate = 2.0f;

    public int gravityFrames = 10;
    public float maxGravity = 2.0f;

    private Vector3 startPosition;

    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.Coin;
        GameManager.instance.gameRestart.AddListener(restart);
        startPosition = transform.position;
    }

    void onAnimated() {
        Debug.Log("ANIMATE_END");
    }

    void restart() {
        this.spawned = true;
        this._restart();

        childObject.GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<Animator>().enabled = true; 
        rigidBody.bodyType = RigidbodyType2D.Static;
        transform.position = startPosition;
        rigidBody.gravityScale = 0.0f;
    }

    void OnCollisionEnter2D(Collision2D col) {
        var contact = col.contacts[0];
        float alignment = Vector2.Dot(contact.normal, Vector2.right);
        bool fromLeft = alignment > 0.5;
        bool fromRight = alignment < -0.5;

        if (col.gameObject.CompareTag("Player") && this.spawned) {
            // TODO: do something when colliding with Player
            // then destroy powerup (optional)
            Debug.Log("COIN_POWERUP_DESTROY");
            DestroyPowerup();
        } else {
            if (fromLeft && !this.goRight) {
                this.goRight = !this.goRight;
            } else if (fromRight && this.goRight) {
                this.goRight = !this.goRight;
            }
        }
        
        /*
        else if (col.gameObject.layer == 10) {
            // else if hitting Pipe, flip travel direction
            if (this.spawned) {
                goRight = !goRight;
                rigidBody.AddForce(
                    Vector2.right * 3 * (goRight ? 1 : -1),
                    ForceMode2D.Impulse
                );
            }
        }
        */
    }

    private void FixedUpdate() {
        float xVelocity = rigidBody.velocity.x;
        int direction = this.goRight ? 1 : -1;
        float velocityDiff = (targetSpeed * direction) - xVelocity;
        float force = Time.deltaTime * velocityDiff * this.catchUpRate;
        rigidBody.AddForce(
            Vector2.right * force, ForceMode2D.Impulse
        );
    }

    // interface implementation
    public override void SpawnPowerup() {
        Debug.Log("POWERUP_SPAWN");
        this.spawned = true;

        childObject.GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<Animator>().enabled = false; 
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.isKinematic = false;
    
        // move to the right
        StartCoroutine(waitForDynamicBody());
    }

    private IEnumerator waitForDynamicBody() {
        yield return null;
        rigidBody.AddForce(
            Vector2.up * 0 + Vector2.right * 2, 
            ForceMode2D.Impulse
        );

        for (int k=0; k<=this.gravityFrames; k++) {
            float gravity = ((float) k / gravityFrames) * this.maxGravity;
            rigidBody.gravityScale = gravity;
            Debug.Log("SET GRAVITY: " + gravity);
            yield return null;
        }
    }

    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object

    }
}