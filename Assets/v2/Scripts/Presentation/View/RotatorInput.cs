using System;
using CAFU.Rotator.Application.ValueObject;
using CAFU.Rotator.Presentation.Presenter.Interface;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityModule;
using Zenject;

namespace CAFU.Rotator.Presentation.View
{
    public class RotatorInput : MonoBehaviour, IRotatorEventDispatcher, IInitializable
    {
        [SerializeField] private Canvas canvas = default;
        public Canvas Canvas => canvas;

        [SerializeField] private RectTransform center = default;
        public RectTransform Center => center;

        private Vector3 CenterPosition { get; set; }
        private Vector2 PreviousPosition { get; set; }
        private IObservable<Unit> UpdateAsObservable { get; set; }

        void IInitializable.Initialize()
        {
            this.RegisterEventActivationHandler();

            CenterPosition = Center.transform.localPosition;
            UpdateAsObservable = this.UpdateAsObservable();
        }

        IObservable<RotatePosition> IRotatorEventDispatcher.StartRotator()
        {
            return UpdateAsObservable
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ => new RotatePosition(CenterPosition, GetMouseLocationInRectangle(Input.mousePosition)))
                .AsObservable();
        }

        IObservable<RotatePosition> IRotatorEventDispatcher.UpdateRotator()
        {
            return Observable
                .Merge(
                    UpdateAsObservable
                        .Where(_ => !Input.GetMouseButtonDown(0) && Input.GetMouseButton(0))
                        .Select(_ => GetMouseLocationInRectangle(Input.mousePosition))
                        .Do(position => PreviousPosition = position)
                        .AsObservable(),
                    // Send rotation radiant while stopping
                    UpdateAsObservable
                        .Where(_ => !Input.GetMouseButton(0))
                        .Select(_ => PreviousPosition)
                        .AsObservable()
                )
                .Select(position => new RotatePosition(CenterPosition, position))
                .AsObservable();
        }

        private Vector2 GetMouseLocationInRectangle(Vector3 inputScreenPosition)
        {
            Vector2 localPoint;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Canvas.GetComponent<RectTransform>(),
                inputScreenPosition,
                Canvas.worldCamera,
                out localPoint
            );

            return localPoint;
        }
    }
}
