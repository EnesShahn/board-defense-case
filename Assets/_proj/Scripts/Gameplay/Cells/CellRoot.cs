using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Cells
{
    public class CellRoot : MonoBehaviour
    {
        [FormerlySerializedAs("_index")]
        [SerializeField] private Vector2Int _cellIndex;

        public Vector2Int CellIndex => _cellIndex;

        public void SetCellIndex(int cellIndex)
        {
            _cellIndex = new Vector2Int(_cellIndex.x, cellIndex);
        }
        public void SetLaneIndex(int laneIndex)
        {
            _cellIndex = new Vector2Int(laneIndex, _cellIndex.y);
        }
    }
}