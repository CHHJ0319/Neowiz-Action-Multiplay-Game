using UnityEngine;

public class WarningTextScroll : MonoBehaviour
{
    [SerializeField] private RectTransform targetText;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float startX = -800f;
    [SerializeField] private float endX = 800f;

    private void Reset()
    {
        targetText = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (targetText != null)
        {
            Vector2 pos = targetText.anchoredPosition;
            pos.x = startX;
            targetText.anchoredPosition = pos;
        }
    }

    private void Update()
    {
        if (targetText == null) return;

        Vector2 pos = targetText.anchoredPosition;
        pos.x += speed * Time.deltaTime;

        if (pos.x > endX)
        {
            pos.x = startX;
        }

        targetText.anchoredPosition = pos;
    }
}