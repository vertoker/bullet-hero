using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Data;

namespace UI
{
    public class SkinWindowController : MonoBehaviour
    {
        [SerializeField] private CustomizationController controller;
        [SerializeField] private SkinSimulation simulation;
        [SerializeField] private GameObject selectedBut;
        [SerializeField] private GameObject selectBut;
        [SerializeField] private GameObject buyBut;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text price;
        private int id;

        [SerializeField] private RectTransform moneyInfo;
        private Vector2 positionMoneyInfo;
        private Coroutine anim;

        private void Awake()
        {
            OnDisable();
        }

        public void Set(Skin skin, int id)
        {
            this.id = id;
            simulation.Set(skin.Preset);
            simulation.gameObject.SetActive(true);
            title.text = skin.GetName();
            price.text = skin.Price.ToString();

            if (controller.IsSelected(id))
                selectedBut.SetActive(true);
            else if (controller.IsPurchased(id))
                selectBut.SetActive(true);
            else
                buyBut.SetActive(true);
        }
        private void OnDisable()
        {
            if (simulation != null)
                simulation.gameObject.SetActive(false);
            title.text = "Title";
            price.text = "Price";

            selectedBut.SetActive(false);
            selectBut.SetActive(false);
            buyBut.SetActive(false);
        }

        public void Buy()
        {
            if (controller.CanBuy(id))
            {
                controller.Buy(id);
                buyBut.SetActive(false);
                selectBut.SetActive(true);
            }
            else
            {
                Denied();
            }
        }
        public void Select()
        {
            controller.Select(id);
            selectBut.SetActive(false);
            selectedBut.SetActive(true);
        }

        public void Denied()
        {
            if (anim != null)
            {
                StopCoroutine(anim);
                moneyInfo.anchoredPosition = positionMoneyInfo;
            }
            anim = StartCoroutine(ShakingAnimation());
        }
        private IEnumerator ShakingAnimation()
        {
            positionMoneyInfo = moneyInfo.anchoredPosition;
            for (float i = 0; i <= 0.4f; i += Time.deltaTime)
            {
                yield return null;
                moneyInfo.anchoredPosition = positionMoneyInfo + (Vector2)Random.insideUnitSphere * 8f;
            }
            moneyInfo.anchoredPosition = positionMoneyInfo;
        }
    }
}