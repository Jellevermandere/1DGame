using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports; // this enables the IO port namespace

// ************* This script manages the Arduino Communication ******************* //
// tutorial I learned this from: https://playground.arduino.cc/Main/MPU-6050/#measurements //
// arduino script at the bottom //

public class ArduinoConnector : MonoBehaviour
{
    [Header("ConnectionSettings")]
    public bool useArduino;
    public string IOPort = "/dev/cu.usbmodem1424301"; //"/dev/cu.HC05-SPPDev"; // Change this to whatever port your Arduino is connected to, this is the port for the specefic bluetooth adaptor used (HC-05 Wireless Bluetooth RF Transceiver)
    public int baudeRate = 115200; //this must match the bauderate of the Arduino script



    [HideInInspector]
    public SerialPort sp;

    private string recievedValue;

    public float recievedDirection = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (useArduino) ActivateSP();
    }

    // Update is called once per frame
    void Update()
    {
        if (useArduino && sp.IsOpen)
        {
            try
            {
                recievedValue = sp.ReadLine(); //reads the serial input
                Debug.Log(recievedValue);
                SetDirection(recievedValue); //translates the string into a angle
            }
            catch (System.Exception)
            {

            }
        }

    }

    void ActivateSP()
    {
        sp = new SerialPort(IOPort, baudeRate, Parity.None, 8, StopBits.One);

        sp.Open();
        sp.ReadTimeout = 25;
    }

    public void WriteToArduino(byte[] message)
    {
        /*
        if (useArduino && sp.IsOpen)
        {
            sp.Write(message, 0, message.Length);
            string mess = "";

            foreach (var item in message)
            {
                mess += item + ", ";
            }
            Debug.Log(mess);
            sp.BaseStream.Flush();
        }
        */
        
        if (useArduino)
        {
            if (sp.IsOpen)
            {
                sp.Write(message, 0, message.Length);
                string mess = "";

                foreach (var item in message)
                {
                    mess += item + ", ";
                }
                Debug.Log(mess);
                //sp.BaseStream.Flush();
            }

        }

    }
    //16 9163841391 3469000000 1221186115 2146761481 4374025500 0015514977

    void SetDirection(string message)
    {
        float.TryParse(message, out recievedDirection);
    }
}
