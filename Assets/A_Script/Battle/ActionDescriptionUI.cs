using TMPro;
using UnityEngine;

public class ActionDescriptionUI : MonoBehaviour
{
    public static ActionDescriptionUI Instance;

    public TMP_Text titleText;
    public TMP_Text descText;

    private bool isLocked = false;

    void Awake()
    {
        Instance = this;
    }

    public void Preview(string title, string desc)
    {
        if (isLocked)
            return;

        titleText.text = title;
        descText.text = desc;
    }

    public void Lock(string title, string desc)
    {
        isLocked = true;

        titleText.text = title;
        descText.text = desc;
    }

    public void HidePreview()
    {
        if (isLocked)
            return;

        titleText.text = "";
        descText.text = "";
    }

    public void Unlock()
    {
        isLocked = false;

        titleText.text = "";
        descText.text = "";
    }
}