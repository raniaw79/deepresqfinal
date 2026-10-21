
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level3WinManager : MonoBehaviour
{
    [Header("Victory Panel")]
    public GameObject gameWinPanel;
    public TMP_Text finalScoreText;

    [Header("Win Requirements")]
    public int totalAnimalsToRescue = 8;
    public int treasuresRequired = 3;

    private int rescuedAnimals = 0;
    private int collectedTreasures = 0;
    private bool gameWon = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("GameWinPanel is not assigned!");
        }

        Debug.Log("Level3WinManager is ready.");
    }

    public void RegisterAnimalRescue()
    {
        if (gameWon)
            return;

        rescuedAnimals++;

        Debug.Log("Animals rescued: " + rescuedAnimals
            + " / " + totalAnimalsToRescue);

        CheckWinCondition();
    }

    public void RegisterTreasureCollected()
    {
        if (gameWon)
            return;

        collectedTreasures++;

        Debug.Log("Treasures collected: " + collectedTreasures
            + " / " + treasuresRequired);

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        bool allAnimalsRescued =
            totalAnimalsToRescue > 0 &&
            rescuedAnimals >= totalAnimalsToRescue;

        bool enoughTreasuresCollected =
            collectedTreasures >= treasuresRequired;

        if (allAnimalsRescued && enoughTreasuresCollected)
        {
            ShowWinPanel();
        }
    }

    private void ShowWinPanel()
    {
        if (gameWon)
            return;

        gameWon = true;

        if (gameWinPanel != null)
        {
            int finalScore = 0;

            if (ScoreManager.Instance != null)
            {
                finalScore = ScoreManager.Instance.CurrentScore;
            }
            else
            {
                Debug.LogWarning(
                    "ScoreManager.Instance not found!");
            }

            if (finalScoreText != null)
            {
                finalScoreText.text = finalScore.ToString();
            }
            else
            {
                Debug.LogWarning(
                    "FinalScoreText is not assigned!");
            }

            gameWinPanel.SetActive(true);
            Time.timeScale = 0f;

            Debug.Log(
                "MISSION COMPLETE! Final Score: " + finalScore);
        }
    }

    // Return to the Home scene.
    public void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }
}
