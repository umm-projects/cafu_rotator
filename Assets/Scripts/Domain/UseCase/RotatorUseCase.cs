using System;
using CAFU.Core.Domain.Model;
using CAFU.Core.Domain.UseCase;
using CAFU.Rotator.Domain.Model;
using ExtraUniRx;
using UnityEngine;

namespace CAFU.Rotator.Domain.UseCase
{
    public interface IRotatorUseCase : IUseCase
    {
        IObservable<float> RotationDiffAsObservable { get; }

        IObservable<float> RotationSpeedAsObservable { get; }

        IObservable<int> RotationCountAsObservable { get; }

        int TotalCount { get; }

        void OnPressStart(Vector3 position, Vector3 centerPosition);

        void OnPressing(Vector3 position, Vector3 centerPosition);
    }

    public class RotatorUseCase : IRotatorUseCase
    {
        public class Factory : DefaultUseCaseFactory<RotatorUseCase>
        {
            private bool inverse;

            protected override void Initialize(RotatorUseCase instance)
            {
                base.Initialize(instance);

                instance.Model = new DefaultModelFactory<RotatorModel>().Create();
                instance.TotalCountProperty = new SubjectProperty<int>();
                instance.Model.GetTotalRotationCount(this.inverse).Subscribe(instance.TotalCountProperty);
            }

            public override RotatorUseCase Create()
            {
                return this.Create(false);
            }

            public RotatorUseCase Create(bool inverse)
            {
                this.inverse = inverse;
                return base.Create();
            }
        }

        public IObservable<float> RotationDiffAsObservable => this.Model.RotationDiffRad;

        public IObservable<float> RotationSpeedAsObservable => this.Model.RotationSpeed;

        public int TotalCount => this.TotalCountProperty.Value;

        private RotatorModel Model { get; set; }

        private SubjectProperty<int> TotalCountProperty { get; set; }

        public IObservable<int> RotationCountAsObservable => this.TotalCountProperty;

        public void OnPressStart(Vector3 position, Vector3 centerPosition)
        {
            this.Model.RotationRad = this.CalcRotationRad(position, centerPosition);
        }

        public void OnPressing(Vector3 position, Vector3 centerPosition)
        {
            var rotation = this.CalcRotationRad(position, centerPosition);
            var rotationDiff = (rotation - this.Model.RotationRad);
            rotationDiff = rotationDiff > Mathf.PI ? rotationDiff - 2 * Mathf.PI : rotationDiff;
            rotationDiff = rotationDiff < -Mathf.PI ? rotationDiff + 2 * Mathf.PI : rotationDiff;
            var rotationSpeed = Time.deltaTime > 0 ? rotationDiff / Time.deltaTime : 0f;
            this.Model.RotationRad = rotation;
            this.Model.RotationDiffRad.Value = rotationDiff;
            this.Model.RotationSpeed.Value = rotationSpeed;
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
    }
}