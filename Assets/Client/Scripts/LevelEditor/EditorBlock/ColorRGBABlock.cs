using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ColorRGBABlock
{
    private readonly Slider sliderR, sliderG, sliderB, sliderA;
    private readonly TMP_Text textR, textG, textB, textA;
    private UnityAction<float> r, g, b, a;
    private readonly GameObject[] objects;

    public float R { get { return sliderR.value; } set { sliderR.value = value; } }
    public float G { get { return sliderG.value; } set { sliderG.value = value; } }
    public float B { get { return sliderB.value; } set { sliderB.value = value; } }

    public ColorRGBABlock(Slider sliderR, Slider sliderG, Slider sliderB, Slider sliderA, TMP_Text textR, TMP_Text textG, TMP_Text textB, TMP_Text textA, GameObject[] objects)
    {
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
        this.objects = objects;
    }

    public void Mod(UnityAction<float> r, UnityAction<float> g, UnityAction<float> b, UnityAction<float> a, float rv, float gv, float bv, float ba)
    {
        if (this.r != null)
        {
            sliderR.onValueChanged.RemoveListener(this.r);
            sliderG.onValueChanged.RemoveListener(this.g);
            sliderB.onValueChanged.RemoveListener(this.b);
            sliderA.onValueChanged.RemoveListener(this.a);
        }
        sliderR.onValueChanged.AddListener(r);
        sliderG.onValueChanged.AddListener(g);
        sliderB.onValueChanged.AddListener(b);
        sliderA.onValueChanged.AddListener(a);
        sliderR.value = rv;
        sliderG.value = gv;
        sliderB.value = bv;
        sliderA.value = ba;
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public void SetActive(bool active)
    {
        for (int i = 0; i < objects.Length; i++)
            objects[i].SetActive(active);
    }

    public void Red(float value) { textR.text = value.ToString("0.000"); }
    public void Green(float value) { textG.text = value.ToString("0.000"); }
    public void Blue(float value) { textB.text = value.ToString("0.000"); }
    public void Alpha(float value) { textA.text = value.ToString("0.000"); }
}