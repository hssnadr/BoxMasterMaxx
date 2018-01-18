#include <Adafruit_NeoPixel.h>
#define ledPin 5
#define BRIGHTNESS 5
int nPix = 140;
Adafruit_NeoPixel strip = Adafruit_NeoPixel(nPix, ledPin, NEO_GRBW + NEO_KHZ800);
long timerLedShow;

// Serial buffer
byte buff[20];
int bufIndex = 0;

void setup() {
  // Initialize serial
  Serial.begin(38400);

  // Initialize led strip
  timerLedShow = millis();
  strip.begin();
  strip.show();
}

void loop() {
  if (Serial.available() > 0) {
    bufIndex = 0;
    do
    {
      buff[bufIndex] = Serial.read();
      if (buff[bufIndex] != -1) {
        bufIndex++;
      }
    } while (buff[bufIndex - 1] != 95);

    if(bufIndex > 2){
      int ipix_ = buff[0];
      int red_ = buff[1];
      int green_ = buff[2];
      int blue_ = buff[3];
      strip.setPixelColor(ipix_, strip.Color(red_, green_, blue_));
    }
  }
  
  if (millis() - timerLedShow > 20) {
    strip.show();
    timerLedShow = millis();
  }
}
