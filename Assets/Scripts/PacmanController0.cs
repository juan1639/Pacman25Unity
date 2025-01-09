using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanController0 : MonoBehaviour
{
    public static PacmanController0 instance;

    [SerializeField] private bool invisiblePacman = false;

    public Vector3 initialPosition = new Vector3(-9.5f, 1.5f, 4.5f);

    private Vector3 direction;

    [SerializeField] private float PACMAN_VELOCITY = 2.0f;
    [SerializeField] private float rotationSpeed = 9.0f;
    private Quaternion targetRotation;

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
        //initialPosition = transform.position;
        direction = Vector3.zero;

        PACMAN_VELOCITY = 2.0F;
    }

    void Update()
    {
        PacmanControls();
        MovePacman();
        RotatePacman();
    }

    private void PacmanControls()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = new Vector3(0f, 0f, -1f);
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = new Vector3(0f, 0f, -1f);
            targetRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = new Vector3(0f, 0f, -1f);
            targetRotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = new Vector3(0f, 0f, -1f);
            targetRotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }

    private void MovePacman()
    {
        transform.Translate(direction * PACMAN_VELOCITY * Time.deltaTime);
    }

    private void RotatePacman()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
