using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class TimerPanel : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public Image timerBar;

        public void UpdateTimerPanel(float time, float timeRate)
        {
            UpdateTimerText(time);
            UpdateTimerBar(timeRate);
        }

        private void UpdateTimerText(float time)
        {
            if (timerText == null) return;

            if (time < 0) time = 0;

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);

            timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
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