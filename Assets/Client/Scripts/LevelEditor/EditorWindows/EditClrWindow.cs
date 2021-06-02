using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditClrWindow : MonoBehaviour, IWindow
{
    private int prefabIndex, clrIndex;

    public void Init()
    {

    }
    public void SelectPrefab(int index)
    {
        prefabIndex = index;
    }
    public void SelectClr(int index)
    {
        clrIndex = index;
    }
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
}
