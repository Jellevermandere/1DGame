//Light Theremin
int sensorValue;
int finalValue;
int sensorLow = 1023;
int sensorHigh = 0;
const int ledPin = 13;

void setup() {
 
 Serial.begin(9600);
 pinMode(ledPin, OUTPUT);
 digitalWrite(ledPin, HIGH);
 
 while (millis() < 5000){
   sensorValue = analogRead(A0);
   if (sensorValue > sensorHigh){
     sensorHigh = sensorValue;
   }
   if (sensorValue < sensorLow){
     sensorLow = sensorValue;
   }
 }
 Serial.println("ready");
 
 digitalWrite(ledPin, LOW);
}

void loop() {
 sensorValue = analogRead(A0);
 delay(10);
 finalValue = map(sensorValue, sensorLow, sensorHigh,  0, 1023);
 Serial.println(finalValue);
}
