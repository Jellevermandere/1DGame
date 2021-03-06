using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulbManager : MonoBehaviour
{
    [SerializeField]
    private GameObject lightBulbPrefab;
    [SerializeField]
    private Transform examplePlayer;

    [SerializeField]
    private bool local = true;
    [SerializeField]
    private bool UpdateRunTime = false;
    [SerializeField]
    private bool showForwardLED = true, showPlacementLED = true;

    [SerializeField]
    private PlayerRaycaster player;

    [SerializeField]
    private float radius = 10;

    List<LightBulb> lightBulbs = new List<LightBulb>();
    [SerializeField]
    private int middleLED = 0;
    byte[] allLEDs = new byte[0];

    [SerializeField]
    private ArduinoConnector arduinoConnector;
    [SerializeField]
    private bool sendData = false;

    [SerializeField] private float updateInterval = 0.5f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        SpawnLightBulbs();
        allLEDs = new byte[player.nrLEDs*3 + 1];
        allLEDs[player.nrLEDs * 3] = (byte)'\n';
    }

    // Update is called once per frame
    void Update()
    {
        if(UpdateRunTime)UpdateLightBulbs();

        if(examplePlayer && !local)
        {
            examplePlayer.up = player.transform.right;
        }

        float minAngle = Mathf.Infinity;
        for (int i = 0; i < player.nrLEDs; i++)
        {
            if (local)
            {
                if (i == 0)
                {
                    Debug.DrawLine(transform.position, lightBulbs[i].transform.position);
                }
                else if (i == player.nrLEDs-1)
                {
                    Debug.DrawLine(transform.position, lightBulbs[i].transform.position);
                }


            }

            Vector2 direction = PlayerRaycaster.rotate2D(local ? player.transform.right : Vector3.up, player.fov / 2f - i * player.fov / (float)player.nrLEDs);

            if(minAngle > Vector2.Angle(direction, player.transform.right))
            {
                minAngle = Vector2.Angle(direction, player.transform.right);
                middleLED = i;
            }

            lightBulbs[i].SetColor(player.CastRay(direction, i));
        }

        if (showForwardLED)
        {
            if (showPlacementLED && player.nrLEDs >= player.racerController.currentPlace)
            {
                for (int i = 0; i < player.racerController.currentPlace; i++)
                {
                    int nr = middleLED + i - (player.racerController.currentPlace / 2);
                    lightBulbs[(nr+player.nrLEDs) % player.nrLEDs].SetColor(Color.green);
                }
            }
            else
            {
                lightBulbs[middleLED].SetColor(Color.green);
            }
        }

        if(sendData) UpdateColorBytes();

    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if( timer > updateInterval)
        {
            if (sendData && arduinoConnector && arduinoConnector.useArduino) arduinoConnector.WriteToArduino(allLEDs);
            timer = 0f;
        }

       
    }

    void SpawnLightBulbs()
    {
        LightBulb newBulb;
        for (int i = 0; i < player.nrLEDs; i++)
        {
            newBulb = Instantiate
                (
                lightBulbPrefab,
                transform.position + transform.rotation * Quaternion.Euler(0, -player.fov / 2f + i * player.fov / (float)player.nrLEDs, 0) * Vector3.forward * radius,
                transform.rotation,
                transform
                ).GetComponent<LightBulb>();

            lightBulbs.Add(newBulb);
        }
        
    }

    void UpdateColorBytes()
    {
        for (int i = 0; i < player.nrLEDs; i++)
        {
            allLEDs[i * 3] = lightBulbs[i].byteColor[0];
            allLEDs[i * 3 + 1] = lightBulbs[i].byteColor[1];
            allLEDs[i * 3 + 2] = lightBulbs[i].byteColor[2];
        }
    }


    void UpdateLightBulbs()
    {
        if(lightBulbs.Count < player.nrLEDs)
        {
            LightBulb newBulb;
            for (int i = lightBulbs.Count-1; i < player.nrLEDs; i++)
            {
                newBulb = Instantiate
                    (
                    lightBulbPrefab,
                    transform.position,
                    transform.rotation,
                    transform
                    ).GetComponent<LightBulb>();

                lightBulbs.Add(newBulb);
            }
        }

        for (int i = 0; i < lightBulbs.Count; i++)
        {
            lightBulbs[i].gameObject.SetActive(false);
        }


        for (int i = 0; i < player.nrLEDs; i++)
        {
            lightBulbs[i].transform.position = transform.position + transform.rotation * Quaternion.Euler(0, -player.fov / 2f + i * player.fov / (float)player.nrLEDs, 0) * Vector3.forward * radius;
            lightBulbs[i].gameObject.SetActive(true);
        }
    }
}
