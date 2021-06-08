using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class IntBlock
{
    private TMP_InputField block;
    private UnityAction<string> modBlock;
    public int CurrentValue { get { return int.Parse(block.text); } set { block.text = value.ToString(); } }

    public IntBlock(TMP_InputField block, Button[] buts)
    {
        this.block = block;
        buts[0].onClick.AddListener(new UnityAction(But1));
        buts[1].onClick.AddListener(new UnityAction(But2));
        buts[2].onClick.AddListener(new UnityAction(But3));
        buts[3].onClick.AddListener(new UnityAction(But4));
        buts[4].onClick.AddListener(new UnityAction(But5));
    }

    public void Mod(UnityAction<string> modBlock, int value)
    {
        if (this.modBlock != null)
            block.onValueChanged.RemoveListener(this.modBlock);
        CurrentValue = value;
        block.onValueChanged.AddListener(modBlock);
        this.modBlock = modBlock;
    }

    private const int but_1_0 = Utils.FRAMES_PER_SECOND_INT;
    private const int but_0_1 = Utils.FRAMES_PER_SECOND_INT / 10;
    private void But1() { CurrentValue = int.Parse(block.text) - but_1_0; }
    private void But2() { CurrentValue = int.Parse(block.text) - but_0_1; }
    private void But3() { CurrentValue = EditorTimer.FrameCurrent; }
    private void But4() { CurrentValue = int.Parse(block.text) + but_0_1; }
    private void But5() { CurrentValue = int.Parse(block.text) + but_1_0; }
}