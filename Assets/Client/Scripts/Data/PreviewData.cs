using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// Базовая информация об уровне
    /// <summary>
    [Serializable]
    public struct PreviewData
    {
        [SerializeField] private string ln;// Название уровня (level name)
        [SerializeField] private long id;// Идентификатор (identificator)

        public string LevelName { get { return ln; } set { ln = value; } }
        public long Identificator { get { return id; } set { id = value; } }

        public PreviewData(string level_name, long identificator)
        {
            ln = level_name;
            id = identificator;
        }
    }
}