using UnityEngine;

namespace ESF.UI
{
    public class UIBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _barRT;
        [SerializeField] private RectTransform _fillRT;

        [SerializeField] private bool _autoShowWhenNotFull = true;
        [SerializeField] private bool _autoShowWhenNotZero = false;
        [SerializeField] private bool _autoHideWhenFull = true;
        [SerializeField] private bool _autoHideWhenZero = false;

        [SerializeField, Range(0, 1f)] private float _fill;


        public void SetFill(float fill)
        {
            fill = Mathf.Clamp(fill, 0, 1);

            bool isFull = Mathf.Approximately(fill, 1);
            bool isZero = Mathf.Approximately(fill, 0);

            if (_autoShowWhenNotFull && !isFull)
                _barRT.gameObject.SetActive(true);
            if (_autoShowWhenNotZero && !isZero)
                _barRT.gameObject.SetActive(true);
            if (_autoHideWhenFull && isFull)
                _barRT.gameObject.SetActive(false);
            if (_autoHideWhenZero && isZero)
                _barRT.gameObject.SetActive(false);

            _fillRT.sizeDelta = new Vector2(fill * _barRT.rect.width, 0);

            _fill = fill;
        }


        private void OnValidate()
        {
            _fillRT.sizeDelta = new Vector2(_fill * _barRT.rect.width, 0);
        }
    }
}