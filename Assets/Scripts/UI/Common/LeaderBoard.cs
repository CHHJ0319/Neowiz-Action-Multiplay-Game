using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class LeaderBoard : MonoBehaviour
    {
        public GameObject leaderBoardEntryPrefab;
        public RectTransform content;

        public Button closeButton;

        private void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(() => SetVisible(false));
            }
        }
        private async void OnEnable()
        {
            try
            {
                RefreshRanking();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[LeaderBoard] 데이터 로드 중 오류 발생: {e.Message}");
            }
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        private async void RefreshRanking()
        {
            foreach (RectTransform child in content)
            {
                Destroy(child.gameObject);
            }

            List<LeaderboardEntry> rankings = await DataManager.Instance.FetchRankings();

            foreach (var ranking in rankings)
            {
                if (!string.IsNullOrEmpty(ranking.Metadata))
                {
                    GameObject entry = Instantiate(leaderBoardEntryPrefab, content);
                    UI.Common.LeaderBoardEntry leaderBoardEntry = entry.GetComponent<UI.Common.LeaderBoardEntry>();

                    var details = JsonConvert.DeserializeObject<Data.TeamData>(ranking.Metadata);
                    leaderBoardEntry.SetData(ranking.Rank + 1, details);
                }
            }
        }
    }
}