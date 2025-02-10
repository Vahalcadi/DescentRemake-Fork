using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weapon;

    [Header("Primary Weapon Region")]
    [SerializeField] private Transform rightCannonBulletSpawner;
    [SerializeField] private Transform leftCannonBulletSpawner;

    [Header("Secondary Weapon Region")]
    [SerializeField] private Transform centralCannonBulletSpawner;


    private float cooldownBetweenShots;
    private float bulletSpeed;
    private int currentFiringCannon = 1; //managing cannons with an integer number. 1 is right cannon, -1 is left cannon

    private Player player;
    private InputManager inputManager;

    private void Start()
    {
        player = PlayerManager.Instance.GetPlayer();

        bulletSpeed = weapon.GetBulletPrefab().GetComponent<Bullet>().GetBulletSpeed();
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        cooldownBetweenShots -= Time.deltaTime;

        if (cooldownBetweenShots > 0)
            return;

        if(weapon.GetWeaponType() == WeaponType.PRIMARY)
        {
            if (inputManager.GetPrimaryFire())
            {
                DoubleBarrelFire();
            }
        }

        if (weapon.GetWeaponType() == WeaponType.SECONDARY)
        {
            if (inputManager.GetSecondaryFire())
            {
                DoubleBarrelFire();
            }
        }    
        
        if (weapon.GetWeaponType() == WeaponType.FLARE)
        {
            if (inputManager.GetFlare())
            {
                Fire(centralCannonBulletSpawner);
            }
        }

    }

    private void DoubleBarrelFire()
    {
        if (weapon.firesAlternatively)
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

    public bool HasBeenUnlocked()
    {
        return weapon.hasBeenUnlocked;
    }

    private void Fire(Transform currentCannonBulletSpawner)
    {

        if (weapon.GetAmmoType() == AmmoType.ENERGY && player.currentEnergy - weapon.GetAmmoCostOfSingleCannon() < 0)
            return;
        else if(weapon.currentAmmo - weapon.GetAmmoCostOfSingleCannon() < 0)
            return;

        var bullet = Instantiate(weapon.GetBulletPrefab(), currentCannonBulletSpawner.position, currentCannonBulletSpawner.rotation);
        bullet.GetComponent<Rigidbody>().velocity = currentCannonBulletSpawner.forward * bulletSpeed;


        if (weapon.GetAmmoType() == AmmoType.ENERGY)
        {
            player.currentEnergy -= weapon.GetAmmoCostOfSingleCannon();
            GameMenu.Instance.SetEnergyUI(player.currentEnergy);
        }
        else
        {
            weapon.currentAmmo -= weapon.GetAmmoCostOfSingleCannon();

            if (weapon.GetBulletType() == BulletType.NORMAL)
                GameMenu.Instance.SetVulkanAmmoUI(weapon.currentAmmo);
            else if(weapon.GetBulletType() == BulletType.CONCUSSION)
                GameMenu.Instance.SetConcussionAmmoUI(weapon.currentAmmo);
            else if (weapon.GetBulletType() == BulletType.HOMING)
                GameMenu.Instance.SetHomingAmmoUI(weapon.currentAmmo);
        }



        Debug.Log(player.currentEnergy);
        Debug.Log(weapon.currentAmmo);

        cooldownBetweenShots = weapon.GetCooldownBetweenShots();

        currentFiringCannon = -currentFiringCannon; 
    }
}
