using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class PlayerPanel : NetworkBehaviour
    {
        public Transform characterImages;
        public TextMeshProUGUI playeyNameText;
        public Button previousButton;
        public Button nextButton;
        public Image readyIcon;

        private Image panelImage;

        public NetworkVariable<FixedString32Bytes> playeyName = new NetworkVariable<FixedString32Bytes>();
        public NetworkVariable<bool> isDisabled = new NetworkVariable<bool>();
        public NetworkVariable<bool> isReady = new NetworkVariable<bool>();
        private NetworkVariable<int> currentIndex = new NetworkVariable<int>(0);

        private Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        private Color normalColor = Color.white;

        void Awake()
        {
            panelImage = GetComponent<Image>();

            previousButton.onClick.AddListener(() => OnPreviousButtonClicked());
            nextButton.onClick.AddListener(() => OnNextButtonClicked());
        }

        public override void OnNetworkSpawn()
        {
            playeyName.OnValueChanged += UpdatePlayerNameText;
            isDisabled.OnValueChanged += UpdateVisualState;
            isReady.OnValueChanged += UpdateReadyIcon;
            currentIndex.OnValueChanged += RefreshDisplay;

            if (IsServer)
            {
                SetDisabledServerRpc(true);
            }
            else
            {
                UpdateVisualState(true, true);
                UpdatePlayerNameText("", "");
                UpdateReadyIcon(true, true);
            }
        }

        public override void OnNetworkDespawn()
        {
            Events.GameEvents.OnReadyGame -= UpdateReadyState;

            playeyName.OnValueChanged -= UpdatePlayerNameText;
            isDisabled.OnValueChanged -= UpdateVisualState;
            isReady.OnValueChanged -= UpdateReadyIcon;
            currentIndex.OnValueChanged -= RefreshDisplay;
        }

        public void Initialize()
        {
            SetDisabledServerRpc(false);

            SetPlayerNameServerRpc(DataManager.Instance.PlayerName);

            //ShowCharacterImage();

            previousButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);

            Events.GameEvents.OnReadyGame += UpdateReadyState;
        }

        public int GetCharacterIndex() 
        {
            return currentIndex.Value;
        }

        [Rpc(SendTo.Server)]
        private void SetPlayerNameServerRpc(string name, RpcParams rpcParams = default)
        {
            playeyName.Value = name;
        }

        private void UpdatePlayerNameText(FixedString32Bytes previousValue, FixedString32Bytes newValue)
        {
            playeyNameText.text = playeyName.Value.ToString();
        }

        private void ShowCharacterImage()
        {
            characterImages.gameObject.SetActive(true);
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
            int childCount = characterImages.childCount;

            for (int i = 0; i < childCount; i++)
            {
                bool isActive = (i == currentIndex.Value);
                characterImages.GetChild(i).gameObject.SetActive(isActive);
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
            readyIcon.gameObject.SetActive(isReady.Value);
        }

        [Rpc(SendTo.Server)]
        private void SetDisabledServerRpc(bool isDisabled)
        {
            this.isDisabled.Value = isDisabled;
        }

        private void UpdateVisualState(bool previousValue, bool newValue)
        {
            if (panelImage == null) return;
            panelImage.color = isDisabled.Value ? disabledColor : normalColor;
        }
    }

}