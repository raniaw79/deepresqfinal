
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
    }

    public void AddTrash()
    {
        if (levelCompleted)
        {
            return;
        }

        trashCollected++;

        UpdateCounter();

        // كل 10 قطع قمامة = 10 نقاط
        if (trashCollected % 10 == 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(10);
            }
        }

        // زيادة سرعة القمامة
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

        // صوت الفوز مسؤول عنه ScoreManager فقط

        // إيقاف حركة اللعبة
        Time.timeScale = 0f;

        Debug.Log("LEVEL 1 COMPLETED!");
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
