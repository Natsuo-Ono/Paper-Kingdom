using UnityEngine;

public class CameraZone : MonoBehaviour
{
    [Header("Camera Position")]
    public Vector3 cameraOffset = new Vector3(0f, 8f, -8f);

    [Header("Camera Rotation")]
    public Vector3 cameraRotation = new Vector3(45f, 0f, 0f);

    [Header("Transition")]
    public float transitionSpeed = 3f;

    [Header("Priority")]
    public int priority = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CameraRig cameraRig = FindFirstObjectByType<CameraRig>();

        if (cameraRig != null)
        {
            cameraRig.EnterZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CameraRig cameraRig = FindFirstObjectByType<CameraRig>();

        if (cameraRig != null)
        {
            cameraRig.ExitZone(this);
        }
    }
}