using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;
    }

    void Update()
    {
        processInput();
        keepPlayerOnScreen();
        rotateToFaceVelocity();
    }

    void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }
        
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void processInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector3 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            touchPosition.z = 10f;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            

            
            movementDirection = transform.position - worldPosition;
            movementDirection.z = 0f;
            movementDirection.Normalize();
            }
        else
        {
            movementDirection = Vector3.zero;
        }
    }

    private void keepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewPortPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewPortPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if(viewPortPosition.x < 0 )
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        if (viewPortPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if(viewPortPosition.y < 0 )
        {
            newPosition.y = -newPosition.y - 0.1f;
        }
        
        transform.position = newPosition;
    }

    private void rotateToFaceVelocity()
    {
        if (rb.velocity == Vector3.zero)
        {
            return;
            
        }
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity,Vector3.back);
        Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}