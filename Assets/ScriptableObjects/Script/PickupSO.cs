using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    ENERGY,
    VULKAN_AMMO,
    CONCUSSION_MISSILE,
    HOMING_MISSILE,
    SHIELD,
    KEY,
    HOSTAGE,
    VULKAN_WEAPON
}

[CreateAssetMenu(fileName = "PickupSO")]
public class PickupSO : ScriptableObject
{
    [Range(0, 100)]
    [SerializeField] private int dropChance;

    [SerializeField] private PickupType pickupType;

    [SerializeField] private int valueToAdd; // set if it adds values like energy, shield or score points

    [SerializeField] private bool canOpenRedDoors; //set to true only when creating key objects


    public int GetDropChance()
    {
        return dropChance;
    }

    public int GetValueToAdd()
    {
        return valueToAdd;
    }

    public bool CanOpenRedDoors()
    {
        return canOpenRedDoors;
    }

    public PickupType GetPickupType()
    {
        return pickupType;
    }
}
