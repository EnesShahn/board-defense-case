using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.WaveSystem
{
    public class WaveInfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _waveNumberText;
        [SerializeField] private Image _waveCooldownImage;
        [SerializeField] private WaveController _waveController;

        private void Awake()
        {
            _waveController.WaveStarted += OnWaveStarted;
            _waveController.WaveCompleted += OnWaveCompleted;

            _waveNumberText.text = $"Wave 1";
            _waveCooldownImage.gameObject.SetActive(true);
            _waveCooldownImage.fillAmount = 1;
        }
        private void OnWaveCompleted(int obj)
        {
            _waveCooldownImage.gameObject.SetActive(true);
            _waveCooldownImage.fillAmount = 1;
        }
        private void OnWaveStarted(int obj)
        {
            _waveCooldownImage.gameObject.SetActive(false);
            _waveNumberText.text = $"Wave {_waveController.CurrentWaveIndex + 1}";
        }

        private void Update()
        {
            // Update fill when wave is not active (i.e. in cooldown)
            if (!_waveController.IsWaveActive)
            {
                float fill = _waveController.WaveTimer / _waveController.CurrentWaveInterval;
                _waveCooldownImage.fillAmount = 1 - fill;
            }
        }
    }
}