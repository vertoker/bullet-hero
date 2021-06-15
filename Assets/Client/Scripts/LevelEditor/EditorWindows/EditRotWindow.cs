using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditRotWindow : MonoBehaviour, IWindow
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private TMP_Dropdown easingDropdown;
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private IntBlock frameBlock;
    [SerializeField] private Button[] typeRandom;
    [SerializeField] private Button[] SAButs;
    [SerializeField] private Button[] EAButs;
    [SerializeField] private Button[] IButs;
    [SerializeField] private TMP_InputField SAField, EAField, IField;
    private FloatBlock SABlock, EABlock, IBlock;

    private int prefabIndex, rotIndex;
    private const int length = 4;
    private Rot rot;

    private UnityAction<int> actionEasing;
    private UnityAction[] actionButs = new UnityAction[length];

    public void Init()
    {
        frameBlock = new IntBlock(timeField, timeButs);
        SABlock = new FloatBlock(SAField, SAButs);
        EABlock = new FloatBlock(EAField, EAButs);
        IBlock = new FloatBlock(IField, IButs);
        typeRandom[0].onClick.AddListener(() => SetActive(false, false));
        typeRandom[1].onClick.AddListener(() => SetActive(true, false));
        typeRandom[2].onClick.AddListener(() => SetActive(true, true));
        typeRandom[3].onClick.AddListener(() => SetActive(true, false));
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
    public void SelectRot(int index)
    {
        rotIndex = index;
    }
    public void Save()
    { LevelManager.level.Prefabs[prefabIndex].Rot[rotIndex] = rot; }
    public RectTransform Open()
    {
        parent.gameObject.SetActive(true);
        rot = LevelManager.level.Prefabs[prefabIndex].Rot[rotIndex];
        easingDropdown.value = (int)rot.Easing;

        for (int i = 0; i < length; i++)
        {
            actionButs[i] = () => { rot.RandomType = (FloatRandomType)i; };
            typeRandom[i].onClick.AddListener(actionButs[i]);
        }

        actionEasing = (int value) => { rot.Easing = (EasingType)value; Save(); };
        easingDropdown.onValueChanged.AddListener(actionEasing);
        frameBlock.Mod((string value) => { rot.Frame = Utils.String2Int(value); Save(); }, rot.Frame);
        SABlock.Mod((string value) => { rot.SA = Utils.String2Float(value); Save(); }, rot.SA);
        EABlock.Mod((string value) => { rot.EA = Utils.String2Float(value); Save(); }, rot.EA);
        IBlock.Mod((string value) => { rot.Interval = Utils.String2Float(value); Save(); }, rot.Interval);
        return parent;
    }
    public void Close()
    {
        parent.gameObject.SetActive(false);
    }
    private void SetActive(bool endBlocks, bool intervalBlock)
    {
        EABlock.SetActive(endBlocks);
        IBlock.SetActive(intervalBlock);
    }
}
