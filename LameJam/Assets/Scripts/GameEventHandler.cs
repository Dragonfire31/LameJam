using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using JetBrains.Annotations;

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

    private float timeRemaining;

    // Enemy spawn fields
    private float spawnDelay = 2.0f;
    private float timeLeft = 0.0f;

    // Game state
    private bool gameStarted = false;

    // Reference to your camera
    public Camera mainCamera;

    // List of enemies currently active in the game
    private List<GameObject> activeEnemies = new List<GameObject>();


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

        // Check if any active enemies are off-screen and remove them from the list
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (!IsVisibleFrom(activeEnemies[i].GetComponent<Renderer>(), mainCamera))
            {
                Destroy(activeEnemies[i]);
                activeEnemies.RemoveAt(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    // Helper method to check if a renderer is visible from a camera
    bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    #region PauseMenu
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
    #endregion


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


}
