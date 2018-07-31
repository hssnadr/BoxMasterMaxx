float getPixCooX(int pix_) {
  // Return pixel X coordinate in millimeters
  int x_ = pix_ % COLS;
  x_ *= dwX;
  return x_;
}

float getPixCooY(int pix_) {
  // Return pixel Y coordinate in millimeters
  int y_ = ROWS - 1 - pix_ / COLS;
  y_ *= dhY;
  return y_;
}
