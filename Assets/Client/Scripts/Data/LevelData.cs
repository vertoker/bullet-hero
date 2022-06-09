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
        [SerializeField] private int ev;// Версия редактора (editor version)
        [SerializeField] private string mt;// Название трека (music title)
        [SerializeField] private string ma;// Автор трека (music author)
        [SerializeField] private string la;// Автор уровня (level author)
        [SerializeField] private float sfo;// Начало убывания музыки (start fade out)
        [SerializeField] private float efo;// Конец убывания музыки (end fade out)

        public string LevelName { get { return ln; } set { ln = value; } }
        public int EditorVersion { get { return ev; } set { ev = value; } }
        public string MusicTitle { get { return mt; } set { mt = value; } }
        public string MusicAuthor { get { return ma; } set { ma = value; } }
        public string LevelAuthor { get { return la; } set { la = value; } }
        public float StartFadeOut { get { return sfo; } set { sfo = value; } }
        public float EndFadeOut { get { return efo; } set { efo = value; } }

        public LevelData(string level_name,
            int editor_version,
            string music_title,
            string music_author,
            string level_author,
            float start_fade_out,
            float end_fade_out)
        {
            ln = level_name;
            ev = editor_version;
            mt = music_title;
            ma = music_author;
            la = level_author;
            sfo = start_fade_out;
            efo = end_fade_out;
        }
    }
}