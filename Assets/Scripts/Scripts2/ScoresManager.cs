using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoresManager : MonoBehaviour
{
    public static ScoresManager instance;

    [SerializeField] private Text pointsText; // Componente UI.Text
    [SerializeField] private TMP_Text pointsTMP; // Componente TextMeshPro (opcional)
    [SerializeField] private Text levelText; // Componente UI.Text
    [SerializeField] private TMP_Text levelTMP; // Componente TextMeshPro (opcional)
    [SerializeField] private Text hiText; // Componente UI.Text
    [SerializeField] private TMP_Text hiTMP; // Componente TextMeshPro (opcional)

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
        if (pointsText == null && pointsTMP == null)
        {
            Debug.LogError("No se ha asignado ningún componente de texto al *** Score-points ***");
        }

        if (levelText == null && levelTMP == null)
        {
            Debug.LogError("No se ha asignado ningún componente de texto al *** Score-level ***");
        }

        if (hiText == null && hiTMP == null)
        {
            Debug.LogError("No se ha asignado ningún componente de texto al *** Score-hiScore ***");
        }
    }

    void Update()
    {
        CanvasShowPoints();
        CanvasShowLevel();
        CanvasShowHi();
    }

    private void CanvasShowPoints()
    {
        if (pointsText != null)
        {
            pointsText.text = $"Score: {GameManager2.instance.GetPoints()} ";
        }

        if (pointsTMP != null)
        {
            pointsTMP.text = $"Score: {GameManager2.instance.GetPoints()} ";
        }

    }

    private void CanvasShowLevel()
    {
        if (levelText != null)
        {
            levelText.text = $"Level: {GameManager2.instance.GetLevel()} ";
        }

        if (levelTMP != null)
        {
            levelTMP.text = $"Level: {GameManager2.instance.GetLevel()} ";
        }
    }

    private void CanvasShowHi()
    {
        if (hiText != null)
        {
            hiText.text = $"Hi: {GameManager2.instance.GetHi()} ";
        }

        if (hiTMP != null)
        {
            hiTMP.text = $"Hi: {GameManager2.instance.GetHi()} ";
        }
    }
}
