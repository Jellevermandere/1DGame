using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


//todo add
// sounds
// direction hint
// game loop
// more levels
// arduino connection
public class GameManager : MonoBehaviour
{
    public bool isRacing = false;
    public bool waiting = true;
    public bool loopingTrack = true;
    public GameObject[] checkPoints;
    public RacerController[] racers;
    public int nrOfLaps = 1;
    public bool showDebugLines = false;


    private void Awake()
    {
        //FindCheckPoints();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        UpdatePlacement();

        for (int i = 0; i < racers.Length; i++)
        {
            racers[i].currentPlace = i + 1;
        }


    }

    private void OnDrawGizmos()
    {
        if (showDebugLines)
        {
            if (checkPoints.Length > 0)
            {
                Debug.DrawLine(loopingTrack ? checkPoints.Last().transform.position : transform.position, checkPoints[0].transform.position, Color.red);

                for (int i = 1; i < checkPoints.Length; i++)
                {
                    Debug.DrawLine(checkPoints[i - 1].transform.position, checkPoints[i].transform.position, Color.red);
                }
            }
        }

        
    }

    void FindCheckPoints()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
    }

    private void UpdatePlacement()
    {
        racers = racers.OrderByDescending(a => a.finished).ThenByDescending(a => a.currentLap).ThenByDescending(a => a.currentCheckPoint).ThenByDescending(a => a.raceDistance).ToArray();
    }

    void findPlayers()
    {

    }

    public void ToggleRacing()
    {
        isRacing = !isRacing;
        waiting = false;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
