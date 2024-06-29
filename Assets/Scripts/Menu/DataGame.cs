using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Animations;

public class DataGame : MonoBehaviour
{
    public Player player;
    public Text[] moneyTexts;
    public Skin[] skins;
    public Track[] levels;
    public GraphicAsset[] assets;
    public int IdSelect { get; set; } = 0;

    public void SetSelectID(int id)
    {
        IdSelect = id;
        player.SetPlayerPreset();
        return;
    }
    public GameObject GetPlayerPresets()
    {
        return skins[IdSelect].playerPreset;
    }
    public Sprite GetSpriteSkin(int id)
    {
        return skins[id].sprite;
    }

    public int GetPriceSkin(int id)
    {
        return skins[id].price;
    }
    public void UpdateMoney()
    {
        int money = PlayerPrefs.GetInt("money");
        for (int i = 0; i < moneyTexts.Length; i++)
        {
            moneyTexts[i].text = money.ToString();
        }
        return;
    }
}

[System.Serializable]
public class Skin
{
    public Sprite sprite;
    public GameObject playerPreset;
    public int price;
}

[System.Serializable]
public class Track
{
    public GameObject animator;
    public AudioClip clip;
    public string title;
    public string musician;
    public float startFadeOut;
    public float endFadeOut;
}