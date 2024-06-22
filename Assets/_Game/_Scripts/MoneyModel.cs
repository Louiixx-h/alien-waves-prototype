using System;
using UnityEngine;

namespace _Game._Scripts
{
    public class MoneyModel : MonoBehaviour
    {
        public int MoneyAmount { get; private set; }

        public Action<int> OnMoneyChange { get; set; }

        public void CreditOperation(int amount)
        {
            MoneyAmount += amount;
            OnMoneyChange?.Invoke(MoneyAmount);
        }

        public void DebitOperation(int amount)
        {
            MoneyAmount -= amount;
            OnMoneyChange?.Invoke(MoneyAmount);
        }
        
        private static MoneyModel _instance;

        public static MoneyModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MoneyModel>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<MoneyModel>();
                        singletonObject.name = typeof(MoneyModel) + " (Singleton)";
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}