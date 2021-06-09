using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private Spriter[] spriters;
    public static SpriteChanger Instance;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < spriters.Length; i++)
            spriters[i].Activate();
    }

    public void SpriteNext(int index)
    {
        spriters[index].Next();
        spriters[index].Activate();
    }
    public static void SpriteMod(int index, int indexSprite)
    {
        Instance.spriters[index].Activate(indexSprite);
    }

    [System.Serializable]
    private class Spriter
    {
        [SerializeField] private Image image;
        [SerializeField] private int activeSprite;
        [SerializeField] private Sprite[] sprites;

        public void Activate()
        {
            image.sprite = sprites[activeSprite];
        }
        public void Activate(int index)
        {
            image.sprite = sprites[index];
        }
        public void Next()
        {
            activeSprite++;
            if (activeSprite == sprites.Length)
                activeSprite = 0;
        }
    }
}