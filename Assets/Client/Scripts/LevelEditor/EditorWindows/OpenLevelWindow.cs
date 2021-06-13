using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevelWindow : MonoBehaviour, IWindow
{
    [SerializeField] private RectTransform parent;
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
