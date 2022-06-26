using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// ������� ���������� �� ������
    /// <summary>
    [Serializable]
    public struct PreviewData
    {
        [SerializeField] private string ln;// �������� ������ (level name)
        [SerializeField] private long id;// ������������� (identificator)

        public string LevelName { get { return ln; } set { ln = value; } }
        public long Identificator { get { return id; } set { id = value; } }

        public PreviewData(string level_name, long identificator)
        {
            ln = level_name;
            id = identificator;
        }
    }
}