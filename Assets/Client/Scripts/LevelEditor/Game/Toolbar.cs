using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Toolbar : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private TMP_Text textTimer, textTimeScale;
    private Dictionary<string, ButtonToolbar> toolbarButtons;

    private void Start()
    {
        toolbarButtons = new Dictionary<string, ButtonToolbar>()
        {
            { "timer", new ButtonToolbar(GetBackground(0), GetButtonsStandard(0), true) },
            { "play", new ButtonToolbar(GetBackground(1), GetButtonsStandard(1), true) },
            { "undo", new ButtonToolbar(GetBackground(2), GetButtonsStandard(2), true) },
            { "redo", new ButtonToolbar(GetBackground(3), GetButtonsStandard(3), true) },
            { "tool_timeline", new ButtonToolbar(GetBackground(4), GetButtonsStandard(4), true) },
            { "delete", new ButtonToolbar(GetBackground(5), GetButtonsStandard(5), true) },
            { "create_prefab", new ButtonToolbar(GetBackground(6), GetButtonsStandard(6), true) },
            { "create_checkpoint", new ButtonToolbar(GetBackground(7), GetButtonsStandard(7), true) },
            { "create_marker", new ButtonToolbar(GetBackground(8), GetButtonsStandard(8), true) },
            { "find", new ButtonToolbar(GetBackground(9), GetButtonsStandard(9), true) },
            { "replace", new ButtonToolbar(GetBackground(10), GetButtonsStandard(10), true) },
            { "copy", new ButtonToolbar(GetBackground(11), GetButtonsStandard(11), true) },
            { "paste", new ButtonToolbar(GetBackground(12), GetButtonsStandard(12), false) },
            { "time_scale", new ButtonToolbar(GetBackground(13),
            new Button[] { parent.GetChild(13).GetChild(1).GetComponent<Button>(),
                parent.GetChild(13).GetChild(2).GetComponent<Button>() }, true) },
            { "save", new ButtonToolbar(GetBackground(14), GetButtonsStandard(14), true) },
            { "lvl_manager", new ButtonToolbar(GetBackground(15), GetButtonsStandard(15), true) }
        };
        void actionTimerText(float sec) 
        { textTimer.text = Utils.Sec2Text(sec); }
        EditorTimer.Add(actionTimerText);
        EditorTimer.Add(() => { SpriteChanger.Instance.SpriteNext(0); });
    }

    public void UpdateTextScale()
    {
        textTimeScale.text = EditorTimer.TimeScale.ToString("0.0");
    }
    public void Enable(string name)
    {
        toolbarButtons[name].SetActive(true);
    }
    public void Disable(string name)
    {
        toolbarButtons[name].SetActive(false);
    }

    private Image GetBackground(int index)
    { return parent.GetChild(index).GetComponent<Image>(); }
    private Button[] GetButtonsStandard(int index)
    { return new Button[] { parent.GetChild(index).GetComponent<Button>() }; }

    private class ButtonToolbar
    {
        private Image bg;
        private Button[] buts;
        private int length;
        
        public ButtonToolbar(Image bg, Button[] buts, bool active = true)
        {
            this.bg = bg;
            this.buts = buts;
            SetActive(active);
        }

        public void SetActive(bool active)
        {
            for (int i = 0; i < length; i++)
                buts[i].enabled = active;
        }
    }
}