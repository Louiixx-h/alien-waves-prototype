using TMPro;
using UnityEngine;

namespace _Game._Scripts
{
    public class WaveCounterController : MonoBehaviour
    {
        public TextMeshProUGUI text;
        
        public void SetWave(int currentWave, int maxWave)
        {
            text.text = $"{currentWave}/{maxWave}";
        }
    }
}