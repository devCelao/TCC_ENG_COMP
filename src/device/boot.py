# This file is executed on every boot (including wake-boot from deepsleep)
#import esp
#esp.osdebug(None)
#import webrepl
#webrepl.start()
import machine
import time

print("Iniciando device")
# Defina o nome do arquivo que será executado ao inicializar
app_file = 'app.py'

# Espere alguns segundos para permitir que a conexão serial se estabilize
#time.sleep(5)

# Execute o arquivo 'app.py'
#exec(open(app_file).read())

