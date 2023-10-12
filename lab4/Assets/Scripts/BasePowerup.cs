using UnityEngine;


public abstract class BasePowerup : MonoBehaviour, IPowerup
{
    public PowerupType type;
    public bool spawned = false;
    protected bool consumed = false;
    protected bool goRight = true;
    protected Rigidbody2D rigidBody;
    
    private bool startSpawned;
    private bool startConsumed;
    private bool startGoRight; 

    private Vector3 sPos;

    // base methods
    protected virtual void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
        startSpawned = spawned;
        startConsumed = consumed;
        startGoRight = goRight;

        sPos = this.gameObject.transform.position;
    }

    protected void _restart() {
        this.goRight = startGoRight;
        this.consumed = startConsumed;
        this.spawned = startSpawned;

        this.gameObject.transform.position = sPos;
        this.gameObject.SetActive(true);
    }

    // interface methods
    // 1. concrete methods
    public PowerupType powerupType
    {
        get // getter
        {
            return type;
        }
    }

    public bool hasSpawned
    {
        get // getter
        {
            return spawned;
        }
    }

    public void DestroyPowerup()
    {
        // Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    // 2. abstract methods, must be implemented by derived classes
    public abstract void SpawnPowerup();
    public abstract void ApplyPowerup(MonoBehaviour i);
}