using System;
using CAFU.Core;
using CAFU.Rotator.Application.Enum;
using CAFU.Rotator.Domain.Entity;
using CAFU.Rotator.Domain.UseCase.Interface;
using CAFU.Signal.Domain.Entity;
using ExtraUniRx;
using UniRx;
using UnityEngine;
using Zenject;

namespace CAFU.Rotator.Domain.UseCase
{
    public interface IRotatorUseCase : IUseCase
    {
    }

    public class RotatorUseCase : IRotatorUseCase, IInitializable, IDisposable
    {
        [Inject] private IRotatorEntity RotatorEntity { get; set; }
        [Inject] private IRotatorPresenter RotatorPresenter { get; set; }

        [Inject] private IFinishReporter FinishReporter { get; set; }
        [Inject] private SignalBus SignalBus { get; set; }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        private RotateDirection RotateDirection { get; set; }
        private int TotalRotationCount { get; set; }

        void IInitializable.Initialize()
        {
            Disposable.Add(StartObservingRotateDirection());
            Disposable.Add(StartObservingRotationDiffRad());
            Disposable.Add(StartObservingRotationSpeed());
            Disposable.Add(StartObservingGetTotalRotationCount());
            Disposable.Add(StartObservingStartRotator());
            Disposable.Add(StartObservingUpdateRotator());
            Disposable.Add(StartObservingRotationCount());

            Disposable.Add(
                FinishReporter
                    .OnFinishAsObservable()
                    .Subscribe(_ => RotatorPresenter.ReportTotalCount(TotalRotationCount))
                );
//            Disposable.Add(
//                SignalBus
//                    .GetStream<Signal.Finish>()
//                    .Subscribe(_ => SignalBus.Fire(TotalCountFactory.Create(TotalRotationCount)))
//            );
        }

//        [Inject] private TotalCount.Factory TotalCountFactory { get; set; }
//
//        public struct TotalCount
//        {
//            public int Value { get; }
//
//            public TotalCount(int value)
//            {
//                Value = value;
//            }
//
//            public class Factory : PlaceholderFactory<int, TotalCount>
//            {
//
//            }
//        }

        void IDisposable.Dispose()
        {
            Disposable?.Dispose();
        }

        private IDisposable StartObservingRotateDirection()
        {
            return RotatorPresenter
                .RotateDirectionAsObservable()
                .Subscribe(rotateDirection => RotateDirection = rotateDirection);
        }

        private IDisposable StartObservingRotationDiffRad()
        {
            return RotatorEntity
                .RotationDiffRad
                .Subscribe(rotationDiffRad => RotatorPresenter.UpdateRotationDiffRad(rotationDiffRad));
        }

        private IDisposable StartObservingRotationSpeed()
        {
            return RotatorEntity
                .RotationSpeed
                .Subscribe(rotationSpeed => RotatorPresenter.UpdateRotationSpeed(rotationSpeed));
        }

        private IDisposable StartObservingGetTotalRotationCount()
        {
            return RotatorPresenter
                .GetTotalRotationCountAsObservable()
                .Subscribe(_ => RotatorPresenter.TotalRotationCount(TotalRotationCount));
        }

        private IDisposable StartObservingStartRotator()
        {
            return RotatorPresenter
                .StartRotatorAsObservable()
                .Subscribe(x => { RotatorEntity.RotationRad = CalcRotationRad(x.ToPosition, x.FromPosition); });
        }

        private IDisposable StartObservingUpdateRotator()
        {
            return RotatorPresenter
                .UpdateRotatorAsObservable()
                .Subscribe(x =>
                {
                    var rotation = CalcRotationRad(x.ToPosition, x.FromPosition);
                    var rotationDiff = (rotation - RotatorEntity.RotationRad);
                    rotationDiff = rotationDiff > Mathf.PI ? rotationDiff - 2 * Mathf.PI : rotationDiff;
                    rotationDiff = rotationDiff < -Mathf.PI ? rotationDiff + 2 * Mathf.PI : rotationDiff;
                    var rotationSpeed = Time.deltaTime > 0 ? rotationDiff / Time.deltaTime : 0f;
                    RotatorEntity.RotationRad = rotation;
                    RotatorEntity.RotationDiffRad.Value = rotationDiff;
                    RotatorEntity.RotationSpeed.Value = rotationSpeed;
                });
        }

        private float CalcRotationRad(Vector3 to, Vector3 from)
        {
            var relativePosition = to - from;
            if (relativePosition.sqrMagnitude <= 0f)
            {
                return 0f;
            }

            return Mathf.Atan2(relativePosition.y, relativePosition.x);
        }

        private IDisposable StartObservingRotationCount()
        {
            return RotatorEntity
                .RotationCountAsObservable(RotateDirection)
                .Subscribe(rotationCount =>
                {
                    TotalRotationCount = rotationCount;
                    RotatorPresenter.UpdateRotationCount(rotationCount);
                });
        }
    }
}
