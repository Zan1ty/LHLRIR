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
    [SerializeField]
    GameObject coinPrefab;
    [SerializeField]
    AudioClip[] rightySounds;
    [SerializeField]
    AudioClip leftySound;
    GameObject shadow;

    void Awake()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if (Constants.RightyPlayerNum == playerNum)
        {
            gameObject.tag = "Righty";
            foreach (Transform go in transform)
            {
                if (go.name == "shadow")
                {
                    go.localPosition = new Vector2(0, -2.72f);
                }
            }
        }
        else
        {
            gameObject.tag = "Lefty";
            foreach (Transform go in transform)
            {
                if (go.name == "shadow")
                {
                    go.localPosition = new Vector2(-0.76f, -3.72f);
                }
            }
        }

        DeleteSparePlayerObjects(playerNum);
        
    }

    void Start()
    {
        if (Constants.RightyPlayerNum == playerNum)
        {
            GameObject animF = Instantiate(rightyAnimF, transform);
            GameObject animB = Instantiate(rightyAnimB, transform);
            _rp = new RightyPlayer(playerNum, gameObject.transform, animF, animB, gameObject.GetComponent<BoxCollider2D>(), gameObject.GetComponent<AudioSource>(), rightySounds);
        }
        else
        {
            _lp = new LeftyPlayer(playerNum, gameObject.transform, gameObject.GetComponent<BoxCollider2D>(), gameObject.GetComponent<AudioSource>(), leftySound);
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
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamScribuls>().RemoveTransform(gameObject.transform.GetInstanceID());
                Constants.LeftyAmount--;
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
                _lp.PlayDeadSound();
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "BearTrap")
            {
                _lp.TrapTimer = 1.0f;
            }
        }
    }

    void DeleteSparePlayerObjects(int _playerNum)
    {
        int padAmount = Input.GetJoystickNames().Length;
        if (_playerNum > padAmount)
        {
            Constants.LeftyAmount--;
            Destroy(gameObject);
        }
    }
}

public class LeftyPlayer
{
    InputReader InputReader;
    Transform transform;
    Camera camera;
    BoxCollider2D collider;
    AudioSource audioSource;
    AudioClip audioClip;
    float trapTimer;


    public LeftyPlayer(int _playerNum, Transform _transform, BoxCollider2D _collider, AudioSource _audioSource, AudioClip _audioClip)
    {
        this.InputReader = new InputReader(_playerNum, false);
        this.transform = _transform;
        this.collider = _collider;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        collider.size = new Vector2(1, 2.34f);
        collider.offset = new Vector2(0, -2.63f);
        collider.enabled = true;
        this.audioSource = _audioSource;
        this.audioClip = _audioClip;
        trapTimer = 0;
    }

    public void Movement()
    {
        Vector2 inputVec = InputReader.ReadMovement();
        if (!Trap())
            transform.Translate(new Vector2(AdjustXMovement(inputVec.x), AdjustYMovement(inputVec.y)) * 12 * Time.deltaTime);
      
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

        if (screenPos.y < (Screen.height * 0.45f) && screenPos.y > (Screen.height * 0.3f))
            return inputY;
        else if (screenPos.y < (Screen.height * 0.3f) && inputY > 0)
            return inputY;
        else if (screenPos.y > (Screen.height * 0.45f) && inputY < 0)
            return inputY;
        else
            return 0;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void PlayDeadSound()
    {
        audioSource.PlayOneShot(audioClip);
    }

     bool Trap() {
        if (trapTimer > 0)
        {
            trapTimer -= Time.deltaTime;
            return true;
        }
        else return false;
    }

    public float TrapTimer
    {
        set { trapTimer = value; }
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
    bool yelling;
    GameObject animF;
    GameObject animB;
    BoxCollider2D collider;
    AudioSource audioSource;
    AudioClip[] audioClips;

    public RightyPlayer(int _playerNum, Transform _transform, GameObject _animf, GameObject _animb, BoxCollider2D _collider, AudioSource _audioSource, AudioClip[] _audioClips)
    {
        this.inputReader = new InputReader(_playerNum, true);
        this.location = Location.up;
        this.transform = _transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector2 worldpos = camera.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.48f, 0));
        transform.position = new Vector2(transform.position.x - 8, worldpos.y);
        this.animF = _animf;
        this.animB = _animb;
        animB.SetActive(false);
        this.collider = _collider;
        collider.size = new Vector2(2.24f, 3f);
        collider.offset = new Vector2(0f, 0f);
        collider.enabled = true;
        this.audioSource = _audioSource;
        this.audioClips = _audioClips;
        yelling = false;
    }

    public void Movement()
    {
        Vector2 movement = inputReader.ReadMovement();
        if (!Dashing())
            transform.Translate(new Vector2(AdjustXMovement(movement.x), 0) * 30 * Time.deltaTime);
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
            yelling = false;
            return false;
        }
        Vector2 screenPos = camera.WorldToScreenPoint(transform.position);
        if(!yelling)
        PlayDashingSound();
        if (location == Location.up)
        {
            if (screenPos.y >= (Screen.height * 0.15f))
                transform.Translate(Vector2.down * 25f * Time.deltaTime);
            else
            {
                collider.offset = new Vector2(0f, -1.6f);
                location = Location.down;
                dashing = false;
            }
        }
        else
        {
            if (screenPos.y <= (Screen.height * 0.48f))
                transform.Translate(Vector2.up * 25f * Time.deltaTime);
            else
            {
                location = Location.up;
                collider.offset = new Vector2(0f, 0f);
                dashing = false;
            }
        }
        return true;
    }

    void PlayDashingSound()
    {
        int temp = UnityEngine.Random.Range(0, 2);
        audioSource.PlayOneShot(audioClips[temp]);
        yelling = true;
    }
}
