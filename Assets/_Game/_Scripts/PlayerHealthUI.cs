using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class PlayerHealthUI : MonoBehaviour
    {
        public Image healthBar;
        
        public void SetHealth(float currentHealth, float maxHealth)
        {
            healthBar.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        }
    }
}