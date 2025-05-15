using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIRangeSliderNamespace;
using UnityEngine;

public class Sliders : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;
    [SerializeField] private UIRangeSlider MaxValueHandle = null; // Referencia al Slider
    [SerializeField] private int maxSliderAmount = 6;

    private void Start()
    {
        // Configura el valor máximo del Slider
        MaxValueHandle.maxLimit = maxSliderAmount;

        // Inicializa el texto con el valor inicial del Slider
        sliderText.text = MaxValueHandle.valueMax.ToString("0");

        // Añade un listener para detectar cambios en el Slider
        MaxValueHandle.onMaxValueChanged.AddListener(SliderChange);
    }

    public void SliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        sliderText.text = localValue.ToString("0");
        //Debug.Log(sliderText);
    }
}
