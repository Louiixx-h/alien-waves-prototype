using UnityEngine;

namespace _Game._Scripts
{
    [CreateAssetMenu(fileName = "available_cards_so", menuName = "Scriptable Objects/Cards/Avaialable", order = 0)]
    public class AvailableCards : ScriptableObject
    {
        public CardModelSo[] cards;
    }
}