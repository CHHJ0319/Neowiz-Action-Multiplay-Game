using UnityEngine;

public class ProloguePanel : MonoBehaviour
{
    public RectTransform prologueText;
    public float scrollSpeed = 30f;

    private void Update()
    {
        if (prologueText != null)
        {
            prologueText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }
}