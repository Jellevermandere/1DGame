
#include "FastLED.h"

// Fixed definitions cannot change on the fly.
#define DATA_PIN 6                                             // Serial data pin
#define COLOR_ORDER GRB                                       // It's GRB for WS2812B and BGR for APA102
#define LED_TYPE WS2812B                                       // What kind of strip are you using (APA102, WS2801 or WS2812B)?
#define NUM_LEDS 30  

// Initialize changeable global variables.
uint8_t max_bright = 50;                                     // Overall brightness definition. It can be changed on the fly.

struct CRGB leds[NUM_LEDS];                                   // Initialize our LED array.

const byte numChars = NUM_LEDS*3 + 1;
char receivedChars[numChars];   // an array to store the received data
boolean newData = false;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  delay(1000);
  LEDS.addLeds<LED_TYPE, DATA_PIN, COLOR_ORDER>(leds, NUM_LEDS);         // For WS2812B
  FastLED.setBrightness(max_bright);
  FastLED.setMaxPowerInVoltsAndMilliamps(5, 500);
  for(int i = 0; i < NUM_LEDS; i++)
  {
    leds[i] = 0x00FF00;
    FastLED.show();
    delay(100);
    leds[i] = 0x000000;
  }
  FastLED.show();
  
  Serial.println("<Arduino is ready>");
}

void loop() {
  // put your main code here, to run repeatedly:
  //Serial.readBytes( (char*)leds, NUM_LEDS * 3);
  //FastLED.show();//refresh the pixel LED's
  recvWithEndMarker();
  showNewData();
}

void recvWithEndMarker() {
    static byte ndx = 0;
    char endMarker = '\n';
    char rc;
    
    while (Serial.available() > 0 && newData == false) {
        rc = Serial.read();

        if (rc != endMarker) {
            receivedChars[ndx] = rc;
            ndx++;
            if (ndx >= numChars) {
                ndx = numChars - 1;
            }
        }
        else {
            receivedChars[ndx] = '\0'; // terminate the string
            ndx = 0;
            newData = true;
        }
    }
}

void showNewData() {
    if (newData == true) 
    {
        int nrLED = 0;
        for (int i = 0; i < strlen(receivedChars); i+=3) 
        {
          
          leds[nrLED].r = (int)receivedChars[i];
          leds[nrLED].g = (int)receivedChars[i+1];
          leds[nrLED].b = (int)receivedChars[i+2];
          nrLED++;
        }
        FastLED.show();
        //Serial.print("This just in ... ");
        //Serial.println(receivedChars);
        newData = false;
    }
}
