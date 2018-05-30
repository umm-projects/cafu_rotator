using UnityEngine;
using CAFU.Core.Presentation.Presenter;
using CAFU.Rotator.Domain.UseCase;

namespace CAFU.Rotator.Presentation.Presenter {

    public interface IRotatorPresenter : IPresenter {

        IRotatorUseCase RotatorUseCase { get; }

    }

    public static class IRotatorPresenterExtension {

        public static UniRx.IObservable<float> GetRotationDiffAsObservable(this IRotatorPresenter presenter) {
            return presenter.RotatorUseCase.RotationDiffAsObservable;
        }

        public static UniRx.IObservable<float> GetRotationSpeedAsObservable(this IRotatorPresenter presenter) {
            return presenter.RotatorUseCase.RotationSpeedAsObservable;
        }

        public static UniRx.IObservable<int> GetRotationCountAsObservable(this IRotatorPresenter presenter) {
            return presenter.RotatorUseCase.RotationCountAsObservable;
        }

        public static void StartRotator(this IRotatorPresenter presenter, Vector3 position, Vector3 centerPosition) {
            presenter.RotatorUseCase.OnPressStart(position, centerPosition);
        }

        public static void UpdateRotator(this IRotatorPresenter presenter, Vector3 position, Vector3 centerPosition) {
            presenter.RotatorUseCase.OnPressing(position, centerPosition);
        }

        public static int GetTotalRotationCount(this IRotatorPresenter presenter) {
            return presenter.RotatorUseCase.TotalCount;
        }

    }

}