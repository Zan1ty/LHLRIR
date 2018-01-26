using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    RightyPlayer _rp;
    LeftyPlayer _lp;
    // Use this for initialization
    void Start()
    {
        //_rp = new RightyPlayer(1, gameObject.transform);
        _lp = new LeftyPlayer(1, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        _lp.Movement();
    }
}

public class LeftyPlayer
{
    ReadInput readInput;
    Transform transform;

    public LeftyPlayer(int _playerNum, Transform _transform)
    {
        this.readInput = new ReadInput(_playerNum, false);
        this.transform = _transform;
    }

    public void Movement()
    {
        Vector2 inputVec = readInput.ReadMovement();
        transform.Translate(new Vector2(AdjustXMovement(inputVec.x), inputVec.y) * 2 * Time.deltaTime);
      
    }

    float AdjustXMovement(float inputX)
    {
        if (inputX == 0)
            return 1;
        else
            return 1.1f + inputX;
    }
}

public class RightyPlayer
{
    enum Location { up, down };


    ReadInput readInput;
    Location location;
    Transform transform;
    Camera camera;
    bool dashing;

    public RightyPlayer(int _playerNum, Transform _transform)
    {
        this.readInput = new ReadInput(_playerNum, true);
        this.location = Location.up;
        this.transform = _transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void Movement()
    {
        Vector2 movement = readInput.ReadMovement();
        if (!Dashing())
            transform.Translate(new Vector2(movement.x, 0) * 8 * Time.deltaTime);
    }

    public void CommitActions()
    {
        var actions = readInput.ReadAction();
        if (actions.Count > 0)
        {
            string action = actions.Dequeue();
            switch (action)
            {
                case "fire":
                    dashing = true;
                    Dashing();
                    break;
                default:
                    break;
            }
        }
    }

    bool Dashing()
    {
        if (!dashing) return false;
        Vector2 screenPos = camera.WorldToScreenPoint(transform.position);

        if (location == Location.up)
        {
            if (screenPos.y >= (Screen.height * 0.2f))
                transform.Translate(Vector2.down * 15f * Time.deltaTime);
            else
            {
                location = Location.down;
                dashing = false;
            }
        }
        else
        {
            if (screenPos.y <= (Screen.height * 0.8f))
                transform.Translate(Vector2.up * 15f * Time.deltaTime);
            else
            {
                location = Location.up;
                dashing = false;
            }
        }
        return true;
    }
}
