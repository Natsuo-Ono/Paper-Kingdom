using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;
    public CanvasGroup canvasGroup;

    public float moveSpeed = 1.5f;
    public float lifetime = 0.8f;

    private float timer;
    private Camera mainCamera;

    public void Setup(
        int damage,
        DamageType damageType,
        ElementType elementType
    )
    {
        damageText.text = damage.ToString();

        SetDamageColor(
            damageType,
            elementType
        );
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Float upward
        transform.position +=
            Vector3.up *
            moveSpeed *
            Time.deltaTime;

        // Face camera
        if (mainCamera != null)
        {
            transform.rotation =
                mainCamera.transform.rotation;
        }

        // Fade out
        float fade =
            1f - (timer / lifetime);

        canvasGroup.alpha = fade;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void SetDamageColor(
        DamageType damageType,
        ElementType elementType
    )
    {
        // Physical
        if (damageType == DamageType.Physical)
        {
            SetSolidColor(Color.white);
            return;
        }

        // Elemental
        switch (elementType)
        {
            case ElementType.Pyro:

                SetSolidColor(
                    new Color(
                        1f,
                        0.25f,
                        0.05f
                    )
                );

                break;


            case ElementType.Hydro:

                SetSolidColor(
                    new Color(
                        0.35f,
                        0.75f,
                        1f
                    )
                );

                break;


            case ElementType.Flora:

                SetSolidColor(
                    new Color(
                        0.45f,
                        1f,
                        0.45f
                    )
                );

                break;


            case ElementType.Lumen:

                SetGradient(
                    new Color(
                        1f,
                        0.75f,
                        0.05f
                    ),
                    new Color(
                        1f,
                        1f,
                        0.65f
                    )
                );

                break;


            case ElementType.Umbra:

                SetGradient(
                    new Color(
                        0.25f,
                        0.05f,
                        0.35f
                    ),
                    new Color(
                        0.75f,
                        0.35f,
                        1f
                    )
                );

                break;


            default:

                SetSolidColor(Color.white);

                break;
        }
    }

    private void SetSolidColor(Color color)
    {
        damageText.enableVertexGradient = false;
        damageText.color = color;
    }

    private void SetGradient(
        Color topColor,
        Color bottomColor
    )
    {
        damageText.enableVertexGradient = true;

        damageText.colorGradient =
            new VertexGradient(
                topColor,
                topColor,
                bottomColor,
                bottomColor
            );
    }
}