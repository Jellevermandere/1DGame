using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulbManager : MonoBehaviour
{
    [SerializeField]
    private GameObject lightBulbPrefab;

    [SerializeField]
    private bool local = true;
    [SerializeField]
    private bool UpdateRunTime = false;

    [SerializeField]
    private PlayerRaycaster player;

    [SerializeField]
    private float radius = 10;
    [SerializeField]
    private int test;

    List<LightBulb> lightBulbs = new List<LightBulb>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnLightBulbs();
    }

    // Update is called once per frame
    void Update()
    {
        if(UpdateRunTime)UpdateLightBulbs();

        for (int i = 0; i < player.nrLEDs; i++)
        {
            Vector2 direction = PlayerRaycaster.rotate2D(local ? player.transform.right : Vector3.up, player.fov / 2f - i * player.fov / (float)player.nrLEDs);
            lightBulbs[i].SetColor(player.CastRay(direction, i));
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
                transform.position + Quaternion.Euler(0, -player.fov / 2f + i * player.fov / (float)player.nrLEDs, 0) * transform.forward * radius,
                transform.rotation,
                transform
                ).GetComponent<LightBulb>();

            lightBulbs.Add(newBulb);
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
            lightBulbs[i].transform.position = transform.position + Quaternion.Euler(0, -player.fov / 2f + i * player.fov / (float)player.nrLEDs, 0) * transform.forward * radius;
            lightBulbs[i].gameObject.SetActive(true);
        }
    }
}
