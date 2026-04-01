using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUIController : MonoBehaviour
{
    [Header("Overlay")]
    public GameObject overlayBlocker;

    [Header("Popups")]
    public GameObject popupCreateSession;
    public GameObject popupJoinSession;
    public GameObject popupSettings;

    private void Start()
    {
        // 시작 시 전부 꺼두기
        if (overlayBlocker != null) overlayBlocker.SetActive(false);
        if (popupCreateSession != null) popupCreateSession.SetActive(false);
        if (popupJoinSession != null) popupJoinSession.SetActive(false);
        if (popupSettings != null) popupSettings.SetActive(false);
    }


    // =========================
    // Settings
    // =========================
    public void OpenSettingsPopup()
    {

        if (overlayBlocker != null) overlayBlocker.SetActive(true);
        if (popupSettings != null) popupSettings.SetActive(true);
    }

    public void CloseSettingsPopup()
    {
        if (overlayBlocker != null) overlayBlocker.SetActive(false);
        if (popupSettings != null) popupSettings.SetActive(false);
    }
}