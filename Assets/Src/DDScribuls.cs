using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DDScribuls : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Dropdown dd = gameObject.GetComponent<Dropdown>();
        dd.ClearOptions();
        dd.AddOptions(PlayerDDOptions());

        dd.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dd);
        });

        Constants.RightyPlayerNum = 1;
    }

    List<string> PlayerDDOptions()
    {
        List<string> options = new List<string>();
        string[] joysticks = Input.GetJoystickNames();

        for (int i = 1; i < joysticks.Length + 1; i++)
        {
            options.Add("Player " + i.ToString());
        }

        return options;
    }

    void DropdownValueChanged(Dropdown dd)
    {
        Constants.RightyPlayerNum = dd.value + 1;
    }

}

