#include <Adafruit_NeoPixel.h>
#define ledPin 5
#define BRIGHTNESS 50
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
  strip.setBrightness(BRIGHTNESS);
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

    if(bufIndex == 13){
      int ipix_ = 100*(buff[0]-'0') + 10*(buff[1]-'0') + (buff[2]-'0');
      int red_ = 100*(buff[3]-'0') + 10*(buff[4]-'0') + (buff[5]-'0');
      int green_ = 100*(buff[6]-'0') + 10*(buff[7]-'0') + (buff[8]-'0');
      int blue_ = 100*(buff[9]-'0') + 10*(buff[10]-'0') + (buff[11]-'0');
      strip.setPixelColor(ipix_, strip.Color(red_, green_, blue_));
    }
    else{
      if(bufIndex > 2){
      }
    }
  }
  
  if (millis() - timerLedShow > 20) {
    strip.show();
    timerLedShow = millis();
  }
}
