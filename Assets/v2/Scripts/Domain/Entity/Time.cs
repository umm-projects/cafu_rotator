using System;
using CAFU.Signal.Domain.Entity;
using UniRx;

// TODO: コレは umm 分ける（というか SignalBus 使う？）
namespace CAFU.Signal.Domain.Entity
{
    public interface IFinishReporter
    {
        IObservable<Unit> OnFinishAsObservable();
    }
}

namespace CAFU.Timer.Domain.Entity
{
    public interface ITime : IFinishReporter
    {
        void Start(float threshold);
    }

    public class Time : ITime
    {
        private IObservable<long> Timer { get; set; }

        IObservable<Unit> IFinishReporter.OnFinishAsObservable()
        {
            return Timer.Concat().AsUnitObservable();
        }

        void ITime.Start(float threshold)
        {
            Timer = Observable.Timer(TimeSpan.FromSeconds(threshold));
        }
    }
}