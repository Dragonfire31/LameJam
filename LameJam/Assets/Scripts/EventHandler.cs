using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class EventHandler : MonoBehaviour
{
    // EventManager fields
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;

    // Timer fields
    public float startTime = 300f; // 5 minutes in seconds
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool gameStarted = false;

    private void Start() // Start is called before the first frame update
    {
        // EventManager setup
        startButton.onClick.AddListener(OnStartButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Update()
    {
        if (gameStarted)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                GameEnd(); // Game over
            }

            UpdateTimerText();
        }
    }
    #region ButtonClicks
    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
        StartGame();
        Debug.Log("Game Start Button Clicked");
    }

    private void OnSettingsButtonClicked()
    {
        SceneManager.LoadScene("SettingsScene");
        Debug.Log("Settings Button Clicked");
    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Button Clicked");
        Application.Quit();
    }
    #endregion

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void StartGame()
    {
        Debug.Log("Game Started");
        // Timer setup
        timeRemaining = startTime;
        UpdateTimerText();
        gameStarted = true;
    }

    private void GameEnd()
    {
        Debug.Log("Game Ended");
        gameStarted = false;
    }

}
