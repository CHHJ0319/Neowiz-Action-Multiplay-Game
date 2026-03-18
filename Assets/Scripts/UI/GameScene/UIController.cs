using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene 
{
    public class UIController : MonoBehaviour
    {
        public Image barricadeHPBar;
        public GameObject resultPanel;
        
        public Button roundStartButton;

        private void Awake()
        {
            if (roundStartButton != null)
            {
                roundStartButton.onClick.AddListener(OnRoundStartButtonClicked);
            }
        }

        private void OnEnable()
        {
            Events.PlayerFieldEvents.OnHPChanged += UpdateBarricadeHPBar;

            Events.RoundEvents.OnRoundEnded += ShowResultPanel;
            Events.RoundEvents.OnRoundEnded += ShowRoundStartButton;
        }

        private void OnDisable()
        {
            Events.PlayerFieldEvents.OnHPChanged -= UpdateBarricadeHPBar;

            Events.RoundEvents.OnRoundEnded -= ShowResultPanel;
            Events.RoundEvents.OnRoundEnded -= ShowRoundStartButton;
        }

        private void UpdateBarricadeHPBar(float hpRate)
        {
            float currentHPRate = hpRate;
            currentHPRate = Mathf.Clamp(hpRate, 0f, 1f);

            barricadeHPBar.fillAmount = currentHPRate;
        }

        private void ShowResultPanel()
        {
            resultPanel.SetActive(true);
        }

        private void OnRoundStartButtonClicked()
        {
            Events.RoundEvents.StartRound();
            roundStartButton.gameObject.SetActive(false);
        }

        private void ShowRoundStartButton()
        {
            roundStartButton.gameObject.SetActive(true);
        }
    }
}
