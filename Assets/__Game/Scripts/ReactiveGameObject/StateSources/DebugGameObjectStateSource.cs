namespace ReactiveGameObject.StateSources
{
    public class DebugGameObjectStateSource : MonoGameObjectStateSource
    {
        public bool isOn;

        private void OnValidate()
        {
            IsOnInner.Value = isOn;
        }

        private void Start()
        {
            IsOnInner.Value = isOn;
        }
    }
}