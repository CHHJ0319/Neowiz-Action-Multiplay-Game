using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene 
{
    public class UIController : MonoBehaviour
    {
        public Image barricadeHPBar;
        public GameObject resultPanel;

        public Button quitButton;

        private void Awake()
        {
            if (quitButton != null)
            {
                quitButton.onClick.AddListener(Events.GameEvents.QuitGame);
            }
        }

        private void OnEnable()
        {
            Events.PlayerFieldEvents.OnHPChanged += UpdateBarricadeHPBar;

            Events.RoundEvents.OnRoundEnded += ShowResultPanel;
        }

        private void OnDisable()
        {
            Events.PlayerFieldEvents.OnHPChanged -= UpdateBarricadeHPBar;

            Events.RoundEvents.OnRoundEnded -= ShowResultPanel;
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
    }
}
