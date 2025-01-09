using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusFrutasController : MonoBehaviour
{
    public static BonusFrutasController instance;

    [Header("Arrays de Show-bonus (gameObjects/integers)")]
    [SerializeField] private GameObject[] bonusArray;
    [SerializeField] private int[] addBonusArray = new int[7]
    {
        200, 200, 400, 800, 1600, 3000, 5000
    };

    [Tooltip("Direccion del show-bonus (hacia arriba)")]
    [SerializeField] private Vector3 direction = new Vector3(0f, 1f, 0f);

    [Tooltip("Velocidad del show-bonus")]
    [SerializeField] private float VELOCITY = 75.0f;

    [Tooltip("Duracion del show-bonus")]
    [SerializeField] private float DESTROY_TIMER = 3.1f;

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
        
    }

    void Update()
    {
        
    }

    public void InstanceBonusPoints(Vector3 fruitPosition)
    {
        GameObject bonusPoints;
        bonusPoints = Instantiate(SelectPoints());
        bonusPoints.transform.position = fruitPosition;
        bonusPoints.transform.localScale = new Vector3(1f, 1f, 1f);
        //bonusPoints.transform.Translate(direction * VELOCITY * Time.deltaTime);
        bonusPoints.GetComponent<Rigidbody>().AddForce(transform.up * VELOCITY);

        //Add force on casing to push it out
        //tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        //tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        Destroy(bonusPoints, DESTROY_TIMER);
    }

    private GameObject SelectPoints()
    {
        return bonusArray[GameManager2.instance.GetLevel()];
    }

    public void AddPointsToScore()
    {
        int currentPoints = GameManager2.instance.GetPoints();
        int maxPoints = addBonusArray[addBonusArray.Length - 1];
        int currentLevel = GameManager2.instance.GetLevel(); 

        if (currentLevel >= addBonusArray.Length)
        {
            GameManager2.instance.SetPoints(currentPoints + addBonusArray[maxPoints]);
        }
        else
        {
            GameManager2.instance.SetPoints(currentPoints + addBonusArray[currentLevel]);
        }
    }
}
