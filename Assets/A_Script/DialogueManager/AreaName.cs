using UnityEngine;

public class AreaName : MonoBehaviour
{
    public string areaName;
    public string subtitleAreaName;
    public string areaLocalName;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        AreaNameUI.Instance.ShowArea(
            areaName,
            subtitleAreaName,
            areaLocalName
        );
    }
}