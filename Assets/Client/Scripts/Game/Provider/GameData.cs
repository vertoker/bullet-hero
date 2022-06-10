using Unity.Mathematics;
using UnityEngine;
using Utils;
using Data;

namespace Game.Provider
{
    public class GameData : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        private static GameData instance;

        public static Sprite GetSprite(SpriteType type)
        {
            return instance.sprites[(int)type];
        }
        private void Awake()
        {
            instance = this;
        }
    }
}