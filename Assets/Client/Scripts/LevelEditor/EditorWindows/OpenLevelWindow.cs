using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevelWindow : MonoBehaviour, IWindow
{
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {
        throw new System.NotImplementedException();
    }
}
