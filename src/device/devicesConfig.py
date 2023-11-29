from machine      import Pin, PWM, ADC
from dht 		  import DHT22

class SoilSensor:
    sensor = None
    min_moisture = 0
    max_moisture = 4095
    def __init__(self,configurations):
        pin = configurations.get("devices")["soilSensor"]
        self.sensor = ADC(Pin(pin))
        self.sensor.atten(ADC.ATTN_11DB) #Full range: 3.3v
        self.sensor.width(ADC.WIDTH_12BIT) #range 0 to 4095
    def get_moisture(self):
        self.sensor.read()
        value = (self.max_moisture - self.sensor.read()) * 100 / (self.max_moisture - self.min_moisture) 
        return value
        #if value != 0.0:
        #    return value
        #else:
        #    raise Exception("Erro ao coletar dados do solo") 


class DTHSensor:
    sensor = None
    def __init__(self,configurations):
        pin = configurations.get("devices")["dhtSensor"]
        self.sensor = DHT22(Pin(pin))
    def get_all_values(self):
        self.sensor.measure()
        retorno = {
            "humidity": self.sensor.humidity(),
            "temperature": self.sensor.temperature()
            }
        return retorno
    def getHumidity(self):
        self.sensor.measure()
        return self.sensor.humidity()
    def getTemperature(self):
        self.sensor.measure()
        return self.sensor.temperature()
    
class ServoMotor:
    #motor = ServoMotor(21,zerar= True)
    #motor.move(0)
    servo = None
    current_angle = -0.001
    def __init__(self,configurations):
        pin = configurations.get("devices")["servo"]
        zerar = False
        self.servo = PWM(Pin(pin, mode=Pin.OUT), freq=50)
        if zerar:
            self.zerarMotor()
    def angle_conversion(self,angle):
        min_angle = 0
        max_angle = 180
        min_u10_duty = 26 - 0 # offset for correction
        max_u10_duty = 123- 0  # offset for correction
        angle_conversion_factor = (max_u10_duty - min_u10_duty) / (max_angle - min_angle)
        return int((angle - min_angle) * angle_conversion_factor) + min_u10_duty
    def move(self,angle):
        print("Processando.. {0}".format(self.current_angle))
        # round to 2 decimal places, so we have a chance of reducing unwanted servo adjustments
        angle = round(angle, 2)
        # do we need to move?
        if angle == self.current_angle:
            return
        self.current_angle = angle
        # calculate the new duty cycle and move the motor
        duty_u10 = self.angle_conversion(angle)
        print('Move: {0}'.format(duty_u10))
        self.servo.duty(duty_u10)
    def zerarMotor(self):
        self.current_angle = 0
        self.servo.duty(self.angle_conversion(0))




