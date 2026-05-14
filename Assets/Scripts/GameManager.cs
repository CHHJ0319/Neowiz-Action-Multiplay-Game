using System.Collections;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isTest = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            Initiailize();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Events.GameEvents.OnStartHost += StartHost;
        Events.GameEvents.OnStartClient += StartClient;
        Events.GameEvents.OnQuitGame += QuitGame;
    }

    private void OnDisable()
    {
        Events.GameEvents.OnStartHost -= StartHost;
        Events.GameEvents.OnStartClient -= StartClient;
        Events.GameEvents.OnQuitGame -= QuitGame;
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += ForceDisconnect;
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= ForceDisconnect;
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    private void Initiailize()
    {
        ClearEvents();
    }

    #region Network Service
    private async void StartHost(string playerName, string teamName, string password)
    {
        await Services.UGSService.InitializeUnityServicesAsync();
        StartCoroutine(StartHostSequence(playerName, teamName, password));
    }

    private async void StartClient(string joinCode, string playerName, string password)
    {
        await Services.UGSService.InitializeUnityServicesAsync();
        StartCoroutine(StartClientSequence(joinCode, playerName, password));
    }

    private IEnumerator StartHostSequence(string playerName, string teamName, string password)
    {
        NetworkManager.Singleton.Shutdown();
        while (NetworkManager.Singleton.ShutdownInProgress)
        {
            yield return null;
        }

        yield return StartCoroutine(Services.NetworkService.ConfigureTransportAndStartNgoAsHost());

        if (!NetworkManager.Singleton.IsHost)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(HandleHostConnectionSuccess(playerName, teamName, password));
    }

    private IEnumerator StartClientSequence(string joinCode, string playerName, string password)
    {
        NetworkManager.Singleton.Shutdown();
        while (NetworkManager.Singleton.ShutdownInProgress)
        {
            yield return null;
        }

        yield return StartCoroutine(Services.NetworkService.ConfigureTransportAndStartNgoAsClient(joinCode, password));

        if (!NetworkManager.Singleton.IsClient)
        {
            yield break;
        }

        yield return StartCoroutine(HandleClientConnectionSuccess(playerName));
    }

    private IEnumerator HandleHostConnectionSuccess(string playerName, string teamName, string password)
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        yield return null;

        DataManager.Instance.SetPlayerName(playerName);
        SessionManager.Instance.Initialize(teamName, password);
        yield return null;

        if (isTest)
        {
            UIManager.Instance.Initialize((int)NetworkManager.Singleton.LocalClientId, Utils.SceneNavigator.GetCurrentSceneName());
        }
        else
        {
            Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.LobbyScene);
        }
        yield return null;
    }

    private IEnumerator HandleClientConnectionSuccess(string playerName)
    {
        DataManager.Instance.SetPlayerName(playerName);
        SessionManager.Instance.AddPlayerServerRpc();
        UIManager.Instance.Initialize((int)NetworkManager.Singleton.LocalClientId, Utils.SceneNavigator.GetCurrentSceneName());
        yield return null;
    }

    private void ForceDisconnect(ulong clientId)
    {
        if (IsServer)
        {
            if (Utils.SceneNavigator.GetCurrentSceneName() == Utils.SceneList.Stage1Scene.ToString()
            || Utils.SceneNavigator.GetCurrentSceneName() == Utils.SceneList.Stage2Scene.ToString())
            {
                string message = Utils.LocaleLoader.GetConnectionMessage("ERR_CLIENT_DISCONNECTED_NETWORK");
                UIManager.Instance.ShowCommonPopup(message);
                UIManager.Instance.SetupCommonPopup(() =>
                {
                    Disconnect();
                });
            }
        }
        else
        {
            string message = Utils.LocaleLoader.GetConnectionMessage("ERR_HOST_DISCONNECTED_DISBAND");
            UIManager.Instance.ShowCommonPopup(message);
            UIManager.Instance.SetupCommonPopup(() =>
            {
                Disconnect();
            });
        }
    }

    public void Disconnect()
    {
        if (Services.NetworkService.IsHost())
        {
            SessionManager.Instance.ClearServerRpc();
        }
        else
        {
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            {
                SessionManager.Instance.RemovePlayerServerRpc();
            }

            if (Utils.SceneNavigator.GetCurrentSceneName() == Utils.SceneList.LobbyScene.ToString())
            {
                UIManager.Instance.DisablePlayerPanel();
            }
        }

        Services.NetworkService.ShutdownNetwork();
        Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.TitleScene);
    }
    #endregion

    private void ClearEvents()
    {
        Events.GameEvents.Clear();
        Events.ActorEvents.Clear();
        Events.UIEvents.Clear();
        Events.RoundEvents.Clear();
        Events.ActorEvents.Clear();

    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (request.ClientNetworkId == NetworkManager.Singleton.LocalClientId)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            return;
        }

        byte[] connectionData = request.Payload;
        string clientPassword = Encoding.ASCII.GetString(connectionData);

        string password = SessionManager.Instance.CurrentSessionPassword;
        if (clientPassword == password)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            Debug.Log("클라이언트 접속 승인 완료");
        }
        else
        {
            response.Approved = false;
            response.Reason = "비밀번호가 일치하지 않습니다."; 
            Debug.Log("클라이언트 접속 거부: 비밀번호 불일치");
        }

        response.Pending = false;
    }

    private void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }
}
