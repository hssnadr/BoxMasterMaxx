void displayPix(int x_, int y_, uint32_t c_) {
  // Display one pixel based on XY space coordinate (in millimeters)
  int pix_ = getPixIndex(x_, y_);
  if (getPixColor(pix_) != c_ && c_ != -1) { // do not replace current pix color if the pix already get it
    setNeoPixColor(pix_, c_);
  }
}

void background(uint32_t c_) {
  // Set all pixels the same color
  for (int i = 0; i < nPix; i++) {
    if (getPixColor(i) != c_ && c_ != -1) {
      setNeoPixColor(i, c_);
    }
  }
}

//-----------------------------------------------------
//---------------- GEOMETRIC SHAPES -------------------
//-----------------------------------------------------
void drawCircle(int cx_, int cy_, int r_,  uint32_t c_ ){
  // Draw a circle
  for (int i = dwX * int((cx_ - r_) / float(dwX)); i <= dwX * int((cx_ + r_) / float(dwX)) + 1; i += dwX) {
    for (int j = dhY * int((cy_ - r_) / float(dhY)); j <= dhY * int((cy_ + r_) / float(dhY)) + 1; j += dhY) {
      if (pow(i - cx_, 2) + pow(j - cy_, 2) <= pow(r_, 2)) {
        displayPix(i, j, c_);
      }
    }
  }
}

void drawSplitScreen(float angle_, int shiftX0_, int shiftY0_, uint32_t c_) {
  // Split the screen with a line (inclined) and fill part above the line (use angle+=PI to fill part under the line)
  if (abs(cos(angle_)) > 0.001) {
    // Get ax+b=y from angle
    float a_ = tan(angle_);
    for (int i = -shiftX0_; i < widthPannel - shiftX0_; i += dwX) {
      if (cos(angle_) > 0) {
        for (int j = a_ * i; j < heightPannel - shiftY0_; j += dhY) {
          displayPix(i + shiftX0_, j + shiftY0_, c_);
        }
      }
      else {
        for (int j = a_ * i; j >= -shiftY0_; j -= dhY) {
          displayPix(i + shiftX0_, j + shiftY0_, c_);
        }
      }
    }
  }
  else {
    if (sin(angle_) > 0) {
      for (int i = 0; i < shiftX0_; i += dwX) {
        for (int j = 0; j < heightPannel; j += dhY) {
          displayPix(i, j, c_);
        }
      }
    }
    else {
      for (int i = shiftX0_; i < widthPannel; i += dwX) {
        for (int j = 0; j < heightPannel; j += dhY) {
          displayPix(i, j, c_);
        }
      }
    }
  }
}

void drawThickLine(float angle_, int shiftX0_, int shiftY0_, int strokeWeight_, uint32_t c_) {
  // Draw an inclined line, center by shiftX0_ and shiftY0, with a thickness of strokeWeight_
  if (abs(cos(angle_)) > 0.001) {
    // Get ax+b=y from angle
    float a_ = tan(angle_);
    for (int i = -shiftX0_; i < widthPannel - shiftX0_; i += dwX) {
      for (int j = a_ * i - int(strokeWeight_ / (2.*cos(angle_))); j < a_ * i + int(strokeWeight_ / (2.*cos(angle_))); j++) {
        displayPix(i + shiftX0_, j + shiftY0_, c_);
      }
    }
  }
  else {
    for (int i = shiftX0_ - int(strokeWeight_ / 2.); i < shiftX0_ + int(strokeWeight_ / 2.); i += dwX) {
      for (int j = 0; j < heightPannel; j += dhY) {
        displayPix(i, j, c_);
      }
    }
  }
}

void drawLine(float angle_, int shiftX0_, int shiftY0_, uint32_t c_) {
  // Draw an inclined line, center by shiftX0_ and shiftY0, with a thickness of one pixel
  if (abs(cos(angle_)) > 0.001) {
    // Get ax+b=y from angle
    float a_ = tan(angle_);
    for (int i = -shiftX0_; i < widthPannel - shiftX0_; i += dwX) {
      displayPix(i + shiftX0_, a_ * i + shiftY0_, c_);
    }
  }
  else {
    for (int j = 0; j < heightPannel; j += dhY) {
      displayPix(shiftX0_, j, c_);
    }
  }
}
