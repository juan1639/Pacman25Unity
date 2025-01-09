using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds0 : MonoBehaviour
{
    public static PlaySounds0 instance;
    public AudioSource audioSource;

    void Awake()
    {
        // SINGLETON (nos aseguramos de que solo haya una instancia de esta clase)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        
    }

    public void PlaySonidos(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
