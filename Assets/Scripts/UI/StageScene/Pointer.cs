using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class Pointer : MonoBehaviour
    {
        private Image pointerImage;

        private void Awake()
        {
            pointerImage = GetComponent<Image>();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetColor(Color color)
        {
            pointerImage.color = color;
        }
    }
}
