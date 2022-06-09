using UnityEngine;
using Data;

namespace Game
{
    public class DataProvider : MonoBehaviour
    {
        private Level level;

        public Level Level => level;

        public void Load(Level level)
        {
            this.level = level;
        }
    }
}