using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroTrigger : MonoBehaviour
{
    private Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            enemy.SetEngagedInCombat(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enemy.SetEngagedInCombat(false);
    }
}
