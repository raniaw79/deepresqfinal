
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level3AwarenessMessage : MonoBehaviour
{
    public Image awarenessImage;
    public PlayerMovement playerMovement;

    public float fadeDuration = 1f;
    public float displayDuration = 10f;

    private void Start()
    {
        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        playerMovement.enabled = false;
        awarenessImage.gameObject.SetActive(true);

        Color color = awarenessImage.color;
        color.a = 0;
        awarenessImage.color = color;

        yield return Fade(0, 1);

        yield return new WaitForSecondsRealtime(displayDuration);

        yield return Fade(1, 0);

        awarenessImage.gameObject.SetActive(false);
        playerMovement.enabled = true;
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            Color color = awarenessImage.color;
            color.a = Mathf.Lerp(start, end, elapsed / fadeDuration);
            awarenessImage.color = color;

            yield return null;
        }

        Color finalColor = awarenessImage.color;
        finalColor.a = end;
        awarenessImage.color = finalColor;
    }
}
