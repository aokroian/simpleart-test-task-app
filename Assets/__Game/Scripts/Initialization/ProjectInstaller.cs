using Initialization.EntryPoint;
using Reflex.Core;
using Reflex.Enums;
using UI.Routing;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

namespace Initialization
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            InstallConfigs(builder);
            InstallServices(builder);
        }

        private void InstallConfigs(ContainerBuilder builder)
        {
            builder.RegisterType(
                typeof(UIConfig),
                new[] {typeof(IUIConfig)},
                Lifetime.Singleton,
                Resolution.Eager
            );
        }

        private void InstallServices(ContainerBuilder builder)
        {
            builder.RegisterType(
                typeof(UIRoutingService),
                Lifetime.Singleton,
                Resolution.Eager
            );
            builder.RegisterType(
                typeof(UIRoutingFlowManager),
                Lifetime.Singleton,
                Resolution.Eager
            );
            builder.RegisterType(
                typeof(ProjectEntryPoint),
                new[] {typeof(IEntryPoint)},
                Lifetime.Singleton,
                Resolution.Eager);
        }
    }
}