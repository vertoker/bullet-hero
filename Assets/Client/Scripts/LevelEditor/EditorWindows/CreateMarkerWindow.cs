using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class CreateMarkerWindow : MonoBehaviour, IWindow, IInit
{
    [SerializeField] private Button create;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField descriptionField;
    [Header("Time")]
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private FloatBlock timeBlock;
    [Header("Color")]
    [SerializeField] private Slider sliderR, sliderG, sliderB;
    [SerializeField] private TMP_Text textR, textG, textB;
    [SerializeField] private ColorRGBBlock colorBlock;

    private Marker data;

    public void Init()
    {
        create.onClick.AddListener(new UnityAction(Create));
        nameField.onValueChanged.AddListener((string value) => { data.Name = value; });
        descriptionField.onValueChanged.AddListener((string value) => { data.Description = value; });
        timeBlock = new FloatBlock(timeField, timeButs);
        colorBlock = new ColorRGBBlock(sliderR, sliderG, sliderB, textR, textG, textB);
    }
    public RectTransform Open()
    {
        data = new Marker();
        nameField.text = string.Empty;
        descriptionField.text = string.Empty;
        timeBlock.Mod((string value) => { data.Time = LevelManager.String2Float(value); }, 0);
        colorBlock.Mod((float value) => { data.Red = value; }, (float value) => { data.Green = value; }, (float value) => { data.Blue = value; }, 0.5f, 0.3f, 1);
        return GetComponent<RectTransform>();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Create()
    {
        LevelManager.level.Markers.Add(data);
        Close();
    }
}
