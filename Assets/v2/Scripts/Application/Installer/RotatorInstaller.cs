using CAFU.Rotator.Domain.Entity;
using CAFU.Rotator.Domain.UseCase;
using CAFU.Rotator.Presentation.Presenter;
using Zenject;

namespace CAFU.Rotator.Application.Installer
{
    public class RotatorInstaller : MonoInstaller<RotatorInstaller>
    {
        public override void InstallBindings()
        {
            // Entities
            Container.BindInterfacesTo<RotatorEntity>().AsCached();

            // UseCases
            Container.BindInterfacesTo<RotatorUseCase>().AsCached();

            // Presenters
            Container.BindInterfacesTo<RotatorPresenter>().AsCached();
        }
    }
}
