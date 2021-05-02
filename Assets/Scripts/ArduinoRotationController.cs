using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArduinoRotationController : MonoBehaviour
{
    [SerializeField]
    private ArduinoConnector connector;
    [SerializeField]
    private Vector2 minMaxRecievedValue = new Vector2(0, 1023);

    [SerializeField]
    private float maxMovementSpeed, acceleration;
    [SerializeField]
    private float turningSpeed;

    private float rotationAngle = 0f;

    private Vector3 inputDirection = new Vector3();
    private Rigidbody2D rb;
    private GameManager gm;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (connector && gm)
        {
            if (connector.recieveData && connector.useArduino && gm.isRacing)
            {
                inputDirection.z = 1;    // = Vector3.Scale(transform.rotation * (nextCheckpoint - transform.position).normalized, new Vector3(1,1,-1));
                inputDirection.x = Vector3.Dot(Quaternion.Euler(0, 0, connector.recievedDirection*360f/1023f) * Vector3.up, transform.up);

                if (rb.velocity.magnitude < maxMovementSpeed) rb.AddForce(transform.right * acceleration * inputDirection.z * Time.fixedDeltaTime, ForceMode2D.Impulse);
                rotationAngle = rb.rotation + inputDirection.x * turningSpeed * Time.fixedDeltaTime;
                rb.MoveRotation(rotationAngle);
            }
        }
    }

}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
