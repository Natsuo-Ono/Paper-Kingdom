using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public bool isCave;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        FindObjectOfType<CameraFollow>().isInCave = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        FindObjectOfType<CameraFollow>().isInCave = false;
    }
}