using UnityEngine;
using System;
using static System.Math;
using UnityEngine.UI;
using TMPro;

public class ButtonMatrix : MonoBehaviour
{
    [SerializeField] private Button ButtonAxis;
    [SerializeField] private Animator animator;
    [SerializeField] private Color color1 = Color.red;
    [SerializeField] private Color color2 = Color.green;
    [SerializeField] private Color color3 = Color.white;
    [SerializeField] private Slider sliderVerificator;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text[] cellTexts;
    [SerializeField] private TextMeshProUGUI textTitleMatrix = null;

    private int n = 0;
    bool panelState = false;
    string axisUsed;

    void Start()
    {
        ButtonAxis.image.color = color1;
        ButtonAxis.onClick.AddListener(ChangeState);
        if (animator == null)
            animator = GetComponent<Animator>();
        if (ButtonAxis == null)
            ButtonAxis = GetComponent<Button>();
        panel.SetActive(panelState);
    }

    public void ChangeMatrixUpdate()
    {
        if (!panelState) return; // Solo actuar si el panel está visible
        // Si el botón está en BLANCO (n=0), mostrar matriz del efector final
        if (ButtonAxis.image.color == color3) // color3 = blanco
        {
            ActualizaMatriz(0);
            textTitleMatrix.text = "Matriz de transformación del efector final";
        }
        // Si el botón está en VERDE (n=1), mostrar matriz del eje seleccionado
        else
        {
            ActualizaMatriz((int) sliderVerificator.value);
            //Debug.Log("Hola");
            //Debug.Log((int)sliderVerificator.value);
            //Debug.Log("Angle 1");
            //Debug.Log(ButtonScr.counterVector[1]);
            //Debug.Log("Angle 2");
            //Debug.Log(ButtonScr.counterVector[2]);
            //Debug.Log("Angle 3");
            //Debug.Log(ButtonScr.counterVector[3]);
            //Debug.Log("Angle 4");
            //Debug.Log(ButtonScr.counterVector[4]);
            //Debug.Log("Angle 5");
            //Debug.Log(ButtonScr.counterVector[5]);
            //Debug.Log("Angle 6");
            //Debug.Log(ButtonScr.counterVector[6]);
        }
    }

    public void ChangeState()
    {
        switch (n)
        {
            case 0:
                panelState = true;
                panel.SetActive(panelState);
                ButtonAxis.image.color = color3;
                textTitleMatrix.text = "Matriz de transformación del efector final";
                ActualizaMatriz(0);
                break;
            case 1:
                ButtonAxis.image.color = color2;
                textTitleMatrix.text = "Matriz de transformación del eje seleccionado";
                ActualizaMatriz((int)sliderVerificator.value);
                break;
            case 2:
                panelState = false;
                panel.SetActive(panelState);
                ButtonAxis.image.color = color1;
                n = -1; // Reset para que en el siguiente click vuelva a 0
                break;
        }
        n++;
        Debug.Log($"Estado actual: {n}");
    }

    private Matrix4x4 DenavitMatrix(float a_i, float theta, float d_i, float alpha)
    {
        float cosTheta = Mathf.Cos(theta);
        float sinTheta = Mathf.Sin(theta);
        float cosAlpha = Mathf.Cos(alpha);
        float sinAlpha = Mathf.Sin(alpha);

        Matrix4x4 matrix = new Matrix4x4();

        // Redondeo a 2 decimales para cada elemento
        matrix.m00 = Mathf.Round(cosTheta * 100f) / 100f;
        matrix.m01 = Mathf.Round(-cosAlpha * sinTheta * 100f) / 100f;
        matrix.m02 = Mathf.Round(sinAlpha * sinTheta * 100f) / 100f;
        matrix.m03 = Mathf.Round(a_i * cosTheta * 100f) / 100f;

        matrix.m10 = Mathf.Round(sinTheta * 100f) / 100f;
        matrix.m11 = Mathf.Round(cosAlpha * cosTheta * 100f) / 100f;
        matrix.m12 = Mathf.Round(-sinAlpha * cosTheta * 100f) / 100f;
        matrix.m13 = Mathf.Round(a_i * sinTheta * 100f) / 100f;

        matrix.m20 = 0; // No necesita redondeo ya que es 0 exacto
        matrix.m21 = Mathf.Round(sinAlpha * 100f) / 100f;
        matrix.m22 = Mathf.Round(cosAlpha * 100f) / 100f;
        matrix.m23 = Mathf.Round(d_i * 100f) / 100f;

        matrix.m30 = 0; // No necesita redondeo
        matrix.m31 = 0; // No necesita redondeo
        matrix.m32 = 0; // No necesita redondeo
        matrix.m33 = 1; // No necesita redondeo

        return matrix;
    }

    string FormatDenavitMatrix(Matrix4x4 matrix)
    {
        // Formatear cada fila de la matriz
        string row0 = $"{matrix.m00:0.00} {matrix.m01:0.00} {matrix.m02:0.00} {matrix.m03:0.00}";
        string row1 = $"{matrix.m10:0.00} {matrix.m11:0.00} {matrix.m12:0.00} {matrix.m13:0.00}";
        string row2 = $"{matrix.m20:0.00} {matrix.m21:0.00} {matrix.m22:0.00} {matrix.m23:0.00}";

        // Eliminar ceros redundantes al final de cada fila
        row0 = row0.TrimEnd('0').TrimEnd('.');
        row1 = row1.TrimEnd('0').TrimEnd('.');
        row2 = row2.TrimEnd('0').TrimEnd('.');

        // La �ltima fila siempre es fija [0,0,0,1]
        return $"Matriz de Transformaci�n DH:\n{row0}\n{row1}\n{row2}\n0 0 0 1";
    }
    // Funci�n auxiliar para redondear toda la matriz
    Matrix4x4 TruncateMatrix(Matrix4x4 matrix, int decimalPlaces)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);

        // Truncamiento matem�tico
        matrix.m00 = Mathf.Floor(matrix.m00 * multiplier) / multiplier;
        matrix.m01 = Mathf.Floor(matrix.m01 * multiplier) / multiplier;
        matrix.m02 = Mathf.Floor(matrix.m02 * multiplier) / multiplier;
        matrix.m03 = Mathf.Floor(matrix.m03 * multiplier) / multiplier;

        matrix.m10 = Mathf.Floor(matrix.m10 * multiplier) / multiplier;
        matrix.m11 = Mathf.Floor(matrix.m11 * multiplier) / multiplier;
        matrix.m12 = Mathf.Floor(matrix.m12 * multiplier) / multiplier;
        matrix.m13 = Mathf.Floor(matrix.m13 * multiplier) / multiplier;

        matrix.m20 = Mathf.Floor(matrix.m20 * multiplier) / multiplier;
        matrix.m21 = Mathf.Floor(matrix.m21 * multiplier) / multiplier;
        matrix.m22 = Mathf.Floor(matrix.m22 * multiplier) / multiplier;
        matrix.m23 = Mathf.Floor(matrix.m23 * multiplier) / multiplier;

        // Eliminar ceros redundantes convirtiendo a string y parseando de vuelta
        // (Opcional: Solo si necesitas garantizar formato limpio en los floats)
        matrix.m00 = float.Parse(matrix.m00.ToString("0.########"));
        matrix.m01 = float.Parse(matrix.m01.ToString("0.########"));
        // ... Repetir para los dem�s elementos si es estrictamente necesario

        return matrix;
    }


    private void LogMatrix(Matrix4x4 matrix)
    {
        string matrixString = "Matriz de Transformaci�n de Denavit-Hartenberg:\n";
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matrixString += matrix[i, j].ToString("F4") + "\t";
            }
            matrixString += "\n";
        }
        Debug.Log(matrixString);
    }

    private void UpdateMatrixDisplay(Matrix4x4 matrix)
    {
        if (cellTexts == null || cellTexts.Length < 16)
        {
            Debug.LogError("No hay suficientes textos asignados para mostrar la matriz 4x4");
            return;
        }

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                int index = row * 4 + col;
                if (cellTexts[index] != null)
                {
                    // Formatear a 2 decimales y alinear n�meros
                    float value = matrix[row, col];
                    string formattedValue;

                    // Formato especial para n�meros enteros o con decimales .00
                    if (Mathf.Approximately(value, Mathf.Round(value)))
                    {
                        formattedValue = value.ToString("F0").PadLeft(6);
                    }
                    else
                    {
                        formattedValue = value.ToString("F2").PadLeft(6);
                    }

                    cellTexts[index].text = formattedValue;

                    // Resetear el color primero
                    cellTexts[index].color = Color.white;

                    // Cambiar color para la diagonal
                    if (row == col)
                    {
                        cellTexts[index].color = Color.yellow;
                    }
                }
            }
        }
    }

    public void ActualizaMatriz(int matrixSelector)
    {
        float a1_val = 71.63f;  // Longitud del eslabón 1 (mm)
        float d1_val = 29.6f;   // Desplazamiento articular 1 (mm)
        float d2_val = 8f;      // Desplazamiento articular 2 (mm)
        float a2_val = 105.68f; // Longitud del eslabón 2 (mm)
        float a3_val = 7.86f;   // Longitud del eslabón 3 (mm)
        float d3_val = 18.5f;   // Desplazamiento articular 3 (mm)
        float l1_val = 190.31f; // Longitud adicional (si es necesaria)
        float l2_val = 95.57f;  // Longitud adicional (si es necesaria)
        float T1 = ButtonScr.counterVector[1] * Mathf.Deg2Rad; ;
        float T2 = ButtonScr.counterVector[2] * Mathf.Deg2Rad; ;
        float T3 = ButtonScr.counterVector[3] * Mathf.Deg2Rad; ;
        float T4 = ButtonScr.counterVector[4] * Mathf.Deg2Rad; ;
        float T5 = ButtonScr.counterVector[5] * Mathf.Deg2Rad; ;
        float T6 = ButtonScr.counterVector[6] * Mathf.Deg2Rad; ;

        float off = 0f;

        // Calculate transformation matrices
        Matrix4x4 H1 = DenavitMatrix(a1_val, T1 + off, d1_val, Mathf.PI / 2);
        Matrix4x4 H2 = DenavitMatrix(a2_val, T2 + Mathf.PI / 2 + off, d2_val, 0);
        Matrix4x4 H3 = DenavitMatrix(a3_val, T3 + off, d3_val, -Mathf.PI / 2);
        Matrix4x4 H4 = DenavitMatrix(0, T4 + off, l1_val, -Mathf.PI / 2);
        Matrix4x4 H5 = DenavitMatrix(0, T5 + off, 0, Mathf.PI / 2);
        Matrix4x4 H6 = DenavitMatrix(0, T6 + off, l2_val, 0);
        Matrix4x4 finalMatrix = H1 * H2 * H3 * H4 * H5 * H6;
        


        switch (matrixSelector)
        {
            case 0:
                UpdateMatrixDisplay(finalMatrix);
                LogMatrix(finalMatrix);
                break;
            case 1:
                UpdateMatrixDisplay(H1);
                break;
            case 2:
                UpdateMatrixDisplay(H2);
                break;
            case 3:
                UpdateMatrixDisplay(H3);
                break;
            case 4:
                UpdateMatrixDisplay(H4);
                break;
            case 5:
                UpdateMatrixDisplay(H5);
                break;
            case 6:
                UpdateMatrixDisplay(H6);
                break;


        }

    }
}

