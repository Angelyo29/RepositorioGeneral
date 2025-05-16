from machine import Pin, PWM
import time

# Constantes
NUM_SERVOS = 7
PWM_FREQ_HZ = 50

# Configuración de pines para los servomotores
servo_pins = [10, 11, 12, 13, 14, 15, 16]
servos = []

def init_servos():
    """Inicializar todos los servomotores"""
    global servos
    servos = []
    
    for pin in servo_pins:
        # Crear objeto PWM para cada servo
        pwm = PWM(Pin(pin))
        pwm.freq(PWM_FREQ_HZ)  # Frecuencia de 50Hz (estándar para servos)
        pwm.duty(0)  # Iniciar en posición 0
        servos.append(pwm)
    
    print(f"Inicializados {NUM_SERVOS} servomotores")

def move_servo(servo_num, angle):
    """
    Mover un servo a un ángulo específico
    
    Args:
        servo_num: Número de servo (1-7)
        angle: Ángulo deseado (0-180 grados)
    """
    # Validación de parámetros
    if servo_num < 1 or servo_num > NUM_SERVOS:
        print(f"Error: Número de servo inválido (1-{NUM_SERVOS})")
        return
    
    # Ajustar índice (convertir de 1-7 a 0-6)
    index = servo_num - 1
    
    # Limitar ángulo a rango válido
    if angle < 0:
        angle = 0
    if angle > 180:
        angle = 180
    
    # Conversión de ángulo a duty cycle
    # En MicroPython para ESP32, el rango duty es de 0 a 1023
    # Para servos estándar: 
    # ~25 (0.5ms) corresponde a 0°
    # ~128 (2.5ms) corresponde a 180°
    min_duty = 25
    max_duty = 128
    duty = int(min_duty + (angle * (max_duty - min_duty) / 180.0))
    
    # Aplicar el duty cycle
    servos[index].duty(duty)
    
    print(f"Servo {servo_num} movido a {angle}° (Duty: {duty})")

def main():
    # Inicializar todos los servos
    init_servos()
    
    # Secuencia de movimiento
    try:
        while True:
            move_servo(1, 0)  # Servo 1 a 30°
            time.sleep(1)
            move_servo(1, 90)  # Servo 1 a 30°
            time.sleep(1)
            move_servo(1, 0)  # Servo 1 a 30°
            time.sleep(1)
            
            
            move_servo(2, 45)  # Servo 2 a 45°
            time.sleep(2)
            
            move_servo(3, 60)  # Servo 3 a 60°
            time.sleep(2)
            
            move_servo(4, 90)  # Servo 4 a 90°
            time.sleep(2)
            
            move_servo(5, 0)   # Servo 5 a 0°
            time.sleep(2)
            
            move_servo(6, 180) # Servo 6 a 180°
            time.sleep(2)
            
            move_servo(7, 90)  # Servo 7 a 90°
            time.sleep(2)
            
            # Volver a posición inicial
            for i in range(1, NUM_SERVOS + 1):
                move_servo(i, 90)
            
            time.sleep(2)
            
    except KeyboardInterrupt:
        # Limpiar al salir
        for servo in servos:
            servo.deinit()
        print("Programa terminado")

# Ejecutar el programa principal
if __name__ == "__main__":
    main()
