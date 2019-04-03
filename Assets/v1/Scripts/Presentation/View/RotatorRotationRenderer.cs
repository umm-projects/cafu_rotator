using CAFU.Core.Presentation.View;
using CAFU.Rotator.Presentation.Presenter;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

namespace CAFU.Rotator.Presentation.View
{
    public class RotatorRotationRenderer : UIBehaviour, IView
    {
        protected override void Start()
        {
            this.GetPresenter<IRotatorPresenter>()
                .GetRotationDiffAsObservable()
                .Subscribe(this.Render)
                .AddTo(this);
        }

        private void Render(float rotationDiffRad)
        {
            this.transform.Rotate(Vector3.forward, Mathf.Rad2Deg * rotationDiffRad);
        }
    }
}