using System.Collections;
using TMPro;
using UnityEngine;

public class AreaNameUI : MonoBehaviour
{
    public static AreaNameUI Instance;

    public TMP_Text areaText;
    public TMP_Text areaLocalText;
    public TMP_Text subtitleText;

    public CanvasGroup canvasGroup;

    public float fadeDuration = 1f;
    public float stayDuration = 2f;

    Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;

        canvasGroup.alpha = 0;
    }

    public void ShowArea(string area, string subtitle, string local)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(
            ShowRoutine(area, subtitle, local));
    }

    IEnumerator ShowRoutine(string area, string subtitle, string local)
    {
        areaText.text = area;
        subtitleText.text = subtitle;
        areaLocalText.text = area;

        // Fade In
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(stayDuration);

        // Fade Out
        t = fadeDuration;

        while (t > 0)
        {
            t -= Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}