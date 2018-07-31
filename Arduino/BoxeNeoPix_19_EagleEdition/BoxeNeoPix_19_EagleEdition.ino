#include <Adafruit_NeoPixel.h>

// Led configuration
#define nStrip 5                // number of pin led strips
#define COLS 60                 // total number of pixels columns
#define ROWS 30                 // total number of pixels rows
int nPix = COLS * ROWS;         // total number of leds
int nPixStrip = nPix / nStrip;  // number of pix per strip

// Led strips
Adafruit_NeoPixel strip0 = Adafruit_NeoPixel(nPixStrip, 8, NEO_GRB + NEO_KHZ800);  // first led strip
Adafruit_NeoPixel strip1 = Adafruit_NeoPixel(nPixStrip, 11, NEO_GRB + NEO_KHZ800); // second led stip
Adafruit_NeoPixel strip2 = Adafruit_NeoPixel(nPixStrip, 12, NEO_GRB + NEO_KHZ800); // third ...
Adafruit_NeoPixel strip3 = Adafruit_NeoPixel(nPixStrip, 13, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel strip4 = Adafruit_NeoPixel(nPixStrip, 14, NEO_GRB + NEO_KHZ800);

// Led pannel dimension
#define dwX 16                  // width between pixels in millimeters
#define dhY 35                  // height between pixels in millimeters
int widthPannel = COLS * dwX;   // width of the led pannel in millimeters
int heightPannel = ROWS * dhY;  // height of the led pannel in millimeters

// Led colors
#define BRIGHTNESS 255
uint32_t black = strip0.Color(0, 0, 0);
uint32_t purple = strip0.Color(2, 5, 50);
uint32_t cyan = strip0.Color(2, 25, 50);
uint32_t red = strip0.Color(50, 10, 1);
uint32_t green = strip0.Color(0, 80, 1);
uint32_t yellow = strip0.Color(50, 30, 2);
uint32_t white = strip0.Color(255, 255, 255);

// Led display variables
boolean isConnected = false;    // if false led pannel is not refreshed (avoid serial connection issues)
long timerLedShow;              // time reference to refresh led pannel display
int timeRefresh = 20;           // time in millisecond to refresh led pannel

// Serial buffer
char buff[20];                  // buffer to store incoming messages
int bufIndex = 0;               // index of the last received character

void setup() {
  // Initialize serial
  Serial.begin(38400);

  // Initialize led strips
  strip0.begin();
  strip1.begin();
  strip2.begin();
  strip3.begin();
  strip4.begin();
  timerLedShow = millis();    // initialize display timer

  // Set led strip brightness
  strip0.setBrightness(BRIGHTNESS);
  strip1.setBrightness(BRIGHTNESS);
  strip2.setBrightness(BRIGHTNESS);
  strip3.setBrightness(BRIGHTNESS);
  strip4.setBrightness(BRIGHTNESS);

  // Display strip (set to black without other information)
  stripShow();
}

void loop() {
  // Get serial messages
  if (Serial.available() > 0) {
    // RECEIVE SERIAL MESSAGE
    bufIndex = 0;
    do
    {
      buff[bufIndex] = (char)Serial.read(); // Collect incoming serial character into char array
      if (buff[bufIndex] != -1) {
        bufIndex++;
      }
    } while (buff[bufIndex - 1] != '_');    // End of current message

    // USE SERIAL MESSAGE
    if (bufIndex == 5) {
      setPixelSwatchColorFromSerial(buff);  // Get swatch color for a specific pixel
    }
    else {
      if (bufIndex == 10) {
        setPixelRGBColorFromSerial(buff);   // Get RGB color for a specific pixel
      }
      else {
        getSerialCommand(buff);             // Get other messages for extra command
      }
    }
  }

  // Display new led pattern
  if (isConnected) {
    if (millis() - timerLedShow > timeRefresh) {
      stripShow();
      timerLedShow = millis();
    }
  }
}

void stripShow() {
  // Display current pix colors on led strips
  strip0.show();
  strip1.show();
  strip2.show();
  strip3.show();
  strip4.show();
}
