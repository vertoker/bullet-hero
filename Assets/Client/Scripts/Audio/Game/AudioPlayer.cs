using System.Collections;
using System.Collections.Generic;
using Game.SerializationSaver;
using Audio.Static;
using UnityEngine;
using System.IO;
using Game.Core;
using System;
using Audio;
using Data;

namespace Audio.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private bool loadAudio = false;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SetAudio(LevelData levelData, AudioData audioData, GameRules gameRules)
        {
            loadAudio = LevelSaver.GetAudioPath(levelData, audioData, out string path, out AudioFormat format);
            if (loadAudio)
            {
                var bytes = File.ReadAllBytes(path);
                audioSource.clip = GetClip(bytes, format);
                audioSource.pitch = gameRules.time;
            }
        }

        public void Pause()
        {
            if (loadAudio)
                audioSource.Pause();
        }
        public void Drag(float seconds)
        {
            if (loadAudio)
                audioSource.time = seconds;
        }
        public void Play()
        {
            if (loadAudio)
                audioSource.Play();
        }

        private static AudioClip GetClip(byte[] data, AudioFormat format)
        {
            switch (format)
            {
                case AudioFormat.MP3: return AudioFileConverter.FromMp3Data(data);
                case AudioFormat.WAV: return AudioFileConverter.FromWavData(data);
                case AudioFormat.OGG: return AudioFileConverter.FromOggData(data);
            }
            return null;
        }
    }
}