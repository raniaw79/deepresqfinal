
using UnityEngine;
using System.Collections;

public class TreasureCollect : MonoBehaviour
{
    [Header("Treasure Settings")]
    public int points = 10;

    [Header("Collect Sound")]
    public AudioSource treasureAudioSource;

    [Header("Collect Effect")]
    public float effectDuration = 0.4f;
    public float effectScale = 1.8f;

    [Header("Star Effect")]
    public TreasureCollectEffect starEffect;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }

        collected = true;

        // تشغيل صوت جمع الكنز
        if (treasureAudioSource != null &&
            treasureAudioSource.clip != null)
        {
            treasureAudioSource.PlayOneShot(
                treasureAudioSource.clip
            );
        }

        // إضافة نقاط الكنز
        ScoreManager.Instance.AddScore(points);

        // تسجيل جمع الكنز في نظام الفوز
        Level3WinManager winManager =
            FindObjectOfType<Level3WinManager>();

        if (winManager != null)
        {
            winManager.RegisterTreasureCollected();
        }
        else
        {
            Debug.LogWarning("Level3WinManager not found!");
        }

        // تشغيل تأثير النجوم
        if (starEffect != null)
        {
            starEffect.PlayEffect(transform.position);
        }
        else
        {
            Debug.LogWarning("Star Effect is not assigned!");
        }

        Debug.Log("Treasure collected! +" + points);

        StartCoroutine(CollectAnimation());
    }

    private IEnumerator CollectAnimation()
    {
        Vector3 originalScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < effectDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsed / effectDuration
            );

            transform.localScale = Vector3.Lerp(
                originalScale,
                originalScale * effectScale,
                progress
            );

            yield return null;
        }

        Destroy(gameObject);
    }
}
