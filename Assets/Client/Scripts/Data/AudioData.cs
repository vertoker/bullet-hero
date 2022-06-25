using System.Collections.Generic;
using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// ������� ���������� �� ������
    /// <summary>
    [Serializable]
    public struct AudioData
    {
        [SerializeField] private List<AudioSourceData> asd;// ����� ������ (audio sources data)
        [SerializeField] private float l;// ����� ����� (length)

        public List<AudioSourceData> AudioSourcesData => asd;
        public float Length { get { return l; } set { l = value; } }

        public AudioData(List<AudioSourceData> audio_sources_data,
            float length)
        {
            asd = audio_sources_data;
            l = length;
        }
    }
}