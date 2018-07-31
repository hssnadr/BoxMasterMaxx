void setNeoPixColor(int pix_, uint32_t col_) {
    // Set color to a pix    
    int strip_ = pix_ / nPixStrip; // get index of the strip
    pix_ = pix_ % nPixStrip;       // get index of the led on the current strip
    
    switch (strip_) {
      case 0:
        strip0.setPixelColor(pix_, col_);
        break;
      case 1:
        strip1.setPixelColor(pix_, col_);
        break;
      case 2:
        strip2.setPixelColor(pix_, col_);
        break;
      case 3:
        strip3.setPixelColor(pix_, col_);
        break;
      case 4:
        strip4.setPixelColor(pix_, col_);
        break;
      default:
        break;
    }
}

uint32_t getColorFromSwatch(char c_){
  uint32_t color_ = black;
  switch (c_) {
    case '0':
      color_ = black;
      break;
    case '1':
      color_ = white;
      break;
    case '2':
      color_ = red;
      break;
    case '3':
      color_ = green;
      break;
    case '4':
      color_ = cyan;
      break;
    case '5':
      color_ = purple;
      break;
    case '6':
      color_ = yellow;
      break;
    case '7':
      color_ = black;
      break;
    case '8':
      color_ = black;
      break;
    case '9':
      color_ = black;
      break;
    case 'A':
      color_ = black;
      break;
    case 'B':
      color_ = black;
      break;
    case 'C':
      color_ = black;
      break;
    case 'D':
      color_ = black;
      break;
    case 'E':
      color_ = black;
      break;
    case 'F':
      color_ = black;
      break;
    default:
      color_ = black;
      break;
  }
  return color_;
}

int getPixIndex(int x_, int y_) {
  x_ /= dwX;
  y_ /= dhY;

  if (y_ % 2 != 0) {
    x_ = COLS - x_ - 1;
  }
  y_ = ROWS - y_ - 1;
  int pix_ = y_ * COLS + x_;

  return pix_;
}

uint32_t getPixColor(int pix_) {
  // Return the currenct color of a pixel
  
  uint32_t col_ = -1; // initialize return color

  if (pix_ >= 0 && pix_ < nPix) {
    int strip_ = pix_ / (nPix / nStrip); // get index of the strip
    pix_ = pix_ % (nPix / nStrip);       // get index of the led on the current strip

    if (strip_ <= nStrip) {
      switch (strip_) {
        case 0:
          col_ = strip0.getPixelColor(pix_);
          break;
        case 1:
          col_ = strip1.getPixelColor(pix_);
          break;
        case 2:
          col_ = strip2.getPixelColor(pix_);
          break;
        case 3:
          col_ = strip3.getPixelColor(pix_);
          break;
        case 4:
          col_ = strip4.getPixelColor(pix_);
          break;
        default:
          break;
      }
    }
  }
  return col_;
}


//-----------------------------------------------------
//------------------ COLOR SHADES ---------------------
//-----------------------------------------------------
// See : https://learn.adafruit.com/multi-tasking-the-arduino-part-3/utility-functions

uint32_t getLerpColor(uint32_t color1, uint32_t color2, uint16_t totalSteps, uint16_t index) {
  uint8_t red = ((Red(color1) * (totalSteps - index)) + (Red(color2) * index)) / totalSteps;
  uint8_t green = ((Green(color1) * (totalSteps - index)) + (Green(color2) * index)) / totalSteps;
  uint8_t blue = ((Blue(color1) * (totalSteps - index)) + (Blue(color2) * index)) / totalSteps;

  return strip0.Color(red, green, blue);
}

// Returns the Red component of a 32-bit color
uint8_t Red(uint32_t color)
{
  return (color >> 16) & 0xFF;
}

// Returns the Green component of a 32-bit color
uint8_t Green(uint32_t color)
{
  return (color >> 8) & 0xFF;
}

// Returns the Blue component of a 32-bit color
uint8_t Blue(uint32_t color)
{
  return color & 0xFF;
}
