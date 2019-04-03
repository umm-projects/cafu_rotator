using System;
using CAFU.Rotator.Application.Enum;
using CAFU.Rotator.Presentation.Presenter.Interface;
using UniRx;
using UnityEngine;

namespace CAFU.Rotator.Presentation.View
{
    public class RotatorRotationRenderer : MonoBehaviour, IRotationRenderer
    {
        [SerializeField] private RotateDirection rotateDirection = RotateDirection.Both;
        private RotateDirection RotateDirection => rotateDirection;

        void IRotationRenderer.Render(float rotationDiffRad)
        {
            if (!IsValidRorateDirection(rotationDiffRad))
            {
                return;
            }

            transform.Rotate(Vector3.forward, Mathf.Rad2Deg * rotationDiffRad);
        }

        IObservable<RotateDirection> IRotationRenderer.RotateDirectionAsObservable()
        {
            return Observable.Return(RotateDirection);
        }

        private bool IsValidRorateDirection(float rotationDiffRad)
        {
            switch (RotateDirection)
            {
                case RotateDirection.Both:
                    return true;

                case RotateDirection.Left:
                    return 0 < rotationDiffRad;

                case RotateDirection.Right:
                    return rotationDiffRad < 0;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
