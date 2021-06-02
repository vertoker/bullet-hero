using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class FileDownloader
{
    private static readonly string[] MUSIC_FILES_FORMATS = new string[] 
    { "mp3", "ogg", "vaw", "aiff", "aif", "mod", "it", "s3m", "xm" };

    public static bool ValidAudioFormat(string format)
    {
        for (int i = 0; i < MUSIC_FILES_FORMATS.Length; i++)
        {
            if (MUSIC_FILES_FORMATS[i] == format)
                return true;
        }
        return false;
    }
    public static bool IsUrlValid(string url)
    {
        return Regex.IsMatch(url, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
    }
    public static void Download(string url, string path)
    {
        WebClient client = new WebClient();
        client.DownloadFile(url, path);
    }
}
