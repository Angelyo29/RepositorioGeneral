from machine import Timer, Pin, PWM
import network
import socket
import time
import uerrno

class ESP32Server:
    def __init__(self, ap_name="MiESP32", ap_password="angel123angel", port=8000, max_connections=1, timeout=60):
        self.ap_name = ap_name
        self.ap_password = ap_password
        self.port = port
        self.max_connections = max_connections
        self.timeout = timeout
        self.ap = None
        self.server_socket = None
        self.client_conn = None
        self.client_addr = None
        self.led = Pin(2, Pin.OUT)  # LED integrado para indicar estado
        self.last_activity = time.time()
        
        # Configuración de pines para LED RGB (ajusta según tu ESP32-S3)
        # Estos son ejemplos - verifica los pines correctos para tu placa
        self.led_red = PWM(Pin(41))   # Pin para LED Rojo
        self.led_green = PWM(Pin(40)) # Pin para LED Verde
        self.led_blue = PWM(Pin(39))  # Pin para LED Azul
        
        # Configurar frecuencia PWM
        self.led_red.freq(1000)
        self.led_green.freq(1000)
        self.led_blue.freq(1000)
        
        # Apagar todos los LEDs al inicio
        self.set_rgb_color(0, 0, 0)

    def set_rgb_color(self, red, green, blue):
        """Establece el color del LED RGB (valores de 0 a 255)"""
        # Convertir de 0-255 a 0-65535 (rango PWM)
        self.led_red.duty_u16(int(red * 257))
        self.led_green.duty_u16(int(green * 257))
        self.led_blue.duty_u16(int(blue * 257))

    def setup_ap(self):
        """Configura el Access Point"""
        self.ap = network.WLAN(network.AP_IF)
        self.ap.active(True)
        self.ap.config(essid=self.ap_name, authmode=network.AUTH_WPA_WPA2_PSK, password=self.ap_password)
        self.ap.config(channel=10, max_clients=10)
        
        print("\nAccess Point configurado:")
        print(f"Nombre: {self.ap_name}")
        print(f"Contraseña: {self.ap_password}")
        print(f"Configuración IP: {self.ap.ifconfig()}")

    def start_server(self):
        """Inicia el servidor socket"""
        self.server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        ip = self.ap.ifconfig()[0]
        self.server_socket.bind((ip, self.port))
        self.server_socket.listen(self.max_connections)
        self.server_socket.settimeout(5)  # Timeout para accept
        
        print(f"\nServidor iniciado en {ip}:{self.port}")
        print("Esperando conexiones...")

    def handle_client(self):
        """Maneja la comunicación con el cliente"""
        self.led.on()  # LED encendido cuando hay conexión
        print(f"\nConexión establecida con: {self.client_addr}")
        
        try:
            while True:
                try:
                    data = self.client_conn.recv(1024)
                    self.last_activity = time.time()
                    
                    if not data:  # Cliente cerró la conexión
                        print("Cliente cerró la conexión")
                        break
                        
                    message = data.decode().strip().lower()  # Convertir a minúsculas
                    print(f"Mensaje recibido: {message}")
                    
                    # Procesar el mensaje y controlar LED RGB
                    if message == "hola":
                        print("Mostrando color azul")
                        self.set_rgb_color(0, 0, 255)  # Azul
                    elif message == "adios":
                        print("Mostrando color rojo")
                        self.set_rgb_color(255, 0, 0)    # Rojo
                    else:
                        print("Mensaje diferente recibido")
                        self.set_rgb_color(0, 255, 0)    # Verde para otros mensajes
                    
                    # Enviar respuesta de confirmación
                    try:
                        self.client_conn.send(f"ACK: {message}\n".encode())
                    except OSError as e:
                        print(f"Error al enviar confirmación: {e}")
                        continue
                    
                except OSError as e:
                    if e.args[0] == uerrno.ETIMEDOUT:
                        continue  # Timeout, seguir esperando
                    elif e.args[0] == uerrno.ECONNRESET:
                        print("Conexión reiniciada por el cliente (ECONNRESET)")
                        print("Esperando reconexión...")
                        break
                    else:
                        print(f"Error en la conexión: {e}")
                        break
                    
                # Verificar timeout de inactividad
                if time.time() - self.last_activity > self.timeout:
                    print("Timeout por inactividad")
                    break
                    
        finally:
            self.close_client_connection()

    def close_client_connection(self):
        """Cierra la conexión con el cliente"""
        if self.client_conn:
            try:
                self.client_conn.close()
            except:
                pass
            self.client_conn = None
            self.client_addr = None
            self.led.off()  # LED apagado cuando no hay conexión
            self.set_rgb_color(0, 0, 0)  # Apagar LED RGB
            print("Conexión con cliente cerrada")

    def close_server(self):
        """Cierra el servidor correctamente"""
        self.close_client_connection()
        if self.server_socket:
            self.server_socket.close()
            self.server_socket = None
        print("Servidor cerrado")

    def run(self):
        """Ejecuta el servidor en bucle"""
        self.setup_ap()
        self.start_server()
        
        try:
            while True:
                # Intentar aceptar nueva conexión
                try:
                    self.client_conn, self.client_addr = self.server_socket.accept()
                    self.client_conn.settimeout(5)
                    self.handle_client()
                except OSError as e:
                    if e.args[0] == uerrno.ETIMEDOUT:
                        continue  # Timeout en accept, reintentar
                    print(f"Error aceptando conexión: {e}")
                    time.sleep(1)
                    
        except KeyboardInterrupt:
            print("\nServidor detenido por usuario")
        finally:
            self.close_server()
            self.ap.active(False)

# Configuración e inicio del servidor
if __name__ == "__main__":
    server = ESP32Server(
        ap_name="MiESP32",
        ap_password="angel123angel",
        port=8000,
        max_connections=1,
        timeout=300  # 5 minutos de timeout
    )
    
    server.run()