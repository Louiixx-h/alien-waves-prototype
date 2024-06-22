using System;
using TMPro;
using UnityEngine;

namespace _Game._Scripts
{
    public class CounterController : MonoBehaviour
    {
        public TextMeshProUGUI timeText;
        
        private float _time;
        private bool _isRunning;
        
        public Action OnTimeEnd { get; set; }

        private void Update()
        {
            if (!_isRunning) return;
            if (_time > 0)
            {
                _time -= Time.deltaTime;
                if (_time <= 0)
                {
                    _time = 0;
                    OnTimeEnd?.Invoke();
                    StopCounter();
                }

                UpdateTimeText();
            }
        }

        private void UpdateTimeText()
        {
            var timeSpan = TimeSpan.FromSeconds(_time);
            var timeString = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            timeText.text = timeString;
        }

        public void SetTime(float time)
        {
            _time = time;
            UpdateTimeText();
        }
        
        public void StartCounter()
        {
            _isRunning = true;
        }
        
        public void StopCounter()
        {
            _isRunning = false;
        }
    }
}