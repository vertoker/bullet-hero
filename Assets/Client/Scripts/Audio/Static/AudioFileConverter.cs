using UnityEngine;
using System.IO;
using System;
using NAudio.Wave;
using Data;

namespace Audio.Static
{
    // ������� ��������)))
    public static class AudioFileConverter
    {
        static NVorbis.VorbisReader vorbis;
        public static AudioClip FromOggData(byte[] data)
        {
            // Load the data into a stream
            MemoryStream oggstream = new MemoryStream(data);

            vorbis = new NVorbis.VorbisReader(oggstream, false);

            int samplecount = (int)(vorbis.SampleRate * vorbis.TotalTime.TotalSeconds);

            //  AudioClip audioClip = AudioClip.Create("clip", samplecount, vorbis.Channels, vorbis.SampleRate, false, true, OnAudioRead, OnAudioSetPosition);
            AudioClip audioClip = AudioClip.Create("ogg_type", samplecount, vorbis.Channels, vorbis.SampleRate, false, OnAudioRead);
            // Return the clip
            return audioClip;
        }
        static void OnAudioRead(float[] data)
        {
            var f = new float[data.Length];
            vorbis.ReadSamples(f, 0, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = f[i];
            }
        }
        public static AudioClip FromWavData(byte[] data)
        {
            WAV wav = new WAV(data);
            AudioClip audioClip = AudioClip.Create("wav_type", wav.SampleCount, 1, wav.Frequency, false);
            audioClip.SetData(wav.LeftChannel, 0);
            return audioClip;
        }
        public static AudioClip FromMp3Data(byte[] data)
        {
            // Load the data into a stream
            MemoryStream mp3stream = new MemoryStream(data);
            // Convert the data in the stream to WAV format
            Mp3FileReader mp3audio = new Mp3FileReader(mp3stream);
            WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(mp3audio);
            // Convert to WAV data
            WAV wav = new WAV(AudioMemStream(waveStream).ToArray());
            AudioClip audioClip = AudioClip.Create("mp3_type", wav.SampleCount, 1, wav.Frequency, false);
            audioClip.SetData(wav.LeftChannel, 0);
            // Return the clip
            return audioClip;
        }

        private static MemoryStream AudioMemStream(WaveStream waveStream)
        {
            MemoryStream outputStream = new MemoryStream();
            using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream, waveStream.WaveFormat))
            {
                byte[] bytes = new byte[waveStream.Length];
                waveStream.Position = 0;
                waveStream.Read(bytes, 0, Convert.ToInt32(waveStream.Length));
                waveFileWriter.Write(bytes, 0, bytes.Length);
                waveFileWriter.Flush();
            }
            return outputStream;
        }
    }

    /* From http://answers.unity3d.com/questions/737002/wav-byte-to-audioclip.html */
    public class WAV
    {

        // convert two bytes to one float in the range -1 to 1
        static float bytesToFloat(byte firstByte, byte secondByte)
        {
            // convert two bytes to one short (little endian)
            short s = (short)((secondByte << 8) | firstByte);
            // convert to range from -1 to (just below) 1
            return s / 32768.0F;
        }

        static int bytesToInt(byte[] bytes, int offset = 0)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                value |= ((int)bytes[offset + i]) << (i * 8);
            }
            return value;
        }
        // properties
        public float[] LeftChannel { get; internal set; }
        public float[] RightChannel { get; internal set; }
        public int ChannelCount { get; internal set; }
        public int SampleCount { get; internal set; }
        public int Frequency { get; internal set; }

        public WAV(byte[] wav)
        {

            // Determine if mono or stereo
            ChannelCount = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

            // Get the frequency
            Frequency = bytesToInt(wav, 24);

            // Get past all the other sub chunks to get to the data subchunk:
            int pos = 12;   // First Subchunk ID from 12 to 16

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            pos += 8;

            // Pos is now positioned to start of actual sound data.
            SampleCount = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
            if (ChannelCount == 2) SampleCount /= 2;        // 4 bytes per sample (16 bit stereo)

            // Allocate memory (right will be null if only mono sound)
            LeftChannel = new float[SampleCount];
            if (ChannelCount == 2) RightChannel = new float[SampleCount];
            else RightChannel = null;

            // Write to double array/s:
            int i = 0;
            int maxInput = wav.Length - (RightChannel == null ? 1 : 3);
            // while (pos < wav.Length)
            while ((i < SampleCount) && (pos < maxInput))
            {
                LeftChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                pos += 2;
                if (ChannelCount == 2)
                {
                    RightChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                    pos += 2;
                }
                i++;
            }
        }

        public override string ToString()
        {
            return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", LeftChannel,
        RightChannel, ChannelCount, SampleCount, Frequency);
        }
    }
}