using CAFU.Core.Presentation.View;
using CAFU.Rotator.Presentation.Presenter;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CAFU.Rotator.Presentation.View
{
    [RequireComponent(typeof(Text))]
    public class RotatorCountText : UIBehaviour, IView
    {
        public string Format = "{0}\nかいてん";

        protected override void Start()
        {
            this.GetPresenter<IRotatorPresenter>()
                .GetRotationCountAsObservable()
                .Subscribe(count => this.GetComponent<Text>().text = string.Format(this.Format, count))
                .AddTo(this);
        }
    }
}