using Unity.Mathematics;
using UnityEngine;
using Utils;
using Data;

namespace Game.Provider
{
    public class GameData : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Sprite[] playerSkins;
        private static GameData instance;

        public static Sprite GetSprite(SpriteType type)
        {
            return instance.sprites[(int)type];
        }
        public static Sprite GetSkin(int skinID)
        {
            return instance.playerSkins[skinID];
        }
        private void Awake()
        {
            instance = this;
        }
    }
}