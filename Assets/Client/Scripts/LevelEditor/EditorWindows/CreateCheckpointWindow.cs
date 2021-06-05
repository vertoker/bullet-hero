using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CreateCheckpointWindow : MonoBehaviour, IWindow, IInit
{
    [SerializeField] private Button create;
    [SerializeField] private TMP_InputField nameField;
    [Header("Time")]
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private FloatBlock timeBlock;
    [Header("Position")]
    [SerializeField] private Button[] typeRandom;
    [SerializeField] private Button[] SXButs;
    [SerializeField] private Button[] SYButs;
    [SerializeField] private Button[] EXButs;
    [SerializeField] private Button[] EYButs;
    [SerializeField] private Button[] IButs;
    [SerializeField] private TMP_InputField SXField, SYField, EXField, EYField, IField;
    private FloatBlock SXBlock, SYBlock, EXBlock, EYBlock, IBlock;

    private Checkpoint data;

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
    public RectTransform Open()
    {
        data = new Checkpoint();
        timeBlock.Mod((string value) => { data.Time = LevelManager.String2Float(value); }, 0);
        SXBlock.Mod((string value) => { data.SX = LevelManager.String2Float(value); }, 0);
        SYBlock.Mod((string value) => { data.SY = LevelManager.String2Float(value); }, 0);
        EXBlock.Mod((string value) => { data.EX = LevelManager.String2Float(value); }, 0);
        EYBlock.Mod((string value) => { data.EY = LevelManager.String2Float(value); }, 0);
        IBlock.Mod((string value) => { data.Interval = LevelManager.String2Float(value); }, 0);
        return GetComponent<RectTransform>();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Create()
    {
        LevelManager.level.Checkpoints.Add(data);
        Close();
    }

    private void SetActive(bool endBlocks, bool intervalBlock)
    {
        EXBlock.SetActive(endBlocks);
        EYBlock.SetActive(endBlocks);
        IBlock.SetActive(intervalBlock);
    }
}
