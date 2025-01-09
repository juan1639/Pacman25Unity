using System;
using System.Collections;
using System.Collections.Generic;
//using System.Security.Cryptography;
using UnityEngine;

public class PacmanController2 : MonoBehaviour
{
    public static PacmanController2 instance;

    [Tooltip("Modelo de blender (Posibles ejes cambiados)")]
    [SerializeField] private bool modelFromBlender = true;
    [SerializeField] private bool invisiblePacman = false;
    [Tooltip("Posicion inicial Pacman")]
    public Vector3 initialPosition = new Vector3(-9.5f, 1.5f, 4.5f);

    private Vector3 tryDirection;
    private Vector3 direction;

    [Header("Velocidad/velRotacion de Pacman")]
    [SerializeField] private float PACMAN_VELOCITY = 2.0f;
    [SerializeField] private float rotationSpeed = 9.0f;
    private Quaternion targetRotation;
    private int pacmanDirection = 0;

    [Tooltip("Sistema particulas cuando aparece Pacman")]
    [SerializeField] private GameObject preFabParticlesSystem;
    private GameObject particlesVELOCITYTURBO;

    [Header("Mobile Joystick")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject mobileButtons;
    private bool mobileControls = false;

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
        initialPosition = transform.position;

        tryDirection = Vector3.zero;
        direction = tryDirection;

        PACMAN_VELOCITY = 2.0f;
    }

    void Update()
    {
        TeclaExit();

        // 0 PreJuego | 1 Preparado | 2 EnJuego | 3 Dying | 4 GameOver | 5 NivelSuperado
        if ((int)GameManager2.instance.estadoActual != 2)
        {
            if (((int)GameManager2.instance.estadoActual == 0 && !invisiblePacman) || (GameManager2.instance.estadoActual.Equals(GameManager2.Estado.GameOver) && !invisiblePacman))
            {
                invisiblePacman = true;
                IterateChilds(false);
                return;
            }

            if ((int)GameManager2.instance.estadoActual == 1 && invisiblePacman)
            {
                invisiblePacman = false;
                IterateChilds(true);
                targetRotation = Quaternion.Euler(-90f, 0f, 0f);
                return;
            }

            tryDirection = Vector3.zero;
            return;
        }

        PACMAN_VELOCITY = ChangePacmanVelocityIfBlueGhost();

        PacmanControls();
        PacmanControlsJoystick();
        MovePacman();
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        ParticlesFollowPacmanVELOCITYTURBO();
    }

    private void PacmanControls()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tryDirection = ChooseVector3(-1f);
            targetRotation = Quaternion.Euler(-90f, 0f, 180f);
            pacmanDirection = 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tryDirection = ChooseVector3(-1f);
            targetRotation = Quaternion.Euler(-90f, 0f, 0f);
            pacmanDirection = 2;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tryDirection = new Vector3(0f, -1f, 0f);
            targetRotation = Quaternion.Euler(-90f, 0f, 90f);
            pacmanDirection = 3;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tryDirection = new Vector3(0f, -1f, 0f);
            targetRotation = Quaternion.Euler(-90f, 0f, -90f);
            pacmanDirection = 4;
        }
    }

    private void PacmanControlsJoystick()
    {
        if (joystick.Vertical >= 0.5)
        {
            tryDirection = ChooseVector3(-1f);
            targetRotation = Quaternion.Euler(-90f, 0f, 180f);
            pacmanDirection = 1;
            return;
        }

        if (joystick.Vertical <= -0.5)
        {
            tryDirection = ChooseVector3(-1f);
            targetRotation = Quaternion.Euler(-90f, 0f, 0f);
            pacmanDirection = 2;
            return;
        }

        if (joystick.Horizontal <= -0.5)
        {
            tryDirection = new Vector3(0f, -1f, 0f);
            targetRotation = Quaternion.Euler(-90f, 0f, 90f);
            pacmanDirection = 3;
            return;
        }

        if (joystick.Horizontal >= 0.5)
        {
            tryDirection = new Vector3(0f, -1f, 0f);
            targetRotation = Quaternion.Euler(-90f, 0f, -90f);
            pacmanDirection = 4;
            return;
        }
    }

    public void TouchButtonUP()
    {
        tryDirection = ChooseVector3(-1f);
        targetRotation = Quaternion.Euler(-90f, 0f, 180f);
        pacmanDirection = 1;
    }

    public void TouchButtonDO()
    {
        tryDirection = ChooseVector3(-1f);
        targetRotation = Quaternion.Euler(-90f, 0f, 0f);
        pacmanDirection = 2;
    }

    public void TouchButtonRI()
    {
        tryDirection = new Vector3(0f, -1f, 0f);
        targetRotation = Quaternion.Euler(-90f, 0f, 90f);
        pacmanDirection = 3;
    }

    public void TouchButtonLE()
    {
        tryDirection = new Vector3(0f, -1f, 0f);
        targetRotation = Quaternion.Euler(-90f, 0f, -90f);
        pacmanDirection = 4;
    }

    private void MovePacman()
    {
        direction = tryDirection;
        transform.Translate(direction * PACMAN_VELOCITY * Time.deltaTime);
        //Debug.Log(transform.position);
    }

    private float ChangePacmanVelocityIfBlueGhost()
    {
        if (FantasmasController.instance.GetBlueGhost())
        {
            return 3.5f;// VELOCIDAD TURBO
        }

        return 2.0f;// VELOCIDAD NORMAL
    }

    private void IterateChilds(bool boolean)
    {
        foreach (Transform child in transform)
        {
            
            Renderer childRenderer = child.GetComponent<Renderer>();

            if (childRenderer != null)
            {
                //childRenderer.material.color = Color.black;
                childRenderer.enabled = boolean;
            }

            foreach (Transform child2 in child)
            {
                Renderer childRenderer2 = child2.GetComponent<Renderer>();

                if (childRenderer2 != null)
                {
                    //childRenderer.material.color = Color.black;
                    childRenderer2.enabled = boolean;
                }
            }
        }
    }

    private Vector3 ChooseVector3(float upDown)
    {
        if (modelFromBlender)
        {
            return new Vector3(0f, 1f * upDown, 0f);
        }
        else
        {
            return new Vector3(0f, 0f, 1f * upDown);
        }
    }
    
    void TeclaExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //print(collision.gameObject.name);
    }

    public Vector3 GetTryDirection()
    {
        return tryDirection;
    }

    public void SetTryDirection(Vector3 vector)
    {
        tryDirection = vector;
    }

    public void InstanceParticleSystem()
    {
        float DESTROY_TIMER = 1.7f;

        GameObject particles;
        particles = Instantiate(preFabParticlesSystem);
        particles.transform.position = transform.position;
        particles.transform.localScale = new Vector3(1f, 1f, 1f);

        Destroy(particles, DESTROY_TIMER);
    }

    public void InstanceParticleSystemVELOCITYTURBO()
    {
        float blueGhostDuration = FantasmasController.instance.GetBlueGhostDuration() - (float)GameManager2.instance.GetLevel();
        float DESTROY_TIMER = blueGhostDuration;

        particlesVELOCITYTURBO = Instantiate(FrutasController.instance.preFabParticlesSystem2);
        particlesVELOCITYTURBO.transform.position = transform.position;
        particlesVELOCITYTURBO.transform.localScale = FrutasController.instance.sizeParticlesSystem;

        ConfigParticlesSystem(particlesVELOCITYTURBO, DESTROY_TIMER);

        Destroy(particlesVELOCITYTURBO, DESTROY_TIMER);
    }

    private void ConfigParticlesSystem(GameObject particles, float DESTROY_TIMER)
    {
        var particleSystem = particles.GetComponent<ParticleSystem>();
        particleSystem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        var main = particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.duration = DESTROY_TIMER;
        main.maxParticles = 3000;

        var emission = particleSystem.emission;
        emission.enabled = true;
        emission.rateOverTime = 1200;
        
        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.red, 0.0f),   // Rojo al inicio
                new GradientColorKey(Color.yellow, 0.5f),
                new GradientColorKey(Color.yellow, 1.0f)   // Amarillo al final
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0.0f),   // Alfa completo al inicio
                new GradientAlphaKey(0.6f, 1.0f)    // Medio-transparencia al final
            }
        );

        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        var sizeOverLifetime = particleSystem.sizeOverLifetime;
        sizeOverLifetime.enabled = true;

        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0.0f, 0.7f); // Tamaño pequeño al inicio
        sizeCurve.AddKey(0.5f, 0.5f); // Tamaño completo a mitad de vida
        sizeCurve.AddKey(1.0f, 0.1f); // Tamaño reducido al final

        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, sizeCurve);
    }

    private void ParticlesFollowPacmanVELOCITYTURBO()
    {
        if (!FantasmasController.instance.GetBlueGhost())
        {
            return;
        }
        
        particlesVELOCITYTURBO.transform.position = transform.position;
        particlesVELOCITYTURBO.transform.rotation = transform.rotation;
    }

    public void HideMobileControls(float alpha)
    {
        UnityEngine.UI.Image image = joystick.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image image2 = joystick.transform.Find("Handle").GetComponent<UnityEngine.UI.Image>();

        Color handleColor = image.color;
        handleColor.a = alpha;
        image.color = handleColor;

        Color handleColor2 = image2.color;
        handleColor2.a = alpha;
        image2.color = handleColor2;
    }

    public void HideMobileControls2(bool boolean)
    {
        mobileButtons.SetActive(boolean);
    }

    public bool GetMobileControls()
    {
        return mobileControls;
    }

    public void SetMobileControls(bool boolean)
    {
        mobileControls = boolean;
    }
}
