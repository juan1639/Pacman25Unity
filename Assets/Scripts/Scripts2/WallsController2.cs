using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsController2 : MonoBehaviour
{
    public static WallsController2 instance;

    private GameObject[] allChildren;
    private int numeroChilds;

    [Tooltip("The height of the walls")]
    public float wallsHeight;

    [Tooltip("The height/width of the floor")]
    public float floorHeight;

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
        SetWallsHeight(0.0f);
    }

    void Update()
    {
        
    }

    public void SetWallsHeight(float incWallsHeight)
    {
        float MAX_WALLS_HEIGHT = 3.0f;

        wallsHeight = 0.7f + (incWallsHeight / 2.0f);
        floorHeight = 1.0f;

        if (wallsHeight >= MAX_WALLS_HEIGHT)
        {
            wallsHeight = MAX_WALLS_HEIGHT;
        }

        allChildren = new GameObject[transform.childCount];
        numeroChilds = allChildren.Length;

        for (int i = 0; i < numeroChilds; i ++)
        {
            allChildren[i] = transform.GetChild(i).gameObject;

            if (i != 4)
            {
                allChildren[i].transform.localScale = new Vector3(1, wallsHeight, 1);
            }
            else if (i == 4)
            {
                allChildren[i].transform.localScale = new Vector3(1, floorHeight, 1);
            }
        }
    }
}
