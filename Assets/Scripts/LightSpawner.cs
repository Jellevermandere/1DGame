using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpawner : MonoBehaviour
{
    public enum SpawnMode { linear, radial}

    [Header("SpawnParameters")]
    [SerializeField]
    private GameObject lightBulbPrefab;
    [SerializeField]
    private int nrOfBulbs = 10;
    [SerializeField]
    private SpawnMode spawnMode = SpawnMode.linear;
    [SerializeField]
    private float lightDistance = 1f;
    [SerializeField]
    private Vector3 forwardVector = new Vector3(0, 0, 1);
    [SerializeField]
    private Vector3 angleShift = Vector3.zero;
    [Header("GameParameters")]
    [SerializeField]
    [Range(0, 1)]
    private float nrLight;


    List<LightBulb> lightBulbs = new List<LightBulb>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnLightBulbs();
    }

    // Update is called once per frame
    void Update()
    {
        if(lightBulbs.Count > 0)
        {
            if (Input.GetKeyDown("left"))
            {
                nrLight -= 1 / (float)lightBulbs.Count;
                if (nrLight < 0) nrLight += 1;
                nrLight = nrLight % 1;
            }
            else if (Input.GetKeyDown("right"))
            {
                nrLight += 1 / (float)lightBulbs.Count;
                nrLight = nrLight % 1;
            }

        }

        /*
        if (Input.GetAxisRaw("Horizontal") != 0 && lightBulbs.Count> 0)
        {
            nrLight += Input.GetAxisRaw("Horizontal") / (float)lightBulbs.Count * Time.deltaTime;

            nrLight = nrLight % 1;

        }
        */


        for (int i = 0; i < lightBulbs.Count; i++)
        {
            SetColor(i, 0, 0, 0);
        }

        SetColor(nrLight, 255, 255, 255);
    }


    void SpawnLightBulbs()
    {
        for (int i = 0; i < nrOfBulbs; i++)
        {

            LightBulb newBulb;
            switch (spawnMode)
            {
                case SpawnMode.linear:
                    newBulb = Instantiate(lightBulbPrefab, transform.position + forwardVector * i * lightDistance, transform.rotation).GetComponent<LightBulb>();
                    break;
                case SpawnMode.radial:
                    newBulb = Instantiate(lightBulbPrefab, transform.position + Quaternion.Euler(angleShift.x * i, angleShift.y * i, angleShift.z * i) * forwardVector * lightDistance, transform.rotation).GetComponent<LightBulb>();
                    break;

                default:
                    newBulb = Instantiate(lightBulbPrefab, transform.position + Quaternion.Euler(angleShift.x * i, angleShift.y * i, angleShift.z * i) * forwardVector * i * lightDistance, transform.rotation).GetComponent<LightBulb>();
                    break;
            }

            
            lightBulbs.Add(newBulb);

        }
    }

    void SetColor(int nr, int r, int g, int b)
    {
        if (nr >= lightBulbs.Count) return;


        lightBulbs[nr].SetColor(r, g, b);
    }

    void SetColor(float nr, int r, int g, int b)
    {
        if (nr > 1 || nr < 0) return;
        

        lightBulbs[Mathf.RoundToInt(nr * (lightBulbs.Count-1))].SetColor(r, g, b);
    }
}
