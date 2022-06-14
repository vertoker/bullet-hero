using System.Collections.Generic;
using Game.SerializationSaver;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Data;
using TMPro;

namespace UI
{
    public class CustomizationController : MonoBehaviour
    {
        [SerializeField] private GameObject loaderObj;
        [SerializeField] private GameObject levelListScrollerObj;
        [SerializeField] private GameObject levelListObj;
        private GameObject[] levelView;
        private Image[] levelViewImage;
        private TMP_Text[] levelViewText;

        [SerializeField] private Slider slider;
        [SerializeField] private SkinWindowController skinWindow;
        [SerializeField] private MoneyTextSubscriber[] subs;

        [SerializeField] private SkinsData skinsData;
        [SerializeField] private ShopData shopData;

        private Coroutine loader;
        private string[] levelList;

        private bool isLoaded = false;
        private readonly int levelPageCount = 6;
        private int currentPage;

        private void Awake()
        {
            loaderObj.SetActive(true);
            levelListScrollerObj.SetActive(false);
            levelListObj.SetActive(false);

            levelView = new GameObject[levelPageCount];
            levelViewImage = new Image[levelPageCount];
            levelViewText = new TMP_Text[levelPageCount];
            for (int i = 0; i < levelPageCount; i++)
            {
                levelView[i] = levelListObj.transform.GetChild(i).gameObject;
                levelViewImage[i] = levelView[i].transform.GetChild(0).GetComponent<Image>();
                levelViewText[i] = levelView[i].transform.GetChild(1).GetComponent<TMP_Text>();
            }
        }
        private void OnEnable()
        {
            shopData = ShopData.Loader;
            shopData.Fill(skinsData);
            foreach (var sub in subs)
                sub.UpdateMoney(shopData);
            slider.SliderUpdate += ListUpdate;
        }
        private void OnDisable()
        {
            ShopData.Loader = shopData;
            slider.SliderUpdate -= ListUpdate;
        }
        private void Start()
        {
            StartLoad();
        }

        public void StartLoad()
        {
            isLoaded = false;
            loaderObj.SetActive(true);
            levelListObj.SetActive(false);
            loader = StartCoroutine(Load());
        }
        private IEnumerator Load()
        {
            levelList = skinsData.LoadSkinList();
            yield return null;
            FinishLoad();
        }
        private void FinishLoad()
        {
            isLoaded = true;
            int pagesCount = (levelList.Length - levelList.Length % levelPageCount) / levelPageCount + 1;
            slider.Initialize(pagesCount);
            loaderObj.SetActive(false);
            levelListObj.SetActive(true);
            levelListScrollerObj.SetActive(true);
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
                levelViewImage[i].sprite = skinsData.GetSkin(currentPage * levelPageCount + i).Sprite;
                levelViewText[i].text = levelList[currentPage * levelPageCount + i];
            }
            for (int i = count; i < levelPageCount; i++)
            {
                levelView[i].SetActive(false);
            }
        }

        public void SkinLoad(int id)
        {
            int index = currentPage * levelPageCount + id;
            skinWindow.Set(skinsData.GetSkin(index), index);
        }

        public bool IsPurchased(int id)
        {
            return shopData.SkinStatuses[id];
        }
        public bool IsSelected(int id)
        {
            return shopData.Selected == id;
        }
        public bool CanBuy(int id)
        {
            return skinsData.GetSkin(id).Price <= shopData.Money;
        }
        public void Buy(int id)
        {
            shopData.Money -= skinsData.GetSkin(id).Price;
            shopData.SkinStatuses[id] = true;
            ShopData.Loader = shopData;
        }
        public void Select(int id)
        {
            shopData.Selected = id;
            ShopData.Loader = shopData;
        }
    }
}