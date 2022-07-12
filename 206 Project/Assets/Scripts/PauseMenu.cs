using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    //Progression variables for the game state
    public bool keyCollected;
    public bool gameLost;
    public bool gameWon;
    public bool gameFinished;

    //References to menus and text
    public static bool isPaused;
    public GameObject pauseMenu;
    public GameObject lostMenu;
    public GameObject winMenu;
    public TextMeshProUGUI keyText;

    //Initialisation of variables, as well as cursor and time
    private void Start()
    {
        keyCollected = false;
        gameFinished = false;
        gameLost = false;
        gameWon = false;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        Cursor.visible = false;
        keyText = FindObjectOfType<TextMeshProUGUI>();
    }

    //Checks if the pause menu has been called, as well as the game states
    void Update()
    {
        if(gameFinished == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        if (gameLost)
        {
            Lose();
        }

        if (gameWon)
        {
            Win();
        }

        if (keyCollected)
        {
            keyText.text = "KEY COLLECTED";
        }
    }

    //Resumes the game
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
    }

    //Pauses the game
    void Pause()
    {
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
    }

    //Loads main menu
    public void LoadMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    //Restarts the game
    public void Restart()
    {
        lostMenu.SetActive(false);
        winMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Game is lost, with menu options for leaving or restarting
    public void Lose()
    {
        gameFinished = true;
        gameLost = true;
        lostMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    //Game is won, with menu options for leaving or restarting
    public void Win()
    {
        gameFinished = true;
        gameWon = true;
        winMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    //Exits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
