using CAFU.Core;

namespace CAFU.Rotator.Presentation.Presenter.Interface
{
    public interface ITotalCountReceiver : IView
    {
        void ReceiveTotalCount(int totalCount);
    }
}