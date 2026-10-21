
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score UI")]
    public TMP_Text scoreText;

    [Header("Level 1 - Complete")]
    public GameObject levelCompletePanel;
    public int levelCompleteScore = 50;
    public Button level1ContinueButton;
    public string level2Name = "Level2";

    [Header("Level 1 - Complete Sound")]
    public AudioSource levelCompleteSound;

    [Header("Level 2 - Mission Complete")]
    public GameObject difficultyPanel;
    public Button continueButton;
    public int difficultyScore = 50;

    [Header("Level 2 - Mission Complete Sound")]
    public AudioSource missionCompleteSound;

    [Header("Next Level")]
    public string nextLevelName = "Level3";

    private int score = 0;
    public int CurrentScore => score;

    private bool levelCompleted = false;
    private bool difficultyTriggered = false;

    private void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;

        if (levelCompletePanel == null)
        {
            GameObject found =
                GameObject.Find("LevelCompletePanel");

            if (found != null)
                levelCompletePanel = found;
        }

        if (difficultyPanel == null)
        {
            GameObject found =
                GameObject.Find("MissionCompletePanel");

            if (found != null)
                difficultyPanel = found;
        }

        Debug.Log("ScoreManager Awake | Scene: " +
                  SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);

        if (level1ContinueButton != null)
        {
            level1ContinueButton.onClick.RemoveListener(
                ContinueFromLevel1);

            level1ContinueButton.onClick.AddListener(
                ContinueFromLevel1);
        }

        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(
                ContinueAfterDifficulty);

            continueButton.onClick.AddListener(
                ContinueAfterDifficulty);
        }

        if (levelCompleteSound != null)
        {
            levelCompleteSound.playOnAwake = false;
            levelCompleteSound.loop = false;
            levelCompleteSound.mute = false;
        }

        if (missionCompleteSound != null)
        {
            missionCompleteSound.playOnAwake = false;
            missionCompleteSound.loop = false;
            missionCompleteSound.mute = false;
        }

        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        string sceneName =
            SceneManager.GetActiveScene().name;

        if (sceneName == "Level1" && levelCompleted)
            return;

        if (sceneName == "Level2" && difficultyTriggered)
            return;

        score += points;
        UpdateScoreText();

        Debug.Log("Current Score: " + score);

        if (sceneName == "Level1" &&
            score >= levelCompleteScore)
        {
            levelCompleted = true;
            ShowLevelCompletePanel();
        }
        else if (sceneName == "Level2" &&
                 score >= difficultyScore)
        {
            difficultyTriggered = true;
            ShowDifficultyPanel();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
        else
            Debug.LogWarning("Score Text is not assigned.");
    }

    private void ShowLevelCompletePanel()
    {
        if (levelCompletePanel == null)
        {
            Debug.LogError(
                "LevelCompletePanel is missing in Level1!");
            return;
        }

        levelCompletePanel.SetActive(true);

        if (levelCompleteSound != null)
        {
            levelCompleteSound.Stop();
            levelCompleteSound.Play();
            Debug.Log("Level 1 completion sound played!");
        }
        else
        {
            Debug.LogWarning(
                "Level 1 completion AudioSource is not assigned.");
        }

        Time.timeScale = 0f;

        Debug.Log("LEVEL 1 COMPLETED!");
    }

    private void ShowDifficultyPanel()
    {
        if (difficultyPanel == null)
        {
            Debug.LogError(
                "MissionCompletePanel is missing in Level2!");
            return;
        }

        difficultyPanel.SetActive(true);

        if (missionCompleteSound != null)
        {
            missionCompleteSound.Stop();
            missionCompleteSound.Play();

            Debug.Log("Mission Complete sound played!");
        }
        else
        {
            Debug.LogError(
                "Mission Complete AudioSource is not assigned!");
        }

        Time.timeScale = 0f;

        Debug.Log("MISSION COMPLETE PANEL SHOWN!");
    }

    public void ContinueFromLevel1()
    {
        if (SceneManager.GetActiveScene().name != "Level1")
            return;

        Time.timeScale = 1f;

        Debug.Log("Loading " + level2Name);
        SceneManager.LoadScene(level2Name);
    }

    public void ContinueAfterDifficulty()
    {
        if (SceneManager.GetActiveScene().name != "Level2")
            return;

        Time.timeScale = 1f;

        Debug.Log("Loading " + nextLevelName);
        SceneManager.LoadScene(nextLevelName);
    }
}
