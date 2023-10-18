using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform player; // Mario's Transform
    public Transform endLimit; // GameObject that indicates end of map
    public Transform startLimit;

    private float offset; // initial x-offset between camera and Mario
    private float startX; // smallest x-coordinate of the Camera
    private float endX; // largest x-coordinate of the camera
    private float viewportHalfWidth;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
            
        // other instructions
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // get coordinate of the bottomleft of the viewport
        // z doesn't matter since the camera is orthographic
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        );
        viewportHalfWidth = Mathf.Abs(
            bottomLeft.x - this.transform.position.x
        );

        offset = this.transform.position.x - player.position.x;
        // startX = this.transform.position.x;

        // bug: was -, now is +
        endX = endLimit.transform.position.x + viewportHalfWidth;
        startX = startLimit.transform.position.x - viewportHalfWidth;
    }
    
    public void GameRestart() {
        // reset camera position
        transform.position = startPosition;
    }

    void Update() {
        // Debug.Log("POS_X: " + player.position.x);
        float desiredX = player.position.x + offset;

        // check if desiredX is within startX and endX
        if (desiredX > startX && desiredX < endX) {
            // Debug.Log("IN_OF_BOUNDS");
            this.transform.position = new Vector3(
                desiredX, this.transform.position.y, 
                this.transform.position.z
            );
        }
    }
}