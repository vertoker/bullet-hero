using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour, IWindow, IInit
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
        gameObject.SetActive(false);
    }
}
