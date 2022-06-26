using UnityEngine;
using System.IO;
using System;
using Data;

namespace Game.SerializationSaver
{
    /// <summary>
    /// Using for preview file (3-5 objects)
    /// </summary>
    public static class PreviewSaver
    {
        private static char SIGN = '\n';
        public static void Save(long identificator, string level_name, params string[] paths)
        {
            var path = SaverStatic.PathCombine(paths);
            Save(identificator, level_name, path);
        }
        public static void Save(long identificator, string level_name, string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string result = string.Join(SIGN, identificator, level_name);
            File.WriteAllText(path, result);
        }

        public static PreviewData Load(params string[] paths)
        {
            var path = SaverStatic.PathCombine(paths);
            return Load(path);
        }
        public static PreviewData Load(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError(path + SaverStatic.LOAD_EXCEPTION);
                return new PreviewData();
            }

            string[] result = File.ReadAllText(path).Split(SIGN);
            return new PreviewData(result[1], long.Parse(result[0]));
        }
    }
}