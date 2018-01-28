using UnityEngine;
using System.Collections;

public class BearTrap : MonoBehaviour
{
    [SerializeField]
    Sprite closed;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    AudioClip audioClip;
    AudioSource audioSource;
    bool b_closed;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
        b_closed = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Lefty" && !b_closed)
        {
            spriteRenderer.sprite = closed;
            audioSource.PlayOneShot(audioClip);
            b_closed = true;
            gameObject.tag = "BearTrapClosed";
        }    
    }

}
