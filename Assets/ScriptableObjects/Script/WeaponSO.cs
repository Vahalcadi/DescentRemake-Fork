using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    PRIMARY,
    SECONDARY,
    FLARE
}

public enum AmmoType
{
    ENERGY,
    AMMUNITION
}

[CreateAssetMenu(fileName = "Weapon")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int level;
    [SerializeField] private float startingAmmo;
    [SerializeField] private float ammoCapacity;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldownBetweenShots;
    [SerializeField] private float ammoCostOfSingleCannon;

    public bool firesAlternatively;

    [HideInInspector] public bool hasBeenUnlocked;

    [HideInInspector] public float currentAmmo;

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public float GetAmmoCostOfSingleCannon()
    {
        return ammoCostOfSingleCannon;
    }

    public AmmoType GetAmmoType()
    {
        return ammoType;
    }

    public float GetCooldownBetweenShots()
    {
        return cooldownBetweenShots;
    }
    public GameObject GetBulletPrefab()
    {
        return bulletPrefab;
    }

    public BulletType GetBulletType()
    {
        return bulletPrefab.GetComponent<Bullet>().GetBulletSO().GetBulletType();
    }

    public float GetMaxAmmo()
    {
        return ammoCapacity;
    }

    public void InitiateWeapon()
    {
        currentAmmo = startingAmmo;
    }
}
