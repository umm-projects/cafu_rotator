using System;
using CAFU.Core;
using UniRx;

namespace CAFU.Rotator.Domain.UseCase.Interface
{
    public interface IRotatorFinishReporter : IPresenter
    {
        IObservable<Unit> OnFinishedAsObservable();
    }
}
