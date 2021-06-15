using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditScaWindow : MonoBehaviour, IWindow
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private TMP_Dropdown easingDropdown;
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private IntBlock frameBlock;
    [SerializeField] private Button[] typeRandom;
    [SerializeField] private Button[] SXButs;
    [SerializeField] private Button[] SYButs;
    [SerializeField] private Button[] EXButs;
    [SerializeField] private Button[] EYButs;
    [SerializeField] private Button[] IButs;
    [SerializeField] private TMP_InputField SXField, SYField, EXField, EYField, IField;
    private FloatBlock SXBlock, SYBlock, EXBlock, EYBlock, IBlock;

    private int prefabIndex, scaIndex;
    private const int length = 5;
    private Sca sca;

    private UnityAction<int> actionEasing;
    private UnityAction[] actionButs = new UnityAction[length];

    public void Init()
    {
        frameBlock = new IntBlock(timeField, timeButs);
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
    public void SelectSca(int index)
    {
        scaIndex = index;
    }
    public void Save()
    { LevelManager.level.Prefabs[prefabIndex].Sca[scaIndex] = sca; }
    public RectTransform Open()
    {
        parent.gameObject.SetActive(true);
        sca = LevelManager.level.Prefabs[prefabIndex].Sca[scaIndex];
        easingDropdown.value = (int)sca.Easing;

        for (int i = 0; i < length; i++)
        {
            actionButs[i] = () => { sca.RandomType = (VectorRandomType)i; };
            typeRandom[i].onClick.AddListener(actionButs[i]);
        }

        actionEasing = (int value) => { sca.Easing = (EasingType)value; Save(); };
        easingDropdown.onValueChanged.AddListener(actionEasing);
        frameBlock.Mod((string value) => { sca.Frame = Utils.String2Int(value); Save(); }, sca.Frame);
        SXBlock.Mod((string value) => { sca.SX = Utils.String2Float(value); Save(); }, sca.SX);
        SYBlock.Mod((string value) => { sca.SY = Utils.String2Float(value); Save(); }, sca.SY);
        EXBlock.Mod((string value) => { sca.EX = Utils.String2Float(value); Save(); }, sca.EX);
        EYBlock.Mod((string value) => { sca.EY = Utils.String2Float(value); Save(); }, sca.EY);
        IBlock.Mod((string value) => { sca.Interval = Utils.String2Float(value); Save(); }, sca.Interval);
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
