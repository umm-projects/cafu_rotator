using CAFU.Core.Presentation.View;
using CAFU.ResumePause.Presentation.Presenter;
using CAFU.Rotator.Presentation.Presenter;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityModule;

namespace CAFU.Rotator.Presentation.View
{
    public class RotatorInput : UIBehaviour, IView
    {
        public Canvas Canvas;

        public RectTransform Center;

        private Vector2 PreviousPosition;

        protected override void Start()
        {
            this.RegisterEventActivationHandler();

            var presenter = this.GetPresenter<IRotatorPresenter>();
            var resumePausePresenter = this.GetPresenter<IResumePausePresenter>();
            var center = this.Center.transform.position;

            var updateObservable = this.UpdateAsObservable()
                .Where(_ => resumePausePresenter.IsPlaying);

            updateObservable
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ => this.GetMouseLocationInRectangle(Input.mousePosition))
                .Subscribe(it => presenter.StartRotator(it, center))
                .AddTo(this);

            updateObservable
                .Where(_ => !Input.GetMouseButtonDown(0) && Input.GetMouseButton(0))
                .Select(_ => this.GetMouseLocationInRectangle(Input.mousePosition))
                .Do(position => this.PreviousPosition = position)
                .Subscribe(position => presenter.UpdateRotator(position, center))
                .AddTo(this);

            // Send rotation radiant while stopping
            updateObservable
                .Where(_ => !Input.GetMouseButton(0))
                .Subscribe(position => presenter.UpdateRotator(this.PreviousPosition, center))
                .AddTo(this);
        }

        private Vector2 GetMouseLocationInRectangle(Vector3 inputScreenPosition)
        {
            Vector2 localPoint;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.Canvas.GetComponent<RectTransform>(),
                inputScreenPosition,
                this.Canvas.worldCamera,
                out localPoint
            );

            return localPoint;
        }
    }
}