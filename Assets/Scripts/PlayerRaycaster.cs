using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    [SerializeField]
    private ColorTag[] colorTags;

    public float fov = 90f;
    public int nrLEDs = 10;

    [SerializeField]
    private LayerMask environmentMask;
    [SerializeField]
    private float maxDistance = 10f;

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
            colorVal = (Vector2.Dot(hit.normal, transform.right) + 1) / 2f;
            //ledCol = new Color(1, colorVal, colorVal) * (maxDistance - hit.distance) / maxDistance;
            ledCol = Color.HSVToRGB(hue, 1- colorVal, (maxDistance - hit.distance) / maxDistance);
            Debug.DrawLine(transform.position, hit.point, ledCol);
            return ledCol;
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

    public class ColorTag
    {
        public string name;
        public float hue;
    }
}
