using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    [Serializable]
    public class SettingsData
    {
        public Graphics graphics = Graphics.Basic;
        public bool game_interface = true;
        public bool notifications = true;
        public float music = 1f;

        public SettingsData()
        {
            graphics = Graphics.Basic;
            game_interface = true;
            notifications = true;
            music = 1f;
        }

        public SettingsData Copy()
        {
            return new SettingsData()
            {
                notifications = notifications,
                game_interface = game_interface,
                graphics = graphics,
                music = music
            };
        }
    }

    public enum Graphics : byte
    {
        Basic = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Maximum = 4
    }
}
