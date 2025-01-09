using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EachFantasmaNMA2 : MonoBehaviour
{
    // ****** FANTASMA MAGENTA (Cuando esta cerca HUYE) *******
    private NavMeshAgent agent;
    private Vector3[] wayPoints;
    [SerializeField] private Transform pacman;
    public Vector3 initialPosition = new Vector3(-11.5f, 1.5f, 8.5f);
    [SerializeField] private Vector3 scaredPosition = new Vector3(-11.5f, 1.5f, 8.5f);

    private int indexWayPoints = 0;
    private float distanceToCurrentWayPoint = 1.5f;

    private bool followPacman;
    private float timeNoFollow = 5.0f;
    private float timer = 0.0f;

    private bool fantasmaComido = false;

    [SerializeField] private GameObject preFabParticlesSystem;
    private GameObject particles;
    
    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        agent.speed = FantasmasController.instance.GetVEL_NORMAL();
        agent.speed += (float)GameManager2.instance.GetLevel() / 10;
        //print(agent.speed);

        agent.autoBraking = false;
        agent.acceleration = 10.0f;

        wayPoints = WayPointsManager.instance.GetWayPointsArray();

        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(FantasmasController.instance.InitialRotationGhosts());

        followPacman = false;
    }

    void Update()
    {
        // 0 PreJuego | 1 Preparado | 2 EnJuego | 3 Dying | 4 GameOver | 5 NivelSuperado
        if (GameManager2.instance.estadoActual.Equals(GameManager2.Estado.NivelSuperado) || GameManager2.instance.estadoActual.Equals(GameManager2.Estado.Dying))
        {
            transform.rotation = Quaternion.Euler(FantasmasController.instance.InitialRotationGhosts());
            agent.isStopped = true;
            return;
        }

        if ((int)GameManager2.instance.estadoActual != 2)
        {
            transform.position = initialPosition;
            transform.rotation = Quaternion.Euler(FantasmasController.instance.InitialRotationGhosts());
            return;
        }

        agent.isStopped = false;
        timer += Time.deltaTime;
        SetGhostDestination();
        
        transform.Translate(FantasmasController.instance.KeepYPosition());
        //transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        transform.Rotate(-90f, 0f, 0f);

        CheckIfIntermitent();
        CheckCollisionVsPacman();
    }

    private void SetGhostDestination()
    {
        if (FantasmasController.instance.GetBlueGhost())
        {
            agent.SetDestination(scaredPosition);
            agent.speed = FantasmasController.instance.GetVEL_BLUE();
        }
        else
        {
            agent.speed = FantasmasController.instance.GetVEL_NORMAL();

            if (followPacman)
            {
                agent.SetDestination(pacman.position);

                if (CheckDistance())
                {
                    followPacman = false;
                    timer = 0.0f;
                }
            }
            else
            {
                if (timer > timeNoFollow)
                {
                    followPacman = true;
                }
                
                agent.SetDestination(wayPoints[indexWayPoints]);

                if (!agent.pathPending && agent.remainingDistance <= distanceToCurrentWayPoint)
                {
                    // Incrementa el  ndice, y con el operador m dulo resetea al llegar al final
                    indexWayPoints = (indexWayPoints + 1) % GetWayPointsLENGTH();
                }
            }
        }
    }

    private bool CheckDistance()
    {
        float RANGE = 2.1f;
        float distancia = Vector3.Distance(transform.position, pacman.position);

        if (distancia <= RANGE)
        {
            //Debug.Log("El personaje está dentro del RANGO");
            return true;
        }
        else
        {
            //Debug.Log("El personaje está demasiado lejos.");
            return false;
        }
    }

    private int GetWayPointsLENGTH()
    {
        return wayPoints.Length;
    }

    private void CheckCollisionVsPacman()
    {
        if (!GameManager2.instance.estadoActual.Equals(GameManager2.Estado.EnJuego))
        {
            return;
        }

        float DISTANCIA = FantasmasController.instance.DISTANCE_COLLISION_VS_PACMAN;
        LayerMask layerMask = FantasmasController.instance.layerMask;

        if (Physics.CheckSphere(transform.position, DISTANCIA, layerMask))
        {
            if (!FantasmasController.instance.GetBlueGhost())
            {
                //print("Fant:" + transform.position);
                print("EstadoActual: Dying");

                GameManager2.instance.estadoActual = GameManager2.Estado.Dying;
                PacmanController2.instance.transform.gameObject.SetActive(false);
                GameManager2.instance.InstancePacmanDies(PacmanController2.instance.transform.position);
                PlaySounds2.instance.PlaySonidos(FantasmasController.instance.sonidoPacmanDies);
            }
            else
            {
                if (!fantasmaComido)
                {
                    fantasmaComido = true;
                    FantasmasController.instance.progresion *= 2;

                    foreach (Transform child in transform)
                    {
                        Renderer childRenderer = child.GetComponent<Renderer>();

                        if (childRenderer != null)
                        {
                            //childRenderer.material.color = colores[contador];
                            childRenderer.enabled = false;
                        }
                    }

                    GameManager2.instance.SetPoints(GameManager2.instance.GetPoints() + FantasmasController.instance.progresion);
                    FantasmasController.instance.InstanceBonusPoints(FantasmasController.instance.progresion, transform);

                    InstanceParticleSystem();
                    PlaySounds2.instance.PlaySonidos(FantasmasController.instance.sonidoEatingGhost);
                }
            }
        }
    }

    private void CheckIfIntermitent()
    {
        if (FantasmasController.instance.GetIntermitent())
        {
            foreach (Transform child in transform)
            {
                Renderer childRenderer = child.GetComponent<Renderer>();

                if (childRenderer != null)
                {
                    if (childRenderer.material.color.Equals(Color.white))
                    {
                        childRenderer.material.color = Color.blue;
                    }
                    else
                    {
                        childRenderer.material.color = Color.white;
                    }

                    //childRenderer.enabled = boolean;
                }
            }
        }
    }

    public void InstanceParticleSystem()
    {
        float DESTROY_TIMER = 0.8f;

        particles = Instantiate(preFabParticlesSystem);
        particles.transform.position = transform.position;
        particles.transform.localScale = FantasmasController.instance.sizeParticlesSystem;

        FantasmasController.instance.ConfigParticlesSystem(particles);

        Destroy(particles, DESTROY_TIMER);
    }

    public bool GetFantasmaComido()
    {
        return fantasmaComido;
    }

    public void SetFantasmaComido(bool comido)
    {
        fantasmaComido = comido;
    }
}
