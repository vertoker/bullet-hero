using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditPosWindow : MonoBehaviour, IWindow, IOpenDoubleArray
{

    public void Init()
    {

    }
    public RectTransform Open(int index, int index2)
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
