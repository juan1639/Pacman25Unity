using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsManager : MonoBehaviour
{
    public static WayPointsManager instance;

    [SerializeField] private Vector3[] arrayWayPoints = new Vector3[5];
    private Color newColor;

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
        CreateWayPointsVector3();
    }

    void Update()
    {
        
    }

    private void CreateWayPointsVector3()
    {
        arrayWayPoints[0] = new Vector3(-1.5f, 1.5f, 1.5f);
        arrayWayPoints[1] = new Vector3(-17.5f, 1.5f, 1.5f);
        arrayWayPoints[2] = new Vector3(-9.5f, 1.5f, 8.5f);
        arrayWayPoints[3] = new Vector3(-1.5f, 1.5f, 13.5f);
        arrayWayPoints[4] = new Vector3(-17.5f, 1.5f, 13.5f);
    }

    public void HideWayPoints()
    {
        newColor = Color.red;

        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();

            if (childRenderer != null)
            {
                //childRenderer.material.color = newColor;
                childRenderer.enabled = false;
            }
        }
    }

    public Vector3[] GetWayPointsArray()
    {
        return arrayWayPoints;
    }
}
