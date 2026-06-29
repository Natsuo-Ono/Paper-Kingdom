using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public bool isInCave;

    public Vector3 outsideOffset = new Vector3(0, 10, -7);
    public Vector3 caveOffset = new Vector3(-2, 5, -7);

    public Vector3 outsideRotation = new Vector3(45, 0, 0);
    public Vector3 caveRotation = new Vector3(35, 35, 0);

    public Vector3 offset = new Vector3(0f, 10f, -7f);

    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        if (isInCave)
        {
            offset = caveOffset;
            transform.rotation = Quaternion.Euler(caveRotation);
        }
        else
        {
            offset = outsideOffset;
            transform.rotation = Quaternion.Euler(outsideRotation);
        }
    }
}