using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditPosWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TMP_Dropdown easingDropdown;
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

    private int prefabIndex, posIndex;
    private const int length = 5;
    private Pos pos;

    private UnityAction<int> actionEasing;
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
    public void SelectPrefab(int index)
    {
        if (actionEasing != null)
        {
            easingDropdown.onValueChanged.RemoveListener(actionEasing);
            for (int i = 0; i < length; i++)
                typeRandom[i].onClick.RemoveListener(actionButs[i]);
        }
        prefabIndex = index;
    }
    public void SelectPos(int index)
    {
        posIndex = index;
    }
    public void Save()
    { LevelManager.level.Prefabs[prefabIndex].Pos[posIndex] = pos; }
    public RectTransform Open()
    {
        pos = LevelManager.level.Prefabs[prefabIndex].Pos[posIndex];
        easingDropdown.onValueChanged.RemoveAllListeners();
        easingDropdown.value = (int)pos.Easing;

        for (int i = 0; i < length; i++)
        {
            actionButs[i] = () => { pos.RandomType = (VectorRandomType)i; };
            typeRandom[i].onClick.AddListener(actionButs[i]);
        }

        easingDropdown.onValueChanged.AddListener((int value) => { pos.Easing = (EasingType)value; Save(); });
        timeBlock.Mod((string value) => { pos.Time = LevelManager.String2Float(value); Save(); }, pos.Time);
        SXBlock.Mod((string value) => { pos.SX = LevelManager.String2Float(value); Save(); }, pos.SX);
        SYBlock.Mod((string value) => { pos.SY = LevelManager.String2Float(value); Save(); }, pos.SY);
        EXBlock.Mod((string value) => { pos.EX = LevelManager.String2Float(value); Save(); }, pos.EX);
        EYBlock.Mod((string value) => { pos.EY = LevelManager.String2Float(value); Save(); }, pos.EY);
        IBlock.Mod((string value) => { pos.Interval = LevelManager.String2Float(value); Save(); }, pos.Interval);
        return GetComponent<RectTransform>();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    private void SetActive(bool endBlocks, bool intervalBlock)
    {
        EXBlock.SetActive(endBlocks);
        EYBlock.SetActive(endBlocks);
        IBlock.SetActive(intervalBlock);
    }
}
