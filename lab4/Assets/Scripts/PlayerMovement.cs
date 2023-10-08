using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerMovement: MonoBehaviour {
    public GameConstants gameConstants;
    float deathImpulse = 20.0f;
    float upSpeed = 10;
    float maxSpeed = 20;
    float speed = 10;

    private bool onGroundState = true;
    private GameManager gameManager;

    // keep track of which side the mario sprite is facing
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private Rigidbody2D marioBody;

    /*
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public GameObject gameOverUI;
    public GameObject normalUI;
    */
    public GameObject enemies;

    public JumpOverGoomba jumpOverGoomba;
    private Vector3 startPosition;
    
    // for animation
    public Animator marioAnimator;
    private float skidSpeed = 0.05f;

    // for audio
    public AudioSource marioAudio;
    public AudioClip marioDeath;
    public Transform gameCamera;

    private bool jumpedState = false;
    private bool moving = false;
    private bool wasIdle = true;

    // state
    [System.NonSerialized]
    public bool alive = true;

    // Start is called before the first frame update
    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        // Set constants
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;

        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();

        // assign mario sprite object
        marioSprite = GetComponent<SpriteRenderer>();

        startPosition = marioBody.transform.position;
        // gameOverUI.SetActive(false);

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);

        // subscribe to scene manager scene change
        // SceneManager.activeSceneChanged += SetStartingPosition;
    }

    void Awake() {
        // other instructions
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
        marioBody = GetComponent<Rigidbody2D>();

        // assign mario sprite object
        marioSprite = GetComponent<SpriteRenderer>();
    }

    public void SetStartingPosition(Scene current, Scene next) {
        if (next.name == "World-1-2") {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(-10.2399998f, -4.3499999f, 0.0f);
        }
    }

    public void Jump() {
        if (alive && onGroundState) {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    public void JumpHold() {
        if (alive && jumpedState) {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    void PlayDeathImpulse() {
        Debug.Log("DEATH_IMPULSE");
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene() {
        // stop time
        // set game over scene
        StartCoroutine(this.activateGameOver());
    }

    void PlayJumpSound() {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    // Update is called once per frame
    void Update() {
        // toggle state
        float velocity = Mathf.Abs(marioBody.velocity.x);

        /*
        if (Input.GetKeyDown("a") && faceRightState) {
            faceRightState = false;
            marioSprite.flipX = true;

            if ((velocity > this.skidSpeed) && !wasIdle) {
                marioAnimator.SetTrigger("onSkid");
            }

            wasIdle = false;
        }

        if (Input.GetKeyDown("d") && !faceRightState) {
            faceRightState = true;
            marioSprite.flipX = false;

            if ((velocity > this.skidSpeed) && !wasIdle) {
                marioAnimator.SetTrigger("onSkid");
            }

            wasIdle = false;
        }
        */

        // Debug.Log("X-SPEED " + Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetFloat("xSpeed", velocity);
        if (velocity < 0.01f) { wasIdle = true; }
    }

    void FlipMarioSprite(int value) {
        if (value == -1 && faceRightState) {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f) {
                marioAnimator.SetTrigger("onSkid");
            }
        } else if (value == 1 && !faceRightState) {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f) {
                marioAnimator.SetTrigger("onSkid");
            }
        }
    }

    IEnumerator activateGameOver() {
        // gameOverUI.SetActive(true);
        // normalUI.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0.0f;
    }

    // FixedUpdate is called 50 times a second
    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate() {
        if (alive && moving) {
            Move(faceRightState == true ? 1 : -1);
        }
        /*
        if (!alive) { return; }
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0) {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.velocity.magnitude < maxSpeed) {
                marioBody.AddForce(movement * speed);
            }
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")) {
            // stop
            marioBody.velocity = Vector2.zero;
        }

        if (
            Input.GetKeyDown("space") && onGroundState &&
            (marioBody.velocity.y < 0.01f)
        ) {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            this.onGroundState = false;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
        */
    }

    void Move(int value) {
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < maxSpeed) {
            marioBody.AddForce(movement * speed);
        }
    }

    public void MoveCheck(int value) {
        if (value == 0) {
            moving = false;
        } else {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        // Debug.Log("COLLIDE_TAG: " + col.gameObject.tag);
        var contact = col.contacts[0];
        // check that collision is from the bottom of the player
        bool fromBottom = Vector2.Dot(contact.normal, Vector2.up) > 0.5;

        if (col.gameObject.CompareTag("Enemy")) {
            Debug.Log("Collided with goomba!");
            // StartCoroutine(this.activateGameOver());

            if (fromBottom) {
                gameManager.IncreaseScore(1);
            } else if (alive) {
                gameManager.GameOver();
                // play death animation
                marioAnimator.Play("mario-die");
                marioAudio.PlayOneShot(marioDeath);
                alive = false;
            }
        }

        if (!onGroundState && fromBottom && (
            col.gameObject.CompareTag("Ground") ||
            col.gameObject.CompareTag("Enemy") || 
            col.gameObject.CompareTag("Obstacles") 
        )) {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    public void RestartButtonCallback(int input) {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = startPosition;

        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);
    }

    private void ResetGame() {
        // reset position
        // marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        marioBody.transform.position = startPosition;
        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);

            // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        marioBody.velocity = Vector2.zero;
        // gameOverUI.SetActive(false);
        // normalUI.SetActive(true);

        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        // scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform) {
            // eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
            eachChild.GetComponent<EnemyMovement>().reset();
        }

        // reset score
        jumpOverGoomba.score = 0;
    }
} 