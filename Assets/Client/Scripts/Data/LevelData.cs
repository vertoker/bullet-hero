using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// Базовая информация об уровне
    /// <summary>
    [Serializable]
    public struct LevelData
    {
        [SerializeField] private string ln;// Название уровня (level name)
        [SerializeField] private int ldv;// Версия данных об уровне (level data version)
        [SerializeField] private string mt;// Название трека (music title)
        [SerializeField] private string ma;// Автор трека (music author)
        [SerializeField] private string la;// Автор уровня (level author)

        [SerializeField] private AudioSourceData[] asd;// Автор уровня (audio sources data)
        [SerializeField] private float l;// Длина трека (length)

        public string LevelName { get { return ln; } set { ln = value; } }
        public int LevelDataVersion { get { return ldv; } set { ldv = value; } }
        public string MusicTitle { get { return mt; } set { mt = value; } }
        public string MusicAuthor { get { return ma; } set { ma = value; } }
        public string LevelAuthor { get { return la; } set { la = value; } }
        public AudioSourceData[] AudioSourcesData { get { return asd; } set { asd = value; } }
        public float Length { get { return l; } set { l = value; } }

        public LevelData(string level_name,
            int level_data_version,
            string music_title,
            string music_author,
            string level_author,
            AudioSourceData[] audio_sources_data,
            float length)
        {
            ln = level_name;
            ldv = level_data_version;
            mt = music_title;
            ma = music_author;
            la = level_author;
            asd = audio_sources_data;
            l = length;
        }
    }
}