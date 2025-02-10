using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupSO pickup;
    private Player player;

    private void Start()
    {
        player = PlayerManager.Instance.GetPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        if (pickup.GetPickupType() == PickupType.KEY)
        {
            GameManager.Instance.CollectKey();
            gameObject.SetActive(false);
        }
        else if (pickup.GetPickupType() == PickupType.SHIELD)
        {
            player.ShieldHeal(pickup.GetValueToAdd());
            gameObject.SetActive(false);
        }
        else if (pickup.GetPickupType() == PickupType.HOSTAGE)
        {
            GameManager.Instance.IncreaseScore(pickup.GetValueToAdd());
            GameManager.Instance.IncreaseHostageCounter();
            gameObject.SetActive(false);
        }
        else if (pickup.GetPickupType() == PickupType.VULKAN_WEAPON) 
        {
            GameManager.Instance.GetVulkan(pickup);
            gameObject.SetActive(false);
        }
        else
            GiveAmmo();
    }

    private void GiveAmmo()
    {
        Debug.Log(pickup);

        GameManager.Instance.SupplyGunAmmunition(pickup);
        gameObject.SetActive(false);
    }
}
