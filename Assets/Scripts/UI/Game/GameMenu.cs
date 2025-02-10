using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject restartGameUI;
    [SerializeField] private GameObject pauseGameUI;
    [SerializeField] private GameObject playerControlsUI;
    [SerializeField] private GameObject customiseGameSettingsUI;
    public string sceneName;
    public static GameMenu Instance;

    [SerializeField] private Image keyImage;

    [Header("Score / lives")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI lives;

    [Header("Hp / Energy")]
    [SerializeField] private Slider hp;
    [SerializeField] private Slider energy;

    [SerializeField] private WeaponSO homingMissile;
    [SerializeField] private WeaponSO vulkanGun;


    [Header("Ammo")]
    [SerializeField] private Slider concussionMissileSlider;
    [SerializeField] private TextMeshProUGUI concussionMissileAmmo;

    [SerializeField] private Slider homingMissileSlider;
    [SerializeField] private TextMeshProUGUI homingMissileAmmo;

    [SerializeField] private Slider vulkanAmmoSlider;
    [SerializeField] private TextMeshProUGUI vulkanAmmo;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        keyImage.gameObject.SetActive(false);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SwitchWithKeyTo(restartGameUI);
        }

        if (Input.GetKeyDown(KeyCode.F3))
            SwitchWithKeyTo(pauseGameUI);

        if (Input.GetKeyDown(KeyCode.F8))
            SwitchWithKeyTo(playerControlsUI);

        if (Input.GetKeyDown(KeyCode.F9))
            SwitchWithKeyTo(customiseGameSettingsUI);
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            QuestionDialogUI.Instance.Hide();
            _menu.SetActive(false);

            CheckForInGameUI();

            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        SwitchTo(inGameUI);
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }


        if (inGameUI != null)
        {
            inGameUI.SetActive(true);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }

        if (GameManager.Instance != null)
        {
            if (_menu == inGameUI)
                GameManager.Instance.PauseGame(false);
            else
                GameManager.Instance.PauseGame(true);

        }
    }

    public void SetMaxHpUI(float value)
    {
        hp.maxValue = value;
    }

    public void SetMaxEnergyUI(float value)
    {
        energy.maxValue = value;
    }

    public void SetHpUI(float value)
    {
        hp.value = value;
    }

    public void SetEnergyUI(float value)
    {
        energy.value = value;
    }

    public void SetScoreUI(int value)
    {
        score.text = $"{value}";
    }

    public void SetLivesUI(int value)
    {
        lives.text = $"x{value}";
    }


    public void SetMaxConcussionAmmoUI(float value)
    {
        concussionMissileSlider.maxValue = value;
    }

    public void SetConcussionAmmoUI(float value)
    {

        concussionMissileAmmo.text = $"{value}";
        concussionMissileSlider.value = value;
    }

    public void SetMaxHomingAmmoUI(float value)
    {
        homingMissileSlider.maxValue = value;
    }

    public void SetHomingAmmoUI(float value)
    {
        if (!homingMissile.hasBeenUnlocked)
        {
            homingMissileAmmo.text = $"{0}";
            homingMissileSlider.value = 0;
        }
        else
        {
            homingMissileAmmo.text = $"{value}";
            homingMissileSlider.value = value;
        }
    }

    public void SetMaxVulkanAmmoUI(float value)
    {
        vulkanAmmoSlider.maxValue = value;  
    }

    public void SetVulkanAmmoUI(float value)
    {      
        vulkanAmmo.text = $"{value}";
        vulkanAmmoSlider.value = value;      
    }

    public void ShowKeyUI()
    {
        keyImage.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {

        QuestionDialogUI.Instance.ShowQuestion("This action will restart the game, are you sure?",
            () =>
            {
                Time.timeScale = 1.0f;
                restartGameUI.SetActive(false);
                SceneManager.LoadScene(sceneName);
            },
            () =>
            {
                SwitchWithKeyTo(inGameUI);
            });
    }
}
