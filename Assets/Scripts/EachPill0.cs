using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EachPill0 : MonoBehaviour
{
    [SerializeField] private AudioClip sonidoWaka;

    void OnTriggerEnter(Collider collider)
    {        
        if (!collider.tag.Equals("Player"))
        {
            return;
        }

        PlaySounds0.instance.PlaySonidos(sonidoWaka);
        Destroy(gameObject, 0.2f);
    }
}
