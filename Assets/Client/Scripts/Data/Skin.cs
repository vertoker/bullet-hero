using Utils.Localization;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Skin/New Skin", fileName = "Skin", order = 0)]
    public class Skin : ScriptableObject
    {
        [SerializeField] private LocalizationStringStruct skinName;
        [SerializeField] private int skinPrice;
        [SerializeField] private GameObject skinPreset;
        [SerializeField] private Sprite skinSprite;

        public string GetName(Language language = Language.English)
        {
            return skinName.GetString(language);
        }
        public int Price => skinPrice;
        public GameObject Preset => skinPreset;
        public Sprite Sprite => skinSprite;
    }
}