using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using UnityEngine;

namespace Services
{
    public static class UGSService
    {
        private const string LeaderboardId = "NAMGRanking";

        public static async Task InitializeUnityServicesAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    if (AuthenticationService.Instance.IsSignedIn)
                    {
                        AuthenticationService.Instance.SignOut(true);
                    }
                }
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static async Task SubmitTeamScoreAsync(Data.TeamData teamData)
        {
            try
            {
                await InitializeUnityServicesAsync();

                long combinedScore = (long)(teamData.finalRound * 1000000) + teamData.totalScore;
                var metadata = new Dictionary<string, object>
                {
                    { "teamName", teamData.teamName },
                    { "memberNames", teamData.memberNames },
                    { "finalRound", teamData.finalRound },
                    { "totalScore", teamData.totalScore }
                };

                var options = new AddPlayerScoreOptions
                {
                    Metadata = metadata
                };

                var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(
                    LeaderboardId,
                    combinedScore,
                    options
                );

                Debug.Log($"[UGS] 저장 성공! 순위: {playerEntry.Rank + 1}");
            }
            catch (LeaderboardsException e)
            {
                Debug.LogError($"[UGS] 리더보드 에러: {e.Reason} - {e.Message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[UGS] 알 수 없는 에러 발생: {e.Message}");
            }
        }

        public static async Task FetchTopRankingsAsync()
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Limit = 10 });

            foreach (var entry in scoresResponse.Results)
            {
                if (!string.IsNullOrEmpty(entry.Metadata))
                {
                    TeamData details = JsonConvert.DeserializeObject<TeamData>(entry.Metadata);
                    Debug.Log($"순위: {entry.Rank + 1} | 팀명: {details.teamName} | 점수: {entry.Score}");
                }
                else
                {
                    Debug.Log($"순위: {entry.Rank + 1} | 데이터 없음 | 점수: {entry.Score}");
                }
            }
        }
    }
}
