#include <Wire.h>
#include <Adafruit_MCP4725.h>
#include <Adafruit_ADS1X15.h>
#include <SoftwareSerial.h>

// Define los pines para el puerto serial extra
// RX en pin 10 (lectura), TX en pin 11 (escritura)
SoftwareSerial extraSerial(10, 11); 
// Crea una instancia del objeto MCP4725
Adafruit_MCP4725 dac;
// Crea una instancia del objeto ADS1115
Adafruit_ADS1115 ads;

// Variables para almacenar los datos recibidos
float k1, k2, k3, k4, P, I, referencia, dt,u=0.0;
float R1, R2, R3, R4, L1, L2, C1, C2;
String Circuito = "";
String EstadoControlado = "";
float error_integral=0;

String inputString = ""; // Para construir el mensaje recibido
bool stringComplete = false; // Si se ha recibido el mensaje completo
bool enviarDatos =false;

//..........................
float Vestado0_volts = 0;
float Vestado1_volts = 0;
float Vestado2_volts = 0;
float Vestado3_volts = 0;

void setup() {
  // Inicia la comunicación serial para monitoreo (Plotter Serie)
  Serial.begin(9600);
  // Inicializa el puerto serial extra para pruebas
  extraSerial.begin(9600);
  // Inicia la comunicación I2C y configura el DAC
  dac.begin(0x60);
  // Inicia la comunicación I2C y configura el ADC
  ads.begin();
  // Configura el I2C a 400 kHz
  Wire.setClock(400000);
  // Configurar el DAC inicialmente
  setDACVoltage(4.7);
  delay(3000);
}

void loop() {
  // Llamar a la función de lectura de datos
  LecturaDatos();

  // Solo enviar datos si se ha recibido la orden correspondiente
  if (enviarDatos) {
    LecturaSensoresADC();
    LecturaDatos();
    //Ley de Control
    CalculoControl();
  }
}

// Función para el envío del valor Vi al DAC
void setDACVoltage(float desiredVoltage) {
  // Suponiendo que Vcc es 4.7 V
  float Vcc = 4.82;

  // Calcula el valor digital necesario para obtener el voltaje deseado
  uint16_t value = (uint16_t)((desiredVoltage / Vcc) * 4095);  // Convertir voltaje a valor de 12 bits

  // Enviar el valor al DAC
  dac.setVoltage(value, false);
}

//Función para leer los Voltajes de Ch1, Ch2,Ch3 y Ch4 del ADC
void LecturaSensoresADC() {
  
  // Leer solo los canales necesarios del ADC para optimizar el rendimiento
  int16_t adc0 = ads.readADC_SingleEnded(0);
  int16_t adc1 = ads.readADC_SingleEnded(1);
  int16_t adc2 = ads.readADC_SingleEnded(2);
  int16_t adc3 = ads.readADC_SingleEnded(3);
  // Convertir los valores ADC a voltajes
  float multiplier = 6.144 / 32768.0;  // Calcula el multiplicador basado en el rango ±6.144V
  Vestado0_volts = adc0 * multiplier;
  Vestado1_volts = adc1 * multiplier;
  Vestado2_volts = adc2 * multiplier;
  Vestado3_volts = adc3 * multiplier;
  
   // Enviar los datos al puerto serial en formato <ID:Valor:Tipo>
  if(Circuito.equals("I-V") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts/R2, 2);  // Enviar con dos decimales iL1
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts / R4, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);  // Enviar con dos decimales
    Serial.println(":F>");
  }
  else if(Circuito.equals("I-V") && EstadoControlado.equals("iL2")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts / R4, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts/R2, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);  // Enviar con dos decimales
    Serial.println(":F>");
  }
  else if(Circuito.equals("I-VI") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts / R2, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);  // Enviar con dos decimales
    Serial.println(":F>");
  }
  else if(Circuito.equals("I-VI") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts / R2, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);  // Enviar con dos decimales
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);  // Enviar con dos decimales
    Serial.println(":F>");

  }
  else if(Circuito.equals("I-VII") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts / R2, 2);  // Enviar con dos decimales iL1
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts / R3, 2);  // Enviar con dos decimales iL2
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado3_volts, 2);  // Enviar con dos decimales Vc1
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);  // Enviar con dos decimales u
    Serial.println(":F>");
  }
  else if(Circuito.equals("I-VII") && EstadoControlado.equals("iL2")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts / R3, 2);  // Enviar con dos decimales iL2
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts / R2, 2);  // Enviar con dos decimales iL1
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado3_volts, 2);  // Enviar con dos decimales Vc1
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);  // Enviar con dos decimales u
    Serial.println(":F>");
  }
  else if(Circuito.equals("I-VII") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado3_volts, 2);  // Enviar con dos decimales Vc1
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts / R2, 2);  // Enviar con dos decimales iL1
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado2_volts / R3, 2);  // Enviar con dos decimales iL2
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);  // Enviar con dos decimales u
    Serial.println(":F>");
  }
  else if(Circuito.equals("II-V") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts, 2);  // Enviar con dos decimales Vc1
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts / R3, 2);  // Enviar con dos decimales iL1
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);  // Enviar con dos decimales iL2
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);  // Enviar con dos decimales u
    Serial.println(":F>");    
  }
  else if(Circuito.equals("II-V") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts / R3, 2);  // Enviar con dos decimales Vc1
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts, 2);  // Enviar con dos decimales iL1
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);  // Enviar con dos decimales iL2
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);  // Enviar con dos decimales u
    Serial.println(":F>");    
  }
  else if(Circuito.equals("II-VI") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts, 2);  // Enviar con dos decimales Vc1
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts, 2);  // Enviar con dos decimales iL1
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);  // Enviar con dos decimales iL2
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);  // Enviar con dos decimales u
    Serial.println(":F>");    
  }
  else if(Circuito.equals("II-VI") && EstadoControlado.equals("Vc2")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts, 2);  // Enviar con dos decimales Vc1
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts, 2);  // Enviar con dos decimales iL1
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);  // Enviar con dos decimales iL2
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);  // Enviar con dos decimales u
    Serial.println(":F>");    
  }
  else if(Circuito.equals("II-VII") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts / R2, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado3_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("II-VII") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts / R2, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado3_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado0_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("II-VII") && EstadoControlado.equals("Vc2")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado3_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado2_volts / R2, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("III-V") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts / R3, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("III-V") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts / R3, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(u, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("III-VII") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts / R2, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado3_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("III-VII") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts / R2, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado3_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado0_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("III-VII") && EstadoControlado.equals("Vc2")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado3_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado2_volts / R2, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-V") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts / R1, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado1_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado2_volts / R3, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-V") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado1_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts / R3, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado0_volts / R1, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-V") && EstadoControlado.equals("iL2")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts / R3, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts / R1, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado1_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-VI") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts / R1, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado3_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-VI") && EstadoControlado.equals("Vceq")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado3_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado0_volts / R1, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(u, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-VII") && EstadoControlado.equals("iL1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado0_volts / R1, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado1_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado2_volts / R2, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado3_volts, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-VII") && EstadoControlado.equals("Vc1")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado1_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado2_volts / R2, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado3_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado0_volts / R1, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-VII") && EstadoControlado.equals("iL2")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado2_volts / R2, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado3_volts, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado0_volts / R1, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado1_volts, 2);
    Serial.println(":F>");    
  }
  else if(Circuito.equals("IV-VII") && EstadoControlado.equals("Vc2")){

    Serial.print("<Vestado0:"); 
    Serial.print(Vestado3_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado1:"); 
    Serial.print(Vestado0_volts / R1, 2); 
    Serial.println(":F>");

    Serial.print("<Vestado2:"); 
    Serial.print(Vestado1_volts, 2);
    Serial.println(":F>");

    Serial.print("<Vestado3:"); 
    Serial.print(Vestado2_volts / R2, 2);
    Serial.println(":F>");    
  }

}

// Función para el control
void CalculoControl() {
if(Circuito.equals("I-V") && EstadoControlado.equals("iL1")){
    CircuitoI_V_iL1();
}
else if(Circuito.equals("I-V") && EstadoControlado.equals("iL2")){
    CircuitoI_V_iL2();
}
else if(Circuito.equals("I-VI") && EstadoControlado.equals("Vc1")){
    CircuitoI_VI_Vc1();
}
else if(Circuito.equals("I-VI") && EstadoControlado.equals("iL1")){
    CircuitoI_VI_iL1();
}
else if(Circuito.equals("I-VII") && EstadoControlado.equals("iL1")){
    CircuitoI_VII_iL1();
}
else if(Circuito.equals("I-VII") && EstadoControlado.equals("iL2")){
    CircuitoI_VII_iL2();
}
else if(Circuito.equals("I-VII") && EstadoControlado.equals("Vc1")){
    CircuitoI_VII_Vc1();
}
else if(Circuito.equals("II-V") && EstadoControlado.equals("Vc1")){
    CircuitoII_V_Vc1();
}
else if(Circuito.equals("II-V") && EstadoControlado.equals("iL1")){
    CircuitoII_V_iL1();
}
else if(Circuito.equals("II-VI") && EstadoControlado.equals("Vc1")){
    CircuitoII_VI_Vc1();
}
else if(Circuito.equals("II-VI") && EstadoControlado.equals("Vc2")){
    CircuitoII_VI_Vc2();
}
else if(Circuito.equals("II-VII") && EstadoControlado.equals("Vc1")){
    CircuitoII_VII_Vc1();
}
else if(Circuito.equals("II-VII") && EstadoControlado.equals("iL1")){
    CircuitoII_VII_iL1();
}
else if(Circuito.equals("II-VII") && EstadoControlado.equals("Vc2")){
    CircuitoII_VII_Vc2();
}
else if(Circuito.equals("III-V") && EstadoControlado.equals("Vc1")){
    CircuitoIII_V_Vc1();
}
else if(Circuito.equals("III-V") && EstadoControlado.equals("iL1")){
    CircuitoIII_V_iL1();
}
else if(Circuito.equals("III-VII") && EstadoControlado.equals("Vc1")){
    CircuitoIII_VII_Vc1();
}
else if(Circuito.equals("III-VII") && EstadoControlado.equals("iL1")){
    CircuitoIII_VII_iL1();
}
else if(Circuito.equals("III-VII") && EstadoControlado.equals("Vc2")){
    CircuitoIII_VII_Vc2();
}
else if(Circuito.equals("IV-V") && EstadoControlado.equals("iL1")){
    CircuitoIV_V_iL1();
}
else if(Circuito.equals("IV-V") && EstadoControlado.equals("Vc1")){
    CircuitoIV_V_Vc1();
}
else if(Circuito.equals("IV-V") && EstadoControlado.equals("iL2")){
    CircuitoIV_V_iL2();
}
else if(Circuito.equals("IV-VI") && EstadoControlado.equals("iL1")){
    CircuitoIV_VI_iL1();
}
else if(Circuito.equals("IV-VI") && EstadoControlado.equals("Vceq")){
    CircuitoIV_VI_Vceq();
}
else if(Circuito.equals("IV-VII") && EstadoControlado.equals("iL1")){
    CircuitoIV_VII_iL1();
}
else if(Circuito.equals("IV-VII") && EstadoControlado.equals("Vc1")){
    CircuitoIV_VII_Vc1();
}
else if(Circuito.equals("IV-VII") && EstadoControlado.equals("iL2")){
    CircuitoIV_VII_iL2();
}
else if(Circuito.equals("IV-VII") && EstadoControlado.equals("Vc2")){
    CircuitoIV_VII_Vc2();
}
}

//Funcion para leer datos recibidos desde AppControl
void LecturaDatos(){
  // Revisar si se han recibido datos desde el puerto serial
  while (Serial.available()) {
    char inChar = (char)Serial.read();
    inputString += inChar;

    // Si detectamos un marcador de fin de mensaje ('\n')
    if (inChar == '\n') {
      stringComplete = true;
    }
  }

  // Si el mensaje está completo, lo procesamos
  if (stringComplete) {
    parseData();
    inputString = ""; // Limpiar el buffer de entrada
    stringComplete = false; // Reiniciar la bandera de mensaje completo
  }
}

// Función para procesar los datos recibidos y extraer los valores
void parseData() {
  // Verificar si la trama empieza con "%P"
  if (inputString.startsWith("%P")) {
    // Remover el encabezado "%P"
    inputString = inputString.substring(2);

    // Convertir comas en puntos (si existen)
    inputString.replace(",", ".");

    // Separar los valores basados en los dos puntos ":"
    int index = 0;
    String tokens[18];  // Array para almacenar los tokens
    while (inputString.length() > 0 && index < 18) {
      int separatorIndex = inputString.indexOf(':');
      if (separatorIndex == -1) {
        tokens[index++] = inputString;  // Último token
        break;
      }
      tokens[index++] = inputString.substring(0, separatorIndex);
      inputString = inputString.substring(separatorIndex + 1);
    }
    // Imprimir cada token para verificar
    extraSerial.println("Tokens encontrados:");
    for (int i = 0; i < index; i++) {
      extraSerial.print("Token ");
      extraSerial.print(i);
      extraSerial.print(": ");
      extraSerial.println(tokens[i]);
    }
    //-----------------------------------------------

    // Convertir y almacenar cada token en la variable correspondiente
    if (index >= 18) {
      //ResetVariables();
      k1 = tokens[0].toFloat();
      k2 = tokens[1].toFloat();
      k3 = tokens[2].toFloat();
      k4 = tokens[3].toFloat();
      P = tokens[4].toFloat();
      I = tokens[5].toFloat();
      referencia = tokens[6].toFloat();
      dt = tokens[7].toFloat();
      R1 = tokens[8].toFloat();
      R2 = tokens[9].toFloat();
      R3 = tokens[10].toFloat();
      R4 = tokens[11].toFloat();
      L1 = tokens[12].toFloat();
      L2 = tokens[13].toFloat();
      C1 = tokens[14].toFloat();
      C2 = tokens[15].toFloat();
      Circuito = tokens[16];  // Asignar el valor como cadena
      EstadoControlado = tokens[17];  // Asignar el valor como cadena
      EstadoControlado.trim();
    }

    // Mostrar los valores recibidos solo en extraSerial
    extraSerial.println("Datos recibidos:");
    extraSerial.print("k1: "); extraSerial.println(k1, 6);
    extraSerial.print("k2: "); extraSerial.println(k2, 6);
    extraSerial.print("k3: "); extraSerial.println(k3, 6);
    extraSerial.print("k4: "); extraSerial.println(k4, 6);
    extraSerial.print("P: "); extraSerial.println(P, 6);
    extraSerial.print("I: "); extraSerial.println(I, 6);
    extraSerial.print("Referencia: "); extraSerial.println(referencia, 6);
    extraSerial.print("dt: "); extraSerial.println(dt, 6);
    extraSerial.print("R1: "); extraSerial.println(R1, 6);
    extraSerial.print("R2: "); extraSerial.println(R2, 6);
    extraSerial.print("R3: "); extraSerial.println(R3, 6);
    extraSerial.print("R4: "); extraSerial.println(R4, 6);
    extraSerial.print("L1: "); extraSerial.println(L1, 6);
    extraSerial.print("L2: "); extraSerial.println(L2, 6);
    extraSerial.print("C1: "); extraSerial.println(C1, 6);
    extraSerial.print("C2: "); extraSerial.println(C2, 6);
    extraSerial.print("Circuito: "); extraSerial.println(Circuito);
    extraSerial.print("EstadoControlado: "); extraSerial.println(EstadoControlado);

    //DEBUGUING
    extraSerial.println(EstadoControlado=="Vc1"); 
    extraSerial.println(EstadoControlado=="iL1"); 
    extraSerial.println(Circuito=="II-V");        
  }
  else if (inputString.startsWith("%S")) {
    enviarDatos=true;
    extraSerial.print("Dato: "); 
    extraSerial.println(inputString);
    }
  else if(inputString.startsWith("%C")){
    enviarDatos=false;
    extraSerial.print("Dato: "); 
    extraSerial.println(inputString);
    }
}

//-----------------------------------
//Circuito I-V Control iL1
void CircuitoI_V_iL1(){
  float iL1 = Vestado0_volts / R2;
  float iL2 = Vestado2_volts / R4;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1 * k1 + iL2 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito I-V Control iL2
void CircuitoI_V_iL2(){
  float iL1 = Vestado0_volts / R2;
  float iL2 = Vestado2_volts / R4;
  float error = referencia - iL2; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1 * k1 + iL2 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//----------------------------------
//Circuito I-VI Control Vc1
void CircuitoI_VI_Vc1(){
  float iL1 = Vestado0_volts / R2;
  float Vc1 = Vestado2_volts;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1 * k1 + Vc1 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito I-VI Control iL1
void CircuitoI_VI_iL1(){
  float iL1 = Vestado0_volts / R2;
  float Vc1 = Vestado2_volts;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1 * k1 + Vc1 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito I-VII Control iL1
void CircuitoI_VII_iL1(){
  float iL1 = Vestado0_volts / R2;
  float iL2 = Vestado2_volts/ R3;
  float Vc1 = Vestado3_volts;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1 * k1 + iL2 * k2 + Vc1 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito I-VII Control iL2
void CircuitoI_VII_iL2(){
  float iL1 = Vestado0_volts / R2;
  float iL2 = Vestado2_volts/ R3;
  float Vc1 = Vestado3_volts;
  float error = referencia - iL2; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1 * k1 + iL2 * k2 + Vc1 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito I-VII Control Vc1
void CircuitoI_VII_Vc1(){
  float iL1 = Vestado0_volts / R2;
  float iL2 = Vestado2_volts/ R3;
  float Vc1 = Vestado3_volts;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1 * k1 + iL2 * k2 + Vc1 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito II-V Control Vc1
void CircuitoII_V_Vc1(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R3;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito II-V Control iL1
void CircuitoII_V_iL1(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R3;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito II-VI Control Vc1
void CircuitoII_VI_Vc1(){
  float Vc1 = Vestado0_volts;
  float Vc2 = Vestado2_volts;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + Vc2 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito II-VI Control Vc2
void CircuitoII_VI_Vc2(){
  float Vc1 = Vestado0_volts;
  float Vc2 = Vestado2_volts;
  float error = referencia - Vc2; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + Vc2 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito II-VII Control Vc1
void CircuitoII_VII_Vc1(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2 + Vc2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito II-VII Control iL1
void CircuitoII_VII_iL1(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2 + Vc2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito II-VII Control Vc2
void CircuitoII_VII_Vc2(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - Vc2; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2 + Vc2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito III-V Control Vc1
void CircuitoIII_V_Vc1(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R3;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito III-V Control iL1
void CircuitoIII_V_iL1(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R3;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito III-VI se omite por topología de corto circuito
//-----------------------------------
//Circuito III-VII Control Vc1
void CircuitoIII_VII_Vc1(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2 + Vc2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito III-VII Control iL1
void CircuitoIII_VII_iL1(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2 + Vc2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito III-VII Control Vc2
void CircuitoIII_VII_Vc2(){
  float Vc1 = Vestado0_volts;
  float iL1 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - Vc2; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (Vc1* k1 + iL1 * k2 + Vc2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito IV-V Control iL1
void CircuitoIV_V_iL1(){
  float iL1 = Vestado0_volts / R1;
  float Vc1 = Vestado1_volts;
  float iL2 = Vestado2_volts / R3;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vc1 * k2 + iL2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito IV-V Control Vc1
void CircuitoIV_V_Vc1(){
  float iL1 = Vestado0_volts / R1;
  float Vc1 = Vestado1_volts;
  float iL2 = Vestado2_volts / R3;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vc1 * k2 + iL2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito IV-V Control iL2
void CircuitoIV_V_iL2(){
  float iL1 = Vestado0_volts / R1;
  float Vc1 = Vestado1_volts;
  float iL2 = Vestado2_volts / R3;
  float error = referencia - iL2; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vc1 * k2 + iL2 * k3) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito IV-VI Control iL1
void CircuitoIV_VI_iL1(){
  float iL1 = Vestado0_volts / R1;
  float Vceq = Vestado2_volts;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vceq * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito IV-VI Control Vceq
void CircuitoIV_VI_Vceq(){
  float iL1 = Vestado0_volts / R1;
  float Vceq = Vestado2_volts;
  float error = referencia - Vceq; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vceq * k2) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//-----------------------------------
//Circuito IV-VII Control iL1
void CircuitoIV_VII_iL1(){
  float iL1 = Vestado0_volts / R1;
  float Vc1 = Vestado1_volts;
  float iL2 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - iL1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vc1 * k2 + iL2* k3 + Vc2 * k4) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito IV-VII Control Vc1
void CircuitoIV_VII_Vc1(){
  float iL1 = Vestado0_volts / R1;
  float Vc1 = Vestado1_volts;
  float iL2 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - Vc1; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vc1 * k2 + iL2* k3 + Vc2 * k4) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito IV-VII Control iL2
void CircuitoIV_VII_iL2(){
  float iL1 = Vestado0_volts / R1;
  float Vc1 = Vestado1_volts;
  float iL2 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - iL2; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vc1 * k2 + iL2* k3 + Vc2 * k4) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}
//Circuito IV-VII Control Vc2
void CircuitoIV_VII_Vc2(){
  float iL1 = Vestado0_volts / R1;
  float Vc1 = Vestado1_volts;
  float iL2 = Vestado2_volts / R2;
  float Vc2 = Vestado3_volts;
  float error = referencia - Vc2; //error
  error_integral += error * dt;   // Acumular el error integral
  u = P*referencia - (iL1* k1 + Vc1 * k2 + iL2* k3 + Vc2 * k4) + I*error_integral; // Calcular el valor u a partir de los estados
  u=u/2; //Se divide porque el DAC maneja valores de 0 a 5V, el operacional los multiplic
  // Enviar el valor u al DAC
  setDACVoltage(u); //Valor entre 0 y 5V dc
}