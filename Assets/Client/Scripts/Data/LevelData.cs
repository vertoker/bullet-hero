using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// ������� ���������� �� ������
    /// <summary>
    [Serializable]
    public struct LevelData
    {
        [SerializeField] private string ln;// �������� ������ (level name)
        [SerializeField] private int ldv;// ������ ������ �� ������ (level data version)
        [SerializeField] private string mt;// �������� ����� (music title)
        [SerializeField] private string ma;// ����� ����� (music author)
        [SerializeField] private string la;// ����� ������ (level author)

        public string LevelName { get { return ln; } set { ln = value; } }
        public int LevelDataVersion { get { return ldv; } set { ldv = value; } }
        public string MusicTitle { get { return mt; } set { mt = value; } }
        public string MusicAuthor { get { return ma; } set { ma = value; } }
        public string LevelAuthor { get { return la; } set { la = value; } }

        public LevelData(string level_name,
            int level_data_version,
            string music_title,
            string music_author,
            string level_author)
        {
            ln = level_name;
            ldv = level_data_version;
            mt = music_title;
            ma = music_author;
            la = level_author;
        }
    }
}