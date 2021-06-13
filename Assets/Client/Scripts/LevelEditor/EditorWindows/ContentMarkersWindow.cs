using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class ContentMarkersWindow : MonoBehaviour, IWindow
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
