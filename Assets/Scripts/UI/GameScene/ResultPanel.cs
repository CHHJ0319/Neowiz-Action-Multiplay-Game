using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [Header("TitleImage")]
    public GameObject victoryImage;
    public GameObject defeatImage;

    [Header("")]
    public Transform starRow;
    public TextMeshProUGUI mvpNameText;

    [Header("Buttons")]
    public Button nextStageButton;

    void Awake()
    {
        SetTitleImage(true);

        if(nextStageButton != null)
        {
            nextStageButton.onClick.AddListener(() => OnNextStageButtonClicked());
        }
    }

    private void Update()
    {
    }

    public void SetTitleImage(bool isVictory)
    {
        victoryImage.SetActive(false);
        defeatImage.SetActive(false);

        if (isVictory)
        {
            victoryImage.SetActive(true);
        }

        else
        {
            defeatImage.SetActive(true);
        }
    }

    public void SetStars(int starCount)
    {
        int totalChildren = starRow.childCount;

        for (int i = 0; i < totalChildren; i++)
        {
            starRow.GetChild(i).gameObject.SetActive(i < starCount);
        }
    }

    public void SetMVP(string playerrName)
    {
        if (mvpNameText != null)
        {
            mvpNameText.text = $"MVP: {playerrName}";
        }
    }

    private void OnNextStageButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
