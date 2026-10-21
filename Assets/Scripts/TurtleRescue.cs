
using UnityEngine;

public class TurtleRescue : MonoBehaviour
{
    public int scorePoints = 10;
    public GameObject rescueEffect;

    [Header("Animal Rescue Sound")]
    public AudioSource animalRescueAudioSource;

    private bool rescued = false;

    private void Start()
    {
        Debug.Log("Turtle rescue ready: " + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (rescued || !other.CompareTag("Player"))
            return;

        rescued = true;

        // تشغيل صوت الإنقاذ
        if (animalRescueAudioSource != null &&
            animalRescueAudioSource.clip != null)
        {
            animalRescueAudioSource.PlayOneShot(
                animalRescueAudioSource.clip
            );
        }

        // إظهار تأثير الإنقاذ
        if (rescueEffect != null)
        {
            GameObject effect = Instantiate(
                rescueEffect,
                transform.position,
                Quaternion.identity
            );

            effect.SetActive(true);
            Debug.Log("Rescue effect spawned: " + effect.name);
        }
        else
        {
            Debug.LogWarning("Rescue Effect is NOT assigned!");
        }

        // إضافة النقاط
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scorePoints);
        }

        // تسجيل إنقاذ السلحفاة
        Level3WinManager winManager =
            FindObjectOfType<Level3WinManager>();

        if (winManager != null)
        {
            winManager.RegisterAnimalRescue();
        }
        else
        {
            Debug.LogWarning("Level3WinManager not found!");
        }

        Destroy(gameObject);
    }
}
