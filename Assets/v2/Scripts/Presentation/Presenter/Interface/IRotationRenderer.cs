using System;
using CAFU.Core;
using CAFU.Rotator.Application.Enum;

namespace CAFU.Rotator.Presentation.Presenter.Interface
{
    public interface IRotationRenderer : IView
    {
        void Render(float rotationDiffRad);
        IObservable<RotateDirection> RotateDirectionAsObservable();
    }
}
