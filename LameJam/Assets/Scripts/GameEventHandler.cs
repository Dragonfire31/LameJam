using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameEventHandler : MonoBehaviour
{
    // EventManager fields
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private CreateNewEnemy[] enemyDataList;
    [SerializeField] private float minSpawnDelay = 2.0f;
    [SerializeField] private float maxSpawnDelay = 5.0f;
    [SerializeField] private float spawnRateIncreasePerSecond = 0.1f;
    [SerializeField] private float spawnMinX = 0f; // Minimum value for x
    [SerializeField] private float spawnMaxX = 5f;  // Maximum value for x
    [SerializeField] private float SpawnY = 5f;     // Static value for y

    //Button Event Manager
    [SerializeField] public Button mainMenuBtn;
    [SerializeField] public Button ResumeBtn;


    // Timer fields
    public float startTime = 300f; // 5 minutes in seconds
    [SerializeField] private TextMeshProUGUI timerText;

    // Score fields
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private float timeRemaining;

    // Enemy spawn fields
    private float spawnDelay = 2.0f;
    private float timeLeft = 0.0f;

    // Game state
    private bool gameStarted = false;
    private int totalScore = 0;

    //HistoryScreen
    public GameObject imagePrefab; // The prefab for the image element in the scroll view
    public GameObject HistoryPanel; // The transform of the content panel in the scroll view

    public List<Image> killedSprites = new List<Image>(); 

    public Vector3 spawnPosition = new Vector3(0, -4000, 0);

    private void Start() // Start is called before the first frame update
    {
        // EventManager setup
        mainMenuBtn.onClick.AddListener(OnMainMenuButtonClicked);
        ResumeBtn.onClick.AddListener(ResumeGame);
        LeftHand.GetComponent<RotateClockHands>().MainMenu(false);
        RightHand.GetComponent<RotateClockHands>().MainMenu(false);
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

            if (timeLeft < timeRemaining)
            {
                spawnDelay = Mathf.Clamp(spawnDelay - spawnRateIncreasePerSecond * Time.deltaTime, minSpawnDelay, maxSpawnDelay);
                if (UnityEngine.Random.value < Time.deltaTime / spawnDelay)
                {
                    SpawnEnemy();
                }
            }
        }
        else { Time.timeScale = 0f; }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }


    #region PauseMenu
    private void PauseGame()
    {
        Debug.Log("Game Paused");
        gameStarted = false;
        PausePanel.SetActive(true);
        LeftHand.GetComponent<RotateClockHands>().Pause(true);
        RightHand.GetComponent<RotateClockHands>().Pause(true);

    }

    private void ResumeGame()
    {
        Debug.Log("Game Resume");
        gameStarted = true;
        PausePanel.SetActive(false);
        LeftHand.GetComponent<RotateClockHands>().Pause(false);
        RightHand.GetComponent<RotateClockHands>().Pause(false);
    }
    #endregion


    #region ButtonClicks
    private void OnMainMenuButtonClicked()
    {
        Debug.Log("Main Menu Button Clicked");
        LeftHand.GetComponent<RotateClockHands>().Pause(false);
        RightHand.GetComponent<RotateClockHands>().Pause(false);
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

    public void AddScore(int score)
    {
        totalScore += score;
        scoreText.text = totalScore.ToString();
        Debug.Log("Score: " + totalScore);
    }

    private void SpawnEnemy()
    {
        if (gameStarted)
        {
            // Select a random enemy prefab from the enemyPrefabs array
           CreateNewEnemy enemyData = enemyDataList[UnityEngine.Random.Range(0, enemyDataList.Length)];

            // Spawn the enemy at a random position within the spawnZone
            float z = 0f;     // Static value for z

            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(spawnMinX, spawnMaxX), SpawnY, z);
            enemyData.SpawnEnemy(randomPosition);
        }
    }

    public void OnEnemyScore(Sprite sprite)
    {
        const int maxImagesPerRow = 10;
        const int rowSpacing = 50;
        const int colSpacing = -50;

        // Check if we need to start a new row
        bool startNewRow = killedSprites.Count % maxImagesPerRow == 0 && killedSprites.Count > 0;

        // If we need to start a new row, adjust y position
        if (startNewRow)
        {
            spawnPosition.y += colSpacing;
            spawnPosition.x -= rowSpacing * (maxImagesPerRow - 1);
            startNewRow = false;
        }else
        {
            spawnPosition.x += rowSpacing;
        }
        // Instantiate a new image element from the prefab
        GameObject newImage = Instantiate(imagePrefab, Vector3.zero, Quaternion.identity, HistoryPanel.transform);
        newImage.GetComponent<RectTransform>().localPosition = spawnPosition;

        // Set the sprite of the new image element
        newImage.GetComponent<Image>().sprite = sprite;

        // Add the sprite image to the list of killed sprites
        killedSprites.Add(newImage.GetComponent<Image>());

    }


}
