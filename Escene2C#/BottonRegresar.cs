using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System;
using System.Threading.Tasks;

public class BottonRegresar : MonoBehaviour
{
    [Header("Configuración de Conexión")]
    public string ipAddress = "192.168.4.1"; // IP por defecto del AP de ESP32
    public int port = 8000;

    [Header("Debug")]
    public string status = "Disconnected";

    private TcpClient client;
    private NetworkStream stream;
    private bool isConnected = false;

    // EscenaAnterior
    public void LoadPreviousScene()
    {
        SendMessageToESP32("bye desde Unity!");
        Disconnect();

        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
        else
        {
            Debug.LogWarning("No hay escena anterior. Volviendo a la escena 0.");
            SceneManager.LoadScene(0);
        }
    }

    // EscenaSiguiente
    public async void LoadNextScene()
    {
        await ConnectToESP32Async();
        SendMessageToESP32("Hola desde Unity!");

        if (isConnected)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No hay más escenas disponibles. Volviendo a la escena 0.");
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            Debug.LogWarning("No se pudo conectar a la ESP32.");
        }
    }

    public async Task ConnectToESP32Async()
    {
        if (isConnected) return;

        Debug.Log("Intentando conectar a ESP32...");
        client = new TcpClient();

        try
        {
            var connectTask = client.ConnectAsync(IPAddress.Parse(ipAddress), port);
            var timeoutTask = Task.Delay(3000); // Timeout de 3 segundos

            var completedTask = await Task.WhenAny(connectTask, timeoutTask);

            if (completedTask == connectTask && client.Connected)
            {
                stream = client.GetStream();
                isConnected = true;
                status = "Connected to ESP32";
                Debug.Log(status);
            }
            else
            {
                status = "Conexión a ESP32 fallida o tiempo de espera agotado.";
                Debug.LogWarning(status);
                client.Close();
            }
        }
        catch (Exception e)
        {
            status = "Error: " + e.Message;
            Debug.LogError(status);
        }
    }
    public void ConnectToESP32()
    {
        if (isConnected) return;

        Debug.Log("Intentando conectar a ESP32...");
        try
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddress), port);

            if (client.Connected)
            {
                isConnected = true;
                stream = client.GetStream();
                status = "Connected to ESP32";
                Debug.Log(status);
            }
        }
        catch (Exception e)
        {
            status = "Error: " + e.Message;
        }
    }
    public void SendMessageToESP32(string message)
    {
        if (!isConnected)
        {
            Debug.LogWarning("No conectado al servidor.");
            ConnectToESP32();
            return;
        }

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message + "\n");
            stream.Write(data, 0, data.Length);
            Debug.Log("Mensaje enviado: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error enviando mensaje: " + e.Message);
            Disconnect();
        }
    }

    public void SendbyButton(string message)
    {
        Debug.Log("Hola");
        SendMessageToESP32("Hola, estoy mandando algo");
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }

    public void Disconnect()
    {
        if (!isConnected) return;

        try
        {
            isConnected = false;

            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

            if (client != null)
            {
                client.Close();
                client = null;
            }

            status = "Disconnected";
            Debug.Log("Desconectado de ESP32");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al desconectar: " + e.Message);
        }
    }
}
