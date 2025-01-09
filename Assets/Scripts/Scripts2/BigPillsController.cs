using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPillsController : MonoBehaviour
{
    public static BigPillsController instance;

    [SerializeField] private float emissionIntensity = 3.0f;

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
        ConfigureBigPills();
    }

    void Update()
    {
        
    }

    private void ConfigureBigPills()
    {
        foreach (Transform child in transform)
        {
            if (child.name.EndsWith("Pill"))
            {
                child.position = new Vector3(-1.5f, 1.5f, 1.5f);
            }
            else if (child.name.EndsWith("Pill (1)"))
            {
                child.position = new Vector3(-17.5f, 1.5f, 1.5f);
            }
            else if (child.name.EndsWith("Pills (2)"))
            {
                child.position = new Vector3(-1.5f, 1.5f, 13.5f);
            }
            else if (child.name.EndsWith("Pills (3)"))
            {
                child.position = new Vector3(-17.5f, 1.5f, 13.5f);
            }
        }
    }

    public void SetActiveBigPills()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
