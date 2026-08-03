using System.Collections.Generic;
using ESF.Core.Logging;

namespace ESF.Core.IDGen
{
    public class LegitSmartIDController
    {
        private const uint MaxIDNumber = uint.MaxValue - 10;

        private HashSet<uint> _usedIds = new();
        private uint _lastUsedItemId = 1;

        public bool AllocateId(uint id)
        {
            if (id == 0)
            {
                ELogger.LogError<LegitSmartIDController>("Id 0 is NOT usable.");
                return false;
            }

            if (_usedIds.Contains(id))
            {
                ELogger.LogError<LegitSmartIDController>($"Id: {id} is already being used.");
                return false;
            }

            _usedIds.Add(id);
            return true;
        }
        public uint AllocateNextId()
        {
            uint newId = _lastUsedItemId;
            newId = newId == 0 ? 1 : newId;
            while (_usedIds.Contains(newId))
            {
                newId++;
                if (newId >= MaxIDNumber)
                    newId = 1;
            }

            _usedIds.Add(newId);
            _lastUsedItemId = newId;
            return newId;
        }
        public bool ReleaseId(uint id)
        {
            return _usedIds.Remove(id);
        }
    }
}