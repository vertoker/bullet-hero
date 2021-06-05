using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ObjectEditorWindow : MonoBehaviour, IWindow, IInit
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
    private IntBlock startTimeBlock;
    [Header("End time")]
    [SerializeField] private TMP_InputField endTimeField;
    [SerializeField] private Button[] endTimeButs;
    private IntBlock endTimeBlock;
    [Header("Parent")]
    [SerializeField] private Button parentButton;
    [SerializeField] private Button cancelParentButton;
    [Header("Anchor")]
    [SerializeField] private Button[] anchorButtons;
    private ButtonsSelectBlock anchors;
    [Header("Shape")]
    [SerializeField] private Button[] shapeButtons;
    private ButtonsSelectBlock shapes;

    private int prefabSelect;

    public void Init()
    {
        startTimeBlock = new IntBlock(startTimeField, startTimeButs);
        endTimeBlock = new IntBlock(endTimeField, endTimeButs);
        anchors = new ButtonsSelectBlock(anchorButtons, WindowManager.butEnable, WindowManager.butDisable);
        shapes = new ButtonsSelectBlock(shapeButtons, WindowManager.butEnable, WindowManager.butDisable);
    }
    public void PrefabSelect(int index)
    {
        prefabSelect = index;
    }
    public RectTransform Open()
    {
        Prefab prefab = LevelManager.level.Prefabs[prefabSelect];
        nameFieldAction = LevelManager.PrefabName(prefabSelect);
        activeToggleAction = LevelManager.PrefabActive(prefabSelect);
        colliderToggleAction = LevelManager.PrefabCollider(prefabSelect);

        nameField.onValueChanged.AddListener(nameFieldAction);
        activeToggle.onValueChanged.AddListener(activeToggleAction);
        colliderToggle.onValueChanged.AddListener(colliderToggleAction);

        startTimeBlock.Mod(LevelManager.PrefabStartFrame(prefabSelect), prefab.StartFrame);
        endTimeBlock.Mod(LevelManager.PrefabEndFrame(prefabSelect), prefab.EndFrame);

        anchors.Mod(LevelManager.PrefabAnchorPresets(prefabSelect), (int)prefab.Anchor);
        shapes.Mod(LevelManager.PrefabSpriteType(prefabSelect), (int)prefab.SpriteType);
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
}
