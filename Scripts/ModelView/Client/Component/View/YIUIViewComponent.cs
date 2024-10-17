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
    [ComponentOf(typeof(YIUIChild))]
    public partial class YIUIViewComponent : Entity, IAwake, IYIUIInitialize, IDestroy
    {
        private EntityRef<YIUIChild> _uiBase;
        private YIUIChild            _UIBase => _uiBase;

        public YIUIChild UIBase
        {
            get
            {
                if (_UIBase == null)
                {
                    var yiuiChild = this.GetParent<YIUIChild>();
                    if (yiuiChild is { IsDisposed: false })
                    {
                        _uiBase = yiuiChild;
                    }
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
