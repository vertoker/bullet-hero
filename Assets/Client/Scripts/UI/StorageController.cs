using System.Collections.Generic;
using Game.SerializationSaver;
using System.Collections;
using UnityEngine;
using Data;
using TMPro;

namespace UI
{
    public class StorageController : MonoBehaviour
    {
        [SerializeField] private GameObject loaderObj;
        [SerializeField] private GameObject loaderEmptyObj;
        [SerializeField] private GameObject levelListScrollerObj;
        [SerializeField] private GameObject levelListObj;
        private GameObject[] levelView;
        private TMP_Text[] levelViewText;

        [SerializeField] private Slider slider;
        [SerializeField] private LevelWindowController levelWindow;
        [SerializeField] private ButtonListAnimator storageAnimator;
        [SerializeField] private ButtonListAnimator levelWindowAnimator;

        private Coroutine loader;
        private string[] levelList;

        private bool isLoaded = false;
        private readonly int levelPageCount = 6;
        private int currentPage;

        private void Awake()
        {
            loaderObj.SetActive(true);
            loaderEmptyObj.SetActive(false);
            levelListScrollerObj.SetActive(false);
            levelListObj.SetActive(false);

            levelView = new GameObject[levelPageCount];
            levelViewText = new TMP_Text[levelPageCount];
            for (int i = 0; i < levelPageCount; i++)
            {
                levelView[i] = levelListObj.transform.GetChild(i).gameObject;
                levelViewText[i] = levelView[i].transform.GetChild(0).GetComponent<TMP_Text>();
            }
        }
        private void OnEnable()
        {
            slider.SliderUpdate += ListUpdate;
        }
        private void OnDisable()
        {
            slider.SliderUpdate -= ListUpdate;
        }
        private void Start()
        {
            StartLoad();
        }

        public void AddDefaultLevel()
        {
            LevelSaver.Save(Level.DEFAULT_LEVEL);
            StartLoad();
        }
        public void StartLoad()
        {
            if (loader != null)
                StopCoroutine(loader);
            isLoaded = false;
            loaderObj.SetActive(true);
            levelListObj.SetActive(false);
            loaderEmptyObj.SetActive(false);
            loader = StartCoroutine(Load());
        }
        private IEnumerator Load()
        {
            levelList = LevelSaver.LoadListAll();
            yield return null;
            FinishLoad();
        }
        private void FinishLoad()
        {
            loaderObj.SetActive(false);
            if (levelList.Length == 0)
            {
                loaderEmptyObj.SetActive(true);
                levelListScrollerObj.SetActive(false);
            }
            else
            {
                isLoaded = true;
                int pagesCount = (levelList.Length - levelList.Length % levelPageCount) / levelPageCount + 1;
                slider.Initialize(pagesCount);
                levelListObj.SetActive(true);
                levelListScrollerObj.SetActive(true);
            }
        }

        private void ListUpdate(int currentPage, int pagesCount)
        {
            if (!isLoaded)
                return;

            this.currentPage = currentPage;
            int count = levelPageCount;
            if (pagesCount == currentPage + 1)
                count = levelList.Length % levelPageCount;

            for (int i = 0; i < count; i++)
            {
                levelView[i].SetActive(true);
                levelViewText[i].text = levelList[currentPage * levelPageCount + i];
            }
            for (int i = count; i < levelPageCount; i++)
            {
                levelView[i].SetActive(false);
            }
        }

        public void LevelLoad(int id)
        {
            int index = currentPage * levelPageCount + id;
            Debug.Log(index);
            if (LevelSaver.Exists(levelList[index]))
            {
                var level = LevelSaver.Load(levelList[index]);
                levelWindow.LoadLevelInfo(level);
            }
            else
            {
                storageAnimator.Timeout();
                levelWindowAnimator.Timeout();
                StartLoad();
            }
        }
    }
}