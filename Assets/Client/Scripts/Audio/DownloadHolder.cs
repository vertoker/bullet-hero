using System.Collections;
using System.Collections.Generic;
using VideoLibrary;
using UnityEngine;
using System.IO;
using Data.Enum;

namespace Audio
{
    public class DownloadHolder : MonoBehaviour
    {
        public void Download(string link, AudioLinkType parserType)
        {
            switch (parserType)
            {
                case AudioLinkType.AudioLink:
                case AudioLinkType.Youtube:
                default:
                    break;
            }
        }

        void SaveMusicToDisk(string link)
        {
            var youTube = YouTube.Default; // starting point for YouTube actions
            YouTubeVideo video = youTube.GetVideo(link); // gets a Video object with info about the video
            File.WriteAllBytes(@"C:\" + video.FullName, video.GetBytes());
        }
    }
}