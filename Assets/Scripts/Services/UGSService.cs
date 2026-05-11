using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards.Models;
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

        public static async Task<List<LeaderboardEntry>> GetRankingsAsync()
        {
            try
            {
                await InitializeUnityServicesAsync();

                var options = new GetScoresOptions
                {
                    Limit = 8,
                    IncludeMetadata = true
                };

                var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, options);

                return scoresResponse.Results;
            }
            catch (Exception e)
            {
                Debug.LogError($"[UGS] 순위 불러오기 실패: {e.Message}");
                return new List<LeaderboardEntry>();
            }
        }
    }
}
