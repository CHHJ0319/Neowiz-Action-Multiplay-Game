using System.Collections;
using Unity.Netcode;

public class DataManager : NetworkBehaviour
{
    public static DataManager Instance { get; private set; }

    public string PlayerName { get; private set; }
    public int ClientID{ get; private set; }

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

    public IEnumerator SetClientInfo(int id)
    {
        ClientID = id;
        PlayerName = "Player" + (ClientID + 1);

        yield return null;
    }
}
