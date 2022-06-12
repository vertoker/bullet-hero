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
            return Saver.Load<Level>(Application.dataPath, "Levels", name, "level.json");
        }
        public static bool Exists(string name)
        {
            return File.Exists(Path.Combine(Application.dataPath, "Levels", name, "level.json"));
        }
        public static void Save(Level level)
        {
            string path = Path.Combine(Application.dataPath, "Levels", level.LevelData.LevelName, "level.json");
            Saver.Save(level, path);
        }

        public static string[] LoadListAll()
        {
            string path = Path.Combine(Application.dataPath, "Levels");
            return Directory.GetDirectories(path).Select(GetName).ToArray();
        }
        private static string GetName(string directory)
        {
            return Path.GetFileName(directory);
        }
    }
}
