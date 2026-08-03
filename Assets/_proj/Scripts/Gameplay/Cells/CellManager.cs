using UnityEngine;
using NaughtyAttributes;

namespace Game.Cells
{
    public class CellManager : MonoBehaviour
    {
        [SerializeField] private CellLane[] _cellLanes;


        public int GetLaneCount()
        {
            return _cellLanes.Length;
        }
        public CellLane GetLane(int laneIndex)
        {
            if (laneIndex < 0 || laneIndex >= _cellLanes.Length)
                return null;
            return _cellLanes[laneIndex];
        }

        [Button("Validate")]
        private void OnValidate()
        {
            int laneIndex = 0;
            foreach (var cellLane in _cellLanes)
            {
                cellLane.UpdateCellsLaneIndex(laneIndex);
                cellLane.OnValidate();
                laneIndex++;
            }
        }
    }
}