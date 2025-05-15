using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eslabon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform TransformSlave;
    public float offset = 1;
    public float coef = 1f;

    public Vector3 displa;
    // Arrays de ángulos
    int[] angles = {
    90, 85, 80, 75, 70, 65, 60, 55, 50, 45,
    40, 35, 30, 25, 20, 15, 10, 5, 0, -5,
    -10, -15, -20, -25, -30, -35, -40, -45, -50, -55,
    -60, -65, -70, -75, -80, -85, -90
    };

    public List<float> theta2 = new List<float> {
        98.33052250774611f, 99.71910090269643f, 101.1482486420304f, 102.522710438907f, 103.8575302261015f,
        105.2719077630253f, 106.4212229156376f, 107.7256375118571f, 108.9045288907132f, 114.5929948171511f,
        118.8724693960043f, 122.0085742419114f, 124.5576505173287f, 126.6598707465439f, 128.3582850381305f,
        129.6556833096235f, 130.5313986571293f, 130.9466109860156f, 130.844494688159f, 130.1460450778485f,
        128.7386028086311f, 126.4446311685725f, 122.911882380003f, 116.8262405427398f, 110.4053959693186f,
        108.3868208134764f, 105.9039908415846f, 103.1633265075793f, 99.9624471592047f, 96.67910293607858f,
        92.99753267676222f, 89.28741116820468f, 85.64705716428877f, 82.04119647973488f, 78.88455134648176f,
        75.96354166927834f, 73.37703434901965f
    };

    public List<float> theta3 = new List<float> {
        98.16657901964786f, 99.7155759825388f, 101.1144254990818f, 102.5272099443226f, 103.968405117418f,
        104.9380820522146f, 106.6587621718108f, 107.7059700970273f, 108.8966077409885f, 92.54438964252401f,
        80.31345629088814f, 71.3551732367894f, 63.51262541619126f, 56.13779857465262f, 48.90991027002884f,
        41.62127348394014f, 34.10712241005159f, 26.21249544237088f, 17.76871855567585f, 8.566236469739973f,
        -1.693298635312964f, -13.51329595893741f, -28.00085456705293f, -49.74113792344828f, -69.6259136415884f,
        -71.54622826185218f, -74.23757397189534f, -76.85781736580432f, -80.01819989762467f, -83.29373193167774f,
        -87.10781508271832f, -90.74250498981833f, -94.37305974948157f, -98.25884343264198f, -101.0678520380707f,
        -104.1145161906764f, -106.6746413138151f
    };

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        displa = TransformSlave.localRotation.eulerAngles;

        if (displa.x > 180)
        {
            displa.x -= 360;
        }
        //transform.localEulerAngles = new Vector3(displa.x * coef + offset, 0, 0);
        
        PrintAngleData((int)Math.Round(displa.x));
        //Debug.Log((int)Math.Round(displa.x));
    }

    void PrintAngleData(int angle)
    {
        // Buscar el índice del ángulo en la lista angles
        int index = Array.IndexOf(angles, angle);
        //Debug.Log(index);

        // Si el ángulo existe en la lista
        if (index != -1)
        {
            // Obtener los valores correspondientes de theta2 y theta3
            float correspondingTheta2 = ((theta2[index]- 130.146045077848f)-displa.x)* coef;
            //float correspondingTheta3 = theta3[index];

            // Imprimir los valores
            //Debug.Log($"Para el ángulo {angle} en la posición {index}:");
            transform.localEulerAngles = new Vector3(correspondingTheta2, 0, 0);
            //Debug.Log($"theta2: {correspondingTheta2}");
            //Debug.Log($"theta3: {correspondingTheta3}");
        }
        else
        {
            Debug.Log($"El ángulo {angle} no se encontró en la lista angles.");
        }
    }
}