
using UnityEngine;

public class Level3SharkKill : MonoBehaviour
{
    [Header("Game Over Settings")]
    public GameObject gameOverPanel;

    [Header("Shark Attack Sound")]
    public AudioSource sharkAttackAudioSource;

    private bool gameOver = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckPlayer(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckPlayer(other.gameObject);
    }

    private void CheckPlayer(GameObject other)
    {
        if (gameOver)
            return;

        if (other.CompareTag("Player"))
        {
            gameOver = true;

            // تشغيل صوت هجوم القرش
            if (sharkAttackAudioSource != null &&
                sharkAttackAudioSource.clip != null)
            {
                sharkAttackAudioSource.PlayOneShot(
                    sharkAttackAudioSource.clip
                );
            }

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning(
                    "Game Over Panel is not assigned!"
                );
            }

            // إيقاف اللعبة بعد بدء الصوت
            Time.timeScale = 0f;
        }
    }
}
