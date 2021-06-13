using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MarkerEditorWindow : MonoBehaviour, IWindow, IInit
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField descriptionField;
    [SerializeField] private TMP_InputField timeField;
    [SerializeField] private Button[] timeButs;
    private FloatBlock timeBlock;
    [SerializeField] private Slider sliderR, sliderG, sliderB;
    [SerializeField] private TMP_Text textR, textG, textB;
    [SerializeField] private ColorRGBBlock colorBlock;

    private int markerSelect;

    private UnityAction<string> actionName;
    private UnityAction<string> actionDescription;

    public void Init()
    {
        timeBlock = new FloatBlock(timeField, timeButs);
        colorBlock = new ColorRGBBlock(sliderR, sliderG, sliderB, textR, textG, textB);
    }
    public void MarkerSelect(int index)
    {
        if (actionName != null)
        {
            nameField.onValueChanged.RemoveListener(actionName);
            descriptionField.onValueChanged.RemoveListener(actionDescription);
        }
        markerSelect = index;
    }
    public RectTransform Open()
    {
        parent.gameObject.SetActive(true);
        Marker marker = LevelManager.level.Markers[markerSelect];
        actionName = LevelManager.MarkerName(markerSelect);
        nameField.onValueChanged.AddListener(actionName);
        actionDescription = LevelManager.MarkerDescription(markerSelect);
        descriptionField.onValueChanged.AddListener(actionDescription);

        timeBlock.Mod(LevelManager.MarkerTime(markerSelect), marker.Time);
        colorBlock.Mod(LevelManager.MarkerColorR(markerSelect), 
            LevelManager.MarkerColorG(markerSelect),
            LevelManager.MarkerColorB(markerSelect),
            marker.Red, marker.Green, marker.Blue);
        return parent;
    }
    public void Close()
    {
        parent.gameObject.SetActive(false);
    }
}
