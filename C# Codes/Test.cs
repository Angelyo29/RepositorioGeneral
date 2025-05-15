using System.Collections;
using System.Collections.Generic;
using UIRangeSliderNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Transform Transform;
    public Slider Slider;
    public float Scale = 10;
    private float theta = 0;
    public Slider SliderO;
    public UIRangeSlider SliderTwo;
    // Start is called before the first frame update
    void Start()
    {
        theta = Transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        theta = Slider.value * Scale;
        Transform.rotation = Quaternion.Euler(90, theta, 90);
    }
}
