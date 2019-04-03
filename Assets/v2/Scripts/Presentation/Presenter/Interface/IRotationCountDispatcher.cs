using System;
using UniRx;

namespace CAFU.Rotator.Presentation.Presenter.Interface
{
    public interface IRotationCountDispatcher
    {
        void DispatchRotationCount(int rotationCount);
        IObservable<Unit> GetTotalRotationCountAsObservable();
        void DispatchTotalRotationCount(int totalRotationCount);
    }
}
