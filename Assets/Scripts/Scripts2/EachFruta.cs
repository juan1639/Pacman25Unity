using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EachFruta : MonoBehaviour
{
    private NavMeshAgent agent;   
    [SerializeField] private float NORMAL_SIZE = 0.38f;
    [SerializeField] private Vector3[] INITIAL_POSITION = new Vector3[3];

    private Vector3[] wayPoints;

    private int indexWayPoints = 0;
    private float distanceToCurrentWayPoint = 1.5f;
    private float timer = 0.0f;
    private float timerComeFruta = 0.0f;

    private Animator animator;

    public AudioClip sonidoEatingCherry;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        agent.speed = 1.8f;
        //print(agent.speed);

        agent.autoBraking = false;
        agent.acceleration = 10.0f;

        INITIAL_POSITION[0] = new Vector3(-9.5f, 1.5f, 11.5f);
        INITIAL_POSITION[1] = new Vector3(-9.5f, 1.5f, 8.5f);
        INITIAL_POSITION[2] = new Vector3(-9.5f, 1.5f, 4.5f);
        int choose = Random.Range(0, INITIAL_POSITION.Length);

        agent.Warp(INITIAL_POSITION[choose]);
        FrutasController.instance.InstanceParticleSystem(INITIAL_POSITION[choose]);

        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("Aparecer", true);
        }
        else
        {
            Debug.Log("Error: Animator NO encontrado");
        }

        wayPoints = WayPointsManager.instance.GetWayPointsArray();

        timerComeFruta = 0.0f;
        timer = 0.0f;
    }

    void Update()
    {
        // 0 PreJuego | 1 Preparado | 2 EnJuego | | 3 Dying | 4 GameOver | 5 NivelSuperado
        transform.Translate(FantasmasController.instance.KeepYPosition());

        if (GameManager2.instance.estadoActual.Equals(GameManager2.Estado.NivelSuperado))
        {
            ResetFruitLevelUp();
        }
        
        if (!GameManager2.instance.estadoActual.Equals(GameManager2.Estado.EnJuego))
        {
            return;
        }

        timer += Time.deltaTime;
        timerComeFruta += Time.deltaTime;

        if (timer > 2.0f)
        {
            agent.SetDestination(wayPoints[indexWayPoints]);

            if (!agent.pathPending && agent.remainingDistance <= distanceToCurrentWayPoint)
            {
                // Incrementa el  ndice, y con el operador m dulo resetea al llegar al final
                indexWayPoints = (indexWayPoints + 1) % GetWayPointsLENGTH();
            }
        }
    }

    private int GetWayPointsLENGTH()
    {
        return wayPoints.Length;
    }

    void OnTriggerEnter(Collider collider)
    {
        //print(collider.gameObject.transform.position);
        if (!collider.tag.Equals("Pacman") || timerComeFruta < 1.7f)
        {
            return;
        }

        timerComeFruta = 0.0f;
        timer = 0.0f;
        PlaySounds2.instance.PlaySonidos(sonidoEatingCherry);
        FrutasController.instance.EatFruit();

        BonusFrutasController.instance.InstanceBonusPoints(transform.position);
        BonusFrutasController.instance.AddPointsToScore();
        InstanceParticleSystem();

        Destroy(gameObject);

        print("**Bonus come-fruta**" + GameManager2.instance.GetPoints());
    }

    public void ResetFruitLevelUp()
    {
        timerComeFruta = 0.0f;
        timer = 0.0f;
        FrutasController.instance.EatFruit();

        Destroy(gameObject);
    }

    public void InstanceParticleSystem()
    {
        float DESTROY_TIMER = 0.8f;

        GameObject particles;
        particles = Instantiate(FrutasController.instance.preFabParticlesSystem2);
        particles.transform.position = transform.position;
        particles.transform.localScale = FrutasController.instance.sizeParticlesSystem;

        FrutasController.instance.ConfigParticlesSystem(particles);

        Destroy(particles, DESTROY_TIMER);
    }
}
