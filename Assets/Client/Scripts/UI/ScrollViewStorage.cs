using Game.SerializationSaver;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;
using TMPro;
using Data;

namespace UI
{
    public class ScrollViewStorage : MonoBehaviour
    {
        [SerializeField] private TMP_InputField searchInputField;
        [SerializeField] private GameObject loaderObj;
        [SerializeField] private GameObject loaderEmptyObj;
        [SerializeField] private GameObject listScrollerObj;
        [SerializeField] private GameObject listObj;
        [SerializeField] private Slider slider;

        private GameObject[] view;
        private TMP_Text[] viewText;
        private string searchMask = string.Empty;

        private Coroutine loader;
        private string[] dataList;

        private bool isLoaded = false;
        private readonly int pageCount = 6;
        private int currentPage;

        [SerializeField] private UnityEvent<Level> selectEvent = new UnityEvent<Level>();
        public event UnityAction<Level> SelectEvent
        {
            add => selectEvent.AddListener(value);
            remove => selectEvent.AddListener(value);
        }

        private void Awake()
        {
            loaderObj.SetActive(true);
            loaderEmptyObj.SetActive(false);
            listScrollerObj.SetActive(false);
            listObj.SetActive(false);

            view = new GameObject[pageCount];
            viewText = new TMP_Text[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                view[i] = listObj.transform.GetChild(i).gameObject;
                viewText[i] = view[i].transform.GetChild(0).GetComponent<TMP_Text>();
            }
        }
        private void OnEnable()
        {
            slider.SliderUpdate += ListUpdate;
            StartLoad();
        }
        private void OnDisable()
        {
            slider.SliderUpdate -= ListUpdate;
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
            listObj.SetActive(false);
            loaderEmptyObj.SetActive(false);
            loader = StartCoroutine(Load());
        }
        private IEnumerator Load()
        {
            dataList = LevelSaver.LoadListAll();
            if (searchMask.Length != 0)
                dataList = dataList.Where(d => d.Contains(searchMask)).ToArray();
            yield return null;
            FinishLoad();
        }
        private void FinishLoad()
        {
            loaderObj.SetActive(false);
            if (dataList.Length == 0)
            {
                loaderEmptyObj.SetActive(true);
                listScrollerObj.SetActive(false);
            }
            else
            {
                isLoaded = true;
                int pagesCount = (dataList.Length - dataList.Length % pageCount) / pageCount + 1;
                slider.Initialize(pagesCount);
                listObj.SetActive(true);
                listScrollerObj.SetActive(true);
            }
        }

        private void ListUpdate(int currentPage, int pagesCount)
        {
            if (!isLoaded)
                return;

            this.currentPage = currentPage;
            int count = pageCount;
            if (pagesCount == currentPage + 1)
                count = dataList.Length % pageCount;

            for (int i = 0; i < count; i++)
            {
                view[i].SetActive(true);
                viewText[i].text = dataList[currentPage * pageCount + i];
            }
            for (int i = count; i < pageCount; i++)
            {
                view[i].SetActive(false);
            }
        }

        public void SelectLevel(int id)
        {
            int index = currentPage * pageCount + id;
            if (LevelSaver.Exists(dataList[index]))
            {
                selectEvent.Invoke(LevelSaver.Load(dataList[index]));
            }
            else
            {
                StartLoad();
            }
        }
        public void SearchUpdate()
        {
            searchMask = searchInputField.text;
        }
    }
}