using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    RightyPlayer _rp;
    LeftyPlayer _lp;
    [SerializeField]
    int playerNum;
    // Use this for initialization
    [SerializeField]
    GameObject leftyAnim;
    [SerializeField]
    GameObject rightyAnimF;
    [SerializeField]
    GameObject rightyAnimB;

    void Awake()
    {
        if (Constants.RightyPlayerNum == playerNum)
            gameObject.tag = "Righty";
        else
            gameObject.tag = "Lefty";
    }

    void Start()
    {
        if (Constants.RightyPlayerNum == playerNum)
        {
            GameObject animF = Instantiate(rightyAnimF, transform);
            GameObject animB = Instantiate(rightyAnimB, transform);
            _rp = new RightyPlayer(playerNum, gameObject.transform, animF, animB);
        }
        else
        {
            _lp = new LeftyPlayer(playerNum, gameObject.transform, gameObject.GetComponent<Collider2D>());
            GameObject anim = Instantiate(leftyAnim, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Constants.RightyPlayerNum == playerNum)
        {
            _rp.Movement();
            _rp.CommitActions();
        }
        else 
            _lp.Movement();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Constants.RightyPlayerNum == playerNum)
        {
            
        }   
        else
        {
            if (collision.gameObject.tag == "FinishLine")
                _lp.DisableCollider();
            else if (collision.gameObject.tag == "Righty")
                Destroy(gameObject);
        }
    }
}

public class LeftyPlayer
{
    InputReader InputReader;
    Transform transform;
    Camera camera;
    Collider2D collider;


    public LeftyPlayer(int _playerNum, Transform _transform, Collider2D _collider)
    {
        this.InputReader = new InputReader(_playerNum, false);
        this.transform = _transform;
        this.collider = _collider;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void Movement()
    {
        Vector2 inputVec = InputReader.ReadMovement();
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


    InputReader inputReader;
    Location location;
    Transform transform;
    Camera camera;
    bool dashing;
    GameObject animF;
    GameObject animB;

    public RightyPlayer(int _playerNum, Transform _transform, GameObject _animf, GameObject _animb)
    {
        this.inputReader = new InputReader(_playerNum, true);
        this.location = Location.up;
        this.transform = _transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector2 worldpos = camera.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.8f, 0));
        transform.position = new Vector2(transform.position.x, worldpos.y);
        this.animF = _animf;
        this.animB = _animb;
        animB.SetActive(false);
    }

    public void Movement()
    {
        Vector2 movement = inputReader.ReadMovement();
        if (!Dashing())
            transform.Translate(new Vector2(AdjustXMovement(movement.x), 0) * 8 * Time.deltaTime);
    }

    public void CommitActions()
    {
        var actions = inputReader.ReadAction();
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
        if (!dashing)
        {
            animB.SetActive(location == Location.up ? false : true);
            animF.SetActive(location == Location.down ? false : true);
            return false;
        }
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
