using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private int damagePerSecond;
    private Player player;

    private void Start()
    {
        player = PlayerManager.Instance.GetPlayer();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
