using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditPosWindow : MonoBehaviour, IWindow
{
    private int prefabIndex, posIndex;

    public void Init()
    {

    }
    public void SelectPrefab(int index)
    {
        prefabIndex = index;
    }
    public void SelectPos(int index)
    {
        posIndex = index;
    }
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
}
