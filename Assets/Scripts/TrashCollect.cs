
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashCollect : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (collected)
            return;

        collected = true;

        // Add score once.
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
        }

        // Level 1 only: play the pickup sound from the Player.
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            AudioSource playerAudio =
                other.GetComponent<AudioSource>();

            if (playerAudio == null)
            {
                playerAudio =
                    other.GetComponentInParent<AudioSource>();
            }

            if (playerAudio != null && playerAudio.clip != null)
            {
                playerAudio.PlayOneShot(playerAudio.clip);
            }
            else
            {
                Debug.LogWarning(
                    "Level1: Player AudioSource or Pickup AudioClip is missing!"
                );
            }
        }
        else
        {
            // Preserve the existing sound system in other levels.
            TrashCollectSound collectSound =
                FindFirstObjectByType<TrashCollectSound>();

            if (collectSound != null)
            {
                collectSound.PlayCollectSound();
            }
        }

        // Move the trash back to the top.
        Vector3 newPosition = transform.position;
        newPosition.y = 6f;
        transform.position = newPosition;

        collected = false;
    }
}
