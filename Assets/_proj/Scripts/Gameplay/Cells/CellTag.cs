using UnityEngine;
using ESF.Core.Tags;

namespace Game.Cells
{
    [DisallowMultipleComponent]
    public class CellTag : Tag<CellTag>, ITag
    {
        [SerializeField] private CellRoot _cellRoot;

        public CellRoot CellRoot => _cellRoot;

        private void OnValidate()
        {
            _cellRoot ??= GetComponent<CellRoot>();
            _cellRoot ??= GetComponentInParent<CellRoot>();
        }
    }
}