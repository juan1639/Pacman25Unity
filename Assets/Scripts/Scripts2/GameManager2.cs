using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 instance;

    [Header("GameObjects/PreFabs")]
    [SerializeField] private Transform pacman;
    [SerializeField] private GameObject pacmanInMenu;
    [SerializeField] private GameObject fantasmaInMenu;
    [SerializeField] private GameObject pills;
    [SerializeField] private GameObject preFabPacmanDies;
    [SerializeField] private GameObject preFabPacmanShowLives;

    private float DESTROY_TIMER_PACMAN_DIES = 3.1f;

    [Header("Archivos de audio")]
    [SerializeField] private AudioClip sonidoPreparado;
    [SerializeField] private AudioClip sonidoIntermision;
    [SerializeField] private AudioClip sonidoGameover;
    [SerializeField] private AudioClip sonidoVozGameOver;
    [SerializeField] private AudioClip sonidoApearFruit;
    [SerializeField] private AudioClip sonidoCongrats;
    [SerializeField] private AudioClip sonidoLevelPassed;
    
    private int level = 1;
    private int points = 0;
    private int hi = 12000;
    [Tooltip("Vidas Iniciales")]
    [SerializeField] private int initialLives = 3;
    private int lives;
    private List<GameObject> arrayLives = new List<GameObject>();

    public enum Estado
    {
        PreJuego,
        Preparado,
        EnJuego,
        Dying,
        GameOver,
        NivelSuperado
    }
    
    [Tooltip("Enum de Estados del juego")]
    public Estado estadoActual;

    float timer = 0f;

    private bool bandera = true;
    private bool banderaParticulasNivelSuperado = false;

    [Header("Sistema de particulas (nivel superado)")]
    [SerializeField] private GameObject preFabParticlesSystem;
    [SerializeField] private Vector3 sizeParticlesSystem = new Vector3(1f, 1f, 1f);

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        estadoActual = Estado.PreJuego;
    }

    void Start()
    {
        ResetValuesNewGame();
        PlaySounds2.instance.PlaySonidos(sonidoIntermision);
    }

    void Update()
    {
        CheckNivelSuperado();
        //CheckTxtPreparadoShow();
        TimeInc();
    }

    public void CheckNivelSuperado()
    {
        //print(pills.transform.childCount);

        if (pills.transform.childCount <= 0)
        {
            if (!banderaParticulasNivelSuperado)
            {
                banderaParticulasNivelSuperado = true;
                InstanciarParticulasNivelSuperado();
                PlaySounds2.instance.PlaySonidos(sonidoLevelPassed);
            }

            print("Nivel superado");

            estadoActual = Estado.NivelSuperado;
            ButtonsMenu.instance.txtNivelSuperado.gameObject.SetActive(true);
        }
    }

    private void TimeInc()
    {
        if (estadoActual.Equals(Estado.Preparado))
        {
            if (bandera)
            {
                bandera = false;

                pacmanInMenu.SetActive(false);
                fantasmaInMenu.SetActive(false);

                if (PlaySounds2.instance.audioSource.isPlaying)
                {
                    PlaySounds2.instance.audioSource.Stop();
                }

                PlaySounds2.instance.PlaySonidos(sonidoPreparado);
                Invoke("StartGame", 4.2f);
            }
        }
    }

    private void StartGame()
    {
        estadoActual = Estado.EnJuego;
        ButtonsMenu.instance.txtPreparado.gameObject.SetActive(false);
        print("EstadoActual: " + estadoActual);
    }

    public void InstancePacmanDies(Vector3 position)
    {
        GameObject pacmanDies;
        pacmanDies = Instantiate(preFabPacmanDies);
        pacmanDies.transform.position = position;
        //pacmanDies.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        //bonusPoints.transform.Translate(direction * VELOCITY * Time.deltaTime);
        //pacmanDies.GetComponent<Rigidbody>().AddForce(fruitPos.forward * VELOCITY);

        Destroy(pacmanDies, DESTROY_TIMER_PACMAN_DIES);
        //Invoke("ResetInitialPositionGhosts", DESTROY_TIMER_PACMAN_DIES);
        Invoke("SetPacmanToActive", DESTROY_TIMER_PACMAN_DIES + 1.0f);
    }

    public void SetPacmanToActive()
    {
        if (estadoActual.Equals(Estado.Dying))
        {
            SubstractLive();
        }

        if (!estadoActual.Equals(Estado.GameOver))
        {
            estadoActual = Estado.EnJuego;
        }

        PacmanController2.instance.transform.position = PacmanController2.instance.initialPosition;
        PacmanController2.instance.SetTryDirection(Vector3.zero);
        PacmanController2.instance.transform.gameObject.SetActive(true);
        PacmanController2.instance.InstanceParticleSystem();
        PlaySounds2.instance.PlaySonidos(sonidoApearFruit);

        FantasmasController.instance.IterateChildsRESETInitialPosition();
    }

    public void ResetInitialPositionGhosts()
    {
        FantasmasController.instance.IterateChildsRESETInitialPosition();
    }

    private void InstanceInitialLives(int lives, int total)
    {
        float posY = total;

        for (int i = 0; i < lives; i ++)
        {
            GameObject showLive;
            showLive = Instantiate(preFabPacmanShowLives);
            showLive.transform.position = new Vector3(1f, 1.5f, posY + 4);
            showLive.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            arrayLives.Add(showLive);
            posY ++;
        }
        
        print("Live:" + arrayLives[0]);
    }

    private void SubstractLive()
    {
        if (arrayLives.Count <= 0)
        {
            print("EstadoActual: " + estadoActual);

            estadoActual = Estado.GameOver;
            ButtonsMenu.instance.txtGameover.gameObject.SetActive(true);
            PlaySounds2.instance.PlaySonidos(sonidoGameover);
            Invoke("VozGameOver", 3.9f);

            if (CheckNewRecord())
            {
                hi = points;
                PlaySounds2.instance.PlaySonidos(sonidoCongrats);
            }

            return;
        }

        GameObject lastLive = arrayLives[arrayLives.Count - 1];
        Destroy(lastLive);
        arrayLives.RemoveAt(arrayLives.Count - 1);

        lives --;
        print("Vidas: " + lives);
    }

    private void ResetValuesNewGame()
    {
        estadoActual = Estado.PreJuego;

        lives = initialLives;
        InstanceInitialLives(initialLives, 0);
        
        level = 1;
        points = 0;

        timer = 0.0f;
        bandera = true;
    }

    public void PlayAnotherGame_ResetScene()
    {
        ResetValuesNewGame();
        WallsController2.instance.SetWallsHeight(0.0f);
        PillsController2.instance.DestroyAllPills();
        PillsController2.instance.InstanceAllPills();
        BigPillsController.instance.SetActiveBigPills();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void resetGame()
    {
        Process.Start("PacClon25.exe", "");
        Application.Quit();
    }

    private bool CheckNewRecord()
    {
        if (points >= hi)
        {
            print("*** Enhorabuena! Nuevo record! ***");
            return true;
        }

        return false;
    }

    private void VozGameOver()
    {
        PlaySounds2.instance.PlaySonidos(sonidoVozGameOver);
    }

    private void InstanciarParticulasNivelSuperado()
    {
        float DESTROY_TIMER = 5.5f;

        GameObject particles;
        particles = Instantiate(preFabParticlesSystem);
        particles.transform.position = pacman.position;
        particles.transform.localScale = sizeParticlesSystem;

        ConfigParticlesSystem(particles);

        Destroy(particles, DESTROY_TIMER);
    }

    private void ConfigParticlesSystem(GameObject particles)
    {
        var particleSystem = particles.GetComponent<ParticleSystem>();

        var main = particleSystem.main;
        main.duration = 5.0f;
        main.loop = true;
        main.startSpeed = 9.0f;
        main.gravityModifier = 2.0f;
        main.maxParticles = 2000;

        var emission = particleSystem.emission;
        emission.rateOverTime = 500;

        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.yellow, 0.0f),   // Azul al inicio
                new GradientColorKey(Color.white, 1.0f)   // Cyan al final
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0.0f),   // Alfa completo al inicio
                new GradientAlphaKey(0.9f, 1.0f)    // Medio-transparencia al final
            }
        );

        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetLevel(int nivel)
    {
        level = nivel;
    }

    public int GetPoints()
    {
        return points;
    }

    public void SetPoints(int ptos)
    {
        points = ptos;
    }

    public int GetLives()
    {
        return lives;
    }

    public void SetLives(int vidas)
    {
        lives = vidas;
    }

    public int GetHi()
    {
        return hi;
    }

    public void SetHi(int record)
    {
        hi = record;
    }

    public bool GetBanderaParticulasNivelSuperado()
    {
        return banderaParticulasNivelSuperado;
    }

    public void SetBanderaParticulasNivelSuperado(bool boolean)
    {
        banderaParticulasNivelSuperado = boolean;
    }
}
