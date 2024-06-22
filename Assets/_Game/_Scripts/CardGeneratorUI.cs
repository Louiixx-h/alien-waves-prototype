using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game._Scripts
{
    public class CardGeneratorUI : MonoBehaviour
    {
        public int maxCards = 3;
        public AvailableCards availableCards;
        public CardController cardPrefab;
        public GameObject cardContainer;
        public Action<CardModelSo> OnCardSelected;
        
        private List<CardController> _cards = new();
        
        public void GenerateCards()
        {
            var moneyModel = MoneyModel.Instance;
            moneyModel.OnMoneyChange += OnMoneyChange;
            
            for (var i = 0; i < maxCards; i++)
            {
                var card = GetRandomCard();
                var cardObject = Instantiate(cardPrefab, cardContainer.transform);
                cardObject.SetCard(card);
                if (moneyModel.MoneyAmount < card.cost)
                {
                    cardObject.NoMoneyEnough();
                }
                else
                {
                    cardObject.OnCardClick += OnCardClicked;
                }
                _cards.Add(cardObject);
            }
        }

        private void OnDisable()
        {
            foreach (var cardObject in _cards)
            {
                cardObject.OnCardClick -= OnCardClicked;
            }

            foreach (Transform cardObject in cardContainer.transform)
            {
                Destroy(cardObject.gameObject);
            }
            
            _cards = new List<CardController>();
            MoneyModel.Instance.OnMoneyChange -= OnMoneyChange;
        }

        private void OnCardClicked(CardModelSo cardModel, CardController cardController)
        {
            MoneyModel.Instance.DebitOperation(cardController.Card.cost);
            OnCardSelected?.Invoke(cardModel);
            cardController.DestroyCard();
            _cards.Remove(cardController);
            
            if (_cards.Count == 0)
            {
                GenerateCards();
            }
        }

        private CardModelSo GetRandomCard()
        {
            return availableCards.cards[Random.Range(0, availableCards.cards.Length)];
        }
        
        private void OnMoneyChange(int moneyAmount)
        {
            foreach (var cardObject in _cards)
            {
                if (moneyAmount < cardObject.Card.cost)
                {
                    cardObject.NoMoneyEnough();
                }
            }
        }
    }
}