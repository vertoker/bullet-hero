using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CreateCheckpointWindow : MonoBehaviour, IWindow, IOpen
{

    public void Init()
    {

    }
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
    public IWindow GetIClose()
    {
        return this;
    }
}
