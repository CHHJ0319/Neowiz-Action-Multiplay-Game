using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioMixer gameAudioMixer;
    
    public bool IsBGMMuted { get; private set; }
    public bool IsSFXMuted { get; private set; }

    public float BGMVolume { get; private set; } = 1f;
    public float SFXVolume { get; private set; } = 1f;
    public float LastBGMVolume { get; private set; } = 1f;
    public float LastSFXVolume { get; private set; } = 1f;

    private const string BGM_VOLUME_KEY = "BGM_VOLUME";
    private const string SFX_VOLUME_KEY = "SFX_VOLUME";
    private const string BGM_MUTE_KEY = "BGM_MUTED";
    private const string SFX_MUTE_KEY = "SFX_MUTED";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadSettings();
    }

    public void SetBGMVolume(float volume)
    {
        BGMVolume = volume;

        float dB = volume > 0.0001f ? Mathf.Log10(volume) * 20 : -80f;
        gameAudioMixer.SetFloat("BGMVolume", dB);

        if (IsBGMMuted && BGMVolume > 0f)
        {
            IsBGMMuted = false;
        }
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;

        float dB = volume > 0.0001f ? Mathf.Log10(volume) * 20 : -80f;
        gameAudioMixer.SetFloat("SFXVolume", dB);

        if (IsSFXMuted && SFXVolume > 0f)
        {
            IsSFXMuted = false;
        }
    }

    public void ToggleBGMMute()
    {
        IsBGMMuted = !IsBGMMuted;
    }

    public void ToggleSFXMute()
    {
        IsSFXMuted = !IsSFXMuted;
    }

    public void BackupBGMVolume()
    {
        LastBGMVolume = BGMVolume;
    }

    public void LoadLastBGMVolume()
    {
        BGMVolume = LastBGMVolume;
    }

    public void BackupSFXVolume()
    {
        LastSFXVolume = SFXVolume;
    }

    public void LoadLastSFXVolume()
    {
        SFXVolume = LastSFXVolume;
    }

    public void LoadSettings()
    {
        BGMVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        IsBGMMuted = PlayerPrefs.GetInt(BGM_MUTE_KEY, 0) == 1;
        IsSFXMuted = PlayerPrefs.GetInt(SFX_MUTE_KEY, 0) == 1;

        UIManager.Instance.UpdateSettingPanel();
    }

    public void SaveSettings()
    {
        if(IsBGMMuted)
        {
            PlayerPrefs.SetInt(BGM_MUTE_KEY, 1);
            PlayerPrefs.SetFloat(BGM_VOLUME_KEY, LastBGMVolume);
        }
        else
        {
            PlayerPrefs.SetInt(BGM_MUTE_KEY, 0);
            PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
        }

        if (IsSFXMuted)
        {
            PlayerPrefs.SetInt(SFX_MUTE_KEY, 1);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, LastSFXVolume);
        }
        else
        {
            PlayerPrefs.SetInt(SFX_MUTE_KEY, 0);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SFXVolume);
        }
    }
}
