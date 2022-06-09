using UnityEngine;
using Utils;
using Data;

namespace Game.Provider
{
    public class GameData : MonoBehaviour
    {
        [SerializeField] private int seed = 666;
        [SerializeField] private Sprite[] sprites;

        private Vector2 borderScreen;
        private FastNoise noise;

        private static GameData instance;
        public static Vector2 BorderScreen => instance.borderScreen;
        public static FastNoise Noise => instance.noise;

        public static Sprite GetSprite(SpriteType type)
        {
            return instance.sprites[(int)type];
        }

        private void Awake()
        {
            float aspect = Screen.width / Screen.height;
            borderScreen = new Vector2(aspect * 10, 10);

            noise = new FastNoise(seed);
            instance = this;
        }
    }
}