using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Normal Camera")]
    public Vector3 normalOffset = new Vector3(0f, 8f, -8f);
    public Vector3 normalRotation = new Vector3(45f, 0f, 0f);

    [Header("Follow")]
    public float followSpeed = 8f;
    public float rotationSpeed = 8f;

    private List<CameraZone> activeZones = new List<CameraZone>();

    private CameraZone currentZone;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredOffset = normalOffset;
        Vector3 desiredRotation = normalRotation;

        if (currentZone != null)
        {
            desiredOffset = currentZone.cameraOffset;
            desiredRotation = currentZone.cameraRotation;
        }

        Vector3 targetPosition =
            target.position + desiredOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        Quaternion targetQuaternion =
            Quaternion.Euler(desiredRotation);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetQuaternion,
            rotationSpeed * Time.deltaTime
        );
    }

    public void EnterZone(CameraZone zone)
    {
        if (!activeZones.Contains(zone))
        {
            activeZones.Add(zone);
        }

        UpdateHighestPriorityZone();
    }

    public void ExitZone(CameraZone zone)
    {
        if (activeZones.Contains(zone))
        {
            activeZones.Remove(zone);
        }

        UpdateHighestPriorityZone();
    }

    private void UpdateHighestPriorityZone()
    {
        currentZone = null;

        foreach (CameraZone zone in activeZones)
        {
            if (currentZone == null)
            {
                currentZone = zone;
                continue;
            }

            if (zone.priority > currentZone.priority)
            {
                currentZone = zone;
            }
        }
    }
}