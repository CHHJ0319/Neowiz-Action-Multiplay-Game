using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public string PlayerName { get; private set; }
    public int ID { get; private set; }
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

    public void SetID(int id)
    {
        ID = id;
    }

    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    public void SetCharacterIndex(int index)
    {
        CharacterIndex = index;
    }

    public async void SaveRanking(Data.TeamData teamData)
    {
        await Services.UGSService.SubmitTeamScoreAsync(teamData);
    }

    public async Task<List<LeaderboardEntry>> FetchRankings()
    {
        var rankings = await Services.UGSService.FetchRankingsAsync();
        return rankings;
    }
}
