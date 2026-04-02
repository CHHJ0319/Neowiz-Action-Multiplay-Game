using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

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

    private void Initiailize()
    {
        //ClearEvents();

        Utils.NetworkService.InitializeUnityServicesAsync();
    }

    public static void LoadTitleScene()
    {
        Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.TitleScene);
    }

    #region Network Service
    private void StartHost()
    {
        StartCoroutine(StartHostSequence());
    }

    private void StartClient(string joinCode)
    {
        StartCoroutine(StartClientSequence(joinCode));
    }

    private IEnumerator StartHostSequence()
    {
        yield return StartCoroutine(Utils.NetworkService.ConfigureTransportAndStartNgoAsHost());

        if (!NetworkManager.Singleton.IsHost)
        {
            yield break;
        }

        yield return StartCoroutine(ActorManager.Instance.SpawnPlayer(NetworkManager.Singleton.LocalClientId));
        yield return new WaitForSeconds(0.1f);

        UIManager.Instance.Initialize((int)NetworkManager.Singleton.LocalClientId, Utils.SceneNavigator.GetCurrentSceneName());
        //Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.LobbyScene);
    }

    private IEnumerator StartClientSequence(string joinCode)
    {
        yield return StartCoroutine(Utils.NetworkService.ConfigureTransportAndStartNgoAsClient(joinCode));

        yield return StartCoroutine(ActorManager.Instance.SpawnPlayer(NetworkManager.Singleton.LocalClientId));
    
        UIManager.Instance.Initialize((int)NetworkManager.Singleton.LocalClientId, Utils.SceneNavigator.GetCurrentSceneName());
    }
    #endregion

    private void ClearEvents()
    {
        Events.GameEvents.Clear();
        Events.ActorEvents.Clear();
        Events.UIEvents.Clear();
        Events.RoundEvents.Clear();
        Events.PlayerFieldEvents.Clear();

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
