using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    public FloatingText prefab;
    public Canvas canvas;

    void Awake()
    {
        Instance = this;
    }

    public void Spawn(RectTransform target, int amount, Color color, bool heal)
    {
        FloatingText text =
            Instantiate(prefab, canvas.transform);

        text.GetComponent<RectTransform>().anchoredPosition =
            target.anchoredPosition;

        text.Setup(amount, color, heal);
    }

    public void SpawnMessage(RectTransform target, string message, Color color)
    {
        FloatingText text =
            Instantiate(prefab, canvas.transform);

        text.GetComponent<RectTransform>().anchoredPosition =
            target.anchoredPosition + new Vector2(0, 35);

        text.SetupMessage(message, color);
    }
}