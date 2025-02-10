using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletType
{
    NORMAL,
    HOMING,
    CONCUSSION,
    FLARE
}

[CreateAssetMenu(fileName = "Bullet")]
public class BulletSO : ScriptableObject
{
    [SerializeField] private string bulletName;
    [SerializeField] private BulletType bulletType;
    [SerializeField] private float trackingAndExplosionRadius;
    [SerializeField] private float damagePerBullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;

    public float GetTrackingAndExplosionRadius()
    {
        return trackingAndExplosionRadius;
    }

    public BulletType GetBulletType()
    { 
        return bulletType; 
    }

    public float GetBulletDamage()
    {
        return damagePerBullet;
    }
    
    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    public float GetBulletLifeTime()
    {
        return bulletLifeTime;
    }
}
