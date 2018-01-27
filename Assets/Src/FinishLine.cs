using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lefty")
        {
            Constants.Winner = "LEFTIES";
            SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);
        }
    }

    void Update()
    {
        if (Constants.LeftyAmount <= 0)
        {
            Constants.Winner = "RIGHTIES";
            SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);
        }
    }
}
