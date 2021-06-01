using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ColorRGBABlock
{
    private readonly Slider sliderR;
    private readonly Slider sliderG;
    private readonly Slider sliderB;
    private readonly Slider sliderA;
    private readonly TMP_Text textR;
    private readonly TMP_Text textG;
    private readonly TMP_Text textB;
    private readonly TMP_Text textA;
    private float r = 0;
    private float g = 0;
    private float b = 0;
    private float a = 0;

    public float R { get { return r; } set { sliderR.value = value; } }
    public float G { get { return g; } set { sliderG.value = value; } }
    public float B { get { return b; } set { sliderB.value = value; } }
    public float A { get { return a; } set { sliderA.value = value; } }

    public ColorRGBABlock(Slider sliderR, Slider sliderG, Slider sliderB, Slider sliderA, 
        TMP_Text textR, TMP_Text textG, TMP_Text textB, TMP_Text textA,
        UnityAction<float> r, UnityAction<float> g, UnityAction<float> b, UnityAction<float> a)
    {
        sliderR.onValueChanged.AddListener(r);
        sliderG.onValueChanged.AddListener(g);
        sliderB.onValueChanged.AddListener(b);
        sliderB.onValueChanged.AddListener(a);
        sliderR.onValueChanged.AddListener(Red);
        sliderG.onValueChanged.AddListener(Green);
        sliderB.onValueChanged.AddListener(Blue);
        sliderB.onValueChanged.AddListener(Alpha);
        this.sliderR = sliderR;
        this.sliderG = sliderG;
        this.sliderB = sliderB;
        this.sliderA = sliderA;
        this.textR = textR;
        this.textG = textG;
        this.textB = textB;
        this.textA = textA;
    }

    public void Red(float value) { textR.text = value.ToString("0.000"); r = value; }
    public void Green(float value) { textG.text = value.ToString("0.000"); g = value; }
    public void Blue(float value) { textB.text = value.ToString("0.000"); b = value; }
    public void Alpha(float value) { textA.text = value.ToString("0.000"); a = value; }
}