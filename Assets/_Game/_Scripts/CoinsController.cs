using System;
using TMPro;
using UnityEngine;

namespace _Game._Scripts
{
    public class CoinsController : MonoBehaviour
    {
        public TextMeshProUGUI coinsText;

        private MoneyModel _moneyModel;

        private void Awake()
        {
            _moneyModel = MoneyModel.Instance;
        }

        private void OnEnable()
        {
            _moneyModel.OnMoneyChange += UpdateCoins;
        }

        private void OnDisable()
        {
            _moneyModel.OnMoneyChange -= UpdateCoins;
        }

        private void Start()
        {
            UpdateCoins(_moneyModel.MoneyAmount);
        }

        private void UpdateCoins(int coins)
        {
            coinsText.text = coins.ToString();
        }
    }
}