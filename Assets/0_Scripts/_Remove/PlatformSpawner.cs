using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; 
    public Camera mainCamera;        

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnPrefabAtMousePosition();
        }
    }

    private void SpawnPrefabAtMousePosition()
    {
        
        Vector3 mouseScreenPosition = Input.mousePosition;

        
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0; 

        
        Instantiate(prefabToSpawn, mouseWorldPosition, Quaternion.identity);
    }
}
