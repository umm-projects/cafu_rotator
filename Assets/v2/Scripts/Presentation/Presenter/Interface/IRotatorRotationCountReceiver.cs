using CAFU.Core;

namespace CAFU.Rotator.Presentation.Presenter.Interface
{
    public interface IRotatorRotationCountReceiver : IView
    {
        void ReceiveRotationCount(int rotationCount);
        void ReceiveTotalRotationCount(int totalRotationCount);
    }
}
