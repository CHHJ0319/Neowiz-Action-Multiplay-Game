using UnityEngine;
using UnityEngine.UI;

public class OptionButton: MonoBehaviour
{
    [Header("Individual Sound")]
    public AudioClip clickSound;

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
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clickSound, cameraPos);
        }
    }
}