using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ButtonsSelectBlock
{
    private UnityAction<int> action;
    private Color enable, disable;
    private Image[] images;
    private int selectID;

    public int SelectID 
    {
        get { return selectID; } 
        set { ReActivate(selectID, value); selectID = value; } 
    }

    public ButtonsSelectBlock(Button[] buttons, int selectID, UnityAction<int> action, Color enable, Color disable)
    {
        this.action = action;
        this.selectID = selectID;
        this.enable = enable;
        this.disable = disable;
        images = new Image[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(Activate);
            buttons[i].onClick.AddListener(() => { ReActivate(this.selectID, i); this.selectID = i; });
            images[i] = buttons[i].image;
            images[i].color = disable;
        }
        images[selectID].color = enable;
    }

    public void Activate()
    {
        action.Invoke(selectID);
    }
    public void ReActivate(int last, int next)
    {
        images[last].color = disable;
        images[next].color = enable;
    }
}