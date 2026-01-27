using Reflex.Attributes;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

namespace __Game.Scripts.Initialization
{
    public class DummyService
    {
        [Inject]
        private void Inject()
        {
            Debug.Log("DummyService inject");
        }
    }

    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            InstallConfigs(builder);
            InstallServices(builder);
        }

        private void InstallConfigs(ContainerBuilder builder)
        {
        }

        private void InstallServices(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(DummyService), Lifetime.Singleton, Resolution.Eager);
        }
    }
}