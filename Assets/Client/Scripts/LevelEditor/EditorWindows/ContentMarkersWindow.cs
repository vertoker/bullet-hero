using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class ContentMarkersWindow : MonoBehaviour, IWindow
{
    [SerializeField] private RectTransform editor_left_rect;
    [SerializeField] private RectTransform editor_right_rect;

    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
