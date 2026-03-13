using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene 
{
    public class UIController : MonoBehaviour
    {
        public Image barricadeHPBar;

        private void OnEnable()
        {
            Events.PlayerFieldEvents.OnHPChanged += UpdateBarricadeHPBar;
        }

        private void OnDisable()
        {
            Events.PlayerFieldEvents.OnHPChanged -= UpdateBarricadeHPBar;
        }

        private void UpdateBarricadeHPBar(float hpRate)
        {
            float currentHPRate = hpRate;
            currentHPRate = Mathf.Clamp(hpRate, 0f, 1f);

            barricadeHPBar.fillAmount = currentHPRate;
        }
    }
}
