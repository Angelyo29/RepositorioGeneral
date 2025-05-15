using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System;
//-0.009
public class ButtonScr : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Referencias a los transformes de los joints
    public Transform transform1; // J1
    public Transform transform2; // J2
    public Transform transform3; // J3
    public Transform transform4; // J4
    public Transform transform5; // J5
    public Transform transform6; // J6



    // UI Components
    public Button increaseButton, decreaseButton;
    public Slider sliderVerificator;


    private int[] counterVector = new int[] { 0, 0, 0, 0, 0,0,0 };
    private bool isIncreasePressed = false;
    private bool isDecreasePressed = false;
    //private bool isOpen = false;
    private float holdDelay = 0.3f;

    void Start()
    {
        // Asignar eventos a los botones

        // Configurar detección de presión continua
        AddButtonHoldDetection(increaseButton.gameObject, true);
        AddButtonHoldDetection(decreaseButton.gameObject, false);

        


        // Inicializar rotaciones (opcional)
        ResetRotations();
    }

    // Método para restablecer rotaciones a valores iniciales
    private void ResetRotations()
    {
        // Establecer rotaciones iniciales para cada joint si es necesario
        transform1.localEulerAngles = new Vector3(90, 0, 0);
        transform2.localEulerAngles = new Vector3(0, 0, 0);
        transform3.localEulerAngles = new Vector3(0, 0, 0);
        transform4.localEulerAngles = new Vector3(0, 0, 0);
        transform5.localEulerAngles = new Vector3(0, 0, 0);
        transform6.localEulerAngles = new Vector3(0, 0, 0);
    }

    private float GetSliderValue()
    {
        return sliderVerificator.value;
    }

    private int rotationObj(int angleRotationLocal, int ObjValueLocal)
    {
        //Debug.Log("El angulo es");
        //Debug.Log(angleRotation);
        if (angleRotationLocal >= 90)
        {
            angleRotationLocal = 90;

        }
        else if(angleRotationLocal <= -90)
        {
            angleRotationLocal = -90;

        }


        switch (ObjValueLocal)
        {
            case 1: // J1
                // Mantiene X=90, modifica Y, mantiene Z=0
                transform1.localEulerAngles = new Vector3(90, angleRotationLocal, 0);
                break;

            case 2: // J2
                // Rota alrededor del eje Y local
                transform2.localEulerAngles = new Vector3(angleRotationLocal, 0, 0);
                break;

            case 3: // J3
                // Rota alrededor del eje X local
                transform3.localEulerAngles = new Vector3(angleRotationLocal, 0, 0);
                break;

            case 4: // J4
                // Rota alrededor del eje Y local
                transform4.localEulerAngles = new Vector3(0, angleRotationLocal, 0);
                break;

            case 5: // J5
                // Rota alrededor del eje X local
                transform5.localEulerAngles = new Vector3(angleRotationLocal, 0, 0);
                break;

            case 6: // J6
                // Rota alrededor del eje Y local
                transform6.localEulerAngles = new Vector3(0, angleRotationLocal, 0);
                break;

            default:
                Debug.LogWarning("Articulación fuera de rango (1-6)");
                break;
        }
        return angleRotationLocal;
    }

    private void AddButtonHoldDetection(GameObject buttonObj, bool isIncrease)
    {
        ButtonHoldHandler handler = buttonObj.AddComponent<ButtonHoldHandler>();
        handler.Initialize(this, isIncrease);
    }

    public void StartButtonHold(bool isIncrease)
    {
        if (isIncrease)
        {
            isIncreasePressed = true;
            StartCoroutine(ContinuousIncrease());
        }
        else
        {
            isDecreasePressed = true;
            StartCoroutine(ContinuousDecrease());
        }
    }

    public void StopButtonHold(bool isIncrease)
    {
        if (isIncrease)
            isIncreasePressed = false;
        else
            isDecreasePressed = false;
    }

    private IEnumerator ContinuousIncrease()
    {
        while (isIncreasePressed)
        {
            IncreaseCounter();
            yield return new WaitForSeconds(holdDelay);
        }
    }

    private IEnumerator ContinuousDecrease()
    {
        while (isDecreasePressed)
        {
            DecreaseCounter();
            yield return new WaitForSeconds(holdDelay);
        }
    }

    void IncreaseCounter()
    {

        int ObjValue = (int)GetSliderValue();
        counterVector[ObjValue] = counterVector[ObjValue] + 5; // Cambia el valor del tercer elemento a 10
        int angleRotation = counterVector[ObjValue];
        counterVector[ObjValue]=rotationObj(angleRotation,ObjValue);
    }

    void DecreaseCounter()
    {
        int ObjValue = (int)GetSliderValue();
        counterVector[ObjValue] = counterVector[ObjValue] - 5; // Cambia el valor del tercer elemento a 10
        int angleRotation = counterVector[ObjValue];
        rotationObj(angleRotation, ObjValue);
    }



    // Implementación de interfaces
    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }
}

// Clase auxiliar para manejar cada botón
public class ButtonHoldHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private ButtonScr mainScript;
    private bool isIncrease;

    public void Initialize(ButtonScr script, bool isIncreaseButton)
    {
        mainScript = script;
        isIncrease = isIncreaseButton;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mainScript.StartButtonHold(isIncrease);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mainScript.StopButtonHold(isIncrease);
    }
}