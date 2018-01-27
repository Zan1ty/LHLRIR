using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScribuls : MonoBehaviour
{

    GameObject pauseMenu;
    bool paused;

    void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
    }

    void Start()
    {
        paused = false;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("pause"))
            PauseGame();
    }

    public void PauseGame()
    {
        if (paused)
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            paused = true;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }    
}
