using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxPowerupController: MonoBehaviour, IPowerupController {
    public Animator powerupAnimator;
    public BasePowerup powerup; // reference to this question box's powerup
    public bool allowPowerupSpawn = true;
    public AudioSource spawnAudio;

    private bool prevAllowPowerupSpawn;

    void Start()
    {
        prevAllowPowerupSpawn = allowPowerupSpawn;
        // GameManager.instance.gameRestart.AddListener(restart);
    }

    // Update is called once per frame
    void Update() {

    }

    public void restart() {
        allowPowerupSpawn = prevAllowPowerupSpawn;

        if (powerupAnimator != null) {
            powerupAnimator.SetBool("spawn", false);
        }
        // powerupAnimator.enabled = true;
        // powerupAnimator.enabled = true;
        // powerupAnimator.SetBool("spawn", false);
        // powerupAnimator.enabled = false;
        // StartCoroutine(despawn());

        this.powerup.restart();
        Debug.Log("PRE_ALLOW_SPAWN " + prevAllowPowerupSpawn);
        Debug.Log("PRE_ALLOW_SPAWN2 " + allowPowerupSpawn);
    }

    private IEnumerator despawn() {
        yield return null;
        powerupAnimator.SetBool("spawn", false);
    }

    void PlayJumpSound() {
        // play jump sound
        spawnAudio.PlayOneShot(spawnAudio.clip);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        var contact = col.contacts[0];
        // check that collision is from the bottom of the object
        float alignment = Vector2.Dot(contact.normal, Vector2.up);
        bool fromBottom = alignment > 0.5;
        Debug.Log("POWERUP_SPAWN_PREENTER " + this.allowPowerupSpawn);

        if (
            (col.gameObject.tag == "Player") && 
            this.allowPowerupSpawn && fromBottom
        ) {
            this.allowPowerupSpawn = false;
            Debug.Log("POWERUP_SPAWN_ENTER");

            // show disabled sprite
            // this.GetComponent<Animator>().SetTrigger("spawned");
            // spawn the powerup
            // powerupAnimator.SetTrigger("spawned");
            powerupAnimator.SetBool("spawn", true);
            PlayJumpSound();
        }
    }

    // used by animator
    public void Disable() {
        powerupAnimator.enabled = false;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}

