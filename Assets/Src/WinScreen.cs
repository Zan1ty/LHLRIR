using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{

    public void LoadMainMenu()
    {
        Constants.LeftyAmount = 3;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
