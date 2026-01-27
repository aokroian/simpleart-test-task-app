using Constants;
using Utils;

namespace UI.Routing
{
    public class UIConfig : IUIConfig
    {
        public string[] GetUiRoutePoints()
        {
            return ReflectionUtils.GetConstantValues<string>(typeof(Const.UIRoutingPointIDs));
        }
    }
}