//set all register pins to LOW
void clearRegisters(){
  for(int i = numOfRegisterPins - 1; i >=  0; i--){
     registers[i] = LOW;
  }
} 
 
//Set and display registers
//Only call AFTER all values are set how you would like (slow otherwise)
void writeRegisters(){
  digitalWrite(STCP_Pin, LOW);
 
  for(int i = numOfRegisterPins - 1; i >=  0; i--){
    digitalWrite(SHCP_Pin, LOW);
 
    int val = registers[i];
 
    digitalWrite(SERIAL_Pin, val);
    digitalWrite(SHCP_Pin, HIGH);
 
  }
  digitalWrite(STCP_Pin, HIGH);
}
 
//set an individual pin HIGH or LOW
void setRegisterPin(int index, int value){
  registers[index] = value;
}
