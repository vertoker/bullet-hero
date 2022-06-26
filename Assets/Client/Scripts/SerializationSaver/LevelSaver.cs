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
        private const string LEVEL_NAME = "level.json";
        private const string LEVEL_PREVIEW = "preview.txt";
        private const char POINT = '.';

        private static readonly Dictionary<AudioLinkType, string> names = new Dictionary<AudioLinkType, string>()
        {
            { AudioLinkType.FilePath, "music" },
            { AudioLinkType.AudioLink, "music_link" },
            { AudioLinkType.Youtube, "music_link_youtube" }
        };
        private static readonly Dictionary<string, AudioFormat> converter = new Dictionary<string, AudioFormat>()
        {
            { "mp3", AudioFormat.MP3 },
            { "wav", AudioFormat.WAV },
            { "ogg", AudioFormat.OGG }
        };
        private static readonly string[] extensions = new string[]
        {
            "mp3", "wav", "ogg"
        };

        public static bool GetAudioPath(IdentificationData identificationData, AudioData audioData, out string path, out AudioFormat format)
        {
            foreach (var source in audioData.AudioSourcesData)
            {
                if (source.LinkType == AudioLinkType.FilePath)
                {
                    if (File.Exists(source.LinkPath))
                    {
                        path = source.LinkPath;
                        if (converter.TryGetValue(source.LinkPath.Split(POINT)[^1], out format))
                            return true;
                        else
                            continue;
                    }
                }
                foreach (var extension in extensions)
                {
                    format = converter[extension];
                    var file = string.Join(POINT, names[source.LinkType], extension);
                    path = GetFilePath(identificationData.Identificator, file);
                    if (File.Exists(path))
                        return true;
                }
            }
            format = AudioFormat.None;
            path = string.Empty;
            return false;
        }

        public static string GetFilePath(long levelID, string fileName)
        {
            return Path.Combine(Application.persistentDataPath, "Levels", levelID.ToString(), fileName);
        }
        public static Level Load(long id)
        {
            return Saver.Load<Level>(GetFilePath(id, LEVEL_NAME));
        }
        public static bool Exists(long id)
        {
            return File.Exists(GetFilePath(id, LEVEL_NAME));
        }
        public static void Save(Level level)
        {
            long id = level.IdentificationData.Identificator;
            Saver.Save(level, GetFilePath(id, LEVEL_NAME));
            PreviewSaver.Save(id, level.LevelData.LevelName, GetFilePath(id, LEVEL_PREVIEW));
        }

        public static PreviewData[] LoadListAll()
        {
            string path = Path.Combine(Application.persistentDataPath, "Levels");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return new PreviewData[0];
            }
            return Directory.GetDirectories(path).Select(GetName).ToArray();
        }
        private static PreviewData GetName(string directory)
        {
            return PreviewSaver.Load(Path.Combine(directory, LEVEL_PREVIEW));
        }
    }
}
