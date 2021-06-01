using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FloatBlock
{
    private readonly TMP_InputField block;
    private float currentValue = 0;
    public float CurrentValue { get { return currentValue; } set { block.text = value.ToString(); } }

    public FloatBlock(TMP_InputField block, Button[] buts, UnityAction<string> modBlock)
    {
        block.onValueChanged.AddListener(modBlock);
        block.onValueChanged.AddListener(new UnityAction<string>(UpdateValue));
        buts[0].onClick.AddListener(new UnityAction(But1));
        buts[1].onClick.AddListener(new UnityAction(But2));
        buts[2].onClick.AddListener(new UnityAction(But3));
        buts[3].onClick.AddListener(new UnityAction(But4));
        buts[4].onClick.AddListener(new UnityAction(But5));
    }

    private void But1() { CurrentValue = currentValue - 1f; }
    private void But2() { CurrentValue = currentValue - 0.1f; }
    private void But3() { CurrentValue = EditorTimer.SecCurrent; }
    private void But4() { CurrentValue = currentValue + 0.1f; }
    private void But5() { CurrentValue = currentValue + 1f; }

    private void UpdateValue(string value)
    {
        currentValue = float.Parse(value);
    }
}
