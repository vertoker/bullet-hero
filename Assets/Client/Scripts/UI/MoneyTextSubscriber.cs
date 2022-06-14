using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Data;

namespace UI
{
    public class MoneyTextSubscriber : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void OnEnable()
        {
            ShopData.DataUpdate += UpdateMoney;
        }
        private void OnDisable()
        {
            ShopData.DataUpdate -= UpdateMoney;
        }
        public void UpdateMoney(ShopData shopData)
        {
            text.text = shopData.Money.ToString();
        }
    }
}