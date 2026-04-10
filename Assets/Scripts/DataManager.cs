using System.Collections;
using Unity.Netcode;

public class DataManager : NetworkBehaviour
{
    public static DataManager Instance { get; private set; }

    public string PlayerName { get; private set; }
    public int ID { get; private set; }

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

    public IEnumerator SetClientInfo()
    {
        ID = SessionManager.Instance.PlayerCount.Value + 1;
        PlayerName = "Player" + ID;

        yield return null;
    }
}
