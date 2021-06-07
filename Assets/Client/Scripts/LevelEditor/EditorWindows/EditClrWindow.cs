using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditClrWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TMP_Dropdown easingDropdown;
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private FloatBlock timeBlock;
    [SerializeField] private Button[] typeRandom;

    [SerializeField] private Slider sliderSR, sliderSG, sliderSB, sliderSA;
    [SerializeField] private TMP_Text textSR, textSG, textSB, textSA;
    [SerializeField] private GameObject[] objectsStart;
    private ColorRGBABlock colorStartBlock;
    [SerializeField] private Slider sliderER, sliderEG, sliderEB, sliderEA;
    [SerializeField] private TMP_Text textER, textEG, textEB, textEA;
    [SerializeField] private GameObject[] objectsEnd;
    private ColorRGBABlock colorEndBlock;

    [SerializeField] private Button[] IButs;
    [SerializeField] private TMP_InputField IField;
    private FloatBlock IBlock;

    private int prefabIndex, clrIndex;
    private const int length = 3;
    private Clr clr;

    private UnityAction<int> actionEasing;
    private UnityAction[] actionButs = new UnityAction[length];

    public void Init()
    {
        timeBlock = new FloatBlock(timeField, timeButs);
        colorStartBlock = new ColorRGBABlock(sliderSR, sliderSG, sliderSB, sliderSA, textSR, textSG, textSB, textSA, objectsStart);
        colorEndBlock = new ColorRGBABlock(sliderER, sliderEG, sliderEB, sliderEA, textER, textEG, textEB, textEA, objectsEnd);
        IBlock = new FloatBlock(IField, IButs);
        typeRandom[0].onClick.AddListener(() => SetActive(false, false));
        typeRandom[1].onClick.AddListener(() => SetActive(true, false));
        typeRandom[2].onClick.AddListener(() => SetActive(true, true));
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
    public void SelectClr(int index)
    {
        clrIndex = index;
    }
    public void Save()
    { LevelManager.level.Prefabs[prefabIndex].Clr[clrIndex] = clr; }
    public RectTransform Open()
    {
        clr = LevelManager.level.Prefabs[prefabIndex].Clr[clrIndex];
        easingDropdown.value = (int)clr.Easing;

        for (int i = 0; i < length; i++)
        {
            actionButs[i] = () => { clr.RandomType = (ColorRandomType)i; };
            typeRandom[i].onClick.AddListener(actionButs[i]);
        }

        actionEasing = (int value) => { clr.Easing = (EasingType)value; Save(); };
        easingDropdown.onValueChanged.AddListener(actionEasing);
        timeBlock.Mod((string value) => { clr.Time = LevelManager.String2Float(value); Save(); }, clr.Time);
        void sr(float value) { clr.SR = value; Save(); };
        void sg(float value) { clr.SG = value; Save(); };
        void sb(float value) { clr.SB = value; Save(); };
        void sa(float value) { clr.SA = value; Save(); };
        void er(float value) { clr.ER = value; Save(); };
        void eg(float value) { clr.EG = value; Save(); };
        void eb(float value) { clr.EB = value; Save(); };
        void ea(float value) { clr.EA = value; Save(); };
        colorStartBlock.Mod(sr, sg, sb, sa, clr.SR, clr.SG, clr.SB, clr.SA);
        colorEndBlock.Mod(er, eg, eb, ea, clr.ER, clr.EG, clr.EB, clr.EA);
        IBlock.Mod((string value) => { clr.Interval = LevelManager.String2Float(value); Save(); }, clr.Interval);
        return GetComponent<RectTransform>();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    private void SetActive(bool endBlocks, bool intervalBlock)
    {
        colorEndBlock.SetActive(endBlocks);
        IBlock.SetActive(intervalBlock);
    }
}
