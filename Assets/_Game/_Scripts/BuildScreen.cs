using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class BuildScreen : MonoBehaviour
    {
        public GameObject buildScreen;
        public Button startWaveButton;
        public CardGeneratorUI cardGeneratorUI;
     
        private CardModelSo _selectedCard;
        private LevelController _levelController;

        private void Awake()
        {
            _levelController = FindFirstObjectByType<LevelController>();   
        }

        private void OnEnable()
        {
            startWaveButton.onClick.AddListener(OnStartWave);
            cardGeneratorUI.OnCardSelected += OnCardSelected;
        }

        private void OnDisable()
        {
            startWaveButton.onClick.RemoveListener(OnStartWave);
            cardGeneratorUI.OnCardSelected -= OnCardSelected;
        }

        private void OnStartWave()
        {
            _levelController.StartWave();
        }
        
        private void OnCardSelected(CardModelSo card)
        {
            _levelController.AddSelectedCard(card);
        }
        
        public void ShowBuildScreen()
        {
            cardGeneratorUI.GenerateCards();
            buildScreen.SetActive(true);
        }
        
        public void HideBuildScreen()
        {
            buildScreen.SetActive(false);
        }
    }
}