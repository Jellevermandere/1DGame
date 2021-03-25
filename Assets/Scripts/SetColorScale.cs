using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorScale : MonoBehaviour
{
    [SerializeField]
    private bool setColor = true;
    [SerializeField]
    private Color color = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        if(setColor) SetColor();
    }

    // Update is called once per frame
    void SetColor()
    {
        Color.RGBToHSV(color, out float H, out float S, out float V);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, H);
    }
}
