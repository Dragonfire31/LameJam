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

    // HandsField
    [SerializeField] private GameObject LeftHand;


    private void Start() // Start is called before the first frame update
    {
        // EventManager setup
        startButton.onClick.AddListener(OnStartButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Update()
    {
        LeftHand.GetComponent<RotateClockHands>().MainMenu(true);
    }

    #region ButtonClicks
    private void OnStartButtonClicked()
    {
        LeftHand.GetComponent<RotateClockHands>().MainMenu(false);
        Debug.Log("Game Start Button Clicked");
        SceneManager.LoadScene("GameScene");
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings Button Clicked");
        SceneManager.LoadScene("SettingsScene");
    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Button Clicked");
        Application.Quit();
    }
    #endregion

}
