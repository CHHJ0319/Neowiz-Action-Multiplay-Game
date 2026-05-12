using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Leaderboards.Models;

public class SessionManager : NetworkBehaviour
{
    public static SessionManager Instance { get; private set; }

    public string CurrentSessionPassword { get; private set; }

    public NetworkVariable<FixedString64Bytes> TeamName = new NetworkVariable<FixedString64Bytes>();
    public NetworkVariable<int> PlayerCount = new NetworkVariable<int>(0);

    private Dictionary<string, int> scores = new();
    public int TotalScore { get; private set; }
    public int ExpectedRank { get; private set; }

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

    public void Initialize(string teamName, string password)
    {
        ClearServerRpc();
        SetSessionPassword(password);
        SetTeamNameServerRpc(teamName);
        AddPlayerServerRpc();
    }

    [Rpc(SendTo.Server)]
    public void SetTeamNameServerRpc(string teamName, RpcParams rpcParams = default)
    {
        TeamName.Value = teamName;
    }

    public string GetTeamName()
    {
        return TeamName.Value.ToString();
    }

    [Rpc(SendTo.Server)]
    public void AddPlayerServerRpc(RpcParams rpcParams = default)
    {
        PlayerCount.Value++;
    }

    [Rpc(SendTo.Server)]
    public void RemovePlayerServerRpc(RpcParams rpcParams = default)
    {
        PlayerCount.Value--;
    }

    [Rpc(SendTo.Server)]
    public void ClearServerRpc(RpcParams rpcParams = default)
    {
        PlayerCount.Value = 0;
        TeamName.Value = "";
        ResetTotalScore();
        ClearScores();
    }

    public bool IsAllPlayersReady()
    {
        int readyCount = UIManager.Instance.GetReadyPlayerCount();

        if(PlayerCount.Value > 3 && 
            PlayerCount.Value - 1 == readyCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetSessionPassword(string password)
    {
        CurrentSessionPassword = password;
    }

    #region Score
    [Rpc(SendTo.Server)]
    public void UpdateScoreServerRpc(string playerName, RpcParams rpcParams = default)
    {
        if (!scores.ContainsKey(playerName))
        {
            scores[playerName] = 0;
        }
        scores[playerName]++;
    }

    public string GetMVP()
    {
        if (scores.Count == 0) return string.Empty;

        string mvpName = string.Empty;
        int maxValue = int.MinValue;

        foreach (var kvp in scores)
        {
            if (kvp.Value > maxValue)
            {
                maxValue = kvp.Value;
                mvpName = kvp.Key;
            }
        }

        return mvpName;
    }

    public async void CalculateTotalScore()
    {
        int sum = 0;
        foreach (var score in scores.Values)
        {
            sum += score;
        }
        TotalScore += sum;
        long combinedScore = Algorythm.ScoreCalculator.GetCombinedScore(StageManager.Instance.WaveIndex, TotalScore);

        List<LeaderboardEntry> rankings = await DataManager.Instance.GetRankings();
        ExpectedRank = rankings.Count(entry => entry.Score > combinedScore) + 1;

        ClearScores();
    }

    private void ClearScores()
    {
        scores.Clear();
    }

    public void ResetTotalScore()
    {
        TotalScore = 0;
    }
    #endregion
}
