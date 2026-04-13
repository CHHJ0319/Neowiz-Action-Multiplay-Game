using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class TimerPanel : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public Image timerBar;

        public void UpdateTimerPanel(string time, float timeRate)
        {
            UpdateTimerText(time);
            UpdateTimerBar(timeRate);
        }

        private void UpdateTimerText(string time)
        {
            if (timerText == null) return;

            timerText.text = time;
        }

        private void UpdateTimerBar(float timeRate)
        {
            if (timerBar == null) return;

            float currentHPRate = timeRate;
            currentHPRate = Mathf.Clamp(timeRate, 0f, 1f);

            timerBar.fillAmount = currentHPRate;
        }
    }
}