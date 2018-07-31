void setPixelSwatchColorFromSerial(char buff_[20]) {
  // Get pixel and swatch color index from serial message
  String msg_ = "";   // initialize string message variable

  // Get pixel index
  for (int i = 0; i < 3; i++) {
    msg_ += buff_[i];    // collect serial buffer characters into one string message
  }
  int pix_ = hexToDec(msg_);

  // Get pixel color
  uint32_t curCol_ = getColorFromSwatch(buff_[3]); // color reference is encoded in one hexadecimal digit
  
  // Set pixel color
  setNeoPixColor(pix_, curCol_);
}

void setPixelRGBColorFromSerial(char buff_[20]) {
  // Get pixel and RGB color coordinate from serial message
  String msg_ = "";   // initialize string message variable

  // Get pixel index (3 digits)
  for (int i = 0; i < 3; i++) {
    msg_ += buff_[i];    // collect serial buffer characters into one string message
  }
  int pix_ = hexToDec(msg_);

  // Get red component (2 digits)
  msg_ = buff_[3];
  msg_ += buff_[4];
  int red_ = hexToDec(msg_);

  // Get green component (2 digits)
  msg_ = buff_[5];
  msg_ += buff_[6];
  int green_ = hexToDec(msg_);

  // Get blue component (2 digits)
  msg_ = buff_[7];
  msg_ += buff_[8];
  int blue_ = hexToDec(msg_);

  // Set pixel color
  setNeoPixColor(pix_, strip0.Color(red_, green_, blue_));
}

void getSerialCommand(char buff_[20]) {
  // Get and use other incoming serial messages
  //!\\ number of char must be different from 5 or 10 (otherwise it s received as a led info)

  String msg_ = "";             // initialize string message variable

  for (int i = 0; i < bufIndex - 1; i++) {
    msg_ += (char)buff_[i];   // collect serial buffer characters into one string message
  }

  // Use messages
  if (msg_ == "connect") {
    isConnected = true;
    //Serial.println("connected");
  }
  if (msg_ == "disconnect") {
    isConnected = false;
    //Serial.println("disconnected");
  }
}

//----------------------------------------------------
//------------------- UTILITIES ----------------------
//----------------------------------------------------

unsigned int hexToDec(String hexString) {
  // Convert a hexadecimal string number into a decimal integer
  // See : https://github.com/benrugg/Arduino-Hex-Decimal-Conversion/blob/master/hex_dec.ino

  unsigned int decValue = 0; // initialize return value
  int nextInt;

  for (int i = 0; i < hexString.length(); i++) {
    nextInt = int(hexString.charAt(i));
    if (nextInt >= 48 && nextInt <= 57) nextInt = map(nextInt, 48, 57, 0, 9);
    if (nextInt >= 65 && nextInt <= 70) nextInt = map(nextInt, 65, 70, 10, 15);
    if (nextInt >= 97 && nextInt <= 102) nextInt = map(nextInt, 97, 102, 10, 15);
    nextInt = constrain(nextInt, 0, 15);

    decValue = (decValue * 16) + nextInt;
  }
  return decValue;
}
