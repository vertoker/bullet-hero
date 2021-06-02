using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using TMPro;

public class NewLevelWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TMP_InputField levelNameField;
    [SerializeField] private TMP_InputField musicLinkField;


    public void Init()
    {

    }
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
    public void Create()
    {
        string name = levelNameField.text;
        string url = musicLinkField.text;

        if (Application.internetReachability == NetworkReachability.NotReachable)
            return;

        if (!FileDownloader.IsUrlValid(url))
            return;

        string[] split_url = url.Split('.');
        string file_type = split_url[split_url.Length - 1];

        if (!FileDownloader.ValidAudioFormat(file_type))
            return;

        string path = Application.dataPath + "/levels/" + name + "/audio." + file_type;

        FileDownloader.Download(url, path);

        levelNameField.text = string.Empty;
        musicLinkField.text = string.Empty;
    }
}
