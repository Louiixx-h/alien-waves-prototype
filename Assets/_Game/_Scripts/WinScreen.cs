using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class WinScreen : MonoBehaviour
    {
        public GameObject winScreen;
        public Button exitButton;

        private void OnEnable()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDisable()
        {
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        public void ShowWinScreen()
        {
            winScreen.SetActive(true);
        }
        
        public void HideWinScreen()
        {
            winScreen.SetActive(false);
        }
    }
}