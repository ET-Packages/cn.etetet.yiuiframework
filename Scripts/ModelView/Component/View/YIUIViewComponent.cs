//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// UI界面组件
    /// </summary>
    [ComponentOf(typeof(YIUIComponent))]
    public partial class YIUIViewComponent : Entity, IAwake, IYIUIInitialize, IDestroy
    {
        private EntityRef<YIUIComponent> _uiBase;
        private YIUIComponent            _UIBase => _uiBase;

        public YIUIComponent UIBase
        {
            get
            {
                if (_UIBase == null)
                {
                    _uiBase = this.GetParent<YIUIComponent>();
                }

                return _UIBase;
            }
        }

        private EntityRef<Entity> _ownerUIEntity;
        private Entity            _OwnerUIEntity => _ownerUIEntity;

        public Entity OwnerUIEntity
        {
            get
            {
                if (_OwnerUIEntity == null)
                {
                    _ownerUIEntity = UIBase?.OwnerUIEntity;
                }

                return _OwnerUIEntity;
            }
        }

        private EntityRef<YIUIWindowComponent> _uiWindow;
        private YIUIWindowComponent            _UIWindow => _uiWindow;

        public YIUIWindowComponent UIWindow
        {
            get
            {
                if (_UIWindow == null)
                {
                    _uiWindow = UIBase?.GetComponent<YIUIWindowComponent>();
                }

                return _UIWindow;
            }
        }

        public EViewWindowType ViewWindowType = EViewWindowType.View;

        public EViewStackOption StackOption = EViewStackOption.Visible;
    }
}
