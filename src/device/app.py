from devicesConfig 	import DTHSensor, SoilSensor
import thingBroker as _broker
import wifiConfig as _wifi
from machine      	import Pin, ADC, PWM
from time 			import sleep, time
import ujson
import utime

#Variáveis globais
executar = True
ler = True
configs = None
soilSensor = None
dhtSensor = None
wifi = None
broker = None
###################
delay_pub = 15 #versao free
device_id = 1001
device_group = 0
device_frequency = 60 #segundos
device_ind_soil = True
device_ind_dht_temp = True
device_ind_dht_moist = True
device_ult_publicacao = None
###################

#led = Pin(2,Pin.OUT)
ledErro = Pin(33,Pin.OUT)
ledErro.value(0)
ledCommand = Pin(25,Pin.OUT)
ledCommand.value(0)
ledPublish = Pin(26,Pin.OUT)
ledPublish.value(0)

########################## Lendo Json
def loadJsonConfigurations():
    global configs
    pathFileConfig = "json/config.json"
    arquivo = open(pathFileConfig).read()
    configs = eval(arquivo)
########################## Testando sensores
def test_devices():
    print("Temperatura: {0} -- Humidade: {1} -- Solo: {2}".format(
        dhtSensor.getHumidity(),
        dhtSensor.getTemperature(),
        soilSensor.get_moisture()))
########################## Inicialização dos componentes
def init():
    global configs
    global soilSensor
    global dhtSensor
    global wifi
    global broker
    try:
        loadJsonConfigurations()
        soilSensor = SoilSensor(configs)
        dhtSensor = DTHSensor(configs)
        wifi = _wifi.WIFI(configs)
        broker = _broker.TBroker(configs)
    except Exception as e:
        print(f"Erro >> {e}")
        ledErro.value(1)

def sync_perfil():
    global delay_pub
    global device_id
    sleep(delay_pub)
    print('Solicitando sync do perfil')
    command = 104
    value = f'field1={command}{device_id}'
    broker.publish_http_data(value)
def testeBroker():
    '''
    print('testeBroker')
    payload = {
                'Campo1': 1,
                'Campo2': 2,
                'Campo3': 3
                }
    payload_json = ujson.dumps(payload)
    broker.public_mqtt_single(payload_json)
    '''
    global device_id
    global device_group
    global device_frequency
    command = 201
    s1 = 1
    s2 = 2
    s3 = 3
    s4 = 4
    values = f'field1={command}&field2={device_id}&field3={device_group}&field4={device_frequency}&field5={s1}&field6={s2}&field7={s3}&field7={s4}'
    #broker.public_mqtt_fields(values)
def PublicaSensores():
    global delay_pub
    global device_id
    global device_group
    global device_frequency
    global device_ind_soil
    global device_ind_dht_temp
    global device_ind_dht_moist
    global device_ult_publicacao
    # Obter a data e hora atual
    device_ult_publicacao = utime.localtime()
    command = 201
    ledPublish.value(1)
    sleep(delay_pub)
    s1 = str(dhtSensor.getHumidity()) if device_ind_soil else None
    s2 = str(dhtSensor.getTemperature()) if device_ind_dht_temp else None
    s3 = str(soilSensor.get_moisture()) if device_ind_dht_moist else None
    s4 = None
    values = f'field1={command}&field2={device_id}&field3={device_group}&field4={device_frequency}&field5={s1}&field6={s2}&field7={s3}&field7={s4}'
    print('Publicando sensores no broker..')
    broker.publish_http_data(values)
    print('Publicação realizada!')
    ledPublish.value(0)
def printaConfigs():
    global delay_pub
    global device_id
    global device_group
    global device_frequency
    global device_ind_soil
    global device_ind_dht_temp
    global device_ind_dht_moist
    
    try:
        print('#################################################')
        print(f'ID do Dispositivo: {device_id}')
        print(f'Grupo Cadastrado: {device_group}')
        print(f'Delay de publicação: {delay_pub}')
        print(f'Frequência de publicação: {device_frequency}')
        print(f'Publicação Habilitada para Solo: {device_ind_soil}')
        print(f'Publicação Habilitada para Temperatura: {device_ind_dht_temp}')
        print(f'Publicação Habilitada para Umidade: {device_ind_dht_moist}')
        print(f'Solo: {soilSensor.get_moisture()}')
        print(f'Temperatura: {dhtSensor.getTemperature()}')
        print(f'Umidade: {dhtSensor.getHumidity()}')
        print('#################################################')
    except Exception as e:
        print(f"printaConfigs Erro >> {e}")
    
def ExecutaComandos(entrada):
    if entrada is None:
        return
    ledCommand.value(1)
    global ler
    global device_id
    global device_group
    global device_frequency
    global device_ind_soil
    global device_ind_dht_temp
    global device_ind_dht_moist
    print('Comando recebido: ', entrada)
    try:
        data = entrada.decode('utf-8') if isinstance(entrada, bytes) else entrada
        if len(data) >= 3:
            # Converte os três primeiros caracteres para inteiro e atribui a 'command'
            command = int(data[:3])
            # Se for comando de publicação, aborta as validações.
            if command == 201:
                ledCommand.value(0)
                return
            # Verifica se há um ponto na string
            if '.' in data:
                # Divide a string em duas partes, antes e depois do ponto
                parts = data.split('.')
                # Verifica se há números após o ponto
                id_device = int(parts[0][3:]) if len(parts[0]) > 3 else 0
                value = int(parts[1]) if len(parts) > 1 and parts[1].isdigit() else 0
            else:            
                # Se não houver ponto, os caracteres após o terceiro são atribuídos a 'id_device'
                id_device = int(data[3:]) if data[3:].isdigit() else 0
                value = 0
        #Verifica se a mensagem foi enviada para esse id, grupo ou se foi para todos os dispositivos        
        verificar = id_device == device_id or id_device == device_group or id_device == 0

        if verificar:
            if command == 101 or command == 102 or command == 103:
                PublicaSensores()
            if command == 301 or command == 302 or command == 303:
                ler = True
                
            if command == 304 or command == 305 or command == 306:
                ler = False
                
            if command == 401 or command == 402 or command == 403:
                device_group = 0
                device_frequency = 60 #segundos
                device_ind_soil = True
                device_ind_dht_temp = True
                device_ind_dht_moist = True
                
            if command == 404 or command == 405 or command == 406:
                device_frequency = value
                
            if command == 407 or command == 408 or command == 409:
                device_group = value
                
            if command == 410 or command == 411 or command == 412:
                device_ind_soil = value == 1
                
            if command == 413 or command == 414 or command == 415:
                device_ind_dht_temp = value == 1
                
            if command == 416 or command == 417 or command == 418:
                device_ind_dht_moist = value == 1
                
            if command == 999:
                printaConfigs()

            print(f'Fim da execução. Comando {command} > Id {id_device} > Value {value}')
    except Exception as e:
        print(f"ExecutaComandos Erro >> {e}")
    ledCommand.value(0)

def main():
    global wifi
    global broker
    global ler
    global executar
    global delay_pub
    global device_ult_publicacao
    global device_frequency
    device_publicacao_atual = None
    try:
        init()
        wifi.connect()
        broker.initReader()

        if ledErro.value() == 1:
            raise Exception("Erro ao inicializar configurações.")
        sync_perfil()

        # Loop principal
        while ledErro.value() != 1:

            device_publicacao_atual = utime.localtime()
            #print(f'Ultima publicacao: {device_ult_publicacao}')
            #print(f'Data atual: {device_publicacao_atual}')
            try:
                if device_ult_publicacao is None:
                    PublicaSensores()
                
                broker.aguardar_mensagem()
                ExecutaComandos(broker.lastCommand)
                broker.lastCommand = None

                if (utime.mktime(device_publicacao_atual) - utime.mktime(device_ult_publicacao)) >= device_frequency:
                    if ler:
                        #Ler sensores quando 'ler' for verdadeira.
                        PublicaSensores()
                    else:
                        print('Publicação desligada!')

            except KeyboardInterrupt:
                ledErro.value(1)
                print('Interrupção de teclado. Saindo...')
                executar = False
        # Fim do Loop principal
            finally:
                pass
    except Exception as e:
        print('Ocorreu um erro:', e)
        ledErro.value(1)



if __name__ == "__main__":
    main()






