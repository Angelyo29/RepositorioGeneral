using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonScript : MonoBehaviour
{
    public Transform fingerGripper1;
    public Transform fingerGripper2;
    public Transform gearGripper;

    public Button GripperButton;
    private bool isOpen = false;
    private bool isMoving = false;

    // Posiciones para abierto y cerrado
    private Vector3 openPos1 = new Vector3(0.009f, 0f, 0f);
    private Vector3 openPos2 = new Vector3(-0.009f, 0f, 0f);
    private Vector3 closedPos = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        GripperButton.onClick.AddListener(CheckCounter);
    }

    public void CheckCounter()
    {
        if (!isMoving)
        {
            if (isOpen == false)
            {
                StartCoroutine(MoveGripper(openPos1, openPos2));
                isOpen = true;
            }
            else
            {
                StartCoroutine(MoveGripper(closedPos, closedPos));
                isOpen = false;
            }
        }
    }

    IEnumerator MoveGripper(Vector3 targetPos1, Vector3 targetPos2)
    {
        isMoving = true;
        int steps = 20;
        Vector3 startPos1 = fingerGripper1.localPosition;
        Vector3 startPos2 = fingerGripper2.localPosition;

        for (int i = 1; i <= steps; i++)
        {
            float t = (float)i / steps;
            fingerGripper1.localPosition = Vector3.Lerp(startPos1, targetPos1, t);
            fingerGripper2.localPosition = Vector3.Lerp(startPos2, targetPos2, t);
            yield return null; // Espera al siguiente frame
        }

        isMoving = false;
    }
}
