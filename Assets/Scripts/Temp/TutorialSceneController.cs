using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneController : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void AcceptPrologue()
    {
        Debug.Log("프롤로그 수락");
        // 나중에 다음 튜토리얼 페이지나 게임 씬으로 이동
    }
}