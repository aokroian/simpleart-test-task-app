using R3;

namespace Initialization.EntryPoint
{
    public interface IEntryPoint
    {
        public Observable<Unit> OnDone { get; }
    }
}