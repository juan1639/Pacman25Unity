using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EachPill2 : MonoBehaviour
{
    public AudioClip sonidoWaka2;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        //print(collider.gameObject.transform.position);

        if (!collider.tag.Equals("Pacman"))
        {
            return;
        }
        
        PlaySounds2.instance.PlaySonidos(sonidoWaka2);
        PillsController2.instance.AddPointsToScore();
        Destroy(gameObject, 0.2f);
    }
}
