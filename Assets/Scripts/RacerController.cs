using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerController : MonoBehaviour
{
    public int currentPlace = -1;
    public int currentCheckPoint = 0;
    public int currentLap = 0;
    public bool finished = false;
    public float raceDistance = 0;
    public float forwardDotProduct = 1f;
    [HideInInspector]
    public GameManager gm;

    [HideInInspector]
    public Vector3 nextCheckPoint = Vector3.zero;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("GameController"))
        {
            gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gm) UpdateDistance();
    }

    private void UpdateDistance()
    {
        raceDistance = 0;
        nextCheckPoint = gm.checkPoints[currentCheckPoint].transform.position;

        if (currentCheckPoint > 0)
        {
            Vector3 lastCheck = gm.checkPoints[currentCheckPoint - 1].transform.position;

            float dist = (transform.position - lastCheck).magnitude;
            raceDistance = dist * Mathf.Cos(Mathf.Deg2Rad * Vector3.SignedAngle(transform.position - lastCheck, nextCheckPoint - lastCheck, Vector3.up));

            Debug.DrawLine(transform.position, (nextCheckPoint - lastCheck).normalized * raceDistance + lastCheck);
        }
        else
        {
            float dist = (transform.position).magnitude;
            raceDistance = dist * Mathf.Cos(Mathf.Deg2Rad * Vector3.SignedAngle(transform.position, nextCheckPoint, Vector3.up));

            Debug.DrawLine(transform.position, (nextCheckPoint).normalized * raceDistance);
        }

        CheckForward(nextCheckPoint - transform.position);
    }

    private void CheckForward(Vector3 direction)
    {
        forwardDotProduct = Vector3.Dot(direction, transform.right);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("entered trigger");
        if (gm)
        {
            if (other.tag == "CheckPoint")
            {

                if (other.gameObject == gm.checkPoints[currentCheckPoint])
                {

                    if (currentCheckPoint == gm.checkPoints.Length - 1)
                    {
                        Debug.Log("Entered all the triggers, finishing a lap: ");
                        currentCheckPoint = 0;
                        currentLap++;

                        if (currentLap >= gm.nrOfLaps)
                        {
                            finished = true;
                            Debug.Log(gameObject.name + " has Finished!");
                            //currentLap = 0;
                        }

                        return;
                    }

                    currentCheckPoint = Mathf.Min(gm.checkPoints.Length - 1, currentCheckPoint + 1);
                    Debug.Log("Entered trigger, total triggers entered: " + currentCheckPoint);

                }
                else Debug.Log("Incorrect checkpoint");
            }
            else Debug.Log("Incorrect tag:" + other.tag);



        }
        else Debug.Log("no GM");
    }
}
