using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class MainMenuScreen : MonoBehaviour
    {
        public Button playButton;
        
        private void OnEnable()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        
        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
        
        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}