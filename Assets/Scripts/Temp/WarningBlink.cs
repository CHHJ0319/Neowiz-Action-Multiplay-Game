using UnityEngine;
using UnityEngine.UI;

public class WarningBlink : MonoBehaviour
{
    [SerializeField] private Graphic targetGraphic;
    [SerializeField] private float blinkSpeed = 4f;
    [SerializeField] private float minAlpha = 0.35f;
    [SerializeField] private float maxAlpha = 1f;

    private void Reset()
    {
        targetGraphic = GetComponent<Graphic>();
    }

    private void Update()
    {
        if (targetGraphic == null) return;

        Color color = targetGraphic.color;
        float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        color.a = Mathf.Lerp(minAlpha, maxAlpha, t);
        targetGraphic.color = color;
    }
}