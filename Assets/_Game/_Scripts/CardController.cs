using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class CardController : MonoBehaviour
    {
        public TextMeshProUGUI cardName;
        public TextMeshProUGUI cardCost;
        public Image cardImage;
        public Button cardButton;
        public Action<CardModelSo, CardController> OnCardClick;

        private readonly Color _noMoneyColor = new(0.5f, 0.5f, 0.5f);
        
        public CardModelSo Card { get; private set; }

        private void OnEnable()
        {
            cardButton.onClick.AddListener(OnCardClicked);
        }
        
        private void OnDisable()
        {
            cardButton.onClick.RemoveListener(OnCardClicked);
        }

        public void SetCard(CardModelSo card)
        {
            Card = card;
            cardName.text = Card.cardName;
            cardCost.text = Card.cost.ToString();
        }

        public void NoMoneyEnough()
        {
            cardButton.interactable = false;
            cardImage.color = _noMoneyColor;
        }
        
        public void DestroyCard()
        {
            Destroy(gameObject);
        }

        private void OnCardClicked()
        {
            OnCardClick?.Invoke(Card, this);
        }
    }
}