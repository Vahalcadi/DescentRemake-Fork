using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.GetHasSelfDestructionStarted())
        {
            GameManager.Instance.SetHasExitedBuild(true);
        }
    }
}
