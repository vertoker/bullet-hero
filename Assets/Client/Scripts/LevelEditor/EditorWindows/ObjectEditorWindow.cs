using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ObjectEditorWindow : MonoBehaviour, IWindow, IOpenSingleArray
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Toggle activeToggle;
    [SerializeField] private Toggle colliderToggle;
    private UnityAction<string> nameFieldAction;
    private UnityAction<bool> activeToggleAction;
    private UnityAction<bool> colliderToggleAction;
    [Header("Start time")]
    [SerializeField] private TMP_InputField startTimeField;
    [SerializeField] private Button[] startTimeButs;
    private FloatBlock startTimeBlock;
    [Header("End time")]
    [SerializeField] private TMP_InputField endTimeField;
    [SerializeField] private Button[] endTimeButs;
    [SerializeField] private FloatBlock endTimeBlock;
    [Header("Parent")]
    [SerializeField] private Button parentButton;
    [SerializeField] private Button cancelParentButton;
    [Header("Anchor")]
    [SerializeField] private Button[] anchorButtons;
    private ButtonsSelectBlock anchors;
    [Header("Shape")]
    [SerializeField] private Button[] shapeButtons;
    private ButtonsSelectBlock shapes;

    public void Init()
    {
        startTimeBlock = new FloatBlock(startTimeField, startTimeButs);
        endTimeBlock = new FloatBlock(endTimeField, endTimeButs);
        anchors = new ButtonsSelectBlock(anchorButtons, WindowManager.butEnable, WindowManager.butDisable);
        shapes = new ButtonsSelectBlock(shapeButtons, WindowManager.butEnable, WindowManager.butDisable);
    }
    public RectTransform Open(int index)
    {
        Prefab prefab = LevelManager.level.Prefabs[index];
        nameFieldAction = LevelManager.PrefabName(index);
        activeToggleAction = LevelManager.PrefabActive(index);
        colliderToggleAction = LevelManager.PrefabCollider(index);

        nameField.onValueChanged.AddListener(nameFieldAction);
        activeToggle.onValueChanged.AddListener(activeToggleAction);
        colliderToggle.onValueChanged.AddListener(colliderToggleAction);

        startTimeBlock.Mod(LevelManager.PrefabStartFrame(index));
        endTimeBlock.Mod(LevelManager.PrefabEndFrame(index));

        anchors.Mod(LevelManager.PrefabAnchorPresets(index), (int)prefab.Anchor);
        shapes.Mod(LevelManager.PrefabSpriteType(index), (int)prefab.SpriteType);
        return GetComponent<RectTransform>();
    }
    public void Close()
    {
        if (nameFieldAction != null)
        {
            nameField.onValueChanged.RemoveListener(nameFieldAction);
            activeToggle.onValueChanged.RemoveListener(activeToggleAction);
            colliderToggle.onValueChanged.RemoveListener(colliderToggleAction);
        }
    }
    public IWindow GetIClose()
    {
        return this;
    }
}
