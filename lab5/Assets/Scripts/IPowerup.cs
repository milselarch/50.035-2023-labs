using UnityEngine;

public enum IPowerupType {
    Coin = 0,
    MagicMushroom = 1,
    OneUpMushroom = 2,
    StarMan = 3
}

public interface IPowerup
{
    void DestroyPowerup();
    void SpawnPowerup();
    void ApplyPowerup(MonoBehaviour i);

    IPowerupType powerupType
    {
        get;
    }

    bool hasSpawned
    {
        get;
    }
}

public interface IPowerupApplicable
{
    public void RequestPowerupEffect(IPowerup i);
}