using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    [Header("Encounter")]
    public EncounterArea area;

    [Range(0, 100)]
    public float encounterChance = 10f;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.35f);

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        // Flat box (X-Z plane)
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 0.05f, 1f));

        Gizmos.matrix = oldMatrix;
    }
}