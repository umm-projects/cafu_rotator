using System;
using CAFU.Core;
using CAFU.Rotator.Application.Enum;
using CAFU.Rotator.Application.ValueObject;

namespace CAFU.Rotator.Domain.UseCase.Interface
{
    public interface IRotatorPresenter : IPresenter
    {
        void ReportRotationDiffRad(float rotationDiffRad);
        void ReportRotationSpeed(float rotationSpeed);
        void ReportRotationCount(int rotationCount);
        void ReportTotalRotationCount(int totalRotationCount);
        IObservable<RotateDirection> RequestRotateDirectionAsObservable();
        IObservable<RotatePosition> StartRotatorAsObservable();
        IObservable<RotatePosition> UpdateRotatorAsObservable();
    }
}
