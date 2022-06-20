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

        public void SetAudio(LevelData levelData, GameRules gameRules)
        {
            loadAudio = LevelSaver.GetAudioPath(levelData, out string path, out AudioFormat format);
            if (loadAudio)
            {
                var bytes = File.ReadAllBytes(path);
                audioSource.clip = GetClip(levelData, bytes, format);
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

        private static AudioClip GetClip(LevelData levelData, byte[] data, AudioFormat format)
        {
            switch (format)
            {
                case AudioFormat.MP3: return AudioFileConverter.FromMp3Data(levelData, data);
                case AudioFormat.WAV: return AudioFileConverter.FromWavData(levelData, data);
                case AudioFormat.OGG: return AudioFileConverter.FromOggData(levelData, data);
            }
            return null;
        }
    }
}