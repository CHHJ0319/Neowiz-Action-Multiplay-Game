using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool IsAllPlayersReady()
    {
        int playerCount = ActorManager.Instance.GetPlayerCount();
        int readyCount = UIManager.Instance.GetReadyPlayerCount();

        if(playerCount > 1 && playerCount - 1 == readyCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
