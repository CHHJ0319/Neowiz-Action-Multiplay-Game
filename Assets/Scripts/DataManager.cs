using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public string PlayerName { get; private set; }
    public int ID { get; private set; }
    public int SessionPlayerIndex { get; private set; }
    public int CharacterIndex { get; private set; }

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

    public void SetClientInfo(int id)
    {
        ID = id;
        PlayerName = "PLAYER" + ID;
    }

    public void SetSessionPlayerIndex(int index)
    {
        SessionPlayerIndex = index;
    }

    public void SetCharacterIndex(int index)
    {
        CharacterIndex = index;
    }
}
