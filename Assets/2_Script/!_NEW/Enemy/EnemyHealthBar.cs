using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    public Image healthFill;
    public CanvasGroup canvasGroup;

    [Header("Display")]
    public float visibleDuration = 30f;
    public float fadeDuration = 0.5f;

    private EnemyHealth enemyHealth;
    private Camera mainCamera;

    private float hideTimer;
    private bool isVisible;
    private bool isFading;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        mainCamera = Camera.main;

        HideImmediately();
    }

    private void Update()
    {
        // Face the camera like 2D UI
        if (mainCamera != null)
        {
            transform.rotation = mainCamera.transform.rotation;
        }

        UpdateHealthBar();

        if (!isVisible)
            return;

        // Count down after the last hit
        hideTimer -= Time.deltaTime;

        if (hideTimer <= 0f && !isFading)
        {
            StartCoroutine(FadeOut());
        }
    }

    public void ShowHealthBar()
    {
        StopAllCoroutines();

        isVisible = true;
        isFading = false;

        hideTimer = visibleDuration;

        canvasGroup.alpha = 1f;
    }

    private void UpdateHealthBar()
    {
        if (enemyHealth == null || healthFill == null)
            return;

        healthFill.fillAmount =
            (float)enemyHealth.CurrentHealth /
            enemyHealth.MaxHealth;
    }

    private void HideImmediately()
    {
        isVisible = false;
        isFading = false;

        canvasGroup.alpha = 0f;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        isFading = true;

        float startAlpha = canvasGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / fadeDuration;

            canvasGroup.alpha =
                Mathf.Lerp(startAlpha, 0f, progress);

            yield return null;
        }

        canvasGroup.alpha = 0f;

        isVisible = false;
        isFading = false;
    }
}