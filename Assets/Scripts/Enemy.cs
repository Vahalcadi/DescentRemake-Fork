using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO enemy;
    [SerializeField] private GameObject[] dropItemsPrefabs;
    [SerializeField] private ParticleSystem EnemyDeathParticles;

    private Transform playerTransform;
    private bool isEngagedInCombat;
    private Vector3 positionBetweenThisAndPlayer;

    [Header("Enemy weapons")]
    [SerializeField] private Transform rightCannonBulletSpawner;
    [SerializeField] private Transform leftCannonBulletSpawner;
    [Header("Core additional weapons")]
    [SerializeField] private Transform frontCannonBulletSpawner;
    [SerializeField] private Transform backCannonBulletSpawner;

    private float cooldownBetweenShots;
    private float bulletSpeed;
    private int currentFiringCannon = 1; //managing cannons with an integer number. 1 is right cannon, -1 is left cannon
    private float currentHp;


    [Range(0, 1)]
    [SerializeField] private float coneVision; // 0 = 180� angle, 0.5 = 90� ,1 = 0� angle


    [Header("Collisions")]
    [SerializeField] private LayerMask mask;
    [SerializeField] private float proximityDistance;
    [SerializeField] private float fireRaycastLenght;
    RaycastHit hitFront;
    RaycastHit hitBack;
    RaycastHit hitRight;
    RaycastHit hitLeft;
    RaycastHit hitUp;
    RaycastHit hitDown;
    RaycastHit fireRaycast;
    float dotProduct;

    private void Start()
    {
        playerTransform = PlayerManager.Instance.GetPlayerTransform();
        currentHp = enemy.GetMaxHp();
        enemy.currentSpeed = enemy.GetSpeed();
        enemy.enemyType = enemy.GetEnemyType();
        bulletSpeed = enemy.GetBulletPrefab().GetComponent<EnemyBullet>().GetBulletSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        positionBetweenThisAndPlayer = playerTransform.position - transform.position;
        //positionBetweenThisAndPlayer.Normalize();

        if (isEngagedInCombat)
        {
            RotateTowardsPlayer();
            MoveTowardsPlayer();
            AttackEngagedPlayer();
        }

        CanMoveCheck();
    }

    private void AttackEngagedPlayer()
    {
        cooldownBetweenShots -= Time.deltaTime;
        if (cooldownBetweenShots < 0)
        {
            if (enemy.enemyType == EnemyType.CORE)
                CoreFire();
            else
                NormalFire();

            cooldownBetweenShots = enemy.GetCooldownBetweenShots();
        }
    }

    private void CoreFire()
    {
        dotProduct = Vector3.Dot((playerTransform.position - transform.position).normalized, transform.forward);

        Debug.Log("core fire");

        if (dotProduct > coneVision)
            Fire(frontCannonBulletSpawner);
        else if (dotProduct < -coneVision)
            Fire(backCannonBulletSpawner);
        else
        {
            dotProduct = Vector3.Dot((playerTransform.position - transform.position).normalized, transform.right);

            if (dotProduct > 0)
                Fire(rightCannonBulletSpawner);
            else
                Fire(leftCannonBulletSpawner);
        }

    }

    private void NormalFire()
    {
        dotProduct = Vector3.Dot((playerTransform.position - transform.position).normalized, transform.forward);

        if (dotProduct > coneVision)
        {
            if (enemy.firesAlternatively)
            {
                if (currentFiringCannon == 1)
                    Fire(rightCannonBulletSpawner);
                else
                    Fire(leftCannonBulletSpawner);
            }
            else
            {
                Fire(rightCannonBulletSpawner);
                Fire(leftCannonBulletSpawner);
            }
        }
    }

    private void Fire(Transform currentCannonBulletSpawner)
    {
        if (fireRaycast.collider != null && !fireRaycast.collider.CompareTag("Player"))
        {
            return;
        }

        var bullet = Instantiate(enemy.GetBulletPrefab(), currentCannonBulletSpawner.position, currentCannonBulletSpawner.rotation);

        if (enemy.enemyType == EnemyType.CORE)
            bullet.transform.LookAt(playerTransform);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

        currentFiringCannon = -currentFiringCannon;


    }

    public void RotateTowardsPlayer()
    {
        //Debug.DrawLine(transform.position,transform.position + positionBetweenThisAndPlayer * 5f, Color.red);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(positionBetweenThisAndPlayer), enemy.GetTurningSpeed() * Time.deltaTime);
    }

    public void MoveTowardsPlayer()
    {
        if ((fireRaycast.collider != null && !fireRaycast.collider.CompareTag("Player") && !fireRaycast.collider.CompareTag("Enemy")) || enemy.enemyType == EnemyType.HULK || enemy.enemyType == EnemyType.CORE)
            return;

        //if (transform.position.z - playerTransform.position.z > proximityDistance)
        if (hitFront.collider == null)
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, enemy.currentSpeed * Time.deltaTime);
    }

    public void CanMoveCheck()
    {
        Physics.Raycast(transform.position, transform.forward, out fireRaycast, fireRaycastLenght, mask);
        Debug.DrawLine(transform.position, transform.position + transform.forward * fireRaycastLenght, Color.yellow);

        if (enemy.enemyType == EnemyType.HULK || enemy.enemyType == EnemyType.CORE)
            return;

        Physics.Raycast(transform.position, transform.forward, out hitFront, proximityDistance, mask);
        Physics.Raycast(transform.position, -transform.forward, out hitBack, proximityDistance, mask);
        Physics.Raycast(transform.position, transform.right, out hitRight, proximityDistance, mask);
        Physics.Raycast(transform.position, -transform.right, out hitLeft, proximityDistance, mask);
        Physics.Raycast(transform.position, transform.up, out hitUp, proximityDistance, mask);
        Physics.Raycast(transform.position, -transform.up, out hitDown, proximityDistance, mask);

        Debug.DrawLine(transform.position, transform.position + transform.forward * proximityDistance, Color.black);
        Debug.DrawLine(transform.position, transform.position - transform.forward * proximityDistance, Color.black);
        Debug.DrawLine(transform.position, transform.position + transform.right * proximityDistance, Color.black);
        Debug.DrawLine(transform.position, transform.position - transform.right * proximityDistance, Color.black);
        Debug.DrawLine(transform.position, transform.position + transform.up * proximityDistance, Color.black);
        Debug.DrawLine(transform.position, transform.position - transform.up * proximityDistance, Color.black);


        if (hitFront.collider != null)
        {
            if (hitFront.transform.position.magnitude > proximityDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.forward, enemy.currentSpeed * Time.deltaTime);

                if (hitUp.collider == null)
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, enemy.currentSpeed * Time.deltaTime);
            }
        }

        if (hitBack.collider != null)
        {
            if (isEngagedInCombat && hitFront.collider == null)
                MoveTowardsPlayer();
            else if (hitFront.collider != null)
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, enemy.currentSpeed * Time.deltaTime);
            else
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, enemy.currentSpeed * Time.deltaTime);
        }

        if (hitRight.collider != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.right, enemy.currentSpeed * Time.deltaTime);
        }

        if (hitLeft.collider != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right, enemy.currentSpeed * Time.deltaTime);
        }

        if (hitUp.collider != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.up, enemy.currentSpeed * Time.deltaTime);
        }

        if (hitDown.collider != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, enemy.currentSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp < 0)
        {
            int dropChance = Random.Range(1, 101);

            if (dropChance < 10 && enemy.enemyType != EnemyType.CORE)
            {
                Instantiate(dropItemsPrefabs[Random.Range(0, dropItemsPrefabs.Length)], transform.position, Quaternion.identity);
            }

            Instantiate(EnemyDeathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetEngagedInCombat(bool state)
    {
        isEngagedInCombat = state;
    }

    private void OnDestroy()
    {       
        GameManager.Instance.IncreaseScore(enemy.PointsOnDestroy());     

        if (enemy.enemyType == EnemyType.CORE)
        {
            GameManager.Instance.SetHasSelfDestructionStarted(true);
            GameManager.Instance.FinishGame();
        }
    }
}
