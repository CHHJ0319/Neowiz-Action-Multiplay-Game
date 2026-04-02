using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class PlayerPanel : NetworkBehaviour
    {
        public GameObject characterImages;
        public TextMeshProUGUI playeyName;
        public Button previousButton;
        public Button nextButton;
        public Image readyIcon;

        public NetworkVariable<bool> isReady = new NetworkVariable<bool>();

        private NetworkVariable<int> currentIndex = new NetworkVariable<int>(0);

        void Awake()
        {
            previousButton.onClick.AddListener(() => OnPreviousButtonClicked());
            nextButton.onClick.AddListener(() => OnNextButtonClicked());
        }

        public override void OnNetworkSpawn()
        {
            currentIndex.OnValueChanged += RefreshDisplay;
            isReady.OnValueChanged += UpdateReadyIcon;
        }

        public override void OnNetworkDespawn()
        {
            Events.GameEvents.OnReadyGame -= UpdateReadyState;

            currentIndex.OnValueChanged -= RefreshDisplay;
            isReady.OnValueChanged -= UpdateReadyIcon;
        }

        public void Initialize(bool isOwner)
        {
            ShowCharacterImage();

            if(isOwner)
            {
                previousButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);

                Events.GameEvents.OnReadyGame += UpdateReadyState;
            }
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

        private void UpdateReadyState()
        {
            UpdateReadyStateServerRpc();
        }

        [Rpc(SendTo.Server)]
        private void UpdateReadyStateServerRpc(RpcParams rpcParams = default)
        {
            isReady.Value = !isReady.Value;
        }

        private void UpdateReadyIcon(bool previousValue, bool newValue)
        {
            if (readyIcon == null) return;

            bool currentState = readyIcon.gameObject.activeSelf;
            readyIcon.gameObject.SetActive(!currentState);
        }
    }

}