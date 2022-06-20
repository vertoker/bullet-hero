using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using Data.Enum;
using Audio;
using Data;

namespace Game.SerializationSaver
{
    public static class LevelSaver
    {
        private static readonly Dictionary<AudioLinkType, string> names = new Dictionary<AudioLinkType, string>()
        {
            { AudioLinkType.FilePath, "music" },
            { AudioLinkType.AudioLink, "music_link" },
            { AudioLinkType.Youtube, "music_link_youtube" }
        };
        private static readonly Dictionary<string, AudioFormat> converter = new Dictionary<string, AudioFormat>()
        {
            { ".mp3", AudioFormat.MP3 },
            { ".wav", AudioFormat.WAV },
            { ".ogg", AudioFormat.OGG }
        };
        private static readonly string[] extensions = new string[]
        {
            ".mp3", ".wav", ".ogg"
        };

        public static bool GetAudioPath(LevelData levelData, out string path, out AudioFormat format)
        {
            foreach (var source in levelData.AudioSourcesData)
            {
                foreach (var extension in extensions)
                {
                    format = converter[extension];
                    var file = names[source.LinkType] + extension;
                    path = Path.Combine(Application.persistentDataPath, "Levels", levelData.LevelName, file);
                    if (File.Exists(path))
                        return true;
                }
            }
            format = AudioFormat.None;
            path = string.Empty;
            return false;
        }

        public static string GetFilePath(string nameLevel, string fileName)
        {
            return Path.Combine(Application.persistentDataPath, "Levels", nameLevel, fileName);
        }
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
