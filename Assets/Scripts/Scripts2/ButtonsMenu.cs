using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonsMenu : MonoBehaviour
{
    public static ButtonsMenu instance;

    public Transform mainMenuItems;
    public Transform txtPreparado;
    public Transform marcadores;
    public Transform txtGameover;
    public Transform txtNivelSuperado;
    public Transform insideSettings;

    public AudioClip selectOption;
    public AudioClip returnOption;

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
        mainMenuItems = transform.Find("MainMenuItems");
        insideSettings = transform.Find("InsideSettings");
        ResetCanvas();
        PacmanController2.instance.HideMobileControls(0.0f);
        PacmanController2.instance.HideMobileControls2(false);
    }

    void Update()
    {
        
    }

    public void ComenzarPartida()
    {
        GameManager2.instance.estadoActual = GameManager2.Estado.Preparado;

        print("Comenzar partida");
        print("EstadoActual: " + GameManager2.instance.estadoActual);

        mainMenuItems = transform.Find("MainMenuItems");
        mainMenuItems.gameObject.SetActive(false);

        txtPreparado.gameObject.SetActive(true);
        marcadores.gameObject.SetActive(true);

        PlaySounds2.instance.PlaySonidos(selectOption);
    }

    public void Settings()
    {
        print("Settings");

        ToggleSettingsInsideSettings(false);
        PlaySounds2.instance.PlaySonidos(selectOption);
    }

    public void BotonExit()
    {
        PlaySounds2.instance.PlaySonidos(selectOption);
        Application.Quit();
    }

    public void SetMobileControls()
    {
        PacmanController2.instance.SetMobileControls(true);
        ToggleSettingsInsideSettings(true);
        PacmanController2.instance.HideMobileControls(0.8f);
        PacmanController2.instance.HideMobileControls2(false);
    }

    public void SetMobileControls2()
    {
        PacmanController2.instance.SetMobileControls(true);
        ToggleSettingsInsideSettings(true);
        PacmanController2.instance.HideMobileControls2(true);
        PacmanController2.instance.HideMobileControls(0.0f);
    }

    public void SetDefaultCursorControls()
    {
        PacmanController2.instance.SetMobileControls(false);
        ToggleSettingsInsideSettings(true);
        PacmanController2.instance.HideMobileControls(0.0f);
        PacmanController2.instance.HideMobileControls2(false);
    }

    public void OtraPartida()
    {
        print("*** otra partida ***");

        //GameManager2.instance.resetGame();
        ResetCanvas();
        GameManager2.instance.PlayAnotherGame_ResetScene();
        PlaySounds2.instance.PlaySonidos(selectOption);
    }

    public void ResetCanvas()
    {
        mainMenuItems.gameObject.SetActive(true);

        txtPreparado = transform.Find("TxtPreparado");
        txtPreparado.gameObject.SetActive(false);

        marcadores = transform.Find("Marcadores");
        marcadores.gameObject.SetActive(false);

        txtGameover = transform.Find("TxtGameover");
        txtGameover.gameObject.SetActive(false);

        txtNivelSuperado = transform.Find("NivelSuperado");
        txtNivelSuperado.gameObject.SetActive(false);

        insideSettings.gameObject.SetActive(false);
    }

    public void NextLevel()
    {
        WallsController2.instance.SetWallsHeight((float)GameManager2.instance.GetLevel());

        GameManager2.instance.SetLevel(GameManager2.instance.GetLevel() + 1);

        txtNivelSuperado = transform.Find("NivelSuperado");
        txtNivelSuperado.gameObject.SetActive(false);
        GameManager2.instance.SetBanderaParticulasNivelSuperado(false);

        PillsController2.instance.InstanceAllPills();
        BigPillsController.instance.SetActiveBigPills();
        GameManager2.instance.SetPacmanToActive();

        PlaySounds2.instance.PlaySonidos(selectOption);
    }

    public void ToggleSettingsInsideSettings(bool boolean)
    {
        mainMenuItems.gameObject.SetActive(boolean);
        insideSettings.gameObject.SetActive(!boolean);

        if (boolean)
        {
            PlaySounds2.instance.PlaySonidos(returnOption);
        }
        else
        {
            PlaySounds2.instance.PlaySonidos(selectOption);
        }
    }
}
