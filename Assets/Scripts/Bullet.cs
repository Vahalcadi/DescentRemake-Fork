using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletSO bullet;
    [SerializeField] private LayerMask mask;
    [SerializeField] private ParticleSystem DeathParticles;
    private Rigidbody rb;

    Transform target;
    Vector3 direction;
    Vector3 rotateAmount;

    private List<Collider> colliders = new List<Collider>();

    private void Awake()
    {
        Destroy(gameObject, bullet.GetBulletLifeTime());

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (bullet.GetBulletType() == BulletType.HOMING)
        {
            Detection();
        }
    }

    public BulletSO GetBulletSO()
    {
        return bullet;
    }

    public float GetBulletSpeed()
    {
        return bullet.GetBulletSpeed();
    }


    private void Detection()
    {
        colliders = Physics.OverlapSphere(transform.position, bullet.GetTrackingAndExplosionRadius(), mask).ToList();

        if (colliders.Count > 0)
        {
            target = colliders[0].GetComponent<Transform>();

            direction = target.position - transform.position;

            rotateAmount = Vector3.Cross(direction.normalized, transform.forward);

            rb.angularVelocity = -rotateAmount * 10;

            rb.velocity = transform.forward * bullet.GetBulletSpeed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Map"))
            DestroyThisObject(other);       


        if (other.CompareTag("Enemy"))
        {

            if (bullet.GetBulletType() == BulletType.HOMING || bullet.GetBulletType() == BulletType.CONCUSSION)
            {
                colliders = Physics.OverlapSphere(transform.position, bullet.GetTrackingAndExplosionRadius(), mask).ToList();

                foreach (Collider t in colliders)
                {
                    t.GetComponent<Enemy>().TakeDamage(bullet.GetBulletDamage());
                }
                DestroyThisObject(other);
            }

            if (bullet.GetBulletType() == BulletType.NORMAL || bullet.GetBulletType() == BulletType.FLARE)
            {
                other.GetComponent<Enemy>().TakeDamage(bullet.GetBulletDamage());
                DestroyThisObject(other);
            }
        }


        if (other.CompareTag("Door"))
        {
            //StartCoroutine(other.GetComponent<Doors>().OpenDoor());
            DestroyThisObject(other);
        }

        if (other.CompareTag("KeyDoor"))
        {
            DestroyThisObject(other);
        }

        if (other.CompareTag("HostageDoor"))
        {
            other.GetComponent<HostageDoor>().TakeDamage(bullet.GetBulletDamage());
            DestroyThisObject(other);
        }
    }

    public void DestroyThisObject(Collider other)
    {
        if (bullet.GetBulletType() == BulletType.FLARE)
        {
            if (other.CompareTag("Enemy"))
            {
                Instantiate(DeathParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }   
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.parent = other.transform;
            }
        }
        else
        {
            Instantiate(DeathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
