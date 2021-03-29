using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb : MonoBehaviour
{
    [SerializeField]
    private bool UpdateLocally = false;
    [SerializeField]
    MeshRenderer lightMesh;

    [SerializeField]
    [Range(0,255)]
    int r, g, b;
    [SerializeField]
    float intensity = 1.6f;

    public byte[] byteColor = new byte[3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(UpdateLocally)SetColor(r, g, b);
    }

    public void SetColor(int r, int g, int b)
    {
        if (lightMesh) lightMesh.material.SetColor("_EmissionColor", new Color(r/255f,g/255f,b/255f,1) * intensity);
        lightMesh.material.color = new Color(r / 255f, g / 255f, b / 255f, 1);
    }

    public void SetColor(Color col)
    {
        if (lightMesh) lightMesh.material.SetColor("_EmissionColor", col * intensity);
        lightMesh.material.color = col;
        byteColor[0] = (byte)Mathf.FloorToInt(Mathf.Clamp(col.r, 0.01f,1f) * 255);
        byteColor[1] = (byte)Mathf.FloorToInt(Mathf.Clamp(col.g, 0.01f, 1f) * 255);
        byteColor[2] = (byte)Mathf.FloorToInt(Mathf.Clamp(col.b, 0.01f, 1f) * 255);

        byteColor[0] = (byteColor[0] == 10) ? (byte)9 : byteColor[0];
        byteColor[1] = (byteColor[1] == 10) ? (byte)9 : byteColor[1];
        byteColor[2] = (byteColor[2] == 10) ? (byte)9 : byteColor[2];
    }
}
