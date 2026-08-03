using Cysharp.Threading.Tasks;
using ESF.Utilities.Math;
using LitMotion;
using UnityEngine;

namespace ESF.Utilities.LitMotion.Extensions
{
    public static class LitMotionExtensions
    {
        public static async UniTask DOJumpDynamic(Transform tweenObj, Transform targetTransform, float jumpPower, float duration)
        {
            await DOJumpDynamic(tweenObj, targetTransform, Vector3.zero, Quaternion.identity, jumpPower, duration);
        }

        public static MotionHandle DOJumpDynamic(Transform tweenObj, Transform targetTransform, Vector3 posOffset, Quaternion rotOffset, float jumpPower, float duration, bool relativePosOffset = true)
        {
            Vector3 startPosition = tweenObj.position;
            Quaternion startRotation = tweenObj.rotation;

            var motionBuilder = LMotion.Create(0, 1f, duration).WithEase(Ease.OutSine);

            MotionHandle motionHandle = new MotionHandle();
            motionHandle = motionBuilder.Bind((t) =>
            {
                if (tweenObj == null || targetTransform == null)
                {
                    if (motionHandle.IsActive())
                        motionHandle.Cancel();
                    return;
                }

                Vector3 controlPoint = Vector3.Lerp(startPosition, targetTransform.position, 0.75f);
                controlPoint.y = targetTransform.position.y + jumpPower;

                if (relativePosOffset)
                    tweenObj.position = EBezierCurve.QuadBezier(startPosition, controlPoint, targetTransform.position + (targetTransform.rotation * posOffset), t);
                else
                    tweenObj.position = EBezierCurve.QuadBezier(startPosition, controlPoint, targetTransform.position + posOffset, t);

                tweenObj.rotation = Quaternion.Lerp(startRotation, targetTransform.rotation * rotOffset, t * 3f);
            });

            return motionHandle;
        }
    }
}