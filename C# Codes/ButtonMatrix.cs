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
    //[SerializeField] private TextMeshProUGUI sliderText = null;

    private int n = 0;

    void Start()
    {
        ButtonAxis.onClick.AddListener(ChangeState);
        if (animator == null)
            animator = GetComponent<Animator>();
        if (ButtonAxis == null)
            ButtonAxis = GetComponent<Button>();
    }


    //jose

    public void ChangeState()
    {
        switch (n)
        {
            case 0:
                ButtonAxis.image.color = color1;
                //animator.SetTrigger("FirstSelect");
                Debug.Log("Activando color rojo y animaci�n FirstSelect");
                break;
            case 1:
                ButtonAxis.image.color = color2;
                //animator.SetTrigger("SecondSelect");
                Debug.Log("Activando color verde y animaci�n SecondSelect");


                float a1_val = 71.63f;
                float d1_val = 29.6f;
                float T1 = 0.5f;
                float off = 0.1f;

                Matrix4x4 H1 = DenavitMatrix(a1_val, T1 + off, d1_val, Mathf.PI / 2);
 

                panel.SetActive(true);
                LogMatrix(H1);
                UpdateMatrixDisplay(H1);
                break;
            case 2:
                ButtonAxis.image.color = color3;
                animator.SetTrigger("Reset");
                Debug.Log("Activando color azul y animaci�n Reset");
                panel.SetActive(false);
                n = -1;
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
}