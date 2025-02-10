using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private BulletSO bullet;
    private void Awake()
    {
        Destroy(gameObject, bullet.GetBulletLifeTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Map"))
            Destroy(gameObject);

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(bullet.GetBulletDamage());
            Destroy(gameObject);
        }

        if (other.CompareTag("Door") || other.CompareTag("KeyDoor") || other.CompareTag("HostageDoor"))
        {
            
            Destroy(gameObject);
        }
    }

    public float GetBulletSpeed()
    {
        return bullet.GetBulletSpeed();
    }

    
}
