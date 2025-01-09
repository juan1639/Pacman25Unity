using System.Collections;
using System.Collections.Generic;
//using System.Threading;
using UnityEngine;

public class FrutasController : MonoBehaviour
{
    public static FrutasController instance;

    [SerializeField] private bool activeFruit = false;
    private Vector3 SIZE = new Vector3(0.1f, 0.1f, 0.2f);
    [SerializeField] private float timeToActiveFruit = 15.0f; 
    private float timer = 0.0f;

    [SerializeField] private GameObject[] arrayPreFabFruit = new GameObject[16];
    
    [Header("PreFabs ParticlesSystems")]
    [Tooltip("Efecto aparecer")] [SerializeField] private GameObject preFabParticlesSystem;
    [Tooltip("Efecto Eat-bonus")]public GameObject preFabParticlesSystem2;
    public Vector3 sizeParticlesSystem = new Vector3(0.9f, 0.9f, 0.9f);

    public AudioClip sonidoApearFruit;    

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
        //DestroyHierarchyFruits();
        EatFruit();
    }

    void Update()
    {
        if (!activeFruit && GameManager2.instance.estadoActual.Equals(GameManager2.Estado.EnJuego))
        {
            timer += Time.deltaTime;
        }

        ActiveFruit();
    }

    private void ActiveFruit()
    {
        if (GameManager2.instance.estadoActual.Equals(GameManager2.Estado.EnJuego) && !activeFruit && timer > timeToActiveFruit)
        {
            activeFruit = true;
            timer = 0.0f;

            GameObject fruta;
            fruta = Instantiate(arrayPreFabFruit[GameManager2.instance.GetLevel()]);
            fruta.SetActive(true);

            PlaySounds2.instance.PlaySonidos(sonidoApearFruit);
        }

        //print(timer);
    }

    public void EatFruit()
    {
        activeFruit = false;
        timer = 0.0f;
    }

    public Vector3 InitialRotationFrutas()
    {
        return new Vector3(-90f, 0f, 0f);
    }

    public bool GetActiveFruit()
    {
        return activeFruit;
    }

    public void InstanceParticleSystem(Vector3 initialPosition)
    {
        float DESTROY_TIMER = 1.8f;

        GameObject particles;
        particles = Instantiate(preFabParticlesSystem);

        particles.transform.position = initialPosition;
        particles.transform.localScale = new Vector3(1f, 1f, 1f);

        Destroy(particles, DESTROY_TIMER);
    }

    private void DestroyHierarchyFruits()
    {
        foreach (Transform child in transform)
        {

            if (!child.name.StartsWith("FruitInitial"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ConfigParticlesSystem(GameObject particles)
    {
        // No config (usa la config por defecto del Prefab)
    }
}
