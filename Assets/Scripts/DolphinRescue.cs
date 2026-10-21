
using UnityEngine;

public class DolphinRescue : MonoBehaviour
{
    [Header("Movement Before Rescue")]
    public float idleBobAmount = 0.12f;
    public float idleBobSpeed = 1.5f;
    public float idleTiltAmount = 4f;

    [Header("Swimming After Rescue")]
    public float swimSpeed = 2f;
    public float swimWaveAmount = 0.3f;
    public float swimWaveSpeed = 2f;
    public float swimTiltAmount = 8f;

    [Header("Rescue Score")]
    public int scorePoints = 20;
    public RescueScorePopup scorePopup;

    [Header("Score Popup Position")]
    public float popupHeightOffset = 0.2f;

    [Header("Animal Rescue Sound")]
    public AudioSource animalRescueAudioSource;

    private bool isRescued = false;
    private bool rescueRegistered = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 swimDirection;

    private float swimTime = 0f;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        swimDirection = transform.right;
    }

    private void Update()
    {
        if (!isRescued)
        {
            float bob = Mathf.Sin(Time.time * idleBobSpeed)
                        * idleBobAmount;

            float tilt = Mathf.Sin(Time.time * idleBobSpeed)
                         * idleTiltAmount;

            transform.position = startPosition
                + Vector3.up * bob;

            transform.rotation = startRotation
                * Quaternion.Euler(0f, 0f, tilt);
        }
        else
        {
            swimTime += Time.deltaTime;

            float wave = Mathf.Sin(swimTime * swimWaveSpeed)
                         * swimWaveAmount;

            transform.position += swimDirection
                * swimSpeed * Time.deltaTime;

            transform.position += Vector3.up
                * wave * Time.deltaTime;

            float tilt = Mathf.Sin(swimTime * swimWaveSpeed)
                         * swimTiltAmount;

            transform.rotation = startRotation
                * Quaternion.Euler(0f, 0f, tilt);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isRescued)
            return;

        if (!other.CompareTag("Player"))
            return;

        isRescued = true;
        swimTime = 0f;

        // تشغيل صوت الإنقاذ
        if (animalRescueAudioSource != null &&
            animalRescueAudioSource.clip != null)
        {
            animalRescueAudioSource.PlayOneShot(
                animalRescueAudioSource.clip
            );
        }

        // إضافة النقاط
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scorePoints);
        }

        // تسجيل إنقاذ الدلفين مرة واحدة
        if (!rescueRegistered)
        {
            Level3WinManager winManager =
                FindObjectOfType<Level3WinManager>();

            if (winManager != null)
            {
                winManager.RegisterAnimalRescue();
                rescueRegistered = true;
            }
            else
            {
                Debug.LogWarning(
                    "Level3WinManager not found!"
                );
            }
        }

        // إظهار +20 فوق رأس الدلفين
        if (scorePopup != null)
        {
            SpriteRenderer dolphinSprite =
                GetComponentInChildren<SpriteRenderer>();

            Vector3 popupPosition;

            if (dolphinSprite != null)
            {
                popupPosition = new Vector3(
                    dolphinSprite.bounds.center.x,
                    dolphinSprite.bounds.max.y
                        + popupHeightOffset,
                    transform.position.z
                );
            }
            else
            {
                popupPosition = transform.position
                    + Vector3.up * 0.8f;
            }

            scorePopup.ShowAt(popupPosition);
        }
    }
}
