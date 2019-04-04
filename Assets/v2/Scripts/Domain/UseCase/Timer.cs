using CAFU.Timer.Domain.Entity;
using Zenject;

namespace CAFU.Timer.Domain.UseCase
{
    public interface ITimer
    {
    }

    public class Timer : ITimer, IInitializable
    {
        [Inject] private ITime Time { get; set; }

        void IInitializable.Initialize()
        {
            Time.Start(5.0f);
        }
    }
}