using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ColorRGBBlock
{
    private readonly Slider sliderR;
    private readonly Slider sliderG;
    private readonly Slider sliderB;
    private readonly TMP_Text textR;
    private readonly TMP_Text textG;
    private readonly TMP_Text textB;
    private float r = 0;
    private float g = 0;
    private float b = 0;

    public float R { get { return r; } set { sliderR.value = value; } }
    public float G { get { return g; } set { sliderG.value = value; } }
    public float B { get { return b; } set { sliderB.value = value; } }

    public ColorRGBBlock(Slider sliderR, Slider sliderG, Slider sliderB, TMP_Text textR, TMP_Text textG, TMP_Text textB,
        UnityAction<float> r, UnityAction<float> g, UnityAction<float> b)
    {
        sliderR.onValueChanged.AddListener(r);
        sliderG.onValueChanged.AddListener(g);
        sliderB.onValueChanged.AddListener(b);
        sliderR.onValueChanged.AddListener(Red);
        sliderG.onValueChanged.AddListener(Green);
        sliderB.onValueChanged.AddListener(Blue);
        this.sliderR = sliderR;
        this.sliderG = sliderG;
        this.sliderB = sliderB;
        this.textR = textR;
        this.textG = textG;
        this.textB = textB;
    }

    public void Red(float value) { textR.text = value.ToString("0.000"); r = value; }
    public void Green(float value) { textG.text = value.ToString("0.000"); g = value; }
    public void Blue(float value) { textB.text = value.ToString("0.000"); b = value; }
}