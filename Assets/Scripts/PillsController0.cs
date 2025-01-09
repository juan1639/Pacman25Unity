using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillsController0 : MonoBehaviour
{
    public static PillsController0 instance;

    const int ESCENARIO_HEIGHT = 15;
    const int ESCENARIO_WIDTH = 19;

    public static int[,] arrayEscenario = new int[ESCENARIO_HEIGHT, ESCENARIO_WIDTH]
    {
        {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9},
        {9,5,1,1,1,1,1,1,1,9,1,1,1,1,1,1,1,5,9},
        {9,1,9,9,1,9,9,9,1,9,1,9,9,9,1,9,9,1,9},
    
        {9,1,9,9,1,9,9,9,1,9,1,9,9,9,1,9,9,1,9},
        {9,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,9},
        {9,1,9,9,1,9,1,9,9,9,9,9,1,9,1,9,9,1,9},
    
        {9,1,1,1,1,9,1,1,1,9,1,1,1,9,1,1,1,1,9},
        {9,9,9,9,1,9,9,9,1,9,1,9,9,9,1,9,9,9,9},
        {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,9},
    
        {9,1,9,9,1,9,1,9,1,9,1,9,1,9,1,9,9,1,9},
        {9,1,9,9,1,9,1,9,1,9,1,9,1,9,1,9,9,1,9},
        {0,1,1,1,1,9,1,1,1,1,1,1,1,9,1,1,1,1,0},
    
        {9,1,9,9,1,9,1,9,9,9,9,9,1,9,1,9,9,1,9},
        {9,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,9},
        {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9},
    };

    private int PARED = 9;
    private int PILL = 1;
    private int BIG_PILL = 5;
    private int EMPTY = 0;

    public GameObject childObject;
    private GameObject instanciaObject;
    private List<GameObject> allChildren = new List<GameObject>();
    private int numeroChilds;

    [SerializeField] private int POINTS = 10;

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
        InstanceAllPills();
    }

    void Update()
    {
        
    }

    public void InstanceAllPills()
    {
        float SIZE = 0.15f;
        float Y_POS = 1.4f;
        
        for (int y = 0; y < ESCENARIO_HEIGHT; y ++)
        {
            for (int x = 0; x < ESCENARIO_WIDTH; x ++)
            {
                if (arrayEscenario[y, x] == PILL)
                {
                    instanciaObject = Instantiate(childObject);

                    float zPos = y + 0.5f;
                    float xPos = (x + 0.5f) * -1;
                    instanciaObject.transform.position = new Vector3(xPos, Y_POS, zPos);
                    instanciaObject.transform.localScale = new Vector3(SIZE, SIZE, SIZE);

                    allChildren.Add(instanciaObject);
                }
            }
        }

        // número de puntitos/pills que hay en total
        numeroChilds = allChildren.Count;
    }

    public void DestroyAllPills()
    {
        foreach (GameObject pill in allChildren)
        {
            if (pill != null)
            {
                Destroy(pill);
            }
        }
    }

    public void AddPointsToScore()
    {
        // aqui se suman los puntos
    }
}
