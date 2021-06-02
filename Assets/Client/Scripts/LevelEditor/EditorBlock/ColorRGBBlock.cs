using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ColorRGBBlock
{
    private readonly Slider sliderR, sliderG, sliderB;
    private readonly TMP_Text textR, textG, textB;
    private UnityAction<float> r, g, b;

    public float R { get { return sliderR.value; } set { sliderR.value = value; } }
    public float G { get { return sliderG.value; } set { sliderG.value = value; } }
    public float B { get { return sliderB.value; } set { sliderB.value = value; } }

    public ColorRGBBlock(Slider sliderR, Slider sliderG, Slider sliderB, TMP_Text textR, TMP_Text textG, TMP_Text textB)
    {
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

    public void Mod(UnityAction<float> r, UnityAction<float> g, UnityAction<float> b, float rv, float gv, float bv)
    {
        if (this.r != null)
        {
            sliderR.onValueChanged.RemoveListener(this.r);
            sliderG.onValueChanged.RemoveListener(this.g);
            sliderB.onValueChanged.RemoveListener(this.b);
        }
        sliderR.onValueChanged.AddListener(r);
        sliderG.onValueChanged.AddListener(g);
        sliderB.onValueChanged.AddListener(b);
        sliderR.value = rv;
        sliderG.value = gv;
        sliderB.value = bv;
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public void Red(float value) { textR.text = value.ToString("0.000"); }
    public void Green(float value) { textG.text = value.ToString("0.000"); }
    public void Blue(float value) { textB.text = value.ToString("0.000"); }
}