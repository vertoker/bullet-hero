using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditRotWindow : MonoBehaviour, IWindow
{
    private int prefabIndex, rotIndex;

    public void Init()
    {

    }
    public void SelectPrefab(int index)
    {
        prefabIndex = index;
    }
    public void SelectRot(int index)
    {
        rotIndex = index;
    }
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
}
