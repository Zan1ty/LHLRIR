using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScribuls : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("zGameplay", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
