using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CheckpointEditorWindow : MonoBehaviour, IWindow, IInit
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Toggle activeToggle;
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private FloatBlock timeBlock;
    [SerializeField] private Button[] typeRandom;
    [SerializeField] private Button[] SXButs;
    [SerializeField] private Button[] SYButs;
    [SerializeField] private Button[] EXButs;
    [SerializeField] private Button[] EYButs;
    [SerializeField] private Button[] IButs;
    [SerializeField] private TMP_InputField SXField, SYField, EXField, EYField, IField;
    private FloatBlock SXBlock, SYBlock, EXBlock, EYBlock, IBlock;

    private int checkpointSelect;
    private const int length = 5;

    private UnityAction<string> actionName;
    private UnityAction<bool> actionActive;
    private UnityAction[] actionButs = new UnityAction[5];

    public void Init()
    {
        timeBlock = new FloatBlock(timeField, timeButs);
        SXBlock = new FloatBlock(SXField, SXButs);
        SYBlock = new FloatBlock(SYField, SYButs);
        EXBlock = new FloatBlock(EXField, EXButs);
        EYBlock = new FloatBlock(EYField, EYButs);
        IBlock = new FloatBlock(IField, IButs);

        typeRandom[0].onClick.AddListener(() => SetActive(false, false));
        typeRandom[1].onClick.AddListener(() => SetActive(true, false));
        typeRandom[2].onClick.AddListener(() => SetActive(true, true));
        typeRandom[3].onClick.AddListener(() => SetActive(true, true));
        typeRandom[4].onClick.AddListener(() => SetActive(true, false));
    }
    public void CheckpointSelect(int index)
    {
        if (actionName != null)
        {
            nameField.onValueChanged.RemoveListener(actionName);
            activeToggle.onValueChanged.RemoveListener(actionActive);
            for (int i = 0; i < length; i++)
                typeRandom[i].onClick.RemoveListener(actionButs[i]);
        }
        checkpointSelect = index;
    }
    public RectTransform Open()
    {
        parent.gameObject.SetActive(true);
        Checkpoint checkpoint = LevelManager.level.Checkpoints[checkpointSelect];
        actionName = LevelManager.CheckpointName(checkpointSelect);
        nameField.onValueChanged.AddListener(actionName);
        actionActive = LevelManager.CheckpointActive(checkpointSelect);
        activeToggle.onValueChanged.AddListener(actionActive);
        timeBlock.Mod(LevelManager.CheckpointTime(checkpointSelect), checkpoint.Time);

        for (int i = 0; i < length; i++)
        {
            actionButs[i] = LevelManager.PrefabSpriteTypeButton(checkpointSelect, i);
            typeRandom[i].onClick.AddListener(actionButs[i]);
        }

        SXBlock.Mod(LevelManager.CheckpointSX(checkpointSelect), checkpoint.SX);
        SYBlock.Mod(LevelManager.CheckpointSY(checkpointSelect), checkpoint.SY);
        EXBlock.Mod(LevelManager.CheckpointEX(checkpointSelect), checkpoint.EX);
        EYBlock.Mod(LevelManager.CheckpointEY(checkpointSelect), checkpoint.EY);
        IBlock.Mod(LevelManager.CheckpointInterval(checkpointSelect), checkpoint.Interval);
        return parent;
    }
    public void Close()
    {
        parent.gameObject.SetActive(false);
    }
    private void SetActive(bool endBlocks, bool intervalBlock)
    {
        EXBlock.SetActive(endBlocks);
        EYBlock.SetActive(endBlocks);
        IBlock.SetActive(intervalBlock);
    }
}
