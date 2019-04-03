using System;
using CAFU.Core;
using CAFU.Rotator.Application.Enum;
using CAFU.Rotator.Application.ValueObject;
using UniRx;

namespace CAFU.Rotator.Domain.UseCase.Interface
{
    public interface IRotatorPresenter : IPresenter
    {
        void UpdateRotationDiffRad(float rotationDiffRad);
        void UpdateRotationSpeed(float rotationSpeed);
        void UpdateRotationCount(int rotationCount);
        void TotalRotationCount(int totalRotationCount);
        IObservable<RotateDirection> RotateDirectionAsObservable();
        IObservable<RotatePosition> StartRotatorAsObservable();
        IObservable<RotatePosition> UpdateRotatorAsObservable();
        IObservable<Unit> GetTotalRotationCountAsObservable();
    }
}
