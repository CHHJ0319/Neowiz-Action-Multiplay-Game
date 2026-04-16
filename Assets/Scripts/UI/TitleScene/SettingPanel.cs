using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPanel : MonoBehaviour
{
    [Header("Sliders")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Mute Button Labels")]
    public TextMeshProUGUI textBgm;
    public TextMeshProUGUI textSfx;

    [SerializeField] private Button closeButton;

    private bool isBgmMuted;
    private bool isSfxMuted;

    private float lastBgmVolume = 1f;
    private float lastSfxVolume = 1f;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => SetVisible(false));
    }

    private void Start()
    {
        bgmSlider.minValue = 0f;
        bgmSlider.maxValue = 1f;
        bgmSlider.wholeNumbers = false;

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        sfxSlider.wholeNumbers = false;

        lastBgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        lastSfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);

        isBgmMuted = PlayerPrefs.GetInt("BGM_MUTED", 0) == 1;
        isSfxMuted = PlayerPrefs.GetInt("SFX_MUTED", 0) == 1;

        bgmSlider.value = lastBgmVolume;
        sfxSlider.value = lastSfxVolume;

        ApplyVolumes();
        UpdateMuteLabels();
    }

    public void OnBgmChanged()
    {
        lastBgmVolume = bgmSlider.value;
        PlayerPrefs.SetFloat("BGM_VOLUME", lastBgmVolume);

        if (isBgmMuted && lastBgmVolume > 0f)
        {
            isBgmMuted = false;
            PlayerPrefs.SetInt("BGM_MUTED", 0);
        }

        ApplyVolumes();
        UpdateMuteLabels();
    }

    public void OnSfxChanged()
    {
        lastSfxVolume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFX_VOLUME", lastSfxVolume);

        if (isSfxMuted && lastSfxVolume > 0f)
        {
            isSfxMuted = false;
            PlayerPrefs.SetInt("SFX_MUTED", 0);
        }

        ApplyVolumes();
        UpdateMuteLabels();
    }

    public void ToggleBgmMute()
    {
        isBgmMuted = !isBgmMuted;
        PlayerPrefs.SetInt("BGM_MUTED", isBgmMuted ? 1 : 0);

        if (isBgmMuted)
        {
            bgmSlider.value = 0f;
        }
        else
        {
            bgmSlider.value = lastBgmVolume;
        }


        ApplyVolumes();
        UpdateMuteLabels();
    }

    public void ToggleSfxMute()
    {
        isSfxMuted = !isSfxMuted;
        PlayerPrefs.SetInt("SFX_MUTED", isSfxMuted ? 1 : 0);

        if (isSfxMuted)
        {
            sfxSlider.value = 0f;
        }
        else
        {
            sfxSlider.value = lastSfxVolume;
        }

        ApplyVolumes();
        UpdateMuteLabels();
    }

    private void ApplyVolumes()
    {
        float appliedBgm = isBgmMuted ? 0f : lastBgmVolume;
        float appliedSfx = isSfxMuted ? 0f : lastSfxVolume;
    }

    private void UpdateMuteLabels()
    {
        if (textBgm != null)
        {
            textBgm.text = isBgmMuted ? "BGM OFF" : "BGM";
        }

        if (textSfx != null)
        {
            textSfx.text = isSfxMuted ? "SFX OFF" : "SFX";
        }
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}