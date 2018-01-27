﻿using UnityEngine;
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
        _lp = new LeftyPlayer(1, gameObject.transform, gameObject.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        _lp.Movement();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FinishLine")
            _lp.DisableCollider();
        else if (collision.gameObject.tag == "Righty")
            Destroy(gameObject);
    }
}

public class LeftyPlayer
{
    ReadInput readInput;
    Transform transform;
    Camera camera;
    Collider2D collider;


    public LeftyPlayer(int _playerNum, Transform _transform, Collider2D _collider)
    {
        this.readInput = new ReadInput(_playerNum, false);
        this.transform = _transform;
        this.collider = _collider;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void Movement()
    {
        Vector2 inputVec = readInput.ReadMovement();
        transform.Translate(new Vector2(AdjustXMovement(inputVec.x), AdjustYMovement(inputVec.y)) * 2 * Time.deltaTime);
      
    }

    float AdjustXMovement(float inputX)
    {
        Vector2 screenPos = camera.WorldToScreenPoint(transform.position);
        if (screenPos.x > (Screen.width * 0.99f))
            return 0;
        else if (screenPos.x < (Screen.width * 0.01f))
            return 2.2f;
        else if (inputX == 0)
            return 1;
        else
            return 1.1f + inputX;
    }

    float AdjustYMovement(float inputY)
    {
        Vector2 screenPos = camera.WorldToScreenPoint(transform.position);

        if (screenPos.y < (Screen.height * 0.7f) && screenPos.y > (Screen.height * 0.3f))
            return inputY;
        else if (screenPos.y < (Screen.height * 0.3f) && inputY > 0)
            return inputY;
        else if (screenPos.y > (Screen.height * 0.7f) && inputY < 0)
            return inputY;
        else
            return 0;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }
}

public class RightyPlayer
{
    enum Location { up, down };


    ReadInput readInput;
    Location location;
    Transform transform;
    Camera camera;
    Collider2D collider;
    bool dashing;

    public RightyPlayer(int _playerNum, Transform _transform, Collider2D _collider)
    {
        this.readInput = new ReadInput(_playerNum, true);
        this.location = Location.up;
        this.transform = _transform;
        this.collider = _collider;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void Movement()
    {
        Vector2 movement = readInput.ReadMovement();
        if (!Dashing())
            transform.Translate(new Vector2(AdjustXMovement(movement.x), 0) * 8 * Time.deltaTime);
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

    float AdjustXMovement(float inputX)
    {
        Vector2 screenPos = camera.WorldToScreenPoint(transform.position);

        if (screenPos.x < (Screen.width * 0.01f))
            return 2;
        else if (screenPos.x > (Screen.width * 0.99f))
            return 0;
        else
            return inputX;
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
