using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour, IWindow, IInit
{
    [SerializeField] private RectTransform parent;

    public void Init()
    {

    }
    public RectTransform Open()
    {
        parent.gameObject.SetActive(true);
        return parent;
    }
    public void Close()
    {
        parent.gameObject.SetActive(false);
    }
}
