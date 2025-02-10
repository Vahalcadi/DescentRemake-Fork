using UnityEngine;

public enum EnemyType
{
    CLASS_ONE,
    CLASS_TWO,
    HULK,
    HULK_CORE,
    CORE
}

[CreateAssetMenu(fileName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public EnemyType enemyType;
    [SerializeField] private float maxHp;
    [SerializeField] private int points;
    [SerializeField] private float speed;
    [SerializeField] private float turningSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldownBetweenShots;
    public bool firesAlternatively;

    [HideInInspector] public float currentSpeed;

    public float GetCooldownBetweenShots()
    {
        return cooldownBetweenShots;
    }
    public GameObject GetBulletPrefab()
    {
        return bulletPrefab;
    }
    public float GetMaxHp()
    {
        return maxHp;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public float GetTurningSpeed()
    {
        return turningSpeed;
    }

    public int GetPoints()
    {
        return points;
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    public int PointsOnDestroy()
    {
        return points;
    }
}
