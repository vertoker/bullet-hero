using Game.SerializationSaver;
using UnityEngine;
using System.IO;
using Data;

namespace Game.Core
{
    public static class SettingsProvider
    {
        public static SettingsData DATA 
        {
            get
            {
                return Load();
            }
            set
            {
                Save(value);
            }
        }

        public static SettingsData Load()
        {
            string path = Path.Combine(Application.persistentDataPath, "settings.json");

            if (!File.Exists(path))
                Saver.Save(new SettingsData(), path);
            return Saver.Load<SettingsData>(path);
        }
        public static void Save(SettingsData settings)
        {
            string path = Path.Combine(Application.persistentDataPath, "settings.json");
            Saver.Save(settings, path);
        }
    }
}