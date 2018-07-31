#include "Wire.h"
#include "I2Cdev.h"
#include "MPU6050.h"
MPU6050 accelgyro(0x68);

// IMU sensor
int16_t ax, ay, az;
int16_t gx, gy, gz;
int16_t mx, my, mz;

// Touch surface
#define  ROWS  24
#define  COLS  24

//Mux control pins
int s0 = 8;
int s1 = 7;
int s2 = 6;
int s3 = 5;
int controlPin[] = {s0, s1, s2, s3};
#define numOfmuxChannel 16
int muxChannel[numOfmuxChannel][4] = {{0, 0, 0, 0}, {1, 0, 0, 0}, {0, 1, 0, 0}, {1, 1, 0, 0}, {0, 0, 1, 0}, {1, 0, 1, 0}, {0, 1, 1, 0}, {1, 1, 1, 0}, {0, 0, 0, 1}, {1, 0, 0, 1}, {0, 1, 0, 1}, {1, 1, 0, 1}, {0, 0, 1, 1}, {1, 0, 1, 1}, {0, 1, 1, 1}, {1, 1, 1, 1}};

//Mux in “SIG” pin
int SIG_pin0 = 9;
int SIG_pin1 = 8;

// MULTIPLEXER DIGITAL
int SERIAL_Pin = 2;   //pin 14 on the 75HC595
int STCP_Pin = 3;     //pin 12 on the 75HC595
int SHCP_Pin = 4;     //pin 11 on the 75HC595
//How many of the shift registers - change this
#define number_of_74hc595s 3
#define numOfRegisterPins number_of_74hc595s * 8
boolean registers[numOfRegisterPins];

void setup() {
  Wire.begin();  
  Serial.begin(38400);

  //--------------------------------
  //------------MPU6050-------------
  //--------------------------------
  delay(100);
  Serial.println("Initializing I2C devices...");
  accelgyro.initialize();
  Serial.println("Testing device connections...");
  Serial.println(accelgyro.testConnection() ? "MPU6050 connection successful" : "MPU6050 connection failed");

  //--------------------------------
  //-------MULTIPLEXER ANALOG-------
  //--------------------------------
  pinMode(s0, OUTPUT);
  pinMode(s1, OUTPUT);
  pinMode(s2, OUTPUT);
  pinMode(s3, OUTPUT);
  digitalWrite(s0, LOW);
  digitalWrite(s1, LOW);
  digitalWrite(s2, LOW);
  digitalWrite(s3, LOW);

  //--------------------------------
  //------MULTIPLEXER DIGITAL-------
  //--------------------------------
  pinMode(SERIAL_Pin, OUTPUT);
  pinMode(STCP_Pin, OUTPUT);
  pinMode(SHCP_Pin, OUTPUT);
  //reset all register pins
  clearRegisters();
  writeRegisters();
}

void loop() {
  for (int row = 0; row < ROWS; row++) {
    setRegisterPin(row, HIGH);
    writeRegisters();
    
    Serial.print("z");
    Serial.printf("%02X", row);
    Serial.print("x");

    // READ LINE VALUES
    for (int col = 0; col < COLS; col++) {
      if(col < numOfmuxChannel){
        Serial.printf("%03X", readMux(SIG_pin0, col));
      }
      else{
        Serial.printf("%03X", readMux(SIG_pin1, col%numOfmuxChannel));
      }
      
      if (col != COLS - 1) {
        Serial.print("x");
      }
    }
    Serial.print('q');

    // Set row pin in high-impedance state
    setRegisterPin(row, LOW);
    writeRegisters();
  }

  //--------------------------------
  //------------MPU6050-------------
  //--------------------------------
  // read raw accel/gyro measurements from device
  
  accelgyro.getAcceleration(&ax, &ay, &az);
  Serial.print("a");
  Serial.printf("%03X", (int)((ax+32768)*4095/65536.));
  Serial.print("c");
  Serial.printf("%03X", (int)((ay+32768)*4095/65536.));
  Serial.print("c"); // maybe not useful for punching bag
  Serial.printf("%03X", (int)((az+32768)*4095/65536.));
  Serial.print('q');
}

int readMux(int sigPin, int channel) {
  for (int i = 0; i < 4; i ++) {
    digitalWrite(controlPin[i], muxChannel[channel][i]);
  } //read the value at the SIG pin

  int val = analogRead(sigPin); //return the value
  return val;
}
