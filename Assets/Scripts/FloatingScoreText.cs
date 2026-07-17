using UnityEngine;

public class FloatingScoreText : MonoBehaviour
{
    [SerializeField] private float riseDistance = 1f;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private TextMesh label;

    private Vector3 startPosition;
    private float elapsed;
    private Color startColor;

    private void Awake()
    {
        startPosition = transform.position;
        if (label != null) startColor = label.color;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);
        transform.position = startPosition + Vector3.up * riseDistance * t;
        if (label != null)
        {
            Color fadedColor = startColor;
            fadedColor.a = 1f - t;
            label.color = fadedColor;
        }
        if (t >= 1f) Destroy(gameObject);
    }

    public void SetText(string text)
    {
        if (label != null) label.text = text;
    }
}