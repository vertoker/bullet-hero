using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class CreateMarkerWindow : MonoBehaviour, IWindow, IOpen
{
    [SerializeField] private Button create;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField descriptionField;
    [Header("Time")]
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private FloatBlock timeBlock;

    private Marker data;

    public void Init()
    {
        create.onClick.AddListener(new UnityAction(Create));
        timeBlock = new FloatBlock(timeField, timeButs);
        nameField.onValueChanged.AddListener((string value) => { data.Name = value; });
        descriptionField.onValueChanged.AddListener((string value) => { data.Description = value; });
        timeBlock.Mod((string value) => { data.Time = LevelManager.String2Float(value); });
    }
    public RectTransform Open()
    {
        data = new Marker();
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
    public IWindow GetIClose()
    {
        return this;
    }

    private void Create()
    {
        LevelManager.level.Markers.Add(data);
        Close();
    }
}
