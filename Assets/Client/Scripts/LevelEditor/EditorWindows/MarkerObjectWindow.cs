using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MarkerObjectWindow : MonoBehaviour, IWindow
{
    private int markerIndex;
    public void Init()
    {

    }
    public void MarkerSelect(int index)
    {
        markerIndex = index;
    }
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
}
