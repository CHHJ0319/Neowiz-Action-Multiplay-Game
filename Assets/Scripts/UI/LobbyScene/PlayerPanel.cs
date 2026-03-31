using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : NetworkBehaviour
{
    public GameObject characterImages;
    public TextMeshProUGUI playeyName;
    public Button previousButton;
    public Button nextButton;

    private NetworkVariable<int> currentIndex = new NetworkVariable<int>(0);

    void Awake()
    {
        previousButton.onClick.AddListener(OnPreviousButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    public override void OnNetworkSpawn()
    {
        currentIndex.OnValueChanged += RefreshDisplay;
    }

    public override void OnNetworkDespawn()
    {
        currentIndex.OnValueChanged -= RefreshDisplay;
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
        GetNextIndexServerRpc(-1);
    }

    private void OnNextButtonClicked()
    {
        GetNextIndexServerRpc(1);
    }

    private void RefreshDisplay(int previousValue, int newValue)
    {
        int childCount = characterImages.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            bool isActive = (i == currentIndex.Value);
            characterImages.transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }

    [Rpc(SendTo.Server)]
    private void GetNextIndexServerRpc(int direction, RpcParams rpcParams = default)
    {
        int childCount = characterImages.transform.childCount;
        if (childCount == 0) currentIndex.Value = 0;

        int nextIndex = currentIndex.Value + direction;

        if (nextIndex < 0)
        {
            currentIndex.Value = childCount - 1;
        }
        else
        {
            currentIndex.Value = nextIndex % childCount;
        }
    }
}
