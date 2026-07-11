using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TMP_Text text;
    public CanvasGroup canvasGroup;

    public float moveSpeed = 60f;
    public float lifeTime = 1.5f;

    Vector3 startPos;
    Vector3 endPos;

    float duration = 0.7f;
    float timer;

    float curveHeight;
    float randomX;

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / duration;

        Vector3 pos = Vector3.Lerp(startPos, endPos, t);

        // upside-down U
        pos.y += 4f * curveHeight * t * (1f - t);

        transform.localPosition = pos;

        Color c = text.color;
        c.a = 1f - t;
        text.color = c;

        if (t >= 1f)
            Destroy(gameObject);
    }

    // Damage / Heal
    public void Setup(int amount, Color color, bool heal)
    {
        text.text = heal ? "+" + amount : "-" + amount;
        text.color = color;

        StartAnimation();
    }

    // Message (Effective!, Resisted!, etc.)
    public void SetupMessage(string message, Color color)
    {
        text.text = message;
        text.color = color;

        StartAnimation();
    }

    void StartAnimation()
    {
        startPos = transform.localPosition;

        randomX = Random.Range(-70f, 70f);
        curveHeight = Random.Range(80f, 130f);

        endPos = startPos + new Vector3(randomX, 0, 0);

        timer = 0f;
    }
}