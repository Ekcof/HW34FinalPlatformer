using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjectByClickScript : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreatePrefab();
        }
    }

    private void CreatePrefab()
    {
        Vector3 worldPoint = Input.mousePosition;
        worldPoint.z = Mathf.Abs(cam.transform.position.z);
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(worldPoint);
        mouseWorldPosition.z = 0f;
        Instantiate(prefab, mouseWorldPosition, Quaternion.identity);
    }
}