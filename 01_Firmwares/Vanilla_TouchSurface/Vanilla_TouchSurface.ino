#include "Wire.h"
#include "I2Cdev.h"
#include "MPU6050.h"
MPU6050 accelgyro;

// IMU sensor
int16_t ax, ay, az;
int16_t gx, gy, gz;
int16_t mx, my, mz;

// Touch surface
#define  ROWS  8
#define  COLS  8

//Mux control pins
int s0 = 8;
int s1 = 9;
int s2 = 10;
int s3 = 11;
int controlPin[] = {s0, s1, s2, s3};
int muxChannel[16][4] = {{0, 0, 0, 0}, {1, 0, 0, 0}, {0, 1, 0, 0}, {1, 1, 0, 0}, {0, 0, 1, 0}, {1, 0, 1, 0}, {0, 1, 1, 0}, {1, 1, 1, 0}, {0, 0, 0, 1}, {1, 0, 0, 1}, {0, 1, 0, 1}, {1, 1, 0, 1}, {0, 0, 1, 1}, {1, 0, 1, 1}, {0, 1, 1, 1}, {1, 1, 1, 1}};

//Mux in “SIG” pin
int SIG_pin = 0;

// MULTIPLEXER DIGITAL
int SER_Pin = 4;   //pin 14 on the 75HC595
int RCLK_Pin = 5;  //pin 12 on the 75HC595
int SRCLK_Pin = 6; //pin 11 on the 75HC595
//How many of the shift registers - change this
#define number_of_74hc595s 1
//do not touch
#define numOfRegisterPins number_of_74hc595s * 8
boolean registers[numOfRegisterPins];

void setup() {
  Wire.begin();
  
  Serial.begin(38400);

  //--------------------------------
  //------------MPU6050-------------
  //--------------------------------
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
  pinMode(SER_Pin, OUTPUT);
  pinMode(RCLK_Pin, OUTPUT);
  pinMode(SRCLK_Pin, OUTPUT);
  //reset all register pins
  clearRegisters();
  writeRegisters();
}

void loop() {
  for (int row = 0; row < ROWS; row++) {
    setRegisterPin(row, HIGH);
    writeRegisters();
    
    Serial.print("z");
    Serial.print(row);
    Serial.print("x");

    // READ LINE VALUES
    for (int col = 0; col < COLS; col++) {
      Serial.print(readMux(col));
      if (col != COLS - 1) {
        Serial.print("x");
      }
    }
    //Serial.println();
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
  Serial.print(ax);
  Serial.print("c");
  Serial.print(ay);
  Serial.print("c"); // maybe not useful for punching bag
  Serial.print(az);  // maybe not useful for punching bag
  Serial.print('q');
}

int readMux(int channel) {
  for (int i = 0; i < 4; i ++) {
    digitalWrite(controlPin[i], muxChannel[channel][i]);
  } //read the value at the SIG pin

  int val = analogRead(SIG_pin); //return the value
  return val;
}
