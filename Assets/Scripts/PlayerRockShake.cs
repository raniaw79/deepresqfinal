
using System.Collections;
using UnityEngine;

public class PlayerRockShake : MonoBehaviour
{
    public Transform sprite;
    public float shakeDuration = 0.15f;
    public float shakeAmount = 0.035f;

    private Vector3 originalPosition;
    private Coroutine shakeRoutine;

    private void Start()
    {
        if (sprite != null)
            originalPosition = sprite.localPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
            ShakePlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rock"))
            ShakePlayer();
    }

    private void ShakePlayer()
    {
        if (sprite == null)
            return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            sprite.localPosition = originalPosition +
                (Vector3)Random.insideUnitCircle * shakeAmount;

            elapsed += Time.deltaTime;
            yield return null;
        }

        sprite.localPosition = originalPosition;
        shakeRoutine = null;
    }
}
