using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCenter : MonoBehaviour
{
    [SerializeField] private int maxEnergyToAdd;
    [SerializeField] private float energyToAddPerSecond;
    private float energyReplenished;
    

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (energyReplenished >= maxEnergyToAdd)
                return;

            GameManager.Instance.SupplyEnergy(energyToAddPerSecond * Time.deltaTime);           
        }
    }
}
