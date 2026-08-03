using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.TowerInventories.UI
{
    public class TowerInventorySlotView : MonoBehaviour
    {
        private int _indexInSlot;

        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private Button _button;
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _itemStackCount;
        [SerializeField] private GameObject _highlight;

        [SerializeField] private Sprite _defaultItemSprite;

        public int IndexInSlot => _indexInSlot;

        public event Action<int> ButtonClicked; // int = slot index

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
            _highlight.SetActive(false);
        }
        private void OnButtonClicked()
        {
            ButtonClicked?.Invoke(_indexInSlot);
        }

        public void SetIndexInSlot(int index)
        {
            _indexInSlot = index;
        }
        public void SetName(string itemName)
        {
            _itemName.text = itemName;
        }
        public void SetSprite(Sprite spr)
        {
            _itemImage.sprite = spr;
            _itemImage.gameObject.SetActive(true);
        }
        public void SetTowerCount(int count)
        {
            _itemStackCount.text = $"{count}";
        }
        public void SetHighlight(bool highlight)
        {
            _highlight.SetActive(highlight);
        }
        public void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }
        
        public void Clear()
        {
            _itemImage.sprite = _defaultItemSprite;
            if (_itemImage.sprite == null)
                _itemImage.gameObject.SetActive(false);
            _itemName.text = "Empty";
            _itemStackCount.text = "";
        }
    }
}