using System.Linq;
using UnityEngine;
using System.IO;
using Data;

namespace Game.SerializationSaver
{
    public static class LevelSaver
    {
        public static Level Load(string name)
        {
            return Saver.Load<Level>(Application.persistentDataPath, "Levels", name, "level.json");
        }
        public static bool Exists(string name)
        {
            return File.Exists(Path.Combine(Application.persistentDataPath, "Levels", name, "level.json"));
        }
        public static void Save(Level level)
        {
            string path = Path.Combine(Application.persistentDataPath, "Levels", level.LevelData.LevelName, "level.json");
            Saver.Save(level, path);
        }

        public static string[] LoadListAll()
        {
            string path = Path.Combine(Application.persistentDataPath, "Levels");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return new string[0];
            }
            return Directory.GetDirectories(path).Select(GetName).ToArray();
        }
        private static string GetName(string directory)
        {
            return Path.GetFileName(directory);
        }
    }
}
