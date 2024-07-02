using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float thrust;
    public float rotationSpeed;
    public float maxSpeed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        HandleThrust();
        HandleRotation();
    }

    private void HandleThrust()
    {
        if (InputManager.instance.ImpulseActionPressed() && rb.velocity.y < maxSpeed)
        {
            rb.AddRelativeForce(Vector3.up * thrust);
        }
    }

    private void HandleRotation()
    {
        float rotation = 0f;

        if (InputManager.instance.LeftRotationPressed())
        {
            rotation = rotationSpeed;
        }
        else if (InputManager.instance.RightRotationPressed())
        {
            rotation = -rotationSpeed;
        }

        rb.AddTorque(rotation * Time.deltaTime);
    }
}
