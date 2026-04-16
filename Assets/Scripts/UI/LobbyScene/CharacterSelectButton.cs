using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    public AudioClip clickSound;
    public float soundVolume = 1.0f;

    private void Awake()
    {
        Button btn = GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(() => PlayClickSound());
        }
    }

    private void PlayClickSound()
    {
        if (clickSound != null)
        {
            GameObject soundObj = new GameObject("TempClickSound");
            AudioSource asource = soundObj.AddComponent<AudioSource>();
            asource.clip = clickSound;
            asource.playOnAwake = false;
            asource.volume = soundVolume;

            DontDestroyOnLoad(soundObj);


            asource.Play();

            Destroy(soundObj, clickSound.length);
        }
    }
}