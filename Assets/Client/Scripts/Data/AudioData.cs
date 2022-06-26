using System.Collections.Generic;
using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// Базовая информация об уровне
    /// <summary>
    [Serializable]
    public struct AudioData
    {
        [SerializeField] private List<AudioSourceData> asd;// Автор уровня (audio sources data)
        [SerializeField] private float l;// Длина трека (length)

        public List<AudioSourceData> AudioSourcesData => asd;
        public float Length { get { return l; } set { l = value; } }

        public void SetAudioSourcesData(List<AudioSourceData> audio_sources_data) { asd = audio_sources_data; }

        public AudioData(List<AudioSourceData> audio_sources_data,
            float length)
        {
            asd = audio_sources_data;
            l = length;
        }
    }
}