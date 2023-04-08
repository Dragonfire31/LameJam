using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameEventHandler : MonoBehaviour
{
    // EventManager fields
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;

    //Button Event Manager
    [SerializeField] public Button mainMenuBtn;
    [SerializeField] public Button ResumeBtn;


    // Timer fields
    public float startTime = 300f; // 5 minutes in seconds
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool gameStarted = false;


    private void Start() // Start is called before the first frame update
    {
        // EventManager setup
        mainMenuBtn.onClick.AddListener(OnMainMenuButtonClicked);
        ResumeBtn.onClick.AddListener(ResumeGame);
        StartGame();
    }

    private void Update()
    {
        if (gameStarted)
        {
            Time.timeScale = 1;

            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                GameEnd(); // Game over
            }

            UpdateTimerText();
        }
        else { Time.timeScale = 0f; }

       if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Debug.Log("Game Paused");
        gameStarted = false;
        PausePanel.SetActive(true);
        LeftHand.GetComponent<RotateClockHands>().Pause();
        RightHand.GetComponent<RotateClockHands>().Pause();

    }

    private void ResumeGame()
    {
        Debug.Log("Game Resume");
        gameStarted = true;
        PausePanel.SetActive(false);
        LeftHand.GetComponent<RotateClockHands>().Pause();
        RightHand.GetComponent<RotateClockHands>().Pause();
    }

    #region ButtonClicks
    private void OnMainMenuButtonClicked()
    {
        Debug.Log("Main Menu Button Clicked");
        GameEnd();
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region Timer
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion

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
