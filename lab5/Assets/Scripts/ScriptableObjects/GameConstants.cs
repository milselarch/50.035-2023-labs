using UnityEngine;

[CreateAssetMenu(
    fileName = "GameConstants",
    menuName = "ScriptableObjects/GameConstants", 
    order = 1
)]
public class GameConstants: ScriptableObject {
 // lives
    public int maxLives = 10;

    // Mario's movement
    public int speed = 150;
    public int maxSpeed = 5;
    public int upSpeed = 30;
    public int deathImpulse = 45;
    public Vector3 marioStartingPosition = new Vector3(-5.33f, -4.69f, 0.0f);

    // Goomba's movement
    public float goombaPatrolTime = 2.0f;
    public float goombaMaxOffset = 5.0f;

    public float flickerInterval = 0.2f;
}