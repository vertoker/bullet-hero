using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ObjectEditorWindow : MonoBehaviour, IWindow
{
    public TMP_InputField nameField;
    public Toggle activeToggle;
    public Toggle colliderToggle;
    [Header("Start time")]
    public TMP_InputField startTimeField;
    public Button[] startTimeButs;
    private FloatBlock startTimeBlock;
    [Header("End time")]
    public TMP_InputField endTimeField;
    public Button[] endTimeButs;
    public FloatBlock endTimeBlock;
    [Header("Parent")]
    public Button parentButton;
    public Button cancelParentButton;
    [Header("Anchor")]
    public Button[] anchorButtons;
    private ButtonsSelectBlock anchors;
    [Header("Shape")]
    public Button[] shapeButtons;
    private ButtonsSelectBlock shapes;

    public void Init()
    {
        /*startTimeBlock = new FloatBlock(startTimeField, startTimeButs, LevelManager.PrefabStartFrame(index));
        endTimeBlock = new FloatBlock(endTimeField, endTimeButs, LevelManager.PrefabEndFrame(index));*/
    }
    public RectTransform Open()
    {
        /*nameField.onValueChanged.AddListener(LevelManager.PrefabName(index));
        activeToggle.onValueChanged.AddListener(LevelManager.PrefabActive(index));
        colliderToggle.onValueChanged.AddListener(LevelManager.PrefabCollider(index));*/
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
}
