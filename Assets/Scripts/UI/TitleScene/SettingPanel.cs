using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.TitleScene
{
    public class SettingPanel : MonoBehaviour
    {
        public static SettingPanel Instance;

        [Header("Mute Buttons")]
        public Button bgmMuteButton;
        public TextMeshProUGUI bgmMuteButtonLabel;
        public Button sfxMuteButton;
        public TextMeshProUGUI sfxMuteButtonLabel;

        [Header("Sliders")]
        public Slider bgmSlider;
        public Slider sfxSlider;

        [SerializeField] private Button saveButton;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
        }

        private void Start()
        {
            UpdateUIState();
        }

        private void Initialize()
        {
            bgmSlider.minValue = 0f;
            bgmSlider.maxValue = 1f;
            bgmSlider.wholeNumbers = false;

            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.wholeNumbers = false;

            bgmSlider.onValueChanged.AddListener(OnBgmSliderMoved);
            sfxSlider.onValueChanged.AddListener(OnSfxSliderMoved);

            bgmMuteButton.onClick.AddListener(() => ToggleBgmMute());
            sfxMuteButton.onClick.AddListener(() => ToggleSfxMute());

            saveButton.onClick.AddListener(() => OnSaveButtonClicked());
            closeButton.onClick.AddListener(() => OnCloseButtonClicked());
        }

        public void UpdateUIState()
        {
            if (SoundManager.Instance.IsBGMMuted)
            {
                bgmSlider.value = 0f;
            }
            else
            {
                bgmSlider.value = SoundManager.Instance.BGMVolume;
            }

            if (SoundManager.Instance.IsSFXMuted)
            {
                sfxSlider.value = 0f;
            }
            else
            {
                sfxSlider.value = SoundManager.Instance.SFXVolume;
            }
        }

        private void OnBgmSliderMoved(float value)
        {
            SoundManager.Instance.SetBGMVolume(value);

            UpdateMuteLabels();
        }

        private void OnSfxSliderMoved(float value)
        {
            SoundManager.Instance.SetSFXVolume(value);

            UpdateMuteLabels();
        }

        private void ToggleBgmMute()
        {
            SoundManager.Instance.ToggleBGMMute();

            if (SoundManager.Instance.IsBGMMuted)
            {
                SoundManager.Instance.BackupBGMVolume();
                bgmSlider.value = 0f;
            }
            else
            {
                SoundManager.Instance.LoadLastBGMVolume();
                bgmSlider.value = SoundManager.Instance.BGMVolume;
            }

            UpdateMuteLabels();
        }

        private void ToggleSfxMute()
        {
            SoundManager.Instance.ToggleSFXMute();

            if (SoundManager.Instance.IsSFXMuted)
            {
                SoundManager.Instance.BackupSFXVolume();
                sfxSlider.value = 0f;
            }
            else
            {
                SoundManager.Instance.LoadLastSFXVolume();
                sfxSlider.value = SoundManager.Instance.SFXVolume;
            }

            UpdateMuteLabels();
        }

        private void UpdateMuteLabels()
        {
            bool isBgmMuted = SoundManager.Instance.IsBGMMuted;
            bool isSfxMuted = SoundManager.Instance.IsSFXMuted;

            if (bgmMuteButtonLabel != null)
            {
                bgmMuteButtonLabel.text = isBgmMuted ? "BGM OFF" : "BGM";
            }

            if (sfxMuteButtonLabel != null)
            {
                sfxMuteButtonLabel.text = isSfxMuted ? "SFX OFF" : "SFX";
            }
        }

        private void OnSaveButtonClicked()
        {
            SoundManager.Instance.SaveSettings();
            SetVisible(false);
        }

        private void OnCloseButtonClicked()
        {
            SoundManager.Instance.LoadSettings();
            SetVisible(false);
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}