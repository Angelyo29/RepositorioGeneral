using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIRangeSliderNamespace;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] public TextMeshProUGUI sliderText;
    //private int maxSliderAmount = 6;

    private void Start()
    {
        // Inicializar el texto con el valor actual
        sliderText.text = slider.value.ToString();

        // Suscribir el método SliderChange al evento del Slider
        slider.onValueChanged.AddListener(SliderChange);
    }

    public void SliderChange(float value)
    {
        // Actualizar el texto con el nuevo valor
        sliderText.text = value.ToString();
    }

    // Opcional: Limpiar la suscripción cuando el objeto sea destruido
    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(SliderChange);
    }
}