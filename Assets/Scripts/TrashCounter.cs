
using UnityEngine;
using TMPro;

public class TrashCounter : MonoBehaviour
{
    public static TrashCounter Instance;

    [Header("UI")]
    public TMP_Text counterText;
    public GameObject levelCompletePanel;

    [Header("Trash Target")]
    public int trashTarget = 50;

    [Header("Level Complete Sound")]
    public AudioSource levelCompleteSound;

    private int trashCollected = 0;
    private bool levelCompleted = false;

    private void Awake()
    {
        Instance = this;

        // البحث عن LevelCompletePanel تلقائيًا
        if (levelCompletePanel == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();

            if (canvas != null)
            {
                Transform panel = FindChildRecursive(
                    canvas.transform,
                    "LevelCompletePanel"
                );

                if (panel != null)
                {
                    levelCompletePanel = panel.gameObject;
                }
            }
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        UpdateCounter();

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (levelCompleteSound == null)
        {
            Debug.LogWarning(
                "TrashCounter: AudioSource is not assigned!"
            );
        }
        else if (levelCompleteSound.clip == null)
        {
            Debug.LogWarning(
                "TrashCounter: AudioClip is missing!"
            );
        }
    }

    public void AddTrash()
    {
        if (levelCompleted)
        {
            return;
        }

        trashCollected++;

        UpdateCounter();

        // كل 10 زبالة = 10 نقاط
        if (trashCollected % 10 == 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(10);
            }
        }

        // زيادة سرعة الزبالة
        FallingTrash[] allTrash =
            FindObjectsByType<FallingTrash>(
                FindObjectsSortMode.None
            );

        foreach (FallingTrash trash in allTrash)
        {
            trash.IncreaseSpeed(0.1f);
        }

        // إنهاء المستوى عند جمع الهدف
        if (trashCollected >= trashTarget)
        {
            levelCompleted = true;
            ShowLevelComplete();
        }
    }

    private void ShowLevelComplete()
    {
        // إظهار لوحة إكمال المستوى
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }
        else
        {
            Debug.LogError(
                "TrashCounter: LevelCompletePanel not found!"
            );
        }

        // تشغيل صوت النجاح مرة واحدة
        if (levelCompleteSound != null &&
            levelCompleteSound.clip != null)
        {
            levelCompleteSound.Stop();

            levelCompleteSound.PlayOneShot(
                levelCompleteSound.clip
            );

            Debug.Log(
                "Level 2 success sound requested: " +
                levelCompleteSound.clip.name
            );
        }
        else
        {
            Debug.LogError(
                "TrashCounter: AudioSource or AudioClip is missing!"
            );
        }

        // إيقاف حركة اللعبة
        Time.timeScale = 0f;

        Debug.Log("LEVEL 2 COMPLETED!");
    }

    private void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = ": " + trashCollected;
        }
    }

    private Transform FindChildRecursive(
        Transform parent,
        string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform result = FindChildRecursive(
                child,
                childName
            );

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
