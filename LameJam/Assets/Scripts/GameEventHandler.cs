using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class GameEventHandler : MonoBehaviour
{
    // EventManager fields
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;

    //Spawning
    [SerializeField] private CreateNewEnemy[] enemyDataList; // The list of enemy data to spawn from
    [SerializeField] public float spawnInterval = 2.0f; // The base spawn interval in seconds.
    [SerializeField] public float maxSpawnRate = 0.5f;  // The maximum spawn rate in probability per second.
    [SerializeField] public float spawnRateIncreaseInterval = 10.0f; // The interval at which to increase the spawn rate.
    [SerializeField] public float spawnRateIncreaseAmount = 0.05f;// The amount by which to increase the spawn rate at each interval.
    [SerializeField] private float spawnRateTimer = 0.0f;    // The timer for tracking when to increase the spawn rate.
    [SerializeField] private float spawnRate = 0.0f;    // The current spawn rate in probability per second.
    [SerializeField] public float minSpawnDelay = 1.0f;// The minimum time to wait before spawning a new monster, in seconds.
    [SerializeField] private float spawnDelayTimer = 0.0f;// The timer for tracking the time since the last monster was spawned.
    [SerializeField] private float[] spawnMinX; // Minimum value for x
    [SerializeField] private float[] spawnMaxX;  // Maximum value for x
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
    private int totalScore;

    //HistoryScreen
    public GameObject imagePrefab; // The prefab for the image element in the scroll view
    public GameObject HistoryPanel; // The transform of the content panel in the scroll view

    public List<Image> killedSprites = new List<Image>(); 

    public Vector3 spawnPosition = new Vector3(0, -4000, 0);

    private string filePath;

    private void Start() // Start is called before the first frame update
    {
        filePath = Application.persistentDataPath + "/score.txt";
        LoadScore();
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

            // Increment the spawn rate timer by the time that has passed since the last frame.
            spawnRateTimer += Time.deltaTime;

            // Increase the spawn rate if enough time has passed since the last increase.
            if (spawnRateTimer >= spawnRateIncreaseInterval)
            {
                // Subtract the spawn rate increase interval from the timer.
                spawnRateTimer -= spawnRateIncreaseInterval;

                // Increase the spawn rate by the spawn rate increase amount, up to the maximum spawn rate.
                spawnRate = Mathf.Min(spawnRate + spawnRateIncreaseAmount, maxSpawnRate);
            }

            // Increment the spawn delay timer by the time that has passed since the last frame.
            spawnDelayTimer += Time.deltaTime;

            // Spawn a monster randomly based on the current spawn rate and the time that has passed since the last frame,
            // but always ensure that there is at least one monster spawned at all times.
            if (UnityEngine.Random.value < spawnRate * Time.deltaTime)
            {
                SpawnEnemy();
                spawnDelayTimer = 0.0f; // Reset the spawn delay timer.
            }
            if (spawnDelayTimer >= minSpawnDelay) // Spawn a monster if enough time has passed since the last monster was spawned.
            {
                SpawnEnemy();
                spawnDelayTimer = 0.0f; // Reset the spawn delay timer.
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
        SpawnEnemy();
    }

    private void GameEnd()
    {
        SaveScore();
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
            float minX = 0;
            float maxX = 0;

            bool randbool = UnityEngine.Random.Range(0,2) == 0;
            if (randbool)
            {
                minX = spawnMinX[0];
                maxX = spawnMinX[1];
            }
            else
            {
                minX = spawnMaxX[0];
                maxX = spawnMaxX[1];
            }

            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), SpawnY, z);
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

        void SaveScore()
    {
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine(totalScore.ToString()); // write the total score as a string to the file
        writer.Close();
    }

        void LoadScore()
    {
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string scoreString = reader.ReadLine(); // read the score as a string from the file
            reader.Close();

            int.TryParse(scoreString, out totalScore); // convert the string to an int and store it as the total score
        }
    }


}
