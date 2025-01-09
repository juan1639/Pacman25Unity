using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPacman0 : MonoBehaviour
{
    [SerializeField] private Transform pacman;

    [SerializeField] private Vector3 offSet;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    void LateUpdate()
    {
        if (pacman == null)
        {
            return;
        }

        Vector3 desiredPosition = pacman.position + offSet;

        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedZ = Mathf.Clamp(desiredPosition.z, minBounds.y, maxBounds.y);

        transform.position = new Vector3(clampedX, desiredPosition.y, clampedZ);
    }
}
