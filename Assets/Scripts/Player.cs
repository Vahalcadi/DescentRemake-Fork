using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement region")]
    [SerializeField] Rigidbody rb;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] float speed;
    [SerializeField] private float ascendDescentSpeed;
    public float maxVelocity;
    public Vector2 accelDecel;
    public Vector2 ascendDescent;
    public Vector2 rotation;

    [Header("Crossair movement")]
    [SerializeField] private float aimSensitivity = 500f;

    [Header("Weapons")]
    [SerializeField] private GameObject laserCannon;
    [SerializeField] private GameObject vulkanCannon;
    [SerializeField] private GameObject homingMissile;
    [SerializeField] private GameObject concussionMissile;

    [Header("Hp Region")]
    [SerializeField] private float maxHp = 200;
    private bool isShieldBroken;
    private float currentHp;

    [Header("Energy")]
    [SerializeField] private float maxEnergy;
    [HideInInspector] public float currentEnergy;

    private InputManager inputManager;
    void Start()
    {
        inputManager = InputManager.Instance;
        currentHp = 100;
        currentEnergy = 100;

        GameMenu.Instance.SetMaxHpUI(maxHp);
        GameMenu.Instance.SetMaxEnergyUI(maxEnergy);
        GameMenu.Instance.SetHpUI(currentHp);
        GameMenu.Instance.SetEnergyUI(currentEnergy);
    }

    void Update()
    {
        MoveFwdBwd();
        MoveLeftRight();
        CrossairMovement();
        SwapWeapon();

        if (!inputManager.isLegacyControlsOn)
            AscendDescent();
    }

    void MoveFwdBwd()
    {
        //Get value of x and y from input using Input Action component
        accelDecel = inputManager.GetAccelDecel();


        if (accelDecel.y != 0)
        {
            float finalSpeed = accelDecel.y * speed;

            //Move player
            rb.AddForce(transform.forward * finalSpeed * Time.deltaTime);
        }

        rb.velocity = new Vector3(ClampVelocityAxis(true), rb.velocity.y, ClampVelocityAxis(false));
    }

    void MoveLeftRight()
    {
        accelDecel = inputManager.GetAccelDecel();


        if (accelDecel.x != 0)
        {
            float finalSpeed = accelDecel.x * speed;

            //Move player
            rb.AddForce(transform.right * finalSpeed * Time.deltaTime);
        }

        rb.velocity = new Vector3(ClampVelocityAxis(true), rb.velocity.y, ClampVelocityAxis(false));
    }

    private float ClampVelocityAxis(bool isX)
    {
        float currentMaxVelocity = maxVelocity;
        float speed;

        if (isX)
            speed = Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity);
        else
            speed = Mathf.Clamp(rb.velocity.z, -maxVelocity, maxVelocity);

        maxVelocity = currentMaxVelocity;
        return speed;
    }

    public void AscendDescent()
    {
        //Get value of x and y from input using Input Action component
         ascendDescent = inputManager.GetAscendDescent();


        if (ascendDescent.y != 0)
        {
            float finalSpeed = ascendDescent.y * ascendDescentSpeed;

            //Move player
            rb.AddForce(transform.up * finalSpeed * Time.deltaTime);
        }
        
    }

    public void CrossairMovement()
    {
        rotation = inputManager.GetRotation();
        float mouseX = inputManager.GetLookPosition().normalized.x * aimSensitivity * Time.deltaTime;
        float mouseY = -inputManager.GetLookPosition().normalized.y * aimSensitivity * Time.deltaTime;

        //-------------------- Old Script -------------------------//
        //Remember to set sensitivity to 500 if you decide to rollback this change

        /*float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;*/

        //--------------------------------------------------------//

        transform.Rotate(new Vector3(mouseY, mouseX, -rotation.x * rotationSpeed * Time.deltaTime));
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        Debug.Log(currentHp);

        if (currentHp < 0)
        {

            if (!isShieldBroken)
            {
                currentHp = 0;
                isShieldBroken = true;
            }
            else
            {
                currentHp = 0;
                GameManager.Instance.Death();
            }
        }

        GameMenu.Instance.SetHpUI(currentHp);
    }

    public void SwapWeapon()
    {
        if (inputManager.GetLaserGun())
        {
            vulkanCannon.SetActive(false);
            laserCannon.SetActive(true);
        }

        if (inputManager.GetVulkanGun() && vulkanCannon.GetComponent<Weapon>().HasBeenUnlocked())
        {
            laserCannon.SetActive(false);
            vulkanCannon.SetActive(true);
        }

        if (inputManager.GetHomingMissiles() && homingMissile.GetComponent<Weapon>().HasBeenUnlocked())
        {
            concussionMissile.SetActive(false);
            homingMissile.SetActive(true);
        }

        if (inputManager.GetConcussionMissiles())
        {
            homingMissile.SetActive(false);
            concussionMissile.SetActive(true);
        }
    }

    public void ResetHpEnergyAndWeapon()
    {
        isShieldBroken = false;
        currentHp = 100;
        currentEnergy = 100;

        vulkanCannon.SetActive(false);
        homingMissile.SetActive(false);

        laserCannon.SetActive(true);
        concussionMissile.SetActive(true);
    }

    public void ShieldHeal(float healAmount)
    {
        currentHp += healAmount;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        GameMenu.Instance.SetHpUI(currentHp);
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }
}
