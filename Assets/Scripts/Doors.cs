using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    //private Vector3 startingPosition;
    private bool isOpen;
    [SerializeField] private Animator anim;
    [SerializeField] private float durationDoorOpened;
    //private float currentTime;
    
    /*void Start()
    {
        startingPosition = transform.position;
    }*/

    /*void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.up * 3f, Time.deltaTime / 2);
        }
        else
        {
            Debug.Log("is Not OPEN");
            transform.position = Vector3.Lerp(transform.position, startingPosition, Time.deltaTime / 2);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("KeyDoor") && GameManager.Instance.GetCanOpenDoor() && (other.CompareTag("Bullet") || other.CompareTag("Player")))
        {
            StartCoroutine(OpenDoor());
        }
        else if (!gameObject.CompareTag("KeyDoor") && (other.CompareTag("Bullet") || other.CompareTag("Player")))
        {
            StartCoroutine(OpenDoor());
        }

        
    }

    public IEnumerator OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;

            Debug.Log("isOpen");
            anim.SetBool("isOpen",true);
            yield return new WaitForSeconds(durationDoorOpened);
            Debug.Log("isNotOpen");
            anim.SetBool("isOpen", false);
        }
    }

    public void CloseDoor()
    {
        isOpen = false;
    }
}
