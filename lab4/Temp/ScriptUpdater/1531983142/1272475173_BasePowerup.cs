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

    // base methods
    protected virtual void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
        startSpawned = spawned;
        startConsumed = consumed;
        startGoRight = goRight;
    }

    protected void _restart() {
        this.goRight = startGoRight;
        this.consumed = startConsumed;
        this.spawned = startSpawned;

        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        GetComponent<Renderer>().enabled = true;
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
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        GetComponent<Renderer>().enabled = false;
    }

    // 2. abstract methods, must be implemented by derived classes
    public abstract void SpawnPowerup();
    public abstract void ApplyPowerup(MonoBehaviour i);
}