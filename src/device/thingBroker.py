from umqtt.simple import MQTTClient
from machine      import Pin
import urequests
import json
'''
mqtt_server = 'mqtt3.thingspeak.com'
mqtt_port = 1883
mqtt_user = 'LRgpHzYsIDQfAxMyNRA1Cx8' # caso necessário
mqtt_pass = 'LnProSHsvRcXAXBtmpoam6dS' # caso necessário
CHANNEL_ID = '2154206'
client_id = 'LRgpHzYsIDQfAxMyNRA1Cx8'
'''
class TBroker():
    reader = None
    publisher = None
    pub_topic = None
    sub_topic = None
    lastCommand = None
    
    def __init__(self,configurations):
        print('Inicializando configurações do broker.')
        id_channel_common = configurations.get("broker")["id_channel_common"]
        self.server = configurations.get("broker")["server"]
        self.port = configurations.get("broker")["port"]
        self.user = configurations.get("broker")["user"]
        self.passW = configurations.get("broker")["password"]
        self.channel = id_channel_common
        self.cient = configurations.get("broker")["client_id"] 
        self.time = configurations.get("broker")["time"]
        self.api_key = configurations.get("broker")["common_write_api"]
        self.common_pub_topic = "channels/" + id_channel_common +  "/publish/fields/field1"
        self.common_sub_topic = "channels/" + id_channel_common  + "/subscribe/fields/field1"
        
    def initReader(self):
        print('Iniciando Reader')
        self.reader = MQTTClient(self.cient, self.server, self.port, self.user, self.passW,keepalive=self.time)
        time1 = self.reader.keepalive
        print(f'reader keepalive > {time1}')
        self.reader.set_callback(self.Commands)
    
    def initPublisher(self):
        print('Iniciando Publisher')
        self.publisher = MQTTClient(self.cient, self.server, self.port, self.user, self.passW)
        time2 = self.publisher.keepalive
        print(f'publisher > {time2}')
    
    def setReaderTime(self,time):
        print('Setando novo time ao reader')
        self.time = time
        self.reader = MQTTClient(self.cient, self.server, self.port, self.user, self.passW,keepalive=self.time)
    
    def get_time(self):
        time1 = self.reader.keepalive
        time2 = self.publisher.keepalive
        print(f'reader > {time1}, publisher > {time2}')
    #MQTT Field1
    def public_mqtt_single(self,message):
        try:
            print(f'Publicação: {message}')
            topic = self.common_pub_topic
            print(f'Topic: {topic}')
            self.reader.connect()
            self.reader.publish(topic,message)
            self.reader.disconnect()
        except Exception as e:
            print(f'Error on public_mqtt_single >> {e}')

    #HTTP
    def publish_http_data(self,data):
        try:
            #print('Publicando no broker..')
            #url = 'https://api.thingspeak.com/update?api_key={0}'.format(self.api_key)
            url = 'https://api.thingspeak.com/update?api_key={0}&{1}'.format(self.api_key,data)
            response = urequests.get(url)
            response.close()
            #print('Publicação realizada!')
        except Exception as e:
            print(f'Error on publish_http_data >> {e}')
        
    #MQTT for more one field
    def publish_data2(self,payload):
        try:
            self.publisher.connect()
            publish_topic = 'channels/{0}/publish/{1}'.format(self.channel, self.api_key)
            #payload = {
            #    'field2': sensor1_value,
            #    'field3': sensor2_value,
            #    'field4': sensor3_value
            #    }
            payload_json = json.dumps(payload)
            self.publisher.publish(publish_topic, payload_json, retain=True)
            self.publisher.disconnect()
        except Exception as e:
            print(f'Error on publish_data2 >> {e}')
            
    def subBroker(self):
        try: 
            common_topic = self.common_sub_topic
            if self.reader.sock is not None:
                pass
            else:
                print('Estabelecendo inscrição...')
                self.reader.connect(clean_session=True)
                print('Inscrição estabelecida!')
            self.reader.subscribe(common_topic)
        except Exception as e:
            print(f'Erro ao estabelecer inscrição >> {e} ')
    
    def aguardar_mensagem(self):
        try: 
            common_topic = self.common_sub_topic
            if self.reader.sock is not None:
                pass
            else:
                print('Estabelecendo inscrição...')
                self.reader.connect(clean_session=True)
                print('Inscrição estabelecida!')
            self.reader.subscribe(common_topic)
            
            print('Aguardando dados do broker...')
            #self.reader.wait_msg()
            self.reader.check_msg()
        except Exception as e:
            print(f'aguardar_mensagem >> {e}')
            self.reader.disconnect()
            self.reader.loop_stop()
        
        
    def set_callback(self):
        self.reader.set_callback(self.Commands)
        
    def Commands(self,topic,value):
        try:
            self.lastCommand = value#.decode('utf-8')
        except Exception as error:
            print('Valor {0} inválido'.format(value))
            print('Erro ao receber comando - {0}'.format(error))
            self.recebido = False
            self.reader.disconnect()
