using System;
using CAFU.Rotator.Application.Enum;
using CAFU.Rotator.Presentation.Presenter.Interface;
using UniRx;
using UnityEngine;

namespace CAFU.Rotator.Presentation.View
{
    public class RotatorRotationRenderer : MonoBehaviour, IRotatorRotationRenderer
    {
        [SerializeField] private RotateDirection rotateDirection = RotateDirection.Both;
        private RotateDirection RotateDirection => rotateDirection;

        void IRotatorRotationRenderer.ReceiveRotationDiffRad(float rotationDiffRad)
        {
            if (!IsValidRotateDirection(rotationDiffRad))
            {
                return;
            }

            transform.Rotate(Vector3.forward, Mathf.Rad2Deg * rotationDiffRad);
        }

        IObservable<RotateDirection> IRotatorRotationRenderer.RequestRotateDirectionAsObservable()
        {
            return Observable.Return(RotateDirection);
        }

        private bool IsValidRotateDirection(float rotationDiffRad)
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
