using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsController0 : MonoBehaviour
{
    public static WallsController0 instance;

    private GameObject[] allChildren;
    private int numberOfChilds;

    [SerializeField] private float wallsHeight = 1.0f;
    [SerializeField] private float floorHeight = 1.0f;

    void Awake()
    {
        // SINGLETON (nos aseguramos de que solo haya una instancia de esta clase)
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
        SetWallsHeight();
        
    }

    void Update()
    {
        
    }

    private void SetWallsHeight()
    {
        allChildren = new GameObject[transform.childCount];
        numberOfChilds = allChildren.Length;

        for (int i = 0; i < numberOfChilds; i ++)
        {
            allChildren[i] = transform.GetChild(i).gameObject;

            if (i != 4)
            {
                allChildren[i].transform.localScale = new Vector3(1f, wallsHeight, 1f);
            }
            else if (i == 4)
            {
                allChildren[i].transform.localScale = new Vector3(1f, floorHeight, 1f);
            }
        }

    }
}
