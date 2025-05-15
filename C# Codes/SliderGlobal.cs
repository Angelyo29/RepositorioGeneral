using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderGlobal : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public static int value;
    void Start(){
        value = ((int)slider.value);


    }

    // Update is called once per frame
    void Update(){
        value = ((int)slider.value);
    }
}
