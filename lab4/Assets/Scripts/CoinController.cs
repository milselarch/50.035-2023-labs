using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinController : MonoBehaviour {
    // Start is called before the first frame update
    public AudioSource coinAudio;
    public Rigidbody2D rigidBody;
    public Animator questionBoxAnimator;

    public bool disableOnReturn = true;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void onCoinReturn() {
        // Debug.Log("COIN_RETURN");
        this.playCoinReturnSound();

        if (disableOnReturn) {
            // make the box unaffected by Physics
            rigidBody.bodyType = RigidbodyType2D.Static;
            questionBoxAnimator.enabled = false;
        }
    }

    void playCoinReturnSound() {
        // play jump sound
        coinAudio.PlayOneShot(coinAudio.clip);
    }
}
