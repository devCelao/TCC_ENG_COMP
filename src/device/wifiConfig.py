import network  
from time import sleep

class WIFI:

    def __init__(self,configurations):
        self.ssid = configurations.get("wifi")["ssid"]
        self.password = configurations.get("wifi")["password"]
        self.wlan = network.WLAN(network.STA_IF)
        
    def connect(self):
        if not self.wlan.isconnected():
            print(f"Tentando conectar-se à rede {self.ssid}...")
            self.wlan.active(True)
            self.wlan.connect(self.ssid, self.password)
            while not self.wlan.isconnected():
                print(".", end="")
                sleep(0.3)
        print(f"Conectado à rede {self.ssid}")
    def disconnect(self):
        if self.wlan.isconnected():
            self.wlan.disconnect()
            #self.wlan.active(False)
            print("Desconectado da rede WiFi")
    def isConnected(self):
        return self.wlan.isconnected()

    def list_networks(self):
        try:
            self.wlan.active(True)
            networks = self.wlan.scan()
            print("Redes WiFi disponíveis:")
            for net in networks:
                ssid = net[0].decode("utf-8")
                rssi = net[3]
                print(f"SSID: {ssid}, RSSI: {rssi} dBm")
        except Exception as e: 
            print(f"Erro >> {e}")




