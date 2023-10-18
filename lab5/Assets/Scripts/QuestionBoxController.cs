using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxController: MonoBehaviour {
    public Sprite usedQuestionBox;
    public Animator slideAnimator;
    public Animator flashingAnimator; // question box animator
    public SpriteRenderer spriteRenderer;
    public bool allowCoinSpawn = true;

    private Sprite startSprite;
    private bool startFlashing;
    private bool startAllowCoinSpawn; 

    // Start is called before the first frame update
    void Start() {
        startSprite = spriteRenderer.sprite;
        startFlashing = flashingAnimator.enabled;
        startAllowCoinSpawn = allowCoinSpawn;
    }

    public void restart() {
        spriteRenderer.sprite = startSprite;
        flashingAnimator.enabled = startFlashing;
        allowCoinSpawn = startAllowCoinSpawn;
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        var contact = col.contacts[0];
        // check that collision is from the bottom of the object
        float alignment = Vector2.Dot(contact.normal, Vector2.up);
        bool fromBottom = alignment > 0.5;
        // Debug.Log("HIT_BOX_ALIGN " + alignment);

        if (fromBottom && this.allowCoinSpawn) {
            Debug.Log("HIT_BOX");
            slideAnimator.SetTrigger("start");
            this.allowCoinSpawn = false;

            // change sprite to be "used-box" sprite
            spriteRenderer.sprite = usedQuestionBox;
            // disable animation of question box (flashing effect)
            flashingAnimator.enabled = false;
        }
    }
}
