using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CreateMarkerWindow : MonoBehaviour, IWindow, IInit
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private Button create;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField descriptionField;
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private IntBlock frameBlock;
    [SerializeField] private Slider sliderR, sliderG, sliderB;
    [SerializeField] private TMP_Text textR, textG, textB;
    [SerializeField] private ColorRGBBlock colorBlock;

    private Marker data;

    public void Init()
    {
        create.onClick.AddListener(new UnityAction(Create));
        nameField.onValueChanged.AddListener((string value) => { data.Name = value; });
        descriptionField.onValueChanged.AddListener((string value) => { data.Description = value; });
        frameBlock = new IntBlock(timeField, timeButs);
        colorBlock = new ColorRGBBlock(sliderR, sliderG, sliderB, textR, textG, textB);
    }
    public RectTransform Open()
    {
        parent.gameObject.SetActive(true);
        data = new Marker();
        nameField.text = string.Empty;
        descriptionField.text = string.Empty;
        frameBlock.Mod((string value) => { data.Frame = Utils.String2Int(value); }, 0);
        colorBlock.Mod((float value) => { data.Red = value; }, (float value) => { data.Green = value; }, (float value) => { data.Blue = value; }, 0.5f, 0.3f, 1);
        return parent;
    }
    public void Close()
    {
        parent.gameObject.SetActive(false);
    }

    public void Create()
    {
        LevelManager.level.Markers.Add(data);
        Close();
    }
}
