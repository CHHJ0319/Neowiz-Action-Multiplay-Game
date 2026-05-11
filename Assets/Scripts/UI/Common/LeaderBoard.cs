using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class LeaderBoard : MonoBehaviour
    {
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
                await UGSService.FetchTopRankingsAsync();

                Debug.Log("[LeaderBoard] 랭킹 업데이트 완료.");
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
    }
}