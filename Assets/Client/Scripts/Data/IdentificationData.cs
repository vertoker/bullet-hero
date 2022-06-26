using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// ������� ���������� �� ������
    /// <summary>
    [Serializable]
    public struct IdentificationData
    {
        [SerializeField] private long id;// ������������� (identificator)

        public long Identificator { get { return id; } set { id = value; } }

        public IdentificationData(long identificator)
        {
            id = identificator;
        }
    }
}