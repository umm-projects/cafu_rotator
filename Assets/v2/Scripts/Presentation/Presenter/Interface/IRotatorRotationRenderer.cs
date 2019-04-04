using System;
using CAFU.Core;
using CAFU.Rotator.Application.Enum;

namespace CAFU.Rotator.Presentation.Presenter.Interface
{
    public interface IRotatorRotationRenderer : IView
    {
        void ReceiveRotationDiffRad(float rotationDiffRad);
        IObservable<RotateDirection> RequestRotateDirectionAsObservable();
    }
}
