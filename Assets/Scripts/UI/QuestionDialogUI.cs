using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDialogUI : MonoBehaviour
{
    public static QuestionDialogUI Instance { get; private set; }
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] Button yesBtn;
    [SerializeField] Button noBtn;

    private void Awake()
    {
        Instance = this;

        Hide();
    }

    public void ShowQuestion(string questionText, Action yesAction, Action noAction)
    {
        gameObject.SetActive(true);

        textMeshPro.text = questionText;
        yesBtn.onClick.AddListener(() =>
        {
            Hide();
            yesAction();
        });
        noBtn.onClick.AddListener(() =>
        {
            Hide();
            noAction();
        });

    }

    public void Hide()
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}

