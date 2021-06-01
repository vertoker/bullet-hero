using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ButtonsSelectBlock
{
    private Button[] buttons;
    private UnityAction<int> action;
    private Color enable, disable;
    private Image[] images;
    private int selectID;
    private readonly int length;

    public int SelectID 
    {
        get { return selectID; } 
        set { ReActivate(selectID, value); selectID = value; } 
    }

    public ButtonsSelectBlock(Button[] buttons, Color enable, Color disable)
    {
        length = buttons.Length;
        this.enable = enable;
        this.disable = disable;
        this.buttons = buttons;
        images = new Image[length];
        for (int i = 0; i < length; i++)
        {
            buttons[i].onClick.AddListener(Activate);
            buttons[i].onClick.AddListener(() => { ReActivate(selectID, i); selectID = i; });
        }
        images[selectID].color = enable;
    }

    public void Mod(UnityAction<int> action, int selectID)
    {
        if (this.action != null)
        {
            for (int i = 0; i < length; i++)
                buttons[i].onClick.RemoveAllListeners();
        }
        for (int i = 0; i < length; i++)
        {
            buttons[i].onClick.AddListener(Activate);
            buttons[i].onClick.AddListener(() => { ReActivate(selectID, i); selectID = i; });
            images[i] = buttons[i].image;
            images[i].color = disable;
        }
        this.action = action;
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