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
    [SerializeField]
    private bool useController = false;


    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!useController)
        {
            if (Input.GetAxis("Vertical") != 0 && rb.velocity.magnitude < maxMovementSpeed)
            {
                rb.AddForce(transform.right * acceleration * Input.GetAxis("Vertical") * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                rotationAngle = rb.rotation - Input.GetAxis("Horizontal") * turningSpeed * Time.fixedDeltaTime;
                rb.MoveRotation(rotationAngle);
            }
        }
        else
        {
            if ((Input.GetAxis("VerticalController")+1)/2f != 0 && rb.velocity.magnitude < maxMovementSpeed)
            {
                rb.AddForce(transform.right * acceleration * (Input.GetAxis("VerticalController") + 1) / 2f * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            if (Input.GetAxis("HorizontalController") != 0)
            {
                rotationAngle = rb.rotation - Input.GetAxis("Horizontal") * turningSpeed * Time.fixedDeltaTime;
                rb.MoveRotation(rotationAngle);
            }
        }


    }

    
}
