using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public GameObject victoryImage;
    public GameObject defeatImage;

    public Transform starRow;
    public TextMeshProUGUI mvpNameText;

    void Awake()
    {
        SetTitleImage(true);
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
}
