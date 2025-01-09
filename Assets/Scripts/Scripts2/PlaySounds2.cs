using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds2 : MonoBehaviour
{
    public static PlaySounds2 instance;
    public AudioSource audioSource;

    void Awake()
    {
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

    public void PlaySonidosLoop(AudioClip clip, bool activar)
    {
        if (activar)
        {
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.loop = true;
        }
        else
        {
            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.Stop();
        }
    }
}
