namespace Game.TowerInventories
{
    public class TowerInventorySlot
    {
        private int _slotIndex;
        private int _currentTowerConfigId = -1;
        private int _currentStackCount = -1;
        private InventorySlotState _currentSlotState = InventorySlotState.Empty;

        public int SlotIndex => _slotIndex;
        public int CurrentTowerConfigId => _currentTowerConfigId;
        public int CurrentStackCount => _currentStackCount;
        public InventorySlotState CurrentSlotState => _currentSlotState;

        public TowerInventorySlot(int slotIndex)
        {
            _slotIndex = slotIndex;
        }

        #region Internal methods, should ONLY be called from InventoryController
        internal void SetTowerId(int towerConfigId)
        {
            _currentTowerConfigId = towerConfigId;
        }
        internal void SetStackCount(int stackCount)
        {
            _currentStackCount = stackCount;
        }
        internal void SetState(InventorySlotState state)
        {
            _currentSlotState = state;
        }
        #endregion
    }

    public enum InventorySlotState
    {
        Undefined,
        Empty,
        LockedSetting,
        LockedUnsetting,
        Set
    }
}