using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput {

    int playerNum;
    Queue<string> actions;

    public ReadInput(int _playerNum, bool _isRighty)
    {
        this.playerNum = _playerNum;
        if (_isRighty)
            actions = new Queue<string>();
    }

    public Vector2 ReadMovement()
    {
        float x = Input.GetAxis("Horizontal" + playerNum.ToString());
        float y = Input.GetAxis("Vertical" + playerNum.ToString());
        return new Vector2(x, y);
    }

    public Queue<string> ReadAction()
    {
        if (Input.GetButtonDown("fire" + playerNum.ToString()))
            actions.Enqueue("fire");

        return actions;
    }
}
