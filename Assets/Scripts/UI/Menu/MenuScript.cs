using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private string sceneName = "TestLevel";
    //[SerializeField] private GameObject leaderboardUI;
    [SerializeField] private GameObject howToPlayUI;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject MainUI;

    void Update()
    {
        /*//exit game on esc
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(leaderboardUI);


        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(creditsUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(howToPlayUI);*/
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);

            CheckForInGameUI();

            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        SwitchTo(MainUI);
    }
    public void SwitchTo(GameObject _menu)
    {


        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }

        /*if (GameManager.Instance != null)
        {
            if (_menu == MainUI)
                GameManager.Instance.PauseGame(false);
            else
                GameManager.Instance.PauseGame(true);

        }*/
    }


    //load level
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    //credits
    /*public void CreditsScene()
    {
        SceneManager.LoadScene(2);
    }*/

    //howtoplay
    /*public void HowToScene()
    {
        SceneManager.LoadScene(3);
    }*/

    //exit game
    public void ExitGame()
    {
        QuestionDialogUI.Instance.ShowQuestion("This action will close the application, are you sure?",
            () =>
            {
                Application.Quit();
            },
            () =>
            {
                SwitchWithKeyTo(MainUI);
            });
        //Application.Quit();
    }

    //GoBack
    /*public void GoBack()
    {
        SceneManager.LoadScene(0);
    }*/

}
