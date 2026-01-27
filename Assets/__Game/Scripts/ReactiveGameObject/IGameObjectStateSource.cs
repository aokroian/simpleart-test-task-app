using R3;

namespace ReactiveGameObject
{
    public interface IGameObjectStateSource
    {
        public ReadOnlyReactiveProperty<bool> IsOn { get; }
    }
}