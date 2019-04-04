using System;
using CAFU.Core;
using CAFU.Rotator.Application.Enum;
using CAFU.Rotator.Domain.Entity;
using CAFU.Rotator.Domain.UseCase.Interface;
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

        [InjectOptional] private IRotatorFinishReportEntity RotatorFinishReportEntity { get; set; }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        private RotateDirection RotateDirection { get; set; }
        private int TotalRotationCount { get; set; }

        void IInitializable.Initialize()
        {
            Disposable.Add(StartObservingRequestRotateDirection());
            Disposable.Add(StartObservingReportRotationDiffRad());
            Disposable.Add(StartObservingReportRotationSpeed());
            Disposable.Add(StartObservingReportRotationCount());
            Disposable.Add(StartObservingStartRotator());
            Disposable.Add(StartObservingUpdateRotator());
            Disposable.Add(StartObservingReceiveTotalRotationCount());
        }

        void IDisposable.Dispose()
        {
            Disposable?.Dispose();
        }

        private IDisposable StartObservingRequestRotateDirection()
        {
            return RotatorPresenter
                .RequestRotateDirectionAsObservable()
                .Subscribe(rotateDirection => RotateDirection = rotateDirection);
        }

        private IDisposable StartObservingReportRotationDiffRad()
        {
            return RotatorEntity
                .RotationDiffRad
                .Subscribe(rotationDiffRad => RotatorPresenter.ReportRotationDiffRad(rotationDiffRad));
        }

        private IDisposable StartObservingReportRotationSpeed()
        {
            return RotatorEntity
                .RotationSpeed
                .Subscribe(rotationSpeed => RotatorPresenter.ReportRotationSpeed(rotationSpeed));
        }

        private IDisposable StartObservingReportRotationCount()
        {
            return RotatorEntity
                .RotationCountAsObservable(RotateDirection)
                .Subscribe(rotationCount =>
                {
                    TotalRotationCount = rotationCount;
                    RotatorPresenter.ReportRotationCount(rotationCount);
                });
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

        private IDisposable StartObservingReceiveTotalRotationCount()
        {
            return RotatorFinishReportEntity?
                .OnFinishedAsObservable()
                .Subscribe(_ => RotatorPresenter.ReportTotalRotationCount(TotalRotationCount));
        }
    }
}
