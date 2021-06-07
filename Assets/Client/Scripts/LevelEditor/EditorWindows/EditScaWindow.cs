using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditScaWindow : MonoBehaviour, IWindow
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

    private int prefabIndex, scaIndex;
    private const int length = 5;
    private Sca sca;

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
    public void SelectSca(int index)
    {
        scaIndex = index;
    }
    public void Save()
    { LevelManager.level.Prefabs[prefabIndex].Sca[scaIndex] = sca; }
    public RectTransform Open()
    {
        sca = LevelManager.level.Prefabs[prefabIndex].Sca[scaIndex];
        easingDropdown.onValueChanged.RemoveAllListeners();
        easingDropdown.value = (int)sca.Easing;

        for (int i = 0; i < length; i++)
        {
            actionButs[i] = () => { sca.RandomType = (VectorRandomType)i; };
            typeRandom[i].onClick.AddListener(actionButs[i]);
        }

        easingDropdown.onValueChanged.AddListener((int value) => { sca.Easing = (EasingType)value; Save(); });
        timeBlock.Mod((string value) => { sca.Time = LevelManager.String2Float(value); Save(); }, sca.Time);
        SXBlock.Mod((string value) => { sca.SX = LevelManager.String2Float(value); Save(); }, sca.SX);
        SYBlock.Mod((string value) => { sca.SY = LevelManager.String2Float(value); Save(); }, sca.SY);
        EXBlock.Mod((string value) => { sca.EX = LevelManager.String2Float(value); Save(); }, sca.EX);
        EYBlock.Mod((string value) => { sca.EY = LevelManager.String2Float(value); Save(); }, sca.EY);
        IBlock.Mod((string value) => { sca.Interval = LevelManager.String2Float(value); Save(); }, sca.Interval);
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
