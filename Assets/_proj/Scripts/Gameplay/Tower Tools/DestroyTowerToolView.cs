using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.TowerTools
{
    public class DestroyTowerToolView : MonoBehaviour
    {
        [SerializeField] private Button _destroyTowerButton;
        [SerializeField] private GameObject _destroyTowerButtonHighlight;

        public event Action ToolButtonClicked;
        
        private void Awake()
        {
            _destroyTowerButton.onClick.AddListener(OnDestroyTowerButtonClicked);
            _destroyTowerButtonHighlight.SetActive(false);
        }
        private void OnDestroyTowerButtonClicked()
        {
            ToolButtonClicked?.Invoke();
        }

        public void SetHighlight(bool highlight)
        {
            _destroyTowerButtonHighlight.SetActive(highlight);
        }
    }
}