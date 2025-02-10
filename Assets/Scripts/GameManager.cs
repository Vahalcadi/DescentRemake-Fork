using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool hasExitedBuilding;
    private bool hasSelfDestructionStarted;
    private bool canOpenDoor;
    public static GameManager Instance;

    [SerializeField] private WeaponSO vulkan;
    [SerializeField] private WeaponSO homingMissile;
    [SerializeField] private WeaponSO concussionMissile;
    [SerializeField] private WeaponSO laser;
    [SerializeField] private WeaponSO flare;
    [SerializeField] private PickupSO hostage;

    [SerializeField] private GameObject vulkanPrefab;
    [SerializeField] private GameObject homingMissileX4Prefab;
    [SerializeField] private GameObject homingMissilePrefab;
    [SerializeField] private GameObject concussionMissileX4Prefab;
    [SerializeField] private GameObject concussionMissilePrefab;

    [Header("Respawn point")]
    [SerializeField] private Transform respawnPoint;

    [Header("Points")]
    private int totalPoints = 0;
    private int hostageCounter;

    

    [Header("Lives")]
    [SerializeField] private int lives;

    private bool isRespawning;

    float timeToFinish = 40;

    private Player player;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }


    void Start()
    {

        player = PlayerManager.Instance.GetPlayer();
        vulkan.hasBeenUnlocked = false;
        homingMissile.hasBeenUnlocked = false;

        player.currentEnergy = 100;

        GameMenu.Instance.SetMaxVulkanAmmoUI(vulkan.GetMaxAmmo());       
        GameMenu.Instance.SetMaxConcussionAmmoUI(concussionMissile.GetMaxAmmo()); 
        GameMenu.Instance.SetMaxHomingAmmoUI(homingMissile.GetMaxAmmo());
        GameMenu.Instance.SetScoreUI(totalPoints);
        GameMenu.Instance.SetLivesUI(lives);

        concussionMissile.InitiateWeapon();
        homingMissile.currentAmmo = 0;
        vulkan.currentAmmo = 0;

        Debug.Log(laser.currentAmmo);

        laser.currentAmmo = player.currentEnergy;
        flare.currentAmmo = player.currentEnergy;

        Debug.Log(laser.currentAmmo);

        GameMenu.Instance.SetConcussionAmmoUI(concussionMissile.currentAmmo);
        GameMenu.Instance.SetHomingAmmoUI(0);
        GameMenu.Instance.SetVulkanAmmoUI(0);
     
    }

    private void Update()
    {
        if (hasSelfDestructionStarted)
        {
            FinishGame();
        }

        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("MapTest");
        }

        if(Input.GetKey(KeyCode.O))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GetVulkan(PickupSO pickup)
    {
        if (!vulkan.hasBeenUnlocked)
        {
            vulkan.hasBeenUnlocked = true;
            vulkan.currentAmmo += pickup.GetValueToAdd();

            GameMenu.Instance.SetVulkanAmmoUI(vulkan.currentAmmo);
        }      
    }

    public void SupplyGunAmmunition(PickupSO pickup)
    {
        Debug.Log(pickup);

        if (pickup.GetPickupType() == PickupType.ENERGY)
        {
            Debug.Log(pickup.GetValueToAdd());
            SupplyEnergy(pickup.GetValueToAdd());
        }

        if (pickup.GetPickupType() == PickupType.VULKAN_AMMO)
        {                    
            vulkan.currentAmmo += pickup.GetValueToAdd();

            if (vulkan.currentAmmo > vulkan.GetMaxAmmo())
            {
                vulkan.currentAmmo = vulkan.GetMaxAmmo();
            }           

            GameMenu.Instance.SetVulkanAmmoUI(vulkan.currentAmmo);
        }

        if (pickup.GetPickupType() == PickupType.HOMING_MISSILE)
        {
            if (!homingMissile.hasBeenUnlocked)
            {
                homingMissile.InitiateWeapon();
                homingMissile.hasBeenUnlocked = true;
            }
            else
            {
                homingMissile.currentAmmo += pickup.GetValueToAdd();

                if (homingMissile.currentAmmo > homingMissile.GetMaxAmmo())
                {
                    homingMissile.currentAmmo = homingMissile.GetMaxAmmo();
                }
            }

            GameMenu.Instance.SetHomingAmmoUI(homingMissile.currentAmmo);
        }

        if (pickup.GetPickupType() == PickupType.CONCUSSION_MISSILE)
        {
            concussionMissile.currentAmmo += pickup.GetValueToAdd();

            if (concussionMissile.currentAmmo > concussionMissile.GetMaxAmmo())
            {
                concussionMissile.currentAmmo = concussionMissile.GetMaxAmmo();
            }
            GameMenu.Instance.SetConcussionAmmoUI(concussionMissile.currentAmmo);

        }
    }

    public void SupplyEnergy(float value)
    {
        player.currentEnergy += value;

        if(player.currentEnergy > player.GetMaxEnergy())
            player.currentEnergy = player.GetMaxEnergy();

        GameMenu.Instance.SetEnergyUI(player.currentEnergy);
    }

    public void CollectKey()
    {
        GameMenu.Instance.ShowKeyUI();
        canOpenDoor = true;
    }

    public bool GetCanOpenDoor()
    {
        return canOpenDoor;
    }

    public void IncreaseHostageCounter()
    {
        hostageCounter++;
    }

    public void IncreaseScore(int score)
    {
        totalPoints += score;

        GameMenu.Instance.SetScoreUI(totalPoints);
    }

    public void Death()
    {
        lives--;

        if (lives >= 0 && !isRespawning)
        {
            Respawn();        
        }

        if (lives < 0)
        {
            lives = 0;
            RestartGame($"You lost. Score: {totalPoints}");
        }

        GameMenu.Instance.SetLivesUI(lives);
    }

    public void Respawn()
    {
        isRespawning = true;

        player.GetComponent<SphereCollider>().enabled = false;
        InputManager.Instance.OnDisable();
        //reset weapons and remove points

        if(vulkan.hasBeenUnlocked)
            Instantiate(vulkanPrefab, player.transform.position, Quaternion.identity);

        DropAmmo();

        vulkan.hasBeenUnlocked = false;
        homingMissile.hasBeenUnlocked = false;

        concussionMissile.InitiateWeapon();
        

        totalPoints = totalPoints - (hostageCounter * hostage.GetValueToAdd());
        hostageCounter = 0;

        GameMenu.Instance.SetScoreUI(totalPoints);

        player.ResetHpEnergyAndWeapon();
        player.transform.position = respawnPoint.transform.position;
        player.GetComponent<SphereCollider>().enabled = true;
        InputManager.Instance.OnEnable();

        isRespawning = false;
    }

    public void DropAmmo()
    {
        while (homingMissile.currentAmmo > 0)
        {
            if (homingMissile.currentAmmo - 4 >= 0)
            {
                homingMissile.currentAmmo -= 4;
                Instantiate(homingMissileX4Prefab, player.transform.position, Quaternion.identity);
            }
            else if (homingMissile.currentAmmo - 1 >= 0)
            {
                homingMissile.currentAmmo -= 1;
                Instantiate(homingMissilePrefab, player.transform.position, Quaternion.identity);
            }
        }
        
        while (concussionMissile.currentAmmo > 0)
        {
            if (concussionMissile.currentAmmo - 4 >= 0)
            {
                concussionMissile.currentAmmo -= 4;
                Instantiate(concussionMissileX4Prefab, player.transform.position, Quaternion.identity);
            }
            else if (concussionMissile.currentAmmo - 1 >= 0)
            {
                concussionMissile.currentAmmo -= 1;
                Instantiate(concussionMissilePrefab, player.transform.position, Quaternion.identity);
            }
        }
    }

    public void FinishGame()
    {
        timeToFinish -= Time.deltaTime;
        Debug.Log((int)timeToFinish);
        Cursor.visible = true;

        if (timeToFinish > 0 && hasExitedBuilding)
        {
            RestartGame($"You won. Score: {totalPoints}. Play Again?");
        }
        else if (timeToFinish < 0 && !hasExitedBuilding)
        {
            RestartGame($"You lost. Score: {totalPoints}. Play Again?");
        }
    }

    public void RestartGame(string dialog)
    {
        PauseGame(true);

        QuestionDialogUI.Instance.ShowQuestion(dialog,
            () =>
            {
                PauseGame(false);
                SceneManager.LoadScene(GameMenu.Instance.sceneName);
            },
            () =>
            {
                QuestionDialogUI.Instance.ShowQuestion("This will close the game, are you sure?",
                    () =>
                    {
                        Application.Quit();
                    },
                    () =>
                    {
                        RestartGame(dialog);
                    });
            });
    }

    public virtual void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0;
            InputManager.Instance.OnDisable();
            Cursor.visible = true;
        }
        else
        {
            InputManager.Instance.OnEnable();
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }

    public bool GetHasSelfDestructionStarted()
    {
        return hasSelfDestructionStarted;
    }

    public void SetHasSelfDestructionStarted(bool value)
    {
        hasSelfDestructionStarted = value;
    }

    public void SetHasExitedBuild(bool value)
    {
        hasExitedBuilding = value;
    }

    public int GetTotalScore()
    {
        return totalPoints;
    }
}
