using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

public class FantasmasController : MonoBehaviour
{
    public static FantasmasController instance;

    private bool invisibleGhosts = false;
    [SerializeField] private Vector3[] initialGhostPositions;
    private Vector3 SIZE = new Vector3(0.4f, 0.4f, 0.4f);
    public float DISTANCE_COLLISION_VS_PACMAN = 0.3f;
    public LayerMask layerMask;

    private float blueGhostDuration = 9.1f;
    private bool blueGhost = false;
    private bool intermitent = false;

    private float VEL_NORMAL = 2.0f;
    private float VEL_BLUE = 1.0f;

    [Header("Array de gameObjects-bonus")]
    [Tooltip("Valor inicial progresion-bonus (100x2)")]
    public int progresion = 100;
    [Tooltip("GameObjects Bonus")]
    [SerializeField] private GameObject[] bonusArray;

    [Header("Size of Ghost-ParticlesSystem")]
    public Vector3 sizeParticlesSystem = new Vector3(0.75f, 0.75f, 0.75f);

    [Header("Archivos de audio")]
    public AudioClip sonidoPacmanDies;
    public AudioClip sonidoDuranteAzules;
    public AudioClip sonidoEatingGhost;

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
        invisibleGhosts = false;
    }

    void Update()
    {
        ToggleInvisibleGhosts();
    }

    private void ToggleInvisibleGhosts()
    {
        if ((GameManager2.instance.estadoActual.Equals(GameManager2.Estado.PreJuego) && !invisibleGhosts) || (GameManager2.instance.estadoActual.Equals(GameManager2.Estado.GameOver) && !invisibleGhosts))
        {
            invisibleGhosts = true;
            IterateChilds(false);
        }
        else if (GameManager2.instance.estadoActual.Equals(GameManager2.Estado.Preparado) && invisibleGhosts)
        {
            invisibleGhosts = false;
            IterateChilds(true);
        }
    }

    private void IterateChilds(bool boolean)
    {
        foreach (Transform child in transform)
        {
            child.localScale = SIZE;

            foreach (Transform child2 in child)
            {
                Renderer childRenderer = child2.GetComponent<Renderer>();

                if (childRenderer != null)
                {
                    //childRenderer.material.color = Color.black;
                    childRenderer.enabled = boolean;
                }

                foreach (Transform child3 in child2)
                {
                    Renderer childRenderer2 = child3.GetComponent<Renderer>();

                    if (childRenderer2 != null)
                    {
                        //childRenderer.material.color = Color.black;
                        childRenderer2.enabled = boolean;
                    }
                }
            }
        }
    }

    public void IterateChildsRESETInitialPosition()
    {
        int contador = 0;

        foreach (Transform child in transform)
        {
            NavMeshAgent agent = child.GetComponent<NavMeshAgent>();

            agent.Warp(initialGhostPositions[contador]);
            //child.transform.position = initialGhostPositions[contador];
            contador ++;
        }
    }

    public void IterateChildsSetBLUEghost()
    {
        blueGhost = true;

        foreach (Transform child in transform)
        {
            foreach (Transform child2 in child)
            {
                Renderer childRenderer = child2.GetComponent<Renderer>();

                if (childRenderer != null)
                {
                    childRenderer.material.color = Color.blue;
                    //childRenderer.enabled = boolean;
                }
            }
        }

        PlaySounds2.instance.PlaySonidosLoop(sonidoDuranteAzules, true);

        float endBlueGhost = blueGhostDuration - (float)GameManager2.instance.GetLevel();
        Debug.Log(endBlueGhost);

        Invoke("IterateChildsSetNORMALghost", endBlueGhost);
        Invoke("SetINTERMITENTghost", endBlueGhost * 0.8f);
    }

    public void IterateChildsSetNORMALghost()
    {
        blueGhost = false;
        intermitent = false;
        progresion = 100;

        Color[] colores = new Color[4]
        {
            Color.red, Color.cyan, Color.magenta, Color.green
        };

        int contador = 0;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<EachFantasmaNMA>() != null)
            {
                child.GetComponent<EachFantasmaNMA>().SetFantasmaComido(false);
            }

            if (child.GetComponent<EachFantasmaNMA1>() != null)
            {
                child.GetComponent<EachFantasmaNMA1>().SetFantasmaComido(false);
            }

            if (child.GetComponent<EachFantasmaNMA2>() != null)
            {
                child.GetComponent<EachFantasmaNMA2>().SetFantasmaComido(false);
            }

            if (child.GetComponent<EachFantasmaNMA3>() != null)
            {
                child.GetComponent<EachFantasmaNMA3>().SetFantasmaComido(false);
            }

            foreach (Transform child2 in child)
            {
                Renderer childRenderer = child2.GetComponent<Renderer>();

                if (childRenderer != null)
                {
                    childRenderer.enabled = true;
                    childRenderer.material.color = colores[contador];
                }
            }

            contador ++;
        }

        PlaySounds2.instance.PlaySonidosLoop(sonidoDuranteAzules, false);
    }

    public void InstanceBonusPoints(int bonus, Transform fantasma)
    {
        float VELOCITY = 75f;
        float DESTROY_TIMER = 3.1f;

        GameObject bonusPoints;
        bonusPoints = Instantiate(SelectHowManyBonus(bonus));
        bonusPoints.transform.position = fantasma.position;
        bonusPoints.transform.localScale = new Vector3(1f, 1f, 1f);
        //bonusPoints.transform.Translate(direction * VELOCITY * Time.deltaTime);
        bonusPoints.GetComponent<Rigidbody>().AddForce(fantasma.forward * VELOCITY);

        Destroy(bonusPoints, DESTROY_TIMER);
    }

    private GameObject SelectHowManyBonus(int bonus)
    {
        if (bonus == 200)
        {
            return bonusArray[0];
        }

        if (bonus == 400)
        {
            return bonusArray[1];
        }

        if (bonus == 800)
        {
            return bonusArray[2];
        }

        if (bonus == 1600)
        {
            return bonusArray[3];
        }

        return bonusArray[0];
    }

    public void ConfigParticlesSystem(GameObject particles)
    {
        var particleSystem = particles.GetComponent<ParticleSystem>();

        var main = particleSystem.main;
        //main.loop = false;
        main.startSpeed = 9.0f;
        main.gravityModifier = 1.0f;

        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.blue, 0.0f),   // Azul al inicio
                new GradientColorKey(Color.cyan, 1.0f)   // Cyan al final
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0.0f),   // Alfa completo al inicio
                new GradientAlphaKey(0.9f, 1.0f)    // Medio-transparencia al final
            }
        );

        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    public Vector3 InitialRotationGhosts()
    {
        return new Vector3(-90f, 0f, 0f);
    }

    public Vector3 KeepYPosition()
    {
        return new Vector3(0f, 0.5f, 0f);
    }

    private void SetINTERMITENTghost()
    {
        intermitent = true;
    }

    public bool GetIntermitent()
    {
        return intermitent;
    }

    public bool GetBlueGhost()
    {
        return blueGhost;
    }

    public float GetBlueGhostDuration()
    {
        return blueGhostDuration;
    }

    public float GetVEL_NORMAL()
    {
        return VEL_NORMAL + (float)GameManager2.instance.GetLevel() / 20.0f;
    }

    public float GetVEL_BLUE()
    {
        return VEL_BLUE;
    }
}
