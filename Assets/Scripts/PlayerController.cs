using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxMovementSpeed, acceleration;
    [SerializeField]
    private float turningSpeed;

    private float rotationAngle = 0f;


    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") != 0 && rb.velocity.magnitude < maxMovementSpeed)
        {
            rb.AddForce(transform.right * acceleration * Input.GetAxis("Vertical") * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            rotationAngle =  rb.rotation -Input.GetAxis("Horizontal") * turningSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rotationAngle);
        }
    }

    
}
