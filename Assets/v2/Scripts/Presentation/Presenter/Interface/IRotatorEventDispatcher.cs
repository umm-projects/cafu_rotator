using System;
using CAFU.Core;
using CAFU.Rotator.Application.ValueObject;

namespace CAFU.Rotator.Presentation.Presenter.Interface
{
    public interface IRotatorEventDispatcher : IView
    {
        IObservable<RotatePosition> StartRotator();
        IObservable<RotatePosition> UpdateRotator();
    }
}
