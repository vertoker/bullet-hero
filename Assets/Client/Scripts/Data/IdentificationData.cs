using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// Базовая информация об уровне
    /// <summary>
    [Serializable]
    public struct IdentificationData
    {
        [SerializeField] private long id;// Идентификатор (identificator)

        public long Identificator { get { return id; } set { id = value; } }

        public IdentificationData(long identificator)
        {
            id = identificator;
        }
    }
}