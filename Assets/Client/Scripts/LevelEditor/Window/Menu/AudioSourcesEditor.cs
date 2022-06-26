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
        [SerializeField] private List<AudioSourceData> sources;
        private List<ParserInput> inputs;
        private int countSources = 0;

        private void Awake()
        {
            windowController = GetComponent<WindowController>();
        }
        private void OnEnable()
        {
            windowController.Init();

            sources = new List<AudioSourceData>(LevelHolder.Level.AudioData.AudioSourcesData);
            inputs = new List<ParserInput>();
            for (int i = 0; i < sources.Count; i++)
                Add();
            UpdateUI();
        }
        private void OnDisable()
        {
            for (int i = countSources - 1; i >= 0; i--)
                Remove(i);
        }

        public void AddUpdate()
        {
            sources.Add(new AudioSourceData());
            Add();
            UpdateUI();
        }
        public void Add()
        {
            inputs.Add(poolSpawner.Dequeue().GetComponent<ParserInput>());
            inputs[countSources].SetData(sources[countSources]);
            inputs[countSources].ID = countSources;

            inputs[countSources].TapDownload += Download;
            inputs[countSources].TapClear += Clear;
            inputs[countSources].TapDelete += RemoveUpdate;
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
        public void RemoveUpdate(int id)
        {
            sources.RemoveAt(id);
            inputs.RemoveAt(id);
            Remove(id);
            UpdateUI();
        }
        public void Remove(int id)
        {
            inputs[id].TapDownload -= Download;
            inputs[id].TapClear -= Clear;
            inputs[id].TapDelete -= RemoveUpdate;
            inputs[id].TapUp -= MoveUp;
            inputs[id].TapDown -= MoveDown;

            inputs[id].UpdateLinkType -= UpdateLinkType;
            inputs[id].UpdateLink -= UpdateLink;
            inputs[id].UpdateSFI -= UpdateSFI;
            inputs[id].UpdateEFI -= UpdateEFI;
            inputs[id].UpdateSFO -= UpdateSFO;
            inputs[id].UpdateEFO -= UpdateEFO;

            poolSpawner.Enqueue(inputs[id].gameObject);
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
                (sources[id],
                    sources[id - 1]) = 
                    (sources[id - 1],
                    sources[id]);
            }
            UpdateUI();
        }
        public void MoveDown(int id)
        {
            if (id < countSources - 1)
            {
                inputs[id + 1].ID = id;
                inputs[id].ID = id + 1;
                (inputs[id], inputs[id + 1]) = (inputs[id + 1], inputs[id]);
                (sources[id],
                    sources[id + 1]) =
                    (sources[id + 1],
                    sources[id]);
            }
            UpdateUI();
        }

        public void UpdateLinkType(int id, int type)
        {
            var data = sources[id];
            data.SetLinkType((AudioLinkType)type);
            sources[id] = data;
            UpdateUI();
        }
        public void UpdateLink(int id, string link)
        {
            var data = sources[id];
            data.SetLinkPath(link);
            sources[id] = data;
            UpdateUI();
        }
        public void UpdateSFI(int id, float value)
        {
            var data = sources[id];
            data.SetStartFadeIn(value);
            sources[id] = data;
            UpdateUI();
        }
        public void UpdateEFI(int id, float value)
        {
            var data = sources[id];
            data.SetEndFadeIn(value);
            sources[id] = data;
            UpdateUI();
        }
        public void UpdateSFO(int id, float value)
        {
            var data = sources[id];
            data.SetStartFadeOut(value);
            sources[id] = data;
            UpdateUI();
        }
        public void UpdateEFO(int id, float value)
        {
            var data = sources[id];
            data.SetEndFadeOut(value);
            sources[id] = data;
            UpdateUI();
        }

        private void UpdateUI()
        {
            content.sizeDelta = new Vector2(content.sizeDelta.x, countSources * ParserInput.OFFSET_HEIGTH);
            LevelHolder.Level.AudioData = new AudioData(sources, LevelHolder.Level.AudioData.Length);
        }
    }
}