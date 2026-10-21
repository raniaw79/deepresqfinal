
using UnityEngine;

public class TreasureAnimation : MonoBehaviour
{
    [Header("Floating Movement")]
    public float floatHeight = 0.18f;
    public float floatSpeed = 1.5f;

    [Header("Gentle Rotation")]
    public float rotationAngle = 8f;
    public float rotationSpeed = 1.2f;

    [Header("Treasure Glow Pulse")]
    public float pulseAmount = 0.08f;
    public float pulseSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 startScale;
    private Quaternion startRotation;

    private float randomOffset;

    private void OnEnable()
    {
        startPosition = transform.position;
        startScale = transform.localScale;
        startRotation = transform.localRotation;

        randomOffset = Random.Range(0f, 6.28f);
    }

    private void Update()
    {
        // Float gently up and down like underwater treasure
        float floatOffset =
            Mathf.Sin(Time.time * floatSpeed + randomOffset)
            * floatHeight;

        transform.position = startPosition
            + Vector3.up * floatOffset;

        // Rotate gently from side to side
        float angle =
            Mathf.Sin(Time.time * rotationSpeed + randomOffset)
            * rotationAngle;

        transform.localRotation =
            startRotation * Quaternion.Euler(0f, 0f, angle);

        // Make the treasure pulse softly
        float pulse =
            1f + Mathf.Sin(Time.time * pulseSpeed + randomOffset)
            * pulseAmount;

        transform.localScale = startScale * pulse;
    }
}
