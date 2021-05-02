using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    [Range(0,360)]
    public float fov = 90f;
    [Range(1,300)]
    public int nrLEDs = 10;

    [SerializeField]
    private LayerMask environmentMask;
    [SerializeField]
    private float maxDistance = 10f;
    [SerializeField]
    private bool useDirectionFlow = true;
    [SerializeField]
    private bool useNormal = true;
    [SerializeField]
    [Range(0,1)]
    private float minDirectionFlowMod;
    [SerializeField]
    private float frequency = 1, waveLenght = 1;
    
    public RacerController racerController;


    float hue = 0;
    float colorVal = 0;
    Color ledCol = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for (int i = 0; i < nrLEDs; i++)
        {
            CastRay(rotate2D(transform.right, fov / 2f - i * fov / (float)nrLEDs), i);
        }
        */
    }

    public Color CastRay(Vector2 direction, int nr)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, environmentMask);

        // If it hits something...
        if (hit.collider != null)
        {
            hue = hit.transform.localScale.z;
            if (useNormal) colorVal = 1- Mathf.Abs(Vector2.Dot(hit.normal, transform.up));  //(Vector2.Dot(hit.normal, transform.up) + 1) / 2f;
            else colorVal = 0;
            //ledCol = new Color(1, colorVal, colorVal) * (maxDistance - hit.distance) / maxDistance;
            ledCol = Color.HSVToRGB(hue, 1- colorVal, (maxDistance - hit.distance) / maxDistance);
            if (useDirectionFlow) ledCol *= ForwardWaveModifier(hit.point);
            Debug.DrawLine(transform.position, hit.point, ledCol);
            return ledCol;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + ((Vector3)direction * maxDistance), Color.black);
        }

        return Color.black;
    }
    public static Vector2 rotate2D(Vector2 v, float delta)
    {
        delta = delta * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    float ForwardWaveModifier(Vector3 hitPoint)
    {
        if (!racerController) return 1;
        float dotDist = Vector3.Dot((racerController.nextCheckPoint - transform.position).normalized, (hitPoint - transform.position).normalized);
        float val = Map(Mathf.Sin(-Time.time * frequency + dotDist * waveLenght), -1, 1, minDirectionFlowMod, 1);
        //float val = Map(Mathf.Sin(Time.time * frequency + Vector3.Distance(transform.position,hitPoint)), -1, 1, minDirectionFlowMod, 1);
        return val;
    }

    // c#
    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}
