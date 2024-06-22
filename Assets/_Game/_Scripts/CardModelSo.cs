using System;
using UnityEngine;

namespace _Game._Scripts
{
    public enum BuffType
    {
        MeleeDamage,
        RangeDamage,
        AttackSpeed
    }
    
    public enum CardType
    {
        Buff,
        Spell
    }
    
    [Serializable]
    public struct BuffData
    {
        public BuffType type;
        public float value;
    }
    
    [CreateAssetMenu(fileName = "card_model_so", menuName = "Scriptable Objects/Cards/Model", order = 0)]
    public class CardModelSo : ScriptableObject
    {
        public string cardName;
        public BuffData[] buffs;
        public int cost;
    }
}