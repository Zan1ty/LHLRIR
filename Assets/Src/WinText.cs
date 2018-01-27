using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinText : MonoBehaviour
{
    [SerializeField]
    Text winText;
    // Use this for initialization
    void Start()
    {
        winText.text = Constants.Winner;
    }

}
