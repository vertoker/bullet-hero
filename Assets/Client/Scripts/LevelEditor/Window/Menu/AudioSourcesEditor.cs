using System.Collections;
using System.Collections.Generic;
using LevelEditor.Windows;
using UnityEngine;
using Utils.Pool;
using Data.Enum;
using Audio.UI;
using Data;

namespace LevelEditor.Windows.Menu
{
    [RequireComponent(typeof(WindowController))]
    public class AudioSourcesEditor : Window
    {
        [SerializeField] private GameObject originPanel;
        [SerializeField] private PoolSpawner poolSpawner;
        [SerializeField] private RectTransform content;

        private WindowController windowController;
        private List<AudioSourceData> sources;
        private List<ParserInput> inputs;
        private int countSources = 0;

        private void Awake()
        {
            windowController = GetComponent<WindowController>();
        }
        private void OnEnable()
        {
            windowController.Init();

            sources = LevelHolder.Level.AudioData.AudioSourcesData;
            inputs = new List<ParserInput>();
            for (int i = 0; i < sources.Count; i++)
                Add();
        }
        private void OnDisable()
        {
            for (int i = countSources - 1; i >= 0; i--)
                Remove(i);
        }

        public void AddNew()
        {
            sources.Add(new AudioSourceData());
            Add();
        }
        public void Add()
        {
            inputs.Add(poolSpawner.Dequeue().GetComponent<ParserInput>());
            inputs[countSources].SetData(sources[countSources]);
            inputs[countSources].ID = countSources;

            inputs[countSources].TapDownload += Download;
            inputs[countSources].TapClear += Clear;
            inputs[countSources].TapDelete += Remove;
            inputs[countSources].TapUp += MoveUp;
            inputs[countSources].TapDown += MoveDown;

            inputs[countSources].UpdateLinkType += UpdateLinkType;
            inputs[countSources].UpdateLink += UpdateLink;
            inputs[countSources].UpdateSFI += UpdateSFI;
            inputs[countSources].UpdateEFI += UpdateEFI;
            inputs[countSources].UpdateSFO += UpdateSFO;
            inputs[countSources].UpdateEFO += UpdateEFO;

            countSources++;
        }
        public void Remove(int id)
        {
            inputs[id].TapDownload -= Download;
            inputs[id].TapClear -= Clear;
            inputs[id].TapDelete -= Remove;
            inputs[id].TapUp -= MoveUp;
            inputs[id].TapDown -= MoveDown;

            inputs[id].UpdateLinkType -= UpdateLinkType;
            inputs[id].UpdateLink -= UpdateLink;
            inputs[id].UpdateSFI -= UpdateSFI;
            inputs[id].UpdateEFI -= UpdateEFI;
            inputs[id].UpdateSFO -= UpdateSFO;
            inputs[id].UpdateEFO -= UpdateEFO;

            poolSpawner.Enqueue(inputs[id].gameObject);
            sources.RemoveAt(id);
            inputs.RemoveAt(id);
            countSources--;

            for (int i = id; i < countSources; i++)
            {
                inputs[i].ID = i;
            }
        }
        public void Download(int id)
        {

        }
        public void Clear(int id)
        {

        }
        public void MoveUp(int id)
        {
            if (id > 0)
            {
                inputs[id - 1].ID = id;
                inputs[id].ID = id - 1;
                (inputs[id], inputs[id - 1]) = (inputs[id - 1], inputs[id]);
                (sources[id], sources[id - 1]) = (sources[id - 1], sources[id]);
            }
        }
        public void MoveDown(int id)
        {
            if (id < countSources - 1)
            {
                inputs[id + 1].ID = id;
                inputs[id].ID = id + 1;
                (inputs[id], inputs[id + 1]) = (inputs[id + 1], inputs[id]);
                (sources[id], sources[id + 1]) = (sources[id + 1], sources[id]);
            }
        }

        public void UpdateLinkType(int id, int type)
        {
            sources[id].SetLinkType((AudioLinkType)type);
            content.sizeDelta = new Vector2(content.sizeDelta.x, countSources * ParserInput.OFFSET_HEIGTH);
        }
        public void UpdateLink(int id, string link)
        {
            sources[id].SetLinkPath(link);
        }
        public void UpdateSFI(int id, float value)
        {
            sources[id].SetStartFadeIn(value);
        }
        public void UpdateEFI(int id, float value)
        {
            sources[id].SetEndFadeIn(value);
        }
        public void UpdateSFO(int id, float value)
        {
            sources[id].SetStartFadeOut(value);
        }
        public void UpdateEFO(int id, float value)
        {
            sources[id].SetEndFadeOut(value);
        }
    }
}