
using System.Collections;
using UnityEngine;

public class TreasureCollectEffect : MonoBehaviour
{
    public float duration = 0.7f;
    public float maxScale = 1.5f;

    private SpriteRenderer spriteRenderer;
    private Vector3 initialScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
        gameObject.SetActive(false);
    }

    public void PlayEffect(Vector3 position)
    {
        transform.position = position;
        transform.localScale = initialScale;
        gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            transform.localScale =
                initialScale * Mathf.Lerp(0.3f, maxScale, progress);

            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1f - progress;
                spriteRenderer.color = color;
            }

            yield return null;
        }

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        gameObject.SetActive(false);
    }
}
