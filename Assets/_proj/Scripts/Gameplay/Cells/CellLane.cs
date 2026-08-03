using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using NaughtyAttributes;

namespace Game.Cells
{
    public class CellLane : MonoBehaviour
    {
        [SerializeField] private CellRoot[] _cells;
        [SerializeField] private float _distance = 1.25f;

        public void UpdateCellsLaneIndex(int laneIndex)
        {
            foreach (var cellRoot in _cells)
            {
                cellRoot.SetLaneIndex(laneIndex);
            }
        }

        public int GetCellCount()
        {
            return _cells.Length;
        }

        public CellRoot GetCellRoot(int cellIndex)
        {
            if (cellIndex < 0 || cellIndex >= _cells.Length)
                return null;
            return _cells[cellIndex];
        }

        [Button("Validate")]
        public void OnValidate()
        {
            var nameIndexRegex = new Regex(@"\s*\(\d+\)$");
            _cells = GetComponentsInChildren<CellRoot>().ToArray();

            int i = 0;
            foreach (var cellRoot in _cells)
            {
                bool hasIndex = nameIndexRegex.IsMatch(cellRoot.name);
                if (hasIndex)
                {
                    string cellName = nameIndexRegex.Replace(cellRoot.gameObject.name, $" ({i})");
                    cellRoot.gameObject.name = cellName;
                }
                else
                {
                    cellRoot.gameObject.name = cellRoot.gameObject.name + $" ({i})";
                }

                cellRoot.SetCellIndex(i);
                cellRoot.transform.localPosition = new Vector3(0, 0, _distance * i);
                i++;
            }
        }
    }
}