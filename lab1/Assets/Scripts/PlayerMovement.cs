using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement: MonoBehaviour {
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;

    // keep track of which side the mario sprite is facing
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private Rigidbody2D marioBody;

    // other variables
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public GameObject enemies;
    public GameObject gameOverUI;
    public GameObject normalUI;

    public JumpOverGoomba jumpOverGoomba;
    private Vector3 startPosition;


    // Start is called before the first frame update
    void Start() {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();

        // assign mario sprite object
        marioSprite = GetComponent<SpriteRenderer>();

        startPosition = marioBody.transform.position;
        gameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState) {
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState) {
            faceRightState = true;
            marioSprite.flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Collided with goomba!");
            Time.timeScale = 0.0f;
            gameOverUI.SetActive(true);
            normalUI.SetActive(false);
        }
    }

    // FixedUpdate is called 50 times a second
    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate() {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0){
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

        if (Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground")) {
            onGroundState = true;
        }
    }

    public void RestartButtonCallback(int input) {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame() {
        // reset position
        // marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        marioBody.transform.position = startPosition;
        marioBody.velocity = Vector2.zero;
        gameOverUI.SetActive(false);
        normalUI.SetActive(true);

        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform) {
            // eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
            eachChild.GetComponent<EnemyMovement>().reset();
        }

        // reset score
        jumpOverGoomba.score = 0;
    }
} 