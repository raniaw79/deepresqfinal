
using UnityEngine;
using TMPro;

public class OxygenSystem : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float oxygen = 100f;
    public float oxygenDrainRate = 1f;

    public int oxygenTanks = 0;
    public int maxOxygenTanks = 4;

    public TMP_Text oxygenText;
    public GameObject gameOverPanel;
    public AudioSource gameOverAudio;

    // بانيل خاص بنفاد الأكسجين، اختياري
    public GameObject oxygenGameOverPanel;

    public GameObject[] hudElements;

    private bool gameOver = false;

    private void Start()
    {
        oxygen = maxOxygen;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (oxygenGameOverPanel != null)
            oxygenGameOverPanel.SetActive(false);

        SetHUDVisible(true);
        UpdateOxygenText();
    }

    private void Update()
    {
        if (gameOver)
            return;

        oxygen -= oxygenDrainRate * Time.deltaTime;
        oxygen = Mathf.Clamp(oxygen, 0f, maxOxygen);

        if (oxygen <= 0f)
        {
            if (oxygenTanks > 0)
            {
                oxygenTanks--;
                oxygen = maxOxygen;
            }
            else
            {
                UpdateOxygenText();
                OxygenGameOver();
                return;
            }
        }

        UpdateOxygenText();
    }

    private void UpdateOxygenText()
    {
        if (oxygenText != null)
            oxygenText.text = Mathf.RoundToInt(oxygen).ToString();
    }

    public void DamageOxygen(float amount)
    {
        if (gameOver)
            return;

        oxygen = Mathf.Max(0f, oxygen - amount);
        UpdateOxygenText();

        Debug.Log("Damage applied: " + amount +
                  " | Remaining: " + oxygen);

        if (oxygen <= 0f)
        {
            if (oxygenTanks > 0)
            {
                oxygenTanks--;
                oxygen = maxOxygen;
                UpdateOxygenText();
            }
            else
            {
                OxygenGameOver();
            }
        }
    }

    public void AddOxygenTank()
    {
        if (!gameOver && oxygenTanks < maxOxygenTanks)
            oxygenTanks++;
    }

    public void SharkAttack()
    {
        GameOver();
    }

    // Game Over العادي
    public void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        SetHUDVisible(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        PlayGameOverSound();
        Time.timeScale = 0f;
    }

    // Game Over بسبب نفاد الأكسجين
    private void OxygenGameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        SetHUDVisible(false);

        if (oxygenGameOverPanel != null)
        {
            oxygenGameOverPanel.SetActive(true);
        }
        else if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        PlayGameOverSound();
        Time.timeScale = 0f;
    }

    private void PlayGameOverSound()
    {
        if (gameOverAudio != null && !gameOverAudio.isPlaying)
            gameOverAudio.Play();
    }

    private void SetHUDVisible(bool visible)
    {
        if (hudElements == null)
            return;

        foreach (GameObject element in hudElements)
        {
            if (element != null)
                element.SetActive(visible);
        }
    }
}
