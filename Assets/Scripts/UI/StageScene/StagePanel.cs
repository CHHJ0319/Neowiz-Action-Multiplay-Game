using TMPro;
using UnityEngine;

namespace UI.StageScene
{
    public class StagePanel : MonoBehaviour
    {
        public TextMeshProUGUI stageNameText;
        public TextMeshProUGUI waveText;

        public void Start()
        {
            string stageName = Utils.SceneNavigator.GetCurrentSceneName();
            SetStageNameText(stageName.Replace("Scene", ""));
            SetWaveText(1);
        }

        private void SetStageNameText(string name)
        {
            stageNameText.text = name;
        }

        public void SetWaveText(int wave)
        {
            waveText.text = "Wave " + wave;
        }
    }
}