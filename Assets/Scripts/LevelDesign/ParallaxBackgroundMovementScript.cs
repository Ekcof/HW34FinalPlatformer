using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundMovementScript : MonoBehaviour
{
    [SerializeField] private float parallaxCoefficient;
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 deltaMovement = new Vector3 (cameraTransform.position.x - previousCameraPosition.x, cameraTransform.position.y - previousCameraPosition.y, 0);
        transform.position += deltaMovement * parallaxCoefficient;
        previousCameraPosition = new Vector3 (transform.position.x, transform.position.y, 0);
    }
}
