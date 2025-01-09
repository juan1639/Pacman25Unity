using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowPacman : MonoBehaviour
{
    [Tooltip("GameObject al que seguira la camara (Pacman)")]
    [SerializeField] private Transform target;

    [Tooltip("Distancia de la camara al objetivo")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 12f, 0f);

    [Tooltip("Limite inferior-izquierdo (a partir de aqui la camara se para)")]
    [SerializeField] private Vector2 minBounds;


    [Tooltip("Limite superior-derecho (a partir de aqui la camara se para)")]
    [SerializeField] private Vector2 maxBounds;

    private float heightCameraControl = 0.0f;

    void Start()
    {
        
    }
    
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // Calcular la posición deseada de la cámara con el offset.
        Vector3 desiredPosition = target.position + offset + new Vector3(0f, heightCameraControl, 0f);

        // Restringir la posición de la cámara dentro de los límites.
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedZ = Mathf.Clamp(desiredPosition.z, minBounds.y, maxBounds.y);

        // Actualizar la posición de la cámara.
        //transform.position = new Vector3(clampedX, desiredPosition.y, clampedZ);
        transform.position = new Vector3(clampedX, desiredPosition.y, clampedZ);
        
        //print(transform.name);
    }

    public void SliderChange(float value)
    {
        //print("Slider:" + value);
        heightCameraControl = value;
    }

    public float GetHeightCameraControl()
    {
        return heightCameraControl;
    }

    public void SetHeightCameraControl(float variation)
    {
        heightCameraControl = variation;
    }
}
