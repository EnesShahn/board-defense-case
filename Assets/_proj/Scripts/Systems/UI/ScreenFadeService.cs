using Cysharp.Threading.Tasks;
using ESF.Utilities.Extensions;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;

namespace ESF.UI.ScreenFade
{
    public class ScreenFadeService : MonoBehaviour
    {
        [SerializeField] private Image _fadeInOutImage;

        private MotionHandle _fadeInMotionHandle;
        private MotionHandle _fadeOutMotionHandle;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            _fadeInOutImage.color = _fadeInOutImage.color.WithAlpha(0);
        }

        public async UniTask DOFadeInOut(float inTime, float outTime)
        {
            CancelActiveMotions();
            await DOFadeIn(inTime);
            await DOFadeOut(outTime);
        }

        public async UniTask DOFadeIn(float inTime)
        {
            CancelActiveMotions();
            var fadeInMotionBuilder = LMotion.Create(0, 1f, inTime).WithEase(Ease.InQuad);
            _fadeInMotionHandle = fadeInMotionBuilder.Bind(t => { _fadeInOutImage.color = _fadeInOutImage.color.WithAlpha(t); });
            await _fadeInMotionHandle;
        }
        public async UniTask DOFadeOut(float outTime)
        {
            CancelActiveMotions();
            var fadeOutMotionBuilder = LMotion.Create(1f, 0, outTime)
                .WithEase(Ease.OutQuad);
            _fadeOutMotionHandle = fadeOutMotionBuilder.Bind(t => { _fadeInOutImage.color = _fadeInOutImage.color.WithAlpha(t); });
            await _fadeOutMotionHandle;
        }

        public void CancelAndSetState(bool faded)
        {
            CancelActiveMotions();

            if (faded)
                _fadeInOutImage.color = _fadeInOutImage.color.WithAlpha(1);
            else
                _fadeInOutImage.color = _fadeInOutImage.color.WithAlpha(0);
        }

        private void CancelActiveMotions()
        {
            if (_fadeInMotionHandle.IsActive())
                _fadeInMotionHandle.Cancel();
            if (_fadeOutMotionHandle.IsActive())
                _fadeOutMotionHandle.Cancel();
        }
    }
}