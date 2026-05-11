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
            try
            {
                await InitializeUnityServicesAsync();

                var options = new GetScoresOptions
                {
                    Limit = 10,
                    IncludeMetadata = true
                };

                var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, options);

                Debug.Log($"[UGS] 총 {scoresResponse.Results.Count}개의 순위를 불러왔습니다.");

                foreach (var entry in scoresResponse.Results)
                {
                    if (!string.IsNullOrEmpty(entry.Metadata))
                    {
                        Data.TeamData details = JsonConvert.DeserializeObject<Data.TeamData>(entry.Metadata);

                        Debug.Log($"순위: {entry.Rank + 1} | 팀명: {details.teamName} | 라운드: {details.finalRound} | 점수: {details.totalScore}");
                    }
                    else
                    {
                        Debug.LogWarning($"순위: {entry.Rank + 1} | 메타데이터가 여전히 null입니다. (저장 시점 확인 필요)");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[UGS] 순위 불러오기 실패: {e.Message}");
            }
        }
    }
}
