using System;

namespace Game.TowerInventories
{
    // ID-based stack-based Inventory controller. 2 stage flow control with Begin/End
    // flow: to add a Tower you must first allocate/set a slot for that Tower with Tower ID.
    public class TowerInventoryController
    {
        private TowerInventorySlot[] _slots;
        private bool _initialized;

        public TowerInventorySlot[] Slots => _slots;

        // int = inventory slot index 
        public event Action<int> SetBegin;
        public event Action<int> SetEnd;
        public event Action<int> UnsetBegin;
        public event Action<int> UnsetEnd;
        public event Action<int> StackCountIncreased;
        public event Action<int> StackCountDecreased;

        public void Initialize(int slotCount)
        {
            if (_initialized)
                return;
            _initialized = true;

            //Init slots
            _slots = new TowerInventorySlot[slotCount];
            for (int i = 0; i < slotCount; i++)
                _slots[i] = new TowerInventorySlot(i);
        }

        public bool QuickAddTower(int towerConfigId, int count)
        {
            if (count <= 0)
                return false;

            var selectedSlotIndex = GetSlotIndexByTowerId(towerConfigId);
            if (selectedSlotIndex == -1) // Doesn't exist in inventory at all
            {
                int emptySlotIndex = GetFirstSlotIndex(InventorySlotState.Empty);
                if (emptySlotIndex == -1) // there is not empty slots so return false
                    return false;

                BeginSet(emptySlotIndex, towerConfigId, count);
                EndSet(emptySlotIndex, towerConfigId);

                return true;
            }
            else
            {
                IncreaseStackCount(selectedSlotIndex, count);

                return true;
            }
        }
        public bool QuickRemoveTower(int towerConfigId, int count, bool autoUnsetOnEmpty = false)
        {
            if (count <= 0)
                return false;

            // remove count if possible from the slot that has the same tower config id
            var slotIndex = GetSlotIndexByTowerId(towerConfigId);
            if (slotIndex == -1) // Doesn't exist in inventory at all
            {
                return false;
            }
            else
            {
                DecreaseStackCount(slotIndex, count);

                // Auto remove slot when fully empty
                if (autoUnsetOnEmpty && GetSlotStackCount(slotIndex) == 0)
                {
                    BeginUnset(slotIndex);
                    EndUset(slotIndex);
                }

                return true;
            }
        }

        public bool BeginSet(int slotIndex, int towerConfigId, int initialStackSize)
        {
            if (_slots.Length < slotIndex || slotIndex <= -1 )
                return false;
            if (_slots[slotIndex].CurrentSlotState != InventorySlotState.Empty)
                return false;

            _slots[slotIndex].SetTowerId(towerConfigId);
            _slots[slotIndex].SetStackCount(initialStackSize);
            _slots[slotIndex].SetState(InventorySlotState.LockedSetting);

            SetBegin?.Invoke(slotIndex);

            return true;
        }
        public bool EndSet(int slotIndex, int towerConfigId)
        {
            if (_slots.Length < slotIndex || slotIndex <= -1)
                return false;
            if (_slots[slotIndex].CurrentSlotState != InventorySlotState.LockedSetting)
                return false;

            var currentTowerId = _slots[slotIndex].CurrentTowerConfigId;
            if (currentTowerId != towerConfigId)
                return false;

            _slots[slotIndex].SetState(InventorySlotState.Set);

            SetEnd?.Invoke(slotIndex);

            return true;
        }

        public bool BeginUnset(int slotIndex)
        {
            if (_slots.Length < slotIndex || slotIndex <= -1)
                return false;
            if (_slots[slotIndex].CurrentSlotState != InventorySlotState.Set)
                return false;

            _slots[slotIndex].SetState(InventorySlotState.LockedUnsetting);
            _slots[slotIndex].SetStackCount(0);

            UnsetBegin?.Invoke(slotIndex);

            return true;
        }
        public bool EndUset(int slotIndex)
        {
            if (_slots.Length < slotIndex || slotIndex <= -1)
                return false;
            if (_slots[slotIndex].CurrentSlotState != InventorySlotState.LockedUnsetting)
                return false;

            _slots[slotIndex].SetState(InventorySlotState.Empty);
            _slots[slotIndex].SetStackCount(0);
            _slots[slotIndex].SetTowerId(-1);

            UnsetEnd?.Invoke(slotIndex);

            return true;
        }

        public bool IncreaseStackCount(int slotIndex, int amount)
        {
            if (amount <= 0)
                return false;
            if (_slots.Length < slotIndex || slotIndex <= -1)
                return false;
            if (_slots[slotIndex].CurrentSlotState != InventorySlotState.Set)
                return false;

            _slots[slotIndex].SetStackCount(_slots[slotIndex].CurrentStackCount + amount);

            StackCountIncreased?.Invoke(slotIndex);

            return true;
        }
        public bool DecreaseStackCount(int slotIndex, int amount)
        {
            if (amount <= 0)
                return false;
            if (_slots.Length < slotIndex || slotIndex <= -1 )
                return false;
            if (_slots[slotIndex].CurrentSlotState != InventorySlotState.Set)
                return false;

            if (amount > _slots[slotIndex].CurrentStackCount) // not enough stack to decrease by "amount"
                return false;

            _slots[slotIndex].SetStackCount(_slots[slotIndex].CurrentStackCount - amount);

            StackCountDecreased?.Invoke(slotIndex);

            return true;
        }

        public int GetSlotTowerId(int slotIndex)
        {
            if (_slots.Length < slotIndex || slotIndex == -1 || slotIndex < -1)
                return -1;
            return _slots[slotIndex].CurrentTowerConfigId;
        }
        public int GetSlotStackCount(int slotIndex)
        {
            if (_slots.Length < slotIndex || slotIndex == -1 || slotIndex < -1)
                return -1;
            return _slots[slotIndex].CurrentStackCount;
        }
        public InventorySlotState GetInventorySlotState(int slotIndex)
        {
            if (_slots.Length < slotIndex || slotIndex == -1 || slotIndex < -1)
                return InventorySlotState.Undefined;
            return _slots[slotIndex].CurrentSlotState;
        }
        public TowerInventorySlot GetInventorySlot(int slotIndex)
        {
            if (_slots.Length < slotIndex || slotIndex == -1 || slotIndex < -1)
                return null;
            return _slots[slotIndex];
        }

        public int GetSlotIndexByTowerId(int towerConfigId)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].CurrentTowerConfigId == towerConfigId)
                    return i;
            }

            return -1;
        }

        public int GetFirstSlotIndex(InventorySlotState slotState, bool ascending = true)
        {
            if (ascending)
            {
                for (int i = 0; i < _slots.Length; i++)
                {
                    if (_slots[i].CurrentSlotState == slotState)
                        return i;
                }
            }
            else
            {
                for (int i = _slots.Length - 1; i >= 0; i--)
                {
                    if (_slots[i].CurrentSlotState == slotState)
                        return i;
                }
            }

            return -1;
        }
        public int GetSlotCount(InventorySlotState slotState)
        {
            int count = 0;
            foreach (var slot in _slots)
                if (slot.CurrentSlotState == slotState)
                    count++;
            return count;
        }
    }
}