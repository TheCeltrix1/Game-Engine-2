using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour
{
    public Slider cinematicSlider1;
    public Slider cinematicSlider2;

    void Update()
    {
        cinematicSlider1.value += 1 * Time.deltaTime;
        cinematicSlider2.value += 1 * Time.deltaTime;
    }
}
