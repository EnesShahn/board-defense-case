using System;
using UnityEngine;

namespace Game.TowerInventories.UI
{
    public class TowerInventoryView : MonoBehaviour
    {
        [SerializeField] private TowerInventorySlotView _towerInventorySlotViewPrefab;
        [SerializeField] private Transform _slotViewParent;
        private TowerInventorySlotView[] _inventorySlotViews;

        public event Action<int> InventorySlotButtonClicked;

        public void Initialize(int slotCount)
        {
            _inventorySlotViews = new TowerInventorySlotView[slotCount];

            for (int i = 0; i < _inventorySlotViews.Length; i++)
            {
                _inventorySlotViews[i] = Instantiate(_towerInventorySlotViewPrefab, _slotViewParent.transform);
                _inventorySlotViews[i].SetIndexInSlot(i);
                _inventorySlotViews[i].Clear();

                _inventorySlotViews[i].ButtonClicked += OnAnySlotClicked;
            }
        }

        private void OnAnySlotClicked(int slotIndex)
        {
            InventorySlotButtonClicked?.Invoke(slotIndex);
        }

        public void SetName(int slotIndex, string itemName)
        {
            if (slotIndex >= _inventorySlotViews.Length)
                return;
            _inventorySlotViews[slotIndex].SetName(itemName);
        }
        public void SetSprite(int slotIndex, Sprite spr)
        {
            if (slotIndex >= _inventorySlotViews.Length)
                return;
            _inventorySlotViews[slotIndex].SetSprite(spr);
        }
        public void SetTowerCount(int slotIndex, int count)
        {
            if (slotIndex >= _inventorySlotViews.Length)
                return;
            _inventorySlotViews[slotIndex].SetTowerCount(count);
        }
        public void SetHighlight(int slotIndex, bool highlight)
        {
            if (slotIndex >= _inventorySlotViews.Length)
                return;
            _inventorySlotViews[slotIndex].SetHighlight(highlight);
        }
        public void SetInteractable(int slotIndex, bool interactable)
        {
            if (slotIndex >= _inventorySlotViews.Length)
                return;
            _inventorySlotViews[slotIndex].SetInteractable(interactable);
        }

        public void SetAllInteractable(bool interactable)
        {
            foreach (var slotView in _inventorySlotViews)
                slotView.SetInteractable(interactable);
        }
        public void SetAllHighlight(bool highlight)
        {
            foreach (var slotView in _inventorySlotViews)
                slotView.SetHighlight(highlight);
        }

        public void Clear(int slotIndex)
        {
            if (slotIndex >= _inventorySlotViews.Length)
                return;
            _inventorySlotViews[slotIndex].Clear();
        }
    }
}