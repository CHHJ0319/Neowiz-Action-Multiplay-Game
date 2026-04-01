using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Utils.SceneLoader.LoadSceneByName(Utils.SceneList.TitleScene);
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

        yield return StartCoroutine(ActorManager.Instance.SpawnPlayer(NetworkManager.Singleton.LocalClientId));
    }

    private IEnumerator StartClientSequence(string joinCode)
    {
        yield return StartCoroutine(Utils.NetworkService.ConfigureTransportAndStartNgoAsClient(joinCode));

        yield return StartCoroutine(ActorManager.Instance.SpawnPlayer(NetworkManager.Singleton.LocalClientId));
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
