using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageDoor : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private ParticleSystem HostageDoorParticles;

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            Instantiate(HostageDoorParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
