using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditScaWindow : MonoBehaviour, IWindow
{
    private int prefabIndex, scaIndex;

    public void Init()
    {

    }
    public void SelectPrefab(int index)
    {
        prefabIndex = index;
    }
    public void SelectSca(int index)
    {
        scaIndex = index;
    }
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
}
