
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement =
                collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.TakeHit(transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement =
                other.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.TakeHit(transform.position);
            }
        }
    }
}
