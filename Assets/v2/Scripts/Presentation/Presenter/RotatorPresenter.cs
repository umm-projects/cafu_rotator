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
        [Inject] private IRotationRenderer RotationRenderer { get; set; }
        [Inject] private IRotatorEventDispatcher RotatorInput { get; set; }
        [InjectOptional] private IRotationSpeedDispatcher RotationSpeedDispatcher { get; set; }
        [InjectOptional] private IRotationCountDispatcher RotationCountDispatcher { get; set; }
        [Inject] private ITotalCountReceiver TotalCountReceiver { get; set; }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        void IInitializable.Initialize()
        {
        }

        void IDisposable.Dispose()
        {
            Disposable?.Dispose();
        }

        void IRotatorPresenter.UpdateRotationDiffRad(float rotationDiffRad)
        {
            RotationRenderer.Render(rotationDiffRad);
        }

        void IRotatorPresenter.UpdateRotationSpeed(float rotationSpeed)
        {
            if (RotationSpeedDispatcher == default)
            {
                return;
            }

            RotationSpeedDispatcher.DispatchRotationSpeed(rotationSpeed);
        }

        IObservable<RotateDirection> IRotatorPresenter.RotateDirectionAsObservable() =>
            RotationRenderer.RotateDirectionAsObservable();

        IObservable<RotatePosition> IRotatorPresenter.StartRotatorAsObservable() =>
            RotatorInput.StartRotator().AsObservable();

        IObservable<RotatePosition> IRotatorPresenter.UpdateRotatorAsObservable() =>
            RotatorInput.UpdateRotator().AsObservable();

        IObservable<Unit> IRotatorPresenter.GetTotalRotationCountAsObservable() =>
            RotationCountDispatcher.GetTotalRotationCountAsObservable();

        void IRotatorPresenter.ReportTotalCount(int totalCount)
        {
            TotalCountReceiver.ReceiveTotalCount(totalCount);
        }

        void IRotatorPresenter.TotalRotationCount(int totalRotationCount)
        {
            RotationCountDispatcher.DispatchTotalRotationCount(totalRotationCount);
        }

        void IRotatorPresenter.UpdateRotationCount(int rotationCount)
        {
            if (RotationCountDispatcher == default)
            {
                return;
            }

            RotationCountDispatcher.DispatchRotationCount(rotationCount);
        }
    }
}
