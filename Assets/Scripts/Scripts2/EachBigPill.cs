using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EachBigPill : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    public AudioClip sonidoEatingGhost;

    void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }
        
        //ConfigParticlesSystem();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        //print(collider.gameObject.transform.position);

        if (!collider.tag.Equals("Pacman") || FantasmasController.instance.GetBlueGhost())
        {
            return;
        }
        
        PlaySounds2.instance.PlaySonidos(sonidoEatingGhost);
        //Destroy(gameObject, 0.2f);
        gameObject.SetActive(false);

        FantasmasController.instance.IterateChildsSetBLUEghost();
        PacmanController2.instance.InstanceParticleSystemVELOCITYTURBO();
    }

    public void ConfigParticlesSystem()
    {
        print("Particulas:" + particleSystem);

        var main = particleSystem.main;
        main.loop = true;
    }
}
