using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RacerController), typeof(Rigidbody2D))]
public class NPCController : MonoBehaviour
{
    private Vector3 inputDirection = new Vector3();
    private RacerController racerMovement;
    private Vector3 nextCheckpoint;
    private Rigidbody2D rb;

    [SerializeField]
    private float maxMovementSpeed, acceleration;
    [SerializeField]
    private float turningSpeed;

    private float rotationAngle = 0f;
    private GameManager gm;

    // Start is called before the first frame update
    void Awake()
    {
        racerMovement = GetComponent<RacerController>();
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gm.isRacing)
        {
            nextCheckpoint = racerMovement.gm.checkPoints[racerMovement.currentCheckPoint].transform.position;

            inputDirection.z = 1;    // = Vector3.Scale(transform.rotation * (nextCheckpoint - transform.position).normalized, new Vector3(1,1,-1));
            inputDirection.x = Vector3.Dot((nextCheckpoint - transform.position).normalized, transform.up);

            Debug.DrawLine(transform.position, transform.position + transform.rotation * inputDirection, Color.red);
            Debug.DrawLine(transform.position, nextCheckpoint, Color.green);

            if (rb.velocity.magnitude < maxMovementSpeed) rb.AddForce(transform.right * acceleration * inputDirection.z * Time.fixedDeltaTime, ForceMode2D.Impulse);
            rotationAngle = rb.rotation + inputDirection.x * turningSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rotationAngle);
        }

    }
}
