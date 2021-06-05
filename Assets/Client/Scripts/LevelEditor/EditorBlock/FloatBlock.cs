using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FloatBlock
{
    private TMP_InputField block;
    private GameObject[] objects;
    private UnityAction<string> modBlock;
    public float CurrentValue { get { return float.Parse(block.text); } set { block.text = value.ToString(); } }

    public FloatBlock(TMP_InputField block, Button[] buts)
    {
        this.block = block;
        if (buts.Length == 4)
        {
            objects = new GameObject[]
            {
                block.gameObject,
                buts[0].gameObject,
                buts[1].gameObject,
                buts[2].gameObject,
                buts[3].gameObject
            };
            buts[0].onClick.AddListener(new UnityAction(But1));
            buts[1].onClick.AddListener(new UnityAction(But2));
            buts[2].onClick.AddListener(new UnityAction(But4));
            buts[3].onClick.AddListener(new UnityAction(But5));
        }
        else if (buts.Length == 5)
        {
            objects = new GameObject[]
            {
                block.gameObject,
                buts[0].gameObject,
                buts[1].gameObject,
                buts[2].gameObject,
                buts[3].gameObject,
                buts[4].gameObject
            };
            buts[0].onClick.AddListener(new UnityAction(But1));
            buts[1].onClick.AddListener(new UnityAction(But2));
            buts[2].onClick.AddListener(new UnityAction(But3));
            buts[3].onClick.AddListener(new UnityAction(But4));
            buts[4].onClick.AddListener(new UnityAction(But5));
        }
    }

    public void Mod(UnityAction<string> modBlock, float value)
    {
        if (this.modBlock != null)
            block.onValueChanged.RemoveListener(this.modBlock);
        CurrentValue = value;
        block.onValueChanged.AddListener(modBlock);
        this.modBlock = modBlock;
    }

    public void SetActive(bool active)
    {
        for (int i = 0; i < 6; i++)
            objects[i].SetActive(active);
    }

    private void But1() { CurrentValue = float.Parse(block.text) - 1f; }
    private void But2() { CurrentValue = float.Parse(block.text) - 0.1f; }
    private void But3() { CurrentValue = EditorTimer.SecCurrent; }
    private void But4() { CurrentValue = float.Parse(block.text) + 0.1f; }
    private void But5() { CurrentValue = float.Parse(block.text) + 1f; }
}
