using System;
using CAFU.Rotator.Application.Enum;
using CAFU.Rotator.Application.ValueObject;
using CAFU.Rotator.Domain.UseCase.Interface;
using CAFU.Rotator.Presentation.Presenter.Interface;
using UniRx;
using Zenject;

namespace CAFU.Rotator.Presentation.Presenter
{
    public class RotatorPresenter : IRotatorPresenter, IInitializable, IDisposable
    {
        [Inject] private IRotatorRotationRenderer RotatorRotationRenderer { get; set; }
        [Inject] private IRotatorEventDispatcher RotatorInput { get; set; }

        [InjectOptional] private IRotatorRotationSpeedReceiver RotatorRotationSpeedReceiver { get; set; }
        [InjectOptional] private IRotatorRotationCountReceiver RotatorRotationCountReceiver { get; set; }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        void IInitializable.Initialize()
        {
        }

        void IDisposable.Dispose()
        {
            Disposable?.Dispose();
        }

        void IRotatorPresenter.ReportRotationDiffRad(float rotationDiffRad) =>
            RotatorRotationRenderer.ReceiveRotationDiffRad(rotationDiffRad);

        void IRotatorPresenter.ReportRotationSpeed(float rotationSpeed) =>
            RotatorRotationSpeedReceiver?.ReceiveRotationSpeed(rotationSpeed);

        void IRotatorPresenter.ReportRotationCount(int rotationCount) =>
            RotatorRotationCountReceiver?.ReceiveRotationCount(rotationCount);

        void IRotatorPresenter.ReportTotalRotationCount(int totalRotationCount) =>
            RotatorRotationCountReceiver?.ReceiveTotalRotationCount(totalRotationCount);

        IObservable<RotateDirection> IRotatorPresenter.RequestRotateDirectionAsObservable() =>
            RotatorRotationRenderer.RequestRotateDirectionAsObservable();

        IObservable<RotatePosition> IRotatorPresenter.StartRotatorAsObservable() =>
            RotatorInput.StartRotator().AsObservable();

        IObservable<RotatePosition> IRotatorPresenter.UpdateRotatorAsObservable() =>
            RotatorInput.UpdateRotator().AsObservable();
    }
}
