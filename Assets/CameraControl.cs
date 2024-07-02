using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private Vector3 cameraOffset = new Vector3 (0, 2.5f, -10.0f);



    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + cameraOffset; 
    }
}
