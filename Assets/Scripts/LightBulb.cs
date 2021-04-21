using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb : MonoBehaviour
{
    [SerializeField]
    private bool UpdateLocally = false;
    [SerializeField]
    MeshRenderer lightMesh;

    [Header("RGB")]
    [SerializeField]
    [Range(0,255)]
    int r;
    [SerializeField]
    [Range(0, 255)]
    int g, b;

    [Header("HSV")]
    [SerializeField]
    [Range(0, 1)]
    float h;
    [SerializeField]
    [Range(0, 1)]
    float s, v;

    [SerializeField]
    [Header("ColorPicker")]
    private Color32 color;

    [SerializeField]
    [Space(10)]
    float intensity = 1.6f;

    public byte[] byteColor = new byte[3];

    private float oldh, olds, oldv;
    private int oldr, oldg, oldb;
    private Color oldCol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateLocally)
        {
            if(r != oldr || g != oldg || b != oldb)
            {
                oldr = r;
                oldg = g;
                oldb = b;
                color = new Color32((byte)r, (byte)g, (byte)b, 255);
                Color.RGBToHSV(color, out h,out  s,out v);
            }
            if(h != oldh || s != olds || v != oldv)
            {
                Color32 newCol = Color.HSVToRGB(h, s, v);
                color = newCol;
                r = newCol.r;
                g = newCol.g;
                b = newCol.b;

                oldh = h;
                olds = s;
                oldv = v;
            }
            if(color != oldCol)
            {
                r = color.r;
                g = color.g;
                b = color.b;

                Color.RGBToHSV(color, out h, out s, out v);

                oldCol = color;
            }

            SetColor(r, g, b);


        }
    }

    public void SetColor(int r, int g, int b)
    {
        SetColor(new Color(r / 255f, g / 255f, b / 255f, 1));
        
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
