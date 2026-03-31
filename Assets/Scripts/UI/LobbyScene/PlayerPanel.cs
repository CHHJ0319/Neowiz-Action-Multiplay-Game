using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

public class PlayerPanel : MonoBehaviour
{
    public GameObject characterImages;
    public TextMeshProUGUI playeyName;
    public Button previousButton;
    public Button nextButton;

    private int currentIndex = 0;

    void Awake()
    {
        previousButton.onClick.AddListener(OnPreviousButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    void Start()
    {
        ShowCharacterImage();
    }

    private void ShowCharacterImage() 
    {
        characterImages.SetActive(true);
    }

    private void OnPreviousButtonClicked()
    {
        int childCount = characterImages.transform.childCount;
        if (childCount == 0) return;

        currentIndex--;
        if (currentIndex < 0) currentIndex = childCount - 1;

        RefreshDisplay();
    }

    private void OnNextButtonClicked()
    {
        int childCount = characterImages.transform.childCount;
        if (childCount == 0) return;

        currentIndex = (currentIndex + 1) % childCount;

        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        int childCount = characterImages.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            bool isActive = (i == currentIndex);
            characterImages.transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }
}
